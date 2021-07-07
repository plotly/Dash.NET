module Dash.NET.ComponentGeneration.Prelude

open System
open System.IO
open System.Text.RegularExpressions

let validDULabel = Regex "^[ABCDEFGHIJKLMNOPQRSTUVWXYZ].*"

module String =
    let replace (old: string) (_new: string) (s: string) = s.Replace(old,_new)
    let write path (s: string) = File.WriteAllText(path,s)

    let firstLetter (s: string) = s.ToLower().Substring(0,1)
    let decapitalize (s: string) = (sprintf "%c%s" (Char.ToLowerInvariant(s.[0])) (s.Substring(1)))
    let capitalize (s: string) = (sprintf "%c%s" (Char.ToUpperInvariant(s.[0])) (s.Substring(1)))

    let escape (s: string) = s.Replace("\\","\\\\").Replace("\"","\\\"")

    //DU labels have to start with a capital, if a property/value name starts with an _ or other non-letter character
    //then we have to add a letter in front of it
    //TODO There are other rules to DU names that don't apply to normal variable names (like no control characters like " and \)
    let toValidDULabel (s: string) = 
        let capitalizedLabel = s |> capitalize
        if validDULabel.IsMatch(capitalizedLabel) then capitalizedLabel
        else sprintf "Prop%s" s

