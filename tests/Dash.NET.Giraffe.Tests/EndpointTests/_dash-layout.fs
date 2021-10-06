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
            """{"namespace":"dash_core_components","props":{"id":"test-graph","children":[],"figure":{"data":[{"type":"scatter","mode":"markers","x":[1],"y":[2]}],"frames":[]}},"type":"Graph"}"""
        
        Html.div [
            Attr.children [
                Html.h1 [ Attr.children "Dropdown Component Test:" ];
                Dropdown.dropdown "dropdown" [
                    Dropdown.Attr.loadingState (LoadingState.init(true))
                    Dropdown.Attr.options [
                        DropdownOption.init ("1", 1, false, "3")
                        DropdownOption.init ("2", 2, true)
                        DropdownOption.init ("3", 3)
                    ]
                ]
                Html.h1 [ Attr.children "Component Test:" ];
                Html.h1 [ Attr.children "Component Test:" ];
                Html.h1 [ Attr.children "Component Test:" ];
                Html.h1 [ Attr.children "Component Test:" ];
                Html.h1 [ Attr.children "Component Test:" ];
            ]
        ]
        |> generateLayoutEndpointTest 
            "sample layout" 
            "Incorrect layout JSON"
            """{"namespace":"dash_html_components","props":{"children":[{"namespace":"dash_html_components","props":{"children":["Dropdown Component Test:"]},"type":"H1"},{"namespace":"dash_core_components","props":{"id":"dropdown","children":[],"options":[{"label":"1","value":1,"disabled":false,"title":"3"},{"label":"2","value":2,"disabled":true},{"label":"3","value":3}],"loading_state":{"isLoading":true}},"type":"Dropdown"},{"namespace":"dash_html_components","props":{"children":["Component Test:"]},"type":"H1"},{"namespace":"dash_html_components","props":{"children":["Component Test:"]},"type":"H1"},{"namespace":"dash_html_components","props":{"children":["Component Test:"]},"type":"H1"},{"namespace":"dash_html_components","props":{"children":["Component Test:"]},"type":"H1"},{"namespace":"dash_html_components","props":{"children":["Component Test:"]},"type":"H1"}]},"type":"Div"}"""
    ]