namespace Dash.NET.CSharp.DCC

open System
open ComponentPropTypes
open Dash.NET.CSharp.ComponentStyle

///<summary>
///Part of dcc.Tabs - this is the child Tab component used to render a tabbed page.
///Its children will be set as the content of that tab, which if clicked will become visible.
///</summary>
[<RequireQualifiedAccess>]
module Tab =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Tab.Attr


    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Tab.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Tab.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///The tab's label
        ///</summary>
        static member label(p: string) = 
            guardAgainstNull "p" p
            OAttr.label p |> Attr.Wrap
        ///<summary>
        ///Value for determining which Tab is currently selected
        ///</summary>
        static member value(p: string) = 
            guardAgainstNull "p" p
            OAttr.value p |> Attr.Wrap
        ///<summary>
        ///Determines if tab is disabled or not - defaults to false
        ///</summary>
        static member disabled(p: bool) = 
            guardAgainstNull "p" p
            OAttr.disabled p |> Attr.Wrap
        ///<summary>
        ///Overrides the default (inline) styles when disabled
        ///</summary>
        static member disabledStyle([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.disabledStyle (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Appends a class to the Tab component when it is disabled.
        ///</summary>
        static member disabledClassName(p: string) = 
            guardAgainstNull "p" p
            OAttr.disabledClassName p |> Attr.Wrap
        ///<summary>
        ///Appends a class to the Tab component.
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            OAttr.className p |> Attr.Wrap
        ///<summary>
        ///Appends a class to the Tab component when it is selected.
        ///</summary>
        static member selectedClassName(p: string) = 
            guardAgainstNull "p" p
            OAttr.selectedClassName p |> Attr.Wrap
        ///<summary>
        ///Overrides the default (inline) styles for the Tab component.
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.style (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Overrides the default (inline) styles for the Tab component when it is selected.
        ///</summary>
        static member selectedStyle(p: string) = 
            guardAgainstNull "p" p
            OAttr.selectedStyle p |> Attr.Wrap
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = 
            guardAgainstNull "p" p
            OAttr.loadingState (p |> LoadingState.Convert) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Guid) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Dash.NET.CSharp.Html.DashComponent) = 
            guardAgainstNull "value" value
            OAttr.children (value |> Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children([<ParamArray>] value: array<Dash.NET.CSharp.Html.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            OAttr.children (value |> Array.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<Dash.NET.CSharp.Html.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Seq.iter (guardAgainstNull "value")
            OAttr.children (value |> Seq.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap

    ///<summary>
    ///Part of dcc.Tabs - this is the child Tab component used to render a tabbed page.
    ///Its children will be set as the content of that tab, which if clicked will become visible.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• label (string) - The tab's label
    ///&#10;
    ///• children (a list of or a singular dash component, string or number) - The content of the tab - will only be displayed if this tab is selected
    ///&#10;
    ///• value (string) - Value for determining which Tab is currently selected
    ///&#10;
    ///• disabled (boolean; default false) - Determines if tab is disabled or not - defaults to false
    ///&#10;
    ///• disabled_style (record; default {
    ///    color: '#d6d6d6',
    ///}) - Overrides the default (inline) styles when disabled
    ///&#10;
    ///• disabled_className (string) - Appends a class to the Tab component when it is disabled.
    ///&#10;
    ///• className (string) - Appends a class to the Tab component.
    ///&#10;
    ///• selected_className (string) - Appends a class to the Tab component when it is selected.
    ///&#10;
    ///• style (record) - Overrides the default (inline) styles for the Tab component.
    ///&#10;
    ///• selected_style (record) - Overrides the default (inline) styles for the Tab component when it is selected.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    let tab (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Tab.tab id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Html.DashComponent.Wrap