namespace Dash.NET

open System.Collections.Concurrent
open System.Threading

module ComponentLoader =

    //TODO: add specification as to whether or not the javascript file is hosted locally or remotely (cors)
    type LoadableComponent = 
        { ComponentName: string
          ComponentJavascript: string }

    type ComponentLoader internal () =
        let loadedComponents: ConcurrentDictionary<string,LoadableComponent> = ConcurrentDictionary<string,LoadableComponent>()

        let mailbox =
            MailboxProcessor.Start( fun inbox->
                let rec messageLoop() = 
                    async {
                        let! msg = inbox.Receive()
                        if loadedComponents.TryAdd(msg.ComponentName, msg) then
                            printfn "Loaded component: %s" msg.ComponentName
                        return! messageLoop()
                    }

                messageLoop() )

        member _.LoadComponent(comp: LoadableComponent) =
            mailbox.Post(comp)

        member _.LoadedComponents() = 
            loadedComponents.ToArray()
            |> Array.map (fun kvp -> kvp.Value)
            |> Array.toList

    let ComponentLoader = ComponentLoader()

    let loadComponent = ComponentLoader.LoadComponent
    let loadedComponents = ComponentLoader.LoadedComponents

        
        