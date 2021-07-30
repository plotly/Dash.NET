module Dash.NET.ComponentGeneration.ComponentParameters

open System.IO
open Prelude
open FSharpPlus.Lens
open ReactMetadata


type ComponentParameters = 
    {
        ComponentName:                  string
        ComponentPropsName:             string
        CamelCaseComponentName:         string
        ComponentNamespace:             string
        ComponentType:                  string
        LibraryNamespace:               string
        ComponentJavascript:            string
        ComponentFSharp:                string
        ComponentDescription:           string option
        PropertyNames:                  string list
        DUSafePropertyNames:            string list
        PropertyTypes:                  SafeReactProp list
    }

    static member create (componentName: string) (componentJavascript: string) (componentMetadata: SafeReactComponent) =
        let pnames, pvals =
            (componentMetadata.props.Values |> List.ofSeq)
            |> List.zip (componentMetadata.props.Keys |> List.ofSeq)
            |> List.filter (fst >> (not << (=) "id")) // we handle the id property differently
            |> List.filter (fst >> (not << (=) "children")) // we handle the children property differently
            |> List.unzip

        {
            ComponentName                   = componentName
            ComponentPropsName              = sprintf "%sProps" componentName
            CamelCaseComponentName          = componentName |> String.decapitalize
            ComponentNamespace              = componentName
            ComponentType                   = componentName
            LibraryNamespace                = componentName
            ComponentJavascript             = componentJavascript
            ComponentFSharp                 = sprintf "%s.fs" componentName
            ComponentDescription            = componentMetadata.description
            PropertyNames                   = pnames
            DUSafePropertyNames             = pnames |> List.map String.toValidDULabel
            PropertyTypes                   = pvals
        }

    static member fromReactMetadata (meta: ReactMetadata) =
        (meta.Values |> List.ofSeq)
        |> List.zip (meta.Keys |> List.ofSeq)
        |> List.choose (fun (script, comp) -> 
            //TODO: allow naming the F# version differntly to the js version
            let maybeCName = view SafeReactComponent._displayName comp
            let scriptName = script |> Path.GetFileName
            match maybeCName with
            | Some cName -> ComponentParameters.create cName scriptName comp |> Some
            | None -> None
        )