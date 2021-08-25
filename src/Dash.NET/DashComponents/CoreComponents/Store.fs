namespace Dash.NET.DCC

open Dash.NET
open Plotly.NET
open ComponentPropTypes

///<summary>
///Easily keep data on the client side with this component.
///The data is not inserted in the DOM.
///Data can be in memory, localStorage or sessionStorage.
///The data will be kept with the id as key.
///</summary>
[<RequireQualifiedAccess>]
module Store =
    ///<summary>
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
    type Prop =
        | ClearData         of bool
        | Data              of string
        | ModifiedTimestamp of int
        | StorageType       of PersistenceTypeOptions
        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | ClearData         p-> "clear_data"        , box p
            | Data              p-> "data"              , box p
            | ModifiedTimestamp p-> "modified_timestamp", box p
            | StorageType       p-> "storage_type"      , box p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The type of the web storage.
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member storageType(p: PersistenceTypeOptions) = Prop(StorageType p)
        ///<summary>
        ///The stored data for the id.
        ///</summary>
        static member data(p: string) = Prop(Data p)
        ///<summary>
        ///Set to true to remove the data contained in &#96;data_key&#96;.
        ///</summary>
        static member clearData(p: bool) = Prop(ClearData p)
        ///<summary>
        ///The last time the storage was modified.
        ///</summary>
        static member modifiedTimestamp(p: int) = Prop(ModifiedTimestamp p)
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
    ///Easily keep data on the client side with this component.
    ///The data is not inserted in the DOM.
    ///Data can be in memory, localStorage or sessionStorage.
    ///The data will be kept with the id as key.
    ///</summary>
    type Store() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?storage_type: string,
                ?data: string,
                ?clear_data: string,
                ?modified_timestamp: string
            ) =
            (fun (t: Store) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "storage_type" storage_type
                DynObj.setValueOpt props "data" data
                DynObj.setValueOpt props "clear_data" clear_data
                DynObj.setValueOpt props "modified_timestamp" modified_timestamp
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Store"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?storage_type: string,
                ?data: string,
                ?clear_data: string,
                ?modified_timestamp: string
            ) =
            Store.applyMembers
                (id,
                 children,
                 ?storage_type = storage_type,
                 ?data = data,
                 ?clear_data = clear_data,
                 ?modified_timestamp = modified_timestamp)
                (Store())

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
    let store (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Store.init (id, children)

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
