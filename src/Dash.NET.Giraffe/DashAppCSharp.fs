module DashAppCSharp

open Giraffe
open Dash.NET.Giraffe.Views
open Dash.NET
open Dash.NET.Giraffe

open Microsoft.Extensions.Logging
open System

//Giraffe, Logging and ASP.NET specific
type DashGiraffeConfig =
    {
        HostName: string
        LogLevel: LogLevel
        ErrorHandler: System.Func<System.Exception,HttpHandler>
    }

let private convertConfig (config : DashGiraffeConfig) : Dash.NET.Giraffe.DashGiraffeConfig =
    {
        Dash.NET.Giraffe.DashGiraffeConfig.HostName = config.HostName
        LogLevel = config.LogLevel
        ErrorHandler = FuncConvert.FromFunc(config.ErrorHandler)
    }

type DashApp(Index : IndexView, Layout : DashComponent, Config : DashConfig, Callbacks : CallbackMap) =
    let dashApp : Dash.NET.Giraffe.DashApp = {
        Index = Index
        Layout = Layout
        Config = Config
        Callbacks = Callbacks
    }

    (* Interop helpers *)

    static member private fromDashApp (dashApp : Dash.NET.Giraffe.DashApp) = DashApp(dashApp.Index, dashApp.Layout, dashApp.Config, dashApp.Callbacks)

    (* Builder methods *)

    static member initDefault() = Dash.NET.Giraffe.DashApp.initDefault() |> DashApp.fromDashApp

    static member initDefaultWith (initializer: Dash.NET.Giraffe.DashApp -> Dash.NET.Giraffe.DashApp) = Dash.NET.Giraffe.DashApp.initDefaultWith initializer |> DashApp.fromDashApp

    member _.withConfig (config: DashConfig) =
        Dash.NET.Giraffe.DashApp.withConfig config dashApp
        |> DashApp.fromDashApp

    member _.withIndex (index:IndexView) =
        Dash.NET.Giraffe.DashApp.withIndex index dashApp
        |> DashApp.fromDashApp

    member _.mapIndex (mapping: IndexView -> IndexView) =
        Dash.NET.Giraffe.DashApp.mapIndex mapping dashApp
        |> DashApp.fromDashApp

    member _.appendCSSLinks (hrefs:seq<string>) = 
        Dash.NET.Giraffe.DashApp.appendCSSLinks hrefs dashApp
        |> DashApp.fromDashApp

    member _.appendScripts (sources:seq<string>) = 
        Dash.NET.Giraffe.DashApp.appendScripts sources dashApp
        |> DashApp.fromDashApp

    member _.withLayout (layout:DashComponent) = 
        Dash.NET.Giraffe.DashApp.withLayout layout dashApp
        |> DashApp.fromDashApp

    member _.addCallback (callback: Callback<'Function>) = 
        Dash.NET.Giraffe.DashApp.addCallback callback dashApp
        |> DashApp.fromDashApp

    member _.addCallbacks (callbacks: seq<(Callback<'Function>)>) = 
        Dash.NET.Giraffe.DashApp.addCallbacks callbacks dashApp
        |> DashApp.fromDashApp

    (* Other *)

    member _.getIndexHTML () = 
        Dash.NET.Giraffe.DashApp.getIndexHTML dashApp

    member _.toHttpHandler () = 
        Dash.NET.Giraffe.DashApp.toHttpHandler dashApp

    member _.run (args: string []) (config: DashGiraffeConfig) = 
        let convertedConfig = config |> convertConfig
        Dash.NET.Giraffe.DashApp.run args convertedConfig dashApp
    
