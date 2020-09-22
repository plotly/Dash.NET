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
//Create a plotly graph component from a Plotly.NET chart object

open Plotly.NET
open Dash.NET

//set up and style the chart

let presetAxis title = Axis.LinearAxis.init(Title=title,Mirror=StyleParam.Mirror.All,Ticks=StyleParam.TickOptions.Inside,Showgrid=false,Showline=true,Zeroline=true)
let applyPresetStyle xName yName chart = chart |> Chart.withX_Axis (presetAxis xName) |> Chart.withY_Axis (presetAxis yName)

let rndData amnt = 
    let rnd = System.Random()
    [for i in [1 .. amnt] do yield (rnd.NextDouble(),rnd.NextDouble())]

//Generate the graph component

let testChart = 
    Chart.Point(rndData 1)
    |> applyPresetStyle "xAxis" "yAxis"
    |> Chart.withSize (1000.,1000.)


open Dash.NET.DCC_DSL
open Dash.NET.HTML_DSL
open HTMLPropTypes
open ComponentPropTypes

let dslLayout =
    Div.div "myDiv-1" [ClassName "I am A Div"] [
        Input.input "myInput-1" [
            Input.Type InputType.Text
            Input.ClassName "Hi"
            Input.Name "My Name Is"
            Input.Value "Slim Shady"
        ] []
        Div.div "myDiv-2" [ClassName "I am A Div"] [
        ]
        Input.input "graphChanger" [
            Input.Type InputType.Number
        ] []

        Div.div "GrapContainer" [] [
            Graph.graph "testGraph" [
                Graph.Figure (GenericChart.toFigure testChart)
                Graph.Config (GenericChart.getConfig testChart)
                Graph.Animate true
            ] []
        ]
    ]

let dslCallback1 =
    Callback.create
        [|
            CallbackInput.create ("myInput-1","value")
        |]
        (CallbackOutput.create ("myDiv-2","children"))
        (fun (i:string) -> 
            sprintf "You Typed:%s" i
        )    

let dslCallback2 =
    Callback.create
        [|
            CallbackInput.create ("graphChanger","value")
        |]
        (CallbackOutput.create ("testGraph","figure"))
        (fun (amnt:int64) -> 
            let amnt' = if (int amnt < 0) then 0 else (int amnt)
            let data = rndData amnt'
            Chart.Point(data)
            |> applyPresetStyle "xAxis" "yAxis"
            |> Chart.withSize (1000.,1000.)
            |> GenericChart.toFigure
        )

let myDashApp =
    DashApp.initDefault()
    |> DashApp.withLayout dslLayout
    |> DashApp.withCallbackHandler("myDiv-2.children",dslCallback1)
    |> DashApp.withCallbackHandler("testGraph.figure",dslCallback2)


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
        .UseGiraffe(DashApp.toWebApp myDashApp)

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