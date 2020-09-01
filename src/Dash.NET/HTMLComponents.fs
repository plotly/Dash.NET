namespace Dash.NET

module HTMLComponents =

    open FSharp.Plotly

    type Div () = 
        inherit DynamicObj()

        static member withChildren (children) =
            let d = Div()

            let props = DynamicObj()

            props?id <- System.Guid.NewGuid().ToString()
            props?children <- children
            d?("namespace")<-"dash_html_components"
            d?props <- props
            d?("type") <- "Div"

            d

        


