open Dash.NET.ComponentGeneration
open ComponentParameters
open Prelude
open System.IO
open System.Text.Json
open Argu
open System
open FSharp.Compiler.SourceCodeServices
open FSharp.Compiler.Text

// Hardcoded defaults
let defaultDashVersion = "0.1.0-alpha9"
let defaultIgnore = [ "__pycache__"; ".*\.py" ]

// Command line arguments
type CmdArgs =
    | [<AltCommandLine("-n"); First; Mandatory; Unique>] Name of name:string
    | [<AltCommandLine("-s"); Mandatory; Unique>] ShortName of name:string
    | [<AltCommandLine("-f"); Mandatory; Unique>] ComponentDirectory of folder:string
    | [<AltCommandLine("-m"); Unique>] Metadata of meta:string
    | [<AltCommandLine("-v"); Unique>] DashVersion of version:string
    | [<AltCommandLine("-o"); Unique>] OutputDirectory of folder:string
    | [<AltCommandLine("-a")>] AddFile of source:string
    | [<AltCommandLine("-i")>] Ignore of source:string
    | [<AltCommandLine("-d"); Unique>] DisableDefaultIgnore
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Name _ -> "Name of the component or group of components"
            | ShortName _ -> "Name of the exported javascript namespace component"
            | ComponentDirectory _ -> "Folder containing the component"
            | Metadata _ -> "React docgen metadata.json file, defaults to '<component_folder>/metadata.json'"
            | DashVersion _ -> "The version of Dash.NET to use, defaults to '0.1.0-alpha9'"
            | OutputDirectory _ -> "Directory to create the F# project folder in, defaults to ./"
            | AddFile _ -> "Additional local source file to include"
            | Ignore _ -> "Ignore folders and file paths that match this regex (by default this includes \"__pycache__\" and \".*\.py\")"
            | DisableDefaultIgnore -> "Don't ignore \"__pycache__\" and \".*\.py\" by default"

let performGeneration (componentProjectName: string) (componentShortName: string) (outputFolder: string) (dashVersion: string) (componentFolder: string) (metaFile: string) (localFiles: string list) =
    async {
        let localJavascript =
            localFiles
            |> List.filter (Path.GetExtension >> (fun s -> s.ToLowerInvariant()) >> (=) ".js")
            |> List.map (fun js -> Path.GetRelativePath(componentFolder, js))
            |> List.map (fun js -> Path.Combine(componentProjectName, js))

        let parametersList = 
            metaFile
            |> File.ReadAllText
            |> ReactMetadata.jsonDeserialize
            |> ComponentParameters.fromReactMetadata componentShortName localJavascript

        let projectCreate =
            ProjectGeneration.createProject 
                componentProjectName
                outputFolder
                componentFolder
                localFiles
                dashVersion

        let projectFolder = Path.Combine(outputFolder, componentProjectName)

        return!
            parametersList
            |> List.fold (fun state parameters -> 
                state
                |@> (parameters
                        |> ASTGeneration.createComponentAST 
                        |> ASTGeneration.generateCodeFromAST (Path.Combine(projectFolder, parameters.ComponentFSharp)) ))
                projectCreate
            |@> ProjectGeneration.buildProject componentProjectName outputFolder
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

    let maybeShortName = 
        args 
        |> List.tryPick ( function | ShortName m -> Some m | _ -> None )
        |> Option.map Ok
        |> Option.defaultValue (Error "Missing argument: short name")

    let maybeOutputFolder = 
        args 
        |> List.tryPick ( function | OutputDirectory f -> Some f | _ -> None )
        |> Option.map (fun f -> if Directory.Exists f then Ok f else Error (sprintf "Folder %s does not exist" f))
        |> Option.defaultValue (Ok ".")

    let maybeFolder = 
        args 
        |> List.tryPick ( function | ComponentDirectory f -> Some f | _ -> None )
        |> Option.map (fun f -> if Directory.Exists f then Ok f else Error (sprintf "Folder %s does not exist" f))
        |> Option.defaultValue (Error "Missing argument: folder")

    match maybeName, maybeShortName, maybeFolder, maybeOutputFolder with
    | Ok name, Ok shortName, Ok folder, Ok outputFolder ->
        
        let maybeMetadata =
            args 
            |> List.tryPick ( function | Metadata m -> Some m | _ -> None )
            |> Option.defaultValue (Path.Combine(folder, "metadata.json"))
            |> (fun m -> if File.Exists m then Ok m else Error (sprintf "Metadata file %s does not exist" m))

        let disableDefaultIgnore = args |> List.contains DisableDefaultIgnore

        let ignoreRegexes = 
            args 
            |> List.choose ( function | Ignore m -> Some m | _ -> None )
            |> (if disableDefaultIgnore then id else (@) defaultIgnore)

        let maybeLocalFiles = 
            let rec recursiveFolderFiles f =
                let recursiveFiles =
                    Directory.EnumerateDirectories f 
                    |> List.ofSeq
                    |> List.collect recursiveFolderFiles

                let rootFiles =
                    Directory.EnumerateFiles f
                    |> List.ofSeq

                [ yield! recursiveFiles; yield! rootFiles ]

            recursiveFolderFiles folder
            |> List.filter (fun s -> ignoreRegexes |> List.map String.matches |> List.map (fun f -> f s |> not) |> List.fold (&&) true)
            |> (function | [] -> Error "No local source files found" | j -> Ok j)

        let dashVersion =
            args 
            |> List.tryPick ( function | DashVersion v -> Some v | _ -> None )
            |> Option.defaultValue (defaultDashVersion)

        match maybeMetadata, maybeLocalFiles with 
        | Ok metadata, Ok localFiles -> 
            //This is all in asyncs to make handling async and IO operations easier, it doesn't actually have to run async
            let success, output, error = 
                performGeneration name shortName outputFolder dashVersion folder metadata localFiles 
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
    
    | Error error, _, _, _ 
    | _, Error error, _, _ 
    | _, _, Error error, _ 
    | _, _, _, Error error -> 
        printfn "Error: %s" error
        1

    

    //let checker = FSharpChecker.Create()

    //let sourcetext =
    //    """
    //    fun (testa, testb) testc -> true
    //    """
    //    |> SourceText.ofString

    //let projOptions, errors =
    //    checker.GetProjectOptionsFromScript("test.fs", sourcetext)
    //    |> Async.RunSynchronously

    //let parsingOptions, _errors = checker.GetParsingOptionsFromProjectOptions(projOptions)

    //// Run the first phase (untyped parsing) of the compiler
    //let parseFileResults =
    //    checker.ParseFile("test.fs", sourcetext, parsingOptions)
    //    |> Async.RunSynchronously

    //parseFileResults.ParseTree
    //|> printfn "%A"

    //0