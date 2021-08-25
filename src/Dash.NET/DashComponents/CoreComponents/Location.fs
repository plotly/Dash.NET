namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////Update and track the current window.location object through the window.history state.
/////Use in conjunction with the &#96;dash_core_components.Link&#96; component to make apps with multiple pages.
/////</summary>
//[<RequireQualifiedAccess>]
//module Location =
//    ///<summary>
//    ///• pathname (string) - pathname in window.location - e.g., "/my/full/pathname"
//    ///&#10;
//    ///• search (string) - search in window.location - e.g., "?myargument=1"
//    ///&#10;
//    ///• hash (string) - hash in window.location - e.g., "#myhash"
//    ///&#10;
//    ///• href (string) - href in window.location - e.g., "/my/full/pathname?myargument=1#myhash"
//    ///&#10;
//    ///• refresh (boolean; default true) - Refresh the page when the location is updated?
//    ///</summary>
//    type Prop =
//        | Pathname of string
//        | Search of string
//        | Hash of string
//        | Href of string
//        | Refresh of bool
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Pathname (p) -> "pathname", box p
//            | Search (p) -> "search", box p
//            | Hash (p) -> "hash", box p
//            | Href (p) -> "href", box p
//            | Refresh (p) -> "refresh", box p

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///pathname in window.location - e.g., "/my/full/pathname"
//        ///</summary>
//        static member pathname(p: string) = Prop(Pathname p)
//        ///<summary>
//        ///search in window.location - e.g., "?myargument=1"
//        ///</summary>
//        static member search(p: string) = Prop(Search p)
//        ///<summary>
//        ///hash in window.location - e.g., "#myhash"
//        ///</summary>
//        static member hash(p: string) = Prop(Hash p)
//        ///<summary>
//        ///href in window.location - e.g., "/my/full/pathname?myargument=1#myhash"
//        ///</summary>
//        static member href(p: string) = Prop(Href p)
//        ///<summary>
//        ///Refresh the page when the location is updated?
//        ///</summary>
//        static member refresh(p: bool) = Prop(Refresh p)
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
//    ///Update and track the current window.location object through the window.history state.
//    ///Use in conjunction with the &#96;dash_core_components.Link&#96; component to make apps with multiple pages.
//    ///</summary>
//    type Location() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?pathname: string,
//                ?search: string,
//                ?hash: string,
//                ?href: string,
//                ?refresh: string
//            ) =
//            (fun (t: Location) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "pathname" pathname
//                DynObj.setValueOpt props "search" search
//                DynObj.setValueOpt props "hash" hash
//                DynObj.setValueOpt props "href" href
//                DynObj.setValueOpt props "refresh" refresh
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "Location"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?pathname: string,
//                ?search: string,
//                ?hash: string,
//                ?href: string,
//                ?refresh: string
//            ) =
//            Location.applyMembers
//                (id, children, ?pathname = pathname, ?search = search, ?hash = hash, ?href = href, ?refresh = refresh)
//                (Location())

//    ///<summary>
//    ///Update and track the current window.location object through the window.history state.
//    ///Use in conjunction with the &#96;dash_core_components.Link&#96; component to make apps with multiple pages.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• pathname (string) - pathname in window.location - e.g., "/my/full/pathname"
//    ///&#10;
//    ///• search (string) - search in window.location - e.g., "?myargument=1"
//    ///&#10;
//    ///• hash (string) - hash in window.location - e.g., "#myhash"
//    ///&#10;
//    ///• href (string) - href in window.location - e.g., "/my/full/pathname?myargument=1#myhash"
//    ///&#10;
//    ///• refresh (boolean; default true) - Refresh the page when the location is updated?
//    ///</summary>
//    let location (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = Location.init (id, children)

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
