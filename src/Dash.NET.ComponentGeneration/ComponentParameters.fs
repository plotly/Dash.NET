module Dash.NET.ComponentGeneration.ComponentParameters

open System.IO
open Prelude
open ReactMetadata


type ComponentParameters = 
    {
        ComponentName:                  string
        ComponentPropsName:             string
        CamelCaseComponentName:         string
        ComponentNamespace:             string
        ComponentType:                  string
        LibraryNamespace:               string
        ComponentJavascript:            string list
        ComponentFSharp:                string
        ComponentDescription:           string option
        PropertyNames:                  string list
        DUSafePropertyNames:            string list
        PropertyTypes:                  SafeReactProp list
        Metadata:                       SafeReactComponent
    }

    static member create (componentName: string) (componentNamespace: string) (componentJavascript: string list) (componentMetadata: SafeReactComponent) =
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
            ComponentNamespace              = componentNamespace
            ComponentType                   = componentName
            LibraryNamespace                = componentName
            ComponentJavascript             = componentJavascript
            ComponentFSharp                 = sprintf "%s.fs" componentName
            ComponentDescription            = componentMetadata.description
            PropertyNames                   = pnames
            DUSafePropertyNames             = pnames |> List.map String.toValidDULabel
            PropertyTypes                   = pvals
            Metadata                        = componentMetadata
        }

    static member fromReactMetadata (componentShortName: string) (javascriptFiles: string list) (meta: ReactMetadata) =
        (meta.Values |> List.ofSeq)
        |> List.zip (meta.Keys |> List.ofSeq)
        |> List.choose (fun (_, comp) ->
            //TODO: allow naming the F# version differntly to the js version
            let maybeCName = comp.displayName
            match maybeCName with
            | Some cName -> ComponentParameters.create cName componentShortName javascriptFiles comp |> Some
            | None -> None
        )