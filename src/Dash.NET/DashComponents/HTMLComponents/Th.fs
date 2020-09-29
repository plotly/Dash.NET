//---
//ComponentName: Th
//camelCaseComponentName: th
//ComponentChar: t
//ComponentNamespace: dash_html_components
//ComponentType: Th
//LibraryNamespace: Dash.NET.HTML
//---

namespace Dash.NET.HTML

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module Th =

    type Th() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (t:Th) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue t "namespace" "dash_html_components"
                    DynObj.setValue t "props" props
                    DynObj.setValue t "type" "Th"

                    t

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                Th()
                |> Th.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let th (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let t = Th.init(children)
        let componentProps = 
            match (t.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue t "props" 
        t :> DashComponent