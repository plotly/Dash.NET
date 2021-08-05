//------------------------------------------------------------------------------
//        This file has been automatically generated.
//        Changes to this file will be lost when the code is regenerated.
//------------------------------------------------------------------------------
namespace TestComponent

open Dash.NET
open System
open Plotly.NET
open System.Text.Json

///<summary>
///TestComponent is an example component.
///</summary>
[<RequireQualifiedAccess>]
module TestComponent =
    ///<summary>
    ///record with the fields: 'aField: boolean (optional)', 'bField: string (optional)', 'cField: string (required)'
    ///</summary>
    type BProp =
        { AField: Option<bool>
          BField: Option<string>
          CField: string }
        override this.ToString() =
            JsonSerializer.Serialize
                {| aField = Some(this.AField.ToString())
                   bField = Some(this.BField.ToString())
                   cField = this.CField.ToString() |}

    ///<summary>
    ///value equal to: 'enabled', 'disabled', 'auto'
    ///</summary>
    type CPropCase0Type =
        | Enabled
        | Disabled
        | Auto
        override this.ToString() =
            match this with
            | Enabled -> "enabled"
            | Disabled -> "disabled"
            | Auto -> "auto"

    ///<summary>
    ///value equal to: 'enabled', 'disabled', 'auto' | boolean
    ///</summary>
    type CProp =
        | CPropCase0 of CPropCase0Type
        | CPropCase1 of bool
        override this.ToString() =
            match this with
            | CPropCase0 (v) -> v.ToString()
            | CPropCase1 (v) -> v.ToString()

    ///<summary>
    ///list with values of type: string
    ///</summary>
    type DProp =
        | DProp of list<string>
        override this.ToString() =
            match this with
            | DProp (v) -> JsonSerializer.Serialize(List.map (fun (i: string) -> i.ToString()) v)

    ///<summary>
    ///boolean
    ///</summary>
    type EProp =
        | EProp of bool
        override this.ToString() =
            match this with
            | EProp (v) -> v.ToString()

    ///<summary>
    ///• id (string) - The ID used to identify this component in Dash callbacks.
    ///&#10;
    ///• aProp (string) - A property.
    ///&#10;
    ///• bProp (record with the fields: 'aField: boolean (optional)', 'bField: string (optional)', 'cField: string (required)') - A different property.
    ///&#10;
    ///• cProp (value equal to: 'enabled', 'disabled', 'auto' | boolean) - A fancy property.
    ///&#10;
    ///• dProp (list with values of type: string) - A fancy property.
    ///&#10;
    ///• eProp (boolean) - A fancy property.
    ///</summary>
    type TestComponentProps =
        | AProp of string
        | BProp of BProp
        | CProp of CProp
        | DProp of DProp
        | EProp of EProp
        static member toDynamicMemberDef(prop: TestComponentProps) =
            match prop with
            | AProp (p) -> "aProp", box p
            | BProp (p) -> "bProp", box (p.ToString())
            | CProp (p) -> "cProp", box (p.ToString())
            | DProp (p) -> "dProp", box (p.ToString())
            | EProp (p) -> "eProp", box (p.ToString())

    ///<summary>
    ///TestComponent is an example component.
    ///</summary>
    type TestComponent() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?aProp: string,
                ?bProp: string,
                ?cProp: string,
                ?dProp: string,
                ?eProp: string
            ) =
            (fun (t: TestComponent) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "aProp" aProp
                DynObj.setValueOpt props "bProp" bProp
                DynObj.setValueOpt props "cProp" cProp
                DynObj.setValueOpt props "dProp" dProp
                DynObj.setValueOpt props "eProp" eProp
                DynObj.setValue t "namespace" "TestComponent"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "TestComponent"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?aProp: string,
                ?bProp: string,
                ?cProp: string,
                ?dProp: string,
                ?eProp: string
            ) =
            TestComponent.applyMembers
                (id, children, ?aProp = aProp, ?bProp = bProp, ?cProp = cProp, ?dProp = dProp, ?eProp = eProp)
                (TestComponent())

        static member definition: LoadableComponentDefinition =
            { ComponentName = "TestComponent"
              ComponentJavascript = "test_component.js" }

    ///<summary>
    ///TestComponent is an example component.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID used to identify this component in Dash callbacks.
    ///&#10;
    ///• aProp (string) - A property.
    ///&#10;
    ///• bProp (record with the fields: 'aField: boolean (optional)', 'bField: string (optional)', 'cField: string (required)') - A different property.
    ///&#10;
    ///• cProp (value equal to: 'enabled', 'disabled', 'auto' | boolean) - A fancy property.
    ///&#10;
    ///• dProp (list with values of type: string) - A fancy property.
    ///&#10;
    ///• eProp (boolean) - A fancy property.
    ///</summary>
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
