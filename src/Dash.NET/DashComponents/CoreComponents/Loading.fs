namespace Dash.NET.DCC

open System
open DynamicObj
open Dash.NET
open Dash.NET.Common

///<summary>
///A Loading component that wraps any other component and displays a spinner until the wrapped component has rendered.
///</summary>
[<RequireQualifiedAccess>]
module Loading =
    ///<summary>
    ///value equal to: 'graph', 'cube', 'circle', 'dot', 'default'
    ///</summary>
    type LoadingType =
        | Graph
        | Cube
        | Circle
        | Dot
        | Default

        static member convert =
            DU.convertAsString

    type PropName =
        | Type
        | Fullscreen
        | Debug
        | ClassName
        | ParentClassName
        | Style
        | ParentStyle
        | Color
        | LoadingState

        member this.toString () =
            match this with
            | ClassName         _   -> "className"
            | ParentClassName   _   -> "parentClassName"
            | prop                  -> prop |> Prop.createName |> NamingStrategy.toSnakeCase

    ///<summary>
    ///• children (list with values of type: a list of or a singular dash component, string or number | a list of or a singular dash component, string or number) - Array that holds components to render
    ///&#10;
    ///• type (value equal to: 'graph', 'cube', 'circle', 'dot', 'default'; default default) - Property that determines which spinner to show
    ///one of 'graph', 'cube', 'circle', 'dot', or 'default'.
    ///&#10;
    ///• fullscreen (boolean) - Boolean that makes the spinner display full-screen
    ///&#10;
    ///• debug (boolean) - If true, the spinner will display the component_name and prop_name
    ///while loading
    ///&#10;
    ///• className (string) - Additional CSS class for the spinner root DOM node
    ///&#10;
    ///• parent_className (string) - Additional CSS class for the outermost dcc.Loading parent div DOM node
    ///&#10;
    ///• style (record) - Additional CSS styling for the spinner root DOM node
    ///&#10;
    ///• parent_style (record) - Additional CSS styling for the outermost dcc.Loading parent div DOM node
    ///&#10;
    ///• color (string; default #119DFF) - Primary colour used for the loading spinners
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    type Prop =
        | Type of LoadingType
        | Fullscreen of bool
        | Debug of bool
        | ClassName of string
        | ParentClassName of string
        | Style of DashComponentStyle
        | ParentStyle of DashComponentStyle
        | Color of string
        | LoadingState of LoadingState

        static member convert = function
            | Type              p -> LoadingType.convert p
            | Fullscreen        p -> box p
            | Debug             p -> box p
            | ClassName         p -> box p
            | ParentClassName   p -> box p
            | Style             p -> box p
            | ParentStyle       p -> box p
            | Color             p -> box p
            | LoadingState      p -> box p

        static member toPropName = function
            | Type              _ -> PropName.Type
            | Fullscreen        _ -> PropName.Fullscreen
            | Debug             _ -> PropName.Debug
            | ClassName         _ -> PropName.ClassName
            | ParentClassName   _ -> PropName.ParentClassName
            | Style             _ -> PropName.Style
            | ParentStyle       _ -> PropName.ParentStyle
            | Color             _ -> PropName.Color
            | LoadingState      _ -> PropName.LoadingState

        static member toDynamicMemberDef prop =
            prop |> Prop.toPropName |> fun cp -> cp.toString()
            , Prop.convert prop

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///Property that determines which spinner to show
        ///one of 'graph', 'cube', 'circle', 'dot', or 'default'.
        ///</summary>
        static member loadingType(p: LoadingType) = Prop(Type p)
        ///<summary>
        ///Boolean that makes the spinner display full-screen
        ///</summary>
        static member fullscreen(p: bool) = Prop(Fullscreen p)
        ///<summary>
        ///If true, the spinner will display the component_name and prop_name
        ///while loading
        ///</summary>
        static member debug(p: bool) = Prop(Debug p)
        ///<summary>
        ///Additional CSS class for the spinner root DOM node
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Additional CSS class for the outermost dcc.Loading parent div DOM node
        ///</summary>
        static member parentClassName(p: string) = Prop(ParentClassName p)
        ///<summary>
        ///Additional CSS styling for the spinner root DOM node
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Additional CSS styling for the outermost dcc.Loading parent div DOM node
        ///</summary>
        static member parentStyle(p: seq<Css.Style>) = Prop(ParentStyle(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Primary colour used for the loading spinners
        ///</summary>
        static member color(p: string) = Prop(Color p)
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
    ///A Loading component that wraps any other component and displays a spinner until the wrapped component has rendered.
    ///</summary>
    type Loading() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?loadingType: LoadingType,
                ?fullscreen,
                ?debug,
                ?className,
                ?parentClassName,
                ?style,
                ?parentStyle,
                ?color,
                ?loadingState
            ) =
            (fun (t: Loading) ->
                let props = DashComponentProps()
                let setPropValueOpt prop =
                    DynObj.setPropValueOpt props Prop.convert prop

                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                setPropValueOpt Type loadingType
                setPropValueOpt Fullscreen fullscreen
                setPropValueOpt Debug debug
                setPropValueOpt ClassName className
                setPropValueOpt ParentClassName parentClassName
                setPropValueOpt Style style
                setPropValueOpt ParentStyle parentStyle
                setPropValueOpt Color color
                setPropValueOpt LoadingState loadingState
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Loading"
                t)

        static member init
            (
                id,
                children,
                ?loadingType,
                ?fullscreen,
                ?debug,
                ?className,
                ?parentClassName,
                ?style,
                ?parentStyle,
                ?color,
                ?loadingState
            ) =
            Loading.applyMembers
                (id,
                 children,
                 ?loadingType = loadingType,
                 ?fullscreen = fullscreen,
                 ?debug = debug,
                 ?className = className,
                 ?parentClassName = parentClassName,
                 ?style = style,
                 ?parentStyle = parentStyle,
                 ?color = color,
                 ?loadingState = loadingState)
                (Loading())

    ///<summary>
    ///A Loading component that wraps any other component and displays a spinner until the wrapped component has rendered.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• children (list with values of type: a list of or a singular dash component, string or number | a list of or a singular dash component, string or number) - Array that holds components to render
    ///&#10;
    ///• type (value equal to: 'graph', 'cube', 'circle', 'dot', 'default'; default default) - Property that determines which spinner to show
    ///one of 'graph', 'cube', 'circle', 'dot', or 'default'.
    ///&#10;
    ///• fullscreen (boolean) - Boolean that makes the spinner display full-screen
    ///&#10;
    ///• debug (boolean) - If true, the spinner will display the component_name and prop_name
    ///while loading
    ///&#10;
    ///• className (string) - Additional CSS class for the spinner root DOM node
    ///&#10;
    ///• parent_className (string) - Additional CSS class for the outermost dcc.Loading parent div DOM node
    ///&#10;
    ///• style (record) - Additional CSS styling for the spinner root DOM node
    ///&#10;
    ///• parent_style (record) - Additional CSS styling for the outermost dcc.Loading parent div DOM node
    ///&#10;
    ///• color (string; default #119DFF) - Primary colour used for the loading spinners
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    let loading (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Loading.init (id, children)

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
