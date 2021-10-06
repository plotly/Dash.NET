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

[<RequireQualifiedAccess>]
module NameCase =
    open Newtonsoft.Json

    let private toCase (ns: Serialization.NamingStrategy) name =
        ns.GetPropertyName(name, false)

    let toSnakeCase name = toCase (Serialization.SnakeCaseNamingStrategy()) name
    let toCamelCase name = toCase (Serialization.CamelCaseNamingStrategy()) name
    let toKebabCase name = toCase (Serialization.KebabCaseNamingStrategy()) name

    let fromDiscriminatedUnion du =
        du
        |> sprintf "%A"
        |> fun s -> s.Split(' ')
        |> Array.rev
        |> Array.head
        |> toKebabCase

    let fromMappedDiscriminatedUnions dus =
        dus |> Map.map (fun _ -> fromDiscriminatedUnion)

    let fromProp prop =
        prop
        |> sprintf "%O"
        |> fun s -> s.Replace('\n', ' ').Split(' ')
        |> Array.head

module Prop =
    let toDynamicMemberPropName prop =
        prop
        |> NameCase.fromProp
        |> NameCase.toSnakeCase
        |> fun s -> s.Replace("class_name", "className")

[<RequireQualifiedAccess>]
module Convert =
    let fromDiscriminatedUnion du =
        du
        |>NameCase.fromDiscriminatedUnion 
        |> box

    let fromMappedDiscriminatedUnions dus =
        dus
        |> NameCase.fromMappedDiscriminatedUnions
        |> box

[<RequireQualifiedAccess>]
module DynObj =
    open DynamicObj

    let private toPropName prop =
        prop
        |> NameCase.fromProp
        |> NameCase.toCamelCase

    let setPropValueOpt props prop convert maybeValue =
        prop
        |> toPropName
        |> fun propName ->
            maybeValue
            |> Option.map (prop >> convert)
            |> DynObj.setValueOpt props propName
