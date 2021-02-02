namespace Dash.NET

type CallbackResultBinding = {
    Target: Dependency // Dependency
    BoxedResult: obj
} with
    static member create (target:Dependency) (boxedResult:obj) =
        {
            Target = target
            BoxedResult = boxedResult
        }

    static member inline bindResult (target:Dependency) (callbackResult) = 
        {
            Target = target
            BoxedResult =  box callbackResult
        }
