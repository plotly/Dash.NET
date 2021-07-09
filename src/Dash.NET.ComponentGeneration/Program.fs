open Dash.NET.ComponentGeneration
open FSharp.Compiler.SourceCodeServices
open FSharp.Compiler.Text
open System.IO
open ComponentParameters

[<EntryPoint>]
let main argv =
    async {
        let! success, output, error = ProjectGeneration.createProject "TestComponent"

        #if DEBUG
        printfn "Output: %s" output
        #endif

        if not success then
            printfn "Error: %s" error
            return ()

        ComponentParameters.create "TestComponent" "TestNamespace" "TestType" "TestNamespace" ["normalProp";"🥑";"_test"]
        |> ASTGeneration.createComponentAST 
        |> ASTGeneration.generateCodeFromAST "./TestComponent/TestComponentAST.fs"
    }
    |> Async.RunSynchronously
    0