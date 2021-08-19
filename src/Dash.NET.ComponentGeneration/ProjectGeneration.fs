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
        //TODO: make sure dotnet 5.0 cli is installed
        //TODO: specify we are using dotnet 5 cli

        let outputPath = Path.Combine (outputFolder, name)
        let localFilesDirPath = Path.Combine (outputPath,"WebRoot","components",name)

        printfn "Creating project %s" name
        return!
            //Create project folder
            runFunctionAsync (fun () ->
                try
                    let _ = Directory.CreateDirectory(outputPath)
                    true, sprintf "Created directory %s" outputPath, ""
                with | ex ->
                    false, "", sprintf "Failed to create folder %s\n%s" outputPath (ex.ToString()))

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

            //Create project folder
            |@> runFunctionAsync (fun () ->
                try
                    let _ = Directory.CreateDirectory(localFilesDirPath)
                    true, sprintf "Created directory %s" localFilesDirPath, ""
                with | ex ->
                    false, "", sprintf "Failed to create folder %s\n%s" localFilesDirPath (ex.ToString()))

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
                            true, sprintf "Copied file %s to %s" localFileName localFilesDirPath, ""
                        else
                            false, "", sprintf "File %s does not exists" localFile
                    with | ex ->
                        false, "", sprintf "Failed to copy file %s to %s\n%s" localFile localFilesDirPath (ex.ToString()))
                |> List.reduce ( fun (s1, o1, e1) (s2, o2, e2) -> (s1 && s2, sprintf "%s\n%s" o1 o2, sprintf "%s\n%s" e1 e2) ))

    }
    //Uninstall the template again
    //Not doing this can cause weird conflicts if the template is ever updated
    |@!> runCommandAsync "." "dotnet" ["new"; "-u"; Path.GetFullPath(Path.Combine(thisPath, "template"))]

let buildProject (name: string) (path: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            printfn "Building project %s" name
            return! 
                runCommandAsync path "dotnet" ["build"; name]
        else
            return false, "", "The project does not exist"
    }

let packageProject (name: string) (path: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            printfn "Packaging project %s" name
            return! 
                runCommandAsync path "dotnet" ["pack"; name; "--configuration"; "Release"]
        else
            return false, "", "The project does not exist"
    }

let publishProject (name: string) (path: string) (version: string) (apiKey: string) =
    async {
        if (Directory.Exists (Path.Combine(path, name))) then
            printfn "Publishing project %s" name
            return! 
                runCommandAsync path "dotnet" 
                    [ "nuget"; "push"; Path.Combine(name, "bin", "Release", sprintf "%s.%s.nupkg" name version)
                      "--api-key"; apiKey
                      "--source"; "https://api.nuget.org/v3/index.json" ]
        else
            return false, "", "The project does not exist"
    }