//---
//ComponentName: RadioItems
//camelCaseComponentName: radioItems
//ComponentChar: r
//ComponentNamespace: dash_core_components
//ComponentType: RadioItems
//LibraryNamespace: Dash.NET.DCC
//---

namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module RadioItems =

    type RadioItemsProps =
        | ClassName of string
        | Style of DashComponentStyle
        | Options of seq<RadioItemsOption>
        | Value of IConvertible
        | InputClassName of string
        | InputStyle of DashComponentStyle
        | LabelClassName of string
        | LabelStyle of DashComponentStyle
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions
        static member toDynamicMemberDef (prop:RadioItemsProps) =
            match prop with
            | ClassName         p   -> "className"          , box p
            | Style             p   -> "style"              , box p
            | Options           p   -> "options"            , box p
            | Value             p   -> "value"              , box p
            | InputClassName    p   -> "inputClassName"     , box p
            | InputStyle        p   -> "inputStyle"         , box p
            | LabelClassName    p   -> "labelClassName"     , box p
            | LabelStyle        p   -> "labelStyle"         , box p
            | LoadingState      p   -> "loading_state"      , box p
            | Persistence       p   -> "persistence"        , box p
            | PersistedProps    p   -> "persisted_props"    , box p
            | PersistenceType   p   -> "persistence_type"   , PersistenceTypeOptions.convert p
            
    type RadioItems() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?ClassName      : string,
                ?Style          : DashComponentStyle,
                ?Options        : seq<RadioItemsOption>,
                ?Value          : IConvertible,
                ?InputClassName : string,
                ?InputStyle     : DashComponentStyle,
                ?LabelClassName : string,
                ?LabelStyle     : DashComponentStyle,
                ?LoadingState   : LoadingState,
                ?Persistence    : IConvertible,
                ?PersistedProps : string [],
                ?PersistenceType: PersistenceTypeOptions
            ) =
            (
                fun (r:RadioItems) -> 

                    let props = DashComponentProps()

                    id              |> DynObj.setValue props "id"
                    children        |> DynObj.setValue props "children"
                    ClassName       |> DynObj.setValueOpt props "className"
                    Style           |> DynObj.setValueOpt props "style"
                    Options         |> DynObj.setValueOpt props "options"
                    Value           |> DynObj.setValueOpt props "value"
                    InputClassName  |> DynObj.setValueOpt props "inputClassName"
                    InputStyle      |> DynObj.setValueOpt props "inputStyle"
                    LabelClassName  |> DynObj.setValueOpt props "labelClassName"
                    LabelStyle      |> DynObj.setValueOpt props "labelStyle"
                    LoadingState    |> DynObj.setValueOpt props "loading_state"
                    Persistence     |> DynObj.setValueOpt props "persistence"
                    PersistedProps  |> DynObj.setValueOpt props "persisted_props"
                    PersistenceType |> DynObj.setValueOpt props "persistence_type"

                    DynObj.setValue r "namespace" "dash_core_components"
                    DynObj.setValue r "props" props
                    DynObj.setValue r "type" "RadioItems"

                    r

            )
        static member init 
            (
                id,
                children,
                ?ClassName,
                ?Style,
                ?Options,
                ?Value,
                ?InputClassName,
                ?InputStyle,
                ?LabelClassName,
                ?LabelStyle,
                ?LoadingState,
                ?Persistence,
                ?PersistedProps,
                ?PersistenceType
            ) = 
                RadioItems()
                |> RadioItems.applyMembers 
                    (
                        id,
                        children,   
                        ?ClassName          = ClassName,
                        ?Style              = Style,
                        ?Options            = Options,
                        ?Value              = Value,
                        ?InputClassName     = InputClassName,
                        ?InputStyle         = InputStyle,
                        ?LabelClassName     = LabelClassName,
                        ?LabelStyle         = LabelStyle,
                        ?LoadingState       = LoadingState,
                        ?Persistence        = Persistence,
                        ?PersistedProps     = PersistedProps,
                        ?PersistenceType    = PersistenceType
                    )

    let radioItems (id:string) (props:seq<RadioItemsProps>) (children:seq<DashComponent>) =
        let r = RadioItems.init(id,children)
        let componentProps = 
            match (r.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> RadioItemsProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue r "props" 
        r :> DashComponent