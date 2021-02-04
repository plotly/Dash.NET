namespace Dash.NET

//This module should most likely be its own nuget package, bundled with the dynamic object from Plotly.NET
/// Reflection based dynamic invocation of functions and related helpers
module DynamicInvoke =

    open System
    open FSharp.Reflection

    type InvokeResult =
        | InvokeSuccess of obj
        | ObjectWasNotAFunction of Type

    /// Returns an array of input types (the domain) for the given function type (without the range)
    let getFunctionDomain t =
        let rec loop (f:System.Type) acc =
            if FSharpType.IsFunction f then
                let firstArg, rest = FSharpType.GetFunctionElements f
                loop rest (firstArg::acc)
            else
                acc
        loop t []
        |> List.rev

    /// Returns an array of input types (the domain) for the given function type (without the range)
    let getFunctionRange t =
        let rec loop (f:System.Type) =
            if FSharpType.IsFunction f then
                let firstArg, rest = FSharpType.GetFunctionElements f
                loop rest
            else
                f
        loop t

    /// Returns true if the given type implements the IConvertible interface
    let isIConvertible (t:Type) =
        t.GetInterfaces()
        |> Array.exists (fun t -> t = typeof<IConvertible>)

    /// Returns true if the given type implements the IEnumerable interface
    let isSeq (t:Type) =
        t.GetInterfaces()
        |> Array.exists (fun t -> t = typeof<System.Collections.IEnumerable>)

    /// Returns true if the given type implements the IEnumerable interface and the type contained in the collection implements the IConvertible interface
    let isIConvertibleSeq (t:Type) =
        if isSeq t then
            let genericArgs = t.GetGenericArguments()
            if genericArgs.Length > 0 then
                isIConvertible genericArgs.[0]
            else
                false
        else
            false

    //This function is (as far as i see it) a 'necessary evil' for solving the problem of callbacks having arbitrary amounts of parameters.
    //However, just like DynamicObj in Plotly.NET, it is definately usable when correctly encapsulated to prevent direct usage.
    
    ///<summary>Invokes the given function with the given arguments (passed as boxed values) </summary>
    ///<param name="fn">an obj type representing the function to dynamically invoke. Internally a check is performed if it is a FSharp function - and if true - it will be consequitively be invoked with the arguments provided by args</param>
    ///<param name="args">a sequence or boxed function arguments</param>
    let invokeDynamic<'FunctionResult> (fn: obj) (args: obj seq): Result<'FunctionResult, System.Exception> =
        let rec dynamicFunctionInternal (next: obj) (args: obj list): InvokeResult =
            match args.IsEmpty with
            | false ->
                let fType = next.GetType()

                if FSharpType.IsFunction fType then

                    let (head, tail) = (args.Head, args.Tail)

                    let methodInfo =
                        fType.GetMethods()
                        |> Seq.filter (fun x -> x.Name = "Invoke" && x.GetParameters().Length = 1)
                        |> Seq.head

                    let partialResult = methodInfo.Invoke(next, [| head |])
                    dynamicFunctionInternal partialResult tail
                else
                    ObjectWasNotAFunction fType
            | true -> InvokeSuccess(next)

        match dynamicFunctionInternal fn (args |> List.ofSeq) with
        | InvokeSuccess r -> Ok(r |> unbox<'FunctionResult>)
        | ObjectWasNotAFunction t -> Error(new System.Exception(sprintf "The type %s is not a function" t.FullName))
