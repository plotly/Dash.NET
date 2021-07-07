open Dash.NET.ComponentGeneration
open FSharp.Compiler.SourceCodeServices
open FSharp.Compiler.Text
open System.IO
open ComponentParameters

[<EntryPoint>]
let main argv =
    ComponentParameters.create "TestComponent" "TestNamespace" "TestType" "TestNamespace" ["normalProp";"🥑";"_test"]
    |> ASTGeneration.createComponentAST 
    |> ASTGeneration.generateCodeFromAST "TestComponentAST.fs"

    0