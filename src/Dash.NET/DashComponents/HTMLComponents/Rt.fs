//---
//ComponentName: Rt
//camelCaseComponentName: rt
//ComponentChar: r
//ComponentNamespace: dash_html_components
//ComponentType: Rt
//LibraryNamespace: Dash.NET.HTML_DSL
//---

namespace Dash.NET.HTML_DSL

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module Rt =

    type Rt() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (r:Rt) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue r "namespace" "dash_html_components"
                    DynObj.setValue r "props" props
                    DynObj.setValue r "type" "Rt"

                    r

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                Rt()
                |> Rt.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let rt (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let r = Rt.init(children)
        let componentProps = 
            match (r.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue r "props" 
        r :> DashComponent