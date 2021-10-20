module Map

let fromDictionary (dic : System.Collections.Generic.Dictionary<_,_>) =
    dic
    |> Seq.map(|KeyValue|)
    |> Map.ofSeq