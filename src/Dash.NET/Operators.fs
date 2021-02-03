namespace Dash.NET

module Operators =

    /// Shorthand for creation of a dependency (binding of the property of a component)
    let inline (@.) (componentId:string) (componentProperty:ComponentProperty) =
        Dependency.create(componentId, componentProperty)

    /// Shorthand operator for defining a CallbackResultBinding
    let inline (=>) (target:Dependency) (callbackResult) = 
        CallbackResultBinding.bindResult target callbackResult
