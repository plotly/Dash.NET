namespace Dash.NET

open Newtonsoft.Json
open System

type HotReloadSettings =
    {
        [<JsonProperty("intervall")>]
        Intervall: int
        [<JsonProperty("max_retry")>]
        MaxRetry: int
    }

//todo: use standard casing and add serialization flags instead.
type DashConfig =
    {
        //Dash Specific
        [<JsonProperty("url_base_pathname")>]
        UrlBasePathname: string Option
        [<JsonProperty("requests_pathname_prefix")>]
        RequestsPathamePrefix: string
        [<JsonProperty("serve_locally")>]
        ServeLocally: bool
        [<JsonProperty("ui")>]
        UI: bool
        [<JsonProperty("props_check")>]
        PropsCheck: bool
        [<JsonProperty("show_undo_redo")>]
        ShowUndoRedo: bool
        [<JsonProperty("suppress_callback_exceptions")>]
        SuppressCallbackExceptions: bool
        [<JsonProperty("update_title")>]
        UpdateTitle: string
    }
    static member create
        urlBasePathname
        requestsPathamePrefix
        serveLocally
        uI
        propsCheck
        showUndoRedo
        suppressCallbackExceptions
        updateTitle

        //hostName
        //logLevel
        //errorHandler
        =
        {
            UrlBasePathname = urlBasePathname
            RequestsPathamePrefix = requestsPathamePrefix
            ServeLocally = serveLocally
            UI = uI
            PropsCheck = propsCheck
            ShowUndoRedo = showUndoRedo
            SuppressCallbackExceptions = suppressCallbackExceptions
            UpdateTitle = updateTitle
        //hot_reload                  = {Intervall = 3000; MaxRetry = 8}

            //HostName = hostName
            //LogLevel = logLevel
            //ErrorHandler = errorHandler
        }

    static member initDefault() =
        DashConfig.create 
            None 
            (let s = Environment.GetEnvironmentVariable("DASH_REQUESTS_PATHNAME_PREFIX") in if s = null then "/" else ("/" + s))
            false 
            true 
            true 
            false 
            false 
            "Updating..."
            //"localhost"
            //LogLevel.Debug
            //((fun ex ->  ex.Message) >> text)

    static member initDefaultWith(initializer: DashConfig -> DashConfig) = DashConfig.initDefault () |> initializer
