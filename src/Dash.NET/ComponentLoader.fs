namespace Dash.NET

open System.Collections.Generic
open System.Threading

module ComponentLoader =

    //TODO: add specification as to whether or not the javascript file is hosted locally or remotely (cors)
    type LoadableComponent = 
        { ComponentName: string
          ComponentJavascript: string }

    type ComponentLoader internal () =
        let loadedComponents: Dictionary<string,LoadableComponent> = Dictionary<string,LoadableComponent>()

        member _.LoadComponent(comp: LoadableComponent) =
            if loadedComponents.TryAdd(comp.ComponentName, comp) then
                printfn "Loaded component: %s" comp.ComponentName

        member _.LoadedComponents() = 
            loadedComponents.Values
            |> Seq.toList

    let ComponentLoader = ComponentLoader()

    let loadComponent = ComponentLoader.LoadComponent
    let loadedComponents = ComponentLoader.LoadedComponents

        
        