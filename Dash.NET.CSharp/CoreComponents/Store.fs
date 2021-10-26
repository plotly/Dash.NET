namespace Dash.NET.CSharp.DCC

open System
open Dash.NET.CSharp.ComponentStyle

///<summary>
///Easily keep data on the client side with this component.
///The data is not inserted in the DOM.
///Data can be in memory, localStorage or sessionStorage.
///The data will be kept with the id as key.
///</summary>
[<RequireQualifiedAccess>]
module Store =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Store.Attr

    
    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Store.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Store.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///The type of the web storage.
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member storageType(p: PersistenceTypeOptions) = 
            guardAgainstNull "p" p
            OAttr.storageType (p |> PersistenceTypeOptions.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The stored data for the id.
        ///</summary>
        static member data(p: string) = 
            guardAgainstNull "p" p
            OAttr.data p |> Attr.Wrap
        ///<summary>
        ///Set to true to remove the data contained in &#96;data_key&#96;.
        ///</summary>
        static member clearData(p: bool) = 
            guardAgainstNull "p" p
            OAttr.clearData p |> Attr.Wrap
        ///<summary>
        ///The last time the storage was modified.
        ///</summary>
        static member modifiedTimestamp(p: int64) = 
            guardAgainstNull "p" p
            OAttr.modifiedTimestamp p |> Attr.Wrap
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
    ///Easily keep data on the client side with this component.
    ///The data is not inserted in the DOM.
    ///Data can be in memory, localStorage or sessionStorage.
    ///The data will be kept with the id as key.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• storage_type (value equal to: 'local', 'session', 'memory'; default memory) - The type of the web storage.
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///&#10;
    ///• data (record | list | number | string | boolean) - The stored data for the id.
    ///&#10;
    ///• clear_data (boolean; default false) - Set to true to remove the data contained in &#96;data_key&#96;.
    ///&#10;
    ///• modified_timestamp (number; default -1) - The last time the storage was modified.
    ///</summary>
    let radioItems (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Store.store id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap