module Dash.NET.ComponentGeneration.DocumentationGeneration

open ReactMetadata
open Prelude

let getIsRequired (req: bool) =
    if req then
        "required"
    else
        "optional"

let getTypeIsRequired (ptype: SafeReactPropType) =
    (ptype |> SafeReactPropType.getProps).required
    |> Option.defaultValue false 
    |> getIsRequired

let getTypeDescription (ptype: SafeReactPropType) =
    (ptype |> SafeReactPropType.getProps).description

let getTypeHasDefault (dval: SafeReactPropType) =
    match dval with
    | Any (_, Some v) -> sprintf "default %s" v |> Some
    | _ -> None

//TODO formating for complex types?
let rec getTypePropDocumentation (ptype: SafeReactPropType) =
    match ptype with 
    | Array _ -> "list"
    | Bool _ -> "boolean"
    | Number _ -> "number"
    | String _ -> "string"
    | Object _ -> "record"
    | Any _ -> "boolean | number | string | record | list"
    | Element _ -> "dash component"
    | Node _ -> "a list of or a singular dash component, string or number"

    // Special cases, each type will have a unique name
    | Enum (_, Some values) -> 
        let valueList =
            values
            |> List.choose (fun case -> 
                match case with 
                | Any (_, Some value) -> value |> Some
                | _ -> None)
        
        if valueList.Length > 1 then
            valueList
            |> List.map (sprintf "'%s'")
            |> List.reduce (sprintf "%s, %s")
            |> sprintf "value equal to: %s"
        else if valueList.Length = 1 then
            sprintf "value equal to: '%s'" valueList.[0]
        else
            sprintf "value equal to: unknown"

    | Union (_, Some values) ->
        let valueList =
            values
            |> List.map getTypePropDocumentation
        
        if valueList.Length > 1 then
            valueList
            |> List.reduce (sprintf "%s | %s")
        else if valueList.Length = 1 then
            valueList.[0]
        else
            sprintf "unknown"

    | ArrayOf (_, Some value) -> 
        value
        |> getTypePropDocumentation
        |> sprintf "list with values of type: %s"

    | ObjectOf (_, Some value) -> 
        getTypePropDocumentation value

    | Shape (_, Some dict)
    | Exact (_, Some dict) -> 
        let valueList =
            (dict.Keys |> List.ofSeq, dict.Values |> List.ofSeq)
            ||> List.zip
            |> List.map (fun (key, value) ->
                let optionalDoc = 
                    value 
                    |> getTypeHasDefault
                    |> Option.defaultValue (value |> getTypeIsRequired)
                sprintf "%s: %s (%s)" key (getTypePropDocumentation value) optionalDoc) 

        if valueList.Length > 1 then
            valueList
            |> List.map (sprintf "'%s'")
            |> List.reduce (sprintf "%s, %s")
            |> sprintf "record with the fields: %s"
        else if valueList.Length = 1 then
            sprintf "record with the field: '%s'" valueList.[0]
        else
            sprintf "record with the fields: unknown"

    // A type we can't process
    | Other _
    | _ -> "unknown"

let generateComponentPropDocumentation (pname: string) (prop: SafeReactProp) =
    let optionalDoc = 
        (prop.required, prop.defaultValue) 
        ||> Option.map2 (fun preq pdef -> getTypeHasDefault pdef |> Option.defaultValue (getIsRequired preq))
        |> Option.map (sprintf "; %s")
        |> Option.defaultValue ""

    let typeDoc = 
        prop.propType 
        |> Option.map getTypePropDocumentation
        |> Option.defaultValue ""

    let descriptionDoc = 
        prop.description
        |> Option.map (sprintf " - %s")
        |> Option.defaultValue ""

    sprintf "• %s (%s%s)%s" pname typeDoc optionalDoc descriptionDoc

let generateComponentPropsDocumentation (comp: SafeReactComponent) = 
    (comp.props.Keys |> List.ofSeq, comp.props.Values |> List.ofSeq)
    ||> List.map2 generateComponentPropDocumentation

let generateComponentDescription (comp: SafeReactComponent) = 
    [ comp.description |> Option.defaultValue "" ]

let generateComponentDocumentation (comp: SafeReactComponent) = 
    let componentDescription = comp |> generateComponentDescription
    let componentParameterDocs = comp |> generateComponentPropsDocumentation

    [ yield! componentDescription; yield "Properties:"; yield! componentParameterDocs ]

let generatePropDocumentation (prop: SafeReactPropType) =
    let typeDoc = 
        prop
        |> getTypePropDocumentation

    let descriptionDoc = 
        prop
        |> getTypeDescription
        |> Option.defaultValue ""

    [ yield typeDoc; yield descriptionDoc ]

let toXMLDoc (inner: string list) =
    let modInner =
        inner
        |> List.filter (String.length >> (<) 0)
        |> List.indexed
        |> List.fold (fun acc (i, s) -> if i = 0 then s::acc else s::"&#10;"::acc) []
        |> List.rev
    [yield "<summary>"; yield! modInner; yield "</summary>"]
    |> List.map (String.replace "\r" "")
    |> List.map (String.split "\n")
    |> List.concat
    |> List.map String.escape
