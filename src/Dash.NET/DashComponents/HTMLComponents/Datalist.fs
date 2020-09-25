//---
//ComponentName: Datalist
//camelCaseComponentName: datalist
//ComponentChar: d
//ComponentNamespace: dash_html_components
//ComponentType: Datalist
//LibraryNamespace: Dash.NET.HTML_DSL
//---

namespace Dash.NET.HTML_DSL

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module Datalist =

    type Datalist() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (d:Datalist) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue d "namespace" "dash_html_components"
                    DynObj.setValue d "props" props
                    DynObj.setValue d "type" "Datalist"

                    d

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                Datalist()
                |> Datalist.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let datalist (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let d = Datalist.init(children)
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