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


let testGraph =
    DCC.Graph.ofGenericChart(
        Chart.Point([1,2])
        |> Chart.withX_AxisStyle "xAxis"
        |> Chart.withY_AxisStyle "yAxis"
    )

let testLayout =

    HTMLComponents.Div.withChildren [
        box testHeader
        box (testGraph |> DCC.Graph.toComponentJson)
    ]



//{
//    "props": {
//        "children": [
//        {
//          "props": {"children": "Hello Dash"}, 
//          "type": "H1", 
//          "namespace": "dash_html_components"
//        }
//        ]
//    }, 
//    "type": "Div", 
//    "namespace": "dash_html_components"
//}

//Parameter[Name]

let webApp =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler "world"
                route "/_dash-layout" >=> json testLayout
                route "/_dash-dependencies" >=> json []
                routef "/hello/%s" indexHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]

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
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
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