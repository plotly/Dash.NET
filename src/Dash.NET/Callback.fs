namespace Dash.NET

open Plotly.NET
open Newtonsoft.Json
open DynamicInvoke

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
///Type to deserialize calls to _dash-update-component
type CallbackRequest =
    {
        [<JsonProperty("output")>]
        Output: string
        [<JsonProperty("outputs")>]
        Outputs: CallbackOutput
        [<JsonProperty("changedPropIds")>]
        ChangedPropIds: string []
        [<JsonProperty("inputs")>]
        Inputs: RequestInput []
    }

//Central type for Callbacks. Creating an instance of this type and registering it on the callback map is the equivalent of the @app.callback decorator in python.
type Callback<'Function> =
    {
        Inputs: CallbackInput []
        Output: CallbackOutput
        HandlerFunction: 'Function
    }
    static member create inputs output (handler: 'Function) =
        {
            Inputs = inputs
            Output = output
            HandlerFunction = handler
        }

    //Necessary as generic types seem not te be unboxed as easily (problems arise e.g. when unboxing (box Callback<string,string>), as the og types used for
    //the generics are missing, therefore obj,obj is assumed and the cast fails)
    static member pack(handler: Callback<'Function>): Callback<obj> =
        Callback.create handler.Inputs handler.Output (box handler.HandlerFunction)

    //returns a boxed result of the dynamic invokation of the handler function
    static member eval (args: seq<obj>) (handler: Callback<'Function>) =
        invokeDynamic<obj> handler.HandlerFunction args

    //returns the result of the dynamic invokation of the handler function casted to the type of choice
    static member evalAs<'OutputType> (args: seq<obj>) (handler: Callback<'Function>) =
        invokeDynamic<'OutputType> handler.HandlerFunction args

    //returns the dash dependency to serve the client on app start via _dash-dependencies´from the given Callback
    static member toDashDependency (handler: Callback<'Function>) : DashDependency = 
        DashDependency.createWithDefaults handler.Inputs (CallbackOutput.toCompositeId handler.Output) 

    //returns the response object to send as response to a request to _dash-update-component that triggered this callback
    static member getResponseObject (args: seq<obj>) (handler: Callback<'Function>) =

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

            result?(handler.Output.Property) <- r
            response?(handler.Output.Id) <- result

            root?multi <- true
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
