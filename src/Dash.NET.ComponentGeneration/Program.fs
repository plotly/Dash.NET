open System
open System.IO
open Dash.NET.ComponentGeneration

let componentTemplate = File.ReadAllText (__SOURCE_DIRECTORY__ + "/Component.template.fs")

module String =
    let replace (old:string) (_new:string) (s:string) = s.Replace(old,_new)
    let write path (s:string) = File.WriteAllText(path,s)

type ComponentParameters = 
    {
        ComponentName:             string
        CamelCaseComponentName:    string
        ComponentChar:             string
        ComponentNamespace:        string
        ComponentType:             string
        LibraryNamespace:          string
    }
    static member create  (componentName:string) (componentNameSpace:string) (componentType:string) (libraryNameSpace:string) =
        {
            ComponentName               = preventKWUsage componentName
            CamelCaseComponentName      = preventKWUsage (sprintf "%c%s" (Char.ToLowerInvariant(componentName.[0])) (componentName.Substring(1)))
            ComponentChar               = preventKWUsage (componentName.ToLower().Substring(0,1))
            ComponentNamespace          = preventKWUsage componentNameSpace
            ComponentType               = preventKWUsage componentType
            LibraryNamespace            = preventKWUsage libraryNameSpace
        }


let generateComponentTemplateFile path (parameters:ComponentParameters) =
    componentTemplate
    |> String.replace "(*{ComponentName}*)"             parameters.ComponentName
    |> String.replace "(*{camelCaseComponentName}*)"    parameters.CamelCaseComponentName
    |> String.replace "(*{ComponentChar}*)"             parameters.ComponentChar
    |> String.replace "(*{ComponentNamespace}*)"        parameters.ComponentNamespace
    |> String.replace "(*{ComponentType}*)"             parameters.ComponentType
    |> String.replace "(*{LibraryNamespace}*)"          parameters.LibraryNamespace
    |> String.write path 

[<EntryPoint>]
let main argv =
    printfn "This should generate a component"
    0