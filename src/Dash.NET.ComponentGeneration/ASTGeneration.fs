module Dash.NET.ComponentGeneration.ASTGeneration

open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open Fantomas
open System
open System.IO
open FSharp.Compiler.XmlDoc
open ASTHelpers
open ComponentParameters

//TODO: Clean up this file
//TODO: Add documentation

//I was learning AST generation as I went
//As a result this file is a bit of an inconsistent mess at the moment
//This will be fixed

let createComponentAST (parameters: ComponentParameters) =

    printfn "Creating component bindings"

    ///Define the component property descriminated union
    let componentPropertyDUDeclaration =

        ///Define the cases for the descriminated union
        let componentPropertyDUCases =
            parameters.DUSafePropertyNames
            |> List.map (fun prop -> simpleUnionCase prop [anonUnionField "string"])
            |> unionDefinition

        ///Define the static method "toMemberDef"
        let componentPropertyToDynamicMemberDefDeclaration =
            functionPattern "toDynamicMemberDef" [("prop", SynType.Create parameters.ComponentPropsName)]
            |> binding  
              ( SynExpr.CreateIdentString "prop"
                    |> matchStatement 
                      ( (parameters.DUSafePropertyNames, parameters.PropertyNames)
                        ||> List.zip
                        |> List.map (fun (duprop, prop) -> 
                            simpleMatchClause duprop ["p"] None 
                              ( SynExpr.CreateTuple 
                                    [ SynExpr.CreateConstString prop
                                      application [SynExpr.CreateIdentString "box"; SynExpr.CreateIdentString "p"] ])) ) )
            
            |> SynMemberDefn.CreateStaticMember

        //Create the type definition
        parameters.ComponentPropsName
        |> componentInfo
        |> withXMLDoc "This is test documentation" //TODO documentation comments
        |> simpleTypeDeclaration 
            componentPropertyDUCases
            [ componentPropertyToDynamicMemberDefDeclaration ] 

    ///Define the component class
    let componentTypeDeclaration =
        
        ///Define the static method "applyMembers"
        let componentTypeApplyMembersDeclaration =
            memberFunctionPattern "applyMembers" 
                [ yield ("id", SynType.Create "string", true)
                  yield ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true) 
                  yield! parameters.PropertyNames |> List.map (fun prop -> prop, SynType.Create "string", false) ]
            |> binding  
              ( expressionSequence
                    [ yield patternNamed "props" |> binding (application [SynExpr.CreateIdentString "DashComponentProps"; SynExpr.CreateUnit]) |> Let
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "id"; SynExpr.CreateIdentString "id"] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "children"; SynExpr.CreateIdentString "children"] |> Expression
                      
                      yield! parameters.PropertyNames 
                             |> List.map (fun prop -> 
                                application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValueOpt"); SynExpr.CreateIdentString "props"; SynExpr.CreateConstString prop; SynExpr.CreateIdentString prop] |> Expression)


                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "namespace"; SynExpr.CreateConstString parameters.ComponentNamespace] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "props"; SynExpr.CreateIdentString "props"] |> Expression
                      yield application [SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "type"; SynExpr.CreateConstString parameters.ComponentType] |> Expression
                      
                      yield SynExpr.CreateIdentString "t" |> Expression ]

                |> simpleLambdaStatement [("t", SynType.Create parameters.ComponentName)]
                |> SynExpr.CreateParen )
            
            |> SynMemberDefn.CreateStaticMember

        ///Define the static method "init"
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

        ///Define the static member "definition"
        let componentTypeDefinitionDeclaration =
            patternNamed "definition"
            |> withPatternType (SynType.Create "LoadableComponentDefinition")
            |> binding
              ( SynExpr.CreateRecord 
                    [ ((LongIdentWithDots.CreateString "ComponentName",true), Some (SynExpr.CreateConstString parameters.ComponentName)) 
                      ((LongIdentWithDots.CreateString "ComponentJavascript",true), Some (SynExpr.CreateConstString parameters.ComponentJavascript)) ] )

            |> SynMemberDefn.CreateStaticMember

        //Create the type definition
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc "This is additional test documentation" //TODO documentation comments
        |> typeDeclaration 
          [ SynMemberDefn.CreateImplicitCtor()
            typeInherit (application [SynExpr.CreateIdentString "DashComponent"; SynExpr.CreateUnit]) 
            componentTypeApplyMembersDeclaration
            componentTypeInitDeclaration
            componentTypeDefinitionDeclaration ]

    ///Define the component DSL function (used when creating the DOM tree)
    let componentLetDeclaration =

        ///Define the inner expression
        let componentDeclaration =
            expressionSequence
              [ patternNamed "t" |> binding (application [SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentName; "init"]); SynExpr.CreateParenedTuple [SynExpr.CreateIdentString "id"; SynExpr.CreateIdentString "children"]]) |> Let
                patternNamed "componentProps" |> binding
                  ( SynExpr.CreateInstanceMethodCall(LongIdentWithDots.CreateString "t.TryGetTypedValue", [SynType.Create "DashComponentProps"], SynExpr.CreateConstString "props")
                    |> matchStatement
                        [ simpleMatchClause "Some" ["p"] None (SynExpr.CreateIdentString "p") 
                          simpleMatchClause "None" [] None (application [SynExpr.CreateIdentString "DashComponentProps"; SynExpr.CreateUnit]) ]) |> Let 
                application
                  [ SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "Seq.iter")
                    expressionSequence
                      [ patternNamedTuple ["fieldName"; "boxedProp"] |> binding (application [SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentPropsName; "toDynamicMemberDef"]); SynExpr.CreateIdentString "prop"]) |> Let
                        application [SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "componentProps"; SynExpr.CreateIdentString "fieldName"; SynExpr.CreateIdentString "boxedProp" ] |> Expression ]
                    |> simpleLambdaStatement [("prop", SynType.Create parameters.ComponentPropsName)]
                    |> SynExpr.CreateParen
                    SynExpr.CreateIdentString "props" ] |> Expression
                application [SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue"); SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "props"; SynExpr.CreateIdentString "componentProps" ] |> Expression 
                SynExpr.CreateIdentString "t" |> expressionUpcast (SynType.Create "DashComponent") |> Expression ]

        //Create the binding
        functionPattern parameters.CamelCaseComponentName
            [ ("id", SynType.Create "string")
              ("props", SynType.CreateApp(SynType.Create "seq", [SynType.Create parameters.ComponentPropsName]))
              ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"])) ]
        |> binding componentDeclaration
        |> withXMLDocLet "This is additional test documentation" //TODO documentation comments
        |> letDeclaration

    ///Define the component module
    let moduleDeclaration = 
        parameters.ComponentName
        |> componentInfo
        |> withXMLDoc "This is additional test documentation" //TODO documentation comments
        |> withModuleAttribute (SynAttribute.Create "RequireQualifiedAccess")
        |> nestedModule
            [ componentPropertyDUDeclaration 
              componentTypeDeclaration
              componentLetDeclaration ]

    ///Define the component namespace
    let namespaceDeclaration =
        parameters.LibraryNamespace
        |> namespaceInfo
        |> withNamespaceDeclarations
            [ SynModuleDecl.CreateOpen "Dash.NET"
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "Plotly.NET"
              moduleDeclaration ] 

    //Create the file
    ParsedImplFileInputRcd
        .CreateFs(parameters.ComponentName)
        .AddModule(namespaceDeclaration)
    |> ParsedInput.CreateImplFile

let generateCodeFromAST (path: string) ast =
    async {
        let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true }
        let! formattedCode = CodeFormatter.FormatASTAsync(ast, Path.GetFileName path, [], None, cfg)

        let formattedCode =
            [ "//------------------------------------------------------------------------------" //TODO documentation comments
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
            