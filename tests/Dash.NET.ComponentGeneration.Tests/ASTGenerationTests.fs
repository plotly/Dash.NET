module Dash.Net.ComponentGeneration.Tests.ASTGenerationTests

open Expecto
open System
open System.Collections.Generic
open FSharp.Compiler.SourceCodeServices
open FSharp.Compiler.Text
open Serilog

open Dash.NET.ComponentGeneration.Prelude
open Dash.NET.ComponentGeneration.ReactMetadata
open Dash.NET.ComponentGeneration.ComponentParameters
open Dash.NET.ComponentGeneration.ASTGeneration

let checker = FSharpChecker.Create()

// Perform pre-compilation (parsing and type checking) on the generated code to check if it is valid
let propertyCodeGenerationTest (prop: SafeReactProp) =
    async {
        let log = (new LoggerConfiguration()).CreateLogger()

        // making it an fsx makes referencing outside DLLs much easier
        let file = "test.fsx"

        let props =
            [ KeyValuePair("aProp", prop) ]
            |> Dictionary

        let comp =
            { description = Some "test"
              displayName = Some "test"
              props = props }

        let! sourceText =
            ComponentParameters.fromReactMetadata log "test" ["test.js"] ([KeyValuePair("test.js", comp)] |> Dictionary)
            |> List.head
            |> createComponentAST log
            |> generateCodeStringFromAST log file

        let sourceInput =
            // these are reffering to the DLLs in the build output of this tests fsproj
            [ yield "#r \"Dash.NET.dll\""
              yield "#r \"System.Text.Json.dll\"" 
              // On linux this is required but it causes an error on windows
              if System.OperatingSystem.IsLinux() then yield "#r \"System.Net.WebClient.dll\""
              // will through an error if we use the existing dll, we have to grab it from nuget
              // Error: "typecheck error The module/namespace 'System.Dynamic' from compilation unit 'System.Linq.Expressions' did not contain the namespace, module or type 'DynamicObject'"
              // TODO: figure out why this is happening, would be better to use Plotly.NET.dll here
              yield "#r \"nuget: Plotly.NET, 2.0.0-preview.9\""
              yield "#r \"nuget: DynamicObj\"" ]
            |> List.reduce (sprintf "%s\n%s")
            |> sprintf "%s\n%s" sourceText
            |> SourceText.ofString 

        let lineNumberedSourceText =
            sourceText
            |> String.split "\n"
            |> List.mapi (fun i s -> sprintf "% 3d | %s" i s)
            |> List.reduce (sprintf "%s\n%s")

        let! defaultProjOptions, errors = checker.GetProjectOptionsFromScript(file, sourceInput)
        let projOptions = 
            { defaultProjOptions with
                // We dont want to do a complete type check, just enough to know that if the
                // environment was set up correctly that it should compile
                IsIncompleteTypeCheckEnvironment = true }
        
        let! parseFileResults, checkFileResults = checker.ParseAndCheckFileInProject(file, 0, sourceInput, projOptions)

        let parseErrorsString =
            errors
            |> List.map string
            |> List.fold (sprintf "%s\n%s") ""

        match parseFileResults.ParseTree, checkFileResults with
        | Some _, FSharpCheckFileAnswer.Succeeded res -> 
            let checkErrorsString =
                res.Errors
                |> Array.map string
                |> Array.fold (sprintf "%s\n%s") ""

            match res.Errors with
            | [||] -> return Ok ()
            | _ -> return Error (sprintf "Failed to check generated code: \n%s\n\n%s" checkErrorsString lineNumberedSourceText)
        | None, _ -> 
            match errors with
            | [] -> return Error (sprintf "Failed to parse generated code: \nunknown error\n\n%s" lineNumberedSourceText)
            | _ -> return Error (sprintf "Failed to parse generated code: \n%s\n\n%s" parseErrorsString lineNumberedSourceText)
        | _, FSharpCheckFileAnswer.Aborted -> return Error (sprintf "Failed to check generated code: \nchecking aborted for unknown reason\n\n%s" lineNumberedSourceText)
    }

[<Tests>]
let tests =
    testList "AST Generation Tests" [
        testList "Code Generation" [
            testCase "Generate With Array Prop" <| fun _ ->
                let prop =
                    { propType = Array ({ computed = Some false; required = Some true; description = Some "test" }, Some  "[1, 2, 3]") |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Bool Prop" <| fun _ ->
                let prop =
                    { propType = Bool ({ computed = Some true; required = Some false; description = Some "test" }, None) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Number Prop" <| fun _ ->
                let prop =
                    { propType = Number ({ computed = Some false; required = Some true; description = Some "test" }, Some (5.6 :> IConvertible)) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With String Prop" <| fun _ ->
                let prop =
                    { propType = String ({ computed = Some false; required = Some true; description = Some "test" }, Some "test") |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Object Prop" <| fun _ ->
                let prop =
                    { propType = Object ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Any Prop" <| fun _ ->
                let prop =
                    { propType = Any ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Element Prop" <| fun _ ->
                let prop =
                    { propType = Element ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Node Prop" <| fun _ ->
                let prop =
                    { propType = Node ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            // React PropTypes

            testCase "Generate With Enum Prop" <| fun _ ->
                let prop =
                    { propType = 
                        Enum 
                          ( { computed = Some false; required = Some true; description = Some "test" }, 
                            [ SafeReactPropType.Any ({ computed = Some false; required = None; description = None }, Some "e1")
                              SafeReactPropType.Any ({ computed = Some false; required = None; description = None }, Some "e2")
                              SafeReactPropType.Any ({ computed = Some false; required = None; description = None }, Some "e3") ] |> Some ) 
                        |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Union Prop" <| fun _ ->
                let prop =
                    { propType = 
                        Union 
                          ( { computed = Some false; required = Some true; description = Some "test" }, 
                            [ SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None)
                              SafeReactPropType.String ({ computed = None; required = None; description = None }, None)
                              SafeReactPropType.Number ({ computed = None; required = None; description = None }, None) ] |> Some )
                        |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With ArrayOf Prop" <| fun _ ->
                let prop =
                    { propType = 
                        ArrayOf 
                          ( { computed = Some false; required = Some true; description = Some "test" }, 
                            SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None) |> Some )
                        |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With ObjectOf Prop" <| fun _ ->
                let prop =
                    { propType = 
                        ObjectOf 
                          ( { computed = Some false; required = Some true; description = Some "test" }, 
                            SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None) |> Some ) 
                        |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Shape Prop" <| fun _ ->
                let dict =
                    [ KeyValuePair("p1", SafeReactPropType.Bool ({ computed = None; required = Some false; description = Some "test" }, None))
                      KeyValuePair("p2", SafeReactPropType.String ({ computed = None; required = Some false; description = Some "test" }, None))
                      KeyValuePair("p3", SafeReactPropType.Number ({ computed = None; required = Some false; description = Some "test" }, None)) ]
                    |> Dictionary

                let prop =
                    { propType = Shape ( { computed = Some false; required = Some true; description = Some "test" }, dict |> Some ) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err

            testCase "Generate With Exact Prop" <| fun _ ->
                let dict =
                    [ KeyValuePair("p1", SafeReactPropType.Bool ({ computed = None; required = Some false; description = None }, None))
                      KeyValuePair("p2", SafeReactPropType.String ({ computed = None; required = Some false; description = None }, None))
                      KeyValuePair("p3", SafeReactPropType.Number ({ computed = None; required = Some false; description = None }, None)) ]
                    |> Dictionary

                let prop =
                    { propType = Exact ( { computed = Some false; required = Some true; description = Some "test" }, dict |> Some ) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                match propertyCodeGenerationTest prop |> Async.RunSynchronously with
                | Ok _ -> Expect.isTrue true ""
                | Error err -> Expect.isTrue false err
        ]
    ]