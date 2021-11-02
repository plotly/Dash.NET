module Dash.NET.DashTable.Common

open DynamicObj

let [<Literal>] CdnLink = "https://unpkg.com/dash-table@4.12.1/dash_table/bundle.js"

let internal replaceAt targetIndex p =
    List.mapi (fun i v -> if i = targetIndex then p else v)

let internal setValue dynObj name p =
    p |> DynObj.setValue dynObj name
    dynObj

let internal withOptValue set maybeValue dynObj =
    match maybeValue with
    | Some value -> value |> set dynObj
    | None -> dynObj


