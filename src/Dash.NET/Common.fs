namespace Dash.NET.Common

[<RequireQualifiedAccess>]
module Json =
    open System
    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization

    type OptionConverter<'T> () =
        inherit JsonConverter<'T option> ()
        override _.WriteJson(writer: JsonWriter, value: 'T option, serializer: JsonSerializer) =
            match value with
            | Some v -> serializer.Serialize(writer, v);
            | None -> writer.WriteNull()
        override _.ReadJson(reader: JsonReader, objectType: Type, existingValue: 'T option, _ : bool, serializer: JsonSerializer) =
            raise <| new NotImplementedException()

    let mkSerializerSettings () =
        JsonSerializerSettings(
            ContractResolver = DefaultContractResolver(NamingStrategy = new DefaultNamingStrategy()),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        )

module NamingStrategy =
    open Newtonsoft.Json

    let private toCase (ns: Serialization.NamingStrategy) name =
        ns.GetPropertyName(name, false)

    let toSnakeCase name = toCase (Serialization.SnakeCaseNamingStrategy()) name
    let toCamelCase name = toCase (Serialization.CamelCaseNamingStrategy()) name
    let toKebabCase name = toCase (Serialization.KebabCaseNamingStrategy()) name

[<RequireQualifiedAccess>]
module DU =
    let private toString du =
        du
        |> sprintf "%A"
        |> NamingStrategy.toKebabCase

    let convertAsString du =
        du |> toString |> box

    let convertMapped convert =
        Map.map (fun _ -> convert) >> box

[<RequireQualifiedAccess>]
module Prop =
    let createName prop =
        prop
        |> sprintf "%O"
        |> fun s -> s.Replace('\n', ' ').Split(' ')
        |> Array.head

[<RequireQualifiedAccess>]
module DynObj =
    open DynamicObj

    let private toPropName prop =
        prop
        |> Prop.createName
        |> NamingStrategy.toCamelCase

    let setPropValueOpt dynObj convert prop maybeValue =
        prop
        |> toPropName
        |> fun propName ->
            maybeValue
            |> Option.map (prop >> convert)
            |> DynObj.setValueOpt dynObj propName

    let setDUValueOpt dynObj convert dynPropName maybeValue =
        maybeValue
        |> Option.map convert
        |> DynObj.setValueOpt dynObj dynPropName

