//---
//ComponentName: Tab
//camelCaseComponentName: tab
//ComponentChar: t
//ComponentNamespace: dash_core_components
//ComponentType: Tab
//LibraryNamespace: Dash.NET.DCC
//---

namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module Tab =

    type TabProps =
        | ClassName         of string
        | Style             of DashComponentStyle
        | Label             of string
        | Value             of string
        | Disabled          of bool
        | DisabledStyle     of DashComponentStyle
        | DisabledClassName of string
        | SelectedClassName of string
        | SelectedStyle     of string
        | LoadingState      of LoadingState

        static member toDynamicMemberDef (prop:TabProps) =
            match prop with
            | ClassName p           -> "className", box p
            | Style p               -> "style", box p
            | Label p               -> "label", box p
            | Value p               -> "value", box p
            | Disabled p            -> "disabled", box p
            | DisabledStyle p       -> "disabled_style", box p
            | DisabledClassName p   -> "disabled_className", box p
            | SelectedClassName p   -> "selected_className", box p
            | SelectedStyle p       -> "selected_style", box p
            | LoadingState p        -> "loading_state", box p
            
    type Tab() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?ClassName          : string,
                ?Style              : DashComponentStyle,
                ?Label              : string,
                ?Value              : string,
                ?Disabled           : bool,
                ?DisabledStyle      : DashComponentStyle,
                ?DisabledClassName  : string,
                ?SelectedClassName  : string,
                ?SelectedStyle      : string,
                ?LoadingState       : LoadingState
            ) =
            (
                fun (t:Tab) -> 

                    let props = DashComponentProps()

                    id |> DynObj.setValue props "id"
                    children |> DynObj.setValue props "children"
                    ClassName           |> DynObj.setValueOpt props "className"
                    Style               |> DynObj.setValueOpt props "style"
                    Label               |> DynObj.setValueOpt props "label"
                    Value               |> DynObj.setValueOpt props "value"
                    Disabled            |> DynObj.setValueOpt props "disabled"
                    DisabledStyle       |> DynObj.setValueOpt props "disabled_style"
                    DisabledClassName   |> DynObj.setValueOpt props "disabled_className"
                    SelectedClassName   |> DynObj.setValueOpt props "selected_className"
                    SelectedStyle       |> DynObj.setValueOpt props "selected_style"
                    LoadingState        |> DynObj.setValueOpt props "loading_state"

                    DynObj.setValue t "namespace" "dash_core_components"
                    DynObj.setValue t "props" props
                    DynObj.setValue t "type" "Tab"

                    t

            )
        static member init 
            (
                id,
                children,
                ?ClassName,
                ?Style,
                ?Label,
                ?Value,
                ?Disabled,
                ?DisabledStyle,
                ?DisabledClassName,
                ?SelectedClassName,
                ?SelectedStyle,
                ?LoadingState     
            ) = 
                Tab()
                |> Tab.applyMembers 
                    (
                        id,
                        children,
                        ?ClassName         = ClassName        ,
                        ?Style             = Style            ,
                        ?Label             = Label            ,
                        ?Value             = Value            ,
                        ?Disabled          = Disabled         ,
                        ?DisabledStyle     = DisabledStyle    ,
                        ?DisabledClassName = DisabledClassName,
                        ?SelectedClassName = SelectedClassName,
                        ?SelectedStyle     = SelectedStyle    ,
                        ?LoadingState      = LoadingState     
                    )

    let tab (id:string) (props:seq<TabProps>) (children:seq<DashComponent>) =
        let t = Tab.init(id,children)
        let componentProps = 
            match (t.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> TabProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue t "props" 
        t :> DashComponent