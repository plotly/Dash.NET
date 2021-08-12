﻿module Dash.NET.ComponentGeneration.ASTHelpers

open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open FSharp.Compiler.XmlDoc

//TODO: Clean up this file
//TODO: Add documentation

let componentInfo = Ident.CreateLong >> SynComponentInfoRcd.Create
let namespaceInfo = Ident.CreateLong >> SynModuleOrNamespaceRcd.CreateNamespace

let patternNamedOptional (pname: string) = SynPatRcd.OptionalVal {Id = Ident.Create pname; Range = range.Zero}
let patternNamed (pname: string) = SynPatRcd.CreateNamed(Ident.Create pname, SynPatRcd.CreateWild)
let patternNamedTuple (pnames: string list) = pnames |> List.map patternNamed |> SynPatRcd.CreateTuple
let withPatternType (ptype: SynType) (pat: SynPatRcd) = SynPatRcd.CreateTyped(pat, ptype)

let withXMLDoc (docLines: string list) (cinfo: SynComponentInfoRcd) =
    let xml =
        docLines
        |> List.map PreXmlDoc.Create
        |> List.reduce PreXmlDoc.Merge
    { cinfo with XmlDoc = xml }
let withXMLDocLet (docLines: string list) (binfo: SynBindingRcd) =
    let xml =
        docLines
        |> List.map PreXmlDoc.Create
        |> List.reduce PreXmlDoc.Merge
    { binfo with XmlDoc = xml }

let withNamespaceDeclarations (decls: SynModuleDecl list) (moduOrNsp: SynModuleOrNamespaceRcd) = 
    moduOrNsp.AddDeclarations decls
let withModuleAttribute (attrib: SynAttribute) (modu: SynComponentInfoRcd) =
    { modu with Attributes = (SynAttributeList.Create [attrib]) :: modu.Attributes }

let nestedModule (members: SynModuleDecl list) (modu: SynComponentInfoRcd) =
    SynModuleDecl.CreateNestedModule (modu, members)

let binding (expr: SynExpr) (lpatt: SynPatRcd)  =
    { SynBindingRcd.Let with 
        Pattern = lpatt
        Expr = expr }
let memberBinding (expr: SynExpr) (lpatt: SynPatRcd)  =
    { SynBindingRcd.Let with 
        Pattern = lpatt
        Expr = expr
        ValData = SynValData( Some (MemberFlags.InstanceMember) ,SynValInfo ([], SynArgInfo ([], false, None)), None) }
let letDeclaration (binding: SynBindingRcd) =
    SynModuleDecl.CreateLet [ binding ]

let typeDeclaration (tDef: SynMemberDefns) (tInfo: SynComponentInfoRcd) = 
    SynModuleDecl.CreateType(tInfo, tDef) 
let typeInherit (itype: SynExpr) =
    SynMemberDefn.Inherit (SynType.StaticConstantExpr(itype, range.Zero), None, range.Zero)

let simpleTypeDeclaration (tSimpleDef: SynTypeDefnSimpleReprRcd) (tDef: SynMemberDefns)  (tInfo: SynComponentInfoRcd) = 
    SynModuleDecl.CreateSimpleType(tInfo, tSimpleDef, tDef)
let unionDefinition (cases: SynUnionCaseRcd list) =
    cases |> SynTypeDefnSimpleReprUnionRcd.Create |> SynTypeDefnSimpleReprRcd.Union
let recordDefinition (cases: SynFieldRcd list) =
    cases |> SynTypeDefnSimpleReprRecordRcd.Create |> SynTypeDefnSimpleReprRcd.Record
let typeAbbreviationDefinition (atype: SynType) =
    { ParseDetail = ParserDetail.Ok
      Type = atype
      Range = range.Zero } |> SynTypeDefnSimpleReprRcd.TypeAbbrev

let appType (tApp: string list) =
    match tApp with
    | [t] -> SynType.Create t
    | t::at -> SynType.CreateApp(SynType.Create t, at |> List.map SynType.Create)
    | [] -> SynType.Create "obj"

let simpleUnionCase (label: string) (utype: SynFieldRcd list) =
    SynUnionCaseRcd.Create ((Ident.Create label), (SynUnionCaseType.Create utype))

let simpleField (fname:string) (ftype: string) =
    SynFieldRcd.Create (fname, LongIdentWithDots.CreateString ftype) 
let anonSimpleField (ftype: string) =
    { SynFieldRcd.Create ("", LongIdentWithDots.CreateString ftype) with Id = None }

let simpleAppField (fname:string) (funApp: string list) =
    match funApp with
    | [funType] -> simpleField fname funType
    | funType::argTypes -> (SynFieldRcd.CreateApp fname (LongIdentWithDots.CreateString funType) (argTypes |> List.map LongIdentWithDots.CreateString))
    | [] -> simpleField fname "obj"
let anonAppField (funApp: string list) =
    match funApp with
    | [funType] -> anonSimpleField funType
    | funType::argTypes -> { (SynFieldRcd.CreateApp "" (LongIdentWithDots.CreateString funType) (argTypes |> List.map LongIdentWithDots.CreateString)) with Id = None }
    | [] -> anonSimpleField "obj"

let functionPattern (fname: string) (args: (string*SynType) list) =
    let argumentDeclarations = 
        args
        |> List.map (fun (pname, ptype) -> 
            patternNamed pname
            |> withPatternType ptype
            |> SynPatRcd.CreateParen )
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, argumentDeclarations)
let functionPatternNoArgTypes (fname: string) (args: (string) list) =
    let argumentDeclarations = 
        args
        |> List.map (fun pname -> 
            patternNamed pname
            |> SynPatRcd.CreateParen )
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, argumentDeclarations)
let functionPatternThunk (fname: string) =
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, [SynPatRcd.Const {Const = SynConst.Unit; Range = range.Zero}])

let memberFunctionPattern (fname: string) (args: (string*SynType*bool) list) =
    let argumentDecarations = 
        args
        |> List.map (fun (pname, ptype, isRequired) -> 
            if isRequired then patternNamed pname
            else patternNamedOptional pname
            |> withPatternType ptype )
        |> SynPatRcd.CreateTuple
        |> SynPatRcd.CreateParen
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, [argumentDecarations])

let simpleMatchClause (pat: string) (args: string list) (whenc: SynExpr option) (result: SynExpr) =
    SynMatchClause.Clause ((functionPatternNoArgTypes pat args).FromRcd, whenc, result, range.Zero, DebugPointForTarget.No)
let matchStatement (clauses: SynMatchClause list) (matchOn: SynExpr)  =
    SynExpr.CreateMatch (matchOn, clauses)

let anonRecord (fields: (Ident*SynExpr) list) =
    SynExpr.AnonRecd (false, None, fields, range.Zero)

let simpleLambdaStatement (args: (string*SynType) list) (expr: SynExpr) =
    let argumentDecarations = 
        let simplePats =
            args
            |> List.map (fun (pname, ptype) -> 
                SynSimplePat.CreateTyped (Ident.Create pname, ptype))
        SynSimplePats.SimplePats (simplePats, range.Zero)
    SynExpr.Lambda (false, false, argumentDecarations, expr, None, range.Zero)

let expressionUpcast (etype: SynType) (expr: SynExpr) =
    SynExpr.Upcast(expr, etype, range.Zero)

type ExpressionSequenceItem =
    | Let of SynBindingRcd
    | Expression of SynExpr
let expressionSequence (expressions: ExpressionSequenceItem list) =
    match List.rev expressions with
    | [] -> SynExpr.CreateUnit
    | [Let _] -> SynExpr.CreateUnit //cant have an expression with only a let
    | [Expression e] -> e
    | (Let _) :: _ -> SynExpr.CreateUnit //the last item cant be a let
    | (Expression last) :: rest ->
        rest
        |> List.fold (fun nextExpr expr  ->
            match expr with
            | Let b -> SynExpr.LetOrUse (false, false, [ b.FromRcd ], nextExpr, range.Zero )
            | Expression e -> SynExpr.Sequential (DebugPointAtSequential.Both, false, e, nextExpr, range.Zero))
            last

let application (expers: SynExpr list) = 
    match expers with
    | funExpr::argExprs -> argExprs |> List.fold (fun expr newExpr -> SynExpr.CreateApp(expr, newExpr)) funExpr
    | [] -> SynExpr.CreateUnit

let expressionList (expers: SynExpr list) =
    SynExpr.ArrayOrList (false, expers, range.Zero)