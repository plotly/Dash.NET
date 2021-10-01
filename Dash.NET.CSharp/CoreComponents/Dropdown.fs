namespace Dash.NET.CSharp.DCC

open System
open ComponentPropTypes
open Dash.NET.CSharp.ComponentStyle

// Original attr
type internal OAttr = Dash.NET.DCC.Dropdown.Attr

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
    type DropdownValue = private WrappedDropdownValue of Dash.NET.DCC.Dropdown.DropdownValue with
        static member internal Wrap (attr : Dash.NET.DCC.Dropdown.DropdownValue) = WrappedDropdownValue attr
        static member internal Unwrap (attr : DropdownValue) = match attr with | WrappedDropdownValue attr -> attr

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Dropdown.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Dropdown.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///An array of options {label: [string|number], value: [string|number]},
        ///an optional disabled field can be used for each option
        ///</summary>
        static member options(p: seq<DropdownOption>) = OAttr.options (p |> Seq.map DropdownOption.Convert) |> Attr.Wrap
        ///<summary>
        ///The value of the input. If &#96;multi&#96; is false (the default)
        ///then value is just a string that corresponds to the values
        ///provided in the &#96;options&#96; property. If &#96;multi&#96; is true, then
        ///multiple values can be selected at once, and &#96;value&#96; is an
        ///array of items with values corresponding to those in the
        ///&#96;options&#96; prop.
        ///</summary>
        static member value(p: IConvertible) = OAttr.value p |> Attr.Wrap
        ///<summary>
        ///The value of the input. If &#96;multi&#96; is false (the default)
        ///then value is just a string that corresponds to the values
        ///provided in the &#96;options&#96; property. If &#96;multi&#96; is true, then
        ///multiple values can be selected at once, and &#96;value&#96; is an
        ///array of items with values corresponding to those in the
        ///&#96;options&#96; prop.
        ///</summary>
        static member value([<ParamArray>] p: IConvertible array) = OAttr.value (p |> List.ofArray) |> Attr.Wrap

        ///<summary>
        ///height of each option. Can be increased when label lengths would wrap around
        ///</summary>
        static member optionHeight(p: float) = OAttr.optionHeight p |> Attr.Wrap
        ///<summary>
        ///className of the dropdown element
        ///</summary>
        static member className(p: string) = OAttr.className p |> Attr.Wrap
        ///<summary>
        ///Whether or not the dropdown is "clearable", that is, whether or
        ///not a small "x" appears on the right of the dropdown that removes
        ///the selected value.
        ///</summary>
        static member clearable(p: bool) = OAttr.clearable p |> Attr.Wrap
        ///<summary>
        ///If true, this dropdown is disabled and the selection cannot be changed.
        ///</summary>
        static member disabled(p: bool) = OAttr.disabled p |> Attr.Wrap
        ///<summary>
        ///If true, the user can select multiple values
        ///</summary>
        static member multi(p: bool) = OAttr.multi p |> Attr.Wrap
        ///<summary>
        ///The grey, default text shown when no option is selected
        ///</summary>
        static member placeholder(p: string) = OAttr.placeholder p |> Attr.Wrap
        ///<summary>
        ///Whether to enable the searching feature or not
        ///</summary>
        static member searchable(p: bool) = OAttr.searchable p |> Attr.Wrap
        ///<summary>
        ///The value typed in the DropDown for searching.
        ///</summary>
        static member searchValue(p: string) = OAttr.searchValue p |> Attr.Wrap
        ///<summary>
        ///Defines CSS styles which will override styles previously set.
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = OAttr.style (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap

        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = OAttr.loadingState (p |> LoadingState.Convert) |> Attr.Wrap
        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: IConvertible) = OAttr.persistence p |> Attr.Wrap
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps([<ParamArray>] p: string array) = OAttr.persistedProps p |> Attr.Wrap
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType(p: PersistenceTypeOptions) = OAttr.persistenceType (p |> PersistenceTypeOptions.Unwrap) |> Attr.Wrap

        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Dash.NET.CSharp.Html.DashComponent) = OAttr.children (value |> Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children([<ParamArray>] value: array<Dash.NET.CSharp.Html.DashComponent>) = OAttr.children (value |> Array.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<Dash.NET.CSharp.Html.DashComponent>) = OAttr.children (value |> Seq.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap

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
    let markdown (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Dropdown.dropdown id (attrs |> List.ofArray |> List.map Attr.Unwrap)