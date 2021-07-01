//---
//ComponentName: TestComponent
//camelCaseComponentName: testComponent
//ComponentChar: t
//ComponentNamespace: TestNamespace
//ComponentType: TestType
//LibraryNamespace: TestNamespace
//---

namespace ``TestNamespace``

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module ``TestComponent`` =

    type ``TestComponentProps`` =
        | ``NormalProp`` of string | ``P🥑`` of string | ``P_test`` of string 
        static member toDynamicMemberDef (prop:``TestComponentProps``) =
            match prop with
            | ``NormalProp`` p -> "normalProp", box p | ``P🥑`` p -> "🥑", box p | ``P_test`` p -> "_test", box p 
            
    type ``TestComponent``() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?``NormalProp``: string, ?``🥑``: string, ?``_test``: string 
            ) =
            (
                fun (``t``:``TestComponent``) -> 

                    let props = DashComponentProps()

                    id |> DynObj.setValue props "id"
                    children |> DynObj.setValue props "children"

                    ``NormalProp`` |> DynObj.setValueOpt props "normalProp"; ``🥑`` |> DynObj.setValueOpt props "🥑"; ``_test`` |> DynObj.setValueOpt props "_test"; 

                    DynObj.setValue ``t`` "namespace" @"TestNamespace"
                    DynObj.setValue ``t`` "props" props
                    DynObj.setValue ``t`` "type" @"TestType"

                    ``t``

            )
        static member init 
            (
                id,
                children,
                ?``NormalProp``: string, ?``🥑``: string, ?``_test``: string 
            ) = 
                ``TestComponent``()
                |> ``TestComponent``.applyMembers 
                    (
                        id,
                        children,
                        ?``NormalProp`` = ``NormalProp``, ?``🥑`` = ``🥑``, ?``_test`` = ``_test`` 
                    )

    let ``testComponent`` (id:string) (props:seq<``TestComponentProps``>) (children:seq<DashComponent>) =
        let ``t`` = ``TestComponent``.init(id,children)
        let componentProps = 
            match (``t``.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> ``TestComponentProps``.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue ``t`` "props" 
        ``t`` :> DashComponent

