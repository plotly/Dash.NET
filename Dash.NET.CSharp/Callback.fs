namespace Dash.NET.CSharp

open System.Runtime.InteropServices


type Dependency = System.ValueTuple<string, ComponentProperty>

type internal Helpers =
    static member internal ConvertDependency ((id, prop) : System.ValueTuple<string, ComponentProperty>) = Dash.NET.Dependency.create (id, prop |> ComponentProperty.Unwrap)

type CallbackResult = internal WrappedCallbackResult of Dash.NET.CallbackResultBinding with
    static member Create<'a>(target : Dependency, result : 'a) =
        {
            Dash.NET.CallbackResultBinding.Target = target |> Helpers.ConvertDependency
            Dash.NET.CallbackResultBinding.BoxedResult = box result
        }
        |> WrappedCallbackResult

    static member internal Unwrap (value : CallbackResult) : Dash.NET.CallbackResultBinding = match value with | WrappedCallbackResult v -> v

type ClientSideFunction = internal WrappedClientSideFunction of Dash.NET.ClientSideFunction with
    static member Create(Namespace : string, FunctionName : string) =
        {
            Dash.NET.ClientSideFunction.Namespace = Namespace
            Dash.NET.ClientSideFunction.FunctionName = FunctionName
        }
        |> WrappedClientSideFunction

    static member internal Unwrap (value : ClientSideFunction) : Dash.NET.ClientSideFunction = match value with | WrappedClientSideFunction v -> v


type Callback = private WrappedCallback of Dash.NET.Callback<obj> with

    static member Unwrap (v) = match v with | WrappedCallback v -> v // Can't be internal because accessed by Dash.NET.CSharp.Giraffe

    ///// <summary>returns a callback that binds a handler function mapping from multiple input components to a single output component (n -> 1)</summary>
    ///// <param name="input"> A sequence of `CallbackInput` that represents the input components of this callback. Changes to any of these components signalled by the client will trigger the callback. </param>
    ///// <param name="output"> A `CallbackOutput` that represents the output component of this callback </param>
    ///// <param name="handler"> The handler function that maps the callback input components to the callback output components </param>
    ///// <param name="state"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    ///// <param name="preventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    ///// <param name="clientSideFunction"> A client side function to execute with the callback </param>
    //static member Create
    //    (
    //        input : Dependency array,
    //        output : Dependency array,
    //        handler: System.Func<'a, CallbackResult array>,
    //        [<Optional>] state : Dependency array,
    //        [<Optional>] preventInitialCall : System.Nullable<bool>,
    //        [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
    //    ) : Callback =
    //        guardAgainstNull "input" input
    //        guardAgainstNull "output" output
    //        guardAgainstNull "handler" handler

    //        let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
    //        let preventInitialCall = Option.ofNullable preventInitialCall
    //        let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox

    //        let handlerFunction = fun a -> (FuncConvert.FromFunc handler) a |> List.ofArray |> List.map CallbackResult.Unwrap

    //        Dash.NET.Callback.multiOut(
    //            input |> Array.map Helpers.ConvertDependency,
    //            output |> Array.map Helpers.ConvertDependency,
    //            box handlerFunction,
    //            ?State = state,
    //            ?PreventInitialCall = preventInitialCall,
    //            ?ClientSideFunction = clientSideFunction
    //        )
    //        |> WrappedCallback

    //static member Create
    //    (
    //        input : Dependency array,
    //        output : Dependency array,
    //        handler: System.Func<'a, 'b, CallbackResult array>,
    //        [<Optional>] state : Dependency array,
    //        [<Optional>] preventInitialCall : System.Nullable<bool>,
    //        [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
    //    ) : Callback =
    //        guardAgainstNull "input" input
    //        guardAgainstNull "output" output
    //        guardAgainstNull "handlerFunction" handler

    //        let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
    //        let preventInitialCall = Option.ofNullable preventInitialCall
    //        let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox

    //        let handlerFunction = fun a b -> (FuncConvert.FromFunc handler) a b |> List.ofArray |> List.map CallbackResult.Unwrap

    //        Dash.NET.Callback.multiOut(
    //            input |> Array.map Helpers.ConvertDependency,
    //            output |> Array.map Helpers.ConvertDependency,
    //            box handlerFunction,
    //            ?State = state,
    //            ?PreventInitialCall = preventInitialCall,
    //            ?ClientSideFunction = clientSideFunction
    //        )
    //        |> WrappedCallback

    /// <summary>returns a callback that binds a handler function mapping from multiple input components to a single output component (n -> 1)</summary>
    /// <param name="input"> A sequence of `CallbackInput` that represents the input components of this callback. Changes to any of these components signalled by the client will trigger the callback. </param>
    /// <param name="output"> A `CallbackOutput` that represents the output component of this callback </param>
    /// <param name="handler"> The handler function that maps the callback input components to the callback output components </param>
    /// <param name="state"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    /// <param name="preventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    /// <param name="clientSideFunction"> A client side function to execute with the callback </param>
    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =

            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a -> (FuncConvert.FromFunc handler) a |> List.ofArray |> List.map CallbackResult.Unwrap) |> box

            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction

            let handlerFunction = (fun a b -> (FuncConvert.FromFunc handler) a b |> List.ofArray |> List.map CallbackResult.Unwrap) |> box

            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction
            
    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c -> (FuncConvert.FromFunc handler) a b c |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
        
            let handlerFunction = (fun a b c d -> (FuncConvert.FromFunc handler) a b c d |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
        
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction
            
    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
                    
            let handlerFunction = (fun a b c d e -> (FuncConvert.FromFunc handler) a b c d e |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
                    
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
                
            let handlerFunction = (fun a b c d e f -> handler.Invoke(a, b, c, d, e, f) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
                
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c d e f g -> handler.Invoke(a, b, c, d, e, f, g) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c d e f g h -> handler.Invoke(a, b, c, d, e, f, g, h) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    
    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h, 'i, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c d e f g h i -> handler.Invoke(a, b, c, d, e, f, g, h, i) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h, 'i, 'j, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c d e f g h i j -> handler.Invoke(a, b, c, d, e, f, g, h, i, j) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h, 'i, 'j, 'k, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c d e f g h i j k -> handler.Invoke(a, b, c, d, e, f, g, h, i, j, k) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h, 'i, 'j, 'k, 'l, CallbackResult array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : ClientSideFunction
        ) : Callback =
            let state, preventInitialCall, clientSideFunction = Callback.HandleInput input output (box handler) state preventInitialCall clientSideFunction
            
            let handlerFunction = (fun a b c d e f g h i j k l -> handler.Invoke(a, b, c, d, e, f, g, h, i, j, k, l) |> List.ofArray |> List.map CallbackResult.Unwrap) |> box
            
            Callback.CreateCallback input output state preventInitialCall clientSideFunction handlerFunction

    // Private helpers

    static member private HandleInput input output handler state preventInitialCall clientSideFunction =
        guardAgainstNull "input" input
        guardAgainstNull "output" output
        guardAgainstNull "handlerFunction" handler

        let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
        let preventInitialCall = Option.ofNullable preventInitialCall
        let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox |> Option.map ClientSideFunction.Unwrap

        state, preventInitialCall, clientSideFunction

    static member private CreateCallback input output state preventInitialCall clientSideFunction (handlerFunction : obj) =
        Dash.NET.Callback.multiOut(
            input |> Array.map Helpers.ConvertDependency,
            output |> Array.map Helpers.ConvertDependency,
            handlerFunction,
            ?State = state,
            ?PreventInitialCall = preventInitialCall,
            ?ClientSideFunction = clientSideFunction
        )
        |> WrappedCallback

