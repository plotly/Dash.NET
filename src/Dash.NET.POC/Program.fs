module Dash.NET.POC.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Giraffe.ModelBinding
open Dash.NET

// ---------------------------------
// Models
// ---------------------------------

type Message =
    {
        Text : string
    }

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open GiraffeViewEngine
    open Dash

    // ---------------------------------
    // HTML backbone of the Dash application
    // should be refactored into a core library
    // ---------------------------------

    let createConfigScript (config:DashConfig) =
        let innerJson = Newtonsoft.Json.JsonConvert.SerializeObject config
        script [ _id "_dash-config"; _type "application/javascript"] [rawText innerJson]

    let defaultRenderer = rawText """var renderer = new DashRenderer();"""
    
    let createRendererScript renderer =
        script [ _id "_dash-renderer"; _type "application/javascript"] [renderer]

    let createFaviconLink path =
        link [
            _rel "icon"
            _type "image/x-icon"
            _href path
        ]

    let defaultAppEntry = 
        div [_id "react-entry-point"] [
            div [_class "_dash-loading"] [
                encodedText "Loading..."
            ]
        ]

    let dashCDNScripts = [
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/react@16.13.0/umd/react.development.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/react-dom@16.13.0/umd/react-dom.development.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/@babel/polyfill@7.8.7/dist/polyfill.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/prop-types@15.7.2/prop-types.js"] []
                                               
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/dash-renderer@1.7.0/dash_renderer/dash_renderer.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/dash-core-components@1.11.0/dash_core_components/dash_core_components.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://cdn.jsdelivr.net/npm/dash-html-components@1.1.0/dash_html_components/dash_html_components.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://cdn.plot.ly/plotly-latest.min.js"] []
    ]

    let createIndex metas appTitle faviconPath css appEntry config scripts renderer = 
        html [] [
            head [] [
                yield! metas
                title [] [encodedText appTitle]
                createFaviconLink faviconPath
                yield! css
            ]
            body [] [
                appEntry
                footer [] [
                    createConfigScript config
                    yield! scripts
                    createRendererScript defaultRenderer
                ]
            ]
        ]

    let defaultIndex = 
        createIndex
            []
            "Dash.NET"
            "_favicon.ico"
            []
            defaultAppEntry
            Defaults.defaultConfig
            dashCDNScripts
            defaultRenderer



// --------------------
// Set up the dash app components
// --------------------


//Create a plotly graph component from a FSharp.Plotly chart object
open FSharp.Plotly

//set up and style the chart

let presetAxis title = Axis.LinearAxis.init(Title=title,Mirror=StyleParam.Mirror.All,Ticks=StyleParam.TickOptions.Inside,Showgrid=false,Showline=true,Zeroline=true)
let applyPresetStyle xName yName chart = chart |> Chart.withX_Axis (presetAxis xName) |> Chart.withY_Axis (presetAxis yName)

let rndData = 
    let rnd = System.Random()
    [for i in [0 .. 100] do yield (rnd.NextDouble(),rnd.NextDouble())]

//Generate the graph component

let testGraph =
    DCC.Graph.ofGenericChart(
        Chart.Point(rndData)
        |> applyPresetStyle "xAxis" "yAxis"
        |> Chart.withSize (1000.,1000.)
    )


//define the layout of the dash app.
//All components could be generated to match the Giraffe.ViewEngine DSL for html elements.
let testLayout =

    HTMLComponents.div "testDiv" [
        box (HTMLComponents.title "test-title" [box "Hello Dash From F#!"])
        box (DCC.input "input-x" 1 "number")
        box (DCC.input "input-y" 1 "number")
        box (HTMLComponents.div "test-output" [box "Sum will be here"])
        box (testGraph |> DCC.Graph.toComponentJson)
    ]


//define the callbacks to serve on app load via _dash-dependencies
let testCallbacks = [
    DashDependency.create 
        true 
        None
        [|
            Input.create ("input-x","value")
            Input.create ("input-y","value")
        |]
        "test-output.children"
        [||]
    ]
     
//this callback should add the values from the "input-x" and "input-x" components
//The callback definition here resembles the @app.callback decorator
let testCallbackHandler =
    Callback.create
        [|
            Input.create ("input-x","value")
            Input.create ("input-y","value")
        |]
        (Output.create ("test-output","children"))
        (fun (x:float) (y:float) -> sprintf "F# says:%f" (x+y))

let app =
    DashApp.initDefault()
    |> DashApp.withCallbackHandler("test-output.children",testCallbackHandler)


//Set upc a callback map to store all callbacks
//This should be a proper type.
let callbackMap = 
    CallbackMap ()
    |> CallbackMap.registerCallback "test-output.children" testCallbackHandler


// ---------------------------------
// Web app
// ---------------------------------


let webApp =
    choose [
        GET >=>
            choose [
                //serve the index
                route "/" >=> htmlView Views.defaultIndex

                //Dash GET enpoints
                route "/_dash-layout"       >=> json testLayout     //Calls from Dash renderer for what components to render (must return serialized dash components)
                route "/_dash-dependencies" >=> json app.Dependencies  //Serves callback bindings as json on app start.
                route "/_reload-hash"       >=> json obj            //This call is done when using hot reload.
            ]

        POST >=> 
            choose [
                //Dash POST endpoints
                route "/_dash-update-component" //calls from callbacks come in here.
                    >=> bindJson ( fun (cbRequest:CallbackRequest) -> 

                        let inputs = cbRequest.Inputs |> Array.map (fun reqInput -> box reqInput.Value) //generate argument list for the callback

                        let result = 
                            app.Callbacks
                            |> CallbackMap.getPackedCallbackById (cbRequest.Output) //get the callback from then callback map
                            |> Callback.getResponseObject inputs //evaluate the handler function and get the response to send to the client

                        json result //return serialized result of the handler function
                    )
                
            ]
        setStatusCode 404 >=> text "Not Found" 
    ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.EnvironmentName with
    | "Development" -> app.UseDeveloperExceptionPage()
    | _ -> app.UseGiraffeErrorHandler(errorHandler))
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Debug)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
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