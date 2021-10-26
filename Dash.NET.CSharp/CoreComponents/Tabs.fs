namespace Dash.NET.CSharp.DCC

open System
open Dash.NET.CSharp.ComponentStyle

///<summary>
///A Dash component that lets you render pages with tabs - the Tabs component's children
///can be dcc.Tab components, which can hold a label that will be displayed as a tab, and can in turn hold
///children components that will be that tab's content.
///</summary>
[<RequireQualifiedAccess>]
module Tabs =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Tabs.Attr


    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Tabs.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Tabs.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///The value of the currently selected Tab
        ///</summary>
        static member value(p: string) = 
            guardAgainstNull "p" p
            OAttr.value p
        ///<summary>
        ///Appends a class to the Tabs container holding the individual Tab components.
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            OAttr.className p
        ///<summary>
        ///Appends a class to the Tab content container holding the children of the Tab that is selected.
        ///</summary>
        static member contentClassName(p: string) = 
            guardAgainstNull "p" p
            OAttr.contentClassName p
        ///<summary>
        ///Appends a class to the top-level parent container holding both the Tabs container and the content container.
        ///</summary>
        static member parentClassName(p: string) = 
            guardAgainstNull "p" p
            OAttr.parentClassName p
        ///<summary>
        ///Appends (inline) styles to the Tabs container holding the individual Tab components.
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Dsl.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.style (p |> Array.map Dash.NET.CSharp.Dsl.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Appends (inline) styles to the top-level parent container holding both the Tabs container and the content container.
        ///</summary>
        static member parentStyle([<ParamArray>] p: array<Dash.NET.CSharp.Dsl.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.parentStyle (p |> Array.map Dash.NET.CSharp.Dsl.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Appends (inline) styles to the tab content container holding the children of the Tab that is selected.
        ///</summary>
        static member contentStyle([<ParamArray>] p: array<Dash.NET.CSharp.Dsl.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.contentStyle (p |> Array.map Dash.NET.CSharp.Dsl.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Renders the tabs vertically (on the side)
        ///</summary>
        static member vertical(p: bool) = 
            guardAgainstNull "p" p
            OAttr.vertical p
        ///<summary>
        ///Breakpoint at which tabs are rendered full width (can be 0 if you don't want full width tabs on mobile)
        ///</summary>
        static member mobileBreakpoint(p: float) = 
            guardAgainstNull "p" p
            OAttr.mobileBreakpoint p
        ///<summary>
        ///Holds the colors used by the Tabs and Tab components. If you set these, you should specify colors for all properties, so:
        ///colors: {
        ///   border: '#d6d6d6',
        ///   primary: '#1975FA',
        ///   background: '#f9f9f9'
        /// }
        ///</summary>
        static member colors(p: TabColors) = 
            guardAgainstNull "p" p
            OAttr.colors (p |> TabColors.Convert)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = 
            guardAgainstNull "p" p
            OAttr.loadingState (p |> LoadingState.Convert)

        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: IConvertible) = 
            guardAgainstNull "p" p
            OAttr.persistence p

        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps([<ParamArray>] p: string []) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.persistedProps p
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType(p: PersistenceTypeOptions) = 
            guardAgainstNull "p" p
            OAttr.persistenceType (p |> PersistenceTypeOptions.Unwrap)
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
        static member children(value: Dash.NET.CSharp.Dsl.DashComponent) = 
            guardAgainstNull "value" value
            OAttr.children (value |> Dash.NET.CSharp.Dsl.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children([<ParamArray>] value: array<Dash.NET.CSharp.Dsl.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            OAttr.children (value |> Array.map Dash.NET.CSharp.Dsl.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<Dash.NET.CSharp.Dsl.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Seq.iter (guardAgainstNull "value")
            OAttr.children (value |> Seq.map Dash.NET.CSharp.Dsl.DashComponent.Unwrap) |> Attr.Wrap


    ///<summary>
    ///A Dash component that lets you render pages with tabs - the Tabs component's children
    ///can be dcc.Tab components, which can hold a label that will be displayed as a tab, and can in turn hold
    ///children components that will be that tab's content.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• value (string) - The value of the currently selected Tab
    ///&#10;
    ///• className (string) - Appends a class to the Tabs container holding the individual Tab components.
    ///&#10;
    ///• content_className (string) - Appends a class to the Tab content container holding the children of the Tab that is selected.
    ///&#10;
    ///• parent_className (string) - Appends a class to the top-level parent container holding both the Tabs container and the content container.
    ///&#10;
    ///• style (record) - Appends (inline) styles to the Tabs container holding the individual Tab components.
    ///&#10;
    ///• parent_style (record) - Appends (inline) styles to the top-level parent container holding both the Tabs container and the content container.
    ///&#10;
    ///• content_style (record) - Appends (inline) styles to the tab content container holding the children of the Tab that is selected.
    ///&#10;
    ///• vertical (boolean; default false) - Renders the tabs vertically (on the side)
    ///&#10;
    ///• mobile_breakpoint (number; default 800) - Breakpoint at which tabs are rendered full width (can be 0 if you don't want full width tabs on mobile)
    ///&#10;
    ///• children (list with values of type: a list of or a singular dash component, string or number | a list of or a singular dash component, string or number) - Array that holds Tab components
    ///&#10;
    ///• colors (record with the fields: 'border: string (optional)', 'primary: string (optional)', 'background: string (optional)'; default {
    ///    border: '#d6d6d6',
    ///    primary: '#1975FA',
    ///    background: '#f9f9f9',
    ///}) - Holds the colors used by the Tabs and Tab components. If you set these, you should specify colors for all properties, so:
    ///colors: {
    ///   border: '#d6d6d6',
    ///   primary: '#1975FA',
    ///   background: '#f9f9f9'
    /// }
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///&#10;
    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
    ///hasn't changed from its previous value, a &#96;value&#96; that the user has
    ///changed while using the app will keep that change, as long as
    ///the new &#96;value&#96; also matches what was given originally.
    ///Used in conjunction with &#96;persistence_type&#96;.
    ///&#10;
    ///• persisted_props (list with values of type: value equal to: 'value'; default ['value']) - Properties whose user interactions will persist after refreshing the
    ///component or the page. Since only &#96;value&#96; is allowed this prop can
    ///normally be ignored.
    ///&#10;
    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///</summary>
    let tabs (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Tabs.tabs id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap