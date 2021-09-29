namespace Dash.NET.CSharp

open Dash.NET
open Plotly.NET
open System

[<AutoOpen>]
module Html =
    
    (* Start CSharp interop DSL *)

    type HtmlEngineCSharp<'HtmlProp, 'DashComponent>
        (
            mkTaggedElement: (string -> List<'HtmlProp> -> 'DashComponent),
            mkHtmlText: string -> 'DashComponent
        ) =
        member _.text(value: string) = mkHtmlText value
        member _.text(value: int) = mkHtmlText (string value)
        member _.text(value: float) = mkHtmlText (string value)
        member _.text(value: System.Guid) = mkHtmlText (string value)
        member _.custom(key: string, [<ParamArray>] props : 'HtmlProp array) = mkTaggedElement key (List.ofArray props)
        member _.none = mkHtmlText ""
        member _.a ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "A" (List.ofArray props)
        member _.abbr ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Abbr" (List.ofArray props)
        member _.address ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Address" (List.ofArray props)
        member _.anchor ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "A" (List.ofArray props)
        member _.area ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Area" (List.ofArray props)
        member _.article ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Article" (List.ofArray props)
        member _.aside ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Aside" (List.ofArray props)
        member _.audio ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Audio" (List.ofArray props)
        member _.b ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "B" (List.ofArray props)
        member _.base' ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Base" (List.ofArray props)
        member _.bdi ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Bdi" (List.ofArray props)
        member _.bdo ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Bdo" (List.ofArray props)
        member _.blockquote ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Blockquote" (List.ofArray props)
        member _.body ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Body" (List.ofArray props)
        member _.br ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Br" (List.ofArray props)
        member _.button ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Button" (List.ofArray props)
        member _.canvas ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Canvas" (List.ofArray props)
        member _.caption ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Caption" (List.ofArray props)
        member _.cite ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Cite" (List.ofArray props)
        member _.code ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Code" (List.ofArray props)
        member _.col ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Col" (List.ofArray props)
        member _.colgroup ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Colgroup" (List.ofArray props)
        member _.data ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Data" (List.ofArray props)
        member _.datalist ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Datalist" (List.ofArray props)
        member _.dd ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Dd" (List.ofArray props)
        member _.del ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Del" (List.ofArray props)
        member _.details ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Details" (List.ofArray props)
        member _.dfn ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Dfn" (List.ofArray props)
        member _.dialog ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Dialog" (List.ofArray props)
        member _.div ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Div" (List.ofArray props)
        member _.dl ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Dl" (List.ofArray props)
        member _.dt ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Dt" (List.ofArray props)
        member _.em ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Em" (List.ofArray props)
        member _.fieldSet ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Fieldset" (List.ofArray props)
        member _.figcaption ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Figcaption" (List.ofArray props)
        member _.figure ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Figure" (List.ofArray props)
        member _.footer ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Footer" (List.ofArray props)
        member _.form ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Form" (List.ofArray props)
        member _.h1 ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "H1" (List.ofArray props)
        member _.h2 ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "H2" (List.ofArray props)
        member _.h3 ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "H3" (List.ofArray props)
        member _.h4 ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "H4" (List.ofArray props)
        member _.h5 ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "H5" (List.ofArray props)
        member _.h6 ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "H6" (List.ofArray props)
        member _.head ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Head" (List.ofArray props)
        member _.header ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Header" (List.ofArray props)
        member _.hr ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Hr" (List.ofArray props)
        member _.html ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Html" (List.ofArray props)
        member _.i ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "I" (List.ofArray props)
        member _.iframe ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Iframe" (List.ofArray props)
        member _.img ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Img" (List.ofArray props)
        member _.input ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Input" (List.ofArray props)
        member _.ins ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Ins" (List.ofArray props)
        member _.kbd ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Kbd" (List.ofArray props)
        member _.label ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Label" (List.ofArray props)
        member _.legend ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Legend" (List.ofArray props)
        member _.li ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Li" (List.ofArray props)
        member _.listItem ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Li" (List.ofArray props)
        member _.main ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Main" (List.ofArray props)
        member _.map ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Map" (List.ofArray props)
        member _.mark ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Mark" (List.ofArray props)
        member _.metadata ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Metadata" (List.ofArray props)
        member _.meter ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Meter" (List.ofArray props)
        member _.nav ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Nav" (List.ofArray props)
        member _.noscript ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Noscript" (List.ofArray props)
        member _.object ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Object" (List.ofArray props)
        member _.ol ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Ol" (List.ofArray props)
        member _.option ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Option" (List.ofArray props)
        member _.optgroup ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Optgroup" (List.ofArray props)
        member _.orderedList ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Ol" (List.ofArray props)
        member _.output ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Output" (List.ofArray props)
        member _.p ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "P" (List.ofArray props)
        member _.paragraph ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "P" (List.ofArray props)
        member _.picture ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Picture" (List.ofArray props)
        member _.pre ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Pre" (List.ofArray props)
        member _.progress ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Progress" (List.ofArray props)
        member _.q ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Q" (List.ofArray props)
        member _.rb ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Rb" (List.ofArray props)
        member _.rp ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Rp" (List.ofArray props)
        member _.rt ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Rt" (List.ofArray props)
        member _.rtc ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Rtc" (List.ofArray props)
        member _.ruby ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Ruby" (List.ofArray props)
        member _.s ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "S" (List.ofArray props)
        member _.samp ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Samp" (List.ofArray props)
        member _.script ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Script" (List.ofArray props)
        member _.section ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Section" (List.ofArray props)
        member _.select ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Select" (List.ofArray props)
        member _.small ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Small" (List.ofArray props)
        member _.source ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Source" (List.ofArray props)
        member _.span ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Span" (List.ofArray props)
        member _.strong ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Strong" (List.ofArray props)
        member _.sub ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Sub" (List.ofArray props)
        member _.summary ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Summary" (List.ofArray props)
        member _.sup ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Sup" (List.ofArray props)
        member _.table ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Table" (List.ofArray props)
        member _.tableBody ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Tbody" (List.ofArray props)
        member _.tableCell ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Td" (List.ofArray props)
        member _.tableHeader ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Th" (List.ofArray props)
        member _.tableRow ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Tr" (List.ofArray props)
        member _.tbody ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Tbody" (List.ofArray props)
        member _.td ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Td" (List.ofArray props)
        member _.template ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Template" (List.ofArray props)
        member _.textarea ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Textarea" (List.ofArray props)
        member _.tfoot ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Tfoot" (List.ofArray props)
        member _.th ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Th" (List.ofArray props)
        member _.thead ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Thead" (List.ofArray props)
        member _.time ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Time" (List.ofArray props)
        member _.tr ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Tr" (List.ofArray props)
        member _.track ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Track" (List.ofArray props)
        member _.u ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "U" (List.ofArray props)
        member _.ul ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Ul" (List.ofArray props)
        member _.unorderedList ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Ul" (List.ofArray props)
        member _.var ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Var" (List.ofArray props)
        member _.video ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Video" (List.ofArray props)
        member _.wbr ([<ParamArray>] props : 'HtmlProp array) = mkTaggedElement "Wbr" (List.ofArray props)
    
    type DashHtmlEngineCSharp() =
        inherit HtmlEngineCSharp<HtmlProp, DashComponent>(
            (fun tag props -> (tag, props) |> HtmlProp.toDashComponent),
            (fun str -> (Dash.NET.Html.Text str) |> HtmlProp.toDashComponent))
    
    let Html = DashHtmlEngineCSharp()
    
    type DashAttrEngine() =
        inherit Feliz.AttrEngine<HtmlProp>(
            (fun key value -> Prop (key, box value)),
            (fun key value -> Prop (key, box value)))

        member _.style(value: List<Style>) =
            let mapOfStyles =
                value
                |> List.map
                    (fun (s: Style) ->
                        match s with
                        | StyleProperty (cssPropertyName, cssPropertyValue) -> cssPropertyName, cssPropertyValue)
                |> Map.ofList
                |> box

            ("style", mapOfStyles) |> Prop

        /// contextMenu (string; optional): Defines the ID of a <menu> element which will serve as the element's context menu.
        member _.contextMenu(value: string) = ("contextMenu", box value) |> Prop
        
        /// children (a list of or a singular dash component, string or number; optional): The children of this component.
        member _.children(value: int) = [ Html.text value ] |> Children'
        member _.children(value: string) = [ Html.text value ] |> Children'
        member _.children(value: float) = [ Html.text value ] |> Children'
        member _.children(value: System.Guid) = [ Html.text value ] |> Children'
        member _.children(value: DashComponent) = [ value ] |> Children'
        //member _.children(value: DashComponent list) = value |> Children'
        member _.children([<ParamArray>] value: DashComponent array) = value |> List.ofArray |> Children'
        member _.children(value: DashComponent seq) = value |> Seq.toList |> Children'

        /// dir (string; optional): Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left).
        member _.dir(value: string) = ("dir", box value) |> Prop

        /// download (string; optional): Indicates that the hyperlink is to be used for downloading a resource.
        member _.download(value: string) = ("download", box value) |> Prop

        /// draggable (string; optional): Indicates that the hyperlink is to be used for downloading a resource.
        member _.draggable(value: string) = ("draggable", box value) |> Prop

        /// draggable (string; optional): Indicates that the hyperlink is to be used for downloading a resource.
        member _.key(value: string) = ("key", box value) |> Prop

        /// Object that holds the loading state object coming from dash-renderer. loading_state is a dict with keys:
        /// component_name (string; optional): Holds the name of the component that is loading.
        /// is_loading (boolean; optional): Determines if the component is loading or not.
        /// prop_name (string; optional): Holds which property is loading.
        member _.loadingState(value: seq<string * obj>) =
            let mapOfLoadingState = value |> Map.ofSeq |> box
            ("loading_state", mapOfLoadingState) |> Prop

        /// An integer that represents the number of times that this element has been clicked on.
        member _.n_clicks(value: int) = ("n_clicks", box value) |> Prop

        /// An integer that represents the time (in ms since 1970) at which n_clicks changed. This can be used to tell which button was changed most recently.
        member _.n_clicks_timestamp(value: int) =
            ("n_clicks_timestamp", box value) |> Prop
        ///
        /// Create a custom attribute
        ///
        /// You generally shouldn't need to use this, if you notice an attribute missing please submit an issue.
        member _.custom(key: string, value: string) = (key, box value) |> Prop

    let Attr = DashAttrEngine()
    
    (* End CSharp interop DSL *)