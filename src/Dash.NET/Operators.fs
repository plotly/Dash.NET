namespace Dash.NET

module Operators =

    let inline (.@) (componentId:string) (componentProperty:ComponentProperty) =
        Dependency.create(componentId, componentProperty)

    let inline (=>) (target:Dependency) (callbackResult) = 
        CallbackResultBinding.bindResult target callbackResult
