open Dash.NET.ComponentGeneration
open ComponentParameters
open Prelude
open System.IO
open System.Text.Json
open Argu
open System

// Hardcoded defaults
let defaultDashVersion = "0.1.0-alpha9"

let defaultIgnoreFiles = []
let defaultIgnoreFolders = [ "__pycache__" ]
let defaultIgnoreExtensions = [ "py" ]

// Command line arguments
type CmdArgs =
    | [<AltCommandLine("-n"); First; Mandatory; Unique>] Name of name:string
    | [<AltCommandLine("-f"); Mandatory; Unique>] Folder of folder:string
    | [<AltCommandLine("-m"); Unique>] Metadata of meta:string
    | [<AltCommandLine("-v"); Unique>] DashVersion of version:string
    | [<AltCommandLine("-a")>] AddFile of source:string
    | [<AltCommandLine("-i")>] IgnoreFile of source:string
    | [<AltCommandLine("-o")>] IgnoreFolder of folder:string
    | [<AltCommandLine("-x")>] IgnoreExtension of extension:string
    | [<AltCommandLine("-d"); Unique>] DisableDefaultIgnore
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Name _ -> "Name of the component or group of components"
            | Folder _ -> "Folder containing the component"
            | Metadata _ -> "React docgen metadata.json file, defaults to '<component_folder>/metadata.json'"
            | DashVersion _ -> "The version of Dash.NET to use, defaults to '0.1.0-alpha9'"
            | AddFile _ -> "Additional local source file to include"
            | IgnoreFile _ -> "Ignore files with this name"
            | IgnoreFolder _ -> "Ignore folders with this name"
            | IgnoreExtension _ -> "Ignore files in the folder with this extension (eg. py, js, json)"
            | DisableDefaultIgnore -> "Don't ignore *.py and __pycache__ by default"

let performGeneration (componentProjectName: string) (dashVersion: string) (componentFolder: string) (metaFile: string) (localFiles: string list) =
    async {
        let localJavascript =
            localFiles
            |> List.filter (Path.GetExtension >> (fun s -> s.ToLowerInvariant()) >> (=) ".js")
            |> List.map (fun js -> Path.GetRelativePath(componentFolder, js))
            |> List.map (fun js -> Path.Combine("ComponentFiles", js))

        let parametersList = 
            metaFile
            |> File.ReadAllText
            |> ReactMetadata.jsonDeserialize
            |> ComponentParameters.fromReactMetadata localJavascript

        let projectCreate =
            ProjectGeneration.createProject 
                componentProjectName
                componentFolder
                localFiles
                dashVersion

        return!
            parametersList
            |> List.fold (fun state parameters -> 
                state
                |@> (parameters
                        |> ASTGeneration.createComponentAST 
                        |> ASTGeneration.generateCodeFromAST (Path.Combine(".", parameters.ComponentName, parameters.ComponentFSharp)) )

                |@> ProjectGeneration.buildProject parameters.ComponentName)
                projectCreate
    }

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CmdArgs>(errorHandler = errorHandler)

    let args = 
        parser
            .ParseCommandLine(argv)
            .GetAllResults()

    let maybeName = 
        args 
        |> List.tryPick ( function | Name m -> Some m | _ -> None )
        |> Option.map Ok
        |> Option.defaultValue (Error "Missing argument: name")

    let maybeFolder = 
        args 
        |> List.tryPick ( function | Folder f -> Some f | _ -> None )
        |> Option.map (fun f -> if Directory.Exists f then Ok f else Error (sprintf "Folder %s does not exist" f))
        |> Option.defaultValue (Error "Missing argument: folder")

    match maybeName, maybeFolder with
    | Ok name, Ok folder ->
        
        let maybeMetadata =
            args 
            |> List.tryPick ( function | Metadata m -> Some m | _ -> None )
            |> Option.defaultValue (Path.Combine(folder, "metadata.json"))
            |> (fun m -> if File.Exists m then Ok m else Error (sprintf "Metadata file %s does not exist" m))

        let disableDefaultIgnore = args |> List.contains DisableDefaultIgnore

        let ignoreFiles = 
            args 
            |> List.choose ( function | IgnoreFile m -> Some m | _ -> None )
            |> (if disableDefaultIgnore then (@) defaultIgnoreFiles else id)
        let ignoreFolders = 
            args 
            |> List.choose ( function | IgnoreFolder m -> Some m | _ -> None )
            |> (if disableDefaultIgnore then (@) defaultIgnoreFolders else id)
        let ignoreExtensions = 
            args 
            |> List.choose ( function | IgnoreExtension m -> Some m | _ -> None )
            |> (if disableDefaultIgnore then (@) defaultIgnoreExtensions else id)

        let maybeLocalFiles = 
            let rec recursiveFolderFiles f =
                let recursiveFiles =
                    Directory.EnumerateDirectories f 
                    |> List.ofSeq
                    |> List.filter (fun folder -> ignoreFolders |> List.contains folder |> not)
                    |> List.collect recursiveFolderFiles

                let rootFiles =
                    Directory.EnumerateFiles f
                    |> List.ofSeq

                [ yield! recursiveFiles; yield! rootFiles ]

            recursiveFolderFiles folder
            |> List.filter (fun file -> ignoreFiles |> List.contains file |> not)
            |> List.filter (fun ex -> ignoreExtensions |> List.contains (ex |> Path.GetExtension |> sprintf ".%s") |> not)
            |> (function | [] -> Error "No local source files found" | j -> Ok j)

        let dashVersion =
            args 
            |> List.tryPick ( function | DashVersion v -> Some v | _ -> None )
            |> Option.defaultValue (defaultDashVersion)

        match maybeMetadata, maybeLocalFiles with 
        | Ok metadata, Ok localFiles -> 
            //This is all in asyncs to make handling async and IO operations easier, it doesn't actually have to run async
            let success, output, error = 
                performGeneration name dashVersion folder metadata localFiles
                |> Async.RunSynchronously

            //TODO better logging (allow for verbose logging)
            #if DEBUG
            printfn "%s" output
            #endif

            if not success then
                printfn "Error: %s" error
                1

            else 
                0

        | Error error, _ 
        | _, Error error -> 
            printfn "Error: %s" error
            1
    
    | Error error, _ 
    | _, Error error -> 
        printfn "Error: %s" error
        1