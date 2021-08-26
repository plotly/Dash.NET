namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

///<summary>
///RadioItems is a component that encapsulates several radio item inputs.
///The values and labels of the RadioItems is specified in the &#96;options&#96;
///property and the seleced item is specified with the &#96;value&#96; property.
///Each radio item is rendered as an input with a surrounding label.
///</summary>
[<RequireQualifiedAccess>]
module RadioItems =
    ///<summary>
    ///• options (list with values of type: record with the fields: 'label: string | number (required)', 'value: string | number (required)', 'disabled: boolean (optional)'; default []) - An array of options
    ///&#10;
    ///• value (string | number) - The currently selected value
    ///&#10;
    ///• style (record) - The style of the container (div)
    ///&#10;
    ///• className (string) - The class of the container (div)
    ///&#10;
    ///• inputStyle (record; default {}) - The style of the &lt;input&gt; radio element
    ///&#10;
    ///• inputClassName (string; default ) - The class of the &lt;input&gt; radio element
    ///&#10;
    ///• labelStyle (record; default {}) - The style of the &lt;label&gt; that wraps the radio input
    /// and the option's label
    ///&#10;
    ///• labelClassName (string; default ) - The class of the &lt;label&gt; that wraps the radio input
    /// and the option's label
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
        | ClassName of string
        | Style of DashComponentStyle
        | Options of seq<RadioItemsOption>
        | Value of IConvertible
        | InputClassName of string
        | InputStyle of DashComponentStyle
        | LabelClassName of string
        | LabelStyle of DashComponentStyle
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions
        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | ClassName         p   -> "className"          , box p
            | Style             p   -> "style"              , box p
            | Options           p   -> "options"            , box p
            | Value             p   -> "value"              , box p
            | InputClassName    p   -> "inputClassName"     , box p
            | InputStyle        p   -> "inputStyle"         , box p
            | LabelClassName    p   -> "labelClassName"     , box p
            | LabelStyle        p   -> "labelStyle"         , box p
            | LoadingState      p   -> "loading_state"      , box p
            | Persistence       p   -> "persistence"        , box p
            | PersistedProps    p   -> "persisted_props"    , box p
            | PersistenceType   p   -> "persistence_type"   , PersistenceTypeOptions.convert p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///An array of options
        ///</summary>
        static member options(p: seq<RadioItemsOption>) = Prop(Options p)
        ///<summary>
        ///The currently selected value
        ///</summary>
        static member value(p: IConvertible) = Prop(Value p)
        ///<summary>
        ///The style of the container (div)
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///The class of the container (div)
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///The style of the &lt;input&gt; radio element
        ///</summary>
        static member inputStyle(p: seq<Css.Style>) = Prop(InputStyle(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///The class of the &lt;input&gt; radio element
        ///</summary>
        static member inputClassName(p: string) = Prop(InputClassName p)
        ///<summary>
        ///The style of the &lt;label&gt; that wraps the radio input
        /// and the option's label
        ///</summary>
        static member labelStyle(p: seq<Css.Style>) = Prop(LabelStyle(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///The class of the &lt;label&gt; that wraps the radio input
        /// and the option's label
        ///</summary>
        static member labelClassName(p: string) = Prop(LabelClassName p)

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
        static member persistence(p: IConvertible) =
            Prop(Persistence p)
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
    ///RadioItems is a component that encapsulates several radio item inputs.
    ///The values and labels of the RadioItems is specified in the &#96;options&#96;
    ///property and the seleced item is specified with the &#96;value&#96; property.
    ///Each radio item is rendered as an input with a surrounding label.
    ///</summary>
    type RadioItems() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?options: string,
                ?value: string,
                ?style: string,
                ?className: string,
                ?inputStyle: string,
                ?inputClassName: string,
                ?labelStyle: string,
                ?labelClassName: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            (fun (t: RadioItems) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "options" options
                DynObj.setValueOpt props "value" value
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "inputStyle" inputStyle
                DynObj.setValueOpt props "inputClassName" inputClassName
                DynObj.setValueOpt props "labelStyle" labelStyle
                DynObj.setValueOpt props "labelClassName" labelClassName
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValueOpt props "persistence" persistence
                DynObj.setValueOpt props "persisted_props" persisted_props
                DynObj.setValueOpt props "persistence_type" persistence_type
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "RadioItems"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?options: string,
                ?value: string,
                ?style: string,
                ?className: string,
                ?inputStyle: string,
                ?inputClassName: string,
                ?labelStyle: string,
                ?labelClassName: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            RadioItems.applyMembers
                (id,
                 children,
                 ?options = options,
                 ?value = value,
                 ?style = style,
                 ?className = className,
                 ?inputStyle = inputStyle,
                 ?inputClassName = inputClassName,
                 ?labelStyle = labelStyle,
                 ?labelClassName = labelClassName,
                 ?loading_state = loading_state,
                 ?persistence = persistence,
                 ?persisted_props = persisted_props,
                 ?persistence_type = persistence_type)
                (RadioItems())

    ///<summary>
    ///RadioItems is a component that encapsulates several radio item inputs.
    ///The values and labels of the RadioItems is specified in the &#96;options&#96;
    ///property and the seleced item is specified with the &#96;value&#96; property.
    ///Each radio item is rendered as an input with a surrounding label.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• options (list with values of type: record with the fields: 'label: string | number (required)', 'value: string | number (required)', 'disabled: boolean (optional)'; default []) - An array of options
    ///&#10;
    ///• value (string | number) - The currently selected value
    ///&#10;
    ///• style (record) - The style of the container (div)
    ///&#10;
    ///• className (string) - The class of the container (div)
    ///&#10;
    ///• inputStyle (record; default {}) - The style of the &lt;input&gt; radio element
    ///&#10;
    ///• inputClassName (string; default ) - The class of the &lt;input&gt; radio element
    ///&#10;
    ///• labelStyle (record; default {}) - The style of the &lt;label&gt; that wraps the radio input
    /// and the option's label
    ///&#10;
    ///• labelClassName (string; default ) - The class of the &lt;label&gt; that wraps the radio input
    /// and the option's label
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
    let radioItems (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = RadioItems.init (id, children)

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
