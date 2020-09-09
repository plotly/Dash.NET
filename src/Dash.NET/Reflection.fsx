open FSharp
open FSharp.Reflection

type InvokeResult = 
    | InvokeSuccess of obj
    | ObjectWasNotAFunction of System.Type

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


let f (p1:int) = 2 + p1

let f2 (p1:int) (p2:(string*int)) = sprintf "%s:%i" (fst p2) p1

invokeDynamic<string> f2 [box "1"; box ("aa",1)]


type Test<'G> = {
    Field1 : 'G
}

let t = {Field1 = "A"}

let tObj = new System.Collections.Generic.Dictionary<'obj,obj>()

tObj.Add(box "A", box t)

let a : Test<string> = tObj.Item "A" |> unbox



//http://www.fssnip.net/2V/title/Dynamic-operator-using-Reflection