namespace Dash.NET.Interactive

open Microsoft.DotNet.Interactive
open Microsoft.DotNet.Interactive.Formatting
open Dash.NET.Suave
open System
open System.Threading
open System.Net
open System.IO
open System.Threading.Tasks
open Microsoft.AspNetCore.Html
open System.Text.Encodings.Web

module Util = 
  let formatter (app:DashApp) (writer:TextWriter)= 

    let config = 
      { hostname = "localhost"
      ; ip = "127.0.0.1"
      ; port = 0 // Request the server to be launched on a random ephemeral port
      ; errorHandler = Suave.Web.defaultErrorHandler }

    // launch app 
    let (listening, server) = 
      DashApp.runAsync [||] config app

    let cts = new CancellationTokenSource()
    Async.Start(server, cts.Token)

    // Capture assigned port
    let [| Some startData |] = Async.RunSynchronously listening
    let port = startData.binding.port

    let url = sprintf "http://%s:%d/iframe" config.ip port

    let webClient = new WebClient()
    let str = webClient.DownloadString(url)

    let html = new HtmlString(str) :> IHtmlContent
    html.WriteTo(writer, HtmlEncoder.Default)

  let action = Action<DashApp,TextWriter>(formatter)

type DashInteractiveExtension () = 

  interface IKernelExtension with
    member x.OnLoadAsync(kernel:Kernel) : Task = 
      Formatter.Register<DashApp>(Util.formatter, HtmlFormatter.MimeType)

      let ctx = KernelInvocationContext.Current
      if ctx <> null then
        ctx.Display("Dash extension has been loaded. Enjoy!","text/html") |> ignore;
      Task.CompletedTask