module Dash.NET.ComponentGeneration.ComponentString

open System
open System.IO
open System.Text.RegularExpressions

let componentTemplate = File.ReadAllText (__SOURCE_DIRECTORY__ + "/Component.template.fs")

let validDULabel = Regex "^[ABCDEFGHIJKLMNOPQRSTUVWXYZ].*"

module String =
    let replace (old: string) (_new: string) (s: string) = s.Replace(old,_new)
    let write path (s: string) = File.WriteAllText(path,s)

    let firstLetter (s: string) = s.ToLower().Substring(0,1)
    let decapitalize (s: string) = (sprintf "%c%s" (Char.ToLowerInvariant(s.[0])) (s.Substring(1)))
    let capitalize (s: string) = (sprintf "%c%s" (Char.ToUpperInvariant(s.[0])) (s.Substring(1)))

    let escape (s: string) = s.Replace("\\","\\\\").Replace("\"","\\\"")

    //DU labels have to start with a capital, if a property/value name starts with an _ or other non-letter character
    //then we have to add a letter in front of it
    //TODO There are other rules to DU names that don't apply to normal variable names (like no control characters like " and \)
    let toValidDULabel (s: string) = 
        let capitalizedLabel = s |> capitalize
        if validDULabel.IsMatch(capitalizedLabel) then capitalizedLabel
        else sprintf "P%s" s

let generateComponentPropDefinitions = 
    Array.fold 
        ( fun definitions (propertyName: string) -> 
            definitions 
            + sprintf "| ``%s`` of string " //TODO property types
                (propertyName |> String.toValidDULabel) ) 
        ""

let generateComponentPropDynamicMemberMap = 
    Array.fold 
        ( fun definitions (propertyName: string) -> 
            definitions
            + sprintf "| ``%s`` p -> \"%s\", box p " 
                (propertyName |> String.toValidDULabel)
                (propertyName |> String.escape) )
        ""

let generateComponentPropSetValueMap = 
    Array.fold 
        ( fun definitions (propertyName: string) -> 
            definitions 
            + sprintf "``%s`` |> DynObj.setValueOpt props \"%s\"; "
                (propertyName |> String.capitalize)
                (propertyName |> String.escape) )
        ""

let generateComponentPropArguments (propertyNames: string []) = 
    Array.fold 
        ( fun (definitions, count) (propertyName: string) -> 
            let appendedDefinitions =
                definitions 
                + sprintf "?``%s``: string%s " //TODO property types
                    (propertyName |> String.capitalize) 
                    (if count < (propertyNames.Length - 1) then "," else "")
            (appendedDefinitions, count + 1) ) 
        ("", 0)
        propertyNames
    |> fst

let generateComponentPropArgumentsMap (propertyNames: string []) = 
    Array.fold 
        ( fun (definitions, count) (propertyName: string) -> 
            let capitalizedPropertyName = propertyName |> String.capitalize
            let appendedDefinitions =
                definitions 
                + sprintf "?``%s`` = ``%s``%s "
                    capitalizedPropertyName
                    capitalizedPropertyName
                    (if count < (propertyNames.Length - 1) then "," else "")
            (appendedDefinitions, count + 1) ) 
        ("", 0)
        propertyNames
    |> fst

type ComponentParameters = 
    {
        ComponentName:                  string
        CamelCaseComponentName:         string
        ComponentChar:                  string
        ComponentNamespace:             string
        ComponentType:                  string
        LibraryNamespace:               string

        ComponentPropDefinitions:       string
        ComponentPropDynamicMemberMap:  string
        ComponentPropSetValueMap:       string
        ComponentPropArguments:         string
        ComponentPropArgumentsMap:      string
    }
    static member create (componentName: string) (componentNameSpace: string) (componentType: string) (libraryNameSpace: string) (propertyNames: string []) =
        //TODO preventKWUsage incorrectly formats names in some cases
        //and may not be nessecary at all if we are using `` to escape names

        //let capitalizedPropertyNames = propertyNames |> Array.map String.capitalize

        {
            ComponentName                   = componentName
            CamelCaseComponentName          = componentName |> String.decapitalize
            ComponentChar                   = componentName |> String.firstLetter
            ComponentNamespace              = componentNameSpace
            ComponentType                   = componentType
            LibraryNamespace                = libraryNameSpace

            //TODO remove tab count and do definitions inline
            //tabbing looks nicer but is fragile
            ComponentPropDefinitions        = generateComponentPropDefinitions propertyNames
            ComponentPropDynamicMemberMap   = generateComponentPropDynamicMemberMap propertyNames
            ComponentPropSetValueMap        = generateComponentPropSetValueMap propertyNames
            ComponentPropArguments          = generateComponentPropArguments propertyNames
            ComponentPropArgumentsMap       = generateComponentPropArgumentsMap propertyNames
        }


let generateComponentTemplateFile path (parameters:ComponentParameters) =
    componentTemplate
    |> String.replace "{ComponentName}"                     parameters.ComponentName
    |> String.replace "{camelCaseComponentName}"            parameters.CamelCaseComponentName
    |> String.replace "{ComponentChar}"                     parameters.ComponentChar
    |> String.replace "{ComponentNamespace}"                parameters.ComponentNamespace
    |> String.replace "{ComponentType}"                     parameters.ComponentType
    |> String.replace "{LibraryNamespace}"                  parameters.LibraryNamespace
    |> String.replace "{ComponentPropDefinitions}"          parameters.ComponentPropDefinitions
    |> String.replace "{ComponentPropDynamicMemberMap}"     parameters.ComponentPropDynamicMemberMap
    |> String.replace "{ComponentPropSetValueMap}"          parameters.ComponentPropSetValueMap
    |> String.replace "{ComponentPropArguments}"            parameters.ComponentPropArguments
    |> String.replace "{ComponentPropArgumentsMap}"         parameters.ComponentPropArgumentsMap
    |> String.write path 