module Tests

open Expecto

[<Tests>]
let tests =
    testList "test" [
        testList "test" [
            testCase "test" <| fun _ ->
                let subject = true
                Expect.isTrue subject "I compute, therefore I am."
        ]
    ]
