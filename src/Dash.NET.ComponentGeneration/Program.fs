open Dash.NET.ComponentGeneration
open ComponentParameters
open Prelude
open System.IO
open System.Text.Json

let performGeneration (metaFile: string) =
    async {
        if File.Exists metaFile then
            let parametersList = 
                metaFile
                |> File.ReadAllText
                |> ReactMetadata.jsonDeserialize
                |> ComponentParameters.fromReactMetadata

            return!
                parametersList
                |> List.fold (fun state parameters -> 
                    state
                    |@> ProjectGeneration.createProject 
                        parameters.ComponentName 
                        parameters.ComponentFSharp 
                        parameters.ComponentJavascript 
                        "0.1.0-alpha9" //TODO: pass in

                    |@> (parameters
                         |> ASTGeneration.createComponentAST 
                         |> ASTGeneration.generateCodeFromAST (sprintf "./%s/%s" parameters.ComponentName parameters.ComponentFSharp) )

                    |@> ProjectGeneration.buildProject parameters.ComponentName)
                    (async { return (true, "", "")})
        else
            return (false, "", sprintf "Metadata file %s does not exist" metaFile)
    }

[<EntryPoint>]
let main argv =
    async {
        let! success, output, error = performGeneration "metadata.json" //TODO: pass in

        //TODO better logging (allow for verbose logging)
        #if DEBUG
        printfn "%s" output
        #endif

        if not success then
            printfn "Error: %s" error
    }
    //This is all in asyncs to make handling async and IO operations easier, it doesn't actually have to run async
    |> Async.RunSynchronously

    0