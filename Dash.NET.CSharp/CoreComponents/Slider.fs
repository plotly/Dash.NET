﻿namespace Dash.NET.CSharp.DCC

open System
open Dash.NET.CSharp.ComponentStyle
open System.Collections.Generic

///<summary>
///A slider component with a single handle.
///</summary>
[<RequireQualifiedAccess>]
module Slider =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Slider.Attr


    ///<summary>
    ///value equal to: 'mouseup', 'drag'
    ///</summary>
    type UpdateOn = private WrappedUpdateOn of Dash.NET.DCC.Slider.UpdateOn with
        static member internal Wrap (attr : Dash.NET.DCC.Slider.UpdateOn) = WrappedUpdateOn attr
        static member internal Unwrap (attr : UpdateOn) = match attr with | WrappedUpdateOn attr -> attr

        static member Mouseup = Dash.NET.DCC.Slider.UpdateOn.Mouseup |> UpdateOn.Wrap
        static member Drag = Dash.NET.DCC.Slider.UpdateOn.Drag |> UpdateOn.Wrap

    ///<summary>
    ///value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight'
    ///<para>&#160;</para>
    ///Determines the placement of tooltips
    ///See https://github.com/react-component/tooltip#api
    ///top/bottom{*} sets the _origin_ of the tooltip, so e.g. &#96;topLeft&#96;
    ///will in reality appear to be on the top right of the handle
    ///</summary>
    type TooltipPlacement = private WrappedTooltipPlacement of Dash.NET.DCC.Slider.TooltipPlacement with
        static member internal Wrap (attr : Dash.NET.DCC.Slider.TooltipPlacement) = WrappedTooltipPlacement attr
        static member internal Unwrap (attr : TooltipPlacement) = match attr with | WrappedTooltipPlacement attr -> attr

        static member Left = Dash.NET.DCC.Slider.TooltipPlacement.Left |> TooltipPlacement.Wrap
        static member Right = Dash.NET.DCC.Slider.TooltipPlacement.Right |> TooltipPlacement.Wrap
        static member Top = Dash.NET.DCC.Slider.TooltipPlacement.Top |> TooltipPlacement.Wrap
        static member Bottom = Dash.NET.DCC.Slider.TooltipPlacement.Bottom |> TooltipPlacement.Wrap
        static member TopLeft = Dash.NET.DCC.Slider.TooltipPlacement.TopLeft |> TooltipPlacement.Wrap
        static member TopRight = Dash.NET.DCC.Slider.TooltipPlacement.TopRight |> TooltipPlacement.Wrap
        static member BottomLeft = Dash.NET.DCC.Slider.TooltipPlacement.BottomLeft |> TooltipPlacement.Wrap
        static member BottomRight = Dash.NET.DCC.Slider.TooltipPlacement.BottomRight |> TooltipPlacement.Wrap

    ///<summary>
    ///record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)'
    ///</summary>
    type TooltipOptions =
        { AlwaysVisible: bool
          Placement: TooltipPlacement }
        with
        static member internal Convert (v : TooltipOptions) : Dash.NET.DCC.Slider.TooltipOptions =
            Dash.NET.DCC.Slider.TooltipOptions.init (v.AlwaysVisible, v.Placement |> TooltipPlacement.Unwrap)


    ///<summary>
    ///record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type StyledMarkValue =
        { Label: string
          Style: DashComponentStyle }
        with
        static member internal Convert (v : StyledMarkValue) : Dash.NET.DCC.Slider.StyledMarkValue = 
            Dash.NET.DCC.Slider.StyledMarkValue.init (v.Label, v.Style |> DashComponentStyle.Unwrap)

    ///<summary>
    ///string | record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type Mark = private WrappedMark of Dash.NET.DCC.Slider.Mark with
        static member internal Wrap (v : Dash.NET.DCC.Slider.Mark) = WrappedMark v
        static member internal Unwrap (attr : Mark) = match attr with | WrappedMark attr -> attr

        static member Value (value : string) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Slider.Mark.Value value |> Mark.Wrap
        static member StyledValue (value : StyledMarkValue) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Slider.Mark.StyledValue (value |> StyledMarkValue.Convert) |> Mark.Wrap

    
    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Slider.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Slider.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///Marks on the slider.
        ///The key determines the position (a number),
        ///and the value determines what will show.
        ///If you want to set the style of a specific mark point,
        ///the value should be an object which
        ///contains style and label properties.
        ///</summary>
        static member marks(p: Dictionary<float, Mark>) = 
            guardAgainstNull "p" p
            OAttr.marks (p |> Map.fromDictionary |> Map.map (fun k v -> Mark.Unwrap v)) |> Attr.Wrap
        ///<summary>
        ///The value of the input
        ///</summary>
        static member value(p: float) = 
            guardAgainstNull "p" p
            OAttr.value p |> Attr.Wrap
        ///<summary>
        ///The value of the input during a drag
        ///</summary>
        static member dragValue(p: float) = 
            guardAgainstNull "p" p
            OAttr.dragValue p |> Attr.Wrap
        ///<summary>
        ///Additional CSS class for the root DOM node
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            OAttr.className p |> Attr.Wrap
        ///<summary>
        ///If true, the handles can't be moved.
        ///</summary>
        static member disabled(p: bool) = 
            guardAgainstNull "p" p
            OAttr.disabled p |> Attr.Wrap
        ///<summary>
        ///When the step value is greater than 1,
        ///you can set the dots to true if you want to
        ///render the slider with dots.
        ///</summary>
        static member dots(p: bool) = 
            guardAgainstNull "p" p
            OAttr.dots p |> Attr.Wrap
        ///<summary>
        ///If the value is true, it means a continuous
        ///value is included. Otherwise, it is an independent value.
        ///</summary>
        static member included(p: bool) = 
            guardAgainstNull "p" p
            OAttr.included p |> Attr.Wrap
        ///<summary>
        ///Minimum allowed value of the slider
        ///</summary>
        static member min(p: float) = 
            guardAgainstNull "p" p
            OAttr.min p |> Attr.Wrap
        ///<summary>
        ///Maximum allowed value of the slider
        ///</summary>
        static member max(p: float) = 
            guardAgainstNull "p" p
            OAttr.max p |> Attr.Wrap
        ///<summary>
        ///Configuration for tooltips describing the current slider value
        ///</summary>
        static member tooltip(p: TooltipOptions) = 
            guardAgainstNull "p" p
            OAttr.tooltip (p |> TooltipOptions.Convert) |> Attr.Wrap
        ///<summary>
        ///Value by which increments or decrements are made
        ///</summary>
        static member step(p: float) = 
            guardAgainstNull "p" p
            OAttr.step p |> Attr.Wrap
        ///<summary>
        ///If true, the slider will be vertical
        ///</summary>
        static member vertical(p: bool) = 
            guardAgainstNull "p" p
            OAttr.vertical p |> Attr.Wrap
        ///<summary>
        ///The height, in px, of the slider if it is vertical.
        ///</summary>
        static member verticalHeight(p: float) = 
            guardAgainstNull "p" p
            OAttr.verticalHeight p |> Attr.Wrap
        ///<summary>
        ///Determines when the component should update its &#96;value&#96;
        ///property. If &#96;mouseup&#96; (the default) then the slider
        ///will only trigger its value when the user has finished
        ///dragging the slider. If &#96;drag&#96;, then the slider will
        ///update its value continuously as it is being dragged.
        ///If you want different actions during and after drag,
        ///leave &#96;updatemode&#96; as &#96;mouseup&#96; and use &#96;drag_value&#96;
        ///for the continuously updating value.
        ///</summary>
        static member updateMode(p: UpdateOn) = 
            guardAgainstNull "p" p
            OAttr.updateMode (p |> UpdateOn.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = 
            guardAgainstNull "p" p
            OAttr.loadingState (p |> LoadingState.Unwrap) |> Attr.Wrap
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
    ///A slider component with a single handle.
    ///<para>&#160;</para> 
    ///Properties:
    ///<para>&#160;</para> 
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///<para>&#160;</para>
    ///• marks (dict with values of type: string | record with the fields: 'label: string (optional)', 'style: record (optional)') - Marks on the slider.
    ///The key determines the position (a number),
    ///and the value determines what will show.
    ///If you want to set the style of a specific mark point,
    ///the value should be an object which
    ///contains style and label properties.
    ///<para>&#160;</para>
    ///• value (number) - The value of the input
    ///<para>&#160;</para>
    ///• drag_value (number) - The value of the input during a drag
    ///<para>&#160;</para>
    ///• className (string) - Additional CSS class for the root DOM node
    ///<para>&#160;</para>
    ///• disabled (boolean) - If true, the handles can't be moved.
    ///<para>&#160;</para>
    ///• dots (boolean) - When the step value is greater than 1,
    ///you can set the dots to true if you want to
    ///render the slider with dots.
    ///<para>&#160;</para>
    ///• included (boolean) - If the value is true, it means a continuous
    ///value is included. Otherwise, it is an independent value.
    ///<para>&#160;</para>
    ///• min (number) - Minimum allowed value of the slider
    ///<para>&#160;</para>
    ///• max (number) - Maximum allowed value of the slider
    ///<para>&#160;</para>
    ///• tooltip (record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)') - Configuration for tooltips describing the current slider value
    ///<para>&#160;</para>
    ///• step (number) - Value by which increments or decrements are made
    ///<para>&#160;</para>
    ///• vertical (boolean) - If true, the slider will be vertical
    ///<para>&#160;</para>
    ///• verticalHeight (number; default 400) - The height, in px, of the slider if it is vertical.
    ///<para>&#160;</para>
    ///• updatemode (value equal to: 'mouseup', 'drag'; default mouseup) - Determines when the component should update its &#96;value&#96;
    ///property. If &#96;mouseup&#96; (the default) then the slider
    ///will only trigger its value when the user has finished
    ///dragging the slider. If &#96;drag&#96;, then the slider will
    ///update its value continuously as it is being dragged.
    ///If you want different actions during and after drag,
    ///leave &#96;updatemode&#96; as &#96;mouseup&#96; and use &#96;drag_value&#96;
    ///for the continuously updating value.
    ///<para>&#160;</para>
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///<para>&#160;</para>
    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
    ///hasn't changed from its previous value, a &#96;value&#96; that the user has
    ///changed while using the app will keep that change, as long as
    ///the new &#96;value&#96; also matches what was given originally.
    ///Used in conjunction with &#96;persistence_type&#96;.
    ///<para>&#160;</para>
    ///• persisted_props (list with values of type: value equal to: 'value'; default ['value']) - Properties whose user interactions will persist after refreshing the
    ///component or the page. Since only &#96;value&#96; is allowed this prop can
    ///normally be ignored.
    ///<para>&#160;</para>
    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///</summary>
    let slider (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Slider.slider id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap