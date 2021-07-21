open Dash.NET.ComponentGeneration
open ComponentParameters
open Prelude

let performGeneration () =
    async {
        return! 
            ProjectGeneration.createProject "TestComponent" "TestComponentAST.fs" "test_component.js" "0.1.0-alpha9"

            |@> (ComponentParameters.create "TestComponent" "TestNamespace" "TestType" "TestNamespace" ["normalProp";"🥑";"_test"]
                 |> ASTGeneration.createComponentAST 
                 |> ASTGeneration.generateCodeFromAST "./TestComponent/TestComponentAST.fs")

            |@> ProjectGeneration.buildProject "TestComponent"
    }

[<EntryPoint>]
let main argv =
    async {
        let! success, output, error = performGeneration()

        //TODO better logging (allow for verbose logging)
        #if DEBUG
        printfn "%s" output
        #endif

        if not success then
            printfn "Error: %s" error
    }
    |> Async.RunSynchronously
    0