﻿module Dash.NET.ComponentGeneration.ASTGeneration

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

let createCSharpComponentAST (log: Core.Logger) (parameters: ComponentParameters) : ParsedInput =

    // Helpers
    let oAttr = Ident.Create "OAttr"
    //let guardNull name = ...
    let gListMap = SynExpr.CreateIdentStringWithDots "List.map"
    let gArrayMap = SynExpr.CreateIdentStringWithDots "Array.map"
    let gSeqMap = SynExpr.CreateIdentStringWithDots "Seq.map"
    let gListOfArray = SynExpr.CreateIdentStringWithDots "List.ofArray"
    let gAttrUnwrap = SynExpr.CreateIdentStringWithDots "Attr.Unwrap"
    let gPipe = SynExpr.CreateIdentString "|>"
    let gComponentWrap = SynExpr.CreateIdentStringWithDots "Dash.NET.CSharp.Dsl.DashComponent.Wrap"
    let gAttrWrap = SynExpr.CreateIdentStringWithDots "Attr.Wrap"
    let gDashComponent = SynType.CreateIdentStringWithDots "Dash.NET.CSharp.Dsl.DashComponent"
    let gDashComponentUnwrap = SynExpr.CreateIdentStringWithDots "Dash.NET.CSharp.Dsl.DashComponent.Unwrap"


    let attrDeclaration =

        /// Create Feliz style attribute constructors for children
        let componentChildrenConstructorDeclarations =
            let createCon ty args app =
                functionPatternTupled "children" [("value", ty, args)]
                |> binding app
                |> withXMLDocLet (["The child or children of this dash component"] |> toXMLDoc)
                |> SynMemberDefn.CreateStaticMember

            [
                createCon (appType ["int"]) None (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; SynExpr.CreateIdentString "value"; gPipe; gAttrWrap ]) 
                createCon (appType ["string"]) None (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; SynExpr.CreateIdentString "value"; gPipe; gAttrWrap ])
                createCon (appType ["float"]) None (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; SynExpr.CreateIdentString "value"; gPipe; gAttrWrap ])
                createCon (SynType.CreateIdentStringWithDots "System.Guid") None (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; SynExpr.CreateIdentString "value"; gPipe; gAttrWrap ])
                createCon (gDashComponent) None (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; applicationNest [ SynExpr.CreateIdentString "value"; gPipe; gDashComponentUnwrap ]; gPipe; gAttrWrap ])
                createCon (appType ["array"; "Dash.NET.CSharp.Dsl.DashComponent"]) (Some [ SynAttribute.Create "ParamArray" ]) (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; applicationNest [ SynExpr.CreateIdentString "value"; gPipe; gArrayMap; gDashComponentUnwrap ]; gPipe; gAttrWrap ])
                createCon (appType ["seq"; "Dash.NET.CSharp.Dsl.DashComponent"]) None (application [ SynExpr.CreateIdentStringWithDots "OAttr.children"; applicationNest [ SynExpr.CreateIdentString "value"; gPipe; gSeqMap; gDashComponentUnwrap ]; gPipe; gAttrWrap ])
            ]

        let createMemberWrap () =
            functionPattern "Wrap" []
            |> binding (application [SynExpr.CreateIdentStringWithDots "Attr.WrappedAttr"])
            |> SynMemberDefn.CreateStaticMember

        let createMemberUnwrap () =
            //functionPattern "Unwrap" [("attr", SynType.Create "Attr")]
            SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString "Unwrap", [ SynPatRcd.CreateLongIdent (LongIdentWithDots.CreateString "Attr.WrappedAttr", [ (SynPatRcd.CreateLongIdent (LongIdentWithDots.CreateString "attr", [])) ] ) |> SynPatRcd.CreateParen ])
            |> binding (application [SynExpr.CreateIdentString "attr"])
            |> SynMemberDefn.CreateStaticMember

        // private WrappedAttr of OAttr
        //
        /// Attribute declaration
        let componentPropertyDUCase =
            [
                simpleUnionCase "WrappedAttr" [ anonSimpleField "OAttr" ]
            ]
            |> SynTypeDefnSimpleReprUnionRcd.Create
            |> fun x -> { x with Access = Some SynAccess.Private }
            |> SynTypeDefnSimpleReprRcd.Union

        // type Attr =
        //
        // Create the type definition
        "Attr"
        |> componentInfo
        |> simpleTypeDeclaration
            componentPropertyDUCase
            [
                createMemberWrap ()
                createMemberUnwrap ()
                yield! componentChildrenConstructorDeclarations
            ]

    /// Original attribute name alias
    let originalAttribute =
        let typeAbbrev =
            { 
                SynTypeDefnSimpleReprTypeAbbrevRcd.ParseDetail = ParserDetail.Ok
                SynTypeDefnSimpleReprTypeAbbrevRcd.Type = SynType.Create $"Dash.NET.DCC.{parameters.ComponentName}.Attr"
                SynTypeDefnSimpleReprTypeAbbrevRcd.Range = FSharp.Compiler.Range.range.Zero
            }

        SynModuleDecl.CreateSimpleType (
            { SynComponentInfoRcd.Create [ oAttr ] with Access = Some SynAccess.Internal },
            SynTypeDefnSimpleReprRcd.TypeAbbrev typeAbbrev
        )

    /// Define the component DSL function (used when creating the DOM tree)
    let componentLetDeclaration =

        /// Define the inner expression
        let componentDeclaration =
            application [
                SynExpr.CreateIdentStringWithDots $"{parameters.LibraryNamespace}.{parameters.ComponentName}.{parameters.CamelCaseComponentName}"
                SynExpr.CreateIdentString "id"
                applicationNest [
                    SynExpr.CreateIdentString "attrs"
                    gPipe
                    gListOfArray
                    gPipe
                    gListMap
                    gAttrUnwrap
                ]
                gPipe
                gComponentWrap
            ]

        // ///This is additional test documentation
        // let testComponent (id: string) (props: seq<TestComponentProps>) (children: seq<DashComponent>) =
        //
        // Create the binding
        functionPatternTupled parameters.CamelCaseComponentName
            [ ("id", SynType.Create "string", None)
              ("attrs", SynType.CreateApp(SynType.Create "array", [SynType.Create parameters.ComponentAttrsName]), Some [ SynAttribute.Create "ParamArray" ]) ]
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
        |> nestedModule [
            yield originalAttribute
            yield attrDeclaration
            yield componentLetDeclaration 
        ]
            //[ yield! componentPropertyTypeDeclarations
            //  yield componentPropertyDUDeclaration
            //  yield componentAttributeDUDeclaration
            //  yield componentTypeDeclaration
            //  yield componentLetDeclaration ]

    //  namespace TestNamespace
    //  open Dash.NET
    //  open System
    //  open Plotly.NET
    //  open DynamicObj
    //
    /// Define the component namespace
    let namespaceDeclaration =
        parameters.LibraryNamespace
        |> namespaceInfo
        |> withNamespaceDeclarations
            [ SynModuleDecl.CreateOpen "Dash.NET" 
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "Plotly.NET"
              SynModuleDecl.CreateOpen "DynamicObj"
              SynModuleDecl.CreateOpen "Newtonsoft.Json"
              moduleDeclaration ] 

    // Create the file
    ParsedImplFileInputRcd
        .CreateFs(parameters.ComponentName)
        .AddModule(namespaceDeclaration)
    |> ParsedInput.CreateImplFile

let createComponentAST (log: Core.Logger) (parameters: ComponentParameters) : ParsedInput =

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
                    // override this.Convert() =
                    //     match this with
                    //
                    /// Define the json conversion
                    let toCaseValueDefinition =
                        functionPatternNoArgTypes "convert" ["this"]
                        |> binding  
                          ( application 
                              [ SynExpr.CreateIdentString "box"
                                SynExpr.CreateIdentString "this" |> matchStatement caseValues |> SynExpr.CreateParen ] )
                        |> SynMemberDefn.CreateStaticMember
                 
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

                let cases =
                    utypes
                    |> List.indexed
                    |> List.map (fun (i, case) -> 
                        let caseTypeName = 
                            case 
                            |> SafeReactPropType.tryGetFSharpTypeName
                            |> Option.defaultValue ([sprintf "%sCase%dType" propTypeName i])

                        let caseName = 
                            caseTypeName 
                            |> List.rev
                            |> List.map String.toPascalCase
                            |> List.reduce (sprintf "%s%s")

                        (i, case, caseTypeName, caseName))

                // | CPropCase0 of CPropCase0Type
                // | CPropCase1 of bool
                //
                /// Define the cases for the descriminated union
                let duCases =
                    cases
                    |> List.map (fun (_, _, caseTypeName, caseName) ->
                        simpleUnionCase caseName [anonAppField caseTypeName])

                // | CPropCase0 (v) -> box v
                // | CPropCase1 (v) -> box v
                //
                /// Define the values for the cases
                let caseValues =
                    cases
                    |> List.map (fun (i, utype, _, caseName) ->
                        let boxable =
                            if SafeReactPropType.needsConvert utype then 
                                let caseTypeName = sprintf "%sCase%dType" propTypeName i

                                application
                                  [ SynExpr.CreateIdentString "v"
                                    SynExpr.CreateIdentString "|>" 
                                    SynExpr.CreateLongIdent (LongIdentWithDots.Create [caseTypeName; "convert"]) ]
                                |> SynExpr.CreateParen
                            else
                                SynExpr.CreateIdentString "v"
                        simpleMatchClause caseName ["v"] None ( application [ SynExpr.CreateIdentString "box"; boxable ] ))

                // member this.Convert() =
                //     match this with
                //
                /// Define the json conversion
                let toCaseValueDefinition =
                    functionPatternNoArgTypes "convert" ["this"]
                    |> memberBinding  
                        ( SynExpr.CreateIdentString "this"
                        |> matchStatement caseValues )
                    |> SynMemberDefn.CreateStaticMember

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

                // member this.Convert() =
                //     match this with
                //     | DProp (v) -> List.map (fun (i: string) -> box i) v
                //
                /// Define the json conversion
                let toCaseValueDefinition =
                    let matchResult =
                        application
                            [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["List"; "map"])
                              application 
                                [ SynExpr.CreateIdentString "box"
                                  if SafeReactPropType.needsConvert utype then 
                                      application
                                        [ SynExpr.CreateIdentString "i"
                                          SynExpr.CreateIdentString "|>" 
                                          SynExpr.CreateLongIdent (LongIdentWithDots.Create [caseInnerType |> List.head; "convert"]) ]
                                      |> SynExpr.CreateParen
                                  else
                                      SynExpr.CreateIdentString "i" ]
                              |> typedLambdaStatement false [("i", appType caseInnerType)]
                              |> SynExpr.CreateParen
                              SynExpr.CreateIdentString "v"]

                    let matchCase =
                        SynExpr.CreateIdentString "this"
                        |> matchStatement 
                            [ simpleMatchClause propTypeName ["v"] None matchResult ]

                    let objectBox =
                        application 
                          [ SynExpr.CreateIdentString "box"
                            matchCase |> SynExpr.CreateParen ]

                    functionPatternNoArgTypes "convert" ["this"]
                    |> memberBinding objectBox
                    |> SynMemberDefn.CreateStaticMember

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

            | SafeReactPropType.ObjectOf (_, Some utype) -> //TODO
                let recursiveTypes = 
                    generatePropTypes (sprintf "%sValue" propTypeName) utype
                    |> Option.defaultValue []
                
                let caseInnerType = 
                    utype 
                    |> SafeReactPropType.tryGetFSharpTypeName
                    |> Option.defaultValue ([sprintf "%sValue" propTypeName])

                //  | MarksType (v) ->
                //      (v.Keys, v.Values |> Seq.map (fun p -> box (p.Convert()))) ||> Seq.zip |> Map.ofSeq
                //
                /// Define the values for the cases
                let caseValue =
                    let boxable =
                        if SafeReactPropType.needsConvert utype then 
                            application
                              [ SynExpr.CreateIdentString "p"
                                SynExpr.CreateIdentString "|>" 
                                SynExpr.CreateLongIdent (LongIdentWithDots.Create [caseInnerType |> List.head; "convert"]) ]
                            |> SynExpr.CreateParen
                        else
                            SynExpr.CreateIdentString "p"

                    let valueConvert =
                        application [ SynExpr.CreateIdentString "box"; boxable ]
                        |> simpleLambdaStatement true ["p"]
                        |> simpleLambdaStatement false ["k"]
                        |> SynExpr.CreateParen

                    let convertDict =
                        application
                          [ SynExpr.CreateIdentString "v"
                            SynExpr.CreateIdentString "|>"
                            application
                              [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Map"; "map"]) 
                                valueConvert ] ]

                    simpleMatchClause propTypeName ["v"] None convertDict

                // member this.Convert() =
                //     match this with
                //
                /// Define the json conversion
                let toCaseValueDefinition =
                    functionPatternNoArgTypes "convert" ["this"]
                    |> memberBinding  
                      ( application 
                          [ SynExpr.CreateIdentString "box"
                            SynExpr.CreateIdentString "this" |> matchStatement [caseValue] |> SynExpr.CreateParen ] )
                    |> SynMemberDefn.CreateStaticMember

                // type MarksType = 
                //    | MarksType of Map<string, MarksTypeType>
                //
                /// Create the alias definition
                let aliasDefinition =
                    propTypeName
                    |> componentInfo
                    |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                    |> simpleTypeDeclaration
                      ( SynType.CreateApp (SynType.Create "Map", [SynType.Create "string"; appType caseInnerType]) 
                        |> anonTypeField 
                        |> List.singleton
                        |> simpleUnionCase propTypeName
                        |> List.singleton
                        |> unionDefinition )
                        [ toCaseValueDefinition ]

                (aliasDefinition :: recursiveTypes)
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
                                |> Option.defaultValue ([sprintf "%s%sType" propTypeName (pname |> String.toPascalCase)])
                            simpleAppField (pname |> String.toPascalCase) fieldTypeName)

                    // member this.Convert() =
                    //     box
                    //         {| aField = this.AField.Convert()
                    //            bField = this.BField.Convert()
                    //            cField = this.CField |}
                    //
                    /// Define the json conversion
                    let toStringDefinition =
                        let toString =
                            let serializable =
                                fields
                                |> List.map (fun (pname, ptype) -> 
                                    let jsonConversion =
                                        let converted =
                                            if SafeReactPropType.needsConvert ptype then 
                                                let caseTypeName = sprintf "%s%sType" propTypeName (pname |> String.toPascalCase)
                                                application
                                                  [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["this"; pname |> String.toPascalCase])
                                                    SynExpr.CreateIdentString "|>" 
                                                    SynExpr.CreateLongIdent (LongIdentWithDots.Create [caseTypeName; "convert"]) ]
                                                |> SynExpr.CreateParen
                                            else
                                                SynExpr.CreateLongIdent( LongIdentWithDots.Create ["this"; pname |> String.toPascalCase] )

                                        converted
                                            
                                    ( Ident.Create pname, jsonConversion ))
                                |> anonRecord

                            application [ SynExpr.CreateIdentString "box"; serializable ]

                        functionPatternNoArgTypes "convert" ["this"]
                        |> memberBinding toString
                        |> SynMemberDefn.CreateStaticMember

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

        (parameters.PropertyTypeNames, parameters.PropertyTypes)
        ||> List.zip 
        |> List.choose (fun (ptname, ptype) -> 
            ptype.propType
            |> Option.bind (generatePropTypes ptname))
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
            List.zip4 parameters.DUSafePropertyNames parameters.PropertyNames parameters.PropertyTypes parameters.PropertyTypeNames
            |> List.choose (fun (psafe, pname, prop, ptname) -> 
                prop.propType
                |> Option.map (fun ptype ->
                    let propTypeName =
                        SafeReactPropType.tryGetFSharpTypeName ptype
                        |> Option.defaultValue ([ptname])
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
                      ( List.zip4 parameters.DUSafePropertyNames parameters.PropertyNames parameters.PropertyTypes parameters.PropertyTypeNames
                        |> List.map (fun (psafe, pname, prop, ptname) -> 
                            let pConvert =
                                if prop.propType |> Option.map SafeReactPropType.needsConvert = Some true then 
                                    application
                                      [ SynExpr.CreateIdentString "p"
                                        SynExpr.CreateIdentString "|>" 
                                        SynExpr.CreateLongIdent (LongIdentWithDots.Create [ptname; "convert"]) ]
                                else
                                    application [ SynExpr.CreateIdentString "box"; SynExpr.CreateIdentString "p" ]
                            simpleMatchClause psafe ["p"] None 
                              ( SynExpr.CreateTuple 
                                    [ SynExpr.CreateConstString pname
                                      pConvert ] )) ) )
            
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
            List.zip4 parameters.DUSafePropertyNames parameters.PropertyNames parameters.PropertyTypes parameters.PropertyTypeNames
            |> List.choose (fun (psafe, pname, prop, ptname) ->
                prop.propType
                |> Option.map (fun ptype ->
                    let propTypeName =
                        SafeReactPropType.tryGetFSharpTypeName ptype
                        |> Option.defaultValue ([ptname])

                    match ptype with 
                    | Union (_, Some utypes)
                    | FlowUnion (_, Some utypes) ->
                        utypes
                        |> List.indexed
                        |> List.map (fun (i, case) -> 
                            let caseTypeName = 
                                case 
                                |> SafeReactPropType.tryGetFSharpTypeName
                                |> Option.defaultValue ([sprintf "%sCase%dType" ptname i])

                            let caseName = 
                                caseTypeName 
                                |> List.rev
                                |> List.map String.toPascalCase
                                |> List.reduce (sprintf "%s%s")

                            functionPattern (pname |> String.toPascalCase |> String.decapitalize) [("p", appType caseTypeName)]
                            |> binding
                              ( application
                                  [ SynExpr.CreateIdentString "Prop"
                                    application
                                        [ SynExpr.CreateIdentString psafe
                                          application
                                              [ SynExpr.CreateLongIdent (LongIdentWithDots.Create [ ptname; caseName ])
                                                SynExpr.CreateIdentString "p"] |> SynExpr.CreateParen ] |> SynExpr.CreateParen ])
                            |> withXMLDocLet (prop |> generateComponentPropDescription |> toXMLDoc)
                            |> SynMemberDefn.CreateStaticMember)
                        
                    | _ ->
                        functionPattern (pname |> String.toPascalCase |> String.decapitalize) [("p", appType propTypeName)]
                        |> binding (application [ SynExpr.CreateIdentString "Prop"; application [SynExpr.CreateIdentString psafe; SynExpr.CreateIdentString "p"] |> SynExpr.CreateParen ])
                        |> withXMLDocLet (prop |> generateComponentPropDescription |> toXMLDoc)
                        |> SynMemberDefn.CreateStaticMember
                        |> List.singleton))
            |> List.concat

        //  static member children(value: int) = Children([ Html.text value ])
        //  static member children(value: string) = Children([ Html.text value ])
        //  static member children(value: float) = Children([ Html.text value ])
        //  static member children(value: System.Guid) = Children([ Html.text value ])
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
            [ createCon (appType ["int"]) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "text"]); SynExpr.CreateIdentString "value" ] ]) 
              createCon (appType ["string"]) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "text"]); SynExpr.CreateIdentString "value" ] ])
              createCon (appType ["float"]) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "text"]); SynExpr.CreateIdentString "value" ] ])
              createCon (SynType.CreateLongIdent (LongIdentWithDots.Create ["System"; "Guid"]) ) (expressionList [ application [ SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Html"; "text"]); SynExpr.CreateIdentString "value" ] ])
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
            //     ?bProp: BProp,
            //     ?cProp: bool
            // ) =
            memberFunctionPattern "applyMembers" 
                [ yield ("id", SynType.Create "string", true)
                  yield ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true) 
                  yield! 
                      List.zip3 parameters.PropertyNames parameters.PropertyTypes parameters.PropertyTypeNames
                      |> List.map (fun (prop, ptype, ptname) -> 
                          let camelCaseName = prop |> String.toPascalCase |> String.decapitalize
                          let propTypeName = 
                              ptype.propType
                              |> Option.bind SafeReactPropType.tryGetFSharpTypeName
                              |> Option.defaultValue ([ptname])
                          camelCaseName, appType propTypeName, false) ]
            |> binding  
              ( expressionSequence
                    [ // let props = DashComponentProps()
                      // DynObj.setValue props "id" id
                      // DynObj.setValue props "children" children
                      yield patternNamed "props" |> binding (application [SynExpr.CreateIdentString "DashComponentProps"; SynExpr.CreateUnit]) |> Let
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "id"; SynExpr.CreateIdentString "id"] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "children"; SynExpr.CreateIdentString "children"] |> Expression
                      
                      // DynObj.setValueOpt props "aProp" (aProp |> Option.map box)
                      // DynObj.setValueOpt props "bProp" (bProp |> Option.map BProp.convert)
                      // DynObj.setValueOpt props "cProp" (cProp |> Option.map box)
                      yield! 
                          List.zip3 parameters.PropertyNames parameters.PropertyTypes parameters.PropertyTypeNames
                          |> List.map (fun (prop, ptype, ptname) -> 
                              let camelCaseName = prop |> String.toPascalCase |> String.decapitalize
                              let pConvert =
                                  if ptype.propType |> Option.map SafeReactPropType.needsConvert = Some true then 
                                      application
                                        [ SynExpr.CreateIdentString camelCaseName
                                          SynExpr.CreateIdentString "|>" 
                                          SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Option"; "map"])
                                          SynExpr.CreateLongIdent (LongIdentWithDots.Create [ptname; "convert"]) ]
                                  else
                                      application 
                                        [ SynExpr.CreateIdentString camelCaseName
                                          SynExpr.CreateIdentString "|>"
                                          SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Option"; "map"])
                                          SynExpr.CreateIdentString "box" ]
                                  |> SynExpr.CreateParen

                              application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValueOpt"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString camelCaseName; pConvert] |> Expression)
                    
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
                  yield! 
                      List.zip3 parameters.PropertyNames parameters.PropertyTypes parameters.PropertyTypeNames
                      |> List.map (fun (prop, ptype, ptname) -> 
                          let camelCaseName = prop |> String.toPascalCase |> String.decapitalize
                          let propTypeName = 
                              ptype.propType
                              |> Option.bind SafeReactPropType.tryGetFSharpTypeName
                              |> Option.defaultValue ([ptname])
                          camelCaseName, appType propTypeName, false) ]
            |> binding
              ( application
                    [ SynExpr.CreateLongIdent (LongIdentWithDots.Create [parameters.ComponentName; "applyMembers"])
                      SynExpr.CreateParenedTuple 
                        [ yield SynExpr.CreateIdentString "id"
                          yield SynExpr.CreateIdentString "children"
                          yield! parameters.PropertyNames 
                                 |> List.map (fun prop -> 
                                    let camelCaseName = prop |> String.toPascalCase |> String.decapitalize
                                    application [SynExpr.CreateLongIdent (true, LongIdentWithDots.CreateString camelCaseName, None); SynExpr.CreateIdentString "="; SynExpr.CreateIdentString camelCaseName]) ]
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
    //  open DynamicObj
    //
    /// Define the component namespace
    let namespaceDeclaration =
        parameters.LibraryNamespace
        |> namespaceInfo
        |> withNamespaceDeclarations
            [ SynModuleDecl.CreateOpen "Dash.NET" 
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "Plotly.NET"
              SynModuleDecl.CreateOpen "DynamicObj"
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