namespace Dash.NET.CSharp

open System.Runtime.InteropServices


type Dependency = System.ValueTuple<string, ComponentProperty>

type internal Helpers =
    static member internal ConvertDependency ((id, prop) : System.ValueTuple<string, ComponentProperty>) = Dash.NET.Dependency.create (id, prop |> ComponentProperty.Unwrap)

type Callback = private WrappedCallback of Dash.NET.Callback<obj> with

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
            handler: System.Func<'a, Dash.NET.CallbackResultBinding array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
        ) : Callback =
            guardAgainstNull "input" input
            guardAgainstNull "output" output
            guardAgainstNull "handler" handler

            let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
            let preventInitialCall = Option.ofNullable preventInitialCall
            let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox

            let handlerFunction = handler |> FuncConvert.FromFunc >> List.ofArray

            Dash.NET.Callback.multiOut(
                input |> Array.map Helpers.ConvertDependency,
                output |> Array.map Helpers.ConvertDependency,
                box handlerFunction,
                ?State = state,
                ?PreventInitialCall = preventInitialCall,
                ?ClientSideFunction = clientSideFunction
            )
            |> WrappedCallback

    static member Create
        (
            input : Dependency array,
            output : Dependency array,
            handler: System.Func<'a, 'b, Dash.NET.CallbackResultBinding array>,
            [<Optional>] state : Dependency array,
            [<Optional>] preventInitialCall : System.Nullable<bool>,
            [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
        ) : Callback =
            guardAgainstNull "input" input
            guardAgainstNull "output" output
            guardAgainstNull "handlerFunction" handler

            let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
            let preventInitialCall = Option.ofNullable preventInitialCall
            let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox

            let handlerFunction = fun a b -> (FuncConvert.FromFunc handler) a b |> List.ofArray

            Dash.NET.Callback.multiOut(
                input |> Array.map Helpers.ConvertDependency,
                output |> Array.map Helpers.ConvertDependency,
                box handlerFunction,
                ?State = state,
                ?PreventInitialCall = preventInitialCall,
                ?ClientSideFunction = clientSideFunction
            )
            |> WrappedCallback

    ///// <summary>returns a callback that binds a handler function mapping from a single input component to a single output component (1 -> 1)</summary>
    ///// <param name="input"> A `CallbackInput` that represents the input component of this callback. Changes to this component signalled by the client will trigger the callback.</param>
    ///// <param name="output"> A `CallbackOutput` that represents the output component of this callback </param>
    ///// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    ///// <param name="state"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    ///// <param name="preventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    ///// <param name="clientSideFunction"> A client side function to execute with the callback </param>
    //static member singleOut
    //    (
    //        input : Dependency,
    //        output : Dependency,
    //        handlerFunction: 'Function,
    //        [<Optional>] state : Dependency array,
    //        [<Optional>] preventInitialCall : System.Nullable<bool>,
    //        [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
    //    ) =
    //        guardAgainstNull "input" input
    //        guardAgainstNull "output" output
    //        guardAgainstNull "handlerFunction" handlerFunction

    //        let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
    //        let preventInitialCall = Option.ofNullable preventInitialCall
    //        let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox

    //        Dash.NET.Callback.singleOut(
    //            input |> Helpers.ConvertDependency,
    //            output |> Helpers.ConvertDependency,
    //            handlerFunction,
    //            ?State = state,
    //            ?PreventInitialCall = preventInitialCall,
    //            ?ClientSideFunction = clientSideFunction
    //        )
    //        |> WrappedCallback

    ///// <summary>returns a callback that binds a handler function mapping from multiple input components to multiple output components (n -> n)</summary>
    ///// <param name="inputs"> A sequence of `CallbackInput` that represents the input components of this callback. Changes to any of these components signalled by the client will trigger the callback. </param>
    ///// <param name="outputs"> A sequence of `CallbackOutput` that represents the output components of this callback </param>
    ///// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    ///// <param name="state"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    ///// <param name="preventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    ///// <param name="clientSideFunction"> A client side function to execute with the callback </param>
    //static member multiOut
    //    (
    //        inputs : Dependency array,
    //        outputs : Dependency array,
    //        handlerFunction: 'Function,
    //        [<Optional>] state : Dependency array,
    //        [<Optional>] preventInitialCall : System.Nullable<bool>,
    //        [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
    //    ) =
    //        guardAgainstNull "inputs" inputs
    //        guardAgainstNull "outputs" outputs
    //        guardAgainstNull "handlerFunction" handlerFunction
            
    //        let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
    //        let preventInitialCall = Option.ofNullable preventInitialCall
    //        let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox
            
    //        Dash.NET.Callback.multiOut(
    //            inputs |> Array.map Helpers.ConvertDependency,
    //            outputs |> Array.map Helpers.ConvertDependency,
    //            handlerFunction,
    //            ?State = state,
    //            ?PreventInitialCall = preventInitialCall,
    //            ?ClientSideFunction = clientSideFunction
    //        )
    //        |> WrappedCallback

    ///// <summary>returns a callback that binds a handler function mapping from multiple input components to multiple output components (n -> n)</summary>
    ///// <param name="input"> A sequence of `CallbackInput` that represents the input components of this callback. Changes to any of these components signalled by the client will trigger the callback. </param>
    ///// <param name="outputs"> A sequence of `CallbackOutput` that represents the output components of this callback </param>
    ///// <param name="handlerFunction"> The handler function that maps the callback input components to the callback output components </param>
    ///// <param name="state"> A sequence of `CallbackState` that represents additional input components of this callback. In contrast to the other input componenst, these will not trigger the handler function when changed on the client.</param>
    ///// <param name="preventInitialCall"> Wether to prevent the app to call this callback on initialization </param>
    ///// <param name="clientSideFunction"> A client side function to execute with the callback </param>
    //static member multiOut
    //    (
    //        input : Dependency,
    //        outputs : Dependency array,
    //        handlerFunction: 'Function,
    //        [<Optional>] state : Dependency array,
    //        [<Optional>] preventInitialCall : System.Nullable<bool>,
    //        [<Optional>] clientSideFunction : Dash.NET.ClientSideFunction // TODO
    //    ) =
    //        guardAgainstNull "inputs" input
    //        guardAgainstNull "outputs" outputs
    //        guardAgainstNull "handlerFunction" handlerFunction
            
    //        let state = Option.ofObj state |> Option.map (Array.map Helpers.ConvertDependency >> Seq.ofArray)
    //        let preventInitialCall = Option.ofNullable preventInitialCall
    //        let clientSideFunction : Dash.NET.ClientSideFunction option = Option.ofObj (box clientSideFunction) |> unbox
            
    //        Dash.NET.Callback.multiOut(
    //            input |> Helpers.ConvertDependency,
    //            outputs |> Array.map Helpers.ConvertDependency,
    //            handlerFunction,
    //            ?State = state,
    //            ?PreventInitialCall = preventInitialCall,
    //            ?ClientSideFunction = clientSideFunction
    //        )
    //        |> WrappedCallback

    static member Unwrap (v) = match v with | WrappedCallback v -> v