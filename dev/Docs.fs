module Docs

open Dash.NET

// temporary workaround for checking correctness of the F# snippets in the documentation. 
// the usual approach does not work here, because we cannot reference Asp.NetCore in a .fsx script.

// F# Codeblock 1 of docs
let myApp = DashApp.initDefault()


// F# Codeblock 2 of docs
open Dash.NET.Html

//Will create the following html:
//<div>
//  <h1>"Hello world from Dash.NET!"</h1>
//</div>
//
let myLayout = 
    Html.div [
        Attr.children [
            Html.h1 [
                Attr.children "Hello world from Dash.NET!"
            ]
        ]
    ]

let test = 
    DashApp.initDefault()
    |> DashApp.withLayout myLayout


// F# Codeblock 3 of docs

let test2 = 
    DashApp.initDefault()
    |> DashApp.withLayout myLayout
    |> DashApp.appendCSSLinks ["main.css"]

// F# Codeblock 4 of docs

let myLayout3 = 
    Html.div [
        Attr.children [
            Html.h1 [
                Attr.className "title is-1"
                Attr.children "Hello world from Dash.NET!"
            ]
        ]
    ]

let test3 = 
    DashApp.initDefault()
    |> DashApp.withLayout myLayout3
    |> DashApp.appendCSSLinks [
        "https://cdnjs.cloudflare.com/ajax/libs/bulma/0.9.1/css/bulma.min.css"
    ]

// F# Codeblock 5 of docs

open Dash.NET.Html
open Dash.NET.DCC
open Plotly.NET

let myGraph = Chart.Line([(1,1);(2,2)])

let myLayout4 = 
    Html.div  [
        Attr.children [
            Html.h1 [ Attr.children "Hello world from Dash.NET!"]
            Html.h2 [ Attr.children "Take a look at this graph:"]
            Graph.graph "my-ghraph-id" [Graph.Attr.figure (myGraph |> GenericChart.toFigure)]
        ]
    ]

let test4 = 
    DashApp.initDefault()
    |> DashApp.withLayout myLayout4
    
// F# Codeblock 6 of docs

open Dash.NET.Html
open Dash.NET.DCC
open ComponentPropTypes

let myLayout5 = 
    Html.div [
        Attr.children [
            Html.h1 [Attr.children "Hello world from Dash.NET!"]
            Html.h2 [Attr.children "Tell us something!"]
            Input.input "test-input" [Input.Attr.inputType InputType.Text]
            Html.h2 [Attr.children "test-output"] 
        ]
    ]

// F# Codeblock 7 of docs

// a 1 -> 1 callback
let testCallback =
    Callback.singleOut(
        CallbackInput.create("test-input","value"),       // <- Input of the callback is the `value` property of the component with the id "test-input"
        CallbackOutput.create("test-output","children"),    // <- Output of the callback is the `children` property of the component with the id "test-output"
        (fun (input:string) ->                              // this function takes a string as input and returns another message.
            sprintf "You said : %s" input
        )
    )

// F# Codeblock 8 of docs

open Dash.NET.Operators

let testCallback2 =
    Callback.singleOut(
        "test-input" @. Value,       // <- Input of the callback is the `value`property of the component with the id "test-input", now typesafe
        "test-output" @. Children,    // <- Output of the callback is the `children` property of the component with the id "test-output", now typesafe
        (fun (input:string) ->                              // this function takes a string as input and returns another message, now bound to the output property
            "test-output" @. Children => sprintf "You said : %s" input
        )
    )

// F# Codeblock 9 of docs
    
let testCallback3 =
    Callback.singleOut(
        CallbackInput.create("test-input" ,Value),       // <- Input of the callback is the `value` property of the component with the id "test-input"
        CallbackOutput.create("test-output", Children),    // <- Output of the callback is the `children` property of the component with the id "test-output"
        (fun (input:string) ->                              // this function takes a string as input and returns another message.
            CallbackResultBinding.bindResult 
                (CallbackOutput.create("test-output", Children))
                (sprintf "You said : %s" input)
        )
    )

// F# Codeblock 10 of docs
    
let testLayout =

    Html.div [
        Attr.children [
            Html.h1 [Attr.children "Hello world from Dash.NET!"]
            Input.input "test-input1" [Input.Attr.inputType InputType.Number; Input.Attr.value 2.]
            Input.input "test-input2" [Input.Attr.inputType InputType.Number; Input.Attr.value 3.]
            Html.h2 [Attr.children "first number times 2 is:"]
            Html.div [Attr.children "test-output1"] 
            Html.h2 [Attr.children "first number times squared is:"]
            Html.div [Attr.children "test-output2"] 
            Html.h2 [Attr.children "second number times 3 is:"]
            Html.div [Attr.children "test-output3"]
            Html.h2 [Attr.children "second number squared is:"]
            Html.div [Attr.children "test-output4"]
        ]
    ]

// F# Codeblock 11 of docs
    
let multiOutCallbackExample =
    Callback.multiOut(
        [
            CallbackInput.create("test-input1", "value")
            CallbackInput.create("test-input2", "value")
        ],
        [
            CallbackInput.create("test-output1", "children")
            CallbackInput.create("test-output2", "children")            
            CallbackInput.create("test-output3", "children")
            CallbackInput.create("test-output4", "children")
        ],
        (fun (input1:float) (input2:float) ->
            [
               input1 * 2.
               input1 * input1
               input2 * 3.
               input2 * input2
            ]
        )
    )

// F# Codeblock 12 of docs

let multiOutCallbackExample2 =
    Callback.multiOut(
        [
            "test-input1" @. Value
            "test-input2" @. Value
        ],
        [
            "test-output1" @. Children
            "test-output2" @. Children          
            "test-output3" @. Children
            "test-output4" @. Children
        ],
        (fun (input1:float) (input2:float) ->
            [
               "test-output1" @. Children => input1 * 2.
               "test-output3" @. Children => input2 * 3.
               "test-output2" @. Children => input1 * input1
               "test-output4" @. Children => input2 * input2
            ]
        )
    )

// F# Codeblock 13 of docs

let myLayout6 = 
    Html.div [
        Attr.children [
            Html.h1 [Attr.children "Hello world from Dash.NET!"]
            Html.h2 [Attr.children "Tell us something!"]
            Input.input "test-input" [Input.Attr.inputType InputType.Text]
            Html.h3 [Attr.children "Input below will not trigger the callback"]
            Input.input "test-input-state" [Input.Attr.inputType InputType.Text]
            Html.h2 [Attr.children "test-output"]
        ]
    ]

let testCallback4 =
    Callback.singleOut(
        "test-input" @. Value,
        "test-output" @. Children,
        (fun (input:string) (state:string) ->
            "test-output" @. Children => (
                sprintf "You said : '%s' and we added the state: '%s'" input state)
        ),
        State = ["test-input-state" @. Value]
    )

let test5 = 
    DashApp.initDefault()
    |> DashApp.withLayout myLayout6
    |> DashApp.addCallback testCallback
