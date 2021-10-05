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

