namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////The Download component opens a download dialog when the data property changes.
/////</summary>
//[<RequireQualifiedAccess>]
//module Download =
//    ///<summary>
//    ///record with the fields: 'filename: string (required)', 'content: string (required)', 'base64: boolean (optional)', 'type: string (optional)'
//    ///</summary>
//    type DataType =
//        { Filename: string
//          Content: string
//          Base64: Option<bool>
//          Type: Option<string> }
//        member this.Convert() =
//            box
//                {| filename = this.Filename
//                   content = this.Content
//                   base64 = this.Base64
//                   ``type`` = this.Type |}

//    ///<summary>
//    ///• data (record with the fields: 'filename: string (required)', 'content: string (required)', 'base64: boolean (optional)', 'type: string (optional)') - On change, a download is invoked.
//    ///&#10;
//    ///• base64 (boolean; default false) - Default value for base64, used when not set as part of the data property.
//    ///&#10;
//    ///• type (string; default text/plain) - Default value for type, used when not set as part of the data property.
//    ///</summary>
//    type Prop =
//        | Data of DataType
//        | Base64 of bool
//        | Type of string
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Data (p) -> "data", box (p.Convert())
//            | Base64 (p) -> "base64", box p
//            | Type (p) -> "type", box p

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///On change, a download is invoked.
//        ///</summary>
//        static member data(p: DataType) = Prop(Data p)
//        ///<summary>
//        ///Default value for base64, used when not set as part of the data property.
//        ///</summary>
//        static member base64(p: bool) = Prop(Base64 p)
//        ///<summary>
//        ///Default value for type, used when not set as part of the data property.
//        ///</summary>
//        static member ``type``(p: string) = Prop(Type p)
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
//    ///The Download component opens a download dialog when the data property changes.
//    ///</summary>
//    type Download() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?data: string,
//                ?base64: string,
//                ?``type``: string
//            ) =
//            (fun (t: Download) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "data" data
//                DynObj.setValueOpt props "base64" base64
//                DynObj.setValueOpt props "type" ``type``
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "Download"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?data: string,
//                ?base64: string,
//                ?``type``: string
//            ) =
//            Download.applyMembers (id, children, ?data = data, ?base64 = base64, ?``type`` = ``type``) (Download())

//    ///<summary>
//    ///The Download component opens a download dialog when the data property changes.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components in callbacks.
//    ///&#10;
//    ///• data (record with the fields: 'filename: string (required)', 'content: string (required)', 'base64: boolean (optional)', 'type: string (optional)') - On change, a download is invoked.
//    ///&#10;
//    ///• base64 (boolean; default false) - Default value for base64, used when not set as part of the data property.
//    ///&#10;
//    ///• type (string; default text/plain) - Default value for type, used when not set as part of the data property.
//    ///</summary>
//    let download (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = Download.init (id, children)

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
