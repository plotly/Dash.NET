module Dash.NET.ComponentGeneration.ProjectGeneration

open System
open System.Diagnostics
open System.IO
open Prelude
open Serilog

let thisPath = Reflection.Assembly.GetExecutingAssembly().Location |> Path.GetDirectoryName

let runCommandAsync (log: Core.Logger) (workingDir: string) (fileName: string) (args: string list) = 
    async {
        let argString = args |> String.concat " "
        
        let startInfo = 
            ProcessStartInfo(
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = fileName,
                Arguments = argString,
                WorkingDirectory = workingDir
            )
        
        let outputs = System.Collections.Generic.List<string>()
        let errors = System.Collections.Generic.List<string>()
        let outputHandler f (_sender:obj) (args:DataReceivedEventArgs) = f args.Data
        
        let p = new Process(StartInfo = startInfo)
        
        p.OutputDataReceived.AddHandler(DataReceivedEventHandler (outputHandler outputs.Add))
        p.ErrorDataReceived.AddHandler(DataReceivedEventHandler (outputHandler errors.Add))
        
        let started, startedErr = 
            try
                p.Start(), ""
            with | ex ->
                ex.Data.Add("filename", fileName)
                false, ex.ToString()
        
        if not started then
            log.Error("Failed to start process \"{Process}\"", startInfo.ToString())
            log.Error("Error: {Error}", startedErr)
            return false
        else 
            p.BeginOutputReadLine()
            p.BeginErrorReadLine()

            let task = p.WaitForExitAsync()

            do! task |> Async.AwaitTask

            let getOutput l = 
                l 
                |> Seq.filter (fun o -> String.IsNullOrEmpty o |> not)
                |> String.concat "\n"

            if p.ExitCode = 0 then
                log.Debug("Ran process \"{Process}\"", fileName)
                log.Debug("Output: {ProcessOutput}", getOutput outputs)
                return true
            else
                log.Error("Failed to run process \"{Process}\"", fileName)
                log.Error("Output: {ProcessOutput}", getOutput outputs)
                log.Error("Error: {Error}", getOutput errors)
                return false
                
    }

let runFunctionAsync (func: unit -> bool) = async { return func() }

let createProject 
    (log: Core.Logger)
    (name: string) 
    (outputFolder: string) 
    (componentFolder: string) 
    (localFilePaths: string list) 
    (componentVersion: string) 
    (dashVersion: string) 
    (description: string) 
    (authors: string list) =

    //TODO this template may be better moved to nuget instead of being included with the tool?
    //Install template
    runCommandAsync log "." "dotnet" ["new"; "-i"; Path.Combine(thisPath, "template")]
    |@!> async {
        //TODO: make sure dotnet 5.0 cli is installed
        //TODO: specify we are using dotnet 5 cli

        let outputPath = Path.Combine (outputFolder, name)
        let localFilesDirPath = Path.Combine (outputPath,"WebRoot","components",name)

        log.Information("Creating project {ComponentName}", name)
        return!
            //Create project folder
            runFunctionAsync (fun () ->
                try
                    let _ = Directory.CreateDirectory(outputPath)
                    log.Debug("Created directory {ComponentProjectFolder}", outputPath)
                    true
                with | ex ->
                    log.Error(ex, "Failed to create directory {ComponentProjectFolder}", outputPath)
                    false)

            //Create project
            |@> runCommandAsync log outputPath "dotnet" 
                [ yield! [ "new"; "dashcomponent" ] 
                  yield! [ "--force" ]
                  yield! [ "-lang"; "F#" ]
                  yield! [ "-n"; name |> sprintf "\"%s\"" ]
                  yield! [ "--componentVersion"; componentVersion |> sprintf "\"%s\"" ]
                  yield! [ "--dashVersion"; dashVersion |> sprintf "\"%s\"" ]
                  yield! [ "--description"; description |> sprintf "\"%s\""] 
                  yield!
                      match authors with
                      | [] -> []
                      | [auth] -> [ "--authors"; auth |> sprintf "\"%s\"" ]
                      | auths -> [ "--authors"; auths |> List.reduce (sprintf "%s;%s") |> sprintf "\"%s\"" ] ]

            //Create project folder
            |@> runFunctionAsync (fun () ->
                try
                    let _ = Directory.CreateDirectory(localFilesDirPath)
                    log.Debug("Created directory {LocalFilesFolder}", localFilesDirPath)
                    true
                with | ex ->
                    log.Error(ex, "Failed to create directory {LocalFilesFolder}", localFilesDirPath)
                    false)

            //Copy local files
            |@> runFunctionAsync (fun () ->
                localFilePaths
                |> List.map (fun localFile ->
                    try
                        if File.Exists(localFile) then
                            let localFileName = Path.GetRelativePath (componentFolder, localFile)
                            let newJsPath = Path.Combine(localFilesDirPath, localFileName)
                            Directory.CreateDirectory(Path.GetDirectoryName newJsPath) |> ignore
                            File.Copy(localFile, newJsPath, true)
                            log.Debug("Copied file {LocalFile} to {LocalFilesFolder}", localFile, localFilesDirPath)
                            true
                        else
                            log.Error("Error, file {LocalFile} does not exist", localFile)
                            false
                    with | ex ->
                        log.Debug(ex, "Failed to copy file {LocalFile} to {LocalFilesFolder}", localFile, localFilesDirPath)
                        false)
                |> List.reduce (&&))

    }
    //Uninstall the template again
    //Not doing this can cause weird conflicts if the template is ever updated
    |@!> runCommandAsync log "." "dotnet" ["new"; "-u"; Path.GetFullPath(Path.Combine(thisPath, "template"))]

let buildProject (log: Core.Logger) (name: string) (path: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            log.Information("Building project {ComponentName}", name)
            return! 
                runCommandAsync log path "dotnet" ["build"; name]
        else
            log.Error("Error, the project {ComponentProjectFolder} does not exist", Path.Combine(path, name))
            return false
    }

let packageProject (log: Core.Logger) (name: string) (path: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            log.Information("Packaging project {ComponentName}", name)
            return! 
                runCommandAsync log path "dotnet" ["pack"; name; "--configuration"; "Release"]
        else
            log.Error("Error, the project {ComponentProjectFolder} does not exist", Path.Combine(path, name))
            return false
    }

let publishProject (log: Core.Logger) (name: string) (path: string) (version: string) (apiKey: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            log.Information("Publishing project {ComponentName}", name)
            return! 
                runCommandAsync log path "dotnet" 
                    [ "nuget"; "push"; Path.Combine(name, "bin", "Release", sprintf "%s.%s.nupkg" name version)
                      "--api-key"; apiKey
                      "--source"; "https://api.nuget.org/v3/index.json" ]
        else
            log.Error("Error, the project {ComponentProjectFolder} does not exist", Path.Combine(path, name))
            return false
    }