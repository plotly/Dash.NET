open Dash.NET.ComponentGeneration

[<EntryPoint>]
let main argv =
    ComponentString.ComponentParameters.create "TestComponent" "TestNamespace" "TestType" "TestNamespace" [|"normalProp";"🥑";"_test"|]
    |> ComponentString.generateComponentTemplateFile "TestComponentString.fs"

    ComponentAST.createComponentAST ()
    |> ComponentAST.generateCodeFromAST "TestComponentAST.fs"
    0