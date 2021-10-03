module ComponentPropTypes

// Serialization tests for common component props
open Expecto
open SerializationTestUtils
open Dash.NET.ComponentPropTypes

[<Tests>]
let ``Component prop types serialization tests`` =
    testList "SerializationTests.ComponentPropTypes" [
        testCase "LoadingState" (fun _ ->
            let subject = LoadingState.create(true, "Hi", "yes.") |> json
            Expect.equal 
                subject 
                """{"isLoading":true,"propName":"Hi","componentName":"yes."}"""
                "LoadingState not serialized correctly"
        )        
        testCase "DropdownOption" (fun _ ->
            let subject = DropdownOption.create("Label", 42, false, "yes.") |> json
            Expect.equal 
                subject 
                """{"label":"Label","value":42,"disabled":false,"title":"yes."}"""
                "DropdownOption not serialized correctly"
        )        
        testCase "RadioItemsOption" (fun _ ->
            let subject = RadioItemsOption.create("Label", 42, false) |> json
            Expect.equal 
                subject 
                """{"label":"Label","value":42,"disabled":false}"""
                "RadioItemsOption not serialized correctly"
        )        
        testCase "TabColors" (fun _ ->
            let subject = TabColors.create("#ffffff", "#000000", "violet") |> json
            Expect.equal 
                subject 
                """{"border":"#ffffff","primary":"#000000","background":"violet"}"""
                "TabColors not serialized correctly"
        )        
        testCase "ChecklistOption" (fun _ ->
            let subject = ChecklistOption.create("Label", 42, false) |> json
            Expect.equal 
                subject 
                """{"label":"Label","value":42,"disabled":false}"""
                "ChecklistOption not serialized correctly"
        )        
    ]
