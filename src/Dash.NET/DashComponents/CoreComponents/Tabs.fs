namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

///<summary>
///A Dash component that lets you render pages with tabs - the Tabs component's children
///can be dcc.Tab components, which can hold a label that will be displayed as a tab, and can in turn hold
///children components that will be that tab's content.
///</summary>
[<RequireQualifiedAccess>]
module Tabs =
    ///<summary>
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
    type Prop =
        | ClassName         of string
        | Style             of DashComponentStyle
        | Value             of string
        | ContentClassName  of string
        | ParentClassName   of string
        | ParentStyle       of DashComponentStyle
        | ContentStyle      of DashComponentStyle
        | Vertical          of bool
        | MobileBreakpoint  of float
        | Colors            of TabColors
        | LoadingState      of LoadingState
        | Persistence       of IConvertible
        | PersistedProps    of string []
        | PersistenceType   of PersistenceTypeOptions

        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | ClassName p           -> "className", box p
            | Style p               -> "style" , box p
            | Value p               -> "value" , box p
            | ContentClassName p    -> "content_className" , box p
            | ParentClassName p     -> "parent_className" , box p
            | ParentStyle p         -> "parent_style" , box p
            | ContentStyle p        -> "content_style" , box p
            | Vertical p            -> "vertical" , box p
            | MobileBreakpoint p    -> "mobile_breakpoint" , box p
            | Colors p              -> "colors" , box p
            | LoadingState p        -> "loading_state", box p
            | Persistence p         -> "persistence", box p
            | PersistedProps p      -> "persisted_props", box p
            | PersistenceType p     -> "persistence_type", PersistenceTypeOptions.convert p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The value of the currently selected Tab
        ///</summary>
        static member value(p: string) = Prop(Value p)
        ///<summary>
        ///Appends a class to the Tabs container holding the individual Tab components.
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Appends a class to the Tab content container holding the children of the Tab that is selected.
        ///</summary>
        static member contentClassName(p: string) = Prop(ContentClassName p)
        ///<summary>
        ///Appends a class to the top-level parent container holding both the Tabs container and the content container.
        ///</summary>
        static member parentClassName(p: string) = Prop(ParentClassName p)
        ///<summary>
        ///Appends (inline) styles to the Tabs container holding the individual Tab components.
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Appends (inline) styles to the top-level parent container holding both the Tabs container and the content container.
        ///</summary>
        static member parentStyle(p: seq<Css.Style>) = Prop(ParentStyle(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Appends (inline) styles to the tab content container holding the children of the Tab that is selected.
        ///</summary>
        static member contentStyle(p: seq<Css.Style>) = Prop(ContentStyle(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Renders the tabs vertically (on the side)
        ///</summary>
        static member vertical(p: bool) = Prop(Vertical p)
        ///<summary>
        ///Breakpoint at which tabs are rendered full width (can be 0 if you don't want full width tabs on mobile)
        ///</summary>
        static member mobileBreakpoint(p: float) = Prop(MobileBreakpoint p)
        ///<summary>
        ///Holds the colors used by the Tabs and Tab components. If you set these, you should specify colors for all properties, so:
        ///colors: {
        ///   border: '#d6d6d6',
        ///   primary: '#1975FA',
        ///   background: '#f9f9f9'
        /// }
        ///</summary>
        static member colors(p: TabColors) = Prop(Colors p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = Prop(LoadingState p)

        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: IConvertible) = Prop(Persistence p)

        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps(p: string []) = Prop(PersistedProps p)
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType(p: PersistenceTypeOptions) = Prop(PersistenceType p)
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
    ///A Dash component that lets you render pages with tabs - the Tabs component's children
    ///can be dcc.Tab components, which can hold a label that will be displayed as a tab, and can in turn hold
    ///children components that will be that tab's content.
    ///</summary>
    type Tabs() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?value: string,
                ?className: string,
                ?content_className: string,
                ?parent_className: string,
                ?style: string,
                ?parent_style: string,
                ?content_style: string,
                ?vertical: string,
                ?mobile_breakpoint: string,
                ?colors: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            (fun (t: Tabs) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "value" value
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "content_className" content_className
                DynObj.setValueOpt props "parent_className" parent_className
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "parent_style" parent_style
                DynObj.setValueOpt props "content_style" content_style
                DynObj.setValueOpt props "vertical" vertical
                DynObj.setValueOpt props "mobile_breakpoint" mobile_breakpoint
                DynObj.setValueOpt props "colors" colors
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValueOpt props "persistence" persistence
                DynObj.setValueOpt props "persisted_props" persisted_props
                DynObj.setValueOpt props "persistence_type" persistence_type
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Tabs"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?value: string,
                ?className: string,
                ?content_className: string,
                ?parent_className: string,
                ?style: string,
                ?parent_style: string,
                ?content_style: string,
                ?vertical: string,
                ?mobile_breakpoint: string,
                ?colors: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            Tabs.applyMembers
                (id,
                 children,
                 ?value = value,
                 ?className = className,
                 ?content_className = content_className,
                 ?parent_className = parent_className,
                 ?style = style,
                 ?parent_style = parent_style,
                 ?content_style = content_style,
                 ?vertical = vertical,
                 ?mobile_breakpoint = mobile_breakpoint,
                 ?colors = colors,
                 ?loading_state = loading_state,
                 ?persistence = persistence,
                 ?persisted_props = persisted_props,
                 ?persistence_type = persistence_type)
                (Tabs())

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
    let tabs (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Tabs.init (id, children)

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
