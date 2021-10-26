namespace Dash.NET.CSharp.DCC

open System
open Dash.NET.CSharp.ComponentStyle

///<summary>
///Update and track the current window.location object through the window.history state.
///Use in conjunction with the &#96;dash_core_components.Link&#96; component to make apps with multiple pages.
///</summary>
[<RequireQualifiedAccess>]
module Location =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Location.Attr

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Location.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Location.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///pathname in window.location - e.g., "/my/full/pathname"
        ///</summary>
        static member pathname(p: string) = 
            guardAgainstNull "p" p
            OAttr.pathname p |> Attr.Wrap
        ///<summary>
        ///search in window.location - e.g., "?myargument=1"
        ///</summary>
        static member search(p: string) = 
            guardAgainstNull "p" p
            OAttr.search p |> Attr.Wrap
        ///<summary>
        ///hash in window.location - e.g., "#myhash"
        ///</summary>
        static member hash(p: string) = 
            guardAgainstNull "p" p
            OAttr.hash p |> Attr.Wrap
        ///<summary>
        ///href in window.location - e.g., "/my/full/pathname?myargument=1#myhash"
        ///</summary>
        static member href(p: string) = 
            guardAgainstNull "p" p
            OAttr.href p |> Attr.Wrap
        ///<summary>
        ///Refresh the page when the location is updated?
        ///</summary>
        static member refresh(p: bool) = 
            guardAgainstNull "p" p
            OAttr.refresh p |> Attr.Wrap
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
    ///Update and track the current window.location object through the window.history state.
    ///Use in conjunction with the &#96;dash_core_components.Link&#96; component to make apps with multiple pages.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• pathname (string) - pathname in window.location - e.g., "/my/full/pathname"
    ///&#10;
    ///• search (string) - search in window.location - e.g., "?myargument=1"
    ///&#10;
    ///• hash (string) - hash in window.location - e.g., "#myhash"
    ///&#10;
    ///• href (string) - href in window.location - e.g., "/my/full/pathname?myargument=1#myhash"
    ///&#10;
    ///• refresh (boolean; default true) - Refresh the page when the location is updated?
    ///</summary>
    let location (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Location.location id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap