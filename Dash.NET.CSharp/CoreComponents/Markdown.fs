namespace Dash.NET.CSharp.DCC

open System
open Dash.NET.CSharp.ComponentStyle

///<summary>
///A component that renders Markdown text as specified by the
///GitHub Markdown spec. These component uses
///[react-markdown](https://remarkjs.github.io/react-markdown/) under the hood.
///</summary>
[<RequireQualifiedAccess>]
module Markdown =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Markdown.Attr


    ///<summary>
    ///value equal to: 'dark', 'light'
    ///&#10;
    ///Color scheme; default 'light'
    ///</summary>
    type HighlightConfigTheme = private WrappedHighlightConfigTheme of Dash.NET.DCC.Markdown.HighlightConfigTheme with
        static member internal Wrap (attr : Dash.NET.DCC.Markdown.HighlightConfigTheme) = HighlightConfigTheme.WrappedHighlightConfigTheme attr
        static member internal Unwrap (attr : HighlightConfigTheme) = match attr with | HighlightConfigTheme.WrappedHighlightConfigTheme attr -> attr

        static member Dark () = Dash.NET.DCC.Markdown.HighlightConfigTheme.Dark
        static member Light () = Dash.NET.DCC.Markdown.HighlightConfigTheme.Light

    
    
    ///<summary>
    ///record with the field: 'theme: value equal to: 'dark', 'light' (optional)'
    ///</summary>
    type HighlightConfig =
        { Theme: HighlightConfigTheme }
        static member internal Convert (v : HighlightConfig) : Dash.NET.DCC.Markdown.HighlightConfig = { Dash.NET.DCC.Markdown.HighlightConfig.Theme = v.Theme |> HighlightConfigTheme.Unwrap }

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Markdown.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Markdown.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///Class name of the container element
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            OAttr.className p |> Attr.Wrap
        ///<summary>
        ///A boolean to control raw HTML escaping.
        ///Setting HTML from code is risky because it's easy to
        ///inadvertently expose your users to a cross-site scripting (XSS)
        ///(https://en.wikipedia.org/wiki/Cross-site_scripting) attack.
        ///</summary>
        static member dangerouslyAllowHtml(p: bool) = 
            guardAgainstNull "p" p
            OAttr.dangerouslyAllowHtml p |> Attr.Wrap
        ///<summary>
        ///Remove matching leading whitespace from all lines.
        ///Lines that are empty, or contain *only* whitespace, are ignored.
        ///Both spaces and tab characters are removed, but only if they match;
        ///we will not convert tabs to spaces or vice versa.
        ///</summary>
        static member dedent(p: bool) = 
            guardAgainstNull "p" p
            OAttr.dedent p |> Attr.Wrap
        ///<summary>
        ///Config options for syntax highlighting.
        ///</summary>
        static member highlightConfig(p: HighlightConfig) = 
            guardAgainstNull "p" p
            OAttr.highlightConfig (p |> HighlightConfig.Convert) |> Attr.Wrap
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = 
            guardAgainstNull "p" p
            OAttr.loadingState (p |> LoadingState.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Defines CSS styles which will override styles previously set.
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Dsl.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.style (p |> Array.map Dash.NET.CSharp.Dsl.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap

    ///<summary>
    ///A component that renders Markdown text as specified by the
    ///GitHub Markdown spec. These component uses
    ///[react-markdown](https://remarkjs.github.io/react-markdown/) under the hood.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• className (string) - Class name of the container element
    ///&#10;
    ///• dangerously_allow_html (boolean; default false) - A boolean to control raw HTML escaping.
    ///Setting HTML from code is risky because it's easy to
    ///inadvertently expose your users to a cross-site scripting (XSS)
    ///(https://en.wikipedia.org/wiki/Cross-site_scripting) attack.
    ///&#10;
    ///• children (string | list with values of type: string) - A markdown string (or array of strings) that adhreres to the CommonMark spec
    ///&#10;
    ///• dedent (boolean; default true) - Remove matching leading whitespace from all lines.
    ///Lines that are empty, or contain *only* whitespace, are ignored.
    ///Both spaces and tab characters are removed, but only if they match;
    ///we will not convert tabs to spaces or vice versa.
    ///&#10;
    ///• highlight_config (record with the field: 'theme: value equal to: 'dark', 'light' (optional)'; default {}) - Config options for syntax highlighting.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///&#10;
    ///• style (record) - User-defined inline styles for the rendered Markdown
    ///</summary>
    let markdown (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Markdown.markdown id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap