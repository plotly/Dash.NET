namespace Dash.NET

//This module should most likely be its own nuget package, bundled with the dynamic object from FSharp.Plotly

module DynamicInvoke =

    open System
    open FSharp.Reflection 
    
    type InvokeResult = 
        | InvokeSuccess of obj
        | ObjectWasNotAFunction of Type
    
    let invokeDynamic<'FunctionResult> (fn:obj) (args:obj seq) : Result<'FunctionResult,System.Exception> =
        let rec dynamicFunctionInternal (next:obj) (args:obj list) : InvokeResult =
            match args.IsEmpty with
            | false ->
                let fType = next.GetType()
                if FSharpType.IsFunction fType then
                    let (head, tail) = (args.Head, args.Tail)
                    let methodInfo = 
                        fType.GetMethods()
                        |> Seq.filter (fun x -> x.Name = "Invoke" && x.GetParameters().Length = 1)
                        |> Seq.head
                    let partalResult = methodInfo.Invoke(next, [| head |])
                    dynamicFunctionInternal partalResult tail
                else ObjectWasNotAFunction fType
            | true ->
                InvokeSuccess(next)
        
        match dynamicFunctionInternal fn (args |> List.ofSeq ) with
        | InvokeSuccess r           -> Ok (r |> unbox<'FunctionResult>)
        | ObjectWasNotAFunction t   -> Error (new System.Exception(sprintf "The type %s is not a function" t.FullName))
    