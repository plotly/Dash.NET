open Dash.NET.ComponentGeneration
open ComponentParameters
open Prelude
open System.IO
open Argu
open System
open Serilog

// Hardcoded defaults
let defaultVersion = "1.0.0"
let defaultDashVersion = "0.1.0-alpha9"
let defaultIgnore = [ "__pycache__"; ".*\.py" ]

// Command line arguments
type CmdArgs =
    | [<AltCommandLine("-n"); First; Mandatory; Unique>] Name of name:string
    | [<AltCommandLine("-s"); Mandatory; Unique>] ShortName of name:string
    | [<AltCommandLine("-d"); Mandatory; Unique>] ComponentDirectory of folder:string
    | [<AltCommandLine("-e"); Mandatory; Unique>] Description of description:string
    | [<AltCommandLine("-a"); Mandatory>]Author of name:string
    | [<AltCommandLine("-m"); Unique>] Metadata of meta:string
    | [<AltCommandLine("-v"); Unique>] Version of version:string
    | [<AltCommandLine("-vd"); Unique>] DashVersion of version:string
    | [<AltCommandLine("-o"); Unique>] OutputDirectory of folder:string
    | [<AltCommandLine("-f")>] AddFile of source:string
    | [<AltCommandLine("-i")>] Ignore of regex:string
    | [<AltCommandLine("-ddi"); Unique>] DisableDefaultIgnore
    | [<AltCommandLine("-p"); Unique>] PublishToNuget of apiKey:string
    | [<Unique>] Verbose
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Name _ -> "Name of the component or group of components, required"
            | ShortName _ -> "Name of the exported javascript namespace of the component, required"
            | ComponentDirectory _ -> "Folder containing the component, required"
            | Description _ -> "A short description of the component, required"
            | Author _ -> "Author of the component, there can be more than one of these, at least one required"
            | Metadata _ -> "React docgen metadata.json file, defaults to '<component_folder>/metadata.json'"
            | Version _ -> "The version of the compenent, defaults to 1.0.0"
            | DashVersion _ -> "The version of Dash.NET to use, defaults to '0.1.0-alpha9'"
            | OutputDirectory _ -> "Directory to create the F# project folder in, defaults to ./"
            | AddFile _ -> "Additional local source file to include, there can be more than one of these, defaults to none"
            | Ignore _ -> "Ignore folders and file paths that match this regex, by default this includes \"__pycache__\" and \".*\.py\", there can be more than one of these, defaults to none"
            | DisableDefaultIgnore -> "Don't ignore \"__pycache__\" and \".*\.py\" by default"
            | PublishToNuget _ -> "Publish this package straight to nuget with this API key, defaults to not publishing. Can be published later using \"dotnet nuget push <path-to-component>/bin/Release/<component-name>.<component-version>.nupkg --api-key <api-key> --source https://api.nuget.org/v3/index.json\""
            | Verbose -> "Print all logs"

let performGeneration 
    (log: Core.Logger)
    (componentProjectName: string) 
    (componentShortName: string) 
    (outputFolder: string) 
    (componentVersion: string)
    (dashVersion: string) 
    (componentFolder: string) 
    (metaFile: string) 
    (localFiles: string list) 
    (description: string) 
    (authors: string list)
    (maybePublishingKey: string option) =
    async {
        do! ProjectGeneration.checkDotnetVersion log

        let localJavascript =
            localFiles
            |> List.filter (Path.GetExtension >> (fun s -> s.ToLowerInvariant()) >> (=) ".js")
            |> List.map (fun js -> Path.GetRelativePath(componentFolder, js))
            |> List.map (fun js -> Path.Combine("components", componentProjectName, js))

        log.Debug("Javascript files pulled from local files: {LocalJavascriptFiles}", localJavascript)

        let maybeComponentMetadata =
            metaFile
            |> File.ReadAllText
            |> ReactMetadata.jsonDeserialize

        match maybeComponentMetadata with
        | Ok componentMetadata ->
            let parametersList = ComponentParameters.fromReactMetadata log componentShortName localJavascript componentMetadata

            // Create the F# project
            let projectCreate =
                ProjectGeneration.createProject 
                    log
                    componentProjectName
                    outputFolder
                    componentFolder
                    localFiles
                    componentVersion
                    dashVersion
                    description
                    authors

            let projectFolder = Path.Combine(outputFolder, componentProjectName)

            log.Debug("Dotnet project folder set to {ComponentProjectFolder}", projectFolder)

            let projectResult =
                // Generate the F# bindings
                parametersList
                |> List.fold (fun state parameters -> 
                    state
                    |@> (parameters
                            |> ASTGeneration.createComponentAST log
                            |> ASTGeneration.generateCodeFromAST log (Path.Combine(projectFolder, parameters.ComponentFSharp)) ))
                    projectCreate

                // Build the project
                |@> ProjectGeneration.buildProject log componentProjectName outputFolder
                |@> ProjectGeneration.packageProject log componentProjectName outputFolder

            
            match maybePublishingKey with
            | Some apiKey -> 
                log.Debug("Nuget publishing key found")
                return!
                    projectResult
                    // Publish the project
                    |@> ProjectGeneration.publishProject log componentProjectName outputFolder componentVersion apiKey
            | None -> 
                log.Debug("Nuget publishing key not found")
                let! ret = projectResult
                log.Information("Skipping publishing")
                return ret

        | Error err ->
            log.Error(err, "Exception while deserializing metadata json")
            return false
    }

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CmdArgs>(errorHandler = errorHandler)

    let args = 
        parser
            .ParseCommandLine(argv)
            .GetAllResults()

    let isVerbose = args |> List.contains Verbose
    let log = 
        new LoggerConfiguration()
        |> (fun l -> 
            match isVerbose with 
            | true -> l.MinimumLevel.Debug()
            | false -> l.MinimumLevel.Information())
        |> (fun l -> l.WriteTo.Console().CreateLogger())

    log.Debug("Verbose logging enabled")

    // Mandatory, no defaulted
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

    let maybeFolder = 
        args 
        |> List.tryPick ( function | ComponentDirectory f -> Some f | _ -> None )
        |> Option.map (fun f -> if Directory.Exists f then Ok f else Error (sprintf "Folder %s does not exist" f))
        |> Option.defaultValue (Error "Missing argument: folder")

    let maybeDescription = 
        args 
        |> List.tryPick ( function | Description f -> Some f | _ -> None )
        |> Option.map Ok
        |> Option.defaultValue (Error "Missing argument: description")

    let maybeAuthors = 
        args 
        |> List.choose ( function | Author f -> Some f | _ -> None )
        |> function
        | [] -> Error "Missing argument: author"
        | a -> Ok a

    let maybePublishingKey = args |> List.tryPick ( function | PublishToNuget f -> Some f | _ -> None )

    // Mandatory, defaulted
    let maybeOutputFolder = 
        args 
        |> List.tryPick ( function | OutputDirectory f -> Some f | _ -> None )
        |> Option.map (fun f -> if Directory.Exists f then Ok f else Error (sprintf "Folder %s does not exist" f))
        |> Option.defaultValue (Ok ".")

    let componentVersion = 
        args 
        |> List.tryPick ( function | Version f -> Some f | _ -> None )
        |> Option.defaultValue (defaultVersion)

    let dashVersion =
        args 
        |> List.tryPick ( function | DashVersion v -> Some v | _ -> None )
        |> Option.defaultValue (defaultDashVersion)

    let ignoreRegexes = 
        args 
        |> List.choose ( function | Ignore m -> Some m | _ -> None )
        |> (if args |> List.contains DisableDefaultIgnore then id else (@) defaultIgnore)

    match maybeName, maybeShortName, maybeFolder, maybeOutputFolder, maybeDescription, maybeAuthors with
    | Ok name, Ok shortName, Ok folder, Ok outputFolder, Ok description, Ok authors ->

        log.Debug("Component name set to {ComponentName}", name)
        log.Debug("Component short name set to {ComponentShortName}", shortName)
        log.Debug("Component version set to {ComponentVersion}", componentVersion)
        log.Debug("Dash version set to {DashVersion}", dashVersion)
        log.Debug("Component description set to {ComponentDescription}", description)
        log.Debug("Component authors set to {@ComponentAuthors}", authors)
        log.Debug("Component set to component in {ComponentFolder}", folder)
        log.Debug("Working directory set to {OutputFolder}", outputFolder)
        
        let maybeMetadata =
            args 
            |> List.tryPick ( function | Metadata m -> Some m | _ -> None )
            |> Option.defaultValue (Path.Combine(folder, "metadata.json"))
            |> (fun m -> if File.Exists m then Ok m else Error (sprintf "Metadata file %s does not exist" m))

        // Search the folder structure for all of the files in the component
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
            // Ignore files we dont care about (eg. python dash bindings)
            |> List.filter (fun s -> ignoreRegexes |> List.map String.matches |> List.map (fun f -> f s |> not) |> List.fold (&&) true)
            |> (function | [] -> Error "No local source files found" | j -> Ok j)

        match maybeMetadata, maybeLocalFiles with 
        | Ok metadata, Ok localFiles -> 

            log.Debug("Metadata file set to {MetadataFile}", metadata)
            log.Debug("Ignored files include the regexes {@IgnoredFileRegexes}", ignoreRegexes)
            log.Debug("Local files found: {@LocalFiles}", localFiles)

            // Run the actual generation
            let success = 
                performGeneration log name shortName outputFolder componentVersion dashVersion folder metadata localFiles description authors maybePublishingKey
                // This is all in asyncs to make handling async and IO operations easier, it doesn't actually have to run async
                |> Async.RunSynchronously

            if success then 
                //TODO: add in convinient build / publish scripts to template to make republishing easier?
                log.Information("")
                log.Information("Successfully created component {ComponentName}!", name)
                log.Information("")
                log.Information("If you make changes to the generated code please be sure to run \"dotnet pack {ComponentName} --configuration Release\"", name)
                log.Information("Additionally if you are re-publishing a new version remember to update the version number in the {ComponentName}.fsproj", name)
                log.Information("")

                match maybePublishingKey with
                | Some _ ->
                    log.Information("You can re-publish the component later with the command \"dotnet nuget push <path-to-component>/bin/Release/{ComponentName}.<component-version>.nupkg --api-key <api-key> --source https://api.nuget.org/v3/index.json\"", name)
                | None ->
                    log.Information("You can publish the component later with the command \"dotnet nuget push <path-to-component>/bin/Release/{ComponentName}.<component-version>.nupkg --api-key <api-key> --source https://api.nuget.org/v3/index.json\"", name)
                0
            else
                log.Information("")
                log.Error("Component generation failed!")
                1

        | Error error, _ 
        | _, Error error -> 
            log.Error("Error: {Error}", error)
            log.Information("")
            log.Error("Component generation failed!")
            1

    | Error error, _, _, _, _, _ 
    | _, Error error, _, _, _, _ 
    | _, _, Error error, _, _, _ 
    | _, _, _, Error error, _, _
    | _, _, _, _, Error error, _
    | _, _, _, _, _, Error error -> 
        log.Error("Error: {Error}", error)
        log.Information("")
        log.Error("Component generation failed!")
        1
    
    