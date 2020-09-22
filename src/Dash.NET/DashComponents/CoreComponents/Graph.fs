namespace Dash.NET.DCC_DSL

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes



[<RequireQualifiedAccess>]
module Graph =

    type GraphProps = 
        | Figure of Plotly.NET.GenericChart.Figure
        | Config of Plotly.NET.Config
        | ClickData of obj
        | ClickAnnotationData of obj
        | HoverData of obj
        | ClearOnUnhover of bool
        | SelectedData of obj
        | RelayoutData of obj
        | ExtendData of obj
        | PrependData of obj
        | RestyleData of obj
        | Style of obj
        | ClassName of string
        | Animate of bool
        | AnimationOptions of obj
        | LoadingState of LoadingState
        static member toDynamicMemberDef (prop:GraphProps)  =
            match prop with
            | ClickData           p -> "clickData"          , box p
            | ClickAnnotationData p -> "clickAnnotationData", box p
            | HoverData           p -> "hoverData"          , box p
            | ClearOnUnhover      p -> "clear_on_unhover"   , box p
            | SelectedData        p -> "selectedData"       , box p
            | RelayoutData        p -> "relayoutData"       , box p
            | ExtendData          p -> "extendData"         , box p
            | PrependData         p -> "prependData"        , box p
            | RestyleData         p -> "restyleData"        , box p
            | Figure              p -> "figure"             , box p
            | Style               p -> "style"              , box p
            | ClassName           p -> "className"          , box p
            | Animate             p -> "animate"            , box p
            | AnimationOptions    p -> "animation_options"  , box p
            | Config              p -> "config"             , box p
            | LoadingState        p -> "loading_state"      , box p

    type Graph() =
        inherit DashComponent()
        static member applyMembers 
            (
                id                  : string,
                children            : seq<DashComponent>,
                ?Figure             : Plotly.NET.GenericChart.Figure,
                ?Config             : Plotly.NET.Config,
                ?ClickData          : obj,
                ?ClickAnnotationData: obj,
                ?HoverData          : obj,
                ?ClearOnUnhover     : bool,
                ?SelectedData       : obj,
                ?RelayoutData       : obj,
                ?ExtendData         : obj,
                ?PrependData        : obj,
                ?RestyleData        : obj,
                ?Style              : obj,
                ?ClassName          : string,
                ?Animate            : bool,
                ?AnimationOptions   : obj,
                ?LoadingState       : LoadingState
            ) =
            (
                fun (g:Graph) ->
                    
                    let props = DashComponentProps()

                    id                  |> DynObj.setValue props "id"
                    children            |> DynObj.setValue props "children"
                    ClickData           |> DynObj.setValueOpt props "clickData"          
                    ClickAnnotationData |> DynObj.setValueOpt props "clickAnnotationData"
                    HoverData           |> DynObj.setValueOpt props "hoverData"          
                    ClearOnUnhover      |> DynObj.setValueOpt props "clear_on_unhover"   
                    SelectedData        |> DynObj.setValueOpt props "selectedData"       
                    RelayoutData        |> DynObj.setValueOpt props "relayoutData"       
                    ExtendData          |> DynObj.setValueOpt props "extendData"         
                    PrependData         |> DynObj.setValueOpt props "prependData"        
                    RestyleData         |> DynObj.setValueOpt props "restyleData"        
                    Figure              |> DynObj.setValueOpt props "figure"             
                    Style               |> DynObj.setValueOpt props "style"              
                    ClassName           |> DynObj.setValueOpt props "className"          
                    Animate             |> DynObj.setValueOpt props "animate"            
                    AnimationOptions    |> DynObj.setValueOpt props "animation_options"  
                    Config              |> DynObj.setValueOpt props "config"             
                    LoadingState        |> DynObj.setValueOpt props "loading_state"      

                    DynObj.setValue g "namespace" "dash_core_components"
                    DynObj.setValue g "props" props
                    DynObj.setValue g "type" "Graph"

                    g
            )
        static member init 
            (
                id                  ,
                children            ,
                ?Figure             ,
                ?Config             ,
                ?ClickData          ,
                ?ClickAnnotationData,
                ?HoverData          ,
                ?ClearOnUnhover     ,
                ?SelectedData       ,
                ?RelayoutData       ,
                ?ExtendData         ,
                ?PrependData        ,
                ?RestyleData        ,
                ?Style              ,
                ?ClassName          ,
                ?Animate            ,
                ?AnimationOptions   ,
                ?LoadingState       
            ) =
            Graph()
            |> Graph.applyMembers
                (
                    id                 ,
                    children           ,
                    ?Figure             = Figure             ,
                    ?Config             = Config             ,
                    ?ClickData          = ClickData          ,
                    ?ClickAnnotationData= ClickAnnotationData,
                    ?HoverData          = HoverData          ,
                    ?ClearOnUnhover     = ClearOnUnhover     ,
                    ?SelectedData       = SelectedData       ,
                    ?RelayoutData       = RelayoutData       ,
                    ?ExtendData         = ExtendData         ,
                    ?PrependData        = PrependData        ,
                    ?RestyleData        = RestyleData        ,
                    ?Style              = Style              ,
                    ?ClassName          = ClassName          ,
                    ?Animate            = Animate            ,
                    ?AnimationOptions   = AnimationOptions   ,
                    ?LoadingState       = LoadingState       
                ) 

    let graph (id:string) (props:seq<GraphProps>) (children:seq<DashComponent>) =
        let g = Graph.init(id,children)
        let componentProps = 
            match (g.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> GraphProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue g "props" 
        g :> DashComponent