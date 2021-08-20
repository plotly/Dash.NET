module Dash.NET.ComponentGeneration.ReactMetadata

open System.Text.Json
open System
open System.Collections.Generic
open Serilog
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
    description: string 
    
    // flow and ts types
    ``type``: string
    raw: string
    elements: seq<ReactPropType>
    signature: ReactFlowSignature } 

    static member empty =
        { name = null
          value = Nullable()
          computed = Nullable()
          required = Nullable()
          description = null 
          ``type`` = null
          raw = null
          elements = []
          signature = ReactFlowSignature.empty }

    static member fromJsonString (json: string): ReactPropType =
        try
            JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
            |> optional
            |> Option.defaultValue ReactPropType.empty
        with
        | _ -> ReactPropType.empty

and ReactFlowSignature =
  { properties: seq<ReactFlowProperty> }

    static member empty = 
      { properties = seq [] }

and ReactFlowProperty = 
  { key: string
    value: ReactPropType 
    required: Nullable<bool> }

type ReactProp = 
  { ``type``: ReactPropType
    flowType: ReactPropType
    tsType: ReactPropType
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

let jsonToList (json: string): string list =
    JsonSerializer.Deserialize<seq<JsonElement>>(json, jsonOptions)
    |> optional
    |> Option.map (List.ofSeq)
    |> Option.map (List.map (fun (j: JsonElement) -> j.ToString()))
    |> Option.defaultValue []

type SafeReactPropProps = 
    { computed: bool option
      required: bool option
      description: string option }

// TODO: so far only handing the prop types that are also handled in _py_components_generation.py
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

    // Flow types
    | FlowUnion of props: SafeReactPropProps * value: SafeReactPropType list option
    | FlowArray of props: SafeReactPropProps * value: SafeReactPropType option
    | FlowObject of props: SafeReactPropProps * value: Dictionary<string, SafeReactPropType> option

    // A type we can't process
    | Other of name: string * props: SafeReactPropProps * value: string option

    static member tryGetFSharpTypeName (from: SafeReactPropType) =
        match from with
        | Array _ -> Some ["list"; "obj"]
        | Bool _ -> Some ["bool"]
        | Number _ -> Some ["IConvertible"]
        | String _ -> Some ["string"]
        | Object _ -> Some ["obj"]
        | Any _ -> Some ["obj"]
        | Element _ -> Some ["DashComponent"]
        | Node _ -> Some ["DashComponent"] 

        // Special cases, each type will have a unique name
        | Enum _
        | Union _
        | ArrayOf _
        | ObjectOf _
        | Shape _
        | Exact _
        | FlowUnion _
        | FlowArray _
        | FlowObject _ -> None


        // A type we can't process
        | Other _ -> Some ["obj"]

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
        | FlowUnion (props, _)
        | FlowArray (props, _)
        | FlowObject (props, _)
        | Other (_, props, _) ->
            props

    static member fromReactPropType (isFlow: bool) (from: ReactPropType) =
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

        let maybeType: string option =
            from.``type``
            |> optional

        let maybeElements: ReactPropType list =
            from.elements
            |> optional
            |> Option.map List.ofSeq
            |> Option.defaultValue []
            |> List.choose optional

        let maybeSignatureProps: ReactFlowProperty list =
            from.signature
            |> optional
            |> Option.bind (fun s -> 
                s.properties
                |> optional
                |> Option.map List.ofSeq
                |> Option.map (List.choose optional))
            |> Option.defaultValue []

        match from.name |> optional with
        | Some "array" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Array

        | Some "bool" 
        | Some "boolean" -> 
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

        | Some "object" 
        | Some "Object" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Object 

        | Some "any" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Any 

        | Some "element"
        | Some "Element" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Element

        | Some "node" 
        | Some "Node" -> 
            ( props, maybeStringVal )
            |> SafeReactPropType.Node

        // React PropTypes
        | Some "enum" ->
            ( props,
              maybeStringVal
              |> Option.map
                ( jsonToList 
                  >> List.map ReactPropType.fromJsonString
                  >> List.map (SafeReactPropType.fromReactPropType isFlow) ) )
            |> SafeReactPropType.Enum

        | Some "union" when not isFlow ->
            ( props,
              maybeStringVal
              |> Option.map
                ( jsonToList 
                  >> List.map ReactPropType.fromJsonString
                  >> List.map (SafeReactPropType.fromReactPropType isFlow) ) )
            |> SafeReactPropType.Union

        | Some "arrayOf" ->
            ( props,
              maybeStringVal
              |> Option.map ReactPropType.fromJsonString
              |> Option.map (SafeReactPropType.fromReactPropType isFlow) )
            |> SafeReactPropType.ArrayOf

        | Some "objectOf" ->
            ( props,
              maybeStringVal
              |> Option.map ReactPropType.fromJsonString
              |> Option.map (SafeReactPropType.fromReactPropType isFlow) )
            |> SafeReactPropType.ObjectOf

        | Some "shape" ->
            ( props,
              maybeStringVal
              |> Option.map (fun v -> 
                  let jsonDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>> v
                  let newDict = Dictionary<string, SafeReactPropType>()
                  (jsonDict.Keys |> List.ofSeq, jsonDict.Values |> List.ofSeq)
                  ||> List.zip
                  |> List.iter (fun (k,vj) -> 
                      vj
                      |> optional
                      |> Option.map (fun v -> v.ToString())
                      |> Option.map ReactPropType.fromJsonString
                      |> Option.map (SafeReactPropType.fromReactPropType isFlow)
                      |> Option.iter (fun v -> newDict.Add( k, v )))
                  newDict    
              ) )
            |> SafeReactPropType.Shape

        | Some "exact" -> 
            ( props,
              maybeStringVal
              |> Option.map (fun v -> 
                  let jsonDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>> v
                  let newDict = Dictionary<string, SafeReactPropType>()
                  (jsonDict.Keys |> List.ofSeq, jsonDict.Values |> List.ofSeq)
                  ||> List.zip
                  |> List.iter (fun (k,vj) -> 
                      vj
                      |> optional
                      |> Option.map (fun v -> v.ToString())
                      |> Option.map ReactPropType.fromJsonString
                      |> Option.map (SafeReactPropType.fromReactPropType isFlow)
                      |> Option.iter (fun v -> newDict.Add( k, v )))
                  newDict  
              ) )
            |> SafeReactPropType.Exact

        // Flow and ts types
        | Some "union" when isFlow ->
            ( props,
              maybeElements
              |> List.map (SafeReactPropType.fromReactPropType isFlow)
              |> (fun l -> if (l.Length) > 0 then Some l else None) )
            |> SafeReactPropType.FlowUnion

        | Some "Array" -> 
            ( props,
              maybeElements
              |> List.tryHead
              |> Option.map (SafeReactPropType.fromReactPropType isFlow) )
            |> SafeReactPropType.FlowArray

        | Some "signature" when maybeType = Some "object" -> 
            ( props,
              let newDict = Dictionary<string, SafeReactPropType>()
              maybeSignatureProps
              |> List.iter (fun prop -> 
                  let maybeKey = 
                      prop.key
                      |> optional

                  let maybeRequired = 
                      prop.required
                      |> optional

                  let maybeProp =
                      prop.value
                      |> optional
                      |> Option.map (fun (ptype: ReactPropType) ->
                          maybeRequired
                          |> Option.map (fun req -> { ptype with required = req })
                          |> Option.defaultValue ptype)
                      |> Option.map (SafeReactPropType.fromReactPropType isFlow)

                  (maybeKey, maybeProp)
                  ||> Option.map2 (fun k v -> newDict.Add( k, v ))
                  |> ignore)

              Some newDict)
            |> SafeReactPropType.FlowObject

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

    static member fromReactProp (prop: ReactProp): SafeReactProp =
        let propType =
            prop.``type`` 
            |> optional 
            |> Option.map (SafeReactPropType.fromReactPropType false)

            // If there is no prop type defined, check for a flow type
            |> Option.bindNone
              ( prop.flowType
                |> optional 
                |> Option.map (SafeReactPropType.fromReactPropType true) )

            // If there is no flow type defined, check for a ts type
            |> Option.bindNone
              ( prop.tsType
                |> optional 
                |> Option.map (SafeReactPropType.fromReactPropType true) )

        { propType = propType
          required = prop.required |> optional
          description = prop.description  |> optional
          defaultValue = prop.defaultValue |> optional |> Option.map (SafeReactPropType.fromReactPropType false)}

type SafeReactComponent =
  { description: string option
    displayName: string option
    props: Dictionary<string, SafeReactProp> }

    static member fromReactComponent (comp: ReactComponent): SafeReactComponent =
        
        let removeUnrepresentableProps (dict: Dictionary<string, SafeReactProp>) =
            let newDict = Dictionary()
            (dict.Keys |> List.ofSeq, dict.Values |> List.ofSeq)
            ||> List.zip
            |> List.filter (snd >> (fun p -> p.propType) >> function | Some (Other _) | None -> false | _ -> true)
            |> List.iter newDict.Add
            newDict

        { description = comp.description |> optional
          displayName = comp.displayName |> optional 
          props = 
            comp.props 
            |> optional 
            |> Option.map (toSafeDict SafeReactProp.fromReactProp)
            |> Option.defaultValue (Dictionary())
            |> removeUnrepresentableProps }

type ReactMetadata = Dictionary<string, SafeReactComponent>

let toSafe (meta: Dictionary<string, ReactComponent>): ReactMetadata =
    meta 
    |> optional 
    |> Option.map (toSafeDict SafeReactComponent.fromReactComponent)
    |> Option.defaultValue (Dictionary())

let jsonDeserialize (json: string) = 
    try
        JsonSerializer.Deserialize<Dictionary<string, ReactComponent>>(json, jsonOptions) |> toSafe |> Ok
    with 
    | e -> 
        Error e
