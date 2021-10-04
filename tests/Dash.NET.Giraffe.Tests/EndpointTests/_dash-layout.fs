module EndpointTests

open Expecto
open Giraffe
open Dash.NET
open Dash.NET.DCC
open Dash.NET.Giraffe
open Plotly.NET
open TestUtils
open Microsoft.Extensions.Logging
open System.Net.Http


[<Tests>]
let endpointTests =
    testList "EndpointTests._dash_layout tests" [

        Html.div [Attr.children "Test!"]
        |> generateLayoutEndpointTest 
            "HTML Div component" 
            "Incorrect layout JSON"
            """{"namespace":"dash_html_components","props":{"children":["Test!"]},"type":"Div"}"""

        Graph.graph "test-graph" [Graph.Attr.figure (Chart.Point([1,2]) |> GenericChart.toFigure)]
        |> generateLayoutEndpointTest 
            "DCC Graph component" 
            "Incorrect layout JSON"
            """{"namespace":"dash_core_components","props":{"id":"test-graph","children":[],"figure":{"data":[{"type":"scatter","x":[1],"y":[2],"mode":"markers","marker":{}}],"layout":{},"frames":[]}},"type":"Graph"}"""
    ]