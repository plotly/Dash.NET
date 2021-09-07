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

open Dash.NET

//----------------------------------------------------------------------------------------------------
//============================================== LAYOUT ==============================================
//----------------------------------------------------------------------------------------------------

//The layout describes the components that Dash will render for you.
open Dash.NET.Html // this namespace contains the standard html copmponents, such as div, h1, etc.
open Dash.NET.Css // this namespace contains the standard html copmponents, such as div, h1, etc.

open Dash.NET.DCC // this namespace contains the dash core components, the heart of your Dash.NET app.
open ComponentPropTypes

type A = { B: string }

//Note that this layout uses css classes defined by Bulma (https://bulma.io/), which gets defined as a css dependency in the app section below.
let dslLayout =
    Html.div [ 
        Attr.className "section"
        Attr.children [ 
            Dropdown.dropdown "testInput1" [
                Dropdown.Attr.options [
                    DropdownOption.create "1" "1" false "1"
                    DropdownOption.create 2 2 false "2"
                    DropdownOption.create 3L 3L false "3"
                    DropdownOption.create 4.1 4.1 false "4.1"
                ]
                Dropdown.Attr.multi true
            ]
            Html.br []
            Html.label [ Attr.children (Html.text "Selected values :") ]
            Html.br []
            Html.div [ Attr.id "output-1" ]
            Html.div [ Attr.id "output-2" ]
            Html.div [ Attr.id "output-3" ]
            Html.div [ Attr.id "output-4" ]            
            Html.button [ 
                Attr.className "button is-primary"
                Attr.id "testInput2"
                Attr.children "Click ME!" ]
            Html.br []
            Html.label [ Attr.children (Html.text "Number of clicks:") ]
            Html.div [ Attr.id "output-5" ]
            Html.p [ 
                Attr.className "has-text-primary"
                Attr.children (Html.text "Testing Attribute DSL") ]
            Html.text "Testing Html DSL"
            Html.custom ("P", [Attr.children (Html.text "This is a custom Html element")])  
            Html.p [
                Attr.style [(Css.color Feliz.color.blue);  Css.fontSize 30]
                Attr.children (Html.text "Testing Css DSL")] 
        ] 
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
open Dash.NET.Operators

let callbackArrayInput =
    Callback.multiOut(
        ["testInput1" @.Value],
        [
            "output-1" @.Children
            "output-2" @.Children
        ],
        (fun (inputs:float []) (nclicks:float) ->
            [
               (Array.last inputs) * nclicks * 1.
               (Array.last inputs) * nclicks * 2.
            ]
        ),
        State = [
            "testInput2" @. N_Clicks
        ]
    )

// usage of the new operators:
let clickInput =
    Callback.singleOut (
        "testInput2" @. N_Clicks,
        "output-5" @. Children,
        (fun (x: float) -> "output-5" @. Children => sprintf "%A" x)
    )

//----------------------------------------------------------------------------------------------------
//============================================= The App ==============================================
//----------------------------------------------------------------------------------------------------

//The 'DashApp' type is your central DashApp that contains all settings, configs, the layout, styles,
//scripts, etc. that makes up your Dash.NET app.

let myDashApp =
    DashApp.initDefault () // create a Dash.NET app with default settings
    |> DashApp.withLayout dslLayout // register the layout defined above.
    |> DashApp.appendCSSLinks [ 
        "main.css" // serve your custom css
        "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css" // register bulma as an external css dependency
    ]
    |> DashApp.addCallback callbackArrayInput // register the callback that will update the map
    |> DashApp.addCallback clickInput // register the callback that will update the map
    


    // The things below are Giraffe/ASP:NetCore specific and will likely be abstracted in the future.

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

    clearResponse
    >=> setStatusCode 500
    >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:8080")
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    let env =
        app.ApplicationServices.GetService<IWebHostEnvironment>()

    (match env.EnvironmentName with
     | "Development" -> app.UseDeveloperExceptionPage()
     | _ -> app.UseGiraffeErrorHandler(errorHandler))
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(DashApp.toHttpHandler myDashApp)

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder
        .AddFilter(fun l -> l.Equals LogLevel.Debug)
        .AddConsole()
        .AddDebug()
    |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")

    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .UseContentRoot(contentRoot)
                .UseWebRoot(webRoot)
                .UseUrls("http://localhost:5000")
                .Configure(Action<IApplicationBuilder> configureApp)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)
            |> ignore)
        .Build()
        .Run()

    0
