namespace Dash.NET

open Plotly.NET
open Newtonsoft.Json
open DynamicInvoke
open System

type Dependency =
    {
        [<JsonProperty("id")>]
        Id: string
        [<JsonProperty("property")>]
        Property: string
    }
    static member create(id, property) = { Id = id; Property = property }
    static member toCompositeId (d:Dependency) = sprintf "%s.%s" d.Id d.Property

type CallbackInput = Dependency
type CallbackOutput = Dependency
type CallbackState = Dependency

type ClientSideFunction =
    {
        [<JsonProperty("namespace")>]
        Namespace: string
        [<JsonProperty("function_name")>]
        FunctionName: string
    }

//This is the type that will be serialized and served on the _dash-dependencies endpoint to define callback bhaviour. Naming could be improved
//To-Do: autogenerate this from a registered callback handler and add to app Callback Dependency list to automatically serve on app start
type DashDependency =
    {
        [<JsonProperty("prevent_initial_call")>]
        PreventInitialCall: bool
        [<JsonProperty("clientside_function")>]
        ClientsideFunction: ClientSideFunction option
        [<JsonProperty("inputs")>]
        Inputs: CallbackInput []
        [<JsonProperty("output")>]
        Output: string
        [<JsonProperty("state")>]
        State: CallbackState []
    }
    static member create preventInitialCall clientsideFunction inputs output state =
        {
            PreventInitialCall = preventInitialCall
            ClientsideFunction = clientsideFunction
            Inputs = inputs
            Output = output
            State = state
        }

    static member createWithDefaults inputs output = DashDependency.create false None inputs output [||]

type RequestInput = 
    { 
        [<JsonProperty("id")>]
        Id: string
        [<JsonProperty("property")>]
        Property: string
        [<JsonProperty("value")>]
        Value: obj 
    }

open Newtonsoft.Json
open Newtonsoft.Json.Linq

type SingleOrArrayConverter<'T> () =
    inherit JsonConverter<'T []> ()
    
    override _.ReadJson(reader, (objectType:Type), (existingValue:'T []), (hasExistingValue:bool), (serializer:JsonSerializer)) : 'T [] =
        let token = JToken.Load(reader)
        if (token.Type = JTokenType.Array) then
            token.ToObject<'T []>()
        else 
            let res : 'T [] = [|token.ToObject<'T>()|] 
            res

    override _.WriteJson(writer:JsonWriter, value: 'T [], serializer: JsonSerializer) =
        if value.Length = 1 then
            let token = JToken.FromObject(value.[0])
            token.WriteTo(writer)
        else
            let jArr = JArray(value)
            jArr.WriteTo(writer)


///Type to deserialize calls to _dash-update-component
type CallbackRequest =
    {
        [<JsonProperty("output")>]
        Output: string
        [<JsonProperty("outputs")>]
        [<JsonConverter(typeof<SingleOrArrayConverter<CallbackOutput>>)>]
        Outputs: CallbackOutput []
        [<JsonProperty("changedPropIds")>]
        ChangedPropIds: string []
        [<JsonProperty("inputs")>]
        Inputs: RequestInput []
    }

//Central type for Callbacks. Creating an instance of this type and registering it on the callback map is the equivalent of the @app.callback decorator in python.
type Callback<'Function> =
    {
        Inputs: CallbackInput []
        Outputs: CallbackOutput []
        IsMulti: bool
        PreventInitialCall: bool
        HandlerFunction: 'Function
    }

    static member create inputs outputs pic (handler: 'Function) =
        {
            Inputs = inputs
            Outputs = outputs
            IsMulti = outputs.Length > 1
            PreventInitialCall = pic
            HandlerFunction = handler
        }

    //Necessary as generic types seem not te be unboxed as easily (problems arise e.g. when unboxing (box Callback<string,string>), as the og types used for
    //the generics are missing, therefore obj,obj is assumed and the cast fails)
    static member pack(handler: Callback<'Function>): Callback<obj> =
        Callback.create handler.Inputs handler.Outputs handler.PreventInitialCall (box handler.HandlerFunction)

    //returns a boxed result of the dynamic invokation of the handler function
    static member eval (args: seq<obj>) (handler: Callback<'Function>) =
        invokeDynamic<obj> handler.HandlerFunction args

    //returns the result of the dynamic invokation of the handler function casted to the type of choice
    static member evalAs<'OutputType> (args: seq<obj>) (handler: Callback<'Function>) =
        invokeDynamic<'OutputType> handler.HandlerFunction args

    static member toOutputId (handler: Callback<'Function>) =
        if handler.IsMulti then
            handler.Outputs
            |> Array.map (fun output ->
                sprintf "..%s" (Dependency.toCompositeId output)
            )
            |> Array.reduce (fun a b -> sprintf "%s.%s" a b)
            |> sprintf "%s.."
        else handler.Outputs.[0] |> Dependency.toCompositeId

    //returns the dash dependency to serve the client on app start via _dash-dependencies´from the given Callback
    static member toDashDependency (handler: Callback<'Function>) : DashDependency = 
        DashDependency.create
            handler.PreventInitialCall
            None
            handler.Inputs 
            (Callback.toOutputId handler)
            [||]


    //returns the response object to send as response to a request to _dash-update-component that triggered this callback
    static member getResponseObject (args: seq<obj>) (handler: Callback<'Function>) =
        if handler.IsMulti then
            let evalResult =
                handler
                |> Callback.pack
                |> Callback.evalAs<obj[]> args

            match evalResult with
            | Ok r ->

                //This should be properly wrapped/typed
                let root = DynamicObj()
                let response = DynamicObj()

                handler.Outputs
                |> Array.iteri (fun i output ->
                    let result = DynamicObj()
                    result?(output.Property) <- r.[i]
                    response?(output |> Dependency.toCompositeId) <- result
                )

                root?multi <-  handler.IsMulti
                root?response <- response

                root

            | Error e -> failwith e.Message
        else    

            let evalResult =
                handler
                |> Callback.pack
                |> Callback.eval args

            match evalResult with
            | Ok r ->

                //This should be properly wrapped/typed
                let root = DynamicObj()
                let response = DynamicObj()
                let result = DynamicObj()

                
                result?(handler.Outputs.[0].Property) <- r
                response?(handler |> Callback.toOutputId) <- result

                root?multi <-  handler.IsMulti
                root?response <- response

                root

            | Error e -> failwith e.Message

type CallbackMap() =
    inherit DynamicObj()

    static member registerCallback
        (callbackId: string)
        (callback: Callback<'Function>)
        (callbackMap: CallbackMap)
        =
        callbackMap?(callbackId) <- (Callback.pack callback)
        callbackMap

    static member unregisterCallback (callbackId: string) (callbackMap: CallbackMap) =
        match (callbackMap.TryGetTypedValue<Callback<obj>> callbackId) with
        | Some _ ->
            callbackMap.Remove(callbackId) |> ignore
            callbackMap
        | None -> callbackMap

    static member getPackedCallbackById (callbackId: string) (callbackMap: CallbackMap)
        : Callback<obj>
        =
        match (callbackMap.TryGetTypedValue<Callback<obj>> callbackId) with
        | Some cHandler -> cHandler
        | None -> failwithf "No callback handler registered for id %s" callbackId
