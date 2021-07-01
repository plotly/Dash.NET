module Dash.NET.ComponentGeneration.ComponentAST

open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open Fantomas
open System
open System.IO

let withNamespaceDeclarations (decls: SynModuleDecl list) (moduOrNsp: SynModuleOrNamespaceRcd) = 
    moduOrNsp.AddDeclarations decls
let withModuleAttribute (attrib: SynAttribute) (modu: SynComponentInfoRcd) =
    { modu with Attributes = (SynAttributeList.Create [attrib]) :: modu.Attributes }

let nestedModule (members: SynModuleDecl list) (modu: SynComponentInfoRcd) =
    SynModuleDecl.CreateNestedModule (modu, members)

let withTypeDefinition (tDef: SynMemberDefns) (tInfo: SynComponentInfoRcd) = 
    SynModuleDecl.CreateType(tInfo, tDef)
let withSimpleTypeDefinition (tSimpleDef: SynTypeDefnSimpleReprRcd) (tDef: SynMemberDefns)  (tInfo: SynComponentInfoRcd) = 
    SynModuleDecl.CreateSimpleType(tInfo, tSimpleDef, tDef)

let simpleUnionCase (label: string) (utype: SynFieldRcd list) =
    SynUnionCaseRcd.Create ((Ident.Create label), (SynUnionCaseType.Create utype))
let anonUnionField (ftype: string) =
    { SynFieldRcd.Create ("", LongIdentWithDots.CreateString ftype) with Id = None }

let withPatternName (pname: string) (pat: SynPatRcd) = SynPatRcd.CreateNamed(Ident.Create pname, pat)
let withPatternType (ptype: string) (pat: SynPatRcd) = SynPatRcd.CreateTyped(pat, SynType.Create ptype)

let functionPattern (fname: string) (args: (string*string) list) =
    let argumentDecarations = 
        args
        |> List.map (fun (pname, ptype) -> 
            SynPatRcd.CreateWild
            |> withPatternName pname
            |> withPatternType ptype
            |> SynPatRcd.CreateParen )
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, argumentDecarations)

let functionPatternNoArgTypes (fname: string) (args: (string) list) =
    let argumentDecarations = 
        args
        |> List.map (fun pname -> 
            SynPatRcd.CreateWild
            |> withPatternName pname
            |> SynPatRcd.CreateParen )
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, argumentDecarations)

let simpleMatchClause (pat: string) (args: string list) (whenc: SynExpr option) (result: SynExpr) =
    SynMatchClause.Clause ((functionPatternNoArgTypes pat args).FromRcd,whenc,result,range.Zero,DebugPointForTarget.No)
let simpleMatchStatement (matchOn: string) (clauses: SynMatchClause list) =
    SynExpr.CreateMatch (SynExpr.CreateIdentString matchOn,clauses)

let createComponentAST () =
    let componentPropertyDUCases =
        SynTypeDefnSimpleReprUnionRcd.Create
            [ simpleUnionCase "TestComponentProperty1" [anonUnionField "string"]    //TODO pass in
              simpleUnionCase "TestComponentProperty2" [anonUnionField "string"] ]  //TODO pass in
        |> SynTypeDefnSimpleReprRcd.Union

    let componentPropertyToDynamicMemberDefDeclaration =
        { SynBindingRcd.Null with
            Pattern = functionPattern "x.toDynamicMemberDef" [("prop","TestComponentProperties")] //TODO pass in
            Expr = simpleMatchStatement "prop"
                [ simpleMatchClause "TestComponentProperty1" ["p"] None (SynExpr.CreateTuple 
                    [ SynExpr.CreateConstString "TestComponentProperty1" //TODO pass in
                      SynExpr.CreateApp (SynExpr.CreateIdentString "box", SynExpr.CreateConstString "p") ])  
                  simpleMatchClause "TestComponentProperty2" ["p"] None (SynExpr.CreateTuple 
                    [ SynExpr.CreateConstString "TestComponentProperty2" //TODO pass in
                      SynExpr.CreateApp (SynExpr.CreateIdentString "box", SynExpr.CreateConstString "p") ]) ]
        }
        |> SynMemberDefn.CreateStaticMember

    let componentPropertyDUDeclaration =
        "TestComponentProperties" //TODO pass in
        |> Ident.CreateLong
        |> SynComponentInfoRcd.Create
        |> withSimpleTypeDefinition 
            componentPropertyDUCases
            [ componentPropertyToDynamicMemberDefDeclaration ]

    let moduleDeclaration = 
        "TestModule" //TODO pass in
        |> Ident.CreateLong 
        |> SynComponentInfoRcd.Create
        |> withModuleAttribute (SynAttribute.Create "RequireQualifiedAccess")
        |> nestedModule
            [ componentPropertyDUDeclaration ]

    let namespaceDeclaration =
        "TestNamespace" //TODO pass in
        |> Ident.CreateLong 
        |> SynModuleOrNamespaceRcd.CreateNamespace
        |> withNamespaceDeclarations
            [ SynModuleDecl.CreateOpen "Dash.NET"
              SynModuleDecl.CreateOpen "System"
              SynModuleDecl.CreateOpen "Plotly.NET"
              SynModuleDecl.CreateOpen "ComponentPropTypes"
              moduleDeclaration ] 


    ParsedImplFileInputRcd
        .CreateFs("TestComponent") //TODO pass in
        .AddModule(namespaceDeclaration)
    |> ParsedInput.CreateImplFile

let generateCodeFromAST (path: string) ast =
    let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true }
    let formattedCodeAsync = CodeFormatter.FormatASTAsync(ast, Path.GetFileName path, [], None, cfg) //TODO pass in

    let formattedCode =
        [   "//------------------------------------------------------------------------------" //TODO documentation comments
            "//        This file has been automatically generated."
            "//        Changes to this file will be lost when the code is regenerated."
            "//------------------------------------------------------------------------------"
            formattedCodeAsync |> Async.RunSynchronously ] //We dont really need this to be async, as this is all the app is doing
        |> String.concat Environment.NewLine

    File.WriteAllText(path,formattedCode)
            