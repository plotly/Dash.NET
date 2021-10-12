namespace Dash.NET.DCC

open System
open Dash.NET
open DynamicObj

///<summary>
///The Clipboard component copies text to the clipboard
///</summary>
[<RequireQualifiedAccess>]
module Clipboard =
    ///<summary>
    ///• target_id (string | record; default null) - The id of target component containing text to copy to the clipboard.
    ///The inner text of the &#96;children&#96; prop will be copied to the clipboard.  If none, then the text from the
    /// &#96;value&#96; prop will be copied.
    ///&#10;
    ///• content (string; default null) - The text to  be copied to the clipboard if the &#96;target_id&#96; is None.
    ///&#10;
    ///• n_clicks (number; default 0) - The number of times copy button was clicked
    ///&#10;
    ///• title (string) - The text shown as a tooltip when hovering over the copy icon.
    ///&#10;
    ///• style (record) - The icon's styles
    ///&#10;
    ///• className (string) - The class  name of the icon element
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    type Prop =
        | TargetId of string
        | Content of string
        | NClicks of IConvertible
        | Title of string
        | Style of DashComponentStyle
        | ClassName of string
        | LoadingState of LoadingState

        static member convert = function
            | TargetId      p -> box p
            | Content       p -> box p
            | NClicks       p -> box p
            | Title         p -> box p
            | Style         p -> box p
            | ClassName     p -> box p
            | LoadingState  p -> box p

        static member toPropName = function
            | TargetId      _ -> "target_id"
            | Content       _ -> "content"
            | NClicks       _ -> "n_clicks"
            | Title         _ -> "title"
            | Style         _ -> "style"
            | ClassName     _ -> "className"
            | LoadingState  _ -> "loading_state"

        static member toDynamicMemberDef(prop: Prop) =
            Prop.toPropName prop
            , Prop.convert prop

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The id of target component containing text to copy to the clipboard.
        ///The inner text of the &#96;children&#96; prop will be copied to the clipboard.  If none, then the text from the
        /// &#96;value&#96; prop will be copied.
        ///</summary>
        static member targetId(p: string) = Prop(TargetId p )
        ///<summary>
        ///The text to  be copied to the clipboard if the &#96;target_id&#96; is None.
        ///</summary>
        static member content(p: string) = Prop(Content p)
        ///<summary>
        ///The number of times copy button was clicked
        ///</summary>
        static member nClicks(p: IConvertible) = Prop(NClicks p)
        ///<summary>
        ///The text shown as a tooltip when hovering over the copy icon.
        ///</summary>
        static member title(p: string) = Prop(Title p)
        ///<summary>
        ///The icon's styles
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///The class  name of the icon element
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = Prop(LoadingState p)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Guid) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: DashComponent) = Children([ value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: list<DashComponent>) = Children(value)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

    ///<summary>
    ///The Clipboard component copies text to the clipboard
    ///</summary>
    type Clipboard() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?targetId,
                ?content,
                ?nClicks,
                ?title,
                ?style,
                ?className,
                ?loadingState
            ) =
            (fun (t: Clipboard) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "targetId" (targetId |> Option.map box)
                DynObj.setValueOpt props "content" (content |> Option.map box)
                DynObj.setValueOpt props "nClicks" (nClicks |> Option.map box)
                DynObj.setValueOpt props "title" (title |> Option.map box)
                DynObj.setValueOpt props "style" (style |> Option.map box)
                DynObj.setValueOpt props "className" (className |> Option.map box)
                DynObj.setValueOpt props "loadingState" (loadingState |> Option.map box)
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Clipboard"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?targetId,
                ?content,
                ?nClicks,
                ?title,
                ?style,
                ?className,
                ?loadingState
            ) =
            Clipboard.applyMembers
                (id,
                 children,
                 ?targetId = targetId,
                 ?content = content,
                 ?nClicks = nClicks,
                 ?title = title,
                 ?style = style,
                 ?className = className,
                 ?loadingState = loadingState)
                (Clipboard())

    ///<summary>
    ///The Clipboard component copies text to the clipboard
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID used to identify this component.
    ///&#10;
    ///• target_id (string | record; default null) - The id of target component containing text to copy to the clipboard.
    ///The inner text of the &#96;children&#96; prop will be copied to the clipboard.  If none, then the text from the
    /// &#96;value&#96; prop will be copied.
    ///&#10;
    ///• content (string; default null) - The text to  be copied to the clipboard if the &#96;target_id&#96; is None.
    ///&#10;
    ///• n_clicks (number; default 0) - The number of times copy button was clicked
    ///&#10;
    ///• title (string) - The text shown as a tooltip when hovering over the copy icon.
    ///&#10;
    ///• style (record) - The icon's styles
    ///&#10;
    ///• className (string) - The class  name of the icon element
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    let clipboard (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Clipboard.init (id, children)

        let componentProps =
            match t.TryGetTypedValue<DashComponentProps> "props" with
            | Some (p) -> p
            | None -> DashComponentProps()

        Seq.iter
            (fun (prop: Prop) ->
                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
                DynObj.setValue componentProps fieldName boxedProp)
            props

        DynObj.setValue t "props" componentProps
        t :> DashComponent
