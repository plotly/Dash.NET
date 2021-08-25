namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////A component that repeatedly increments a counter &#96;n_intervals&#96;
/////with a fixed time delay between each increment.
/////Interval is good for triggering a component on a recurring basis.
/////The time delay is set with the property "interval" in milliseconds.
/////</summary>
//[<RequireQualifiedAccess>]
//module Interval =
//    ///<summary>
//    ///• interval (number; default 1000) - This component will increment the counter &#96;n_intervals&#96; every
//    ///&#96;interval&#96; milliseconds
//    ///&#10;
//    ///• disabled (boolean) - If True, the counter will no longer update
//    ///&#10;
//    ///• n_intervals (number; default 0) - Number of times the interval has passed
//    ///&#10;
//    ///• max_intervals (number; default -1) - Number of times the interval will be fired.
//    ///If -1, then the interval has no limit (the default)
//    ///and if 0 then the interval stops running.
//    ///</summary>
//    type Prop =
//        | Interval of IConvertible
//        | Disabled of bool
//        | NIntervals of IConvertible
//        | MaxIntervals of IConvertible
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Interval (p) -> "interval", box p
//            | Disabled (p) -> "disabled", box p
//            | NIntervals (p) -> "n_intervals", box p
//            | MaxIntervals (p) -> "max_intervals", box p

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///This component will increment the counter &#96;n_intervals&#96; every
//        ///&#96;interval&#96; milliseconds
//        ///</summary>
//        static member interval(p: IConvertible) = Prop(Interval p)
//        ///<summary>
//        ///If True, the counter will no longer update
//        ///</summary>
//        static member disabled(p: bool) = Prop(Disabled p)
//        ///<summary>
//        ///Number of times the interval has passed
//        ///</summary>
//        static member nIntervals(p: IConvertible) = Prop(NIntervals p)
//        ///<summary>
//        ///Number of times the interval will be fired.
//        ///If -1, then the interval has no limit (the default)
//        ///and if 0 then the interval stops running.
//        ///</summary>
//        static member maxIntervals(p: IConvertible) = Prop(MaxIntervals p)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: int) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: string) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: float) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: System.Guid) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: DashComponent) = Children([ value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: list<DashComponent>) = Children(value)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

//    ///<summary>
//    ///A component that repeatedly increments a counter &#96;n_intervals&#96;
//    ///with a fixed time delay between each increment.
//    ///Interval is good for triggering a component on a recurring basis.
//    ///The time delay is set with the property "interval" in milliseconds.
//    ///</summary>
//    type Interval() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?interval: string,
//                ?disabled: string,
//                ?n_intervals: string,
//                ?max_intervals: string
//            ) =
//            (fun (t: Interval) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "interval" interval
//                DynObj.setValueOpt props "disabled" disabled
//                DynObj.setValueOpt props "n_intervals" n_intervals
//                DynObj.setValueOpt props "max_intervals" max_intervals
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "Interval"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?interval: string,
//                ?disabled: string,
//                ?n_intervals: string,
//                ?max_intervals: string
//            ) =
//            Interval.applyMembers
//                (id,
//                 children,
//                 ?interval = interval,
//                 ?disabled = disabled,
//                 ?n_intervals = n_intervals,
//                 ?max_intervals = max_intervals)
//                (Interval())

//    ///<summary>
//    ///A component that repeatedly increments a counter &#96;n_intervals&#96;
//    ///with a fixed time delay between each increment.
//    ///Interval is good for triggering a component on a recurring basis.
//    ///The time delay is set with the property "interval" in milliseconds.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• interval (number; default 1000) - This component will increment the counter &#96;n_intervals&#96; every
//    ///&#96;interval&#96; milliseconds
//    ///&#10;
//    ///• disabled (boolean) - If True, the counter will no longer update
//    ///&#10;
//    ///• n_intervals (number; default 0) - Number of times the interval has passed
//    ///&#10;
//    ///• max_intervals (number; default -1) - Number of times the interval will be fired.
//    ///If -1, then the interval has no limit (the default)
//    ///and if 0 then the interval stops running.
//    ///</summary>
//    let interval (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = Interval.init (id, children)

//        let componentProps =
//            match t.TryGetTypedValue<DashComponentProps> "props" with
//            | Some (p) -> p
//            | None -> DashComponentProps()

//        Seq.iter
//            (fun (prop: Prop) ->
//                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
//                DynObj.setValue componentProps fieldName boxedProp)
//            props

//        DynObj.setValue t "props" componentProps
//        t :> DashComponent
