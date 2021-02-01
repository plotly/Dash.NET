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

//----------------------------------------------------------------------------------------------------
//============================================== LAYOUT ==============================================
//----------------------------------------------------------------------------------------------------

//The layout describes the components that Dash will render for you. 
open Dash.NET.HTML // this namespace contains the standard html copmponents, such as div, h1, etc.
open Dash.NET.DCC  // this namespace contains the dash core components, the heart of your Dash.NET app.

open HTMLPropTypes
open ComponentPropTypes

type A = {B:string}

//Note that this layout uses css classes defined by Bulma (https://bulma.io/), which gets defined as a css dependency in the app section below.
let dslLayout = 
    Div.div [ClassName "section"] [ //the style for 'main-section' is actually defined in a custom css that you can serve with the dash app.
        Dropdown.dropdown "testInput1" [
            Dropdown.Options [
                DropdownOption.create "1" "1" false "1"
                DropdownOption.create 2 2 false "2"
                DropdownOption.create 3L 3L false "3"
                DropdownOption.create 4.1 4.1 false "4.1"
            ]
            Dropdown.Multi true
        ] []
        Label.label [] [str "selected values:"]
        Div.div [Id "output-1"] []
        Button.button [ClassName "button is-primary"; Id "testInput2"] [str "Click ME!"]
        Br.br [] []
        Label.label [] [str "Number of clicks:"]
        Div.div [Id "output-2"] []
        Input.input "testInput3" [
            Input.Value """{"B":"hallo"}"""
        ] []
        Button.button [ClassName "button is-primary"; Id "testInput4"] [str "Click ME!"]
        Div.div [Id "output-3"] []
    ]



//----------------------------------------------------------------------------------------------------
//============================================= Callbacks ============================================
//----------------------------------------------------------------------------------------------------

//Callbacks define how your components can be updated and update each other. A callback has one or 
//more Input components (defined by their id and the property that acts as input) and an output 
//component (again defined by its id and output property). Additionally, a function that handles the 
//input and returns the desired output is needed.

open Newtonsoft.Json
open Newtonsoft.Json.Linq

let callbackArrayInput =
    Callback(
        Inputs = [
            CallbackInput.create("testInput1","value")
        ],
        Output = CallbackOutput.create("output-1","children"),
        //HandlerFunction=(fun (x:float[]) -> sprintf "%A" x)
        //HandlerFunction=(fun (x:JArray) -> sprintf "%A" (x.ToObject<int[]>()))
        HandlerFunction = (fun (x:int[]) -> sprintf "%A" x)
    )

let clickInput =
    Callback(
        Inputs = [
            CallbackInput.create("testInput2","n_clicks")
        ],
        Output = CallbackOutput.create("output-2","children"),
        //HandlerFunction=(fun (x:int) -> sprintf "%A" x)
        HandlerFunction=(fun (x:int) -> sprintf "%A" x)
    )

let multiOutput (a:(string*obj) list) = a

let callbackArrayInput3 =
    Callback(
        Inputs = [
            CallbackInput.create("testInput4","n_clicks")
        ],
        Outputs = [
            CallbackOutput.create("output2","children")
            CallbackOutput.create("output","children")
            ],
        State = [CallbackState.create("testInput3","value")],
        //HandlerFunction=(fun (x:float[]) -> sprintf "%A" x)
        //HandlerFunction=(fun (x:JArray) -> sprintf "%A" (x.ToObject<int[]>()))
        HandlerFunction = (fun (clicks:int64) (x:A) -> 
            multiOutput [
                "output","children",box 2
                "output2","children", box "hallo"
            ]
        )
    )

//----------------------------------------------------------------------------------------------------
//============================================= The App ==============================================
//----------------------------------------------------------------------------------------------------

//The 'DashApp' type is your central DashApp that contains all settings, configs, the layout, styles, 
//scripts, etc. that makes up your Dash.NET app. 

let myDashApp =
    DashApp.initDefault() // create a Dash.NET app with default settings
    |> DashApp.withLayout dslLayout // register the layout defined above.
    |> DashApp.appendCSSLinks [ 
        "main.css" // serve your custom css
        "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css" // register bulma as an external css dependency
    ]
    |> DashApp.addCallback callbackArrayInput // register the callback that will update the map
    |> DashApp.addCallback clickInput // register the callback that will update the map
    |> DashApp.addCallback callbackArrayInput3 // register the callback that will update the map


// The things below are Giraffe/ASP:NetCore specific and will likely be abstracted in the future.

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
        .UseGiraffe(DashApp.toHttpHandler myDashApp)

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