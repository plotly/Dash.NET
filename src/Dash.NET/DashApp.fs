namespace Dash.NET

open Views

type DashApp =
    {
        Index: IndexView
        Config: DashConfig
        Callbacks: CallbackMap
        Dependencies: DashDependency list
    }

    static member initDefault() =
        {
            Config = DashConfig.initDefault()
            Callbacks = CallbackMap()
            Dependencies = []
            Index = IndexView.initDefault()
        }

    static member initDefaultWith (initializer: DashApp -> DashApp) = DashApp.initDefault () |> initializer

    static member withConfig (config: DashConfig) (app: DashApp) = 
        { app with 
            Config = config 
            Index = app.Index |> IndexView.withConfig config
        }

    static member withIndex (index:IndexView) (app:DashApp) =
        { app with 
            Index = index |> IndexView.withConfig app.Config
        }

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

    static member getIndexHTML (app:DashApp) =
        app.Index |> IndexView.toHTMLComponent
