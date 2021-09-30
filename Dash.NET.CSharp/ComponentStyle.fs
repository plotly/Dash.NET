namespace Dash.NET.CSharp

module ComponentStyle =
    type DashComponentStyle = private WrappedDashComponentStyle of Dash.NET.DashComponentStyle with
        static member internal Wrap (v : Dash.NET.DashComponentStyle) = WrappedDashComponentStyle v
        static member internal Unwrap (v : DashComponentStyle) = match v with WrappedDashComponentStyle value -> value

        static member fromCssStyle (css: seq<Style>) = Dash.NET.DashComponentStyle.fromCssStyle (css |> Seq.map Style.Unwrap) |> DashComponentStyle.Wrap

