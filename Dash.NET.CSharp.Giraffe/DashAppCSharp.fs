namespace Dash.NET.CSharp.Giraffe

module DashApp =

    open Giraffe
    open Dash.NET.Giraffe.Views
    //open Dash.NET
    open Dash.NET.Giraffe

    open Microsoft.Extensions.Logging
    open System
    open Dash.NET.CSharp

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

    type DashConfig = private DashConfigWrapped of Dash.NET.DashConfig with
        static member Wrap (v : Dash.NET.DashConfig) = DashConfigWrapped v
        static member Unwrap (v : DashConfig) = match v with | DashConfigWrapped value -> value
    
    type CallbackMap = private CallbackMapWrapped of Dash.NET.CallbackMap with
        static member Wrap (v : Dash.NET.CallbackMap) = CallbackMapWrapped v
        static member Unwrap (v : CallbackMap) = match v with | CallbackMapWrapped value -> value

    type DashApp(Index : IndexView, Layout : DashComponent, Config : DashConfig, Callbacks : CallbackMap) =
        let dashApp : Dash.NET.Giraffe.DashApp = {
            Index = Index
            Layout = Layout |> DashComponent.Unwrap
            Config = Config |> DashConfig.Unwrap
            Callbacks = Callbacks |> CallbackMap.Unwrap
        }

        (* Interop helpers *)

        static member private fromDashApp (dashApp : Dash.NET.Giraffe.DashApp) = DashApp(dashApp.Index, dashApp.Layout |> DashComponent.Wrap, dashApp.Config |> DashConfig.Wrap, dashApp.Callbacks |> CallbackMap.Wrap)

        (* Builder methods *)

        static member initDefault() = Dash.NET.Giraffe.DashApp.initDefault() |> DashApp.fromDashApp

        static member initDefaultWith (initializer: Dash.NET.Giraffe.DashApp -> Dash.NET.Giraffe.DashApp) = Dash.NET.Giraffe.DashApp.initDefaultWith initializer |> DashApp.fromDashApp

        member _.withConfig (config: DashConfig) =
            Dash.NET.Giraffe.DashApp.withConfig (config |> DashConfig.Unwrap) dashApp
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
            Dash.NET.Giraffe.DashApp.withLayout (layout |> DashComponent.Unwrap) dashApp
            |> DashApp.fromDashApp

        member _.addCallback (callback: Dash.NET.Callback<'Function>) = 
            Dash.NET.Giraffe.DashApp.addCallback callback dashApp
            |> DashApp.fromDashApp

        member _.addCallbacks (callbacks: seq<(Dash.NET.Callback<'Function>)>) = 
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
    
