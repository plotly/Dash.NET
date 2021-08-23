module Dash.NET.ComponentGeneration.ASTGeneration

open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open Fantomas
open System
open System.IO
open Serilog
open Prelude
open ASTHelpers
open ComponentParameters
open ReactMetadata
open DocumentationGeneration

let createComponentAST (log: Core.Logger) (parameters: ComponentParameters) =

    log.Information("Creating component bindings")

    let componentPropertyTypeDeclarations =
        let rec generatePropTypes (name: string) (ptype: SafeReactPropType) =

            let propTypeName = name |> String.toPascalCase

            match ptype with
            | SafeReactPropType.Enum (_, Some cases) ->
                // | ACase
                // | BCase
                // | CCase
                //
                /// Define the cases for the descriminated union
                let duCases =
                    cases
                    |> List.choose (fun case -> 
                        match case with 
                        | Any (_, Some value) -> 
                            let cleanValue = value
                            let duSafeCleanValue = cleanValue |> String.toValidDULabel
                            simpleUnionCase duSafeCleanValue []
                            |> Some
                        | _ -> 
                            log.Warning("Enum property {PropertyName} contained an invalid case, it was ignored", name)
                            None)
                
                // | ACase -> "aCase"
                // | BCase -> "bCase"
                // | CCase -> "cCase"
                //
                /// Define the values for the cases
                let caseValues =
                    cases
                    |> List.choose (fun case -> 
                        match case with 
                        | Any (_, Some value) -> 
                            let cleanValue = value
                            let duSafeCleanValue = cleanValue |> String.toValidDULabel
                            simpleMatchClause duSafeCleanValue [] None (SynExpr.CreateConstString cleanValue)
                            |> Some
                        | _ -> 
                            log.Warning("Enum property {PropertyName} contained an invalid case, it was ignored", name)
                            None)

                if duCases.Length > 0 then
                    // override this.ToString() =
                    //     match this with
                    //
                    /// Define the json conversion
                    let toCaseValueDefinition =
                        functionPatternThunk "this.ToString"
                        |> binding  
                          ( SynExpr.CreateIdentString "this"
                            |> matchStatement caseValues )
                        |> SynMemberDefn.CreateOverrideMember
                 
                    // type CPropCase0Type =
                    //
                    /// Create the union definition
                    propTypeName
                    |> componentInfo
                    |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                    |> simpleTypeDeclaration 
                         (duCases |> unionDefinition)
                         [ toCaseValueDefinition ] 
                    |> List.singleton
                    |> Some

                else
                    log.Error("Enum property {PropertyName} contained no cases, a type could not be created", name)
                    None

            | SafeReactPropType.Union (_, Some utypes) 
            | SafeReactPropType.FlowUnion (_, Some utypes) -> 
                let recursiveTypes = 
                    utypes
                    |> List.indexed
                    |> List.choose (fun (i, case) -> 
                        generatePropTypes (sprintf "%sCase%dType" propTypeName i) case)
                    |> List.concat

                // | CPropCase0 of CPropCase0Type
                // | CPropCase1 of bool
                //
                /// Define the cases for the descriminated union
                let duCases =
                    utypes
                    |> List.indexed
                    |> List.map (fun (i, case) -> 
                        let caseName = (sprintf "%sCase%d" propTypeName i)
                        let caseTypeName = 
                            case 
                            |> SafeReactPropType.tryGetFSharpTypeName
                            |> Option.defaultValue ([sprintf "%sCase%dType" propTypeName i])

                        simpleUnionCase caseName [anonAppField caseTypeName])

                // | CPropCase0 (v) -> v.ToString()
                // | CPropCase1 (v) -> v.ToString()
                //
                /// Define the values for the cases
                let caseValues =
                    utypes
                    |> List.indexed
                    |> List.map (fun (i, _) -> 
                        let caseName = (sprintf "%sCase%d" propTypeName i)
                        simpleMatchClause caseName ["v"] None ( SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["v"; "ToString"]) ))

                // override this.ToString() =
                //     match this with
                //
                /// Define the json conversion
                let toCaseValueDefinition =
                    functionPatternThunk "this.ToString"
                    |> binding  
                        ( SynExpr.CreateIdentString "this"
                        |> matchStatement caseValues )
                    |> SynMemberDefn.CreateOverrideMember

                if duCases.Length > 0 then
                    // type CProp =
                    //
                    /// Create the union definition
                    let unionTypeDefinition =
                        propTypeName
                        |> componentInfo
                        |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                        |> simpleTypeDeclaration 
                                (duCases |> unionDefinition)
                                [ toCaseValueDefinition ]

                    (unionTypeDefinition :: recursiveTypes) 
                    |> Some

                else
                    log.Error("Union property {PropertyName} contained no cases, a type could not be created", name)
                    None

            | SafeReactPropType.ArrayOf (_, Some utype) 
            | SafeReactPropType.FlowArray (_, Some utype) -> 
                let recursiveTypes = 
                    generatePropTypes (sprintf "%sType" propTypeName) utype
                    |> Option.defaultValue []
                
                let caseInnerType = 
                    utype 
                    |> SafeReactPropType.tryGetFSharpTypeName
                    |> Option.defaultValue ([sprintf "%sType" propTypeName])

                // | DProp of list<string>
                //
                /// Create single case DU case
                let singleCaseUnion =
                    [ anonAppField [ yield "list"; yield! caseInnerType ] ]
                    |> simpleUnionCase propTypeName
                    |> List.singleton

                // override this.ToString() =
                //     match this with
                //     | DProp (v) -> JsonSerializer.Serialize(List.map (fun (i: string) -> i.ToString()) v)
                //
                /// Define the json conversion
                let toCaseValueDefinition =
                    let matchResult =
                        let mapStrings =
                            application
                                [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["List"; "map"])
                                  SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["i"; "ToString"])
                                  |> typedLambdaStatement false [("i", appType caseInnerType)]
                                  |> SynExpr.CreateParen
                                  SynExpr.CreateIdentString "v"]
                            |> SynExpr.CreateParen
                        SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["JsonConvert"; "SerializeObject"], mapStrings)

                    let matchCase =
                        SynExpr.CreateIdentString "this"
                        |> matchStatement 
                            [ simpleMatchClause propTypeName ["v"] None matchResult ]

                    functionPatternThunk "this.ToString"
                    |> binding matchCase
                    |> SynMemberDefn.CreateOverrideMember

                // type DProp =
                //
                /// Create the union definition
                let unionDefinition =
                    propTypeName
                    |> componentInfo
                    |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                    |> simpleTypeDeclaration
                        (singleCaseUnion |> unionDefinition)
                        [ toCaseValueDefinition ]

                (unionDefinition :: recursiveTypes)
                |> Some

            | SafeReactPropType.ObjectOf (_, Some utype) -> 
                let recursiveTypes = 
                    generatePropTypes (sprintf "%sType" propTypeName) utype
                    |> Option.defaultValue []

                let caseInnerType = 
                    utype 
                    |> SafeReactPropType.tryGetFSharpTypeName
                    |> Option.defaultValue ([sprintf "%sType" propTypeName])

                // | EProp of bool
                //
                /// Create single case DU case
                let singeCaseUnion =
                    [ anonAppField caseInnerType ]
                    |> simpleUnionCase propTypeName
                    |> List.singleton

                // override this.ToString() =
                //     match this with
                //     | EProp (v) -> v.ToString()
                //
                /// Define the json conversion
                let toCaseValueDefinition =
                    let matchCase =
                        SynExpr.CreateIdentString "this"
                        |> matchStatement 
                            [ simpleMatchClause propTypeName ["v"] None ( SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["v"; "ToString"]) ) ]
                    functionPatternThunk "this.ToString"
                    |> binding matchCase
                    |> SynMemberDefn.CreateOverrideMember

                // type EProp =
                //
                /// Create the union definition
                let unionDefinition =
                    propTypeName
                    |> componentInfo
                    |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                    |> simpleTypeDeclaration
                        (singeCaseUnion |> unionDefinition)
                        [ toCaseValueDefinition ]

                (unionDefinition :: recursiveTypes)
                |> Some

            | SafeReactPropType.Shape (_, Some values) 
            | SafeReactPropType.Exact (_, Some values) 
            | SafeReactPropType.FlowObject (_, Some values) -> 
                let fields =
                    (values.Keys |> List.ofSeq, values.Values |> List.ofSeq)
                    ||> List.zip 

                if fields.Length > 0 then
                    let recursiveTypes =
                        fields
                        |> List.choose (fun (pname, ptype) -> 
                            generatePropTypes (sprintf "%s%sType" propTypeName (pname |> String.toPascalCase)) ptype)
                        |> List.concat
                
                    // { AField: Option<bool>
                    //   BField: Option<string>
                    //   CField: string }
                    //
                    /// Define the fields for the record
                    let fieldDefinitions =
                        fields
                        |> List.map (fun (pname, ptype) -> 
                            let fieldTypeName =
                                SafeReactPropType.tryGetFSharpTypeName ptype
                                |> Option.defaultValue ([sprintf "%s%sType" propTypeName (pname |> String.toPascalCase)])
                            if (ptype |> SafeReactPropType.getProps).required = Some false then 
                                simpleAppField (pname |> String.toPascalCase) [ yield "Option"; yield! fieldTypeName ]
                            else
                                simpleAppField (pname |> String.toPascalCase) fieldTypeName)

                    // override this.ToString() =
                    //     JsonSerializer.Serialize
                    //         {| aField = Some(this.AField.ToString())
                    //            bField = Some(this.BField.ToString())
                    //            cField = this.CField.ToString() |}
                    //
                    /// Define the json conversion
                    let toStringDefinition =
                        let toString =
                            let serializable =
                                fields
                                |> List.map (fun (pname, ptype) -> 
                                    let jsonConversion =
                                        if (ptype |> SafeReactPropType.getProps).required = Some false then 
                                            application
                                                [ SynExpr.CreateIdentString "Some"
                                                  SynExpr.CreateInstanceMethodCall( LongIdentWithDots.Create ["this"; pname |> String.toPascalCase; "ToString"] )
                                                  |> SynExpr.CreateParen ]
                                        else
                                            SynExpr.CreateInstanceMethodCall( LongIdentWithDots.Create ["this"; pname |> String.toPascalCase; "ToString"] )
                                            
                                    ( Ident.Create pname, jsonConversion ))
                                |> anonRecord

                            SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["JsonConvert"; "SerializeObject"], serializable)

                        functionPatternThunk "this.ToString"
                        |> binding toString
                        |> SynMemberDefn.CreateOverrideMember

                    // type BProp =
                    // 
                    /// Create the record definition
                    let recordTypeDefinition =
                        propTypeName
                        |> componentInfo
                        |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                        |> simpleTypeDeclaration 
                                (fieldDefinitions |> recordDefinition)
                                [ toStringDefinition ]

                    (recordTypeDefinition :: recursiveTypes)
                    |> Some

                else
                    log.Error("Object, shape or exact property {PropertyName} contained no properties, a type could not be created", name)
                    None

            // Other types dont need definitions
            | _ -> None

        (parameters.PropertyNames, parameters.PropertyTypes)
        ||> List.zip 
        |> List.choose (fun (pname, ptype) -> 
            ptype.propType
            |> Option.bind (generatePropTypes pname))
        |> List.concat
        |> List.rev

    /// Define the component property descriminated union
    let componentPropertyDUDeclaration =

        // | AProp of string
        // | BProp of int
        // | CProp of CProp
        //
        /// Define the cases for the descriminated union
        let componentPropertyDUCases =
            (parameters.DUSafePropertyNames, parameters.PropertyNames, parameters.PropertyTypes)
            |||> List.zip3 
            |> List.choose (fun (psafe, pname, prop) -> 
                prop.propType
                |> Option.map (fun ptype ->
                    let propTypeName =
                        SafeReactPropType.tryGetFSharpTypeName ptype
                        |> Option.defaultValue ([pname |> String.toPascalCase])
                    simpleUnionCase psafe [anonAppField propTypeName]))
            |> unionDefinition

        // static member toDynamicMemberDef(prop: TestComponentProps) =
        //     match prop with
        //     | AProp (p) -> "aProp", box p
        //     | BProp (p) -> "bProp", box p
        //     | CProp (p) -> "cProp", box (p.ToString())
        //
        /// Define the static method "toMemberDef"
        let componentPropertyToDynamicMemberDefDeclaration =
            functionPattern "toDynamicMemberDef" [("prop", SynType.Create parameters.ComponentPropsName)]
            |> binding  
              ( SynExpr.CreateIdentString "prop"
                    |> matchStatement 
                      ( (parameters.DUSafePropertyNames, parameters.PropertyNames, parameters.PropertyTypes)
                        |||> List.zip3
                        |> List.map (fun (psafe, pname, prop) -> 
                            let pConvert =
                                match prop.propType with 
                                | Some (Array _)
                                | Some (Bool _)
                                | Some (Number _)
                                | Some (String _)-> 
                                    SynExpr.CreateIdentString "p"

                                | Some (Object _)
                                | Some (Any _)
                                | Some (Element _)
                                | Some (Node _) -> 
                                    SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["JsonConvert"; "SerializeObject"], SynExpr.CreateIdentString "p")
                                    |> SynExpr.CreateParen

                                // Special cases, each type has a custom ToString
                                | Some (Enum _)
                                | Some (Union _)
                                | Some (ArrayOf _)
                                | Some (ObjectOf _)
                                | Some (Shape _)
                                | Some (Exact _)
                                | Some (FlowUnion _)
                                | Some (FlowArray _)
                                | Some (FlowObject _) -> 
                                    SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["p"; "ToString"])
                                    |> SynExpr.CreateParen

                                // A type we can't process
                                | Some (Other _)
                                | None -> 
                                    SynExpr.CreateIdentString "p"
                            simpleMatchClause psafe ["p"] None 
                              ( SynExpr.CreateTuple 
                                    [ SynExpr.CreateConstString pname
                                      application [ SynExpr.CreateIdentString "box"; pConvert ] ] )) ) )
            
            |> SynMemberDefn.CreateStaticMember

        // type TestComponentProps =
        //
        // Create the type definition
        parameters.ComponentPropsName
        |> componentInfo
        |> withXMLDoc (parameters.Metadata |> generateComponentPropsDocumentation false |> toXMLDoc)
        |> simpleTypeDeclaration 
            componentPropertyDUCases
            [ componentPropertyToDynamicMemberDefDeclaration ] 

    /// Define the component property descriminated union
    let componentAttributeDUDeclaration =
        // | Prop of SampleDashComponentProps
        // | Children of DashComponent list
        //
        /// Define the cases for the descriminated union
        let componentAttributeDUCases =
            [ simpleUnionCase "Prop" [anonSimpleField parameters.ComponentPropsName]
              simpleUnionCase "Children" [anonAppField ["list"; "DashComponent"]] ]
            |> unionDefinition

        //  static member AProp(p: string) = Prop(AProp p)
        //  static member BProp(p: int) = Prop(BProp p)
        //  static member CProp(p: CProp) = Prop(CProp p)
        //
        /// Create Feliz style attribute constructors for properties
        let componentPropertyConstructorDeclarations =
            (parameters.DUSafePropertyNames, parameters.PropertyNames, parameters.PropertyTypes)
            |||> List.zip3
            |> List.choose (fun (psafe, pname, prop) ->
                prop.propType
                |> Option.map (fun ptype ->
                    let propTypeName =
                        SafeReactPropType.tryGetFSharpTypeName ptype
                        |> Option.defaultValue ([pname |> String.toPascalCase])
                    functionPattern pname [("p", appType propTypeName)]
                    |> binding (application [ SynExpr.CreateIdentString "Prop"; application [SynExpr.CreateIdentString psafe; SynExpr.CreateIdentString "p"] |> SynExpr.CreateParen])
                    |> withXMLDocLet (prop |> generateComponentPropDescription |> toXMLDoc)
                    |> SynMemberDefn.CreateStaticMember))

        //  static member children(value: int) = Children([ Html.Html.text value ])
        //  static member children(value: string) = Children([ Html.Html.text value ])
        //  static member children(value: float) = Children([ Html.Html.text value ])
        //  static member children(value: System.Guid) = Children([ Html.Html.text value ])
        //  static member children(value: DashComponent) = Children([ value ])
        //  static member children(value: list<DashComponent>) = Children(value)
        //  static member children(value: seq<DashComponent>) = Children(List.ofSeq value)
        //
        /// Create Feliz style attribute constructors for children
        let componentChildrenConstructorDeclarations =
            let createCon ty app =
                functionPattern "children" [("value", ty)]
                |> binding (application [SynExpr.CreateIdentString "Children"; app |> SynExpr.CreateParen])
                |> withXMLDocLet (["The child or children of this dash component"] |> toXMLDoc)
                |> SynMemberDefn.CreateStaticMember
            [ createCon (appType ["int"]) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "Html"; "text"]); SynExpr.CreateIdentString "value" ] ]) 
              createCon (appType ["string"]) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "Html"; "text"]); SynExpr.CreateIdentString "value" ] ])
              createCon (appType ["float"]) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "Html"; "text"]); SynExpr.CreateIdentString "value" ] ])
              createCon (SynType.CreateLongIdent (LongIdentWithDots.Create ["System"; "Guid"]) ) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "Html"; "text"]); SynExpr.CreateIdentString "value" ] ])
              createCon (appType ["DashComponent"]) (expressionList [ SynExpr.CreateIdentString "value" ])
              createCon (appType ["list"; "DashComponent"]) (SynExpr.CreateIdentString "value")
              createCon (appType ["seq"; "DashComponent"]) (application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["List"; "ofSeq"]); SynExpr.CreateIdentString "value" ]) ]

        // type SampleDashComponentAttr =
        //
        // Create the type definition
        parameters.ComponentAttrsName
        |> componentInfo
        |> withXMLDoc (["A list of children or a property for this dash component"] |> toXMLDoc)
        |> simpleTypeDeclaration 
            componentAttributeDUCases
            [ yield! componentPropertyConstructorDeclarations
              yield! componentChildrenConstructorDeclarations ] 

    /// Define the component class
    let componentTypeDeclaration =
        
        /// Define the static method "applyMembers"
        let componentTypeApplyMembersDeclaration =

            // static member applyMembers
            // (
            //     id: string,
            //     children: seq<DashComponent>,
            //     ?aProp: string,
            //     ?bProp: string,
            //     ?cProp: string
            // ) =
            memberFunctionPattern "applyMembers" 
                [ yield ("id", SynType.Create "string", true)
                  yield ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true) 
                  yield! parameters.PropertyNames |> List.map (fun prop -> prop, SynType.Create "string", false) ]
            |> binding  
              ( expressionSequence
                    [ // let props = DashComponentProps()
                      // DynObj.setValue props "id" id
                      // DynObj.setValue props "children" children
                      yield patternNamed "props" |> binding (application [SynExpr.CreateIdentString "DashComponentProps"; SynExpr.CreateUnit]) |> Let
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "id"; SynExpr.CreateIdentString "id"] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "children"; SynExpr.CreateIdentString "children"] |> Expression
                      
                      // DynObj.setValueOpt props "aProp" aProp
                      // DynObj.setValueOpt props "bProp" bProp
                      // DynObj.setValueOpt props "cProp" cProp
                      yield! parameters.PropertyNames 
                             |> List.map (fun prop -> 
                                application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValueOpt"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString prop; SynExpr.CreateIdentString prop] |> Expression)

                      // DynObj.setValue t "namespace" "TestNamespace"
                      // DynObj.setValue t "props" props
                      // DynObj.setValue t "type" "TestType"
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "namespace"; SynExpr.CreateConstString parameters.ComponentNamespace] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "props"; SynExpr.CreateIdentString "props"] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "type"; SynExpr.CreateConstString parameters.ComponentType] |> Expression
                      
                      // t
                      yield SynExpr.CreateIdentString "t" |> Expression ]

                // fun (t: TestComponent) ->
                |> typedLambdaStatement false [("t", SynType.Create parameters.ComponentName)]
                |> SynExpr.CreateParen )
            
            |> SynMemberDefn.CreateStaticMember

        //  static member init(id: string, children: seq<DashComponent>, ?aProp: string, ?bProp: string, ?cProp: string) =
        //      TestComponent.applyMembers (id, children, ?aProp = aProp, ?bProp = bProp, ?cProp = cProp) (TestComponent())
        //
        /// Define the static method "init"
        let componentTypeInitDeclaration =
            memberFunctionPattern "init" 
                [ ("id", SynType.Create "string", true)
                  ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true)
                  yield! parameters.PropertyNames |> List.map (fun prop -> prop, SynType.Create "string", false) ]
            |> binding
              ( application
                    [ SynExpr.CreateLongIdent (LongIdentWithDots.Create [parameters.ComponentName; "applyMembers"])
                      SynExpr.CreateParenedTuple 
                        [ yield SynExpr.CreateIdentString "id"
                          yield SynExpr.CreateIdentString "children"
                          yield! parameters.PropertyNames 
                                 |> List.map (fun prop -> 
                                    application [SynExpr.CreateLongIdent (true, LongIdentWithDots.CreateString prop, None); SynExpr.CreateIdentString "="; SynExpr.CreateIdentString prop]) ]
                      application [SynExpr.CreateIdentString parameters.ComponentName; SynExpr.CreateUnit] |> SynExpr.CreateParen ] )

            |> SynMemberDefn.CreateStaticMember

        // [ "TestNamespace.min.js"
        //   "supplimentary.min.js" ]
        //
        /// Define the list of javascript files to include
        let componentJavascriptList =
            parameters.ComponentJavascript
            |> List.map String.escape
            |> List.map SynExpr.CreateConstString
            |> expressionList

        //  static member definition: LoadableComponentDefinition =
        //      { ComponentName = "TestComponent"
        //        ComponentJavascript = ... }
        //
        /// Define the static member "definition"
        let componentTypeDefinitionDeclaration =
            patternNamed "definition"
            |> withPatternType (SynType.Create "LoadableComponentDefinition")
            |> binding
              ( SynExpr.CreateRecord 
                    [ ((LongIdentWithDots.CreateString "ComponentName",true), Some (SynExpr.CreateConstString parameters.ComponentName)) 
                      ((LongIdentWithDots.CreateString "ComponentJavascript",true), Some componentJavascriptList) ] )

            |> SynMemberDefn.CreateStaticMember

        // ///This is additional test documentation
        // type TestComponent() =
        //     inherit DashComponent()
        //
        // Create the type definition
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc (parameters.Metadata |> generateComponentDescription |> toXMLDoc)
        |> typeDeclaration 
          [ SynMemberDefn.CreateImplicitCtor() //adds the "()" to the type name
            typeInherit (application [SynExpr.CreateIdentString "DashComponent"; SynExpr.CreateUnit]) 
            componentTypeApplyMembersDeclaration
            componentTypeInitDeclaration
            componentTypeDefinitionDeclaration ]

    /// Define the component DSL function (used when creating the DOM tree)
    let componentLetDeclaration =

        /// Define the inner expression
        let componentDeclaration =
            expressionSequence
              [ //  let props, children =
                //      List.fold
                //          (fun (props, children) (a: SampleDashComponentAttr) ->
                //                  match a with
                //                  | Prop prop -> (prop :: props, children)
                //                  | Children child -> (props, child @ children))
                //          ([], [])
                //          attrs
                patternNamedTuple ["props"; "children"] |> binding 
                  ( application
                      [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["List"; "fold"])
                        
                        SynExpr.CreateIdentString "a"
                        |> matchStatement
                            [ simpleMatchClause "Prop" ["prop"] None ( SynExpr.CreateTuple [ application [ SynExpr.CreateIdentString "prop"; SynExpr.CreateIdentString "::"; SynExpr.CreateIdentString "props" ]; SynExpr.CreateIdentString "children" ] )
                              simpleMatchClause "Children" ["child"] None ( SynExpr.CreateTuple [ SynExpr.CreateIdentString "props"; application [ SynExpr.CreateIdentString "child"; SynExpr.CreateIdentString "@"; SynExpr.CreateIdentString "children" ] ] ) ]
                        |> typedLambdaStatement true [ ("a", SynType.Create parameters.ComponentAttrsName) ]
                        |> simpleLambdaStatement false [ "props"; "children" ]
                        |> SynExpr.CreateParen

                        SynExpr.CreateTuple [expressionList []; expressionList []] |> SynExpr.CreateParen
                        SynExpr.CreateIdentString "attrs" ] ) |> Let 
              
                //  let t = TestComponent.init (id, children)
                patternNamed "t" |> binding (application [SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentName; "init"]); SynExpr.CreateParenedTuple [SynExpr.CreateIdentString "id"; SynExpr.CreateIdentString "children"]]) |> Let
                
                //  let componentProps =
                //      match t.TryGetTypedValue<DashComponentProps> "props" with
                //      | Some (p) -> p
                //      | None -> DashComponentProps()
                patternNamed "componentProps" |> binding
                  ( SynExpr.CreateInstanceMethodCall(LongIdentWithDots.CreateString "t.TryGetTypedValue", [SynType.Create "DashComponentProps"], SynExpr.CreateConstString "props")
                    |> matchStatement
                        [ simpleMatchClause "Some" ["p"] None (SynExpr.CreateIdentString "p") 
                          simpleMatchClause "None" [] None (application [SynExpr.CreateIdentString "DashComponentProps"; SynExpr.CreateUnit]) ]) |> Let 
                
                //  Seq.iter
                //      (fun (prop: TestComponentProps) ->
                //          let fieldName, boxedProp =
                //              TestComponentProps.toDynamicMemberDef prop
                //          DynObj.setValue componentProps fieldName boxedProp)
                //      props
                application
                  [ SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "Seq.iter")
                    expressionSequence
                      [ patternNamedTuple ["fieldName"; "boxedProp"] |> binding (application [SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentPropsName; "toDynamicMemberDef"]); SynExpr.CreateIdentString "prop"]) |> Let
                        application [SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "componentProps"; SynExpr.CreateIdentString "fieldName"; SynExpr.CreateIdentString "boxedProp" ] |> Expression ]
                    |> typedLambdaStatement false [("prop", SynType.Create parameters.ComponentPropsName)]
                    |> SynExpr.CreateParen
                    SynExpr.CreateIdentString "props" ] |> Expression
                
                //  DynObj.setValue t "props" componentProps
                application [SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "props"; SynExpr.CreateIdentString "componentProps" ] |> Expression 
                
                //  t :> DashComponent
                SynExpr.CreateIdentString "t" |> expressionUpcast (SynType.Create "DashComponent") |> Expression ]

        // ///This is additional test documentation
        // let testComponent (id: string) (props: seq<TestComponentProps>) (children: seq<DashComponent>) =
        //
        // Create the binding
        functionPattern parameters.CamelCaseComponentName
            [ ("id", SynType.Create "string")
              ("attrs", SynType.CreateApp(SynType.Create "list", [SynType.Create parameters.ComponentAttrsName])) ]
        |> binding componentDeclaration
        |> withXMLDocLet (parameters.Metadata |> generateComponentDocumentation |> toXMLDoc)
        |> letDeclaration

    //  ///This is additional test documentation
    //  [<RequireQualifiedAccess>]
    //  module TestComponent =
    //
    /// Define the component module
    let moduleDeclaration = 
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc (parameters.Metadata |> generateComponentDescription |> toXMLDoc)
        |> withModuleAttribute (SynAttribute.Create "RequireQualifiedAccess")
        |> nestedModule
            [ yield! componentPropertyTypeDeclarations
              yield componentPropertyDUDeclaration
              yield componentAttributeDUDeclaration
              yield componentTypeDeclaration
              yield componentLetDeclaration ]

    //  namespace TestNamespace
    //  open Dash.NET
    //  open System
    //  open Plotly.NET
    //
    /// Define the component namespace
    let namespaceDeclaration =
        parameters.LibraryNamespace
        |> namespaceInfo
        |> withNamespaceDeclarations
            [ SynModuleDecl.CreateOpen "Dash.NET" 
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "Plotly.NET"
              SynModuleDecl.CreateOpen "Newtonsoft.Json"
              moduleDeclaration ] 

    // Create the file
    ParsedImplFileInputRcd
        .CreateFs(parameters.ComponentName)
        .AddModule(namespaceDeclaration)
    |> ParsedInput.CreateImplFile

let generateCodeStringFromAST (log: Core.Logger) (path: string) ast =
    async {
        let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true }
        let! formattedCode = CodeFormatter.FormatASTAsync(ast, Path.GetFileName path, [], None, cfg)

        return
            [ "//------------------------------------------------------------------------------"
              "//        This file has been automatically generated."
              "//        Changes to this file will be lost if it is regenerated."
              "//------------------------------------------------------------------------------"
              formattedCode ]
            |> String.concat Environment.NewLine
    }

let generateCodeFromAST (log: Core.Logger) (path: string) ast =
    async {
        let! formattedCode = generateCodeStringFromAST log path ast
        try
            File.WriteAllText(path,formattedCode)
            log.Debug("Created file {ComponentFSharpFile}",path)
            return true
        with | ex -> 
            log.Error(ex, "Failed to write file {ComponentFSharpFile}",path)
            return false
    }
            