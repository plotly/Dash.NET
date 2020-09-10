namespace Dash.NET

module Callbacks =

    open FSharp.Plotly
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

    type Input = Dependency
    type Output = Dependency
    type State = Dependency

    type ClientSideFunction =
        {
            [<JsonProperty("namespace")>]
            Namespace: string
            [<JsonProperty("function_name")>]
            FunctionName: string
        }

    //This is the type that will be serialized and served on the _dash-dependencies endpoint to define callback bhaviour. Naming could be improved
    //To-Do: autogenerate this from a registered callback handler and add to app Callback Dependency list to automatically serve on app start
    type Callback =
        {
            [<JsonProperty("prevent_initial_call")>]
            PreventInitialCall: bool
            [<JsonProperty("clientside_function")>]
            ClientsideFunction: ClientSideFunction option
            [<JsonProperty("inputs")>]
            Inputs: Input []
            [<JsonProperty("output")>]
            Output: string
            [<JsonProperty("state")>]
            State: State []
        }
        static member create preventInitialCall clientsideFunction inputs output state =
            {
                PreventInitialCall = preventInitialCall
                ClientsideFunction = clientsideFunction
                Inputs = inputs
                Output = output
                State = state
            }

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
            Outputs: Output
            [<JsonProperty("changedPropIds")>]
            ChangedPropIds: string []
            [<JsonProperty("inputs")>]
            Inputs: RequestInput []
        }

    //Central type for Callbacks. Creating an instance of this type and registering it on the callback map is the equivalent of the @app.callback decorator in python.
    type CallbackHandler<'Function> =
        {
            Inputs: Input []
            Output: Output
            HandlerFunction: 'Function
        }
        static member create inputs output (handler: 'Function) =
            {
                Inputs = inputs
                Output = output
                HandlerFunction = handler
            }

        //Necessary as generic types seem not te be unboxed as easily (problems arise e.g. when unboxing (box CallbackHandler<string,string>), as the og types used for
        //the generics are missing, therefore obj,obj is assumed and the cast fails)
        static member pack(handler: CallbackHandler<'Function>): CallbackHandler<obj> =
            CallbackHandler.create handler.Inputs handler.Output (box handler.HandlerFunction)

        //returns a boxed result of the dynamic invokation of the handler function
        static member eval (args: seq<obj>) (handler: CallbackHandler<'Function>) =
            invokeDynamic<obj> handler.HandlerFunction args

        //returns the result of the dynamic invokation of the handler function casted to the type of choice
        static member evalAs<'OutputType> (args: seq<obj>) (handler: CallbackHandler<'Function>) =
            invokeDynamic<'OutputType> handler.HandlerFunction args

        //returns the response object to send as response to a request to _dash-update-component that triggered this callback
        static member getResponseObject (args: seq<obj>) (handler: CallbackHandler<'Function>) =

            let evalResult =
                handler
                |> CallbackHandler.pack
                |> CallbackHandler.eval args

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

        static member registerCallbackHandler
            (callbackId: string)
            (callbackHandler: CallbackHandler<'Function>)
            (callbackMap: CallbackMap)
            =
            callbackMap?(callbackId) <- (CallbackHandler.pack callbackHandler)
            callbackMap

        static member unregisterCallbackHandler (callbackId: string) (callbackMap: CallbackMap) =
            match (callbackMap.TryGetTypedValue<CallbackHandler<obj>> callbackId) with
            | Some _ ->
                callbackMap.Remove(callbackId) |> ignore
                callbackMap
            | None -> callbackMap

        static member getPackedCallbackHandlerById (callbackId: string) (callbackMap: CallbackMap)
            : CallbackHandler<obj>
            =
            match (callbackMap.TryGetTypedValue<CallbackHandler<obj>> callbackId) with
            | Some cHandler -> cHandler
            | None -> failwithf "No callback handler registered for id %s" callbackId
