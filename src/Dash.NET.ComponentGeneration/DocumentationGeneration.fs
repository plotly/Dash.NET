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

//TODO nicer formatting for complex types
let rec getTypePropDocumentation (ptype: SafeReactPropType) =
    match ptype with 
    | Array _ -> "list" |> Some
    | Bool _ -> "boolean" |> Some
    | Number _ -> "number" |> Some
    | String _ -> "string" |> Some
    | Object _ -> "record" |> Some
    | Any _ -> "boolean | number | string | record | list" |> Some
    | Element _ -> "dash component" |> Some
    | Node _ -> "a list of or a singular dash component, string or number" |> Some

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
            |> Some
        else if valueList.Length = 1 then
            sprintf "value equal to: '%s'" valueList.[0]
            |> Some
        else
            sprintf "value equal to: unknown"
            |> Some

    | Union (_, Some values) ->
        let valueList =
            values
            |> List.choose getTypePropDocumentation
        
        if valueList.Length > 1 then
            valueList
            |> List.reduce (sprintf "%s | %s")
            |> Some
        else if valueList.Length = 1 then
            valueList.[0]
            |> Some
        else
            sprintf "unknown"
            |> Some

    | ArrayOf (_, Some value) -> 
        value
        |> getTypePropDocumentation
        |> Option.map (sprintf "list with values of type: %s")

    | ObjectOf (_, Some value) -> 
        getTypePropDocumentation value

    | Shape (_, Some dict)
    | Exact (_, Some dict) -> 
        let valueList =
            (dict.Keys |> List.ofSeq, dict.Values |> List.ofSeq)
            ||> List.zip
            |> List.choose (fun (key, value) ->
                let optionalDoc = 
                    value 
                    |> getTypeHasDefault
                    |> Option.defaultValue (value |> getTypeIsRequired)
                getTypePropDocumentation value
                |> Option.map (fun pdoc -> sprintf "%s: %s (%s)" key pdoc optionalDoc))

        if valueList.Length > 1 then
            valueList
            |> List.map (sprintf "'%s'")
            |> List.reduce (sprintf "%s, %s")
            |> sprintf "record with the fields: %s"
            |> Some
        else if valueList.Length = 1 then
            sprintf "record with the field: '%s'" valueList.[0] 
            |> Some
        else
            sprintf "record with the fields: unknown" 
            |> Some

    // A type we can't process
    | Other _
    | _ -> None

let generateComponentPropDescription (prop: SafeReactProp) =
    prop.description
    |> Option.defaultValue ""
    |> List.singleton

let generateComponentPropDocumentation (pname: string) (prop: SafeReactProp) =
    let optionalDoc = 
        (prop.required, prop.defaultValue) 
        ||> Option.map2 (fun preq pdef -> getTypeHasDefault pdef |> Option.defaultValue (getIsRequired preq))
        |> Option.map (sprintf "; %s")
        |> Option.defaultValue ""

    let typeDoc = 
        prop.propType 
        |> Option.bind getTypePropDocumentation

    let descriptionDoc = 
        prop.description
        |> Option.map (sprintf " - %s")
        |> Option.defaultValue ""

    typeDoc
    |> Option.map (fun tdoc -> sprintf "• %s (%s%s)%s" pname tdoc optionalDoc descriptionDoc)

let generateComponentPropsDocumentation (keepId: bool) (comp: SafeReactComponent) = 
    (comp.props.Keys |> List.ofSeq, comp.props.Values |> List.ofSeq)
    ||> List.zip
    |> (if not keepId then List.filter (fun (k,_) -> k.ToLowerInvariant() = "id" |> not) else id)
    |> List.choose (fun (k,v) -> generateComponentPropDocumentation k v)

let generateComponentDescription (comp: SafeReactComponent) = 
    [ comp.description |> Option.defaultValue "" ]

let generateComponentDocumentation (comp: SafeReactComponent) = 
    let componentDescription = comp |> generateComponentDescription
    let componentParameterDocs = comp |> generateComponentPropsDocumentation true

    [ yield! componentDescription; yield "Properties:"; yield! componentParameterDocs ]

// <summary>
// ExampleComponent is an example component.
// It takes a property, `label`, and
// displays it.
// It renders an input with the property `value`
// which is editable by the user.
// &#10;
// Properties:
// &#10;
// • id (string) - The ID used to identify this component in Dash callbacks.
// &#10;
// • label (string) - A label that will be printed when this component is rendered.
// &#10;
// • value (string) - The value displayed in the input.
// </summary>
let generatePropDocumentation (prop: SafeReactPropType) =
    let typeDoc = 
        prop
        |> getTypePropDocumentation

    let descriptionDoc = 
        prop
        |> getTypeDescription
        |> Option.defaultValue ""

    typeDoc
    |> Option.map (fun tdoc -> [ yield tdoc; yield descriptionDoc ])

let toXMLDoc (inner: string list) =
    let modInner =
        inner
        |> List.filter (String.length >> (<) 0)
        |> List.indexed
        //adding "&#10;" is the best way I could find to allow fantomas to add newlines to xml docs
        |> List.fold (fun acc (i, s) -> if i = 0 then s::acc else s::"&#10;"::acc) []
        |> List.rev
        |> List.map (String.replace "<" "&lt;")
        |> List.map (String.replace ">" "&gt;")
        |> List.map (String.replace "`" "&#96;")
    [yield "<summary>"; yield! modInner; yield "</summary>"]
    |> List.map (String.replace "\r" "")
    |> List.map (String.split "\n")
    |> List.concat
    |> List.map String.escape
