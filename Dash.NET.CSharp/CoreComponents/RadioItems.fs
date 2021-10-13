namespace Dash.NET.CSharp.DCC

open System
open ComponentPropTypes
open Dash.NET.CSharp.ComponentStyle

///<summary>
///RadioItems is a component that encapsulates several radio item inputs.
///The values and labels of the RadioItems is specified in the &#96;options&#96;
///property and the seleced item is specified with the &#96;value&#96; property.
///Each radio item is rendered as an input with a surrounding label.
///</summary>
[<RequireQualifiedAccess>]
module RadioItems =

    // Original attr
    type internal OAttr = Dash.NET.DCC.RadioItems.Attr


    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.RadioItems.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.RadioItems.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///An array of options
        ///</summary>
        static member options([<ParamArray>] p: array<RadioItemsOption>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.options (p |> Array.map RadioItemsOption.Convert) |> Attr.Wrap
        ///<summary>
        ///The currently selected value
        ///</summary>
        static member value(p: IConvertible) = 
            guardAgainstNull "p" p
            OAttr.value p |> Attr.Wrap
        ///<summary>
        ///The style of the container (div)
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.style (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The class of the container (div)
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            OAttr.className p |> Attr.Wrap
        ///<summary>
        ///The style of the &lt;input&gt; radio element
        ///</summary>
        static member inputStyle([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.inputStyle (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The class of the &lt;input&gt; radio element
        ///</summary>
        static member inputClassName(p: string) = 
            guardAgainstNull "p" p
            OAttr.inputClassName p |> Attr.Wrap
        ///<summary>
        ///The style of the &lt;label&gt; that wraps the radio input
        /// and the option's label
        ///</summary>
        static member labelStyle([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.labelStyle (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The class of the &lt;label&gt; that wraps the radio input
        /// and the option's label
        ///</summary>
        static member labelClassName(p: string) = 
            guardAgainstNull "p" p
            OAttr.labelClassName p |> Attr.Wrap

        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = 
            guardAgainstNull "p" p
            OAttr.loadingState (p |> LoadingState.Convert) |> Attr.Wrap
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
            OAttr.persistence p |> Attr.Wrap
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps([<ParamArray>] p: string []) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.persistedProps p |> Attr.Wrap
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType(p: PersistenceTypeOptions) = 
            guardAgainstNull "p" p
            OAttr.persistenceType (p |> PersistenceTypeOptions.Unwrap) |> Attr.Wrap

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
    let radioItems (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.RadioItems.radioItems id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Html.DashComponent.Wrap