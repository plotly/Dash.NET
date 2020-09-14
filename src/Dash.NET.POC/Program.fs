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

// --------------------
// Set up the dash app components
// --------------------
//Create a plotly graph component from a FSharp.Plotly chart object

open FSharp.Plotly

//set up and style the chart

let presetAxis title = Axis.LinearAxis.init(Title=title,Mirror=StyleParam.Mirror.All,Ticks=StyleParam.TickOptions.Inside,Showgrid=false,Showline=true,Zeroline=true)
let applyPresetStyle xName yName chart = chart |> Chart.withX_Axis (presetAxis xName) |> Chart.withY_Axis (presetAxis yName)

let rndData amnt = 
    let rnd = System.Random()
    [for i in [1 .. amnt] do yield (rnd.NextDouble(),rnd.NextDouble())]

//Generate the graph component

let testGraph =
    DCC.Graph.ofGenericChart "testGraph" (
        Chart.Point(rndData 1)
        |> applyPresetStyle "xAxis" "yAxis"
        |> Chart.withSize (1000.,1000.)
    )

//define the layout of the dash app.
//All components could be generated to match the Giraffe.ViewEngine DSL for html elements.
let testLayout =

    HTMLComponents.div "testDiv" [
        box (HTMLComponents.title "test-title" [box "Hello Dash From F#!"])
        box (HTMLComponents.div "test-title2" [box "How many random points to render?"])
        box (DCC.input "test-input" 1 "number")
        box (testGraph |> DCC.Graph.toComponentJson)
    ]

//this callback should add the values from the "input-x" and "input-x" components
//The callback definition here resembles the @app.callback decorator
let testCallbackHandler =
    Callback.create
        [|
            Input.create ("test-input","value")
        |]
        (Output.create ("testGraph","figure"))
        (fun (amnt:int64) -> 
            let amnt' = if (int amnt < 0) then 0 else (int amnt)
            let data = rndData amnt'
            Chart.Point(data)
            |> applyPresetStyle "xAxis" "yAxis"
            |> Chart.withSize (1000.,1000.)
            |> DCC.PlotlyFigure.ofGenericChart
        )

let app =
    DashApp.initDefault()
    |> DashApp.withCallbackHandler("testGraph.figure",testCallbackHandler)

// ---------------------------------
// Web app
// ---------------------------------


let webApp =
    choose [
        GET >=>
            choose [
                //serve the index
                route "/" >=> htmlView (app |> DashApp.getIndexHTML)

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