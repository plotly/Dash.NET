module Dash.NET.ComponentGeneration.ComponentParameters

open System.IO
open Prelude
open ReactMetadata
open Serilog


type ComponentParameters = 
    {
        ComponentName:                  string
        ComponentPropsName:             string
        ComponentAttrsName:             string
        CamelCaseComponentName:         string
        ComponentNamespace:             string
        ComponentType:                  string
        LibraryNamespace:               string
        ComponentJavascript:            string list
        ComponentFSharp:                string
        ComponentCSharp:                string
        ComponentDescription:           string option
        PropertyNames:                  string list
        DUSafePropertyNames:            string list
        PropertyTypeNames:              string list
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
            ComponentPropsName              = "Prop"//sprintf "%sProp" componentName
            ComponentAttrsName              = "Attr"//sprintf "%sAttr" componentName
            CamelCaseComponentName          = componentName |> String.decapitalize
            ComponentNamespace              = componentNamespace
            ComponentType                   = componentName
            LibraryNamespace                = componentName
            ComponentJavascript             = componentJavascript
            ComponentFSharp                 = sprintf "%s.fs" componentName
            ComponentCSharp                 = sprintf "%s.csharp.fs" componentName
            ComponentDescription            = componentMetadata.description
            PropertyNames                   = pnames
            DUSafePropertyNames             = pnames |> List.map String.toValidDULabel
            PropertyTypeNames               = pnames |> List.map String.toPascalCase |> List.map (sprintf "%sType")
            PropertyTypes                   = pvals
            Metadata                        = componentMetadata
        }

    static member fromReactMetadata (log: Core.Logger) (componentShortName: string) (javascriptFiles: string list) (meta: ReactMetadata) =
        (meta.Values |> List.ofSeq)
        |> List.zip (meta.Keys |> List.ofSeq)
        |> List.choose (fun (_, comp) ->
            let maybeCName = comp.displayName
            match maybeCName with
            | Some cName -> ComponentParameters.create cName componentShortName javascriptFiles comp |> Some
            | None -> None
        )