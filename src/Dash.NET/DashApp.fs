namespace Dash.NET

type DashApp =
    {
        Config: DashConfig
        Callbacks: CallbackMap
        Dependencies: DashDependency list
    }

    static member initDefault() =
        {
            Config = Defaults.defaultConfig
            Callbacks = CallbackMap()
            Dependencies = []
        }

    static member initDefaultWith (initializer: DashApp -> DashApp) = DashApp.initDefault () |> initializer

    static member withConfig (config: DashConfig) (app: DashApp) = { app with Config = config }

    static member withCallbackHandler (callbackId: string, callback: Callback<'Function>) (app: DashApp) =
        
        let dashDependency = Callback.toDashDependency callback

        //To-Do: Maybe use copy utility for all direct calls to underlying DynamicObjs.
        { app with
            Callbacks =
                app.Callbacks
                |> CallbackMap.registerCallback callbackId callback

            Dependencies = dashDependency::app.Dependencies
        }

    static member withCallbackHandlers (callbacks: seq<(string * Callback<'Function>)>) (app: DashApp) =
        //To-Do: Maybe use copy utility for all direct calls to underlying DynamicObjs.
        { app with
            Callbacks =
                callbacks
                |> Seq.fold (fun cMap (cId, cHandler) ->
                    cMap
                    |> CallbackMap.registerCallback cId cHandler) app.Callbacks
        }
