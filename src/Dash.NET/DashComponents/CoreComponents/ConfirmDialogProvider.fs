namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////A wrapper component that will display a confirmation dialog
/////when its child component has been clicked on.
/////For example:
/////&#96;&#96;&#96;
/////dcc.ConfirmDialogProvider(
/////    html.Button('click me', id='btn'),
/////    message='Danger - Are you sure you want to continue.'
/////    id='confirm')
/////&#96;&#96;&#96;
/////</summary>
//[<RequireQualifiedAccess>]
//module ConfirmDialogProvider =
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
//    ///• message (string) - Message to show in the popup.
//    ///&#10;
//    ///• submit_n_clicks (number; default 0) - Number of times the submit was clicked
//    ///&#10;
//    ///• submit_n_clicks_timestamp (number; default -1) - Last time the submit button was clicked.
//    ///&#10;
//    ///• cancel_n_clicks (number; default 0) - Number of times the popup was canceled.
//    ///&#10;
//    ///• cancel_n_clicks_timestamp (number; default -1) - Last time the cancel button was clicked.
//    ///&#10;
//    ///• displayed (boolean) - Is the modal currently displayed.
//    ///&#10;
//    ///• children (boolean | number | string | record | list) - The children to hijack clicks from and display the popup.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    type Prop =
//        | Message of string
//        | SubmitNClicks of IConvertible
//        | SubmitNClicksTimestamp of IConvertible
//        | CancelNClicks of IConvertible
//        | CancelNClicksTimestamp of IConvertible
//        | Displayed of bool
//        | LoadingState of LoadingStateType
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Message (p) -> "message", box p
//            | SubmitNClicks (p) -> "submit_n_clicks", box p
//            | SubmitNClicksTimestamp (p) -> "submit_n_clicks_timestamp", box p
//            | CancelNClicks (p) -> "cancel_n_clicks", box p
//            | CancelNClicksTimestamp (p) -> "cancel_n_clicks_timestamp", box p
//            | Displayed (p) -> "displayed", box p
//            | LoadingState (p) -> "loading_state", box (p.Convert())

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///Message to show in the popup.
//        ///</summary>
//        static member message(p: string) = Prop(Message p)
//        ///<summary>
//        ///Number of times the submit was clicked
//        ///</summary>
//        static member submitNClicks(p: IConvertible) = Prop(SubmitNClicks p)
//        ///<summary>
//        ///Last time the submit button was clicked.
//        ///</summary>
//        static member submitNClicksTimestamp(p: IConvertible) = Prop(SubmitNClicksTimestamp p)
//        ///<summary>
//        ///Number of times the popup was canceled.
//        ///</summary>
//        static member cancelNClicks(p: IConvertible) = Prop(CancelNClicks p)
//        ///<summary>
//        ///Last time the cancel button was clicked.
//        ///</summary>
//        static member cancelNClicksTimestamp(p: IConvertible) = Prop(CancelNClicksTimestamp p)
//        ///<summary>
//        ///Is the modal currently displayed.
//        ///</summary>
//        static member displayed(p: bool) = Prop(Displayed p)
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
//    ///A wrapper component that will display a confirmation dialog
//    ///when its child component has been clicked on.
//    ///For example:
//    ///&#96;&#96;&#96;
//    ///dcc.ConfirmDialogProvider(
//    ///    html.Button('click me', id='btn'),
//    ///    message='Danger - Are you sure you want to continue.'
//    ///    id='confirm')
//    ///&#96;&#96;&#96;
//    ///</summary>
//    type ConfirmDialogProvider() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?message: string,
//                ?submit_n_clicks: string,
//                ?submit_n_clicks_timestamp: string,
//                ?cancel_n_clicks: string,
//                ?cancel_n_clicks_timestamp: string,
//                ?displayed: string,
//                ?loading_state: string
//            ) =
//            (fun (t: ConfirmDialogProvider) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "message" message
//                DynObj.setValueOpt props "submit_n_clicks" submit_n_clicks
//                DynObj.setValueOpt props "submit_n_clicks_timestamp" submit_n_clicks_timestamp
//                DynObj.setValueOpt props "cancel_n_clicks" cancel_n_clicks
//                DynObj.setValueOpt props "cancel_n_clicks_timestamp" cancel_n_clicks_timestamp
//                DynObj.setValueOpt props "displayed" displayed
//                DynObj.setValueOpt props "loading_state" loading_state
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "ConfirmDialogProvider"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?message: string,
//                ?submit_n_clicks: string,
//                ?submit_n_clicks_timestamp: string,
//                ?cancel_n_clicks: string,
//                ?cancel_n_clicks_timestamp: string,
//                ?displayed: string,
//                ?loading_state: string
//            ) =
//            ConfirmDialogProvider.applyMembers
//                (id,
//                 children,
//                 ?message = message,
//                 ?submit_n_clicks = submit_n_clicks,
//                 ?submit_n_clicks_timestamp = submit_n_clicks_timestamp,
//                 ?cancel_n_clicks = cancel_n_clicks,
//                 ?cancel_n_clicks_timestamp = cancel_n_clicks_timestamp,
//                 ?displayed = displayed,
//                 ?loading_state = loading_state)
//                (ConfirmDialogProvider())

//    ///<summary>
//    ///A wrapper component that will display a confirmation dialog
//    ///when its child component has been clicked on.
//    ///For example:
//    ///&#96;&#96;&#96;
//    ///dcc.ConfirmDialogProvider(
//    ///    html.Button('click me', id='btn'),
//    ///    message='Danger - Are you sure you want to continue.'
//    ///    id='confirm')
//    ///&#96;&#96;&#96;
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• message (string) - Message to show in the popup.
//    ///&#10;
//    ///• submit_n_clicks (number; default 0) - Number of times the submit was clicked
//    ///&#10;
//    ///• submit_n_clicks_timestamp (number; default -1) - Last time the submit button was clicked.
//    ///&#10;
//    ///• cancel_n_clicks (number; default 0) - Number of times the popup was canceled.
//    ///&#10;
//    ///• cancel_n_clicks_timestamp (number; default -1) - Last time the cancel button was clicked.
//    ///&#10;
//    ///• displayed (boolean) - Is the modal currently displayed.
//    ///&#10;
//    ///• children (boolean | number | string | record | list) - The children to hijack clicks from and display the popup.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    let confirmDialogProvider (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t =
//            ConfirmDialogProvider.init (id, children)

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
