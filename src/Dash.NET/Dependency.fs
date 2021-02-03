namespace Dash.NET

open Plotly.NET
open Newtonsoft.Json
open System

type Dependency =
    {
        [<JsonProperty("id")>]
        Id: string
        [<JsonProperty("property")>]
        Property: string
    }
    static member create(id:string, property) = 

        if (Char.IsUpper(id.[0]) || id.Contains(".") || id = "") then
            failwithf "Callback IDs must be lowercase, (non-empty) character strings that do not contain one or more dots/periods. Please verify that the component ID is valid."
        else
            { Id = id; Property = property }

    static member create(id, property:ComponentProperty) = {Id = id; Property = ComponentProperty.toPropertyName property}
    
    static member toCompositeId (d:Dependency) = sprintf "%s.%s" d.Id d.Property

    static member toMultiCompositeId (d:seq<Dependency>) =
        d
        |> Seq.map Dependency.toCompositeId
        |> String.concat "..."
        |> sprintf "..%s.."

    static member ofList (dict:seq<string*string>) =
        dict
        |> Seq.map Dependency.create

type CallbackInput = Dependency
type CallbackOutput = Dependency
type CallbackState = Dependency
