namespace Dash.NET.Suave

module Views =
  open Dash.NET
  open Suave.Html

  let createConfigScript (config:DashConfig) =
      let innerJson = Newtonsoft.Json.JsonConvert.SerializeObject config
      script [ "id", "_dash-config"; "type", "application/javascript"] (rawText innerJson)

  let defaultRenderer = rawText """var renderer = new DashRenderer();"""
 
  let createRendererScript renderer =
      script [ "id", "_dash-renderer"; "type", "application/javascript"] renderer
  let createFaviconLink path =
      link [
          "rel", "icon"
          "type", "image/x-icon"
          "href", path
      ]

  let defaultAppEntry = 
      div ["id", "react-entry-point"] [
          div ["class", "_dash-loading"] (text "Loading...")
      ]

  let defaultCSS = link ["rel", "stylesheet"; "href", "https://codepen.io/chriddyp/pen/bWLwgP.css" ; "_crossorigin", " "]

  let dashCDNScripts = [
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://unpkg.com/react@16.13.0/umd/react.development.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://unpkg.com/react-dom@16.13.0/umd/react-dom.development.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://unpkg.com/@babel/polyfill@7.8.7/dist/polyfill.min.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://unpkg.com/prop-types@15.7.2/prop-types.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://unpkg.com/dash-renderer@1.7.0/dash_renderer/dash_renderer.min.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://unpkg.com/dash-core-components@1.17.1/dash_core_components/dash_core_components.min.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://cdn.jsdelivr.net/npm/dash-html-components@1.1.0/dash_html_components/dash_html_components.min.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", "https://cdn.plot.ly/plotly-latest.min.js"] []
      script ["type", "application/javascript"; "crossorigin", " "; "src", DashTable.CdnLink ] []
  ]

  type IndexView =
      {
          Metas : seq<Node>
          AppTitle : string
          FaviconPath : string
          CSS : seq<Node>
          AppEntry : Node
          Config : DashConfig
          Scripts : seq<Node>
          Renderer : Node
      }
      static member create metas appTitle faviconPath css appEntry config scripts renderer =
          {
              Metas       = metas
              AppTitle    = appTitle
              FaviconPath = faviconPath
              CSS         = css
              AppEntry    = appEntry
              Config      = config
              Scripts     = scripts
              Renderer    = renderer
          }
      static member initDefault () =
          IndexView.create
              []
              "Dash.NET"
              "_favicon.ico"
              []
              defaultAppEntry
              (DashConfig.initDefault())
              dashCDNScripts
              (List.head defaultRenderer)

      static member initDefaultWith (initializer:IndexView -> IndexView) = IndexView.initDefault() |> initializer

      ///returns an IndexView with the given meta tags in place of the old.
      static member withMetas (metas:seq<Node>) (indexView:IndexView) =
          {indexView with
              Metas = metas
          }

      ///returns an IndexView with the given meta tag appended to the ones already in place.
      static member appendMetas (meta:seq<Node>) (indexView:IndexView) =
          {indexView with
              Metas = Seq.append indexView.Metas  meta
          }

      ///returns an IndexView with the given css link tags in place of the old.
      static member withCSSLinks (css:seq<Node>) (indexView:IndexView) =
          {indexView with
              CSS = css
          }

      ///returns an IndexView with the given css link tag appended to the ones already in place.
      static member appendCSSLinks (css:seq<Node>) (indexView:IndexView) =
          {indexView with
              CSS = Seq.append indexView.CSS css
          }

      ///returns an IndexView with the given script tags in place of the old.
      static member withScripts (scripts:seq<Node>) (indexView:IndexView) =
          {indexView with
              Scripts = scripts
          }

      ///returns an IndexView with the given script tag appended to the ones already in place.
      static member appendScripts (script:seq<Node>) (indexView:IndexView) =
          {indexView with
              Scripts = Seq.append indexView.Scripts script
          }

      static member withConfig (dashConfig:DashConfig) (indexView:IndexView) =
          {indexView with
              Config = dashConfig
          }

      static member toHTMLComponent (indexView:IndexView) =
          html [] [
              head [] [
                  yield! indexView.Metas
                  title [] indexView.AppTitle
                  createFaviconLink indexView.FaviconPath
                  yield! indexView.CSS               
              ]
              body [] [
                  indexView.AppEntry
                  div [] [
                      createConfigScript indexView.Config
                      yield! indexView.Scripts
                      createRendererScript defaultRenderer
                  ]
              ]
          ]