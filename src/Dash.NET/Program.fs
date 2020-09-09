module Dash.NET.App

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

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title []  [ encodedText "Dash.NET" ]
                link [ _rel  "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let partial () =
        h1 [] [ encodedText "Dash.NET" ]

    let index (model : Message) =
        [
            partial()
            p [] [ encodedText model.Text ]
        ] |> layout

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (name : string) =
    let greetings = sprintf "Hello %s, from Giraffe!" name
    let model     = { Text = greetings }
    let view      = Views.index model
    htmlView Views.defaultIndex

open FSharp.Plotly
open DynObj

let testHeader = DynamicObj()

let testText = DynamicObj()

testText?children <- "Hello Dash from F#"

testHeader?("type") <- "H1"
testHeader?("namespace") <- "dash_html_components"
testHeader?props <- testText


//render an example chart from the FSharp.Plotly GenericChart type :

let presetAxis title = Axis.LinearAxis.init(Title=title,Mirror=StyleParam.Mirror.All,Ticks=StyleParam.TickOptions.Inside,Showgrid=false,Showline=true,Zeroline=true)

let applyPresetStyle xName yName chart = chart |> Chart.withX_Axis (presetAxis xName) |> Chart.withY_Axis (presetAxis yName)

let rndData = 
    let rnd = System.Random()
    [for i in [0 .. 100] do yield (rnd.NextDouble(),rnd.NextDouble())]

let testGraph =
    DCC.Graph.ofGenericChart(
        Chart.Point(rndData)
        |> applyPresetStyle "xAxis" "yAxis"
        |> Chart.withSize (1000.,1000.)
    )


let testLayout =

    HTMLComponents.div "testDiv" [
        box testHeader
        box (DCC.input "input-x" 1 "number")
        box (DCC.input "input-y" 1 "number")
        box (HTMLComponents.div "test-output" [box "Sum will be here"])
        box (testGraph |> DCC.Graph.toComponentJson)
    ]

let testCallbacks = [
    Callbacks.Callback.create 
        true 
        None
        [|
            Callbacks.Input.create ("input-x","value")
            Callbacks.Input.create ("input-y","value")
        |]
        "test-output.children"
        [||]
    ]
     
//this callback should add the values from the "input-x" and "input-x" components

let testCallbackHandler =
    Callbacks.CallbackHandler.create
        [|
            Callbacks.Input.create ("input-x","value")
            Callbacks.Input.create ("input-y","value")
        |]
        (Callbacks.Output.create ("test-output","children"))
        (fun (x:float) (y:float) -> sprintf "F# says:%f" (x+y))

let callbackMap = DynamicObj ()

callbackMap?("test-output.children") <- (testCallbackHandler |> Callbacks.CallbackHandler.pack)

let webApp =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler "world"

                //Dash enpoints
                route "/_dash-layout"       >=> json testLayout //Calls from Dash renderer for what components to render (must return serialized dash components)
                
                //example response: 
                //{
                //    "clientside_function": null, 
                //    "inputs": [
                //      {
                //        "id": "my-input1", 
                //        "property": "value"
                //      }
                //    ], 
                //    "output": "my-output1.children", 
                //    "prevent_initial_call": false, 
                //    "state": []
                //  }
                route "/_dash-dependencies" >=> json testCallbacks        //Serves callback bindings as json on app start.

                //Example response: 
                //{
                //  "files": [], 
                //  "hard": false, 
                //  "packages": [
                //    "dash_renderer", 
                //    "dash_html_components", 
                //    "dash_core_components"
                //  ], 
                //  "reloadHash": "4b31131566a240aa8f794e75e2fcb319"
                //}
                route "/_reload-hash"       >=> json obj        //This call is done when using hot reload.

                routef "/hello/%s" indexHandler
            ]

        POST >=> 
            choose [
                //calls from callbacks come in here.
                route "/_dash-update-component" 
                    >=> bindJson ( fun (cbRequest:Callbacks.CallbackRequest) -> 
                        //doing this statically typed will not be so easy
                        //let handler:Callbacks.CallbackHandler<_,_> = callbackMap?cbRequest.output
                        
                        let changedProps = cbRequest.changedPropIds //To-Do ordering of these tuples is important

                        let inputs = cbRequest.inputs |> Array.map (fun x -> box x.value) // To-Do: tuple generation from array?

                        let handler = callbackMap?(cbRequest.output)

                        let result = 
                            Callbacks.CallbackHandler.getResponseObject inputs (unbox handler)

                        json result
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