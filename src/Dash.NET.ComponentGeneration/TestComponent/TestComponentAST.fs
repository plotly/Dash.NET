//------------------------------------------------------------------------------
//        This file has been automatically generated.
//        Changes to this file will be lost when the code is regenerated.
//------------------------------------------------------------------------------
namespace TestNamespace

open Dash.NET
open System
open Plotly.NET

///This is additional test documentation
[<RequireQualifiedAccess>]
module TestComponent =
    ///This is test documentation
    type TestComponentProps =
        | NormalProp of string
        | ``Prop🥑`` of string
        | Prop_test of string
        static member toDynamicMemberDef(prop: TestComponentProps) =
            match prop with
            | NormalProp (p) -> "normalProp", box p
            | ``Prop🥑`` (p) -> "🥑", box p
            | Prop_test (p) -> "_test", box p

    ///This is additional test documentation
    type TestComponent() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?normalProp: string,
                ?``🥑``: string,
                ?_test: string
            ) =
            (fun (t: TestComponent) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "normalProp" normalProp
                DynObj.setValueOpt props "🥑" ``🥑``
                DynObj.setValueOpt props "_test" _test
                DynObj.setValue t "namespace" "TestNamespace"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "TestType"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?normalProp: string,
                ?``🥑``: string,
                ?_test: string
            ) =
            TestComponent.applyMembers
                (id, children, ?normalProp = normalProp, ?``🥑`` = ``🥑``, ?_test = _test)
                (TestComponent())

    ///This is additional test documentation
    let testComponent (id: string) (props: seq<TestComponentProps>) (children: seq<DashComponent>) =
        let t = TestComponent.init (id, children)

        let componentProps =
            match t.TryGetTypedValue<DashComponentProps> "props" with
            | Some (p) -> p
            | None -> DashComponentProps()

        Seq.iter
            (fun (prop: TestComponentProps) ->
                let fieldName, boxedProp =
                    TestComponentProps.toDynamicMemberDef prop

                DynObj.setValue componentProps fieldName boxedProp)
            props

        DynObj.setValue t "props" componentProps
        t :> DashComponent
