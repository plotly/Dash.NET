namespace Dash.NET.CSharp.DCC

open System

[<RequireQualifiedAccess>]
module Graph =
    (*
        We're doing some 'funky stuff' here to make sure the signature of our graph component takes attributes from the `Dash.NET.CSharp` namespace, as opposed to the `Dash.NET` namespace.
        This is as to not confuse the user. Under the hood we are still using Attr's from the original `Dash.NET` namespace but we are just wrapping them (and then unwrapping them again before passing it on to the original component).
    *)

    type Attr =
        | WrappedAttr of Dash.NET.DCC.Graph.Attr
        static member internal Wrap (attr : Dash.NET.DCC.Graph.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        (*
            Here we shadow all the methods on the original `Dash.NET.DCC.Graph.Attr` with slight modifications to usability from the C# side (e.g. avoid FSharpList, FSharpFunc, use [<ParamArray>], etc.).
            We wrap the Attr as to not let the abstraction be known to the consumer (see comment 'funky stuff' above)
        *)

        
        ///<summary>
        ///Data from latest click event. Read-only.
        ///</summary>
        static member clickData(p: obj) =
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.clickData p |> Attr.Wrap
        ///<summary>
        ///Data from latest click annotation event. Read-only.
        ///</summary>
        static member clickAnnotationData(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.clickAnnotationData p |> Attr.Wrap
        ///<summary>
        ///Data from latest hover event. Read-only.
        ///</summary>
        static member hoverData(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.hoverData p |> Attr.Wrap
        ///<summary>
        ///If True, &#96;clear_on_unhover&#96; will clear the &#96;hoverData&#96; Dash.NET.DCC.Graph.Property
        ///when the user "unhovers" from a point.
        ///If False, then the &#96;hoverData&#96; Dash.NET.DCC.Graph.Property will be equal to the
        ///data from the last point that was hovered over.
        ///</summary>
        static member clearOnUnhover(p: bool) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.clearOnUnhover p |> Attr.Wrap
        ///<summary>
        ///Data from latest select event. Read-only.
        ///</summary>
        static member selectedData(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.selectedData p |> Attr.Wrap
        ///<summary>
        ///Data from latest relayout event which occurs
        ///when the user zooms or pans on the plot or other
        ///layout-level edits. Has the form &#96;{&lt;attr string&gt;: &lt;value&gt;}&#96;
        ///describing the changes made. Read-only.
        ///</summary>
        static member relayoutData(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.relayoutData p |> Attr.Wrap
        
        ///<summary>
        ///Data that should be appended to existing traces. Has the form
        ///&#96;[updateData, traceIndices, maxPoints]&#96;, where &#96;updateData&#96; is an object
        ///containing the data to extend, &#96;traceIndices&#96; (optional) is an array of
        ///trace indices that should be extended, and &#96;maxPoints&#96; (optional) is
        ///either an integer defining the maximum number of points allowed or an
        ///object with key:value pairs matching &#96;updateData&#96;
        ///Reference the Plotly.extendTraces API for full usage:
        ///https://plotly.com/javascript/plotlyjs-function-reference/#plotlyextendtraces
        ///</summary>
        static member extendData(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.extendData p |> Attr.Wrap

        ///<summary>
        ///Data that should be prepended to existing traces. Has the form
        ///&#96;[updateData, traceIndices, maxPoints]&#96;, where &#96;updateData&#96; is an object
        ///containing the data to prepend, &#96;traceIndices&#96; (optional) is an array of
        ///trace indices that should be prepended, and &#96;maxPoints&#96; (optional) is
        ///either an integer defining the maximum number of points allowed or an
        ///object with key:value pairs matching &#96;updateData&#96;
        ///Reference the Plotly.prependTraces API for full usage:
        ///https://plotly.com/javascript/plotlyjs-function-reference/#plotlyprependtraces
        ///</summary>
        static member prependData(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.prependData p |> Attr.Wrap

        ///<summary>
        ///Data from latest restyle event which occurs
        ///when the user toggles a legend item, changes
        ///parcoords selections, or other trace-level edits.
        ///Has the form &#96;[edits, indices]&#96;, where &#96;edits&#96; is an object
        ///&#96;{&lt;attr string&gt;: &lt;value&gt;}&#96; describing the changes made,
        ///and &#96;indices&#96; is an array of trace indices that were edited.
        ///Read-only.
        ///</summary>
        static member restyleData([<ParamArray>] p: obj array) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            Dash.NET.DCC.Graph.Attr.restyleData (p |> List.ofArray) |> Attr.Wrap
        ///<summary>
        ///Plotly &#96;figure&#96; object. See schema:
        ///https://plotly.com/javascript/reference
        ///&#96;config&#96; is set separately by the &#96;config&#96; Dash.NET.DCC.Graph.Property
        ///</summary>
        static member figure(p: Plotly.NET.GenericChart.Figure) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.figure p |> Attr.Wrap
        ///<summary>
        ///Generic style overrides on the plot div
        ///</summary>
        static member style([<ParamArray>] p: Dash.NET.CSharp.Html.Style array) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            Dash.NET.DCC.Graph.Attr.style (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///className of the parent div
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.className p |> Attr.Wrap
        ///<summary>
        ///Beta: If true, animate between updates using
        ///plotly.js's &#96;animate&#96; function
        ///</summary>
        static member animate(p: bool) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.animate p |> Attr.Wrap
        ///<summary>
        ///Beta: Object containing animation settings.
        ///Only applies if &#96;animate&#96; is &#96;true&#96;
        ///</summary>
        static member animationOptions(p: obj) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.animationOptions p |> Attr.Wrap
        ///<summary>
        ///Plotly.js config options.
        ///See https://plotly.com/javascript/configuration-options/
        ///for more info.
        ///</summary>
        static member config(p: Plotly.NET.Config) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.config p |> Attr.Wrap
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: Dash.NET.CSharp.DCC.ComponentPropTypes.LoadingState) = 
            guardAgainstNull "p" p
            Dash.NET.DCC.Graph.Attr.loadingState (p |> Dash.NET.CSharp.DCC.ComponentPropTypes.LoadingState.Convert) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Graph.Attr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Graph.Attr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) =
            guardAgainstNull "value" value
            Dash.NET.DCC.Graph.Attr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) =
            guardAgainstNull "value" value
            Dash.NET.DCC.Graph.Attr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Dash.NET.CSharp.Html.DashComponent) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Graph.Attr.children (value |> Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children([<ParamArray>] value: array<Dash.NET.CSharp.Html.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            Dash.NET.DCC.Graph.Attr.children (value |> Array.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<Dash.NET.CSharp.Html.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Seq.iter (guardAgainstNull "value")
            Dash.NET.DCC.Graph.Attr.children (value |> Seq.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap

    let Graph (id: string, [<ParamArray>] attrs : Attr array) = Dash.NET.DCC.Graph.graph id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Html.DashComponent.Wrap

