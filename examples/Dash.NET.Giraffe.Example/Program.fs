
// Learn more about Dash.NET at https://plotly.github.io/Dash.NET/

open Dash.NET.Giraffe
open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET
open Microsoft.Extensions.Logging
open Giraffe

let myGraph = Chart.Line([(1,1);(2,2)])

let myLayout = 
  Html.div [
    Attr.children [
      Html.h1 [
          Attr.children "Hello world from Dash.NET and Giraffe!"
      ];
      Html.h2 [ Attr.children "Take a look at this graph:"];
      Graph.graph "my-ghraph-id" [Graph.Attr.figure(GenericChart.toFigure myGraph)]
    ]
  ]

[<EntryPoint>]
let main argv =

  let dashApp =
    DashApp.initDefault()
    |> DashApp.withLayout myLayout

  let config = 
    { HostName = "localhost"
    ; LogLevel = LogLevel.Debug
    ; ErrorHandler = (fun ex -> text ex.Message) }

  DashApp.run [||] config dashApp
