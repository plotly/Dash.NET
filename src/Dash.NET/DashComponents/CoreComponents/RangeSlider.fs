namespace Dash.NET.DCC

open System
open DynamicObj
open Dash.NET
open Dash.NET.Common

///<summary>
///A double slider with two handles.
///Used for specifying a range of numerical values.
///</summary>
[<RequireQualifiedAccess>]
module RangeSlider =
    ///<summary>
    ///value equal to: 'mouseup', 'drag'
    ///</summary>
    type UpdateOn =
        | Mouseup
        | Drag
        static member convert =
            function
            | Mouseup -> "mouseup"
            | Drag -> "drag"
            >> box

    ///<summary>
    ///value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight'
    ///&#10;
    ///Determines the placement of tooltips
    ///See https://github.com/react-component/tooltip#api
    ///top/bottom{*} sets the _origin_ of the tooltip, so e.g. &#96;topLeft&#96;
    ///will in reality appear to be on the top right of the handle
    ///</summary>
    type TooltipPlacement =
        | Left
        | Right
        | Top
        | Bottom
        | TopLeft
        | TopRight
        | BottomLeft
        | BottomRight
        static member convert =
            function
            | Left          -> "left"
            | Right         -> "right"
            | Top           -> "top"
            | Bottom        -> "bottom"
            | TopLeft       -> "topLeft"
            | TopRight      -> "topRight"
            | BottomLeft    -> "bottomLeft"
            | BottomRight   -> "bottomRight"
            >> box

    ///<summary>
    ///record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)'
    ///</summary>
    type TooltipOptions () =
        inherit DynamicObj()
        static member init
            (
                ?alwaysVisible: bool,
                ?placement: TooltipPlacement
            ) =
                let t = TooltipOptions()

                alwaysVisible |> DynObj.setValueOpt t "always_visible"
                placement |> DynObj.setDUValueOpt t TooltipPlacement.convert "placement"

                t

    ///<summary>
    ///boolean | number
    ///</summary>
    type PushableType =
        | Bool of bool
        | Number of int
        static member convert = function
            | Bool v -> box v
            | Number v -> box v

    ///<summary>
    ///record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type StyledMarkValue () =
        inherit DynamicObj()
        static member init
            (
                ?label: string,
                ?style: DashComponentStyle
            ) =
                let t = StyledMarkValue()

                label |> DynObj.setValueOpt t "label"
                style |> DynObj.setValueOpt t "style"

                t

    ///<summary>
    ///string | record with the fields: 'label: string (optional)', 'style: record (optional)'
    ///</summary>
    type Mark =
        | Value of string
        | StyledValue of StyledMarkValue
        static member convert = function
            | Value v -> box v
            | StyledValue v -> box v

    ///<summary>
    ///• marks (dict with values of type: string | record with the fields: 'label: string (optional)', 'style: record (optional)') - Marks on the slider.
    ///The key determines the position (a number),
    ///and the value determines what will show.
    ///If you want to set the stylMarksTypec mark point,
    ///the value should be an object which
    ///contains style and label properties.
    ///&#10;
    ///• value (list with values of type: number) - The value of the iMarksType#10;
    ///• drag_value (list with values of type: number) - The value of the input during a drag
    ///&#10;
    ///• allowCross (boolean) - allowCross could be set as true to allow those handles to cross.
    ///&#10;
    ///• className (string) - Additional CSS class for the root DOM node
    ///&#10;
    ///• count (number) - Determine how many ranges to render, and multiple handles
    ///will be rendered (number + 1).
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
    ///• pushable (boolean | number) - pushable could be set as true to allow pushing of
    ///surrounding handles when moving an handle.
    ///When set to a number, the number will be the
    ///minimum ensured distance between handles.
    ///&#10;
    ///• tooltip (record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)') - Configuration for tooltips describing the current slider values
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
    ///Note that for the latter case, the &#96;drag_value&#96;
    ///property could be used instead.
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
        | Marks of Map<float, Mark>
        | Value of float list
        | DragValue of float list
        | AllowCross of bool
        | ClassName of string
        | Count of int
        | Disabled of bool
        | Dots of bool
        | Included of bool
        | Min of float
        | Max of float
        | Pushable of PushableType
        | Tooltip of TooltipOptions
        | Step of float
        | Vertical of bool
        | VerticalHeight of float
        | Updatemode of UpdateOn
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions

        static member convert = function
            | Marks             p -> p |> Map.map (fun _ -> Mark.convert) |> box
            | Value             p -> box p
            | DragValue         p -> box p
            | AllowCross        p -> box p
            | ClassName         p -> box p
            | Count             p -> box p
            | Disabled          p -> box p
            | Dots              p -> box p
            | Included          p -> box p
            | Min               p -> box p
            | Max               p -> box p
            | Pushable          p -> PushableType.convert p
            | Tooltip           p -> box p
            | Step              p -> box p
            | Vertical          p -> box p
            | VerticalHeight    p -> box p
            | Updatemode        p -> UpdateOn.convert p
            | LoadingState      p -> box p
            | Persistence       p -> box p
            | PersistedProps    p -> box p
            | PersistenceType   p -> PersistenceTypeOptions.convert p

        static member toPropName = function
            | Marks             _ -> "marks"
            | Value             _ -> "value"
            | DragValue         _ -> "drag_value"
            | AllowCross        _ -> "allowCross"
            | ClassName         _ -> "className"
            | Count             _ -> "count"
            | Disabled          _ -> "disabled"
            | Dots              _ -> "dots"
            | Included          _ -> "included"
            | Min               _ -> "min"
            | Max               _ -> "max"
            | Pushable          _ -> "pushable"
            | Tooltip           _ -> "tooltip"
            | Step              _ -> "step"
            | Vertical          _ -> "vertical"
            | VerticalHeight    _ -> "verticalHeight"
            | Updatemode        _ -> "updatemode"
            | LoadingState      _ -> "loading_state"
            | Persistence       _ -> "persistence"
            | PersistedProps    _ -> "persisted_props"
            | PersistenceType   _ -> "persistence_type"

        static member toDynamicMemberDef(prop: Prop) =
            Prop.toPropName prop
            , Prop.convert prop

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of DashComponent list
        ///<summary>
        ///Marks on the slider.
        ///The key determines the position (a number),
        ///and the value determines what will show.
        ///If you want to set the style of a specific mark point,
        ///the value should be an object which
        ///contains style and label properties.
        ///</summary>
        static member marks p = Prop(Marks p)
        ///<summary>
        ///The value of the input
        ///</summary>
        static member value p = Prop(Value p)
        ///<summary>
        ///The value of the input during a drag
        ///</summary>
        static member dragValue p = Prop(DragValue p)
        ///<summary>
        ///allowCross could be set as true to allow those handles to cross.
        ///</summary>
        static member allowCross p = Prop(AllowCross p)
        ///<summary>
        ///Additional CSS class for the root DOM node
        ///</summary>
        static member className p = Prop(ClassName p)
        ///<summary>
        ///Determine how many ranges to render, and multiple handles
        ///will be rendered (number + 1).
        ///</summary>
        static member count p = Prop(Count p)
        ///<summary>
        ///If true, the handles can't be moved.
        ///</summary>
        static member disabled p = Prop(Disabled p)
        ///<summary>
        ///When the step value is greater than 1,
        ///you can set the dots to true if you want to
        ///render the slider with dots.
        ///</summary>
        static member dots p = Prop(Dots p)
        ///<summary>
        ///If the value is true, it means a continuous
        ///value is included. Otherwise, it is an independent value.
        ///</summary>
        static member included p = Prop(Included p)
        ///<summary>
        ///Minimum allowed value of the slider
        ///</summary>
        static member min p = Prop(Min p)
        ///<summary>
        ///Maximum allowed value of the slider
        ///</summary>
        static member max p = Prop(Max p)
        ///<summary>
        ///pushable could be set as true to allow pushing of
        ///surrounding handles when moving an handle.
        ///When set to a number, the number will be the
        ///minimum ensured distance between handles.
        ///</summary>
        static member pushable p = Prop(Pushable(PushableType.Bool p))
        ///<summary>
        ///pushable could be set as true to allow pushing of
        ///surrounding handles when moving an handle.
        ///When set to a number, the number will be the
        ///minimum ensured distance between handles.
        ///</summary>
        static member pushable p = Prop(Pushable(PushableType.Number p))
        ///<summary>
        ///pushable could be set as true to allow pushing of
        ///surrounding handles when moving an handle.
        ///When set to a number, the number will be the
        ///minimum ensured distance between handles.
        ///</summary>
        static member pushable p = Prop(Pushable p)
        ///<summary>
        ///Configuration for tooltips describing the current slider values
        ///</summary>
        static member tooltip p = Prop(Tooltip p)
        ///<summary>
        ///Value by which increments or decrements are made
        ///</summary>
        static member step p = Prop(Step p)
        ///<summary>
        ///If true, the slider will be vertical
        ///</summary>
        static member vertical p = Prop(Vertical p)
        ///<summary>
        ///The height, in px, of the slider if it is vertical.
        ///</summary>
        static member verticalHeight p = Prop(VerticalHeight p)
        ///<summary>
        ///Determines when the component should update its &#96;value&#96;
        ///property. If &#96;mouseup&#96; (the default) then the slider
        ///will only trigger its value when the user has finished
        ///dragging the slider. If &#96;drag&#96;, then the slider will
        ///update its value continuously as it is being dragged.
        ///Note that for the latter case, the &#96;drag_value&#96;
        ///property could be used instead.
        ///</summary>
        static member updatemode p = Prop(Updatemode p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState p = Prop(LoadingState p)
        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence p = Prop(Persistence p)
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps p = Prop(PersistedProps p)
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType p = Prop(PersistenceType p)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Guid) = Children([ Html.text value ])
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
    ///A double slider with two handles.
    ///Used for specifying a range of numerical values.
    ///</summary>
    type RangeSlider() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?marks,
                ?value,
                ?dragValue,
                ?allowCross,
                ?className,
                ?count,
                ?disabled,
                ?dots,
                ?included,
                ?min,
                ?max,
                ?pushable,
                ?tooltip,
                ?step,
                ?vertical,
                ?verticalHeight,
                ?updatemode,
                ?loadingState,
                ?persistence,
                ?persistedProps,
                ?persistenceType
            ) =
            (fun (t: RangeSlider) ->
                let props = DashComponentProps()
                let setPropValueOpt prop =
                    DynObj.setPropValueOpt props Prop.convert prop

                DynObj.setValue props "id" id
                DynObj.setValue props "children" children

                setPropValueOpt Marks marks
                setPropValueOpt Value value
                setPropValueOpt DragValue dragValue
                setPropValueOpt AllowCross allowCross
                setPropValueOpt ClassName className
                setPropValueOpt Count count
                setPropValueOpt Disabled disabled
                setPropValueOpt Dots dots
                setPropValueOpt Included included
                setPropValueOpt Min min
                setPropValueOpt Max max
                setPropValueOpt Pushable pushable
                setPropValueOpt Tooltip tooltip
                setPropValueOpt Step step
                setPropValueOpt Vertical vertical
                setPropValueOpt VerticalHeight verticalHeight
                setPropValueOpt Updatemode updatemode
                setPropValueOpt LoadingState loadingState
                setPropValueOpt Persistence persistence
                setPropValueOpt PersistedProps persistedProps
                setPropValueOpt PersistenceType persistenceType

                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "RangeSlider"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?marks,
                ?value,
                ?dragValue,
                ?allowCross,
                ?className,
                ?count,
                ?disabled,
                ?dots,
                ?included,
                ?min,
                ?max,
                ?pushable,
                ?tooltip,
                ?step,
                ?vertical,
                ?verticalHeight,
                ?updatemode,
                ?loadingState,
                ?persistence,
                ?persistedProps,
                ?persistenceType
            ) =
            RangeSlider.applyMembers
                (id,
                 children,
                 ?marks = marks,
                 ?value = value,
                 ?dragValue = dragValue,
                 ?allowCross = allowCross,
                 ?className = className,
                 ?count = count,
                 ?disabled = disabled,
                 ?dots = dots,
                 ?included = included,
                 ?min = min,
                 ?max = max,
                 ?pushable = pushable,
                 ?tooltip = tooltip,
                 ?step = step,
                 ?vertical = vertical,
                 ?verticalHeight = verticalHeight,
                 ?updatemode = updatemode,
                 ?loadingState = loadingState,
                 ?persistence = persistence,
                 ?persistedProps = persistedProps,
                 ?persistenceType = persistenceType)
                (RangeSlider())

    ///<summary>
    ///A double slider with two handles.
    ///Used for specifying a range of numerical values.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• marks (dict with values of type: string | record with the fields: 'label: string (optional)', 'style: record (optional)') - Marks on the slider.
    ///The key determines the position (a number),
    ///and the value determines what will show.
    ///If you want to set the style of a specific mark point,
    ///the value should be an object which
    ///contains style and label properties.
    ///&#10;
    ///• value (list with values of type: number) - The value of the input
    ///&#10;
    ///• drag_value (list with values of type: number) - The value of the input during a drag
    ///&#10;
    ///• allowCross (boolean) - allowCross could be set as true to allow those handles to cross.
    ///&#10;
    ///• className (string) - Additional CSS class for the root DOM node
    ///&#10;
    ///• count (number) - Determine how many ranges to render, and multiple handles
    ///will be rendered (number + 1).
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
    ///• pushable (boolean | number) - pushable could be set as true to allow pushing of
    ///surrounding handles when moving an handle.
    ///When set to a number, the number will be the
    ///minimum ensured distance between handles.
    ///&#10;
    ///• tooltip (record with the fields: 'always_visible: boolean (optional)', 'placement: value equal to: 'left', 'right', 'top', 'bottom', 'topLeft', 'topRight', 'bottomLeft', 'bottomRight' (optional)') - Configuration for tooltips describing the current slider values
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
    ///Note that for the latter case, the &#96;drag_value&#96;
    ///property could be used instead.
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
    let rangeSlider (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = RangeSlider.init (id, children)

        let componentProps =
            match t.TryGetTypedValue<DashComponentProps> "props" with
            | Some p -> p
            | None -> DashComponentProps()

        Seq.iter
            (fun (prop: Prop) ->
                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
                DynObj.setValue componentProps fieldName boxedProp)
            props

        DynObj.setValue t "props" componentProps
        t :> DashComponent
