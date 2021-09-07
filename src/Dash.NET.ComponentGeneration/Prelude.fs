module Dash.NET.ComponentGeneration.Prelude

open System
open System.IO
open System.Text.RegularExpressions

//Bind two Async<bool>'s
let (|@!>) (c1: Async<bool>) (c2: Async<bool>) =
    async {
        let! success = c1
        let! newSuccess = c2
        return newSuccess && success
    }

//Bind two Async<bool>'s, but only if the first one succeeded
let (|@>) (c1: Async<bool>) (c2: Async<bool>) =
    async {
        let! success = c1
        if success then
            return! c2
        else
            return success
    }

let validDULabel = Regex "^[ABCDEFGHIJKLMNOPQRSTUVWXYZ].*"

module Option =
    let bindNone (ifNone: 'a option) (op: 'a option) =
        match op with
        | Some _ -> op
        | None -> ifNone

module String =
    let replace (old: string) (_new: string) (s: string) = s.Replace(old,_new)
    let split (on: string) (s: string) = s.Split(on) |> List.ofArray
    let write path (s: string) = File.WriteAllText(path,s)

    let firstLetter (s: string) = s.ToLower().Substring(0,1)
    let decapitalize (s: string) = (sprintf "%c%s" (Char.ToLowerInvariant(s.[0])) (s.Substring(1)))
    let capitalize (s: string) = (sprintf "%c%s" (Char.ToUpperInvariant(s.[0])) (s.Substring(1)))

    let toPascalCase (s:string) =
        s
        |> split "_"
        |> List.map capitalize
        |> (function
            | [] -> ""
            | [s] -> s
            | l -> List.reduce (sprintf "%s%s") l)

    let removeQuotes (s: string) = 
        if Regex.IsMatch(s, "^(['\"`]).*(['\"`])$") then
            s.Substring(1,s.Length - 2)
        else if Regex.IsMatch(s, "^(\\\").*(\\\")$") then
            s.Substring(2,s.Length - 4)
        else
            s

    let escape = replace @"\" @"\\"

    //DU labels have to start with a capital, if a property/value name starts with an _ or other non-letter character
    //then we have to add a letter in front of it
    let toValidDULabel (s: string) = 
        let capitalizedLabel = s |> toPascalCase
        if validDULabel.IsMatch(capitalizedLabel) then capitalizedLabel
        else sprintf "Prop%s" s
        |> replace @"\" ""
        |> replace "\"" ""
        |> replace "+" "Plus"

    let matches (reg: string) (s: string) = Regex.IsMatch(s, reg)

module List =
    let zip4 l1 l2 l3 l4 =
        (List.zip l1 l2, List.zip l3 l4)
        ||> List.zip
        |> List.map (fun ((a1,a2),(a3,a4)) -> (a1,a2,a3,a4))


