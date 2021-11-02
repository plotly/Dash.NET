namespace Dash.NET.CSharp.DCC

open System
open Dash.NET.CSharp.ComponentStyle

///<summary>
///A component that repeatedly increments a counter &#96;n_intervals&#96;
///with a fixed time delay between each increment.
///Interval is good for triggering a component on a recurring basis.
///The time delay is set with the property "interval" in milliseconds.
///</summary>
[<RequireQualifiedAccess>]
module Interval =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Interval.Attr


    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Interval.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Interval.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///This component will increment the counter &#96;n_intervals&#96; every
        ///&#96;interval&#96; milliseconds
        ///</summary>
        static member interval(p: IConvertible) = 
            guardAgainstNull "p" p
            OAttr.interval p |> Attr.Wrap
        ///<summary>
        ///If True, the counter will no longer update
        ///</summary>
        static member disabled(p: bool) = 
            guardAgainstNull "p" p
            OAttr.disabled p |> Attr.Wrap
        ///<summary>
        ///Number of times the interval has passed
        ///</summary>
        static member nIntervals(p: IConvertible) = 
            guardAgainstNull "p" p
            OAttr.nIntervals p |> Attr.Wrap
        ///<summary>
        ///Number of times the interval will be fired.
        ///If -1, then the interval has no limit (the default)
        ///and if 0 then the interval stops running.
        ///</summary>
        static member maxIntervals(p: IConvertible) = 
            guardAgainstNull "p" p
            OAttr.maxIntervals p |> Attr.Wrap
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
    ///A component that repeatedly increments a counter &#96;n_intervals&#96;
    ///with a fixed time delay between each increment.
    ///Interval is good for triggering a component on a recurring basis.
    ///The time delay is set with the property "interval" in milliseconds.
    ///<para>&#160;</para>
    ///Properties:
    ///<para>&#160;</para>
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///<para>&#160;</para>
    ///• interval (number; default 1000) - This component will increment the counter &#96;n_intervals&#96; every
    ///&#96;interval&#96; milliseconds
    ///<para>&#160;</para>
    ///• disabled (boolean) - If True, the counter will no longer update
    ///<para>&#160;</para>
    ///• n_intervals (number; default 0) - Number of times the interval has passed
    ///<para>&#160;</para>
    ///• max_intervals (number; default -1) - Number of times the interval will be fired.
    ///If -1, then the interval has no limit (the default)
    ///and if 0 then the interval stops running.
    ///</summary>
    let interval (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Interval.interval id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap