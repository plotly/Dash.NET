module Dash.NET.ComponentGeneration.ASTGeneration

open System
open System.IO
open System.Collections.Generic
open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open Fantomas
open Humanizer
open Serilog
open Prelude
open ASTHelpers
open ComponentParameters
open ReactMetadata
open DocumentationGeneration

module Dict =
    let toList =
        Seq.map(|KeyValue|)
        >> Seq.toList

let private mkCaseTypeName =
    sprintf "%s%s"

let private toCaseTypeName propTypeName =
    SafeReactPropType.toName
    >> mkCaseTypeName propTypeName

let private mkFieldTypeNames propTypeName (pname: string) =
    SafeReactPropType.tryGetFSharpTypeName
    >> Option.defaultValue [ sprintf "%s%s" propTypeName (pname.Pascalize()) ]

let private mkMemberFunctionPatternArgs propTypeName (name: string, propType) =
    let fieldTypeName = propType |> mkFieldTypeNames propTypeName name |> appType
    let vprops = propType |> SafeReactPropType.getProps
    let isRequired = vprops.required |> Option.defaultValue false
    name.Camelize(), fieldTypeName, isRequired

let private defineDynObjSetValueExpression propTypeName valueName (prop: string, ptype) =
    let ptname = ptype |> mkFieldTypeNames propTypeName prop |> List.head
    let convertInfo, setInfo =
        let optionSuffix = "Opt"
        match SafeReactPropType.needsConvert ptype, (SafeReactPropType.getProps ptype).required with
        | true, Some true ->
            [
                SynExpr.CreateIdentString "|>" 
                SynExpr.CreateLongIdent (LongIdentWithDots.Create [ ptname; "convert" ])
            ]
            , ""
        | true, _ ->
            [
                SynExpr.CreateIdentString "|>" 
                SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Option"; "map"])
                SynExpr.CreateLongIdent (LongIdentWithDots.Create [ ptname; "convert" ])
            ]
            , optionSuffix
        | false, Some true -> [], ""
        | false, _ -> [], optionSuffix

    [
        yield SynExpr.CreateIdentString (prop.Camelize())
        yield! convertInfo
        yield SynExpr.CreateIdentString "|>" 
        yield SynExpr.CreateLongIdent (LongIdentWithDots.CreateString (sprintf "DynObj.setValue%s" setInfo))
        yield SynExpr.CreateIdentString valueName
        yield SynExpr.CreateConstString prop
    ]
    |> application
    |> Expression

let private defineStaticMethod name propTypeName methodName fields =
    let valueName = "t"
    fields
    |> List.map (mkMemberFunctionPatternArgs propTypeName)
    |> memberFunctionPattern methodName
    |> binding (
        [
            yield patternNamed valueName |> binding (application [SynExpr.CreateIdentString name; SynExpr.CreateUnit]) |> Let
            yield! fields |> List.map (defineDynObjSetValueExpression propTypeName valueName)
            yield SynExpr.CreateIdentString valueName |> Expression
        ]
        |> expressionSequence
    )
    |> SynMemberDefn.CreateStaticMember

let private defineDynamicObjType name propTypeName fields =
    name
    |> componentInfo
    |> typeDeclaration [
        SynMemberDefn.CreateImplicitCtor() //adds the "()" to the type name
        typeInherit (application [ SynExpr.CreateIdentString "DynamicObj"; SynExpr.CreateUnit ]) 
        fields |> defineStaticMethod name propTypeName "init"
      ]

let rec private generatePropTypes (name: string) (ptype: SafeReactPropType) =
    let propTypeName = name.Pascalize()

    let toCaseTypeName = toCaseTypeName propTypeName

    let mkRecursiveTypes mkTypeName =
        List.choose (fun (name: string, propType: SafeReactPropType) -> 
            generatePropTypes (mkTypeName propTypeName name) propType
        )

    let mkDiscriminatedUnion (propTypes: SafeReactPropType list) =
        let mkValuedUnion propType =
            let childCaseTypeName = propType |> toCaseTypeName
            let maybeChildRecursiveTypes = propType |> generatePropTypes childCaseTypeName
            let childCaseName = SafeReactPropType.toName propType
            let childTypeNameArgs = 
                propType 
                |> SafeReactPropType.tryGetFSharpTypeName
                |> Option.defaultValue [ childCaseTypeName ]
            let childCase = simpleUnionCase childCaseName [ anonAppField childTypeNameArgs ]
            let childConvertCase =
                let valueName = "v"
                let boxable =
                    if SafeReactPropType.needsConvert propType then 
                        application [
                            SynExpr.CreateIdentString valueName
                            SynExpr.CreateIdentString "|>" 
                            SynExpr.CreateLongIdent (LongIdentWithDots.Create [ childTypeNameArgs.[0]; "convert" ])
                        ]
                    else
                        SynExpr.CreateIdentString valueName

                simpleMatchClause childCaseName [valueName] None (
                    application [
                        boxable
                        SynExpr.CreateIdentString "|>" 
                        SynExpr.CreateIdentString "box" 
                    ]
                )

            (childCase, childConvertCase, maybeChildRecursiveTypes), (childCaseName, childTypeNameArgs)

        let mkUnvaluedUnion v =
            let childCaseName = v |> String.toValidDULabel
            let childCase = simpleUnionCase childCaseName []
            let childConvertCase = 
                simpleMatchClause childCaseName [] None (
                    application [
                        SynExpr.CreateIdentString "box" 
                        SynExpr.CreateConstString v
                    ]
                )

            (childCase, childConvertCase, None), (childCaseName, [])

        let childDetails =
            propTypes
            |> List.choose (fun propType ->
                match propType with
                | Array (_, Some v)
                | String (_, Some v)
                | Object (_, Some v)
                | Any (_, Some v)
                | Element (_, Some v)
                | Node (_, Some v) -> v |> mkUnvaluedUnion |> Some
                | Bool (_, Some v) -> v |> string |> mkUnvaluedUnion |> Some
                | Number (_, Some v) -> v |> string |> mkUnvaluedUnion |> Some
                | Array (_, None)
                | Bool (_, None)
                | Number (_, None)
                | String (_, None)
                | Object (_, None)
                | Any (_, None)
                | Element (_, None)
                | Node (_, None)
                | Enum (_, _)
                | Union (_, _)
                | ArrayOf (_, _)
                | ObjectOf (_, _)
                | Shape (_, _)
                | Exact (_, _)
                | FlowUnion (_, _)
                | FlowArray (_, _)
                | FlowObject (_, _) -> propType |> mkValuedUnion |> Some
                | Other (_, _, _) -> None
            )
            |> List.distinctBy (snd >> fun (childCaseName: string, _) -> childCaseName.ToLowerInvariant())

        match childDetails with
        | [] ->
            Log.Warning("Property {PropertyName} contained no cases, a type could not be created", name)
            None
        | children ->
            let childCases, childCaseValues, maybeChildRecursiveTypes, nameDetails =
                children
                |> List.unzip
                |> fun (typeDetails, nameDetails) ->
                    let childCases, childCaseValues, maybeChildRecursiveTypes = typeDetails |> List.unzip3
                    childCases, childCaseValues, maybeChildRecursiveTypes, nameDetails

            let childRecursiveTypesDefinition =
                maybeChildRecursiveTypes
                |> List.choose id
                |> List.map fst
                |> List.concat

            let childConvertMethodDefinition =
                let valueName = "t"
                functionPatternNoArgTypes "convert" [ valueName ]
                |> memberBinding  
                    ( SynExpr.CreateIdentString valueName
                    |> matchStatement childCaseValues )
                |> SynMemberDefn.CreateStaticMember

            let childUnionTypeDefinition =
                propTypeName
                |> componentInfo
                |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
                |> simpleTypeDeclaration (childCases |> unionDefinition) [ childConvertMethodDefinition ]

            Some (childUnionTypeDefinition :: childRecursiveTypesDefinition, nameDetails)

    let mkAlias (utype: SafeReactPropType) =
        let valueName = "p"

        let recursiveTypes = 
            generatePropTypes (sprintf "%sValue" propTypeName) utype
            |> Option.map fst
            |> Option.defaultValue []
        
        let childTypeNameArgs = 
            utype 
            |> SafeReactPropType.tryGetFSharpTypeName
            |> Option.defaultValue ([sprintf "%sValue" propTypeName])

        let caseValue =
            let boxable =
                if SafeReactPropType.needsConvert utype then 
                    application
                      [ SynExpr.CreateIdentString valueName
                        SynExpr.CreateIdentString "|>" 
                        SynExpr.CreateLongIdent (LongIdentWithDots.Create [ childTypeNameArgs.[0]; "convert" ]) ]
                    |> SynExpr.CreateParen
                else
                    SynExpr.CreateIdentString valueName

            let valueConvert =
                application [ SynExpr.CreateIdentString "box"; boxable ]
                |> simpleLambdaStatement true [ valueName ]
                |> simpleLambdaStatement false ["k"]
                |> SynExpr.CreateParen

            let convertDict =
                application [
                    SynExpr.CreateIdentString "v"
                    SynExpr.CreateIdentString "|>"
                    application [
                        SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Map"; "map"]) 
                        valueConvert
                      ]
                ]

            simpleMatchClause propTypeName ["v"] None convertDict

        let toCaseValueDefinition =
            functionPatternNoArgTypes "convert" ["this"]
            |> memberBinding (
                application [
                    SynExpr.CreateIdentString "box"
                    SynExpr.CreateIdentString "this" |> matchStatement [ caseValue ] |> SynExpr.CreateParen
                ]
            )
            |> SynMemberDefn.CreateStaticMember

        let aliasDefinition =
            propTypeName
            |> componentInfo
            |> withXMLDoc (ptype |> generatePropDocumentation |> Option.defaultValue [] |> toXMLDoc)
            |> simpleTypeDeclaration (
                SynType.CreateApp (SynType.Create "Map", [SynType.Create "string"; appType childTypeNameArgs]) 
                |> anonTypeField 
                |> List.singleton
                |> simpleUnionCase propTypeName
                |> List.singleton
                |> unionDefinition
            ) [ toCaseValueDefinition ]

        Some (aliasDefinition :: recursiveTypes, [ propTypeName, childTypeNameArgs ])

    let mkDynamicObj (values: IDictionary<string, SafeReactPropType>) =
        if values.Count = 0 then
            Log.Error("Object, shape or exact property {PropertyName} contained no properties, a type could not be created", name)
            None
        else
            let fields = values |> Dict.toList
            let dynObjDef = fields |> defineDynamicObjType name propTypeName
            let mkTypeName typeName (propName: string) = sprintf "%s%s" typeName (propName.Pascalize())
            let recursiveTypes =
                fields
                |> mkRecursiveTypes mkTypeName
                |> List.map fst
                |> List.concat

            Some (dynObjDef::recursiveTypes, [ name, [ propTypeName ] ] )

    let mkDynamicObjs (values: IDictionary<string, SafeReactPropType>) =
        if values.Count = 0 then
            Log.Error("Object, shape or exact property {PropertyName} contained no properties, a type could not be created", name)
            None
        else
            let recursiveTypes =
                values
                |> Dict.toList
                |> mkRecursiveTypes (fun _ propName -> propName)
                |> List.map fst
                |> List.concat
            Some (recursiveTypes, [ name, [] ])

    match ptype with
    | Enum (_, Some utypes)
    | Union (_, Some utypes) 
    | FlowUnion (_, Some utypes) -> mkDiscriminatedUnion utypes
    | ArrayOf (_, Some utype) 
    | FlowArray (_, Some utype) -> [ name, utype ] |> dict |> mkDynamicObjs
    | ObjectOf (_, Some utype) -> mkAlias utype
    | Shape (_, Some values) 
    | Exact (_, Some values) 
    | FlowObject (_, Some values) -> mkDynamicObj values
    | _ -> None

let createComponentAST (parameters: ComponentParameters) =
    Log.Information("Creating component bindings")

    let typesDeclaration =
        parameters.Properties
        |> List.choose (fun paramProp -> paramProp.Info.propType |> Option.bind (generatePropTypes paramProp.TypeName))
        |> List.map fst
        |> List.concat
        |> List.rev

    let mkPropertyDuDeclaration () =
        let duCasesDefinition =
            let props = parameters.Properties
            props
            |> List.choose (fun paramProp -> 
                let ll =
                    paramProp
                    |> ComponentParametersProperty.tryTypeNameArgs
                    |> Option.map (fun propTypeName -> simpleUnionCase paramProp.CaseName [anonAppField propTypeName])
                ll
            )
            |> unionDefinition

        let toDynamicMemberDefMethodDefinition =
            functionPattern "toDynamicMemberDef" [("prop", SynType.Create parameters.ComponentPropsName)]
            |> binding (
                "prop"
                |> SynExpr.CreateIdentString
                |> matchStatement (
                    parameters.Properties
                    |> List.map (fun paramProp ->
                        let valueName = "p"
                        let pConvert =
                            match paramProp |> ComponentParametersProperty.valueConvArgs with
                            | [ arg ] -> [ SynExpr.CreateIdentString arg; SynExpr.CreateIdentString valueName ]
                            | args ->
                                [
                                    SynExpr.CreateIdentString valueName
                                    SynExpr.CreateIdentString "|>" 
                                    SynExpr.CreateLongIdent (LongIdentWithDots.Create args)
                                ]
                            |> application
                        simpleMatchClause paramProp.CaseName [ valueName ] None (
                            SynExpr.CreateTuple [
                                SynExpr.CreateConstString paramProp.Name
                                pConvert
                            ]
                        )
                    )
                )
            )
            |> SynMemberDefn.CreateStaticMember

        parameters.ComponentPropsName
        |> componentInfo
        |> withXMLDoc (parameters.Metadata |> generateComponentPropsDocumentation false |> toXMLDoc)
        |> simpleTypeDeclaration 
            duCasesDefinition
            [ toDynamicMemberDefMethodDefinition ]

    let mkAttributeDuDeclaration () =
        let duCasesDefinition =
            [
                simpleUnionCase "Prop" [ anonSimpleField parameters.ComponentPropsName ]
                simpleUnionCase "Children" [ anonAppField [ "list"; "DashComponent" ] ]
            ]
            |> unionDefinition

        let constructorsDeclarations =
            let valueName = "p"
            parameters.Properties
            |> List.choose (fun paramProp ->
                let toCaseTypeName = toCaseTypeName paramProp.Name
                let setFunctionName = paramProp |> ComponentParametersProperty.mkSetFunctionName
                paramProp.Info.propType
                |> Option.map (fun ptype ->
                    match ptype with 
                    | Union (_, Some _)
                    | FlowUnion (_, Some _) ->
                        ptype
                        |> generatePropTypes paramProp.Name
                        |> Option.map (snd >> fun xs ->
                                xs
                                |> List.map (fun (innerCaseName, innerPropTypeName) ->
                                    functionPattern setFunctionName [ valueName, appType innerPropTypeName ]
                                    |> binding (
                                        application [
                                            SynExpr.CreateIdentString "Prop"
                                            application [
                                                SynExpr.CreateIdentString paramProp.CaseName
                                                application [
                                                    SynExpr.CreateLongIdent (LongIdentWithDots.Create [ paramProp.TypeName; innerCaseName ])
                                                    SynExpr.CreateIdentString valueName
                                                ] |> SynExpr.CreateParen
                                            ] |> SynExpr.CreateParen
                                          ]
                                      )
                                    |> withXMLDocLet (paramProp.Info |> generateComponentPropDescription |> toXMLDoc)
                                    |> SynMemberDefn.CreateStaticMember


                                )
                        )
                        |> Option.defaultValue []

                    | _ ->
                        let propTypeName = ptype |> ComponentParametersProperty.typeNameArgs paramProp
                        functionPattern setFunctionName [ valueName, appType propTypeName ]
                        |> binding (
                            application [
                                SynExpr.CreateIdentString "Prop"
                                application [
                                    SynExpr.CreateIdentString paramProp.CaseName
                                    SynExpr.CreateIdentString valueName
                                ] |> SynExpr.CreateParen
                            ]
                        )
                        |> withXMLDocLet (paramProp.Info |> generateComponentPropDescription |> toXMLDoc)
                        |> SynMemberDefn.CreateStaticMember
                        |> List.singleton
                )
            )
            |> List.concat

        let childrenConstructorDeclarations =
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

        parameters.ComponentAttrsName
        |> componentInfo
        |> withXMLDoc (["A list of children or a property for this dash component"] |> toXMLDoc)
        |> simpleTypeDeclaration 
            duCasesDefinition
            [ yield! constructorsDeclarations
              yield! childrenConstructorDeclarations ] 

    let mkTypeDeclaration () =
        let applyMembersStaticMethodDefinition =
            let valueName = "t"
            memberFunctionPattern "applyMembers" 
                [ yield ("id", SynType.Create "string", true)
                  yield ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true) 
                  yield! 
                      parameters.Properties
                      |> List.map (fun paramProp ->
                          let camelCaseName = paramProp.Name.Camelize()
                          let propTypeName = 
                              paramProp.Info.propType
                              |> Option.bind SafeReactPropType.tryGetFSharpTypeName
                              |> Option.defaultValue ([paramProp.TypeName])
                          camelCaseName, appType propTypeName, false) ]
            |> binding (
                expressionSequence [
                    yield
                        patternNamed "props"
                        |> binding (
                            application [
                                SynExpr.CreateIdentString "DashComponentProps"
                                SynExpr.CreateUnit
                            ]
                        ) |> Let

                    yield
                        application [
                            SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")
                            SynExpr.CreateIdentString "props"
                            SynExpr.CreateConstString "id"
                            SynExpr.CreateIdentString "id"
                        ] |> Expression

                    yield
                        application [
                            SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")
                            SynExpr.CreateIdentString "props"
                            SynExpr.CreateConstString "children"
                            SynExpr.CreateIdentString "children"
                        ] |> Expression

                    yield! 
                        parameters.Properties
                        |> List.map (fun paramProp ->
                            let prop = paramProp.Name
                            let ptype = paramProp.Info
                            let ptname = paramProp.TypeName
                            let camelCaseName = prop |> String.toPascalCase |> String.decapitalize
                            let pConvert =
                                if ptype.propType |> Option.map SafeReactPropType.needsConvert = Some true then 
                                    application
                                      [ SynExpr.CreateIdentString camelCaseName
                                        SynExpr.CreateIdentString "|>" 
                                        SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Option"; "map"])
                                        SynExpr.CreateLongIdent (LongIdentWithDots.Create [ ptname; "convert" ]) ]
                                else
                                    application 
                                      [ SynExpr.CreateIdentString camelCaseName
                                        SynExpr.CreateIdentString "|>"
                                        SynExpr.CreateLongIdent (LongIdentWithDots.Create ["Option"; "map"])
                                        SynExpr.CreateIdentString "box" ]
                                |> SynExpr.CreateParen

                            application [
                                SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValueOpt")
                                SynExpr.CreateIdentString "props"
                                SynExpr.CreateConstString camelCaseName
                                pConvert
                            ] |> Expression
                        )
                    
                    yield
                        application [
                            SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")
                            SynExpr.CreateIdentString valueName
                            SynExpr.CreateConstString "namespace"
                            SynExpr.CreateConstString parameters.ComponentNamespace
                        ] |> Expression
                    yield
                        application [
                            SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")
                            SynExpr.CreateIdentString valueName
                            SynExpr.CreateConstString "props"
                            SynExpr.CreateIdentString "props"
                        ] |> Expression
                    yield
                        application [
                            SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")
                            SynExpr.CreateIdentString valueName
                            SynExpr.CreateConstString "type"
                            SynExpr.CreateConstString parameters.ComponentType
                        ] |> Expression
                    
                    yield SynExpr.CreateIdentString valueName |> Expression
                ]
                |> typedLambdaStatement false [ valueName, SynType.Create parameters.ComponentName ]
                |> SynExpr.CreateParen
            )
            
            |> SynMemberDefn.CreateStaticMember

        let initStaticMethodDefinition =
            memberFunctionPattern "init" [
                ("id", SynType.Create "string", true)
                ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true)
                yield! 
                    parameters.Properties
                    |> List.map (fun paramProp ->
                        let camelCaseName = paramProp.Name.Camelize()
                        let propTypeName = 
                            paramProp.Info.propType
                            |> Option.bind SafeReactPropType.tryGetFSharpTypeName
                            |> Option.defaultValue ([paramProp.TypeName])
                        camelCaseName, appType propTypeName, false)
            ]
            |> binding (
                application [
                    SynExpr.CreateLongIdent (LongIdentWithDots.Create [parameters.ComponentName; "applyMembers"])
                    SynExpr.CreateParenedTuple [
                        yield SynExpr.CreateIdentString "id"
                        yield SynExpr.CreateIdentString "children"
                        yield! parameters.Properties
                               |> List.map (fun paramProp -> 
                                  let camelCaseName = paramProp.Name.Camelize()
                                  application [
                                    SynExpr.CreateLongIdent (true, LongIdentWithDots.CreateString camelCaseName, None)
                                    SynExpr.CreateIdentString "="
                                    SynExpr.CreateIdentString camelCaseName
                                  ]
                               )
                    ]
                    application [
                        SynExpr.CreateIdentString parameters.ComponentName
                        SynExpr.CreateUnit
                    ] |> SynExpr.CreateParen
                ]
            )
            |> SynMemberDefn.CreateStaticMember

        let defineIncludedJavaScriptFiles =
            parameters.ComponentJavascript
            |> List.map String.escape
            |> List.map SynExpr.CreateConstString
            |> expressionList

        let definitionStaticMethodDefinitioin =
            patternNamed "definition"
            |> withPatternType (SynType.Create "LoadableComponentDefinition")
            |> binding (
                SynExpr.CreateRecord [
                    (LongIdentWithDots.CreateString "ComponentName",true), Some (SynExpr.CreateConstString parameters.ComponentName)
                    (LongIdentWithDots.CreateString "ComponentJavascript",true), Some defineIncludedJavaScriptFiles
                ]
            )
            |> SynMemberDefn.CreateStaticMember

        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc (parameters.Metadata |> generateComponentDescription |> toXMLDoc)
        |> typeDeclaration [
            SynMemberDefn.CreateImplicitCtor() //adds the "()" to the type name
            typeInherit (application [SynExpr.CreateIdentString "DashComponent"; SynExpr.CreateUnit]) 
            applyMembersStaticMethodDefinition
            initStaticMethodDefinition
            definitionStaticMethodDefinitioin
        ]

    let mkLetDeclaration () =
        let innerExpressionDefinition =
            expressionSequence [
                patternNamedTuple ["props"; "children"]
                |> binding (
                    application [
                        SynExpr.CreateLongIdent (LongIdentWithDots.Create ["List"; "fold"])
                        SynExpr.CreateIdentString "a"
                        |> matchStatement [
                            simpleMatchClause "Prop" ["prop"] None (
                                SynExpr.CreateTuple [
                                    application [
                                        SynExpr.CreateIdentString "prop"
                                        SynExpr.CreateIdentString "::"
                                        SynExpr.CreateIdentString "props"
                                    ]
                                    SynExpr.CreateIdentString "children"
                                ]
                            )
                            simpleMatchClause "Children" ["child"] None (
                                SynExpr.CreateTuple [
                                    SynExpr.CreateIdentString "props"
                                    application [
                                        SynExpr.CreateIdentString "child"
                                        SynExpr.CreateIdentString "@"
                                        SynExpr.CreateIdentString "children"
                                    ]
                                ]
                            )
                        ]
                        |> typedLambdaStatement true [ "a", SynType.Create parameters.ComponentAttrsName ]
                        |> simpleLambdaStatement false [ "props"; "children" ]
                        |> SynExpr.CreateParen

                        SynExpr.CreateTuple [expressionList []; expressionList []] |> SynExpr.CreateParen
                        SynExpr.CreateIdentString "attrs"
                    ]
                ) |> Let 
              
                patternNamed "t" 
                |> binding (
                    application [
                        SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentName; "init"])
                        SynExpr.CreateParenedTuple [SynExpr.CreateIdentString "id"; SynExpr.CreateIdentString "children" ]
                    ]
                ) |> Let
                
                patternNamed "componentProps"
                |> binding (
                    SynExpr.CreateInstanceMethodCall(
                        LongIdentWithDots.CreateString "t.TryGetTypedValue"
                        , [SynType.Create "DashComponentProps"]
                        , SynExpr.CreateConstString "props"
                    )
                    |> matchStatement [
                        simpleMatchClause "Some" ["p"] None (SynExpr.CreateIdentString "p") 
                        simpleMatchClause "None" [] None (application [SynExpr.CreateIdentString "DashComponentProps"; SynExpr.CreateUnit])
                    ]
                ) |> Let 
                
                application [
                    SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "Seq.iter")
                    expressionSequence [
                        patternNamedTuple ["fieldName"; "boxedProp"]
                        |> binding (
                            application [
                                SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentPropsName; "toDynamicMemberDef"])
                                SynExpr.CreateIdentString "prop"
                            ]
                        ) |> Let
                        application [
                            SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue")
                            SynExpr.CreateIdentString "componentProps"
                            SynExpr.CreateIdentString "fieldName"
                            SynExpr.CreateIdentString "boxedProp"
                        ] |> Expression
                    ]
                    |> typedLambdaStatement false [ "prop", SynType.Create parameters.ComponentPropsName ]
                    |> SynExpr.CreateParen
                    SynExpr.CreateIdentString "props"
                ] |> Expression
                
                application [
                    SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue")
                    SynExpr.CreateIdentString "t"
                    SynExpr.CreateConstString "props"
                    SynExpr.CreateIdentString "componentProps"
                ] |> Expression 
                
                SynExpr.CreateIdentString "t" |> expressionUpcast (SynType.Create "DashComponent") |> Expression
            ]

        functionPattern parameters.CamelCaseComponentName
            [ ("id", SynType.Create "string")
              ("attrs", SynType.CreateApp(SynType.Create "list", [SynType.Create parameters.ComponentAttrsName])) ]
        |> binding innerExpressionDefinition
        |> withXMLDocLet (parameters.Metadata |> generateComponentDocumentation |> toXMLDoc)
        |> letDeclaration

    let moduleContentDeclaration = 
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc (parameters.Metadata |> generateComponentDescription |> toXMLDoc)
        |> if not parameters.IsHelper then withModuleAttribute (SynAttribute.Create "RequireQualifiedAccess")
           else id
        |> nestedModule [
            yield! typesDeclaration
            if not parameters.IsHelper then
                if not <| String.IsNullOrWhiteSpace parameters.ComponentPropsName then
                    yield mkPropertyDuDeclaration ()
                    yield mkAttributeDuDeclaration ()
                yield mkTypeDeclaration ()
                yield mkLetDeclaration ()
        ]

    let moduleDeclaration =
        parameters.LibraryNamespace
        |> namespaceInfo
        |> withNamespaceDeclarations [
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "DynamicObj"
              SynModuleDecl.CreateOpen "Dash.NET" 
              if not parameters.IsHelper then SynModuleDecl.CreateOpen "Common" 
              moduleContentDeclaration ] 

    ParsedImplFileInputRcd
        .CreateFs(parameters.ComponentName)
        .AddModule(moduleDeclaration)
    |> ParsedInput.CreateImplFile

let generateCodeStringFromAST (path: string) ast =
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

let generateCodeFromAST (path: string) ast =
    async {
        let! formattedCode = generateCodeStringFromAST path ast
        try
            File.WriteAllText(path,formattedCode)
            Log.Debug("Created file {ComponentFSharpFile}",path)
            return true
        with | ex -> 
            Log.Error(ex, "Failed to write file {ComponentFSharpFile}",path)
            return false
    }
            