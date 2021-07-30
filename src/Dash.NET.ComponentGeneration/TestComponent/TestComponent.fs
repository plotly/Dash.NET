//------------------------------------------------------------------------------
//        This file has been automatically generated.
//        Changes to this file will be lost when the code is regenerated.
//------------------------------------------------------------------------------
namespace TestComponent

open Dash.NET
open System
open Plotly.NET
open System.Text.Json

///This is additional test documentation
[<RequireQualifiedAccess>]
module TestComponent =
    ///This is test documentation
    type BProp =
        { AField: bool
          BField: string
          CField: string }
        override this.ToString() =
            JsonSerializer.Serialize
                {| aField = this.AField.ToString()
                   bField = this.BField.ToString()
                   cField = this.CField.ToString() |}

    ///This is test documentation
    type CPropCase0Type =
        | Enabled
        | Disabled
        | Auto
        override this.ToString() =
            match this with
            | Enabled -> "enabled"
            | Disabled -> "disabled"
            | Auto -> "auto"

    ///This is test documentation
    type CProp =
        | CPropCase0 of CPropCase0Type
        | CPropCase1 of bool
        override this.ToString() =
            match this with
            | CPropCase0 (v) -> v.ToString()
            | CPropCase1 (v) -> v.ToString()

    ///This is test documentation
    type TestComponentProps =
        | AProp of string
        | BProp of BProp
        | CProp of CProp
        static member toDynamicMemberDef(prop: TestComponentProps) =
            match prop with
            | AProp (p) -> "aProp", box p
            | BProp (p) -> "bProp", box (p.ToString())
            | CProp (p) -> "cProp", box (p.ToString())

    ///This is additional test documentation
    type TestComponent() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?aProp: string,
                ?bProp: string,
                ?cProp: string
            ) =
            (fun (t: TestComponent) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "aProp" aProp
                DynObj.setValueOpt props "bProp" bProp
                DynObj.setValueOpt props "cProp" cProp
                DynObj.setValue t "namespace" "TestComponent"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "TestComponent"
                t)

        static member init(id: string, children: seq<DashComponent>, ?aProp: string, ?bProp: string, ?cProp: string) =
            TestComponent.applyMembers (id, children, ?aProp = aProp, ?bProp = bProp, ?cProp = cProp) (TestComponent())

        static member definition: LoadableComponentDefinition =
            { ComponentName = "TestComponent"
              ComponentJavascript = "test_component.js" }

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
