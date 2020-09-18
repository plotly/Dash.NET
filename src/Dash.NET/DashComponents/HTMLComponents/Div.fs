namespace Dash.NET.HTML_DSL

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

module Div = 

    type Div() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?ClassName: string
            ) =
            (fun (d:Div) ->

                let props = DashComponentProps()
                
                id |> DynObj.setValue props "id"
                children |> DynObj.setValue props "children"
                ClassName |> DynObj.setValueOpt props "className"

                DynObj.setValue d "namespace" "dash_html_components"
                DynObj.setValue d "props" props
                DynObj.setValue d "type" "Div"

                d
            )

        static member init 
            (
                id,
                children,
                ?ClassName
            ) = 
            Div()
            |> Div.applyMembers
                (
                    id,
                    children,
                    ?ClassName = ClassName
                )

    let div (id:string) (props:seq<HTMLProps>) (children:seq<DashComponent>) =
        let d = Div.init(id,children)
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