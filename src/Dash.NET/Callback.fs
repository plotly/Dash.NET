namespace Dash.NET

open Plotly.NET
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System
open DynamicInvoke

type ClientSideFunction =
    {
        [<JsonProperty("namespace")>]
        Namespace: string
        [<JsonProperty("function_name")>]
        FunctionName: string
    }

type RequestInput = 
    { 
        [<JsonProperty("id")>]
        Id: string
        [<JsonProperty("property")>]
        Property: string
        [<JsonProperty("value")>]
        Value: JToken
    }

open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System

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
        [<JsonProperty("state")>]
        State:RequestInput []
    }

type CallbackResponse() = 
    inherit DynamicObj()

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

    //static members
    [<JsonIgnore()>]
    member _.HandlerFunction : 'Function = HandlerFunction

    [<JsonProperty("multi")>]
    member _.Multi : bool = Multi 

    [<JsonProperty("prevent_initial_call")>]
    member _.PreventInitialCall = defaultArg PreventInitialCall true

    [<JsonProperty("clientside_function")>]
    member _.ClientSideFunction = ClientSideFunction

    //initializers using dynamic fields

    ////dynamic members
    //[<JsonProperty("inputs")>]
    //member _.Inputs = Inputs
    //[<JsonProperty("output")>]
    //[<JsonConverter(typeof<OutputConverter>)>]
    //member _.Output = Output
    //[<JsonProperty("state")>]
    //member _.State = defaultArg State Seq.empty



    /// returns a callback that binds a handler function mapping from multiple input components to a single output component (n -> 1)
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
            let state = 
                defaultArg State Seq.empty
                |> Seq.map Dependency.toCompositeId

            callbackId  |> DynObj.setValue cb "output"
            inputs      |> DynObj.setValue cb "inputs"
            state       |> DynObj.setValue cb "state"

            output |> Seq.singleton |> DynObj.setValue cb "outputDependencies"

            cb
    
    /// returns a callback that binds a handler function mapping from a single input component to a single output component
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
                    argument.ToObject(targetType))
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
                    CallbackResponse.multiOut(bindings)

                | :? seq<obj> as boxedResults -> 

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

                | _ -> failwithf "multi callback result %O was not a collection." r

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

    static member registerCallback
        (callback: Callback<'Function>)
        (callbackMap: CallbackMap)
        =
        let callbackId = callback?output |> unbox<string>
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

    static member toDependencies (callbackMap: CallbackMap) =
        let members = callbackMap.GetDynamicMemberNames()
        members
        |> Seq.map (fun cName ->
            CallbackMap.getPackedCallbackById cName callbackMap
            |> Callback.toDependencyGraph
        )

        
