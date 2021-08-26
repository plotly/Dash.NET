namespace Dash.NET.DCC

open Dash.NET
open Plotly.NET
open ComponentPropTypes

///<summary>
///Part of dcc.Tabs - this is the child Tab component used to render a tabbed page.
///Its children will be set as the content of that tab, which if clicked will become visible.
///</summary>
[<RequireQualifiedAccess>]
module Tab =
    ///<summary>
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
    type Prop =
        | ClassName         of string
        | Style             of DashComponentStyle
        | Label             of string
        | Value             of string
        | Disabled          of bool
        | DisabledStyle     of DashComponentStyle
        | DisabledClassName of string
        | SelectedClassName of string
        | SelectedStyle     of string
        | LoadingState      of LoadingState

        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | ClassName p           -> "className", box p
            | Style p               -> "style", box p
            | Label p               -> "label", box p
            | Value p               -> "value", box p
            | Disabled p            -> "disabled", box p
            | DisabledStyle p       -> "disabled_style", box p
            | DisabledClassName p   -> "disabled_className", box p
            | SelectedClassName p   -> "selected_className", box p
            | SelectedStyle p       -> "selected_style", box p
            | LoadingState p        -> "loading_state", box p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The tab's label
        ///</summary>
        static member label(p: string) = Prop(Label p)
        ///<summary>
        ///Value for determining which Tab is currently selected
        ///</summary>
        static member value(p: string) = Prop(Value p)
        ///<summary>
        ///Determines if tab is disabled or not - defaults to false
        ///</summary>
        static member disabled(p: bool) = Prop(Disabled p)
        ///<summary>
        ///Overrides the default (inline) styles when disabled
        ///</summary>
        static member disabledStyle(p: seq<Css.Style>) = Prop(DisabledStyle(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Appends a class to the Tab component when it is disabled.
        ///</summary>
        static member disabledClassName(p: string) = Prop(DisabledClassName p)
        ///<summary>
        ///Appends a class to the Tab component.
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Appends a class to the Tab component when it is selected.
        ///</summary>
        static member selectedClassName(p: string) = Prop(SelectedClassName p)
        ///<summary>
        ///Overrides the default (inline) styles for the Tab component.
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Overrides the default (inline) styles for the Tab component when it is selected.
        ///</summary>
        static member selectedStyle(p: string) = Prop(SelectedStyle p)
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
    ///Part of dcc.Tabs - this is the child Tab component used to render a tabbed page.
    ///Its children will be set as the content of that tab, which if clicked will become visible.
    ///</summary>
    type Tab() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?label: string,
                ?value: string,
                ?disabled: string,
                ?disabled_style: string,
                ?disabled_className: string,
                ?className: string,
                ?selected_className: string,
                ?style: string,
                ?selected_style: string,
                ?loading_state: string
            ) =
            (fun (t: Tab) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "label" label
                DynObj.setValueOpt props "value" value
                DynObj.setValueOpt props "disabled" disabled
                DynObj.setValueOpt props "disabled_style" disabled_style
                DynObj.setValueOpt props "disabled_className" disabled_className
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "selected_className" selected_className
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "selected_style" selected_style
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Tab"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?label: string,
                ?value: string,
                ?disabled: string,
                ?disabled_style: string,
                ?disabled_className: string,
                ?className: string,
                ?selected_className: string,
                ?style: string,
                ?selected_style: string,
                ?loading_state: string
            ) =
            Tab.applyMembers
                (id,
                 children,
                 ?label = label,
                 ?value = value,
                 ?disabled = disabled,
                 ?disabled_style = disabled_style,
                 ?disabled_className = disabled_className,
                 ?className = className,
                 ?selected_className = selected_className,
                 ?style = style,
                 ?selected_style = selected_style,
                 ?loading_state = loading_state)
                (Tab())

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
    let tab (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Tab.init (id, children)

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
