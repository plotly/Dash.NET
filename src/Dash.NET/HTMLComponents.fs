namespace Dash.NET

[<RequireQualifiedAccess>]
module HTMLComponents =

    open Plotly.NET

    //These are placeholders and will be replaced by proper generated components

    type HTMLComponent () = 
        inherit DynamicObj()

    type Div () = 
        inherit DynamicObj()

    type Title () = 
        inherit DynamicObj()

    let div (id:string) (children:seq<obj>) = 

        let d       = Div()
        let props   = DynamicObj()

        props?id        <- id
        props?children  <- children
        d?("namespace") <-"dash_html_components"
        d?props         <- props
        d?("type")      <- "Div"

        d

    let title (id:string) (children:seq<obj>) = 

        let t       = Title()
        let props   = DynamicObj()

        props?id        <- id
        props?children  <- children
        t?("namespace") <-"dash_html_components"
        t?props         <- props
        t?("type")      <- "Title"

        t