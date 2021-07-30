module Dash.NET.ComponentGeneration.ASTGeneration

open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open Fantomas
open System
open System.IO
open FSharp.Compiler.XmlDoc
open Prelude
open ASTHelpers
open ComponentParameters
open ReactMetadata

// TODO: Clean up this file
// TODO: Add documentation

let createComponentAST (parameters: ComponentParameters) =

    printfn "Creating component bindings"

    let componentPropertyTypeDeclarations =
        let rec generatePropTypes (name: string) (ptype: SafeReactPropType) =

            let propTypeName = name |> String.capitalize

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
                        | _ -> None)
                
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
                        | _ -> None)

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
                    |> withXMLDoc "This is test documentation" // TODO documentation comments
                    |> simpleTypeDeclaration 
                         (duCases |> unionDefinition)
                         [ toCaseValueDefinition ] 
                    |> List.singleton
                    |> Some

                else
                    None

            | SafeReactPropType.Union (_, Some utypes) -> 
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
                    |> List.choose (fun (i, case) -> 
                        let caseName = (sprintf "%sCase%d" propTypeName i)
                        let caseTypeName = 
                            case 
                            |> SafeReactPropType.tryGetFSharpTypeName
                            |> Option.defaultValue (sprintf "%sCase%dType" propTypeName i)

                        simpleUnionCase caseName [anonSimpleField caseTypeName]
                        |> Some)

                // | CPropCase0 (v) -> v.ToString()
                // | CPropCase1 (v) -> v.ToString()
                //
                /// Define the values for the cases
                let caseValues =
                    utypes
                    |> List.indexed
                    |> List.choose (fun (i, _) -> 
                        let caseName = (sprintf "%sCase%d" propTypeName i)
                        simpleMatchClause caseName ["v"] None ( SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["v"; "ToString"]) ) 
                        |> Some)

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
                        |> withXMLDoc "This is test documentation" // TODO documentation comments
                        |> simpleTypeDeclaration 
                                (duCases |> unionDefinition)
                                [ toCaseValueDefinition ]

                    (unionTypeDefinition :: recursiveTypes) 
                    |> List.rev 
                    |> Some

                else
                    None

            | SafeReactPropType.ArrayOf (_, Some utype) -> 
                //TODO Json compatable ToString
                generatePropTypes (sprintf "%sType" propTypeName) utype

            | SafeReactPropType.ObjectOf (_, Some utype) -> 
                //TODO Json compatable ToString
                generatePropTypes (sprintf "%sType" propTypeName) utype

            | SafeReactPropType.Shape (_, Some values) 
            | SafeReactPropType.Exact (_, Some values) -> 
                let fields =
                    (values.Keys |> List.ofSeq, values.Values |> List.ofSeq)
                    ||> List.zip 

                if fields.Length > 0 then
                    let recursiveTypes =
                        fields
                        |> List.choose (fun (pname, ptype) -> 
                            generatePropTypes (sprintf "%s%sType" propTypeName (pname |> String.capitalize)) ptype)
                        |> List.concat
                
                    // { AField: bool
                    //   BField: string
                    //   CField: string }
                    //
                    /// Define the fields for the record
                    let fieldDefinitions =
                        fields
                        |> List.map (fun (pname, ptype) -> 
                            let fieldTypeName =
                                SafeReactPropType.tryGetFSharpTypeName ptype
                                |> Option.defaultValue (sprintf "%s%sType" propTypeName (pname |> String.capitalize))
                            simpleField (pname |> String.capitalize) fieldTypeName)

                    // override this.ToString() =
                    //     JsonSerializer.Serialize
                    //         {| aField = this.AField.ToString()
                    //            bField = this.BField.ToString()
                    //            cField = this.CField.ToString() |}
                    //
                    /// Define the json conversion
                    let toStringDefinition =
                        let toString =
                            let serializable =
                                fields
                                |> List.map (fun (pname, _) -> 
                                    ( Ident.Create pname, 
                                      SynExpr.CreateInstanceMethodCall( LongIdentWithDots.Create ["this"; pname |> String.capitalize; "ToString"] ) ))
                                |> anonRecord

                            SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["JsonSerializer"; "Serialize"], serializable)

                        functionPatternThunk "this.ToString"
                        |> binding toString
                        |> SynMemberDefn.CreateOverrideMember

                    // type BProp =
                    // 
                    /// Create the record definition
                    let recordTypeDefinition =
                        propTypeName
                        |> componentInfo
                        |> withXMLDoc "This is test documentation" // TODO documentation comments
                        |> simpleTypeDeclaration 
                                (fieldDefinitions |> recordDefinition)
                                [ toStringDefinition ]

                    (recordTypeDefinition :: recursiveTypes)
                    |> List.rev 
                    |> Some

                else
                    None

            // Other types dont need definitions
            | _ -> None

        (parameters.PropertyNames, parameters.PropertyTypes)
        ||> List.zip 
        |> List.choose (fun (pname, ptype) -> 
            ptype.propType
            |> Option.bind (generatePropTypes pname))
        |> List.concat

    /// Define the component property descriminated union
    let componentPropertyDUDeclaration =

        // | AProp of string
        // | BProp of number
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
                        |> Option.defaultValue (pname |> String.capitalize)
                    simpleUnionCase psafe [anonSimpleField propTypeName]))
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
                                match prop.propType |> Option.bind SafeReactPropType.tryGetFSharpTypeName with
                                | Some _ -> 
                                    SynExpr.CreateIdentString "p"
                                | None -> 
                                    SynExpr.CreateInstanceMethodCall(LongIdentWithDots.Create ["p"; "ToString"])
                                    |> SynExpr.CreateParen
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
        |> withXMLDoc "This is test documentation" // TODO documentation comments
        |> simpleTypeDeclaration 
            componentPropertyDUCases
            [ componentPropertyToDynamicMemberDefDeclaration ] 

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
                |> simpleLambdaStatement [("t", SynType.Create parameters.ComponentName)]
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

        //  static member definition: LoadableComponentDefinition =
        //      { ComponentName = "TestComponent"
        //        ComponentJavascript = "TestNamespace.min.js" }
        //
        /// Define the static member "definition"
        let componentTypeDefinitionDeclaration =
            patternNamed "definition"
            |> withPatternType (SynType.Create "LoadableComponentDefinition")
            |> binding
              ( SynExpr.CreateRecord 
                    [ ((LongIdentWithDots.CreateString "ComponentName",true), Some (SynExpr.CreateConstString parameters.ComponentName)) 
                      ((LongIdentWithDots.CreateString "ComponentJavascript",true), Some (SynExpr.CreateConstString parameters.ComponentJavascript)) ] )

            |> SynMemberDefn.CreateStaticMember

        // ///This is additional test documentation
        // type TestComponent() =
        //     inherit DashComponent()
        //
        // Create the type definition
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc "This is additional test documentation" // TODO documentation comments
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
              [ //  let t = TestComponent.init (id, children)
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
                    |> simpleLambdaStatement [("prop", SynType.Create parameters.ComponentPropsName)]
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
              ("props", SynType.CreateApp(SynType.Create "seq", [SynType.Create parameters.ComponentPropsName]))
              ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"])) ]
        |> binding componentDeclaration
        |> withXMLDocLet "This is additional test documentation" //TODO documentation comments
        |> letDeclaration

    //  ///This is additional test documentation
    //  [<RequireQualifiedAccess>]
    //  module TestComponent =
    //
    /// Define the component module
    let moduleDeclaration = 
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc "This is additional test documentation" // TODO documentation comments
        |> withModuleAttribute (SynAttribute.Create "RequireQualifiedAccess")
        |> nestedModule
            [ yield! componentPropertyTypeDeclarations
              yield componentPropertyDUDeclaration 
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
              SynModuleDecl.CreateOpen "System.Text.Json"
              moduleDeclaration ] 

    // Create the file
    ParsedImplFileInputRcd
        .CreateFs(parameters.ComponentName)
        .AddModule(namespaceDeclaration)
    |> ParsedInput.CreateImplFile

let generateCodeFromAST (path: string) ast =
    async {
        let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true }
        let! formattedCode = CodeFormatter.FormatASTAsync(ast, Path.GetFileName path, [], None, cfg)

        let formattedCode =
            [ "//------------------------------------------------------------------------------" // TODO documentation comments
              "//        This file has been automatically generated."
              "//        Changes to this file will be lost when the code is regenerated."
              "//------------------------------------------------------------------------------"
              formattedCode ]
            |> String.concat Environment.NewLine

        try
            File.WriteAllText(path,formattedCode)
            return true, sprintf "Created %s" path, ""
        with | ex -> 
            return false, "", sprintf "Failed to write %s\n%s" path (ex.ToString())
    }
            