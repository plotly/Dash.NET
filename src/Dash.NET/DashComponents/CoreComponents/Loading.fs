namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////A Loading component that wraps any other component and displays a spinner until the wrapped component has rendered.
/////</summary>
//[<RequireQualifiedAccess>]
//module Loading =
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
//    ///value equal to: 'graph', 'cube', 'circle', 'dot', 'default'
//    ///</summary>
//    type TypeType =
//        | Graph
//        | Cube
//        | Circle
//        | Dot
//        | Default
//        member this.Convert() =
//            match this with
//            | Graph -> "graph"
//            | Cube -> "cube"
//            | Circle -> "circle"
//            | Dot -> "dot"
//            | Default -> "default"

//    ///<summary>
//    ///• children (list with values of type: a list of or a singular dash component, string or number | a list of or a singular dash component, string or number) - Array that holds components to render
//    ///&#10;
//    ///• type (value equal to: 'graph', 'cube', 'circle', 'dot', 'default'; default default) - Property that determines which spinner to show
//    ///one of 'graph', 'cube', 'circle', 'dot', or 'default'.
//    ///&#10;
//    ///• fullscreen (boolean) - Boolean that makes the spinner display full-screen
//    ///&#10;
//    ///• debug (boolean) - If true, the spinner will display the component_name and prop_name
//    ///while loading
//    ///&#10;
//    ///• className (string) - Additional CSS class for the spinner root DOM node
//    ///&#10;
//    ///• parent_className (string) - Additional CSS class for the outermost dcc.Loading parent div DOM node
//    ///&#10;
//    ///• style (record) - Additional CSS styling for the spinner root DOM node
//    ///&#10;
//    ///• parent_style (record) - Additional CSS styling for the outermost dcc.Loading parent div DOM node
//    ///&#10;
//    ///• color (string; default #119DFF) - Primary colour used for the loading spinners
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    type Prop =
//        | Type of TypeType
//        | Fullscreen of bool
//        | Debug of bool
//        | ClassName of string
//        | ParentClassName of string
//        | Style of obj
//        | ParentStyle of obj
//        | Color of string
//        | LoadingState of LoadingStateType
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Type (p) -> "type", box (p.Convert())
//            | Fullscreen (p) -> "fullscreen", box p
//            | Debug (p) -> "debug", box p
//            | ClassName (p) -> "className", box p
//            | ParentClassName (p) -> "parent_className", box p
//            | Style (p) -> "style", box p
//            | ParentStyle (p) -> "parent_style", box p
//            | Color (p) -> "color", box p
//            | LoadingState (p) -> "loading_state", box (p.Convert())

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///Property that determines which spinner to show
//        ///one of 'graph', 'cube', 'circle', 'dot', or 'default'.
//        ///</summary>
//        static member ``type``(p: TypeType) = Prop(Type p)
//        ///<summary>
//        ///Boolean that makes the spinner display full-screen
//        ///</summary>
//        static member fullscreen(p: bool) = Prop(Fullscreen p)
//        ///<summary>
//        ///If true, the spinner will display the component_name and prop_name
//        ///while loading
//        ///</summary>
//        static member debug(p: bool) = Prop(Debug p)
//        ///<summary>
//        ///Additional CSS class for the spinner root DOM node
//        ///</summary>
//        static member className(p: string) = Prop(ClassName p)
//        ///<summary>
//        ///Additional CSS class for the outermost dcc.Loading parent div DOM node
//        ///</summary>
//        static member parentClassName(p: string) = Prop(ParentClassName p)
//        ///<summary>
//        ///Additional CSS styling for the spinner root DOM node
//        ///</summary>
//        static member style(p: obj) = Prop(Style p)
//        ///<summary>
//        ///Additional CSS styling for the outermost dcc.Loading parent div DOM node
//        ///</summary>
//        static member parentStyle(p: obj) = Prop(ParentStyle p)
//        ///<summary>
//        ///Primary colour used for the loading spinners
//        ///</summary>
//        static member color(p: string) = Prop(Color p)
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
//    ///A Loading component that wraps any other component and displays a spinner until the wrapped component has rendered.
//    ///</summary>
//    type Loading() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?``type``: string,
//                ?fullscreen: string,
//                ?debug: string,
//                ?className: string,
//                ?parent_className: string,
//                ?style: string,
//                ?parent_style: string,
//                ?color: string,
//                ?loading_state: string
//            ) =
//            (fun (t: Loading) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "type" ``type``
//                DynObj.setValueOpt props "fullscreen" fullscreen
//                DynObj.setValueOpt props "debug" debug
//                DynObj.setValueOpt props "className" className
//                DynObj.setValueOpt props "parent_className" parent_className
//                DynObj.setValueOpt props "style" style
//                DynObj.setValueOpt props "parent_style" parent_style
//                DynObj.setValueOpt props "color" color
//                DynObj.setValueOpt props "loading_state" loading_state
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "Loading"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?``type``: string,
//                ?fullscreen: string,
//                ?debug: string,
//                ?className: string,
//                ?parent_className: string,
//                ?style: string,
//                ?parent_style: string,
//                ?color: string,
//                ?loading_state: string
//            ) =
//            Loading.applyMembers
//                (id,
//                 children,
//                 ?``type`` = ``type``,
//                 ?fullscreen = fullscreen,
//                 ?debug = debug,
//                 ?className = className,
//                 ?parent_className = parent_className,
//                 ?style = style,
//                 ?parent_style = parent_style,
//                 ?color = color,
//                 ?loading_state = loading_state)
//                (Loading())

//    ///<summary>
//    ///A Loading component that wraps any other component and displays a spinner until the wrapped component has rendered.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• children (list with values of type: a list of or a singular dash component, string or number | a list of or a singular dash component, string or number) - Array that holds components to render
//    ///&#10;
//    ///• type (value equal to: 'graph', 'cube', 'circle', 'dot', 'default'; default default) - Property that determines which spinner to show
//    ///one of 'graph', 'cube', 'circle', 'dot', or 'default'.
//    ///&#10;
//    ///• fullscreen (boolean) - Boolean that makes the spinner display full-screen
//    ///&#10;
//    ///• debug (boolean) - If true, the spinner will display the component_name and prop_name
//    ///while loading
//    ///&#10;
//    ///• className (string) - Additional CSS class for the spinner root DOM node
//    ///&#10;
//    ///• parent_className (string) - Additional CSS class for the outermost dcc.Loading parent div DOM node
//    ///&#10;
//    ///• style (record) - Additional CSS styling for the spinner root DOM node
//    ///&#10;
//    ///• parent_style (record) - Additional CSS styling for the outermost dcc.Loading parent div DOM node
//    ///&#10;
//    ///• color (string; default #119DFF) - Primary colour used for the loading spinners
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    let loading (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = Loading.init (id, children)

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
