//---
//ComponentName: Colgroup
//camelCaseComponentName: colgroup
//ComponentChar: c
//ComponentNamespace: dash_html_components
//ComponentType: Colgroup
//LibraryNamespace: Dash.NET.HTML_DSL
//---

namespace Dash.NET.HTML_DSL

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module Colgroup =

    type Colgroup() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (c:Colgroup) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue c "namespace" "dash_html_components"
                    DynObj.setValue c "props" props
                    DynObj.setValue c "type" "Colgroup"

                    c

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                Colgroup()
                |> Colgroup.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let colgroup (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let c = Colgroup.init(children)
        let componentProps = 
            match (c.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue c "props" 
        c :> DashComponent