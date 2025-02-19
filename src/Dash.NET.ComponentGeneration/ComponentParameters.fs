module Dash.NET.ComponentGeneration.ComponentParameters

open System
open Humanizer
open Prelude
open ReactMetadata

let rec private findDuplicatesBy comp cs = function
    | [] -> cs
    | n::ts ->
        ts
        |> findDuplicatesBy comp (
            if cs |> comp n || ts |> comp n |> not then cs
            else n::cs
        )

type ComponentParametersProperty =
    {
        Name: string
        CaseName: string
        TypeName: string
        Info: SafeReactProp
    }

    static member create (propName: string) (prop: SafeReactProp) =
        {
            Name = propName
            CaseName = propName |> String.toValidDULabel
            TypeName =
                propName
                |> fun n ->
                    if n.ToLower().EndsWith("focus") then n
                    else n.Singularize()
                |> fun n -> n.Pascalize()
            Info = prop
        }

    static member structureEquals (x: ComponentParametersProperty) (y: ComponentParametersProperty) =
        match x.Info.propType, y.Info.propType with
        | Some xpt, Some ypt -> SafeReactPropType.valueEquals xpt ypt
        | None, None -> true
        | _ -> false

    static member typeNameArgs paramProp propType =
        SafeReactPropType.tryGetFSharpTypeName propType
        |> Option.map (function
            | [ "seq" as t; "obj" ] -> [ t; paramProp.TypeName ]
            | names -> names
        )
        |> Option.defaultValue [ paramProp.TypeName ]

    static member tryTypeNameArgs paramProp =
        paramProp.Info.propType
        |> Option.map (ComponentParametersProperty.typeNameArgs paramProp)

    static member valueConvArgs paramProp =
        paramProp.Info.propType
        |> Option.bind (fun propType ->
            if propType |> SafeReactPropType.needsConvert then Some [ paramProp.TypeName; "convert" ]
            else None
        )
        |> Option.defaultValue [ "box" ]

    static member mkSetFunctionName paramProp = paramProp.Name.Camelize()

    static member mkArgName paramProp = paramProp.Name.Camelize()


    //override this.Equals other =
    //    match other with
    //    | :? ComponentParametersProperty as otherProp ->
    //        match this.Info.propType, otherProp.Info.propType with
    //        | Some xpt, Some ypt -> SafeReactPropType.equalsValue xpt ypt
    //        | None, None -> true
    //        | _ -> false
    //    | _ -> false

    //interface IComparable with
    //    member this.CompareTo other =
    //        match other with
    //        | :? ComponentParametersProperty as otherProp ->
    //            let ll =
    //                match this.Info.propType, otherProp.Info.propType with
    //                | Some xpt, Some ypt -> SafeReactPropType.equalsValue xpt ypt
    //                | None, None -> true
    //                | _ -> false
    //            -1
    //        | _ -> -1



    //interface IComparable<ComponentParametersProperty> with
    //    member this.CompareTo other = 0

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
        ComponentDescription:           string option
        Properties:                     ComponentParametersProperty list
        Metadata:                       SafeReactComponent
        IsHelper:                       bool
    }

    static member create (componentName: string) (componentNamespace: string) (libraryNamespace: string) (componentJavascript: string list) (componentMetadata: SafeReactComponent) =
        {
            ComponentName           = componentName
            ComponentPropsName      = "Prop"
            ComponentAttrsName      = "Attr"
            CamelCaseComponentName  = componentName |> String.decapitalize
            ComponentNamespace      = componentNamespace
            ComponentType           = componentName
            LibraryNamespace        = libraryNamespace
            ComponentJavascript     = componentJavascript
            ComponentFSharp         = sprintf "%s.fs" componentName
            ComponentDescription    = componentMetadata.description
            Properties              = 
                componentMetadata.props
                |> Map.toList
                |> List.choose (fun (propName, prop) ->
                    // we handle the id property differently
                    // we handle the children property differently
                    if propName = "id" || propName = "children" then None
                    else prop |> ComponentParametersProperty.create propName |> Some
                )
                |> List.distinct
            Metadata                = componentMetadata
            IsHelper                = false
        }

    static member mkCommon componentNamespace libraryNamespace (comps: ComponentParameters list) =
        let existingProp (x: ComponentParametersProperty) =
            List.exists (fun (y: ComponentParametersProperty) ->
                //(x.Name.Contains(y.Name) || y.Name.Contains(x.Name)) && ComponentParametersProperty.structureEquals x y
                let xName = x.Name.ToLowerInvariant()
                let yName = y.Name.ToLowerInvariant()
                let xisy = xName.Contains(yName)
                let yisx = yName.Contains(xName)
                let e = ComponentParametersProperty.structureEquals x y
                let s = (xisy || yisx) && e
                s

            )

        let commonComp =
            let componentName = "Common"
            { ComponentParameters.create
                    componentName
                    componentNamespace
                    libraryNamespace
                    []
                    { description = Some "Common components"; displayName = None; props = Map.empty }
                    with
                        Properties = comps |> List.collect (fun comp -> comp.Properties) |> findDuplicatesBy existingProp []
                        IsHelper = true
            }

        commonComp
        |> List.singleton
        |> List.append (
            comps
            |> List.filter (fun comp -> comp.ComponentName = "Checklist")
            |> List.map (fun comp ->
                let ps = 
                    comp.Properties
                let p =
                    comp.Properties
                    |> List.filter (fun prop -> not (commonComp.Properties |> List.exists (fun cp -> cp.Name = prop.Name)))
                    |> List.distinct
                { comp
                    with
                        Properties =
                            comp.Properties
                            |> List.filter (fun prop -> not (commonComp.Properties |> List.exists (fun cp -> cp.Name = prop.Name)))
                            |> List.distinct
                }
            )
        )

    static member parse (componentShortName: string) libraryNamespace (javascriptFiles: string list) (meta: Map<string, SafeReactComponent>) =
        meta
        |> Map.toList
        |> List.choose (
            snd
            >> fun comp ->
                comp.displayName
                |> Option.map (fun cName ->
                    ComponentParameters.create cName componentShortName libraryNamespace javascriptFiles comp
                )
        )

    static member fromReactMetadata componentShortName libraryNamespace javascriptFiles =
        ComponentParameters.parse componentShortName libraryNamespace javascriptFiles
        // TODO: Extract common elements
        //>> ComponentParameters.mkCommon componentShortName libraryNamespace


