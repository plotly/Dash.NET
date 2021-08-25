namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////ConfirmDialog is used to display the browser's native "confirm" modal,
/////with an optional message and two buttons ("OK" and "Cancel").
/////This ConfirmDialog can be used in conjunction with buttons when the user
/////is performing an action that should require an extra step of verification.
/////</summary>
//[<RequireQualifiedAccess>]
//module ConfirmDialog =
//    ///<summary>
//    ///• message (string) - Message to show in the popup.
//    ///&#10;
//    ///• submit_n_clicks (number; default 0) - Number of times the submit button was clicked
//    ///&#10;
//    ///• submit_n_clicks_timestamp (number; default -1) - Last time the submit button was clicked.
//    ///&#10;
//    ///• cancel_n_clicks (number; default 0) - Number of times the popup was canceled.
//    ///&#10;
//    ///• cancel_n_clicks_timestamp (number; default -1) - Last time the cancel button was clicked.
//    ///&#10;
//    ///• displayed (boolean) - Set to true to send the ConfirmDialog.
//    ///</summary>
//    type Prop =
//        | Message of string
//        | SubmitNClicks of IConvertible
//        | SubmitNClicksTimestamp of IConvertible
//        | CancelNClicks of IConvertible
//        | CancelNClicksTimestamp of IConvertible
//        | Displayed of bool
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Message (p) -> "message", box p
//            | SubmitNClicks (p) -> "submit_n_clicks", box p
//            | SubmitNClicksTimestamp (p) -> "submit_n_clicks_timestamp", box p
//            | CancelNClicks (p) -> "cancel_n_clicks", box p
//            | CancelNClicksTimestamp (p) -> "cancel_n_clicks_timestamp", box p
//            | Displayed (p) -> "displayed", box p

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
//        ///Number of times the submit button was clicked
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
//        ///Set to true to send the ConfirmDialog.
//        ///</summary>
//        static member displayed(p: bool) = Prop(Displayed p)
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
//    ///ConfirmDialog is used to display the browser's native "confirm" modal,
//    ///with an optional message and two buttons ("OK" and "Cancel").
//    ///This ConfirmDialog can be used in conjunction with buttons when the user
//    ///is performing an action that should require an extra step of verification.
//    ///</summary>
//    type ConfirmDialog() =
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
//                ?displayed: string
//            ) =
//            (fun (t: ConfirmDialog) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "message" message
//                DynObj.setValueOpt props "submit_n_clicks" submit_n_clicks
//                DynObj.setValueOpt props "submit_n_clicks_timestamp" submit_n_clicks_timestamp
//                DynObj.setValueOpt props "cancel_n_clicks" cancel_n_clicks
//                DynObj.setValueOpt props "cancel_n_clicks_timestamp" cancel_n_clicks_timestamp
//                DynObj.setValueOpt props "displayed" displayed
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "ConfirmDialog"
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
//                ?displayed: string
//            ) =
//            ConfirmDialog.applyMembers
//                (id,
//                 children,
//                 ?message = message,
//                 ?submit_n_clicks = submit_n_clicks,
//                 ?submit_n_clicks_timestamp = submit_n_clicks_timestamp,
//                 ?cancel_n_clicks = cancel_n_clicks,
//                 ?cancel_n_clicks_timestamp = cancel_n_clicks_timestamp,
//                 ?displayed = displayed)
//                (ConfirmDialog())

//    ///<summary>
//    ///ConfirmDialog is used to display the browser's native "confirm" modal,
//    ///with an optional message and two buttons ("OK" and "Cancel").
//    ///This ConfirmDialog can be used in conjunction with buttons when the user
//    ///is performing an action that should require an extra step of verification.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• message (string) - Message to show in the popup.
//    ///&#10;
//    ///• submit_n_clicks (number; default 0) - Number of times the submit button was clicked
//    ///&#10;
//    ///• submit_n_clicks_timestamp (number; default -1) - Last time the submit button was clicked.
//    ///&#10;
//    ///• cancel_n_clicks (number; default 0) - Number of times the popup was canceled.
//    ///&#10;
//    ///• cancel_n_clicks_timestamp (number; default -1) - Last time the cancel button was clicked.
//    ///&#10;
//    ///• displayed (boolean) - Set to true to send the ConfirmDialog.
//    ///</summary>
//    let confirmDialog (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = ConfirmDialog.init (id, children)

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
