namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

///<summary>
///Dropdown is an interactive dropdown element for selecting one or more
///items.
///The values and labels of the dropdown items are specified in the &#96;options&#96;
///property and the selected item(s) are specified with the &#96;value&#96; property.
///Use a dropdown when you have many options (more than 5) or when you are
///constrained for space. Otherwise, you can use RadioItems or a Checklist,
///which have the benefit of showing the users all of the items at once.
///</summary>
[<RequireQualifiedAccess>]
module Dropdown =
    ///<summary>
    ///string | number | list with values of type: string | number
    ///</summary>
    type DropdownValue =
        | SingleValue of IConvertible
        | MultipleValues of seq<IConvertible>
        static member convert = function
            | SingleValue    p -> box p
            | MultipleValues p -> box p

    ///<summary>
    ///• options (list with values of type: record with the fields: 'label: string | number (required)', 'value: string | number (required)', 'disabled: boolean (optional)', 'title: string (optional)') - An array of options {label: [string|number], value: [string|number]},
    ///an optional disabled field can be used for each option
    ///&#10;
    ///• value (string | number | list with values of type: string | number) - The value of the input. If &#96;multi&#96; is false (the default)
    ///then value is just a string that corresponds to the values
    ///provided in the &#96;options&#96; property. If &#96;multi&#96; is true, then
    ///multiple values can be selected at once, and &#96;value&#96; is an
    ///array of items with values corresponding to those in the
    ///&#96;options&#96; prop.
    ///&#10;
    ///• optionHeight (number; default 35) - height of each option. Can be increased when label lengths would wrap around
    ///&#10;
    ///• className (string) - className of the dropdown element
    ///&#10;
    ///• clearable (boolean; default true) - Whether or not the dropdown is "clearable", that is, whether or
    ///not a small "x" appears on the right of the dropdown that removes
    ///the selected value.
    ///&#10;
    ///• disabled (boolean; default false) - If true, this dropdown is disabled and the selection cannot be changed.
    ///&#10;
    ///• multi (boolean; default false) - If true, the user can select multiple values
    ///&#10;
    ///• placeholder (string) - The grey, default text shown when no option is selected
    ///&#10;
    ///• searchable (boolean; default true) - Whether to enable the searching feature or not
    ///&#10;
    ///• search_value (string) - The value typed in the DropDown for searching.
    ///&#10;
    ///• style (record) - Defines CSS styles which will override styles previously set.
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
        | Options of seq<DropdownOption>
        | Value of DropdownValue
        | OptionHeight of float
        | Clearable of bool
        | Disabled of bool
        | Multi of bool
        | Placeholder of string
        | Searchable of bool
        | SearchValue of bool
        | Style of DashComponentStyle
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions

        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | ClassName p       -> "className", box p
            | Options p         -> "options", box p
            | Value p           -> "value", p |> DropdownValue.convert
            | OptionHeight p    -> "optionHeight", box p
            | Clearable p       -> "clearable", box p
            | Disabled p        -> "disabled", box p
            | Multi p           -> "multi", box p
            | Placeholder p     -> "placeholder", box p
            | Searchable p      -> "searchable", box p
            | SearchValue p     -> "search_value", box p
            | Style p           -> "style", box p
            | LoadingState p    -> "loading_state", box p
            | Persistence p     -> "persistence", box p
            | PersistedProps p  -> "persisted_props", box p
            | PersistenceType p -> "persistence_type", PersistenceTypeOptions.convert p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///An array of options {label: [string|number], value: [string|number]},
        ///an optional disabled field can be used for each option
        ///</summary>
        static member options(p: seq<DropdownOption>) = Prop(Options p)
        ///<summary>
        ///The value of the input. If &#96;multi&#96; is false (the default)
        ///then value is just a string that corresponds to the values
        ///provided in the &#96;options&#96; property. If &#96;multi&#96; is true, then
        ///multiple values can be selected at once, and &#96;value&#96; is an
        ///array of items with values corresponding to those in the
        ///&#96;options&#96; prop.
        ///</summary>
        static member value(p: IConvertible) = Prop(Value(DropdownValue.SingleValue p))
        ///<summary>
        ///The value of the input. If &#96;multi&#96; is false (the default)
        ///then value is just a string that corresponds to the values
        ///provided in the &#96;options&#96; property. If &#96;multi&#96; is true, then
        ///multiple values can be selected at once, and &#96;value&#96; is an
        ///array of items with values corresponding to those in the
        ///&#96;options&#96; prop.
        ///</summary>
        static member value(p: IConvertible list) = Prop(Value(DropdownValue.MultipleValues p))

        ///<summary>
        ///height of each option. Can be increased when label lengths would wrap around
        ///</summary>
        static member optionHeight(p: float) = Prop(OptionHeight p)
        ///<summary>
        ///className of the dropdown element
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Whether or not the dropdown is "clearable", that is, whether or
        ///not a small "x" appears on the right of the dropdown that removes
        ///the selected value.
        ///</summary>
        static member clearable(p: bool) = Prop(Clearable p)
        ///<summary>
        ///If true, this dropdown is disabled and the selection cannot be changed.
        ///</summary>
        static member disabled(p: bool) = Prop(Disabled p)
        ///<summary>
        ///If true, the user can select multiple values
        ///</summary>
        static member multi(p: bool) = Prop(Multi p)
        ///<summary>
        ///The grey, default text shown when no option is selected
        ///</summary>
        static member placeholder(p: string) = Prop(Placeholder p)
        ///<summary>
        ///Whether to enable the searching feature or not
        ///</summary>
        static member searchable(p: bool) = Prop(Searchable p)
        ///<summary>
        ///The value typed in the DropDown for searching.
        ///</summary>
        static member searchValue(p: bool) = Prop(SearchValue p)
        ///<summary>
        ///Defines CSS styles which will override styles previously set.
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))

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
    ///Dropdown is an interactive dropdown element for selecting one or more
    ///items.
    ///The values and labels of the dropdown items are specified in the &#96;options&#96;
    ///property and the selected item(s) are specified with the &#96;value&#96; property.
    ///Use a dropdown when you have many options (more than 5) or when you are
    ///constrained for space. Otherwise, you can use RadioItems or a Checklist,
    ///which have the benefit of showing the users all of the items at once.
    ///</summary>
    type Dropdown() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?options: string,
                ?value: string,
                ?optionHeight: string,
                ?className: string,
                ?clearable: string,
                ?disabled: string,
                ?multi: string,
                ?placeholder: string,
                ?searchable: string,
                ?search_value: string,
                ?style: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            (fun (t: Dropdown) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "options" options
                DynObj.setValueOpt props "value" value
                DynObj.setValueOpt props "optionHeight" optionHeight
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "clearable" clearable
                DynObj.setValueOpt props "disabled" disabled
                DynObj.setValueOpt props "multi" multi
                DynObj.setValueOpt props "placeholder" placeholder
                DynObj.setValueOpt props "searchable" searchable
                DynObj.setValueOpt props "search_value" search_value
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValueOpt props "persistence" persistence
                DynObj.setValueOpt props "persisted_props" persisted_props
                DynObj.setValueOpt props "persistence_type" persistence_type
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Dropdown"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?options: string,
                ?value: string,
                ?optionHeight: string,
                ?className: string,
                ?clearable: string,
                ?disabled: string,
                ?multi: string,
                ?placeholder: string,
                ?searchable: string,
                ?search_value: string,
                ?style: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            Dropdown.applyMembers
                (id,
                 children,
                 ?options = options,
                 ?value = value,
                 ?optionHeight = optionHeight,
                 ?className = className,
                 ?clearable = clearable,
                 ?disabled = disabled,
                 ?multi = multi,
                 ?placeholder = placeholder,
                 ?searchable = searchable,
                 ?search_value = search_value,
                 ?style = style,
                 ?loading_state = loading_state,
                 ?persistence = persistence,
                 ?persisted_props = persisted_props,
                 ?persistence_type = persistence_type)
                (Dropdown())

    ///<summary>
    ///Dropdown is an interactive dropdown element for selecting one or more
    ///items.
    ///The values and labels of the dropdown items are specified in the &#96;options&#96;
    ///property and the selected item(s) are specified with the &#96;value&#96; property.
    ///Use a dropdown when you have many options (more than 5) or when you are
    ///constrained for space. Otherwise, you can use RadioItems or a Checklist,
    ///which have the benefit of showing the users all of the items at once.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• options (list with values of type: record with the fields: 'label: string | number (required)', 'value: string | number (required)', 'disabled: boolean (optional)', 'title: string (optional)') - An array of options {label: [string|number], value: [string|number]},
    ///an optional disabled field can be used for each option
    ///&#10;
    ///• value (string | number | list with values of type: string | number) - The value of the input. If &#96;multi&#96; is false (the default)
    ///then value is just a string that corresponds to the values
    ///provided in the &#96;options&#96; property. If &#96;multi&#96; is true, then
    ///multiple values can be selected at once, and &#96;value&#96; is an
    ///array of items with values corresponding to those in the
    ///&#96;options&#96; prop.
    ///&#10;
    ///• optionHeight (number; default 35) - height of each option. Can be increased when label lengths would wrap around
    ///&#10;
    ///• className (string) - className of the dropdown element
    ///&#10;
    ///• clearable (boolean; default true) - Whether or not the dropdown is "clearable", that is, whether or
    ///not a small "x" appears on the right of the dropdown that removes
    ///the selected value.
    ///&#10;
    ///• disabled (boolean; default false) - If true, this dropdown is disabled and the selection cannot be changed.
    ///&#10;
    ///• multi (boolean; default false) - If true, the user can select multiple values
    ///&#10;
    ///• placeholder (string) - The grey, default text shown when no option is selected
    ///&#10;
    ///• searchable (boolean; default true) - Whether to enable the searching feature or not
    ///&#10;
    ///• search_value (string) - The value typed in the DropDown for searching.
    ///&#10;
    ///• style (record) - Defines CSS styles which will override styles previously set.
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
    let dropdown (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Dropdown.init (id, children)

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
