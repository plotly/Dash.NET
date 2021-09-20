
// Learn more about Dash.NET at https://plotly.github.io/Dash.NET/

open Dash.NET.Suave
open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET
open Dash.NET
open System
open System.Threading

let myGraph = Chart.Line([(1,1);(2,2)])

let myLayout = 
  Html.div [
    Attr.children [
      Html.h1 [
          Attr.children "Hello world from Dash.NET and Suave!"
      ];
      Html.h2 [ Attr.children "Take a look at this graph:"];
      Graph.graph "my-ghraph-id" [Graph.Attr.figure(GenericChart.toFigure myGraph)];
      Html.h2 [Attr.children "Tell us something!"];
      Input.input "test-input" [Input.Attr.inputType InputType.Text];
      Html.h3 [Attr.children "Input below will not trigger the callback"]
      Input.input "test-input-state" [Input.Attr.inputType InputType.Text]
      Html.div [ Attr.id "test-output" ];
    ]
  ]

let testCallback =
  Callback.singleOut(
      "test-input" @. Value,
      "test-output" @. Children,
      (fun (input:string) (state:string) ->
          "test-output" @. Children => (
              sprintf "You said : '%s' and we added the state: '%s'" input state)
      ),
      State = ["test-input-state" @. Value])

let dashApp =
  DashApp.initDefault()
  |> DashApp.withLayout myLayout
  |> DashApp.addCallback testCallback

let config = 
  { hostname = "localhost"
  ; ip = "127.0.0.1"
  ; port = 0 // Request the server to be launched on a random ephemeral port
  ; errorHandler = Suave.Web.defaultErrorHandler }

let listening, server = DashApp.runAsync [||] config dashApp

let cts = new CancellationTokenSource()

Async.Start(server, cts.Token)

printfn "Make requests now"

// Capture assigned port
let [| Some startData |] = Async.RunSynchronously listening
let port = startData.binding.port

let url = sprintf "http://%s:%d" config.ip port

Console.WriteLine ("Opening: {0}", url)

// Open browser
let psi = new System.Diagnostics.ProcessStartInfo()
psi.UseShellExecute <- true
psi.FileName <- url
System.Diagnostics.Process.Start(psi) |> ignore

Console.WriteLine("Press any key to exit application")
Console.ReadKey true |> ignore
cts.Cancel()
