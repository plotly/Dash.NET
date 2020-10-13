//---
//ComponentName: Tabs
//camelCaseComponentName: tabs
//ComponentChar: t
//ComponentNamespace: dash_core_components
//ComponentType: Tabs
//LibraryNamespace: Dash.NET.DCC
//---

namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module Tabs =

    type TabsProps =
        | ClassName         of string
        | Style             of DashComponentStyle
        | Value             of string
        | ContentClassName  of string
        | ParentClassName   of string
        | ParentStyle       of DashComponentStyle
        | ContentStyle      of DashComponentStyle
        | Vertical          of bool
        | MobileBreakpoint  of float
        | Colors            of TabColors
        | LoadingState      of LoadingState
        | Persistence       of IConvertible
        | PersistedProps    of string []
        | PersistenceType   of PersistenceTypeOptions


        static member toDynamicMemberDef (prop:TabsProps) =
            match prop with
            | ClassName p           -> "className", box p
            | Style p               -> "style" , box p
            | Value p               -> "value" , box p
            | ContentClassName p    -> "content_className" , box p
            | ParentClassName p     -> "parent_className" , box p
            | ParentStyle p         -> "parent_style" , box p
            | ContentStyle p        -> "content_style" , box p
            | Vertical p            -> "vertical" , box p
            | MobileBreakpoint p    -> "mobile_breakpoint" , box p
            | Colors p              -> "colors" , box p
            | LoadingState p        -> "loading_state", box p
            | Persistence p         -> "persistence", box p
            | PersistedProps p      -> "persisted_props", box p
            | PersistenceType p     -> "persistence_type", PersistenceTypeOptions.convert p
            
    type Tabs() =
        inherit DashComponent()
        static member applyMembers
            (
                id                  : string,
                children            : seq<DashComponent>,
                ?ClassName          : string,
                ?Style              : DashComponentStyle,
                ?Value              : string,
                ?ContentClassName   : string,
                ?ParentClassName    : string,
                ?ParentStyle        : DashComponentStyle,
                ?ContentStyle       : DashComponentStyle,
                ?Vertical           : bool,
                ?MobileBreakpoint   : float,
                ?Colors             : TabColors,
                ?LoadingState       : LoadingState,
                ?Persistence        : IConvertible,
                ?PersistedProps     : string [],
                ?PersistenceType    : PersistenceTypeOptions
                
            ) =
            (
                fun (t:Tabs) -> 

                    let props = DashComponentProps()

                    id              |> DynObj.setValue props "id"
                    children        |> DynObj.setValue props "children"
                    ClassName       |> DynObj.setValueOpt props "className"
                    Style           |> DynObj.setValueOpt props "style"
                    Value           |> DynObj.setValueOpt props "value"
                    ContentClassName|> DynObj.setValueOpt props "content_className"
                    ParentClassName |> DynObj.setValueOpt props "parent_className"
                    ParentStyle     |> DynObj.setValueOpt props "parent_style"
                    ContentStyle    |> DynObj.setValueOpt props "content_style"
                    Vertical        |> DynObj.setValueOpt props "vertical"
                    MobileBreakpoint|> DynObj.setValueOpt props "mobile_breakpoint"
                    Colors          |> DynObj.setValueOpt props "colors"
                    LoadingState    |> DynObj.setValueOpt props "loading_state"
                    Persistence     |> DynObj.setValueOpt props "persistence"
                    PersistedProps  |> DynObj.setValueOpt props "persisted_props"
                    PersistenceType |> DynObj.setValueOpt props "persistence_type"

                    DynObj.setValue t "namespace" "dash_core_components"
                    DynObj.setValue t "props" props
                    DynObj.setValue t "type" "Tabs"

                    t

            )
        static member init 
            (
                id,
                children,
                ?ClassName       ,
                ?Style           ,
                ?Value           ,
                ?ContentClassName,
                ?ParentClassName ,
                ?ParentStyle     ,
                ?ContentStyle    ,
                ?Vertical        ,
                ?MobileBreakpoint,
                ?Colors          ,
                ?LoadingState    ,
                ?Persistence     ,
                ?PersistedProps  ,
                ?PersistenceType 
            ) = 
                Tabs()
                |> Tabs.applyMembers 
                    (
                        id,
                        children,
                        ?ClassName       = ClassName       ,
                        ?Style           = Style           ,
                        ?Value           = Value           ,
                        ?ContentClassName= ContentClassName,
                        ?ParentClassName = ParentClassName ,
                        ?ParentStyle     = ParentStyle     ,
                        ?ContentStyle    = ContentStyle    ,
                        ?Vertical        = Vertical        ,
                        ?MobileBreakpoint= MobileBreakpoint,
                        ?Colors          = Colors          ,
                        ?LoadingState    = LoadingState    ,
                        ?Persistence     = Persistence     ,
                        ?PersistedProps  = PersistedProps  ,
                        ?PersistenceType = PersistenceType 
                    )

    let tabs (id:string) (props:seq<TabsProps>) (children:seq<DashComponent>) =
        let t = Tabs.init(id,children)
        let componentProps = 
            match (t.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> TabsProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue t "props" 
        t :> DashComponent