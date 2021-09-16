namespace Dash.NET.Interactive

(*
The idea is taking a dash app 

let dashApp = .....

DashInteractive.Visualize dashApp

this will output a type registered 

we register a formatter for dash apps



*)

module Say =
    let hello name =
        printfn "Hello %s" name
