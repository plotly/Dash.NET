namespace Dash.NET

open DynamicObj
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open Dash.NET.Common
open DynamicInvoke

/// JSON converter to always convert a single item as well as a JArray of items on the same field to `Seq<'T>`
type SingleOrArrayConverter<'T>() =

    inherit JsonConverter<'T []>()

    override _.ReadJson(reader, (objectType:Type), (existingValue:'T []), (hasExistingValue:bool), (serializer:JsonSerializer)) =
        let token = JToken.Load(reader)
        if (token.Type = JTokenType.Array) then
            token.ToObject<'T []>()
        else
            [|token.ToObject<'T>()|]

    override _.WriteJson(writer:JsonWriter, value: 'T [], serializer: JsonSerializer) =
        let token = JToken.FromObject(value)
        token.WriteTo(writer)

/// Client side function that can be bound to a callback
type ClientSideFunction =
    {
        [<JsonProperty("namespace")>]
        Namespace: string
        [<JsonProperty("function_name")>]
        FunctionName: string
    }

/// A single element of the `inputs` field of a JSON request to `_dash-update-component`
type RequestInput = 
    { 
        [<JsonProperty("id")>]
        Id: string
        [<JsonProperty("property")>]
        Property: string
        [<JsonProperty("value")>]
        Value: JToken
    }

///Type to deserialize calls to _dash-update-component into
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
        [<JsonProperty("state")>]
        State:RequestInput []
    }

/// The response object that will be serialized and returned as response to requests at `_dash-update-component`
type CallbackResponse() = 
    inherit DynamicObj()

    /// creates a `CallbackResponse` object for a single output callback response for the given property of the given component with the given evaluation result
    static member singleOut
        (
            outputId: string,
            outputProperty:string,
            evaluationResult:obj
        ) = 

            let cr = CallbackResponse()
            
            //This should be properly wrapped/typed
            let response = DynamicObj()
            let result = DynamicObj()

            result?(outputProperty) <- evaluationResult
            response?(outputId) <- result

            cr?multi <- true
            cr?response <- response

            printfn "Result JSON : %A" (result |> JsonConvert.SerializeObject)
            printfn "Response JSON : %A" (response |> JsonConvert.SerializeObject)
            printfn "final response : %A" cr
            printfn "final response JSON : %A" (cr |> JsonConvert.SerializeObject)

            cr

    /// creates a `CallbackResponse` object for a single output callback response from the given `CallbackResultBinding`
    static member singleOut
        (
            binding:CallbackResultBinding
        ) = 

            CallbackResponse.singleOut
                (
                    binding.Target.Id,
                    binding.Target.Property,
                    binding.BoxedResult
                )

    static member multiOut 
        (
            outputs: seq<string*string*obj>
        ) = 
            let cr = CallbackResponse()
            let response = DynamicObj()

            outputs 
            |> Seq.iter (fun (outputId, outputProperty, evaluationResult) ->
                let result = DynamicObj()
                result?(outputProperty) <- evaluationResult
                response?(outputId) <- result
            )

            cr?multi <- true
            cr?response <- response

            cr

    static member multiOut 
           (
               bindings: seq<CallbackResultBinding>
           ) = 
               CallbackResponse.multiOut
                (
                    bindings |> Seq.map (fun binding -> 
                        binding.Target.Id, binding.Target.Property, binding.BoxedResult
                    )
                )

/// The Central Callback type. 
/// This type serves two purposes:
///
/// 1. Externally, the Serialized object is returned via the `_dash-dependencies` endpoint of a dash application to set up the dash renderer loop
///
/// 2. Internally, the HandlerFunction of the Callback is evaluated when a qualified call to `_dash-update-component` is performed.
///
/// As this type needs dynamic members in addition to the static members created by the default constructor, it is not recommended to use the constructor to create a callback,
/// but rather the `singleOut` or `multiOut` members.
///
/// Creating an instance of this type and registering it on the callback map of the DashApp is the equivalent of the @app.callback decorator in python.
type Callback<'Function> 
    (
        HandlerFunction: 'Function,
        Multi: bool,
        ?PreventInitialCall:bool,
        ?ClientSideFunction:ClientSideFunction
    ) =

    inherit DynamicObj()

    //static members -----------------------------------------------------------------------------------------
    
    [<JsonIgnore()>]
    /// The handler function that maps the callback input components to the callback output components
    member _.HandlerFunction : 'Function = HandlerFunction

    [<JsonProperty("multi")>]
    /// Indicates wether the callback is a n -> i mapping (false) or n -> n mapping (true)
    member _.Multi : bool = Multi 

    [<JsonProperty("prevent_initial_call")>]
    /// If true, the callback will not be called during initialization of the DashApp
    member _.PreventInitialCall = defaultArg PreventInitialCall true

    [<JsonProperty("clientside_function"); JsonConverter(typeof<Json.OptionConverter<ClientSideFunction>>)>]
    /// A clientside function that should be run by this callback
    member _.ClientSideFunction = ClientSideFunction

    //dynamic member initialization --------------------------------------------------------------------------

    /// <summary>returns a callback that binds a handler function mapping from multiple input components to a single output component (n -> 1)</summary>
    /// <param name="inputs"> A sequence of `CallbackInput` that represents the input components of this callback. Changes to any of these components signalled by the client will trigger the callback. </param>
    /// <param name="output"> A `CallbackOutput` that represents the output component of this callback </param>
    /// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    /// <param name="State"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    /// <param name="PreventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    /// <param name="ClientSideFunction"> A client side function to execute with the callback </param>
    static member singleOut
        (
            inputs: seq<CallbackInput>,
            output: CallbackOutput,
            handlerFunction: 'Function,
            ?State: seq<CallbackState>,
            ?PreventInitialCall:bool,
            ?ClientSideFunction:ClientSideFunction
        ) =
            let cb = 
                Callback(
                    handlerFunction,
                    false,
                    ?PreventInitialCall = PreventInitialCall,
                    ?ClientSideFunction = ClientSideFunction
                )
            let callbackId = Dependency.toCompositeId output
            let state = defaultArg State Seq.empty

            callbackId  |> DynObj.setValue cb "output"
            inputs      |> DynObj.setValue cb "inputs"
            state       |> DynObj.setValue cb "state"

            output |> Seq.singleton |> DynObj.setValue cb "outputDependencies"

            cb
    
    /// <summary>returns a callback that binds a handler function mapping from a single input component to a single output component (1 -> 1)</summary>
    /// <param name="input"> A `CallbackInput` that represents the input component of this callback. Changes to this component signalled by the client will trigger the callback.</param>
    /// <param name="output"> A `CallbackOutput` that represents the output component of this callback </param>
    /// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    /// <param name="State"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    /// <param name="PreventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    /// <param name="ClientSideFunction"> A client side function to execute with the callback </param>
    static member singleOut
        (
            input: CallbackInput,
            output: CallbackOutput,
            handlerFunction: 'Function,
            ?State: seq<CallbackState>,
            ?PreventInitialCall:bool,
            ?ClientSideFunction:ClientSideFunction
        ) =
            Callback.singleOut
                (
                    input |> Seq.singleton,
                    output,
                    handlerFunction,
                    ?State = State,
                    ?PreventInitialCall=PreventInitialCall,
                    ?ClientSideFunction=ClientSideFunction
                )

    /// <summary>returns a callback that binds a handler function mapping from multiple input components to multiple output components (n -> n)</summary>
    /// <param name="inputs"> A sequence of `CallbackInput` that represents the input components of this callback. Changes to any of these components signalled by the client will trigger the callback. </param>
    /// <param name="outputs"> A sequence of `CallbackOutput` that represents the output components of this callback </param>
    /// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    /// <param name="State"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    /// <param name="PreventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    /// <param name="ClientSideFunction"> A client side function to execute with the callback </param>
    static member multiOut 
        (
            inputs: seq<CallbackInput>,
            outputs: seq<CallbackOutput>,
            handlerFunction: 'Function,
            ?State: seq<CallbackState>,
            ?PreventInitialCall:bool,
            ?ClientSideFunction:ClientSideFunction
        ) = 
            let cb = 
                Callback(
                    handlerFunction,
                    true,
                    ?PreventInitialCall = PreventInitialCall,
                    ?ClientSideFunction = ClientSideFunction
                )

            let callbackId = Dependency.toMultiCompositeId outputs

            let state = defaultArg State Seq.empty

            callbackId  |> DynObj.setValue cb "output"
            inputs      |> DynObj.setValue cb "inputs"
            state       |> DynObj.setValue cb "state"

            outputs |> DynObj.setValue cb "outputDependencies"

            cb

    /// returns a callback that binds a handler function mapping from a single input component to multiple output components (1 -> n)
    /// <param name="input"> A `CallbackInput` that represents the input component of this callback. Changes to this component signalled by the client will trigger the callback. </param>
    /// <param name="outputs"> A sequence of `CallbackOutput` that represents the output components of this callback </param>
    /// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    /// <param name="?State"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    /// <param name="?PreventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    /// <param name="?ClientSideFunction"> A client side function to execute with the callback </param>
    static member multiOut 
        (
            input: CallbackInput,
            outputs: seq<CallbackOutput>,
            handlerFunction: 'Function,
            ?State: seq<CallbackState>,
            ?PreventInitialCall:bool,
            ?ClientSideFunction:ClientSideFunction
        ) = 
            Callback.multiOut
                (
                    input |> Seq.singleton,
                    outputs,
                    handlerFunction,
                    ?State = State,
                    ?PreventInitialCall=PreventInitialCall,
                    ?ClientSideFunction=ClientSideFunction
                )

    /// Returns a copy of the given `Callback` (copies dynamic members aswell)
    static member copy (callback: Callback<'Function>) : Callback<'Function> =

        let copyDynamicMembers (fromObj:DynamicObj) (toObj:DynamicObj) =
            fromObj.GetProperties(false)
            |> Seq.iter (fun (kv) ->
                toObj?(kv.Key) <- kv.Value
            )

        let copy = 
            Callback(
                callback.HandlerFunction,
                callback.Multi,
                callback.PreventInitialCall,
                ?ClientSideFunction = callback.ClientSideFunction
            )

        copyDynamicMembers callback copy

        copy

    /// Returns a copy of the callback with all dynamic fields removed that prevent the callback to be correctly serialized and returned as dependency graph at `_dash-dependencies`
    static member toDependencyGraph (callback: Callback<'Function>) =

        let copy = Callback.copy callback

        DynObj.remove copy "outputDependencies"

        copy


    //Necessary as generic types seem not te be unboxed as easily (problems arise e.g. when unboxing (box Callback<string,string>), as the og types used for
    //the generics are missing, therefore obj,obj is assumed and the cast fails)
    static member pack(handler: Callback<'Function>): Callback<obj> =
        
        let copyDynamicMembers (fromObj:DynamicObj) (toObj:DynamicObj) =
            fromObj.GetProperties(false)
            |> Seq.iter (fun (kv) ->
                toObj?(kv.Key) <- kv.Value
            )

        let packed = 
            Callback(
                box handler.HandlerFunction,
                handler.Multi,
                handler.PreventInitialCall,
                ?ClientSideFunction = handler.ClientSideFunction
            )

        copyDynamicMembers handler packed

        packed

    //returns a boxed result of the dynamic invokation of the handler function
    static member eval (args: seq<obj>) (handler: Callback<'Function>) =
        invokeDynamic<obj> handler.HandlerFunction args

    //returns the result of the dynamic invokation of the handler function casted to the type of choice
    static member evalAs<'OutputType> (args: seq<obj>) (handler: Callback<'Function>) =
        invokeDynamic<'OutputType> handler.HandlerFunction args

    //returns the response object to send as response to a request to _dash-update-component that triggered this callback
    static member getResponseObject (args: seq<JToken>) (handler: Callback<'Function>) =

        // array of types of the input arguments of the handler function
        let callbackHandlerFunctionRange = DynamicInvoke.getFunctionDomain (handler.HandlerFunction.GetType())

        let range = DynamicInvoke.getFunctionRange (handler.HandlerFunction.GetType())

        // shadow input args with a boxed collection of guarded conversions from the jtoken to the handler function's input type
        let args =
            if callbackHandlerFunctionRange.Length = Seq.length args then
                callbackHandlerFunctionRange
                |> Seq.zip args
                |> Seq.map (fun (argument,targetType) -> 
                    //printfn "JsonType: %A;      ArgType:%A" argument.Type targetType
                    // it may be necessary to inspect the JToken when the input is an object/higher kinded type
                    if not <| isNull argument then
                        argument.ToObject(targetType)
                    else
                        //F# can handle a non-nullable type being null, it just might throw an exception if you try to use it
                        //you can get around this with Nullable<...> 
                        null
                )
            else
                failwithf "handler function arguments and targetTypes have different lenght: args:%i vs. types:%i" callbackHandlerFunctionRange.Length (Seq.length args)

        let evalResult =
            handler
            |> Callback.pack
            |> Callback.eval args

        match evalResult with
        | Ok r ->

            if handler.Multi then

                match r with
                | :? seq<CallbackResultBinding> as bindings -> 
                // The result is a sequence of boxed CallbackResultBindings

                    CallbackResponse.multiOut(bindings)

                | :? seq<obj> as boxedResults -> 
                // The result of the multioutput is a sequence of boxed types

                    let outputs = 
                        handler?outputDependencies 
                        |> unbox<seq<CallbackOutput>>

                    if (Seq.length outputs) = (Seq.length boxedResults) then

                        CallbackResponse.multiOut(
                            outputs
                            |> Seq.zip boxedResults
                            |> Seq.map (fun (boxedRes, output) -> output.Id, output.Property, boxedRes)
                        )

                    else
                        failwithf "amount of multi callback outputs did not match to the actual callback binding (expected %i but got %i)" (Seq.length outputs) (Seq.length boxedResults)

                | r when (isIConvertibleSeq (r.GetType())) ->
                // The result of the multioutput is a boxed sequence of IConvertibles, e.g.
                // [2;3;4], [|"1";"2"|]; seq{2.;3.}
                
                    let primitiveSeq = 
                        r 
                        |> unbox<System.Collections.IEnumerable> 
                        |> Seq.cast<IConvertible>

                    let outputs = 
                        handler?outputDependencies 
                        |> unbox<seq<CallbackOutput>>

                    if (Seq.length outputs) = (Seq.length primitiveSeq) then

                        CallbackResponse.multiOut(
                            outputs
                            |> Seq.zip primitiveSeq
                            |> Seq.map (fun (result, output) -> output.Id, output.Property, (box result))

                        )

                    else
                        failwithf "The amount of multi callback outputs returned by the callback function did not match to the amount of outputs defined by the callback dependency (expected %i vs %i). Make sure that the callback function returns a collection of results of length %i" (Seq.length primitiveSeq) (Seq.length outputs) (Seq.length outputs)

                | _ -> failwithf "multi callback result %O of type %O was not a supported collection (supported: seq<IConvertible>, seq<obj>, seq<CallbackResultBinding>). You might be able to circumvent this problem by boxing the return values of your results to generate a seq<obj>." r (r.GetType())
            
            else
                match r with
                | :? CallbackResultBinding as binding -> 
                     CallbackResponse.singleOut(binding)

                | _ -> 
                    let output = 
                        handler?outputDependencies 
                        |> unbox<seq<CallbackOutput>>
                        |> Seq.item 0
                
                    CallbackResponse.singleOut(
                        output.Id,
                        output.Property,
                        r
                    )
        | Error e -> failwith e.Message

type CallbackMap() =
    inherit DynamicObj()

    /// adds the given `Callback` to the given `CallbackMap` by setting it as dynamic member with the field name equal to the callback output composite id.
    static member registerCallback (callback: Callback<'Function>) (callbackMap: CallbackMap) =
        if 
            callbackMap.GetProperties(false) 
            |> Seq.exists(fun kv -> 
                kv.Key = (callback?output |> unbox<string>)
            )  
        then printfn "Warning: duplicate registration of %s. The previous callback registration will be overwritten." (callback?output |> unbox<string>)
        
        let callbackId = callback?output |> unbox<string>
        callbackMap?(callbackId) <- (Callback.pack callback)
        callbackMap

    /// If there is a callback registered at the given id (meaning if there is a dynamic member with the given field name) , removes it from the `CallbackMap`.
    static member unregisterCallback (callbackId: string) (callbackMap: CallbackMap) =
        match (callbackMap.TryGetTypedValue<Callback<obj>> callbackId) with
        | Some _ ->
            callbackMap.Remove(callbackId) |> ignore
            callbackMap
        | None -> callbackMap

    /// Returns the packed `Callback` (meaning the generic type annotation of the `Callback` is obj, as its handlerfunction is boxed) registered at the given id when it exists.
    static member getPackedCallbackById (callbackId: string) (callbackMap: CallbackMap)
        : Callback<obj>
        =
        match (callbackMap.TryGetTypedValue<Callback<obj>> callbackId) with
        | Some cHandler -> cHandler
        | None -> failwithf "No callback handler registered for id %s" callbackId

    static member toDependencies (callbackMap: CallbackMap) =
        let members = callbackMap.GetDynamicMemberNames()
        members
        |> Seq.map (fun cName ->
            CallbackMap.getPackedCallbackById cName callbackMap
            |> Callback.toDependencyGraph
        )

        
