// Learn more about Dash.NET at https://plotly.github.io/Dash.NET/

#r "bin/Debug/net5.0/Dash.NET.dll"
#r "bin/Debug/net5.0/Suave.dll"
#r "bin/Debug/net5.0/Suave.Experimental.dll"
#r "bin/Debug/net5.0/Dash.NET.Suave.dll"
#r "bin/Debug/net5.0/Plotly.NET.dll"
#r "bin/Debug/net5.0/Feliz.Engine.dll"
#r "bin/Debug/net5.0/Newtonsoft.Json.dll"

open Dash.NET.Suave
open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET

let myGraph = Chart.Line([(1,1);(2,2)])

let myLayout = 
  Html.div [
    Attr.children [
      Html.h1 [
          Attr.children "Hello world from Dash.NET and Suave!"
      ];
      Html.h2 [ Attr.children "Take a look at this graph:"];
      Graph.graph "my-ghraph-id" [Graph.Attr.figure(GenericChart.toFigure myGraph)]
    ]
  ]

let dashApp =
  DashApp.initDefault()
  |> DashApp.withLayout myLayout

// run
DashApp.run [||] dashApp
