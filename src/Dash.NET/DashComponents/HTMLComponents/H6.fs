//---
//ComponentName: H6
//camelCaseComponentName: h6
//ComponentChar: h
//ComponentNamespace: dash_html_components
//ComponentType: H6
//LibraryNamespace: Dash.NET.HTML
//---

namespace Dash.NET.HTML

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module H6 =

    type H6() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (h:H6) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue h "namespace" "dash_html_components"
                    DynObj.setValue h "props" props
                    DynObj.setValue h "type" "H6"

                    h

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                H6()
                |> H6.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let h6 (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let h = H6.init(children)
        let componentProps = 
            match (h.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue h "props" 
        h :> DashComponent