//---
//ComponentName: {ComponentName}
//camelCaseComponentName: {camelCaseComponentName}
//ComponentChar: {ComponentChar}
//ComponentNamespace: {ComponentNamespace}
//ComponentType: {ComponentType}
//LibraryNamespace: {LibraryNamespace}
//---

namespace ``{LibraryNamespace}``

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module ``{ComponentName}`` =

    type ``{ComponentName}Props`` =
        {ComponentPropDefinitions}
        static member toDynamicMemberDef (prop:``{ComponentName}Props``) =
            match prop with
            {ComponentPropDynamicMemberMap}
            
    type ``{ComponentName}``() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                {ComponentPropArguments}
            ) =
            (
                fun (``{ComponentChar}``:``{ComponentName}``) -> 

                    let props = DashComponentProps()

                    id |> DynObj.setValue props "id"
                    children |> DynObj.setValue props "children"

                    {ComponentPropSetValueMap}

                    DynObj.setValue ``{ComponentChar}`` "namespace" @"{ComponentNamespace}"
                    DynObj.setValue ``{ComponentChar}`` "props" props
                    DynObj.setValue ``{ComponentChar}`` "type" @"{ComponentType}"

                    ``{ComponentChar}``

            )
        static member init 
            (
                id,
                children,
                {ComponentPropArguments}
            ) = 
                ``{ComponentName}``()
                |> ``{ComponentName}``.applyMembers 
                    (
                        id,
                        children,
                        {ComponentPropArgumentsMap}
                    )

    let ``{camelCaseComponentName}`` (id:string) (props:seq<``{ComponentName}Props``>) (children:seq<DashComponent>) =
        let ``{ComponentChar}`` = ``{ComponentName}``.init(id,children)
        let componentProps = 
            match (``{ComponentChar}``.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> ``{ComponentName}Props``.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue ``{ComponentChar}`` "props" 
        ``{ComponentChar}`` :> DashComponent

