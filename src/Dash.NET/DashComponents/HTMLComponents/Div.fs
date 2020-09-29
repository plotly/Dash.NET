//---
//ComponentName: Div
//camelCaseComponentName: div
//ComponentChar: d
//ComponentNamespace: dash_html_components
//ComponentType: Div
//LibraryNamespace: Dash.NET.HTML
//---

namespace Dash.NET.HTML

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module Div =

    type Div() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (d:Div) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue d "namespace" "dash_html_components"
                    DynObj.setValue d "props" props
                    DynObj.setValue d "type" "Div"

                    d

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                Div()
                |> Div.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let div (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let d = Div.init(children)
        let componentProps = 
            match (d.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue d "props" 
        d :> DashComponent