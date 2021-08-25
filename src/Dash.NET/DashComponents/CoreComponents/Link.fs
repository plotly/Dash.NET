namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////Link allows you to create a clickable link within a multi-page app.
/////For links with destinations outside the current app, &#96;html.A&#96; is a better
/////component to use.
/////</summary>
//[<RequireQualifiedAccess>]
//module Link =
//    ///<summary>
//    ///record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)'
//    ///</summary>
//    type LoadingStateType =
//        { IsLoading: Option<bool>
//          PropName: Option<string>
//          ComponentName: Option<string> }
//        member this.Convert() =
//            box
//                {| is_loading = this.IsLoading
//                   prop_name = this.PropName
//                   component_name = this.ComponentName |}

//    ///<summary>
//    ///• href (string) - The URL of a linked resource.
//    ///&#10;
//    ///• refresh (boolean; default false) - Controls whether or not the page will refresh when the link is clicked
//    ///&#10;
//    ///• className (string) - Often used with CSS to style elements with common properties.
//    ///&#10;
//    ///• style (record) - Defines CSS styles which will override styles previously set.
//    ///&#10;
//    ///• title (string) - Adds the title attribute to your link, which can contain supplementary
//    ///information.
//    ///&#10;
//    ///• target (string) - Specifies where to open the link reference.
//    ///&#10;
//    ///• children (a list of or a singular dash component, string or number) - The children of this component
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    type Prop =
//        | Href of string
//        | Refresh of bool
//        | ClassName of string
//        | Style of obj
//        | Title of string
//        | Target of string
//        | LoadingState of LoadingStateType
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Href (p) -> "href", box p
//            | Refresh (p) -> "refresh", box p
//            | ClassName (p) -> "className", box p
//            | Style (p) -> "style", box p
//            | Title (p) -> "title", box p
//            | Target (p) -> "target", box p
//            | LoadingState (p) -> "loading_state", box (p.Convert())

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///The URL of a linked resource.
//        ///</summary>
//        static member href(p: string) = Prop(Href p)
//        ///<summary>
//        ///Controls whether or not the page will refresh when the link is clicked
//        ///</summary>
//        static member refresh(p: bool) = Prop(Refresh p)
//        ///<summary>
//        ///Often used with CSS to style elements with common properties.
//        ///</summary>
//        static member className(p: string) = Prop(ClassName p)
//        ///<summary>
//        ///Defines CSS styles which will override styles previously set.
//        ///</summary>
//        static member style(p: obj) = Prop(Style p)
//        ///<summary>
//        ///Adds the title attribute to your link, which can contain supplementary
//        ///information.
//        ///</summary>
//        static member title(p: string) = Prop(Title p)
//        ///<summary>
//        ///Specifies where to open the link reference.
//        ///</summary>
//        static member target(p: string) = Prop(Target p)
//        ///<summary>
//        ///Object that holds the loading state object coming from dash-renderer
//        ///</summary>
//        static member loadingState(p: LoadingStateType) = Prop(LoadingState p)
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
//    ///Link allows you to create a clickable link within a multi-page app.
//    ///For links with destinations outside the current app, &#96;html.A&#96; is a better
//    ///component to use.
//    ///</summary>
//    type Link() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?href: string,
//                ?refresh: string,
//                ?className: string,
//                ?style: string,
//                ?title: string,
//                ?target: string,
//                ?loading_state: string
//            ) =
//            (fun (t: Link) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "href" href
//                DynObj.setValueOpt props "refresh" refresh
//                DynObj.setValueOpt props "className" className
//                DynObj.setValueOpt props "style" style
//                DynObj.setValueOpt props "title" title
//                DynObj.setValueOpt props "target" target
//                DynObj.setValueOpt props "loading_state" loading_state
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "Link"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?href: string,
//                ?refresh: string,
//                ?className: string,
//                ?style: string,
//                ?title: string,
//                ?target: string,
//                ?loading_state: string
//            ) =
//            Link.applyMembers
//                (id,
//                 children,
//                 ?href = href,
//                 ?refresh = refresh,
//                 ?className = className,
//                 ?style = style,
//                 ?title = title,
//                 ?target = target,
//                 ?loading_state = loading_state)
//                (Link())

//    ///<summary>
//    ///Link allows you to create a clickable link within a multi-page app.
//    ///For links with destinations outside the current app, &#96;html.A&#96; is a better
//    ///component to use.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• href (string) - The URL of a linked resource.
//    ///&#10;
//    ///• refresh (boolean; default false) - Controls whether or not the page will refresh when the link is clicked
//    ///&#10;
//    ///• className (string) - Often used with CSS to style elements with common properties.
//    ///&#10;
//    ///• style (record) - Defines CSS styles which will override styles previously set.
//    ///&#10;
//    ///• title (string) - Adds the title attribute to your link, which can contain supplementary
//    ///information.
//    ///&#10;
//    ///• target (string) - Specifies where to open the link reference.
//    ///&#10;
//    ///• children (a list of or a singular dash component, string or number) - The children of this component
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    let link (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = Link.init (id, children)

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
