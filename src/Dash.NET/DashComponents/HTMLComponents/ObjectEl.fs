//---
//ComponentName: ObjectEl
//camelCaseComponentName: objectEl
//ComponentChar: o
//ComponentNamespace: dash_html_components
//ComponentType: ObjectEl
//LibraryNamespace: Dash.NET.HTML_DSL
//---

namespace Dash.NET.HTML_DSL

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module ObjectEl =

    type ObjectEl() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (o:ObjectEl) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue o "namespace" "dash_html_components"
                    DynObj.setValue o "props" props
                    DynObj.setValue o "type" "ObjectEl"

                    o

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                ObjectEl()
                |> ObjectEl.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let objectEl (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let o = ObjectEl.init(children)
        let componentProps = 
            match (o.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue o "props" 
        o :> DashComponent