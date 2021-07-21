module Dash.NET.ComponentGeneration.ProjectGeneration

open System
open System.Diagnostics
open System.IO
open Prelude

let thisPath = Reflection.Assembly.GetExecutingAssembly().Location |> Path.GetDirectoryName

let runCommandAsync (workingDir: string) (fileName: string) (args: string list) = 
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
            return (false, "", sprintf "Failed to start process %s\n%s" fileName startedErr)
        else 
            p.BeginOutputReadLine()
            p.BeginErrorReadLine()

            let task = p.WaitForExitAsync()

            do! task |> Async.AwaitTask

            let getOutput l = 
                l 
                |> Seq.filter (fun o -> String.IsNullOrEmpty o |> not)
                |> String.concat "\n"
            return (p.ExitCode = 0, getOutput outputs, getOutput errors)
    }

let runFunctionAsync (func: unit -> bool*string*string) = async { return func() }

let createProject (name: string) (fsFile: string) (jsFilePath:string) (dashVersion: string) =
    //TODO this template may be better moved to nuget instead of being included with the tool?
    //Install template
    runCommandAsync "." "dotnet" ["new"; "-i"; Path.Combine(thisPath, "componentTemplate")]
    |@!> async {
        //TODO: make sure dotnet 5.0 cli is installed
        //TODO: specify we are using dotnet 5 cli

        //TODO: nuget CLI to allow for publishing and to add Dash.NET

        if (File.Exists jsFilePath) then

            let jsFile = Path.GetFileName jsFilePath

            printfn "Creating project %s" name
            return!
                //Create project folder
                runFunctionAsync (fun () ->
                    try
                        let _ = Directory.CreateDirectory(name)
                        true, sprintf "Created directory %s" name, ""
                    with | ex ->
                        false, "", sprintf "Failed to copy file %s to %s\n%s" jsFile name (ex.ToString()))

                //Create project
                |@> runCommandAsync name "dotnet" 
                    [ "new"; "dashcomponent"
                      "--force" 
                      "-lang"; "F#"
                      "-n"; name
                      "--componentFile"; fsFile
                      "--componentJavascript"; jsFile
                      "--dashVersion"; dashVersion ]

                //Copy Js file
                |@> runFunctionAsync (fun () ->
                    try
                        let newJsPath = Path.Combine(name, jsFile)
                        if not (File.Exists(newJsPath)) then
                            File.Copy(jsFilePath, newJsPath)
                            true, sprintf "Copied file %s to %s" jsFile name, ""
                        else
                            true, sprintf "File %s already exists" newJsPath, ""
                    with | ex ->
                        false, "", sprintf "Failed to copy file %s to %s\n%s" jsFile name (ex.ToString()))

                    

        else 
            return false, "", "One of the specified paths does not exist"
    }
    //Uninstall the template again
    //Not doing this can cause weird conflicts if the template is ever updated
    |@!> runCommandAsync "." "dotnet" ["new"; "-u"; Path.GetFullPath(Path.Combine(thisPath, "componentTemplate"))]

let buildProject (name: string) =
    async {
        if (Directory.Exists name) then
            printfn "Building project %s" name
            return! runCommandAsync "." "dotnet" ["build"; name]
        else
            return false, "", "The project does not exist"
    }

//let publishProject

