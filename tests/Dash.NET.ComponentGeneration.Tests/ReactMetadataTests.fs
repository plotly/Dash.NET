module Dash.Net.ComponentGeneration.Tests.ReactMetadataTests

open Expecto
open System.Text.Json
open Dash.NET.ComponentGeneration.ReactMetadata
open System
open System.Collections.Generic

//TODO parse invalid 

[<Tests>]
let tests =
    testList "React Metadata Tests" [
        testList "React Prop Type" [
            testCase "Parse Array Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "array",
                        "value": "[1, 2, 3]",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Array (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some  "[1, 2, 3]") 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Bool Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "bool",
                        "value": true,
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Bool (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some true) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Number Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "number",
                        "value": 5.6,
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Number (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some (5.6 :> IConvertible)) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse String Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "string",
                        "value": "test",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | String (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some "test") 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Object Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "object",
                        "value": "{\"test\" : \"test\"}",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Object (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Any Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "any",
                        "value": "{\"test\" : \"test\"}",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Any (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Element Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "element",
                        "value": "{\"test\" : \"test\"}",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Element (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Node Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "node",
                        "value": "{\"test\" : \"test\"}",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Node (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, Some "{\"test\" : \"test\"}") 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            // React PropTypes

            testCase "Parse Enum Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "enum",
                        "value": [
                            {
                              "value": "'e1'",
                              "computed": false
                            },
                            {
                              "value": "'e2'",
                              "computed": false
                            },
                            {
                              "value": "'e3'",
                              "computed": false
                            }
                        ],
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Enum (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, 
                             [ SafeReactPropType.Any ({ computed = Some false; required = None; description = None }, Some "e1")
                               SafeReactPropType.Any ({ computed = Some false; required = None; description = None }, Some "e2")
                               SafeReactPropType.Any ({ computed = Some false; required = None; description = None }, Some "e3") ] |> Some ) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Union Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "union",
                        "value": [
                            {
                                "name": "bool"
                            },
                            {
                                "name": "string"
                            },
                            {
                                "name": "number"
                            }
                        ],
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Union (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, 
                             [ SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None)
                               SafeReactPropType.String ({ computed = None; required = None; description = None }, None)
                               SafeReactPropType.Number ({ computed = None; required = None; description = None }, None) ] |> Some ) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse ArrayOf Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "arrayOf",
                        "value": {
                            "name": "bool"
                        },
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | ArrayOf (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, 
                             SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None) |> Some ) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse ObjectOf Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "objectOf",
                        "value": {
                            "name": "bool"
                        },
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | ObjectOf (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, 
                             SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None) |> Some ) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Shape Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "shape",
                        "value": {
                            "p1": {
                                "name": "bool",
                                "description": "test",
                                "required": false
                            },
                            "p2": {
                                "name": "string",
                                "description": "test",
                                "required": false
                            },
                            "p3": {
                                "name": "number",
                                "description": "test",
                                "required": false
                            }
                        },
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                let dict =
                    [ KeyValuePair("p1", SafeReactPropType.Bool ({ computed = None; required = Some false; description = Some "test" }, None))
                      KeyValuePair("p2", SafeReactPropType.String ({ computed = None; required = Some false; description = Some "test" }, None))
                      KeyValuePair("p3", SafeReactPropType.Number ({ computed = None; required = Some false; description = Some "test" }, None)) ]
                    |> Dictionary

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Shape (p, v) -> 
                        Expect.equal p { computed = Some false; required = Some true; description = Some "test" } "Json does not correctly deserialize all properties"
                        Expect.equal (v |> Option.map List.ofSeq) (dict |> List.ofSeq |> Some) "Json does not correctly deserialize all properties"  
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Exact Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "exact",
                        "value": {
                            "p1": {
                                "name": "bool",
                                "required": false
                            },
                            "p2": {
                                "name": "string",
                                "required": false
                            },
                            "p3": {
                                "name": "number",
                                "required": false
                            }
                        },
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                let dict =
                    [ KeyValuePair("p1", SafeReactPropType.Bool ({ computed = None; required = Some false; description = None }, None))
                      KeyValuePair("p2", SafeReactPropType.String ({ computed = None; required = Some false; description = None }, None))
                      KeyValuePair("p3", SafeReactPropType.Number ({ computed = None; required = Some false; description = None }, None)) ]
                    |> Dictionary

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType false
                |> function 
                    | Exact (p, v) -> 
                        Expect.equal p { computed = Some false; required = Some true; description = Some "test" } "Json does not correctly deserialize all properties"
                        Expect.equal (v |> Option.map List.ofSeq) (dict |> List.ofSeq |> Some) "Json does not correctly deserialize all properties"  
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Flow Union Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "union",
                        "raw": "bool | string | number",
                        "elements": [
                            { "name": "bool" },
                            { "name": "string" },
                            { "name": "number" }
                        ],
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType true
                |> function 
                    | FlowUnion (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, 
                             [ SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None)
                               SafeReactPropType.String ({ computed = None; required = None; description = None }, None)
                               SafeReactPropType.Number ({ computed = None; required = None; description = None }, None) ] |> Some ) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Flow Array Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "Array",
                        "elements": [{ "name": "bool" }],
                        "raw": "Array<bool>",
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType true
                |> function 
                    | FlowArray (p, v) -> 
                        Expect.equal 
                            (p, v) 
                            ({ computed = Some false; required = Some true; description = Some "test" }, 
                             SafeReactPropType.Bool ({ computed = None; required = None; description = None }, None) |> Some ) 
                            "Json does not correctly deserialize all properties"
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

            testCase "Parse Flow Object Signature Prop Type" <| fun _ ->
                let json =
                    """
                    {
                        "name": "signature",
                        "type": "object",
                        "raw": "{ p1: bool, p2: string, p2: number }",
                        "signature": {
                            "properties": [
                                {
                                    "key": "p1",
                                    "value": {
                                        "name": "boolean",
                                        "nullable": true,
                                        "required": false
                                    }
                                },
                                {
                                    "key": "p2",
                                    "value": {
                                        "name": "string",
                                        "nullable": true,
                                        "required": false
                                    }
                                },
                                {
                                    "key": "p3",
                                    "value": {
                                        "name": "number",
                                        "nullable": true,
                                        "required": false
                                    }
                                }
                            ]
                        },
                        "description": "test",
                        "required": true,
                        "computed": false
                    }
                    """

                let dict =
                    [ KeyValuePair("p1", SafeReactPropType.Bool ({ computed = None; required = Some false; description = None }, None))
                      KeyValuePair("p2", SafeReactPropType.String ({ computed = None; required = Some false; description = None }, None))
                      KeyValuePair("p3", SafeReactPropType.Number ({ computed = None; required = Some false; description = None }, None)) ]
                    |> Dictionary

                JsonSerializer.Deserialize<ReactPropType>(json, jsonOptions)
                |> SafeReactPropType.fromReactPropType true
                |> function 
                    | FlowObject (p, v) -> 
                        Expect.equal p { computed = Some false; required = Some true; description = Some "test" } "Json does not correctly deserialize all properties"
                        Expect.equal (v |> Option.map List.ofSeq) (dict |> List.ofSeq |> Some) "Json does not correctly deserialize all properties"  
                    | _ ->
                        Expect.isTrue false "Json does not deserialize to the correct type"

        ]

        testList "React Prop" [
            testCase "Parse React Prop" <| fun _ ->
                let json =
                    """
                    {
                        "type": {
                            "name": "bool"
                        },
                        "required": false,
                        "description": "test",
                        "defaultValue": {
                            "value": "false",
                            "computed": false
                        }

                    }
                    """

                let expectedProp =
                    { propType = Bool ({ computed = None; required = None; description = None }, None) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = Any ({ computed = Some false; required = None; description = None }, Some "false") |> Some }

                JsonSerializer.Deserialize<ReactProp>(json, jsonOptions)
                |> SafeReactProp.fromReactProp
                |> (fun prop -> Expect.equal prop expectedProp "Json does not correctly deserialize all properties")

            testCase "Parse React Flow Prop" <| fun _ ->
                let json =
                    """
                    {
                        "flowType": {
                            "name": "bool"
                        },
                        "required": false,
                        "description": "test",
                        "defaultValue": {
                            "value": "false",
                            "computed": false
                        }

                    }
                    """

                let expectedProp =
                    { propType = Bool ({ computed = None; required = None; description = None }, None) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = Any ({ computed = Some false; required = None; description = None }, Some "false") |> Some }

                JsonSerializer.Deserialize<ReactProp>(json, jsonOptions)
                |> SafeReactProp.fromReactProp
                |> (fun prop -> Expect.equal prop expectedProp "Json does not correctly deserialize all properties")

            testCase "Parse React Typescript Prop" <| fun _ ->
                let json =
                    """
                    {
                        "tsType": {
                            "name": "bool"
                        },
                        "required": false,
                        "description": "test",
                        "defaultValue": {
                            "value": "false",
                            "computed": false
                        }

                    }
                    """

                let expectedProp =
                    { propType = Bool ({ computed = None; required = None; description = None }, None) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = Any ({ computed = Some false; required = None; description = None }, Some "false") |> Some }

                JsonSerializer.Deserialize<ReactProp>(json, jsonOptions)
                |> SafeReactProp.fromReactProp
                |> (fun prop -> Expect.equal prop expectedProp "Json does not correctly deserialize all properties")
        ]

        testList "React Component" [
            testCase "Parse React Prop" <| fun _ ->
                let json =
                    """
                    {
                        "description": "test",
                        "displayName": "test",
                        "methods": [],
                        "props": {
                            "p1": {
                                "type": {
                                    "name": "bool"
                                },
                                "required": false,
                                "description": "test"
                            },
                            "p2": {
                                "type": {
                                    "name": "bool"
                                },
                                "required": false,
                                "description": "test"
                            }
                        }
                    }
                    """

                let expectedProp =
                    { propType = Bool ({ computed = None; required = None; description = None }, None) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                let expectedProps =
                    [ KeyValuePair("p1", expectedProp)
                      KeyValuePair("p2", expectedProp) ]
                    |> Dictionary

                JsonSerializer.Deserialize<ReactComponent>(json, jsonOptions)
                |> SafeReactComponent.fromReactComponent
                |> (fun comp -> 
                    Expect.equal comp.description (Some "test") "Json does not correctly deserialize all properties"
                    Expect.equal comp.displayName (Some "test") "Json does not correctly deserialize all properties"
                    Expect.equal (comp.props |> List.ofSeq) (expectedProps |> List.ofSeq) "Json does not correctly deserialize all properties" )
        ]

        testList "React Metadata" [
            testCase "Parse Metadata json" <| fun _ ->
                let json =
                    """
                    {
                        "Test.react.js": {
                            "description": "test",
                            "displayName": "test",
                            "methods": [],
                            "props": {
                                "p1": {
                                    "type": {
                                        "name": "bool"
                                    },
                                    "required": false,
                                    "description": "test"
                                },
                                "p2": {
                                    "type": {
                                        "name": "bool"
                                    },
                                    "required": false,
                                    "description": "test"
                                }
                            }
                        }
                    }
                    """

                let expectedProp =
                    { propType = Bool ({ computed = None; required = None; description = None }, None) |> Some
                      required = Some false
                      description = Some "test"
                      defaultValue = None }

                let expectedProps =
                    [ KeyValuePair("p1", expectedProp)
                      KeyValuePair("p2", expectedProp) ]
                    |> Dictionary

                match jsonDeserialize json with
                | Ok dict ->
                    dict
                    |> List.ofSeq
                    |> List.tryHead
                    |> function
                        | Some kvpair -> 
                            let file, comp = kvpair.Key, kvpair.Value
                            Expect.equal file "Test.react.js" "Json does not correctly deserialize all properties"
                            Expect.equal comp.description (Some "test") "Json does not correctly deserialize all properties"
                            Expect.equal comp.displayName (Some "test") "Json does not correctly deserialize all properties"
                            Expect.equal (comp.props |> List.ofSeq) (expectedProps |> List.ofSeq) "Json does not correctly deserialize all properties"
                        | None -> Expect.isTrue false "Json does not correctly deserialize all properties"
                | Error e -> Expect.isTrue false (sprintf "Json fails to deserialize: %s" (e.ToString()))
        ]

    ]