namespace Dash.NET

module Views =
    open Giraffe.ViewEngine
    
    let createConfigScript (config:DashConfig) =
        let innerJson = Newtonsoft.Json.JsonConvert.SerializeObject config
        script [ _id "_dash-config"; _type "application/javascript"] [rawText innerJson]

    let defaultRenderer = rawText """var renderer = new DashRenderer();"""
    
    let createRendererScript renderer =
        script [ _id "_dash-renderer"; _type "application/javascript"] [renderer]

    let createFaviconLink path =
        link [
            _rel "icon"
            _type "image/x-icon"
            _href path
        ]

    let defaultAppEntry = 
        div [_id "react-entry-point"] [
            div [_class "_dash-loading"] [
                encodedText "Loading..."
            ]
        ]

    let defaultCSS = link [_rel "stylesheet"; _href "https://codepen.io/chriddyp/pen/bWLwgP.css" ; _crossorigin " "]

    let dashCDNScripts = [
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/react@16.13.0/umd/react.development.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/react-dom@16.13.0/umd/react-dom.development.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/@babel/polyfill@7.8.7/dist/polyfill.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/prop-types@15.7.2/prop-types.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/dash-renderer@1.7.0/dash_renderer/dash_renderer.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://unpkg.com/dash-core-components@1.11.0/dash_core_components/dash_core_components.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://cdn.jsdelivr.net/npm/dash-html-components@1.1.0/dash_html_components/dash_html_components.min.js"] []
        script [_type "application/javascript"; _crossorigin " "; _src "https://cdn.plot.ly/plotly-latest.min.js"] []
    ]

    let componentScripts () =
        ComponentLoader.loadedComponents()
        |> List.map (fun lc -> script [_type "application/javascript"; _crossorigin " "; _src lc.ComponentJavascript] [])

    let createIndex metas appTitle faviconPath css appEntry config scripts renderer = 
        html [] [
            head [] [
                yield! metas
                title [] [encodedText appTitle]
                createFaviconLink faviconPath
                yield! css
            ]
            body [] [
                appEntry
                footer [] [
                    createConfigScript config
                    yield! scripts
                    createRendererScript defaultRenderer
                ]
            ]
        ]

    type IndexView =
        {
            Metas : seq<XmlNode>
            AppTitle : string
            FaviconPath : string
            CSS : seq<XmlNode>
            AppEntry : XmlNode
            Config : DashConfig
            Scripts : seq<XmlNode>
            Renderer : XmlNode
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
                defaultRenderer

        static member initDefaultWith (initializer:IndexView -> IndexView) = IndexView.initDefault() |> initializer

        ///returns an IndexView with the given meta tags in place of the old.
        static member withMetas (metas:seq<XmlNode>) (indexView:IndexView) =
            {indexView with
                Metas = metas
            }

        ///returns an IndexView with the given meta tag appended to the ones already in place.
        static member appendMetas (meta:seq<XmlNode>) (indexView:IndexView) =
            {indexView with
                Metas = Seq.append indexView.Metas  meta
            }

        ///returns an IndexView with the given css link tags in place of the old.
        static member withCSSLinks (css:seq<XmlNode>) (indexView:IndexView) =
            {indexView with
                CSS = css
            }

        ///returns an IndexView with the given css link tag appended to the ones already in place.
        static member appendCSSLinks (css:seq<XmlNode>) (indexView:IndexView) =
            {indexView with
                CSS = Seq.append indexView.CSS css
            }

        ///returns an IndexView with the given script tags in place of the old.
        static member withScripts (scripts:seq<XmlNode>) (indexView:IndexView) =
            {indexView with
                Scripts = scripts
            }

        ///returns an IndexView with the given script tag appended to the ones already in place.
        static member appendScripts (script:seq<XmlNode>) (indexView:IndexView) =
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
                    title [] [encodedText indexView.AppTitle]
                    createFaviconLink indexView.FaviconPath
                    yield! indexView.CSS               
                ]
                body [] [
                    indexView.AppEntry
                    footer [] [
                        createConfigScript indexView.Config
                        yield! indexView.Scripts
                        yield! componentScripts()
                        createRendererScript defaultRenderer
                    ]
                ]
            ]