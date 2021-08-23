namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open Newtonsoft.Json

///<summary>
///A slider component with a single handle.
///</summary>
[<RequireQualifiedAccess>]
module Slider =
    ///<summary>
    ///value equal to: 'local', 'session', 'memory'
    ///</summary>
    type PersistenceType =
        | Local
        | Session
        | Memory
        override this.ToString() =
            match this with
            | Local -> "local"
            | Session -> "session"
            | Memory -> "memory"

    ///<summary>
    ///value equal to: 'value'
    ///</summary>
    type PersistedPropsType =
        | Value
        override this.ToString() =
            match this with
            | Value -> "value"

    ///<summary>
    ///list with values of type: value equal to: 'value'
    ///</summary>
    type PersistedProps =
        | PersistedProps of list<PersistedPropsType>
        override this.ToString() =
            match this with
            | PersistedProps (v) ->
                JsonConvert.SerializeObject(List.map (fun (i: PersistedPropsType) -> i.ToString()) v)

    ///<summary>
    ///boolean | string | number
    ///</summary>
    type Persistence =
        | PersistenceCase0 of bool
        | PersistenceCase1 of string
        | PersistenceCase2 of IConvertible
        override this.ToString() =
            match this with
            | PersistenceCase0 (v) -> v.ToString()
            | PersistenceCase1 (v) -> v.ToString()
            | PersistenceCase2 (v) -> v.ToString()

    ///<summary>
    ///record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)'
    ///</summary>
    type LoadingState =
        { IsLoading: Option<bool>
          PropName: Option<string>
          ComponentName: Option<string> }
        override this.ToString() =
            JsonConvert.SerializeObject
                {| is_loading = Some(this.IsLoading.ToString())
                   prop_name = Some(this.PropName.ToString())
                   component_name = Some(this.ComponentName.ToString()) |}

    ///<summary>
    ///value equal to: 'mouseup', 'drag'
    ///</summary>
    type Updatemode =
        | Mouseup
        | Drag
        override this.ToString() =
            match this with
            | Mouseup -> "mouseup"
            | Drag -> "drag"

    ///<summary>
    ///value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight'
    ///&#10;
    ///Determines the placement of tooltips
    ///See https://github.com/react-component/tooltip#api
    ///top/bottom{*} sets the _origin_ of the tooltip, so e.g. &#96;topLeft&#96;
    ///will in reality appear to be on the top right of the handle
    ///</summary>
    type TooltipPlacementType =
        | Left
        | Right
        | Top
        | Bottom
        | TopLeft
        | TopRight
        | BottomLeft
        | BottomRight
        override this.ToString() =
            match this with
            | Left -> "left"
            | Right -> "right"
            | Top -> "top"
            | Bottom -> "bottom"
            | TopLeft -> "topLeft"
            | TopRight -> "topRight"
            | BottomLeft -> "bottomLeft"
            | BottomRight -> "bottomRight"

    ///<summary>
    ///record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)'
    ///</summary>
    type Tooltip =
        { AlwaysVisible: Option<bool>
          Placement: Option<TooltipPlacementType> }
        override this.ToString() =
            JsonConvert.SerializeObject
                {| always_visible = Some(this.AlwaysVisible.ToString())
                   placement = Some(this.Placement.ToString()) |}

    ///<summary>
    ///record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type MarksTypeCase1Type =
        { Label: Option<string>
          Style: Option<obj> }
        override this.ToString() =
            JsonConvert.SerializeObject
                {| label = Some(this.Label.ToString())
                   style = Some(this.Style.ToString()) |}

    ///<summary>
    ///string | record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type MarksType =
        | MarksTypeCase0 of string
        | MarksTypeCase1 of MarksTypeCase1Type
        override this.ToString() =
            match this with
            | MarksTypeCase0 (v) -> v.ToString()
            | MarksTypeCase1 (v) -> v.ToString()

    ///<summary>
    ///string | record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type Marks =
        | Marks of MarksType
        override this.ToString() =
            match this with
            | Marks (v) -> v.ToString()

    ///<summary>
    ///• marks (string | record with the fields: 'label: string (optional)', 'style: record (optional)') - Marks on the slider.
    ///The key determines the position (a number),
    ///and the value determines what will show.
    ///If you want to set the style of a specific mark point,
    ///the value should be an object which
    ///contains style and label properties.
    ///&#10;
    ///• value (number) - The value of the input
    ///&#10;
    ///• drag_value (number) - The value of the input during a drag
    ///&#10;
    ///• className (string) - Additional CSS class for the root DOM node
    ///&#10;
    ///• disabled (boolean) - If true, the handles can't be moved.
    ///&#10;
    ///• dots (boolean) - When the step value is greater than 1,
    ///you can set the dots to true if you want to
    ///render the slider with dots.
    ///&#10;
    ///• included (boolean) - If the value is true, it means a continuous
    ///value is included. Otherwise, it is an independent value.
    ///&#10;
    ///• min (number) - Minimum allowed value of the slider
    ///&#10;
    ///• max (number) - Maximum allowed value of the slider
    ///&#10;
    ///• tooltip (record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)') - Configuration for tooltips describing the current slider value
    ///&#10;
    ///• step (number) - Value by which increments or decrements are made
    ///&#10;
    ///• vertical (boolean) - If true, the slider will be vertical
    ///&#10;
    ///• verticalHeight (number; default 400) - The height, in px, of the slider if it is vertical.
    ///&#10;
    ///• updatemode (value equal to: 'mouseup', 'drag'; default mouseup) - Determines when the component should update its &#96;value&#96;
    ///property. If &#96;mouseup&#96; (the default) then the slider
    ///will only trigger its value when the user has finished
    ///dragging the slider. If &#96;drag&#96;, then the slider will
    ///update its value continuously as it is being dragged.
    ///If you want different actions during and after drag,
    ///leave &#96;updatemode&#96; as &#96;mouseup&#96; and use &#96;drag_value&#96;
    ///for the continuously updating value.
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
        | Marks of Marks
        | Value of IConvertible
        | DragValue of IConvertible
        | ClassName of string
        | Disabled of bool
        | Dots of bool
        | Included of bool
        | Min of IConvertible
        | Max of IConvertible
        | Tooltip of Tooltip
        | Step of IConvertible
        | Vertical of bool
        | VerticalHeight of IConvertible
        | Updatemode of Updatemode
        | LoadingState of LoadingState
        | Persistence of Persistence
        | PersistedProps of PersistedProps
        | PersistenceType of PersistenceType
        static member toDynamicMemberDef(prop: Prop) =
            match prop with
            | Marks (p) -> "marks", box (p.ToString())
            | Value (p) -> "value", box p
            | DragValue (p) -> "drag_value", box p
            | ClassName (p) -> "className", box p
            | Disabled (p) -> "disabled", box p
            | Dots (p) -> "dots", box p
            | Included (p) -> "included", box p
            | Min (p) -> "min", box p
            | Max (p) -> "max", box p
            | Tooltip (p) -> "tooltip", box (p.ToString())
            | Step (p) -> "step", box p
            | Vertical (p) -> "vertical", box p
            | VerticalHeight (p) -> "verticalHeight", box p
            | Updatemode (p) -> "updatemode", box (p.ToString())
            | LoadingState (p) -> "loading_state", box (p.ToString())
            | Persistence (p) -> "persistence", box (p.ToString())
            | PersistedProps (p) -> "persisted_props", box (p.ToString())
            | PersistenceType (p) -> "persistence_type", box (p.ToString())

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///Marks on the slider.
        ///The key determines the position (a number),
        ///and the value determines what will show.
        ///If you want to set the style of a specific mark point,
        ///the value should be an object which
        ///contains style and label properties.
        ///</summary>
        static member marks(p: Marks) = Prop(Marks p)
        ///<summary>
        ///The value of the input
        ///</summary>
        static member value(p: IConvertible) = Prop(Value p)
        ///<summary>
        ///The value of the input during a drag
        ///</summary>
        static member drag_value(p: IConvertible) = Prop(DragValue p)
        ///<summary>
        ///Additional CSS class for the root DOM node
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///If true, the handles can't be moved.
        ///</summary>
        static member disabled(p: bool) = Prop(Disabled p)
        ///<summary>
        ///When the step value is greater than 1,
        ///you can set the dots to true if you want to
        ///render the slider with dots.
        ///</summary>
        static member dots(p: bool) = Prop(Dots p)
        ///<summary>
        ///If the value is true, it means a continuous
        ///value is included. Otherwise, it is an independent value.
        ///</summary>
        static member included(p: bool) = Prop(Included p)
        ///<summary>
        ///Minimum allowed value of the slider
        ///</summary>
        static member min(p: IConvertible) = Prop(Min p)
        ///<summary>
        ///Maximum allowed value of the slider
        ///</summary>
        static member max(p: IConvertible) = Prop(Max p)
        ///<summary>
        ///Configuration for tooltips describing the current slider value
        ///</summary>
        static member tooltip(p: Tooltip) = Prop(Tooltip p)
        ///<summary>
        ///Value by which increments or decrements are made
        ///</summary>
        static member step(p: IConvertible) = Prop(Step p)
        ///<summary>
        ///If true, the slider will be vertical
        ///</summary>
        static member vertical(p: bool) = Prop(Vertical p)
        ///<summary>
        ///The height, in px, of the slider if it is vertical.
        ///</summary>
        static member verticalHeight(p: IConvertible) = Prop(VerticalHeight p)
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
        static member updatemode(p: Updatemode) = Prop(Updatemode p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loading_state(p: LoadingState) = Prop(LoadingState p)
        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: Persistence) = Prop(Persistence p)
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persisted_props(p: PersistedProps) = Prop(PersistedProps p)
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistence_type(p: PersistenceType) = Prop(PersistenceType p)
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
    ///A slider component with a single handle.
    ///</summary>
    type Slider() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?marks: string,
                ?value: string,
                ?drag_value: string,
                ?className: string,
                ?disabled: string,
                ?dots: string,
                ?included: string,
                ?min: string,
                ?max: string,
                ?tooltip: string,
                ?step: string,
                ?vertical: string,
                ?verticalHeight: string,
                ?updatemode: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            (fun (t: Slider) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "marks" marks
                DynObj.setValueOpt props "value" value
                DynObj.setValueOpt props "drag_value" drag_value
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "disabled" disabled
                DynObj.setValueOpt props "dots" dots
                DynObj.setValueOpt props "included" included
                DynObj.setValueOpt props "min" min
                DynObj.setValueOpt props "max" max
                DynObj.setValueOpt props "tooltip" tooltip
                DynObj.setValueOpt props "step" step
                DynObj.setValueOpt props "vertical" vertical
                DynObj.setValueOpt props "verticalHeight" verticalHeight
                DynObj.setValueOpt props "updatemode" updatemode
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValueOpt props "persistence" persistence
                DynObj.setValueOpt props "persisted_props" persisted_props
                DynObj.setValueOpt props "persistence_type" persistence_type
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Slider"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?marks: string,
                ?value: string,
                ?drag_value: string,
                ?className: string,
                ?disabled: string,
                ?dots: string,
                ?included: string,
                ?min: string,
                ?max: string,
                ?tooltip: string,
                ?step: string,
                ?vertical: string,
                ?verticalHeight: string,
                ?updatemode: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            Slider.applyMembers
                (id,
                 children,
                 ?marks = marks,
                 ?value = value,
                 ?drag_value = drag_value,
                 ?className = className,
                 ?disabled = disabled,
                 ?dots = dots,
                 ?included = included,
                 ?min = min,
                 ?max = max,
                 ?tooltip = tooltip,
                 ?step = step,
                 ?vertical = vertical,
                 ?verticalHeight = verticalHeight,
                 ?updatemode = updatemode,
                 ?loading_state = loading_state,
                 ?persistence = persistence,
                 ?persisted_props = persisted_props,
                 ?persistence_type = persistence_type)
                (Slider())

    ///<summary>
    ///A slider component with a single handle.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• marks (string | record with the fields: 'label: string (optional)', 'style: record (optional)') - Marks on the slider.
    ///The key determines the position (a number),
    ///and the value determines what will show.
    ///If you want to set the style of a specific mark point,
    ///the value should be an object which
    ///contains style and label properties.
    ///&#10;
    ///• value (number) - The value of the input
    ///&#10;
    ///• drag_value (number) - The value of the input during a drag
    ///&#10;
    ///• className (string) - Additional CSS class for the root DOM node
    ///&#10;
    ///• disabled (boolean) - If true, the handles can't be moved.
    ///&#10;
    ///• dots (boolean) - When the step value is greater than 1,
    ///you can set the dots to true if you want to
    ///render the slider with dots.
    ///&#10;
    ///• included (boolean) - If the value is true, it means a continuous
    ///value is included. Otherwise, it is an independent value.
    ///&#10;
    ///• min (number) - Minimum allowed value of the slider
    ///&#10;
    ///• max (number) - Maximum allowed value of the slider
    ///&#10;
    ///• tooltip (record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)') - Configuration for tooltips describing the current slider value
    ///&#10;
    ///• step (number) - Value by which increments or decrements are made
    ///&#10;
    ///• vertical (boolean) - If true, the slider will be vertical
    ///&#10;
    ///• verticalHeight (number; default 400) - The height, in px, of the slider if it is vertical.
    ///&#10;
    ///• updatemode (value equal to: 'mouseup', 'drag'; default mouseup) - Determines when the component should update its &#96;value&#96;
    ///property. If &#96;mouseup&#96; (the default) then the slider
    ///will only trigger its value when the user has finished
    ///dragging the slider. If &#96;drag&#96;, then the slider will
    ///update its value continuously as it is being dragged.
    ///If you want different actions during and after drag,
    ///leave &#96;updatemode&#96; as &#96;mouseup&#96; and use &#96;drag_value&#96;
    ///for the continuously updating value.
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
    let slider (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Slider.init (id, children)

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


