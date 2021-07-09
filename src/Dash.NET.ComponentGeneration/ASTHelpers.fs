module Dash.NET.ComponentGeneration.ASTHelpers

open FsAst.AstCreate
open FsAst.AstRcd
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open FSharp.Compiler.XmlDoc

//TODO: Clean up this file
//TODO: Add documentation

let patternNamedOptional (pname: string) = SynPatRcd.OptionalVal {Id = Ident.Create pname; Range = range.Zero}
let patternNamed (pname: string) = SynPatRcd.CreateNamed(Ident.Create pname, SynPatRcd.CreateWild)
let patternNamedTuple (pnames: string list) = pnames |> List.map patternNamed |> SynPatRcd.CreateTuple
let withPatternType (ptype: SynType) (pat: SynPatRcd) = SynPatRcd.CreateTyped(pat, ptype)

let withXMLDoc (doc: string) (cinfo: SynComponentInfoRcd) =
    { cinfo with XmlDoc = PreXmlDoc.Create doc }
let withXMLDocLet (doc: string) (binfo: SynBindingRcd) =
    { binfo with XmlDoc = PreXmlDoc.Create doc }

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
let letDeclaration (binding: SynBindingRcd) =
    SynModuleDecl.CreateLet [ binding ]

let typeDefinition (tDef: SynMemberDefns) (tInfo: SynComponentInfoRcd) = 
    SynModuleDecl.CreateType(tInfo, tDef) 
let typeInherit (itype: SynExpr) =
    SynMemberDefn.Inherit (SynType.StaticConstantExpr(itype, range.Zero), None, range.Zero)
let simpleTypeDefinition (tSimpleDef: SynTypeDefnSimpleReprRcd) (tDef: SynMemberDefns)  (tInfo: SynComponentInfoRcd) = 
    SynModuleDecl.CreateSimpleType(tInfo, tSimpleDef, tDef)
let unionDefinition (cases: SynUnionCaseRcd list) =
    cases |> SynTypeDefnSimpleReprUnionRcd.Create |> SynTypeDefnSimpleReprRcd.Union

let simpleUnionCase (label: string) (utype: SynFieldRcd list) =
    SynUnionCaseRcd.Create ((Ident.Create label), (SynUnionCaseType.Create utype))
let anonUnionField (ftype: string) =
    { SynFieldRcd.Create ("", LongIdentWithDots.CreateString ftype) with Id = None }

let functionPattern (fname: string) (args: (string*SynType) list) =
    let argumentDecarations = 
        args
        |> List.map (fun (pname, ptype) -> 
            patternNamed pname
            |> withPatternType ptype
            |> SynPatRcd.CreateParen )
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, argumentDecarations)
let functionPatternNoArgTypes (fname: string) (args: (string) list) =
    let argumentDecarations = 
        args
        |> List.map (fun pname -> 
            patternNamed pname
            |> SynPatRcd.CreateParen )
    SynPatRcd.CreateLongIdent(LongIdentWithDots.CreateString fname, argumentDecarations)

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

let applicationMany (funExpr: SynExpr) (argExprs: SynExpr list) = 
    argExprs |> List.fold (fun expr newExpr -> SynExpr.CreateApp(expr, newExpr)) funExpr