
// Learn more about Dash.NET at https://plotly.github.io/Dash.NET/

open Dash.NET.Giraffe
open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET
open Microsoft.Extensions.Logging
open Giraffe
open Dash.NET.ComponentPropTypes
open Dash.NET

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


open FSharp.Data

let [<Literal>] Csv = "https://raw.githubusercontent.com/plotly/datasets/master/iris-id.csv"
type CsvData = CsvProvider<Csv>
let rows = CsvData.Load(Csv).Cache().Rows 
let _ = Seq.head rows // skip the first line



[<EntryPoint>]
let main argv =


    // Use plotly charts
    let scatterPlot low high = 
        let filtered = Seq.filter (fun (x:CsvData.Row) -> x.Petal_width > low && x.Petal_width < high) rows
        let points = Seq.map (fun (x:CsvData.Row) -> x.Sepal_width,x.Sepal_length) filtered
        let petal_length = Seq.map (fun (x:CsvData.Row) -> 6*int(x.Petal_length)) filtered
        let markers = TraceObjects.Marker.init(MultiSize=petal_length)
        // Map species to different colors
        let colorMap = function | "setosa" -> "#4287f5" | "versicolor" -> "#cb23fa" | "virginica" -> "#23fabd"
        let spec = Seq.map (fun (x:CsvData.Row)  -> x.Species) filtered 
        markers?color <- Seq.map colorMap spec
        Chart.Scatter(points,StyleParam.Mode.Markers)
        |> Chart.withMarker markers
        |> Chart.withTitle("Iris Dataset")
        |> Chart.withXAxisStyle("Sepal Width")
        |> Chart.withYAxisStyle("Sepal Length")
        |> GenericChart.toFigure

    // Layout for our dash app
    let myLayout = 
        Html.div [
        Attr.children [
            Graph.graph "my-graph-id" [];
            Html.p [ Attr.children "Petal Width:" ]
            RangeSlider.rangeSlider "range-slider" [
                RangeSlider.Attr.min 0.
                RangeSlider.Attr.max 2.5
                RangeSlider.Attr.step 0.1
                RangeSlider.Attr.marks (
                    [ 0.; 2.5 ]
                    |> List.map (fun v -> v, v |> sprintf "%g" |> RangeSlider.Mark.Value)
                    |> Map.ofList
                )
                RangeSlider.Attr.value [ 0.5; 2. ]
                RangeSlider.Attr.tooltip <|
                    RangeSlider.TooltipOptions.init (
                        alwaysVisible = true,
                        placement = RangeSlider.TooltipPlacement.Bottom
                    )
            ]
        ]
        ]

    let callback =
        let outputTarget = "my-graph-id" @. CustomProperty "figure"
        Callback.singleOut(
        "range-slider" @. Value,
        outputTarget,
        (fun (sliderRange: decimal []) ->
            let low, high =
                let r1 = sliderRange.[0]
                let r2 = sliderRange.[1]
                if r1 < r2 then r1, r2 else r2, r1
            outputTarget => scatterPlot low high
        ),
        PreventInitialCall = false
        )

    let dashApp =
        DashApp.initDefault()
        |> DashApp.withLayout myLayout
        |> DashApp.addCallback callback


    //let dashApp =
    //    DashApp.initDefault()
    //    |> DashApp.withLayout myLayout

    // To listen in all IP addresses set IpAddress = "*"
    let config = 
        { HostName = "localhost"
        ; IpAddress = "*"
        ; Port = 8000
        ; LogLevel = LogLevel.Debug
        ; ErrorHandler = (fun ex -> text ex.Message) }

    DashApp.run [||] config dashApp
