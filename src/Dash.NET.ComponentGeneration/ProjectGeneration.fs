﻿module Dash.NET.ComponentGeneration.ProjectGeneration

open System
open System.Diagnostics
open System.IO
open Prelude
open Serilog

let thisPath = Reflection.Assembly.GetExecutingAssembly().Location |> Path.GetDirectoryName

let runCommandWithOutputAsync (workingDir: string) (fileName: string) (args: string list) = 
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
            Log.Error("Failed to start process \"{Process}\"", startInfo.ToString())
            Log.Error("Error: {Error}", startedErr)
            return false, "", startedErr
        else 
            p.BeginOutputReadLine()
            p.BeginErrorReadLine()

            let task = p.WaitForExitAsync()

            do! task |> Async.AwaitTask

            let getOutput l = 
                l 
                |> Seq.filter (fun o -> String.IsNullOrEmpty o |> not)
                |> String.concat "\n"

            let output = getOutput outputs
            let error = getOutput errors

            if p.ExitCode = 0 then
                Log.Debug("Ran process \"{Process}\"", fileName)
                Log.Debug("Output: {ProcessOutput}", output)
                Log.Debug("Error: {Error}", error)
                return true, output, error
            else
                Log.Error("Failed to run process \"{WorkingDir}\" \"{Process}\" \"{Arguments}\"", workingDir, fileName, argString)
                Log.Error("Output: {ProcessOutput}", output)
                Log.Error("Error: {Error}", error)
                return false, output, error
                
    }

let runCommandAsync (workingDir: string) (fileName: string) (args: string list) = 
    async.Bind(runCommandWithOutputAsync workingDir fileName args, (fun (s,_,_) -> async {return s}))

let runFunctionAsync (func: unit -> bool) = async { return func() }

let checkDotnetVersion () =
    // Conveniently "dotnet --version" ONLY outputs a version number, so we can take advantage of this
    // since this isn't a very robust way of checking compatability we will only give a warning if it isn't .Net 5
    async {
        let! _,o,_ = runCommandWithOutputAsync "." "dotnet" ["--version"]
        if o |> String.matches "^5" |> not then
            Log.Warning("This tool was built to use the .NET 5 CLI, an unexpected version of the CLI was detected and as a result this application may not work as expected.")
    }

let createProject 
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
    runCommandAsync "." "dotnet" ["new"; "-i"; Path.Combine(thisPath, "template")]
    |@!> async {
        let outputPath = Path.Combine (outputFolder, name)
        let localFilesDirPath = Path.Combine (outputPath,"WebRoot","components")
        let generatedCodeDirPath = Path.Combine (outputPath,"Generated")

        let tryCreateDirectory logDesc path =
            try
                path |> Directory.CreateDirectory |> ignore
                Log.Debug(sprintf "Created directory {%s}" logDesc, path)
                true
            with | ex ->
                Log.Error(ex, sprintf "Failed to create directory {%s}" logDesc, path)
                false

        Log.Information("Creating project {ComponentName}", name)
        return!
            //Create project folder
            runFunctionAsync (fun () ->
                outputPath |> tryCreateDirectory "ComponentProjectFolder"
            )
            //Create project
            |@> runCommandAsync outputPath "dotnet" 
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

            //Create local files folder
            |@> runFunctionAsync (fun () ->
                localFilesDirPath |> tryCreateDirectory "LocalFilesFolder"
            )
            //Create generated code files folder
            |@> runFunctionAsync (fun () ->
                generatedCodeDirPath |> tryCreateDirectory "GeneratedCodeFolder"
            )

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
                            Log.Debug("Copied file {LocalFile} to {LocalFilesFolder}", localFile, localFilesDirPath)
                            true
                        else
                            Log.Error("Error, file {LocalFile} does not exist", localFile)
                            false
                    with | ex ->
                        Log.Debug(ex, "Failed to copy file {LocalFile} to {LocalFilesFolder}", localFile, localFilesDirPath)
                        false)
                |> List.reduce (&&))

    }
    //Uninstall the template again
    //Not doing this can cause weird conflicts if the template is ever updated
    |@!> runCommandAsync "." "dotnet" ["new"; "-u"; Path.GetFullPath(Path.Combine(thisPath, "template"))]

let buildProject (name: string) (path: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            Log.Information("Building project {ComponentName}", name)
            return! 
                runCommandAsync path "dotnet" ["build"; name]
        else
            Log.Error("Error, the project {ComponentProjectFolder} does not exist", Path.Combine(path, name))
            return false
    }

let packageProject (name: string) (path: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            Log.Information("Packaging project {ComponentName}", name)
            return! 
                runCommandAsync path "dotnet" ["pack"; name; "--configuration"; "Release"]
        else
            Log.Error("Error, the project {ComponentProjectFolder} does not exist", Path.Combine(path, name))
            return false
    }

let publishProject (name: string) (path: string) (version: string) (apiKey: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            Log.Information("Publishing project {ComponentName}", name)
            return! 
                runCommandAsync path "dotnet" 
                    [ "nuget"; "push"; Path.Combine(name, "bin", "Release", sprintf "%s.%s.nupkg" name version)
                      "--api-key"; apiKey
                      "--source"; "https://api.nuget.org/v3/index.json" ]
        else
            Log.Error("Error, the project {ComponentProjectFolder} does not exist", Path.Combine(path, name))
            return false
    }