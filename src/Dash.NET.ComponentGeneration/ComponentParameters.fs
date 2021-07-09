module Dash.NET.ComponentGeneration.ComponentParameters

open Prelude

type ComponentParameters = 
    {
        ComponentName:                  string
        ComponentPropsName:             string
        CamelCaseComponentName:         string
        ComponentNamespace:             string
        ComponentType:                  string
        LibraryNamespace:               string
        ComponentJavascript:            string
        PropertyNames:                  string list
        DUSafePropertyNames:            string list
    }
    static member create (componentName: string) (componentNameSpace: string) (componentType: string) (libraryNameSpace: string) (propertyNames: string list) =
        //TODO incorrectly formats names in some cases

        {
            ComponentName                   = componentName
            ComponentPropsName              = sprintf "%sProps" componentName
            CamelCaseComponentName          = componentName |> String.decapitalize
            ComponentNamespace              = componentNameSpace
            ComponentType                   = componentType
            LibraryNamespace                = libraryNameSpace
            ComponentJavascript             = sprintf "%s.min.js" componentNameSpace //TODO not always the case
            PropertyNames                   = propertyNames
            DUSafePropertyNames             = propertyNames |> List.map String.toValidDULabel
        }