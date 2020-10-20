//---
//ComponentName: Store
//camelCaseComponentName: store
//ComponentChar: s
//ComponentNamespace: dash_core_components
//ComponentType: Store
//LibraryNamespace: Dash.NET.DCC
//---

namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module Store =

    type StoreProps =
        | ClearData         of bool
        | Data              of string
        | ModifiedTimestamp of int
        | StorageType       of PersistenceTypeOptions
        static member toDynamicMemberDef (prop:StoreProps) =
            match prop with
            | ClearData         p-> "clear_data"        , box p
            | Data              p-> "data"              , box p
            | ModifiedTimestamp p-> "modified_timestamp", box p
            | StorageType       p-> "storage_type"      , box p
            
    type Store() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?ClearData         : bool,
                ?Data              : string,
                ?ModifiedTimestamp : int,
                ?StorageType       : PersistenceTypeOptions
            ) =
            (
                fun (s:Store) -> 

                    let props = DashComponentProps()

                    id |> DynObj.setValue props "id"
                    children |> DynObj.setValue props "children"

                    ClearData         |> DynObj.setValueOpt props "clear_data"        
                    Data              |> DynObj.setValueOpt props "data"              
                    ModifiedTimestamp |> DynObj.setValueOpt props "modified_timestamp"
                    StorageType       |> DynObj.setValueOpt props "storage_type"      
                    
                    DynObj.setValue s "namespace" "dash_core_components"
                    DynObj.setValue s "props" props
                    DynObj.setValue s "type" "Store"

                    s

            )
        static member init 
            (
                id,
                children,
                ?ClearData,
                ?Data,                
                ?ModifiedTimestamp,
                ?StorageType      
            ) = 
                Store()
                |> Store.applyMembers 
                    (
                        id,
                        children,
                        ?ClearData         = ClearData        ,
                        ?Data              = Data             ,
                        ?ModifiedTimestamp = ModifiedTimestamp,
                        ?StorageType       = StorageType      
                    )

    let store (id:string) (props:seq<StoreProps>) (children:seq<DashComponent>) =
        let s = Store.init(id,children)
        let componentProps = 
            match (s.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> StoreProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue s "props" 
        s :> DashComponent