
// Learn more about Dash.NET at https://plotly.github.io/Dash.NET/

open Dash.NET.Giraffe
open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET
open Microsoft.Extensions.Logging
open Giraffe
open Dash.NET.ComponentPropTypes

let myGraph = Chart.Line([(1,1);(2,2)])

let myLayout = 
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
