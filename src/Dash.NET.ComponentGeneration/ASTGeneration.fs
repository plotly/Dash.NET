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
    let componentPropertyDUCases =
        parameters.DUSafePropertyNames
        |> List.map (fun prop -> simpleUnionCase prop [anonUnionField "string"])
        |> unionDefinition

    let componentPropertyToDynamicMemberDefDeclaration =
        { SynBindingRcd.Null with //TODO replace with binding
            Pattern = functionPattern "toDynamicMemberDef" [("prop", SynType.Create parameters.ComponentPropsName)]
            Expr = SynExpr.CreateIdentString "prop"
                   |> matchStatement 
                     ( (parameters.DUSafePropertyNames, parameters.PropertyNames)
                       ||> List.zip
                       |> List.map (fun (duprop, prop) -> 
                           simpleMatchClause duprop ["p"] None 
                             ( SynExpr.CreateTuple 
                                   [ SynExpr.CreateConstString prop
                                     SynExpr.CreateApp (SynExpr.CreateIdentString "box", SynExpr.CreateIdentString "p") ]) ) )
        }
        |> SynMemberDefn.CreateStaticMember

    let componentPropertyDUDeclaration =
        parameters.ComponentPropsName
        |> Ident.CreateLong
        |> SynComponentInfoRcd.Create
        |> withXMLDoc "This is test documentation" //TODO documentation comments
        |> simpleTypeDefinition 
            componentPropertyDUCases
            [ componentPropertyToDynamicMemberDefDeclaration ]

    let componentTypeApplyMembersDeclaration =
        { SynBindingRcd.Null with //TODO replace with binding
            Pattern = memberFunctionPattern "applyMembers" 
                [ yield ("id", SynType.Create "string", true)
                  yield ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true) 
                  yield! parameters.PropertyNames |> List.map (fun prop -> prop, SynType.Create "string", false) ]
                

            Expr = 
                expressionSequence
                    [ yield patternNamed "props" |> binding (SynExpr.CreateApp(SynExpr.CreateIdentString "DashComponentProps", SynExpr.CreateUnit)) |> Let
                      yield applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")) [SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "id"; SynExpr.CreateIdentString "id"] |> Expression
                      yield applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")) [SynExpr.CreateIdentString "props"; SynExpr.CreateConstString "children"; SynExpr.CreateIdentString "children"] |> Expression
                      
                      yield! parameters.PropertyNames 
                             |> List.map (fun prop -> 
                                applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValueOpt")) [SynExpr.CreateIdentString "props"; SynExpr.CreateConstString prop; SynExpr.CreateIdentString prop] |> Expression)


                      yield applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")) [SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "namespace"; SynExpr.CreateConstString parameters.ComponentNamespace] |> Expression
                      yield applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")) [SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "props"; SynExpr.CreateIdentString "props"] |> Expression
                      yield applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.CreateString "DynObj.setValue")) [SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "type"; SynExpr.CreateConstString parameters.ComponentType] |> Expression
                      
                      yield SynExpr.CreateIdentString "t" |> Expression ]

                |> simpleLambdaStatement [("t", SynType.Create parameters.ComponentName)]
                |> SynExpr.CreateParen  
        }
        |> SynMemberDefn.CreateStaticMember

    let componentTypeInitDeclaration =
        { SynBindingRcd.Null with
            Pattern = memberFunctionPattern "init" 
                [ ("id", SynType.Create "string", true)
                  ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"]), true)
                  yield! parameters.PropertyNames |> List.map (fun prop -> prop, SynType.Create "string", false) ]

            Expr = 
                applicationMany (SynExpr.CreateLongIdent (LongIdentWithDots.Create [parameters.ComponentName; "applyMembers"]))
                    [ SynExpr.CreateParenedTuple 
                        [ yield SynExpr.CreateIdentString "id"
                          yield SynExpr.CreateIdentString "children"
                          yield! parameters.PropertyNames 
                                 |> List.map (fun prop -> 
                                    applicationMany (SynExpr.CreateLongIdent (true, LongIdentWithDots.CreateString prop, None)) [SynExpr.CreateIdentString "="; SynExpr.CreateIdentString prop]) ]
                      SynExpr.CreateApp (SynExpr.CreateIdentString parameters.ComponentName, SynExpr.CreateUnit) |> SynExpr.CreateParen]
        }
        |> SynMemberDefn.CreateStaticMember

    let componentTypeMemberDeclarations =
        [ SynMemberDefn.CreateImplicitCtor()
          typeInherit (SynExpr.CreateApp (SynExpr.CreateIdentString "DashComponent", SynExpr.CreateUnit)) 
          componentTypeApplyMembersDeclaration
          componentTypeInitDeclaration ]

    let componentTypeDeclaration =
        parameters.ComponentName
        |> Ident.CreateLong 
        |> SynComponentInfoRcd.Create
        |> withXMLDoc "This is additional test documentation" //TODO documentation comments
        |> typeDefinition componentTypeMemberDeclarations

    let componentDeclaration =
        expressionSequence
          [ applicationMany (SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "ComponentLoader.loadComponent"))
                [ SynExpr.CreateRecord 
                    [ ((LongIdentWithDots.CreateString "ComponentName",true), Some (SynExpr.CreateConstString parameters.ComponentName)) 
                      ((LongIdentWithDots.CreateString "ComponentJavascript",true), Some (SynExpr.CreateConstString parameters.ComponentJavascript)) ] ] |> Expression
            patternNamed "t" |> binding (SynExpr.CreateApp(SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentName; "init"]), SynExpr.CreateParenedTuple [SynExpr.CreateIdentString "id"; SynExpr.CreateIdentString "children"])) |> Let
            patternNamed "componentProps" |> binding
              ( SynExpr.CreateInstanceMethodCall(LongIdentWithDots.CreateString "t.TryGetTypedValue", [SynType.Create "DashComponentProps"], SynExpr.CreateConstString "props")
                |> matchStatement
                    [ simpleMatchClause "Some" ["p"] None (SynExpr.CreateIdentString "p") 
                      simpleMatchClause "None" [] None (SynExpr.CreateApp(SynExpr.CreateIdentString "DashComponentProps", SynExpr.CreateUnit)) ]) |> Let 
            applicationMany (SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "Seq.iter"))
              [ expressionSequence
                  [ patternNamedTuple ["fieldName"; "boxedProp"] |> binding (SynExpr.CreateApp(SynExpr.CreateLongIdent(LongIdentWithDots.Create [parameters.ComponentPropsName; "toDynamicMemberDef"]), SynExpr.CreateIdentString "prop")) |> Let
                    applicationMany (SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue")) [ SynExpr.CreateIdentString "componentProps"; SynExpr.CreateIdentString "fieldName"; SynExpr.CreateIdentString "boxedProp" ] |> Expression ]
                |> simpleLambdaStatement [("prop", SynType.Create parameters.ComponentPropsName)]
                |> SynExpr.CreateParen
                SynExpr.CreateIdentString "props" ] |> Expression
            applicationMany (SynExpr.CreateLongIdent(LongIdentWithDots.CreateString "DynObj.setValue")) [ SynExpr.CreateIdentString "t"; SynExpr.CreateConstString "props"; SynExpr.CreateIdentString "componentProps" ] |> Expression 
            SynExpr.CreateIdentString "t" |> expressionUpcast (SynType.Create "DashComponent") |> Expression ]

    let componentLetDeclaration = 
        functionPattern parameters.CamelCaseComponentName
            [ ("id", SynType.Create "string")
              ("props", SynType.CreateApp(SynType.Create "seq", [SynType.Create parameters.ComponentPropsName]))
              ("children", SynType.CreateApp(SynType.Create "seq", [SynType.Create "DashComponent"])) ]
        |> binding componentDeclaration
        |> withXMLDocLet "This is additional test documentation" //TODO documentation comments
        |> letDeclaration

    let moduleDeclaration = 
        parameters.ComponentName
        |> Ident.CreateLong 
        |> SynComponentInfoRcd.Create
        |> withXMLDoc "This is additional test documentation" //TODO documentation comments
        |> withModuleAttribute (SynAttribute.Create "RequireQualifiedAccess")
        |> nestedModule
            [ componentPropertyDUDeclaration 
              componentTypeDeclaration
              componentLetDeclaration ]

    let namespaceDeclaration =
        parameters.LibraryNamespace
        |> Ident.CreateLong 
        |> SynModuleOrNamespaceRcd.CreateNamespace
        |> withNamespaceDeclarations
            [ SynModuleDecl.CreateOpen "Dash.NET"
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "Plotly.NET"
              moduleDeclaration ] 


    ParsedImplFileInputRcd
        .CreateFs(parameters.ComponentName)
        .AddModule(namespaceDeclaration)
    |> ParsedInput.CreateImplFile

let generateCodeFromAST (path: string) ast =
    let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true }
    let formattedCodeAsync = CodeFormatter.FormatASTAsync(ast, Path.GetFileName path, [], None, cfg)

    let formattedCode =
        [   "//------------------------------------------------------------------------------" //TODO documentation comments
            "//        This file has been automatically generated."
            "//        Changes to this file will be lost when the code is regenerated."
            "//------------------------------------------------------------------------------"
            formattedCodeAsync |> Async.RunSynchronously ] //We dont really need this to be async, as this is all the app is doing
        |> String.concat Environment.NewLine

    File.WriteAllText(path,formattedCode)
            