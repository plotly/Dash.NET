namespace Dash.NET.Common

[<RequireQualifiedAccess>]
module Json =

    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization

    let mkSerializerSettings () =
        JsonSerializerSettings(
            ContractResolver = DefaultContractResolver(NamingStrategy = new DefaultNamingStrategy())
        )

