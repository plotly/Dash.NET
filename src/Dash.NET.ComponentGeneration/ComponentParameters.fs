module Dash.NET.ComponentGeneration.ComponentParameters

//open System
//open System.Collections.Generic
//open System.IO
//open Serilog
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

//type ClassParameters = 
//    {
//        Name: string
//        Namespace: string
//        Filename: string
//        Description: string option
//        ReferenceNames: string list
//    }

//    static member create (name: string, ``namespace``: string, ?filename: string, ?description: string, ?referenceNames: string list) =
//        {
//            Name = name
//            Namespace = ``namespace``
//            Filename = filename |> function Some fn -> fn | None -> sprintf "%s.fs" name
//            Description = description |> function Some desc -> desc | None -> ""
//            ReferenceNames = referenceNames |> function Some rns -> rns | None -> List.empty
//        }

type ComponentParametersProperty =
    {
        Name:       string
        DuSafeName: string
        TypeName:   string
        Info:       SafeReactProp
    }

    static member create (propName: string) (prop: SafeReactProp) =
        {
            Name       = propName
            DuSafeName = propName |> String.toValidDULabel
            //TypeName   = propName |> String.toPascalCase |> sprintf "%sType"
            // MC
            TypeName   = propName.Singularize().Pascalize()
            Info       = prop
        }

    static member structureEquals (x: ComponentParametersProperty) (y: ComponentParametersProperty) =
        match x.Info.propType, y.Info.propType with
        | Some xpt, Some ypt -> SafeReactPropType.equalsValue xpt ypt
        | None, None -> true
        | _ -> false

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
        Properties:                     Map<string, ComponentParametersProperty>
        Metadata:                       SafeReactComponent
        IsHelper:                       bool
    }

    static member create (componentName: string) (componentNamespace: string) (libraryNamespace: string) (componentJavascript: string list) (componentMetadata: SafeReactComponent) =
        let props =
            componentMetadata.props
            // we handle the id property differently
            // we handle the children property differently
            |> Map.filter (fun propName _ -> propName <> "id" && propName <> "children")
            |> Map.map (fun propName -> ComponentParametersProperty.create propName)
        {
            ComponentName           = componentName
            ComponentPropsName      = "Prop"//sprintf "%sProp" componentName
            ComponentAttrsName      = "Attr"//sprintf "%sAttr" componentName
            CamelCaseComponentName  = componentName |> String.decapitalize
            ComponentNamespace      = componentNamespace
            ComponentType           = componentName
            LibraryNamespace        = libraryNamespace
            ComponentJavascript     = componentJavascript
            ComponentFSharp         = sprintf "%s.fs" componentName
            ComponentDescription    = componentMetadata.description
            Properties              = 
                componentMetadata.props
                // we handle the id property differently
                // we handle the children property differently
                |> Map.filter (fun propName _ -> propName <> "id" && propName <> "children")
                |> Map.map (fun propName -> ComponentParametersProperty.create propName)
            Metadata                = componentMetadata
            IsHelper                = false
        }

//type ComponentParameters = 
//    {
//        ComponentName:                  string
//        ComponentPropsName:             string
//        ComponentAttrsName:             string
//        CamelCaseComponentName:         string
//        ComponentNamespace:             string
//        ComponentType:                  string
//        LibraryNamespace:               string
//        ComponentJavascript:            string list
//        ComponentFSharp:                string
//        ComponentDescription:           string option
//        PropertyNames:                  string list
//        DUSafePropertyNames:            string list
//        PropertyTypeNames:              string list
//        PropertyTypes:                  SafeReactProp list
//        Metadata:                       SafeReactComponent
//    }

//    static member create (componentName: string) (componentNamespace: string) (componentJavascript: string list) (componentMetadata: SafeReactComponent) =
//        let pnames, pvals =
//            //(componentMetadata.props.Values |> List.ofSeq)
//            //|> List.zip (componentMetadata.props.Keys |> List.ofSeq)
//            componentMetadata.props
//            |> Map.toList
//            |> List.filter (fst >> (not << (=) "id")) // we handle the id property differently
//            |> List.filter (fst >> (not << (=) "children")) // we handle the children property differently
//            |> List.unzip
//        {
//            ComponentName                   = componentName
//            ComponentPropsName              = "Prop"//sprintf "%sProp" componentName
//            ComponentAttrsName              = "Attr"//sprintf "%sAttr" componentName
//            CamelCaseComponentName          = componentName |> String.decapitalize
//            ComponentNamespace              = componentNamespace
//            ComponentType                   = componentName
//            LibraryNamespace                = componentName
//            ComponentJavascript             = componentJavascript
//            ComponentFSharp                 = sprintf "%s.fs" componentName
//            ComponentDescription            = componentMetadata.description
//            PropertyNames                   = pnames
//            DUSafePropertyNames             = pnames |> List.map String.toValidDULabel
//            //PropertyTypeNames               = pnames |> List.map String.toPascalCase |> List.map (sprintf "%sType")
//            // MC
//            PropertyTypeNames               = pnames |> List.map (fun (pname:string) -> pname.Singularize().Pascalize())
//            PropertyTypes                   = pvals
//            Metadata                        = componentMetadata
//        }

    static member mkCommon componentNamespace libraryNamespace (comps: ComponentParameters list) =
        let existingProp (yPropName: string, prop) (xs: (string * ComponentParametersProperty) list)  =
            //Map.exists (snd >> ComponentParametersProperty.structureEquals prop)
            //xs
            //|> Map.exists (fun k v ->
            //    let areEqual = ComponentParametersProperty.structureEquals prop v
            //    areEqual
            //)
            xs
            //|> List.exists (snd >> ComponentParametersProperty.structureEquals prop)
            |> List.exists (fun (xPropName, x) -> (yPropName.Contains(xPropName) || xPropName.Contains(yPropName)) && ComponentParametersProperty.structureEquals prop x)


        //let equals (_, _, _, x) =
        //    List.exists (fun (_, _, _, prop) ->
        //        match prop.propType, x.propType with
        //        | Some ppt, Some xpt -> SafeReactPropType.equalsValue ppt xpt
        //        | None, None -> true
        //        | _ -> false
        //    )
        let commonComp =
            let componentName = "ComponentBase"
            //let commonPropertyNames, commonDuSafePropertyNames, commonPropertyTypeNames, commonPropertyTypes =
                //comps
                //|> List.collect (fun comp ->
                //    List.zip4 comp.PropertyNames comp.DUSafePropertyNames comp.PropertyTypeNames comp.PropertyTypes
                //)
                //|> findDuplicatesBy equals []
                //|> List.unzip4


            let commonProperties =
                comps
                |> List.collect (fun comp -> comp.Properties |> Map.toList)
                |> findDuplicatesBy existingProp []
                |> Map.ofList

            {
                ComponentName          = componentName
                ComponentPropsName     = ""
                ComponentAttrsName     = ""
                CamelCaseComponentName = componentName.Camelize()
                ComponentNamespace     = componentNamespace
                ComponentType          = componentName
                LibraryNamespace       = libraryNamespace
                ComponentJavascript    = List.empty
                ComponentFSharp        = sprintf "%s.fs" componentName
                ComponentDescription   = Some "Common components"
                //PropertyNames          = commonPropertyNames
                //DUSafePropertyNames    = commonDuSafePropertyNames |> List.map String.toValidDULabel
                //PropertyTypeNames      = commonPropertyTypeNames |> List.map (fun (pname:string) -> pname.Singularize().Pascalize())
                //PropertyTypes          = commonPropertyTypes
                Properties             = commonProperties
                Metadata               = { description = None; displayName = None; props = Map.empty }
                IsHelper               = true
            }

        commonComp
        |> List.singleton
        |> List.append (
            comps
            |> List.map (fun comp ->
                //////////////////////////////////
                // TODO - working, restore this
                //{ comp
                //    with
                //        Properties =
                //            comp.Properties
                //            |> Map.filter (fun propName _ ->
                //                let isInCommon = commonComp.Properties |> Map.containsKey propName
                //                not isInCommon
                //            )
                //}
                comp

                //////////////////////////////////

                //let propertyNames = comp.PropertyNames |> List.filter (fun pn -> commonComp.PropertyNames |> List.contains pn |> not)
                //let dUSafePropertyNames = comp.DUSafePropertyNames |> List.filter (fun dn -> commonComp.DUSafePropertyNames |> List.contains dn |> not)
                //let propertyTypeNames = comp.PropertyTypeNames |> List.filter (fun tn -> commonComp.PropertyTypeNames |> List.contains tn |> not)
                //let propertyTypes = comp.PropertyTypes |> List.filter (fun pt -> commonComp.PropertyTypes |> List.contains pt |> not)
                //{ comp
                //    with
                //        PropertyNames = comp.PropertyNames |> List.filter (fun pn -> commonComp.PropertyNames |> List.contains pn |> not)
                //        DUSafePropertyNames = comp.DUSafePropertyNames |> List.filter (fun dn -> commonComp.DUSafePropertyNames |> List.contains dn |> not)
                //        PropertyTypeNames = comp.PropertyTypeNames |> List.filter (fun tn -> commonComp.PropertyTypeNames |> List.contains tn |> not)
                //        PropertyTypes = comp.PropertyTypes |> List.filter (fun pt -> commonComp.PropertyTypes |> List.contains pt |> not)
                //}
            )
        )

    static member parse (componentShortName: string) libraryNamespace (javascriptFiles: string list) (meta: Map<string, SafeReactComponent>) =
        //(meta.Values |> List.ofSeq)
        //|> List.zip (meta.Keys |> List.ofSeq)
        //|> List.choose (fun (_, comp) ->
        //    let maybeCName = comp.displayName
        //    match maybeCName with
        //    | Some cName -> ComponentParameters.create cName componentShortName javascriptFiles comp |> Some
        //    | None -> None
        //)

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
        >> ComponentParameters.mkCommon componentShortName libraryNamespace


