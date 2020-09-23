# Dash component DSL design notes

### Common for all dash components:

Backbone with placeholders:
 - `{ComponentName}` : Name of the components
 - `{camelCaseComponentName}` : camelCase Name of the components
 - `{ComponentChar}` : lowercase first character of the component name
 - `{ComponentNamespace}` : namespace of the dash component (e.g. `dash_core_components`)
 - `{ComponentType}` : type of the component (e.g. `Input`)

```F#
//---
//ComponentName: {ComponentName}
//camelCaseComponentName: {camelCaseComponentName}
//ComponentChar: {ComponentChar}
//ComponentNamespace: {ComponentNamespace}
//ComponentType: {ComponentType}
//---

namespace Dash.NET.DCC_DSL

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module {ComponentName} =

    type {ComponentName}Props =
        | ClassName of string
        static member toDynamicMemberDef (prop:{ComponentName}Props) =
            match prop with
            | ClassName p           -> "className", box p
            
    type {ComponentName}() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?ClassName           : string
            ) =
            (
                fun ({ComponentChar}:{ComponentName}) -> 

                    let props = DashComponentProps()

                    id                  |> DynObj.setValue props "id"
                    children            |> DynObj.setValue props "children"
                    ClassName           |> DynObj.setValueOpt props "className"
                    
                    DynObj.setValue {ComponentChar} "namespace" "{ComponentNamespace}"
                    DynObj.setValue {ComponentChar} "props" props
                    DynObj.setValue {ComponentChar} "type" "{ComponentType}"

                    {ComponentChar}

            )
        static member init 
            (
                id,
                children,
                ?ClassName
            ) = 
                {ComponentName}()
                |> {ComponentName}.applyMembers 
                    (
                        id,
                        children,
                        ?ClassName = ClassName
                    )

    let {camelCaseComponentName} (id:string) (props:seq<{ComponentName}Props>) (children:seq<DashComponent>) =
        let {ComponentChar} = {ComponentName}.init(id,children)
        let componentProps = 
            match ({ComponentChar}.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> {ComponentName}Props.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue {ComponentChar} "props" 
        {ComponentChar} :> DashComponent
```
    