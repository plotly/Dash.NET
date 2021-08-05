module Dash.NET.ComponentGeneration.ReactMetadata

open System.Text.Json
open System
open System.Collections.Generic
open FSharpPlus.Lens
open Prelude

let jsonOptions = 
    JsonSerializerOptions(
        DefaultIgnoreCondition = Serialization.JsonIgnoreCondition.WhenWritingNull
    )

// JsonSerializer can make things null that can't be null according to F#'s type
// system, so we do some casting magic to convert it to an option 
let optional (prop: 'a) = prop |> box |> Option.ofObj |> Option.map unbox

type ReactPropType =
  { name: string
    value: Nullable<JsonElement> // Can be many different types, we can convert it later
    computed: Nullable<bool>

    // used in objectOf/exact/ect
    required: Nullable<bool>
    description: string }

    static member fromJsonString (json: string): ReactPropType =
        let emptyPType =
            { name = null
              value = Nullable()
              computed = Nullable()
              required = Nullable()
              description = null }

        try
            JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
            |> optional
            |> Option.defaultValue emptyPType
        with
        | _ -> emptyPType

type ReactProp = 
  { ``type``: ReactPropType
    //flowType: ReactFlowType //TODO
    //tsType: ReactTsType //TODO
    required: Nullable<bool>
    description: string
    defaultValue: ReactPropType }

type ReactComponent =
  { description: string
    displayName: string
    //methods - we dont care about methods
    props: Dictionary<string, ReactProp> }

let toSafeDict (converter: 'a -> 'b) (dict: Dictionary<string, 'a>): Dictionary<string, 'b> =
    let newDict = Dictionary<string, 'b>()
    (dict.Keys |> List.ofSeq, dict.Values |> List.ofSeq)
    ||> List.zip 
    |> List.iter (fun (key, maybeV) -> 
        maybeV
        |> optional
        |> Option.iter (fun v -> 
            newDict.Add(key, v |> converter)
        )
    )
    newDict

let jsonToArray (json: string): string list =
    try
        JsonSerializer.Deserialize<seq<JsonElement>>(json, jsonOptions)
        |> optional
        |> Option.map (List.ofSeq)
        |> Option.map (List.map (fun (j: JsonElement) -> j.ToString()))
        |> Option.defaultValue []
    with
    | e -> [] //TODO better error logging 

type SafeReactPropProps = 
    { computed: bool option
      required: bool option
      description: string option }

type SafeReactPropType =
    | Array of props: SafeReactPropProps * value: string option
    | Bool of props: SafeReactPropProps * value: bool option
    | Number of props: SafeReactPropProps * value: IConvertible option
    | String of props: SafeReactPropProps * value: string option
    | Object of props: SafeReactPropProps * value: string option
    | Any of props: SafeReactPropProps * value: string option
    | Element of props: SafeReactPropProps * value: string option
    | Node of props: SafeReactPropProps * value: string option

    // React PropTypes
    | Enum of props: SafeReactPropProps * value: SafeReactPropType list option
    | Union of props: SafeReactPropProps * value: SafeReactPropType list option
    | ArrayOf of props: SafeReactPropProps * value: SafeReactPropType option
    | ObjectOf of props: SafeReactPropProps * value: SafeReactPropType option
    | Shape of props: SafeReactPropProps * value: Dictionary<string, SafeReactPropType> option
    | Exact of props: SafeReactPropProps * value: Dictionary<string, SafeReactPropType> option

    // A type we can't process
    | Other of name: string * props: SafeReactPropProps * value: string option

    static member tryGetFSharpTypeName (from: SafeReactPropType) =
        match from with
        | Array _ -> Some "obj list"
        | Bool _ -> Some "bool"
        | Number _ -> Some "IConvertible"
        | String _ -> Some "string"
        | Object _ -> Some "obj"
        | Any _ -> Some "obj"
        | Element _ -> Some "obj" //TODO allow passing in dash components?
        | Node _ -> Some "obj" //TODO allow passing in dash components?

        // Special cases, each type will have a unique name
        | Enum _ -> None
        | Union _ -> None
        | ArrayOf _ -> None
        | ObjectOf _ -> None
        | Shape _ -> None
        | Exact _ -> None

        // A type we can't process
        | Other _ -> Some "obj"

    static member getProps (from: SafeReactPropType) =
        match from with
        | Array (props, _)
        | Bool (props, _)
        | Number (props, _)
        | String (props, _)
        | Object (props, _)
        | Any (props, _)
        | Element (props, _)
        | Node (props, _)
        | Enum (props, _)
        | Union (props, _)
        | ArrayOf (props, _)
        | ObjectOf (props, _)
        | Shape (props, _)
        | Exact (props, _)
        | Other (_, props, _) ->
            props

    static member fromReactPropType (from: ReactPropType) =
        let props =
            { computed = from.computed |> optional
              required = from.required |> optional
              description = from.description |> optional }

        let maybeStringVal: string option = 
            from.value 
            |> optional
            |> Option.map (fun dv -> 
                // String values in some cases (eg enum cases) are quoted because they are encoded as a json string
                // this causes annoying issues down the line, so we want to get rid of those quotes here
                dv.ToString() |> String.removeQuotes)

        match from.name |> optional with
        | Some "array" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Array
        | Some "bool" -> 
            ( props,
              maybeStringVal
              |> Option.map (fun dv ->
                  dv.ToLowerInvariant() = "true") )
            |> SafeReactPropType.Bool 
        | Some "number" -> 
            ( props,
              maybeStringVal
              |> Option.bind (fun dv -> 
                  let suc, v = dv |> Double.TryParse
                  if suc then Some (v :> IConvertible) else None) )
            |> SafeReactPropType.Number
        | Some "string" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.String
        | Some "object" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Object 
        | Some "any" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Any 
        | Some "element" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Element
        | Some "node" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Node

        // React PropTypes
        | Some "enum" ->
            ( props,
              maybeStringVal
              |> Option.map
                ( jsonToArray 
                  >> List.map ReactPropType.fromJsonString
                  >> List.map SafeReactPropType.fromReactPropType ) )
            |> SafeReactPropType.Enum
        | Some "union" ->
            ( props,
              maybeStringVal
              |> Option.map
                ( jsonToArray 
                  >> List.map ReactPropType.fromJsonString
                  >> List.map SafeReactPropType.fromReactPropType ) )
            |> SafeReactPropType.Union
        | Some "arrayOf" ->
            ( props,
              maybeStringVal
              |> Option.map ReactPropType.fromJsonString
              |> Option.map SafeReactPropType.fromReactPropType )
            |> SafeReactPropType.ArrayOf
        | Some "objectOf" ->
            ( props,
              maybeStringVal
              |> Option.map ReactPropType.fromJsonString
              |> Option.map SafeReactPropType.fromReactPropType )
            |> SafeReactPropType.ObjectOf
        | Some "shape" ->
            ( props,
              maybeStringVal
              |> Option.map (fun v -> 
                try
                    let jsonDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>> v
                    let newDict = Dictionary<string, SafeReactPropType>()
                    (jsonDict.Keys |> List.ofSeq, jsonDict.Values |> List.ofSeq)
                    ||> List.zip
                    |> List.iter (fun (k,vj) -> 
                        vj
                        |> optional
                        |> Option.map (fun v -> v.ToString())
                        |> Option.map ReactPropType.fromJsonString
                        |> Option.map SafeReactPropType.fromReactPropType
                        |> Option.iter (fun v -> newDict.Add( k, v )))
                    newDict
                with
                | _ -> Dictionary<string, SafeReactPropType>() //TODO better error logging       
              ) )
            |> SafeReactPropType.Shape
        | Some "exact" -> 
            ( props,
              maybeStringVal
              |> Option.map (fun v -> 
                try
                    let jsonDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>> v
                    let newDict = Dictionary<string, SafeReactPropType>()
                    (jsonDict.Keys |> List.ofSeq, jsonDict.Values |> List.ofSeq)
                    ||> List.zip
                    |> List.iter (fun (k,vj) -> 
                        vj
                        |> optional
                        |> Option.map (fun v -> v.ToString())
                        |> Option.map ReactPropType.fromJsonString
                        |> Option.map SafeReactPropType.fromReactPropType
                        |> Option.iter (fun v -> newDict.Add( k, v )))
                    newDict
                with
                | _ -> Dictionary<string, SafeReactPropType>() //TODO better error logging   
              ) )
            |> SafeReactPropType.Exact

        // We dont know how to proccess this type
        | Some name ->
            ( name, props, maybeStringVal )
            |> SafeReactPropType.Other 
        
        // Default (usually defaultValue)
        | None -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Any 

type SafeReactProp = 
  { propType: SafeReactPropType option
    required: bool option
    description: string option
    defaultValue: SafeReactPropType option }

    static member inline _type f (p: SafeReactProp) = f p.propType <&> fun x -> { p with propType = x }
    static member inline _required f (p: SafeReactProp) = f p.required <&> fun x -> { p with required = x }
    static member inline _description f (p: SafeReactProp) = f p.description <&> fun x -> { p with description = x }
    static member inline _defaultValue f (p: SafeReactProp) = f p.defaultValue <&> fun x -> { p with defaultValue = x }

    static member fromReactProp (prop: ReactProp): SafeReactProp =
        { propType = prop.``type`` |> optional |> Option.map (SafeReactPropType.fromReactPropType)
          required = prop.required |> optional
          description = prop.description  |> optional
          defaultValue = prop.defaultValue |> optional |> Option.map (SafeReactPropType.fromReactPropType)}

type SafeReactComponent =
  { description: string option
    displayName: string option
    props: Dictionary<string, SafeReactProp> }

    static member inline _description f (p: SafeReactComponent) = f p.description <&> fun x -> { p with description = x }
    static member inline _displayName f (p: SafeReactComponent) = f p.displayName <&> fun x -> { p with displayName = x }
    static member inline _props f (p: SafeReactComponent) = f p.props <&> fun x -> { p with props = x }

    static member fromReactComponent (comp: ReactComponent): SafeReactComponent =
        { description = comp.description |> optional
          displayName = comp.displayName |> optional 
          props = 
            comp.props 
            |> optional 
            |> Option.map (toSafeDict SafeReactProp.fromReactProp)
            |> Option.defaultValue (Dictionary()) }

type ReactMetadata = Dictionary<string, SafeReactComponent>

let toSafe (meta: Dictionary<string, ReactComponent>): ReactMetadata =
    meta 
    |> optional 
    |> Option.map (toSafeDict SafeReactComponent.fromReactComponent)
    |> Option.defaultValue (Dictionary())

let jsonDeserialize (json: string) = 
    //TODO failure handling
    JsonSerializer.Deserialize<Dictionary<string, ReactComponent>>(json, jsonOptions)
    |> toSafe

