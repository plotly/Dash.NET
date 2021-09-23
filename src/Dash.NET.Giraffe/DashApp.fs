namespace Dash.NET.Giraffe

open Giraffe
open Giraffe.ViewEngine
open Views
open Plotly.NET
open System 
open System.IO

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open System.Reflection
open Dash.NET
open Newtonsoft.Json

//Giraffe, Logging and ASP.NET specific
type DashGiraffeConfig = {
  HostName: string
  LogLevel: LogLevel
  ErrorHandler: Exception -> HttpHandler
}

type DashApp =
    {
        Index: IndexView
        Layout: DashComponent 
        Config: DashConfig
        Callbacks: CallbackMap
    }

    /// Returns a DashApp with all fields initialized with default values.
    static member initDefault() =
        {
            Index = IndexView.initDefault()
            Layout = DashComponent()
            Config = DashConfig.initDefault()
            Callbacks = CallbackMap()
        }

    /// Returns the result of applying an initializer function to a DashApp with all fields initialized with default values.
    static member initDefaultWith (initializer: DashApp -> DashApp) = DashApp.initDefault () |> initializer

    /// Returns a new DashApp with the original DashConfig replaced by the given DashConfig 
    static member withConfig (config: DashConfig) (app: DashApp) = 
        { app with 
            Config = config 
            Index = app.Index |> IndexView.withConfig config
        }

    /// Returns a new DashApp with the original IndexView replaced by the given IndexView 
    static member withIndex (index:IndexView) (app:DashApp) =
        { app with 
            Index = index |> IndexView.withConfig app.Config
        }

    /// Returns a new DashApp with a new IndexView created by applying the mapping function to the original IndexView.
    static member mapIndex (mapping: IndexView -> IndexView) (app:DashApp) =
        { app with 
            Index = mapping app.Index 
        }

    /// Returns a new DashApp with the given css source links appended to the original IndexView's css register as link tags
    static member appendCSSLinks (hrefs:seq<string>) (app:DashApp) =
        let tags = 
            hrefs
            |> Seq.map (fun href ->
                link [_rel "stylesheet"; _href href ; _crossorigin " "]
            )
        app
        |> DashApp.mapIndex (IndexView.appendCSSLinks tags)

    /// Returns a new DashApp with the given script source links appended to the original IndexView's script register as script tags
    static member appendScripts (sources:seq<string>) (app:DashApp) =
        let tags = 
            sources
            |> Seq.map (fun source ->
                script [_type "application/javascript"; _crossorigin " "; _src source] []
            )
        app
        |> DashApp.mapIndex (IndexView.appendScripts tags)

    /// Returns a new DashApp with the original Layout replaced by the given Layout 
    static member withLayout (layout:DashComponent) (app:DashApp) =
        { app with 
            Layout = layout
        }

    /// Returns a new DashApp with the given callback added to the original Callback register
    static member addCallback (callback: Callback<'Function>) (app: DashApp) =

        //To-Do: Maybe use copy utility for all direct calls to underlying DynamicObjs.
        { app with
            Callbacks =
                app.Callbacks
                |> CallbackMap.registerCallback callback
        }

    /// Returns a new DashApp with the given callbacks added to the original Callback register
    static member addCallbacks (callbacks: seq<(Callback<'Function>)>) (app: DashApp) =
        //To-Do: Maybe use copy utility for all direct calls to underlying DynamicObjs.
        { app with
            Callbacks =
                callbacks
                |> Seq.fold (fun cMap cHandler ->
                    cMap
                    |> CallbackMap.registerCallback cHandler) app.Callbacks
        }

    /// Returns the given DashApp's index DOM
    static member getIndexHTML (app:DashApp) =
        app.Index |> IndexView.toHTMLComponent

    /// Returns the DasApp as a http handler to use as Giraffe middleware in an asp.netcore pipeline. 
    ///
    /// This application will provide the following endpoints:
    ///
    /// GET / -> serves the DashApps IndexView (This is also where the Das renderer will render the layout)
    ///
    /// GET /_dash-layout -> serves the DashApp's layout as JSON
    ///
    /// GET /_dash-dependencies -> serves the serialized callback handlers registered in the DashApp's callback register as JSON
    ///
    /// GET /_reload-hash -> Not implemented, returns empty JSON object
    ///
    /// POST /_dash-update-component -> handles callback requests and returns serialized callback JSON responses.
    static member toHttpHandler (app:DashApp) : HttpHandler =
        choose [
            GET >=>
                choose [
                    //serve the index
                    route "/" >=> htmlView (app |> DashApp.getIndexHTML)

                    //Dash GET enpoints
                    route "/_dash-layout"       >=> json app.Layout        //Calls from Dash renderer for what components to render (must return serialized dash components)
                    route "/_dash-dependencies" >=> json (app.Callbacks |> CallbackMap.toDependencies) //Serves callback bindings as json on app start.
                    route "/_reload-hash"       >=> json obj               //This call is done when using hot reload.
                ]

            POST >=> 
                choose [
                    //Dash POST endpoints
                    route "/_dash-update-component" //calls from callbacks come in here.
                        >=> bindJson ( fun (cbRequest:CallbackRequest) -> 

                            let inputs = 
                                let inputs = cbRequest.Inputs |> Array.map (fun reqInput ->  reqInput.Value) //generate argument list for the callback
                                let states = 
                                    //Yes, this is ugly. I currently cant find a way to deserialize an empty array directly when the state property is missing in the JSON, but that sounds like a problem that is solvable.
                                    try
                                        cbRequest.State |> Array.map (fun reqInput -> reqInput.Value)
                                    with _ ->
                                        [||]
                                Array.append inputs states

                            printfn "Input Tokens: %A" inputs

                            let result = 
                                app.Callbacks
                                |> CallbackMap.getPackedCallbackById (cbRequest.Output) //get the callback from then callback map
                                |> Callback.getResponseObject inputs//evaluate the handler function and get the response to send to the client

                            json result //return serialized result of the handler function
                        )
                    
                ]
            setStatusCode 404 >=> text "Not Found" 
        ]

    static member run (args: string []) (config: DashGiraffeConfig) (app: DashApp) =
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
            builder.WithOrigins(sprintf "http://%s:5001" config.HostName)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   |> ignore

        let configureApp (appBuilder : IApplicationBuilder) =
            let env = appBuilder.ApplicationServices.GetService<IWebHostEnvironment>()
            (match env.EnvironmentName with
            | "Development" -> appBuilder.UseDeveloperExceptionPage()
            | _ -> appBuilder.UseGiraffeErrorHandler(errorHandler))
                   .UseHttpsRedirection()
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

        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                fun webHostBuilder ->
                    webHostBuilder
                        .UseContentRoot(contentRoot)
                        .UseWebRoot(webRoot)
                        .Configure(Action<IApplicationBuilder> configureApp)
                        .ConfigureServices(configureServices)
                        .ConfigureLogging(configureLogging)
                        |> ignore)
            .Build()
            .Run()
        0
    
