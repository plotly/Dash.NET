module TestUtils

open FSharp.Control.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.TestHost
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open System.Reflection
open System.Net.Http

open System
open System.IO

open Giraffe
open Dash.NET
open Dash.NET.Giraffe

open Expecto

type DashApp with

    static member getTestHost (config: DashGiraffeConfig) (app:DashApp) =

        //Go through an assembly and look for any types that inherit DashComponent
        //if one is found then look for the LoadableComponentDefinition and add its script
        let tryLoadComponents (innerApp: DashApp) (a: Assembly) =
            try
                a.GetModules()
                |> Array.collect (fun m -> m.GetTypes())
                |> Array.choose (fun t -> if t.IsSubclassOf typeof<DashComponent> then Some t else None)
                |> Array.choose (fun t -> t.GetProperty "definition" |> Option.ofObj)
                |> Array.choose (fun p -> p.GetValue(null,null) |> Option.ofObj)
                |> Array.choose tryUnbox
                |> Array.fold
                    (fun (acc: DashApp) (def: LoadableComponentDefinition) -> 
                        printfn "Loading component: %s" def.ComponentName
                        DashApp.appendScripts def.ComponentJavascript acc)
                    innerApp
            with
            | e -> innerApp

        //Get every assembly referenced by an assembly
        let getReferenced (a: Assembly) = a.GetReferencedAssemblies()

        //Call tryLoadComponents on an assembly and every assembly it references
        let rec loadAssemblies ((innerApp, loaded): DashApp * (string Set)) (toLoad: string) =
            if not(loaded.Contains toLoad) then
                try
                    let assembly = Assembly.Load(toLoad)

                    assembly 
                    |> getReferenced
                    |> List.ofArray 
                    |> List.map (fun a -> a.FullName)
                    |> List.fold 
                        loadAssemblies 
                        (tryLoadComponents innerApp assembly, loaded.Add toLoad)
                with
                | e -> 
                    printfn "Failed to load assembly: %s \n %O" toLoad e
                    innerApp, loaded
            else
                innerApp, loaded

        let loadedApp =
            Assembly.GetEntryAssembly().FullName
            |> loadAssemblies (app, Set.empty)
            |> fst

        //TODO: make this customizable
        let errorHandler (ex : Exception) (logger : ILogger) =
            logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
            clearResponse >=> setStatusCode 500 >=> (config.ErrorHandler ex)

        let configureCors (builder : CorsPolicyBuilder) =
            builder.WithOrigins(sprintf "http://%s:%d" config.HostName config.Port)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    |> ignore

        let configureApp (appBuilder : IApplicationBuilder) =
            let env = appBuilder.ApplicationServices.GetService<IWebHostEnvironment>()
            (match env.EnvironmentName with
            | "Development" -> appBuilder.UseDeveloperExceptionPage()
            | _ -> appBuilder.UseGiraffeErrorHandler(errorHandler))
                    .UseCors(configureCors)
                    .UseStaticFiles()
                    .UseGiraffe(DashApp.toHttpHandler loadedApp)

        let configureServices (services : IServiceCollection) =
            services.AddCors()    |> ignore
            services.AddGiraffe() |> ignore

            Common.Json.mkSerializerSettings()
            |> NewtonsoftJson.Serializer
            |> services.AddSingleton<Json.ISerializer>
            |> ignore
            
        let configureLogging (builder : ILoggingBuilder) =
            builder.AddFilter(fun l -> l.Equals config.LogLevel)
                    .AddConsole()
                    .AddDebug() |> ignore

        // This folder has to be "<path-to-exe>/WebRoot" for the way generated component javascript injection currently works
        let contentRoot = Reflection.Assembly.GetExecutingAssembly().Location |> Path.GetDirectoryName
        let webRoot     = Path.Combine(contentRoot, "WebRoot")

        WebHostBuilder()
                    
            .UseTestServer()
            .UseContentRoot(contentRoot)
            .UseWebRoot(webRoot)
            .Configure(Action<IApplicationBuilder> configureApp)
            .ConfigureServices(configureServices)
            .ConfigureLogging(configureLogging)

let testRequest (request : HttpRequestMessage) (config: DashGiraffeConfig) (app:DashApp) =
    let resp = task {
            
        use server = new TestServer(app |> DashApp.getTestHost config)
        use client = server.CreateClient()
        let! response = request |> client.SendAsync
        return response
    }
    resp.Result

let generateLayoutEndpointTest name message expected (layout:DashComponent) =
    
    let generateTestApp layout = 
        DashApp.initDefault()
        |> DashApp.withLayout layout
    
    let testConfig = {
        HostName = "localhost"
        IpAddress = "127.0.0.1"
        Port = 5000
        LogLevel = LogLevel.Debug
        ErrorHandler = (fun ex -> text ex.Message)
        }

    testCase name (fun _ ->
        let response = 
            layout
            |> generateTestApp
            |> testRequest (new HttpRequestMessage(HttpMethod.Get, "/_dash-layout")) testConfig

        let content = response.Content.ReadAsStringAsync().Result
        Expect.equal content expected message
    )