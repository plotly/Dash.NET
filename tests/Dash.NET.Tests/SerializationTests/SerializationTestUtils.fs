module SerializationTestUtils

// Serialization tests for common component props
open Expecto
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

[<AutoOpen>]
module SerializationSettings =
    let settings = Dash.NET.Common.Json.mkSerializerSettings()
    let json o = JsonConvert.SerializeObject(o, settings)
    let unjson<'T> str = JsonConvert.DeserializeObject<'T>(str,settings)
