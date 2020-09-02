namespace Dash.NET

[<RequireQualifiedAccess>]
module HTMLComponents =

    open FSharp.Plotly

    type Div () = inherit DynamicObj()

    let div (id:string) (children:seq<obj>) = 

        let d = Div()

        let props = DynamicObj()

        props?id <- id
        props?children <- children
        d?("namespace")<-"dash_html_components"
        d?props <- props
        d?("type") <- "Div"

        d

        


