
// Learn more about Dash.NET at https://plotly.github.io/Dash.NET/

open Dash.NET.Suave
open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET
open Dash.NET

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
  ; port = 0
  ; errorHandler = Suave.Web.defaultErrorHandler }

DashApp.run [||] config dashApp
