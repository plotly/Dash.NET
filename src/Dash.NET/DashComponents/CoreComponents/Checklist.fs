//---
//ComponentName: Checklist
//camelCaseComponentName: checklist
//ComponentChar: c
//ComponentNamespace: dash_core_components
//ComponentType: Checklist
//LibraryNamespace: Dash.NET.DCC
//---

namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module Checklist =

    type ChecklistProps =
        | ClassName of string
        static member toDynamicMemberDef (prop:ChecklistProps) =
            match prop with
            | ClassName p           -> "className", box p
            
    type Checklist() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (c:Checklist) -> 

                    let props = DashComponentProps()

                    id |> DynObj.setValue props "id"
                    children |> DynObj.setValue props "children"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue c "namespace" "dash_core_components"
                    DynObj.setValue c "props" props
                    DynObj.setValue c "type" "Checklist"

                    c

            )
        static member init 
            (
                id,
                children,
                ?ClassName,
                ?Style
            ) = 
                Checklist()
                |> Checklist.applyMembers 
                    (
                        id,
                        children,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let checklist (id:string) (props:seq<ChecklistProps>) (children:seq<DashComponent>) =
        let c = Checklist.init(id,children)
        let componentProps = 
            match (c.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> ChecklistProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue c "props" 
        c :> DashComponent