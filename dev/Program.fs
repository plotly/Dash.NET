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
open Plotly.NET

open Dash.NET.DCC
open Dash.NET.HTML
open HTMLPropTypes
open ComponentPropTypes

let createWorldHighlightFigure countryName =
    Chart.ChoroplethMap(locations=[countryName],z=[100],Locationmode = StyleParam.LocationFormat.CountryNames)
    |> Chart.withMapStyle(
        ShowLakes=true,
        ShowOcean=true,
        OceanColor="lightblue",
        ShowRivers=true
    )
    |> Chart.withSize (1000.,1000.)
    |> GenericChart.toFigure

let dslLayout = 
    Div.div [] [
        Input.input "test-input" [Input.Type InputType.Text; Input.Value "Germany"] []
        Div.div [Custom("Id",box"test-output")] []
        Div.div [Custom("Id",box"test-output2")] []
    ]
    //Div.div [ClassName "section"] [
    //    H1.h1 [ClassName "title has-text-centered"] [str "Hello Dash from F#"]
    //    Div.div [ClassName "container"] [
    //        H4.h4 [] [str "type a country to highlight"]
    //        Input.input "country-selection" [
    //            Input.ClassName "input is-primary"
    //            Input.Type InputType.Text
    //            Input.Value "Germany"
    //            Input.Debounce true
    //        ] []
    //    ]
    //    Div.div [ClassName "container"] [
    //        H4.h4 [] [str "Or search for a country here"]
    //        Dropdown.dropdown "dropdown-test" [
    //            Dropdown.Searchable true
    //            Dropdown.Multi false
    //            Dropdown.Placeholder "Type to search for suggestions"
    //            Dropdown.Options [
    //                DropdownOption.create "Germany" "Germany" false "Germany"
    //                DropdownOption.create "Canada" "Canada" false "Canada"
    //            ]
    //        ] []
                
    //    ]
    //    Div.div [ClassName "container"] [
    //        Graph.graph "grapherino" [
    //            Graph.Figure (createWorldHighlightFigure "Germany")
    //        ] []
    //    ]
    //]

let testInputCallback =
    Callback.create
        [|CallbackInput.create("test-input","value")|]
        [|
            CallbackOutput.create("test-output","children")
            CallbackOutput.create("test-output2","children")
        |]
        true
        (fun (x:string) -> [|x;x|])

let countryHighlightCallback =
    Callback.create
        [|CallbackInput.create("country-selection","value")|]
        [|CallbackOutput.create("grapherino","figure")|]
        true
        createWorldHighlightFigure
        
let countryHighlightDropdownCallback =
    Callback.create
        [|CallbackInput.create("dropdown-test","value")|]
        [|(CallbackOutput.create("grapherino","figure"))|]
        true
        createWorldHighlightFigure

let myDashApp =
    DashApp.initDefault()
    |> DashApp.withLayout dslLayout
    |> DashApp.addCSSLinks [
        "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css"
    ]
    |> DashApp.withCallbackHandler testInputCallback

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