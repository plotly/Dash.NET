//------------------------------------------------------------------------------
//        This file has been automatically generated.
//        Changes to this file will be lost when the code is regenerated.
//------------------------------------------------------------------------------
namespace TestNamespace

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module TestModule =
    type TestComponentProperties =
        | TestComponentProperty1 of string
        | TestComponentProperty2 of string
        static member x.toDynamicMemberDef(prop: TestComponentProperties) =
            match prop with
            | TestComponentProperty1 (p) -> "TestComponentProperty1", box "p"
            | TestComponentProperty2 (p) -> "TestComponentProperty2", box "p"
