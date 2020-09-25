namespace Dash.NET.HTML_DSL

open Dash.NET
open System
open Plotly.NET
open HTMLPropTypes

[<AutoOpen>]
module RawString =

    type Str() =
        inherit DashComponent()
        static member init 
            (
                innerHTML:string
            ) = 
                let d = DashComponent(true)
                DynObj.setValue d "innerHTML" innerHTML
                d

    let str (innerHTML:string) =
        Str.init(innerHTML)