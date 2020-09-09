namespace Dash.NET

module Callbacks =
    
    open FSharp.Plotly
    open Newtonsoft.Json
    open DynamicInvoke

    type Dependency = {
        id      : string
        property: string
    } with
        static member create(id,property) = {id = id; property = property}

    type Input     = Dependency
    type Output    = Dependency
    type State     = Dependency


    type ClientSideFunction = {
        [<JsonProperty("namespace")>]
        _namespace      : string
        function_name   : string
    }

    type Callback = {
        prevent_initial_call    : bool
        clientside_function     : ClientSideFunction option
        inputs                  : Input []
        output                  : string
        state                   : State []
    } with
        static member create pic cf i o s =
            {
                prevent_initial_call    = pic
                clientside_function     = cf
                inputs                  = i
                output                  = o
                state                   = s
            }

    type RequestInput = {
        id      : string
        property: string
        value   : obj
    }

    type CallbackRequest = {
        output          : string
        outputs         : Output
        changedPropIds  : string []
        inputs          : RequestInput []
    } 

    open FSharp
    open FSharp.Reflection

    type CallbackHandler<'Inputs,'InnerSignature> = {
        Inputs          : Input []
        Output          : Output
        HandlerFunction : 'Inputs -> 'InnerSignature
    } with
        static member create inputs output (handler:'Inputs -> 'InnerSignature) =
            {
                Inputs          = inputs
                Output          = output
                HandlerFunction = handler
            }

        static member eval (handler:CallbackHandler<'Inputs,'InnerSignature>) (args:seq<obj>) =
            invokeDynamic<obj> handler.HandlerFunction args

        static member evalAs<'OutputType> (handler:CallbackHandler<'Inputs,'InnerSignature>) (args:seq<obj>) =
            invokeDynamic<'OutputType> handler.HandlerFunction args

        static member getResponseObject (handler:CallbackHandler<'Inputs,'InnerSignature>) (args:seq<obj>) =
            let evalResult = CallbackHandler.eval handler args

            match evalResult with
            | Ok r ->
                let root        = DynamicObj()
                let response    = DynamicObj()
                let result      = DynamicObj()

                result?(handler.Output.property) <- r
                response?(handler.Output.id) <- result

                root?multi      <- true
                root?response   <- response

                root
                
            | Error e -> failwith e.Message