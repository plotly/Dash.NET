//---
//ComponentName: Acronym
//camelCaseComponentName: acronym
//ComponentChar: a
//ComponentNamespace: dash_html_components
//ComponentType: Acronym
//LibraryNamespace: Dash.NET.HTML
//---

namespace Dash.NET.HTML

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<RequireQualifiedAccess>]
module Acronym =

    type Acronym() =
        inherit DashComponent()
        static member applyMembers
            (
                children : seq<DashComponent>,
                ?Id : string,
                ?ClassName : string,
                ?Style : DashComponentStyle
            ) =
            (
                fun (a:Acronym) -> 

                    let props = DashComponentProps()

                    children 
                    |> DashComponent.transformChildren
                    |> DynObj.setValue props "children"

                    Id |> DynObj.setValueOpt props "id"
                    ClassName |> DynObj.setValueOpt props "className"
                    Style |> DynObj.setValueOpt props "style"

                    DynObj.setValue a "namespace" "dash_html_components"
                    DynObj.setValue a "props" props
                    DynObj.setValue a "type" "Acronym"

                    a

            )
        static member init 
            (
                children,
                ?Id,
                ?ClassName,
                ?Style
            ) = 
                Acronym()
                |> Acronym.applyMembers 
                    (
                        children,
                        ?Id = Id,
                        ?ClassName = ClassName,
                        ?Style = Style
                    )

    let acronym (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let a = Acronym.init(children)
        let componentProps = 
            match (a.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> HTMLProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue a "props" 
        a :> DashComponent