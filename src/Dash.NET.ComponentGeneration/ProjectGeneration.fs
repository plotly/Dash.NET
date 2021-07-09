module Dash.NET.ComponentGeneration.ProjectGeneration

open System
open System.Diagnostics
open System.IO

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

let (|@>) (c1: Async<bool*string*string>) (c2: Async<bool*string*string>) =
    async {
        let! success, o, e = c1
        if success then
            let! newSuccess, u, r = c2
            return newSuccess, sprintf "%s\n%s" o u, sprintf "%s\n%s" e r
        else
            return success, o, e
    }

let createProject (name: string) =
    async {
        //TODO: make sure dotnet 5.0 cli is installed
        //TODO: specify we are using dotnet 5 cli

        //TODO: nuget CLI to allow for publishing and to add Dash.NET

        printfn "Creating project..."
        return!
            //This is slightly easier then creating a new template, because we dont have to check if it is installed
            runCommandAsync "." "dotnet" ["new"; "classlib"; "--force"; "-lang"; "F#"; "-n"; name]
            |@> runCommandAsync "." "dotnet" ["remove"; name; "reference"; "Library.fs"]
            |@> runFunctionAsync (fun () ->
                let fileName = sprintf "%s/Library.fs" name
                try
                    File.Delete(fileName) 
                    true, sprintf "Deleted file %s" fileName, ""
                with | ex ->
                    false, "", sprintf "Failed to delete file %s\n%s" fileName (ex.ToString()))
    }

//let addComponentToProject

//let publishProject

