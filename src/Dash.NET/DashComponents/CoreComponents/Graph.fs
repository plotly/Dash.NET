namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

///<summary>
///Graph can be used to render any plotly.js-powered data visualization.
///You can define callbacks based on user interaction with Graphs such as
///hovering, clicking or selecting
///</summary>
[<RequireQualifiedAccess>]
module Graph =
    ///<summary>
    ///• responsive (value equal to: 'true', 'false', 'auto'; default auto) - If True, the Plotly.js plot will be fully responsive to window resize
    ///and parent element resize event. This is achieved by overriding
    ///&#96;config.responsive&#96; to True, &#96;figure.layout.autosize&#96; to True and unsetting
    ///&#96;figure.layout.height&#96; and &#96;figure.layout.width&#96;.
    ///If False, the Plotly.js plot not be responsive to window resize and
    ///parent element resize event. This is achieved by overriding &#96;config.responsive&#96;
    ///to False and &#96;figure.layout.autosize&#96; to False.
    ///If 'auto' (default), the Graph will determine if the Plotly.js plot can be made fully
    ///responsive (True) or not (False) based on the values in &#96;config.responsive&#96;,
    ///&#96;figure.layout.autosize&#96;, &#96;figure.layout.height&#96;, &#96;figure.layout.width&#96;.
    ///This is the legacy behavior of the Graph component.
    ///Needs to be combined with appropriate dimension / styling through the &#96;style&#96; prop
    ///to fully take effect.
    ///&#10;
    ///• clickData (record; default null) - Data from latest click event. Read-only.
    ///&#10;
    ///• clickAnnotationData (record; default null) - Data from latest click annotation event. Read-only.
    ///&#10;
    ///• hoverData (record; default null) - Data from latest hover event. Read-only.
    ///&#10;
    ///• clear_on_unhover (boolean; default false) - If True, &#96;clear_on_unhover&#96; will clear the &#96;hoverData&#96; property
    ///when the user "unhovers" from a point.
    ///If False, then the &#96;hoverData&#96; property will be equal to the
    ///data from the last point that was hovered over.
    ///&#10;
    ///• selectedData (record; default null) - Data from latest select event. Read-only.
    ///&#10;
    ///• relayoutData (record; default null) - Data from latest relayout event which occurs
    ///when the user zooms or pans on the plot or other
    ///layout-level edits. Has the form &#96;{&lt;attr string&gt;: &lt;value&gt;}&#96;
    ///describing the changes made. Read-only.
    ///&#10;
    ///• extendData (list | record; default null) - Data that should be appended to existing traces. Has the form
    ///&#96;[updateData, traceIndices, maxPoints]&#96;, where &#96;updateData&#96; is an object
    ///containing the data to extend, &#96;traceIndices&#96; (optional) is an array of
    ///trace indices that should be extended, and &#96;maxPoints&#96; (optional) is
    ///either an integer defining the maximum number of points allowed or an
    ///object with key:value pairs matching &#96;updateData&#96;
    ///Reference the Plotly.extendTraces API for full usage:
    ///https://plotly.com/javascript/plotlyjs-function-reference/#plotlyextendtraces
    ///&#10;
    ///• prependData (list | record; default null) - Data that should be prepended to existing traces. Has the form
    ///&#96;[updateData, traceIndices, maxPoints]&#96;, where &#96;updateData&#96; is an object
    ///containing the data to prepend, &#96;traceIndices&#96; (optional) is an array of
    ///trace indices that should be prepended, and &#96;maxPoints&#96; (optional) is
    ///either an integer defining the maximum number of points allowed or an
    ///object with key:value pairs matching &#96;updateData&#96;
    ///Reference the Plotly.prependTraces API for full usage:
    ///https://plotly.com/javascript/plotlyjs-function-reference/#plotlyprependtraces
    ///&#10;
    ///• restyleData (list; default null) - Data from latest restyle event which occurs
    ///when the user toggles a legend item, changes
    ///parcoords selections, or other trace-level edits.
    ///Has the form &#96;[edits, indices]&#96;, where &#96;edits&#96; is an object
    ///&#96;{&lt;attr string&gt;: &lt;value&gt;}&#96; describing the changes made,
    ///and &#96;indices&#96; is an array of trace indices that were edited.
    ///Read-only.
    ///&#10;
    ///• figure (record with the fields: 'data: list with values of type: record (optional)', 'layout: record (optional)', 'frames: list with values of type: record (optional)'; default {
    ///    data: [],
    ///    layout: {},
    ///    frames: [],
    ///}) - Plotly &#96;figure&#96; object. See schema:
    ///https://plotly.com/javascript/reference
    ///&#96;config&#96; is set separately by the &#96;config&#96; property
    ///&#10;
    ///• style (record) - Generic style overrides on the plot div
    ///&#10;
    ///• className (string) - className of the parent div
    ///&#10;
    ///• animate (boolean; default false) - Beta: If true, animate between updates using
    ///plotly.js's &#96;animate&#96; function
    ///&#10;
    ///• animation_options (record; default {
    ///    frame: {
    ///        redraw: false,
    ///    },
    ///    transition: {
    ///        duration: 750,
    ///        ease: 'cubic-in-out',
    ///    },
    ///}) - Beta: Object containing animation settings.
    ///Only applies if &#96;animate&#96; is &#96;true&#96;
    ///&#10;
    ///• config (record with the fields: 'staticPlot: boolean (optional)', 'plotlyServerURL: string (optional)', 'editable: boolean (optional)', 'edits: record with the fields: 'annotationPosition: boolean (optional)', 'annotationTail: boolean (optional)', 'annotationText: boolean (optional)', 'axisTitleText: boolean (optional)', 'colorbarPosition: boolean (optional)', 'colorbarTitleText: boolean (optional)', 'legendPosition: boolean (optional)', 'legendText: boolean (optional)', 'shapePosition: boolean (optional)', 'titleText: boolean (optional)' (optional)', 'autosizable: boolean (optional)', 'responsive: boolean (optional)', 'queueLength: number (optional)', 'fillFrame: boolean (optional)', 'frameMargins: number (optional)', 'scrollZoom: boolean (optional)', 'doubleClick: value equal to: 'false', 'reset', 'autosize', 'reset+autosize' (optional)', 'doubleClickDelay: number (optional)', 'showTips: boolean (optional)', 'showAxisDragHandles: boolean (optional)', 'showAxisRangeEntryBoxes: boolean (optional)', 'showLink: boolean (optional)', 'sendData: boolean (optional)', 'linkText: string (optional)', 'displayModeBar: value equal to: 'true', 'false', 'hover' (optional)', 'showSendToCloud: boolean (optional)', 'showEditInChartStudio: boolean (optional)', 'modeBarButtonsToRemove: list (optional)', 'modeBarButtonsToAdd: list (optional)', 'modeBarButtons: boolean | number | string | record | list (optional)', 'toImageButtonOptions: record with the fields: 'format: value equal to: 'jpeg', 'png', 'webp', 'svg' (optional)', 'filename: string (optional)', 'width: number (optional)', 'height: number (optional)', 'scale: number (optional)' (optional)', 'displaylogo: boolean (optional)', 'watermark: boolean (optional)', 'plotGlPixelRatio: number (optional)', 'topojsonURL: string (optional)', 'mapboxAccessToken: boolean | number | string | record | list (optional)', 'locale: string (optional)', 'locales: record (optional)'; default {}) - Plotly.js config options.
    ///See https://plotly.com/javascript/configuration-options/
    ///for more info.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    type Prop =
        | Figure of Plotly.NET.GenericChart.Figure
        | Config of Plotly.NET.Config
        | ClickData of obj
        | ClickAnnotationData of obj
        | HoverData of obj
        | ClearOnUnhover of bool
        | SelectedData of obj
        | RelayoutData of obj
        | ExtendData of obj
        | PrependData of obj
        | RestyleData of obj
        | Style of DashComponentStyle
        | ClassName of string
        | Animate of bool
        | AnimationOptions of obj
        | LoadingState of LoadingState
        static member toDynamicMemberDef (prop:Prop)  =
            match prop with
            | ClickData           p -> "clickData"          , box p
            | ClickAnnotationData p -> "clickAnnotationData", box p
            | HoverData           p -> "hoverData"          , box p
            | ClearOnUnhover      p -> "clear_on_unhover"   , box p
            | SelectedData        p -> "selectedData"       , box p
            | RelayoutData        p -> "relayoutData"       , box p
            | ExtendData          p -> "extendData"         , box p
            | PrependData         p -> "prependData"        , box p
            | RestyleData         p -> "restyleData"        , box p
            | Figure              p -> "figure"             , box p
            | Style               p -> "style"              , box p
            | ClassName           p -> "className"          , box p
            | Animate             p -> "animate"            , box p
            | AnimationOptions    p -> "animation_options"  , box p
            | Config              p -> "config"             , box p
            | LoadingState        p -> "loading_state"      , box p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///Data from latest click event. Read-only.
        ///</summary>
        static member clickData(p: obj) = Prop(ClickData p)
        ///<summary>
        ///Data from latest click annotation event. Read-only.
        ///</summary>
        static member clickAnnotationData(p: obj) = Prop(ClickAnnotationData p)
        ///<summary>
        ///Data from latest hover event. Read-only.
        ///</summary>
        static member hoverData(p: obj) = Prop(HoverData p)
        ///<summary>
        ///If True, &#96;clear_on_unhover&#96; will clear the &#96;hoverData&#96; property
        ///when the user "unhovers" from a point.
        ///If False, then the &#96;hoverData&#96; property will be equal to the
        ///data from the last point that was hovered over.
        ///</summary>
        static member clearOnUnhover(p: bool) = Prop(ClearOnUnhover p)
        ///<summary>
        ///Data from latest select event. Read-only.
        ///</summary>
        static member selectedData(p: obj) = Prop(SelectedData p)
        ///<summary>
        ///Data from latest relayout event which occurs
        ///when the user zooms or pans on the plot or other
        ///layout-level edits. Has the form &#96;{&lt;attr string&gt;: &lt;value&gt;}&#96;
        ///describing the changes made. Read-only.
        ///</summary>
        static member relayoutData(p: obj) = Prop(RelayoutData p)
        
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
        static member extendData(p: obj) = Prop(ExtendData p)

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
        static member prependData(p: obj) = Prop(PrependData p)

        ///<summary>
        ///Data from latest restyle event which occurs
        ///when the user toggles a legend item, changes
        ///parcoords selections, or other trace-level edits.
        ///Has the form &#96;[edits, indices]&#96;, where &#96;edits&#96; is an object
        ///&#96;{&lt;attr string&gt;: &lt;value&gt;}&#96; describing the changes made,
        ///and &#96;indices&#96; is an array of trace indices that were edited.
        ///Read-only.
        ///</summary>
        static member restyleData(p: list<obj>) = Prop(RestyleData p)
        ///<summary>
        ///Plotly &#96;figure&#96; object. See schema:
        ///https://plotly.com/javascript/reference
        ///&#96;config&#96; is set separately by the &#96;config&#96; property
        ///</summary>
        static member figure(p: Plotly.NET.GenericChart.Figure) = Prop(Figure p)
        ///<summary>
        ///Generic style overrides on the plot div
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///className of the parent div
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Beta: If true, animate between updates using
        ///plotly.js's &#96;animate&#96; function
        ///</summary>
        static member animate(p: bool) = Prop(Animate p)
        ///<summary>
        ///Beta: Object containing animation settings.
        ///Only applies if &#96;animate&#96; is &#96;true&#96;
        ///</summary>
        static member animationOptions(p: obj) = Prop(AnimationOptions p)
        ///<summary>
        ///Plotly.js config options.
        ///See https://plotly.com/javascript/configuration-options/
        ///for more info.
        ///</summary>
        static member config(p: Plotly.NET.Config) = Prop(Config p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = Prop(LoadingState p)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: DashComponent) = Children([ value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: list<DashComponent>) = Children(value)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

    ///<summary>
    ///Graph can be used to render any plotly.js-powered data visualization.
    ///You can define callbacks based on user interaction with Graphs such as
    ///hovering, clicking or selecting
    ///</summary>
    type Graph() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?responsive: string,
                ?clickData: string,
                ?clickAnnotationData: string,
                ?hoverData: string,
                ?clear_on_unhover: string,
                ?selectedData: string,
                ?relayoutData: string,
                ?extendData: string,
                ?prependData: string,
                ?restyleData: string,
                ?figure: string,
                ?style: string,
                ?className: string,
                ?animate: string,
                ?animation_options: string,
                ?config: string,
                ?loading_state: string
            ) =
            (fun (t: Graph) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "responsive" responsive
                DynObj.setValueOpt props "clickData" clickData
                DynObj.setValueOpt props "clickAnnotationData" clickAnnotationData
                DynObj.setValueOpt props "hoverData" hoverData
                DynObj.setValueOpt props "clear_on_unhover" clear_on_unhover
                DynObj.setValueOpt props "selectedData" selectedData
                DynObj.setValueOpt props "relayoutData" relayoutData
                DynObj.setValueOpt props "extendData" extendData
                DynObj.setValueOpt props "prependData" prependData
                DynObj.setValueOpt props "restyleData" restyleData
                DynObj.setValueOpt props "figure" figure
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "animate" animate
                DynObj.setValueOpt props "animation_options" animation_options
                DynObj.setValueOpt props "config" config
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Graph"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?responsive: string,
                ?clickData: string,
                ?clickAnnotationData: string,
                ?hoverData: string,
                ?clear_on_unhover: string,
                ?selectedData: string,
                ?relayoutData: string,
                ?extendData: string,
                ?prependData: string,
                ?restyleData: string,
                ?figure: string,
                ?style: string,
                ?className: string,
                ?animate: string,
                ?animation_options: string,
                ?config: string,
                ?loading_state: string
            ) =
            Graph.applyMembers
                (id,
                 children,
                 ?responsive = responsive,
                 ?clickData = clickData,
                 ?clickAnnotationData = clickAnnotationData,
                 ?hoverData = hoverData,
                 ?clear_on_unhover = clear_on_unhover,
                 ?selectedData = selectedData,
                 ?relayoutData = relayoutData,
                 ?extendData = extendData,
                 ?prependData = prependData,
                 ?restyleData = restyleData,
                 ?figure = figure,
                 ?style = style,
                 ?className = className,
                 ?animate = animate,
                 ?animation_options = animation_options,
                 ?config = config,
                 ?loading_state = loading_state)
                (Graph())

    ///<summary>
    ///Graph can be used to render any plotly.js-powered data visualization.
    ///You can define callbacks based on user interaction with Graphs such as
    ///hovering, clicking or selecting
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• responsive (value equal to: 'true', 'false', 'auto'; default auto) - If True, the Plotly.js plot will be fully responsive to window resize
    ///and parent element resize event. This is achieved by overriding
    ///&#96;config.responsive&#96; to True, &#96;figure.layout.autosize&#96; to True and unsetting
    ///&#96;figure.layout.height&#96; and &#96;figure.layout.width&#96;.
    ///If False, the Plotly.js plot not be responsive to window resize and
    ///parent element resize event. This is achieved by overriding &#96;config.responsive&#96;
    ///to False and &#96;figure.layout.autosize&#96; to False.
    ///If 'auto' (default), the Graph will determine if the Plotly.js plot can be made fully
    ///responsive (True) or not (False) based on the values in &#96;config.responsive&#96;,
    ///&#96;figure.layout.autosize&#96;, &#96;figure.layout.height&#96;, &#96;figure.layout.width&#96;.
    ///This is the legacy behavior of the Graph component.
    ///Needs to be combined with appropriate dimension / styling through the &#96;style&#96; prop
    ///to fully take effect.
    ///&#10;
    ///• clickData (record; default null) - Data from latest click event. Read-only.
    ///&#10;
    ///• clickAnnotationData (record; default null) - Data from latest click annotation event. Read-only.
    ///&#10;
    ///• hoverData (record; default null) - Data from latest hover event. Read-only.
    ///&#10;
    ///• clear_on_unhover (boolean; default false) - If True, &#96;clear_on_unhover&#96; will clear the &#96;hoverData&#96; property
    ///when the user "unhovers" from a point.
    ///If False, then the &#96;hoverData&#96; property will be equal to the
    ///data from the last point that was hovered over.
    ///&#10;
    ///• selectedData (record; default null) - Data from latest select event. Read-only.
    ///&#10;
    ///• relayoutData (record; default null) - Data from latest relayout event which occurs
    ///when the user zooms or pans on the plot or other
    ///layout-level edits. Has the form &#96;{&lt;attr string&gt;: &lt;value&gt;}&#96;
    ///describing the changes made. Read-only.
    ///&#10;
    ///• extendData (list | record; default null) - Data that should be appended to existing traces. Has the form
    ///&#96;[updateData, traceIndices, maxPoints]&#96;, where &#96;updateData&#96; is an object
    ///containing the data to extend, &#96;traceIndices&#96; (optional) is an array of
    ///trace indices that should be extended, and &#96;maxPoints&#96; (optional) is
    ///either an integer defining the maximum number of points allowed or an
    ///object with key:value pairs matching &#96;updateData&#96;
    ///Reference the Plotly.extendTraces API for full usage:
    ///https://plotly.com/javascript/plotlyjs-function-reference/#plotlyextendtraces
    ///&#10;
    ///• prependData (list | record; default null) - Data that should be prepended to existing traces. Has the form
    ///&#96;[updateData, traceIndices, maxPoints]&#96;, where &#96;updateData&#96; is an object
    ///containing the data to prepend, &#96;traceIndices&#96; (optional) is an array of
    ///trace indices that should be prepended, and &#96;maxPoints&#96; (optional) is
    ///either an integer defining the maximum number of points allowed or an
    ///object with key:value pairs matching &#96;updateData&#96;
    ///Reference the Plotly.prependTraces API for full usage:
    ///https://plotly.com/javascript/plotlyjs-function-reference/#plotlyprependtraces
    ///&#10;
    ///• restyleData (list; default null) - Data from latest restyle event which occurs
    ///when the user toggles a legend item, changes
    ///parcoords selections, or other trace-level edits.
    ///Has the form &#96;[edits, indices]&#96;, where &#96;edits&#96; is an object
    ///&#96;{&lt;attr string&gt;: &lt;value&gt;}&#96; describing the changes made,
    ///and &#96;indices&#96; is an array of trace indices that were edited.
    ///Read-only.
    ///&#10;
    ///• figure (record with the fields: 'data: list with values of type: record (optional)', 'layout: record (optional)', 'frames: list with values of type: record (optional)'; default {
    ///    data: [],
    ///    layout: {},
    ///    frames: [],
    ///}) - Plotly &#96;figure&#96; object. See schema:
    ///https://plotly.com/javascript/reference
    ///&#96;config&#96; is set separately by the &#96;config&#96; property
    ///&#10;
    ///• style (record) - Generic style overrides on the plot div
    ///&#10;
    ///• className (string) - className of the parent div
    ///&#10;
    ///• animate (boolean; default false) - Beta: If true, animate between updates using
    ///plotly.js's &#96;animate&#96; function
    ///&#10;
    ///• animation_options (record; default {
    ///    frame: {
    ///        redraw: false,
    ///    },
    ///    transition: {
    ///        duration: 750,
    ///        ease: 'cubic-in-out',
    ///    },
    ///}) - Beta: Object containing animation settings.
    ///Only applies if &#96;animate&#96; is &#96;true&#96;
    ///&#10;
    ///• config (record with the fields: 'staticPlot: boolean (optional)', 'plotlyServerURL: string (optional)', 'editable: boolean (optional)', 'edits: record with the fields: 'annotationPosition: boolean (optional)', 'annotationTail: boolean (optional)', 'annotationText: boolean (optional)', 'axisTitleText: boolean (optional)', 'colorbarPosition: boolean (optional)', 'colorbarTitleText: boolean (optional)', 'legendPosition: boolean (optional)', 'legendText: boolean (optional)', 'shapePosition: boolean (optional)', 'titleText: boolean (optional)' (optional)', 'autosizable: boolean (optional)', 'responsive: boolean (optional)', 'queueLength: number (optional)', 'fillFrame: boolean (optional)', 'frameMargins: number (optional)', 'scrollZoom: boolean (optional)', 'doubleClick: value equal to: 'false', 'reset', 'autosize', 'reset+autosize' (optional)', 'doubleClickDelay: number (optional)', 'showTips: boolean (optional)', 'showAxisDragHandles: boolean (optional)', 'showAxisRangeEntryBoxes: boolean (optional)', 'showLink: boolean (optional)', 'sendData: boolean (optional)', 'linkText: string (optional)', 'displayModeBar: value equal to: 'true', 'false', 'hover' (optional)', 'showSendToCloud: boolean (optional)', 'showEditInChartStudio: boolean (optional)', 'modeBarButtonsToRemove: list (optional)', 'modeBarButtonsToAdd: list (optional)', 'modeBarButtons: boolean | number | string | record | list (optional)', 'toImageButtonOptions: record with the fields: 'format: value equal to: 'jpeg', 'png', 'webp', 'svg' (optional)', 'filename: string (optional)', 'width: number (optional)', 'height: number (optional)', 'scale: number (optional)' (optional)', 'displaylogo: boolean (optional)', 'watermark: boolean (optional)', 'plotGlPixelRatio: number (optional)', 'topojsonURL: string (optional)', 'mapboxAccessToken: boolean | number | string | record | list (optional)', 'locale: string (optional)', 'locales: record (optional)'; default {}) - Plotly.js config options.
    ///See https://plotly.com/javascript/configuration-options/
    ///for more info.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    let graph (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Graph.init (id, children)

        let componentProps =
            match t.TryGetTypedValue<DashComponentProps> "props" with
            | Some (p) -> p
            | None -> DashComponentProps()

        Seq.iter
            (fun (prop: Prop) ->
                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
                DynObj.setValue componentProps fieldName boxedProp)
            props

        DynObj.setValue t "props" componentProps
        t :> DashComponent
