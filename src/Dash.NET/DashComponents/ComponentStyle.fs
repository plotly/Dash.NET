namespace Dash.NET

open Plotly.NET

type DashComponentStyle() = 
    inherit DynamicObj()

    static member fromCssStyle (css: seq<Css.Style>) =
        let style = DashComponentStyle()
        css
        |> List.ofSeq
        |> List.iter
            (fun (s: Css.Style) ->
                match s with
                | Css.StyleProperty (cssPropertyName, cssPropertyValue) -> 
                    style.SetValue(cssPropertyName, cssPropertyValue))
        style

