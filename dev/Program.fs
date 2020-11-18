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

open Dash.NET.DCC
open Dash.NET.HTML
open HTMLPropTypes
open ComponentPropTypes

let dslLayout = 
    Div.div [] [
        H1.h1 [] [str "Hello Dash from F#"]
        Store.store "test-store" [] []
        Button.button [Id "test-btn"] [str "Test Me Pls"]
        Div.div [Id "test-output"] []
        Div.div [Id "test-output2"] []
        Div.div [Id "test-output3"] []
        Button.button [Id "test-btn2"] [str "Try me at last"]
        Div.div [Id "test-output4"] []
    ]

type Test = {
    A: string
    B: string
}

let storeCallback =
    Callback(
        [CallbackInput.create("test-btn","n_clicks")],
        CallbackOutput.create("test-store","data"),
        (fun (click:IConvertible) ->
            Newtonsoft.Json.JsonConvert.SerializeObject({A = "hallo"; B = "AMK"})
        )
    )

let createStoreUpdateCB outputID outputProp =
    Callback(
        [CallbackInput.create("test-store","data")],
        CallbackOutput.create(outputID,outputProp),
        (fun (dataJson: string) ->
            Newtonsoft.Json.JsonConvert.DeserializeObject<Test>(dataJson)
            |> sprintf "%A"
        )
    )
let myBtnCallback =
    Callback(
        [CallbackInput.create("test-btn2","n_clicks")],
        CallbackOutput.create("test-output4","children"),
        (fun (click:IConvertible) (state1:string) ->
            state1
        ),
        State = [
            CallbackState.create("test-output","children")

        ]
    )

let myDashApp =
    DashApp.initDefault()
    |> DashApp.withLayout dslLayout
    |> DashApp.addCallback(storeCallback)
    |> DashApp.addCallback(createStoreUpdateCB "test-output"  "children")
    |> DashApp.addCallback(createStoreUpdateCB "test-output2" "children")
    |> DashApp.addCallback(createStoreUpdateCB "test-output3" "children")
    |> DashApp.addCallback(myBtnCallback)

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