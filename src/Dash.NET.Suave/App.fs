namespace Dash.NET.Suave

open Views

open Suave.Html
open Suave
open Suave.Operators
open Suave.Filters

open System.Reflection
open Dash.NET
open Newtonsoft.Json
open System.Threading
open System

module Util =
  let json o = JsonConvert.SerializeObject(o)
  let unjson<'T> str = JsonConvert.DeserializeObject<'T>(str)

type DashApp =
  {
    Index: IndexView
    Layout: DashComponent 
    Config: DashConfig
    Callbacks: CallbackMap
  }

  /// Returns a DashApp with all fields initialized with default values.
  static member initDefault() =
    {
      Index = IndexView.initDefault()
      Layout = DashComponent()
      Config = DashConfig.initDefault()
      Callbacks = CallbackMap()
    }

  /// Returns the result of applying an initializer function to a DashApp with all fields initialized with default values.
  static member initDefaultWith (initializer: DashApp -> DashApp) = DashApp.initDefault () |> initializer

  /// Returns a new DashApp with the original DashConfig replaced by the given DashConfig 
  static member withConfig (config: DashConfig) (app: DashApp) = 
    { app with 
        Config = config 
        Index = app.Index |> IndexView.withConfig config
    }

  /// Returns a new DashApp with the original IndexView replaced by the given IndexView 
  static member withIndex (index:IndexView) (app:DashApp) =
      { app with 
          Index = index |> IndexView.withConfig app.Config
      }

  /// Returns a new DashApp with a new IndexView created by applying the mapping function to the original IndexView.
  static member mapIndex (mapping: IndexView -> IndexView) (app:DashApp) =
    { app with 
        Index = mapping app.Index 
    }

  /// Returns a new DashApp with the given css source links appended to the original IndexView's css register as link tags
  static member appendCSSLinks (hrefs:seq<string>) (app:DashApp) =
    let tags = 
      hrefs
      |> Seq.map (fun href ->
          link ["rel", "stylesheet"; "href", href ; "crossorigin", " "]
      )
    app
    |> DashApp.mapIndex (IndexView.appendCSSLinks tags)

  /// Returns a new DashApp with the given script source links appended to the original IndexView's script register as script tags
  static member appendScripts (sources:seq<string>) (app:DashApp) =
    let tags = 
      sources
      |> Seq.map (fun source ->
          script [ "type", "application/javascript"; "crossorigin", " "; "src", source] []
      )
    app
    |> DashApp.mapIndex (IndexView.appendScripts tags)

  /// Returns a new DashApp with the original Layout replaced by the given Layout 
  static member withLayout (layout:DashComponent) (app:DashApp) =
      { app with 
          Layout = layout
      }

  /// Returns a new DashApp with the given callback added to the original Callback register
  static member addCallback (callback: Callback<'Function>) (app: DashApp) =

    //To-Do: Maybe use copy utility for all direct calls to underlying DynamicObjs.
    { app with
        Callbacks =
          app.Callbacks
          |> CallbackMap.registerCallback callback
    }

  /// Returns a new DashApp with the given callbacks added to the original Callback register
  static member addCallbacks (callbacks: seq<(Callback<'Function>)>) (app: DashApp) =
    //To-Do: Maybe use copy utility for all direct calls to underlying DynamicObjs.
    { app with
        Callbacks =
            callbacks
            |> Seq.fold (fun cMap cHandler ->
                cMap
                |> CallbackMap.registerCallback cHandler) app.Callbacks
    }

  /// Returns the given DashApp's index DOM
  static member getIndexHTML (app:DashApp) =
    app.Index |> IndexView.toHTMLComponent

  /// Returns the DasApp as a http handler to use as Giraffe middleware in an asp.netcore pipeline. 
  ///
  /// This application will provide the following endpoints:
  ///
  /// GET / -> serves the DashApps IndexView (This is also where the Das renderer will render the layout)
  ///
  /// GET /_dash-layout -> serves the DashApp's layout as JSON
  ///
  /// GET /_dash-dependencies -> serves the serialized callback handlers registered in the DashApp's callback register as JSON
  ///
  /// GET /_reload-hash -> Not implemented, returns empty JSON object
  ///
  /// POST /_dash-update-component -> handles callback requests and returns serialized callback JSON responses.

  static member toWebPart (app:DashApp) : WebPart =

    let handleCallbackRequest (cbRequest:CallbackRequest) =
      let inputs = 
        let inputs = cbRequest.Inputs |> Array.map (fun reqInput ->  reqInput.Value) //generate argument list for the callback
        let states = 
            //Yes, this is ugly. I currently cant find a way to deserialize an empty array directly when the state property is missing in the JSON, but that sounds like a problem that is solvable.
            try
                cbRequest.State |> Array.map (fun reqInput -> reqInput.Value)
            with _ ->
                [||]
        Array.append inputs states
    
      printfn "Input Tokens: %A" inputs
    
      let result = 
        app.Callbacks
        |> CallbackMap.getPackedCallbackById (cbRequest.Output) //get the callback from then callback map
        |> Callback.getResponseObject inputs//evaluate the handler function and get the response to send to the client
      result

    choose [
      GET >=>
        choose [
          //serve the index
          path "/" >=> Successful.OK(renderHtmlDocument(app |> DashApp.getIndexHTML))

          //Dash GET enpoints
          path "/_dash-layout"       >=> context(fun x -> Successful.OK(Util.json(app.Layout))) >=> Writers.setMimeType "application/json"//Calls from Dash renderer for what components to render (must return serialized dash components)
          path "/_dash-dependencies" >=> Successful.OK(Util.json (app.Callbacks |> CallbackMap.toDependencies)) >=> Writers.setMimeType "application/json"//Serves callback bindings as json on app start.
          path "/_reload-hash"       >=> Successful.OK(Util.json obj) >=> Writers.setMimeType "application/json"//This call is done when using hot reload.
        ]

      POST >=> 
        choose [
          //Dash POST endpoints
          path "/_dash-update-component" //calls from callbacks come in here.
            >=> request(fun r ->
                  let ooo = Util.unjson<CallbackRequest> (System.Text.Encoding.UTF8.GetString r.rawForm)
                  let result = handleCallbackRequest ooo
                  // return serialized result of the handler function
                  Successful.OK(Util.json result) >=> Writers.setMimeType "application/json")
        ]
      RequestErrors.NOT_FOUND "File not found" ]

  static member run (args: string []) (app: DashApp) =
    //Go through an assembly and look for any types that inherit DashComponent
    //if one is found then look for the LoadableComponentDefinition and add its script
    let tryLoadComponents (innerApp: DashApp) (a: Assembly) =
        try
            a.GetModules()
            |> Array.collect (fun m -> m.GetTypes())
            |> Array.choose (fun t -> if t.IsSubclassOf typeof<DashComponent> then Some t else None)
            |> Array.choose (fun t -> t.GetProperty "definition" |> Option.ofObj)
            |> Array.choose (fun p -> p.GetValue(null,null) |> Option.ofObj)
            |> Array.choose tryUnbox
            |> Array.fold
                (fun (acc: DashApp) (def: LoadableComponentDefinition) -> 
                    printfn "Loading component: %s" def.ComponentName
                    DashApp.appendScripts def.ComponentJavascript acc)
                innerApp
        with
        | e -> innerApp

    //Get every assembly referenced by an assembly
    let getReferenced (a: Assembly) = a.GetReferencedAssemblies()

    //Call tryLoadComponents on an assembly and every assembly it references
    let rec loadAssemblies ((innerApp, loaded): DashApp * (string Set)) (toLoad: string) =
        if not(loaded.Contains toLoad) then
            try
                let assembly = Assembly.Load(toLoad)

                assembly 
                |> getReferenced
                |> List.ofArray 
                |> List.map (fun a -> a.FullName)
                |> List.fold 
                    loadAssemblies 
                    (tryLoadComponents innerApp assembly, loaded.Add toLoad)
            with
            | e -> 
                printfn "Failed to load assembly: %s \n %O" toLoad e
                innerApp, loaded
        else
            innerApp, loaded

    let loadedApp =
      // GetEntryAssembly() not good because the entry assembly could be a bigger enclosing application like Visual Studio Code
      // GetCallingAssembly() seems like a better compromise at the moment
      Assembly.GetCallingAssembly().FullName 
      |> loadAssemblies (app, Set.empty)
      |> fst

    // This folder has to be "<path-to-exe>/WebRoot" for the way generated component javascript injection currently works
    //let contentRoot = Reflection.Assembly.GetExecutingAssembly().Location |> Path.GetDirectoryName
    //let webRoot     = Path.Combine(contentRoot, "WebRoot")

    let cts = new CancellationTokenSource()
    
    let conf =
        { defaultConfig with
              cancellationToken = cts.Token
              bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" 0 ] }
    
    // Launch webserver on random ephemeral port
    let listening, server =
        startWebServerAsync conf  (DashApp.toWebPart loadedApp)

    Async.Start(server, cts.Token)
    printfn "Make requests now"

    // Capture assigned port
    let [| Some startData |] = Async.RunSynchronously listening
    let port = startData.binding.port

    let url = sprintf "http://localhost:%d" port

    Console.WriteLine ("Opening: {0}", url)

    // Open browser
    let psi = new System.Diagnostics.ProcessStartInfo()
    psi.UseShellExecute <- true
    psi.FileName <- url
    System.Diagnostics.Process.Start(psi) |> ignore

    Console.WriteLine("Press any key to exit application")
    Console.ReadKey true |> ignore
    cts.Cancel()