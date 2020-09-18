namespace Dash.NET


module DCC = 
    
    open Giraffe.GiraffeViewEngine

    //These are placeholders and will be replaced by proper generated components
    open Plotly.NET
    open Newtonsoft.Json

    type PlotlyFigure = {
        data        : Trace List
        layout      : Layout
        frames      : seq<obj>
    }
    with
        static member ofGenericChart (gChart:GenericChart.GenericChart) =
            
            let traces = GenericChart.getTraces gChart 
            let layout = GenericChart.getLayout gChart 

            {
                data    = traces
                layout  = layout
                frames  = []
            }

    type Graph = {
        id      : string
        figure  : PlotlyFigure
    }
    with 
        static member ofGenericChart id (gChart:GenericChart.GenericChart) =
            
            {
                id          = id
                figure      = PlotlyFigure.ofGenericChart gChart
            }

        static member toComponentJson (graphComponent:Graph) =
            
            let root = DynamicObj()

            root?("namespace")  <- "dash_core_components"
            root?props          <- graphComponent
            root?("type")       <- "Graph"

            root

    let graph id figure = {id = id; figure=figure}

    open System
    open System.Runtime.InteropServices

    type Input() = inherit DynamicObj()

    let input (id:string) value (_type:string) = 

        let i = Input()

        let props = DynamicObj()

        props?id <- id
        props?value <- value
        props?("type") <- _type

        i?("namespace")<-"dash_core_components"
        i?props <- props
        i?("type") <- "Input"

        i

