[<AutoOpen>]
module internal Interop

// Internal interop helpers

let raiseNullArg (name : string) (message : string) = new System.ArgumentNullException(name, message) |> raise

let guardAgainstNull name value =
    if isNull (box value) then raiseNullArg name "Nulls not allowed in Dash.NET.CSharp library functions"
