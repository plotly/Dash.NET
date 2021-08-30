namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json

/////<summary>
/////A component that renders Markdown text as specified by the
/////GitHub Markdown spec. These component uses
/////[react-markdown](https://rexxars.github.io/react-markdown/) under the hood.
/////</summary>
//[<RequireQualifiedAccess>]
//module DashMarkdown =
//    ///<summary>
//    ///record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)'
//    ///</summary>
//    type LoadingStateType =
//        { IsLoading: bool
//          PropName: string
//          ComponentName: string }
//        static member convert(this) =
//            box
//                {| is_loading = this.IsLoading
//                   prop_name = this.PropName
//                   component_name = this.ComponentName |}

//    ///<summary>
//    ///value equal to: 'dark', 'light'
//    ///&#10;
//    ///Color scheme; default 'light'
//    ///</summary>
//    type HighlightConfigTypeThemeType =
//        | Dark
//        | Light
//        static member convert(this) =
//            box (
//                match this with
//                | Dark -> "dark"
//                | Light -> "light"
//            )

//    ///<summary>
//    ///record with the field: 'theme: value equal to: 'dark', 'light' (optional)'
//    ///</summary>
//    type HighlightConfigType =
//        { Theme: HighlightConfigTypeThemeType }
//        static member convert(this) =
//            box {| theme = (this.Theme |> HighlightConfigTypeThemeType.convert) |}

//    ///<summary>
//    ///• className (string) - Class name of the container element
//    ///&#10;
//    ///• dangerously_allow_html (boolean; default false) - A boolean to control raw HTML escaping.
//    ///Setting HTML from code is risky because it's easy to
//    ///inadvertently expose your users to a cross-site scripting (XSS)
//    ///(https://en.wikipedia.org/wiki/Cross-site_scripting) attack.
//    ///&#10;
//    ///• children (string | list with values of type: string) - A markdown string (or array of strings) that adhreres to the CommonMark spec
//    ///&#10;
//    ///• dedent (boolean; default true) - Remove matching leading whitespace from all lines.
//    ///Lines that are empty, or contain *only* whitespace, are ignored.
//    ///Both spaces and tab characters are removed, but only if they match;
//    ///we will not convert tabs to spaces or vice versa.
//    ///&#10;
//    ///• highlight_config (record with the field: 'theme: value equal to: 'dark', 'light' (optional)'; default {}) - Config options for syntax highlighting.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///&#10;
//    ///• style (record) - User-defined inline styles for the rendered Markdown
//    ///</summary>
//    type Prop =
//        | ClassName of string
//        | DangerouslyAllowHtml of bool
//        | Dedent of bool
//        | HighlightConfig of HighlightConfigType
//        | LoadingState of LoadingStateType
//        | Style of obj
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | ClassName (p) -> "className", box p
//            | DangerouslyAllowHtml (p) -> "dangerously_allow_html", box p
//            | Dedent (p) -> "dedent", box p
//            | HighlightConfig (p) -> "highlight_config", p |> HighlightConfigType.convert
//            | LoadingState (p) -> "loading_state", p |> LoadingStateType.convert
//            | Style (p) -> "style", box p

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///Class name of the container element
//        ///</summary>
//        static member className(p: string) = Prop(ClassName p)
//        ///<summary>
//        ///A boolean to control raw HTML escaping.
//        ///Setting HTML from code is risky because it's easy to
//        ///inadvertently expose your users to a cross-site scripting (XSS)
//        ///(https://en.wikipedia.org/wiki/Cross-site_scripting) attack.
//        ///</summary>
//        static member dangerouslyAllowHtml(p: bool) = Prop(DangerouslyAllowHtml p)
//        ///<summary>
//        ///Remove matching leading whitespace from all lines.
//        ///Lines that are empty, or contain *only* whitespace, are ignored.
//        ///Both spaces and tab characters are removed, but only if they match;
//        ///we will not convert tabs to spaces or vice versa.
//        ///</summary>
//        static member dedent(p: bool) = Prop(Dedent p)
//        ///<summary>
//        ///Config options for syntax highlighting.
//        ///</summary>
//        static member highlightConfig(p: HighlightConfigType) = Prop(HighlightConfig p)
//        ///<summary>
//        ///Object that holds the loading state object coming from dash-renderer
//        ///</summary>
//        static member loadingState(p: LoadingStateType) = Prop(LoadingState p)
//        ///<summary>
//        ///User-defined inline styles for the rendered Markdown
//        ///</summary>
//        static member style(p: obj) = Prop(Style p)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: int) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: string) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: float) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: System.Guid) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: DashComponent) = Children([ value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: list<DashComponent>) = Children(value)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

//    ///<summary>
//    ///A component that renders Markdown text as specified by the
//    ///GitHub Markdown spec. These component uses
//    ///[react-markdown](https://rexxars.github.io/react-markdown/) under the hood.
//    ///</summary>
//    type DashMarkdown() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?className: string,
//                ?dangerouslyAllowHtml: bool,
//                ?dedent: bool,
//                ?highlightConfig: HighlightConfigType,
//                ?loadingState: LoadingStateType,
//                ?style: obj
//            ) =
//            (fun (t: DashMarkdown) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "className" (className |> Option.map box)
//                DynObj.setValueOpt props "dangerouslyAllowHtml" (dangerouslyAllowHtml |> Option.map box)
//                DynObj.setValueOpt props "dedent" (dedent |> Option.map box)
//                DynObj.setValueOpt props "highlightConfig" (highlightConfig |> Option.map HighlightConfigType.convert)
//                DynObj.setValueOpt props "loadingState" (loadingState |> Option.map LoadingStateType.convert)
//                DynObj.setValueOpt props "style" (style |> Option.map box)
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "DashMarkdown"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?className: string,
//                ?dangerouslyAllowHtml: bool,
//                ?dedent: bool,
//                ?highlightConfig: HighlightConfigType,
//                ?loadingState: LoadingStateType,
//                ?style: obj
//            ) =
//            DashMarkdown.applyMembers
//                (id,
//                 children,
//                 ?className = className,
//                 ?dangerouslyAllowHtml = dangerouslyAllowHtml,
//                 ?dedent = dedent,
//                 ?highlightConfig = highlightConfig,
//                 ?loadingState = loadingState,
//                 ?style = style)
//                (DashMarkdown())

//    ///<summary>
//    ///A component that renders Markdown text as specified by the
//    ///GitHub Markdown spec. These component uses
//    ///[react-markdown](https://rexxars.github.io/react-markdown/) under the hood.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• className (string) - Class name of the container element
//    ///&#10;
//    ///• dangerously_allow_html (boolean; default false) - A boolean to control raw HTML escaping.
//    ///Setting HTML from code is risky because it's easy to
//    ///inadvertently expose your users to a cross-site scripting (XSS)
//    ///(https://en.wikipedia.org/wiki/Cross-site_scripting) attack.
//    ///&#10;
//    ///• children (string | list with values of type: string) - A markdown string (or array of strings) that adhreres to the CommonMark spec
//    ///&#10;
//    ///• dedent (boolean; default true) - Remove matching leading whitespace from all lines.
//    ///Lines that are empty, or contain *only* whitespace, are ignored.
//    ///Both spaces and tab characters are removed, but only if they match;
//    ///we will not convert tabs to spaces or vice versa.
//    ///&#10;
//    ///• highlight_config (record with the field: 'theme: value equal to: 'dark', 'light' (optional)'; default {}) - Config options for syntax highlighting.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///&#10;
//    ///• style (record) - User-defined inline styles for the rendered Markdown
//    ///</summary>
//    let dashMarkdown (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = DashMarkdown.init (id, children)

//        let componentProps =
//            match t.TryGetTypedValue<DashComponentProps> "props" with
//            | Some (p) -> p
//            | None -> DashComponentProps()

//        Seq.iter
//            (fun (prop: Prop) ->
//                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
//                DynObj.setValue componentProps fieldName boxedProp)
//            props

//        DynObj.setValue t "props" componentProps
//        t :> DashComponent
