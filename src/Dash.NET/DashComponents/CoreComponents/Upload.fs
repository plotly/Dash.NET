namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

///<summary>
///Upload components allow your app to accept user-uploaded files via drag'n'drop
///</summary>
[<RequireQualifiedAccess>]
module Upload =
    type UploadContents =
        | SingleContent of string
        | MultipleContent of string list
        static member convert = function
            | SingleContent p   -> box p
            | MultipleContent p -> box p
    
    type UploadFileName =
        | SingleFile of string
        | MultipleFiles of string list
        static member convert = function
            | SingleFile p      -> box p
            | MultipleFiles p   -> box p
    
    type UploadLastModified =
        | SingleTimeStamp of int
        | MultipleTimeStamps of int list
        static member convert = function
            | SingleTimeStamp p    -> box p
            | MultipleTimeStamps p -> box p

    ///<summary>
    ///• contents (string | list with values of type: string) - The contents of the uploaded file as a binary string
    ///&#10;
    ///• filename (string | list with values of type: string) - The name of the file(s) that was(were) uploaded.
    ///Note that this does not include the path of the file
    ///(for security reasons).
    ///&#10;
    ///• last_modified (number | list with values of type: number) - The last modified date of the file that was uploaded in unix time
    ///(seconds since 1970).
    ///&#10;
    ///• children (a list of or a singular dash component, string or number | string) - Contents of the upload component
    ///&#10;
    ///• accept (string) - Allow specific types of files.
    ///See https://github.com/okonet/attr-accept for more information.
    ///Keep in mind that mime type determination is not reliable across
    ///platforms. CSV files, for example, are reported as text/plain
    ///under macOS but as application/vnd.ms-excel under Windows.
    ///In some cases there might not be a mime type set at all.
    ///See: https://github.com/react-dropzone/react-dropzone/issues/276
    ///&#10;
    ///• disabled (boolean; default false) - Enable/disable the upload component entirely
    ///&#10;
    ///• disable_click (boolean; default false) - Disallow clicking on the component to open the file dialog
    ///&#10;
    ///• max_size (number; default -1) - Maximum file size in bytes. If &#96;-1&#96;, then infinite
    ///&#10;
    ///• min_size (number; default 0) - Minimum file size in bytes
    ///&#10;
    ///• multiple (boolean; default false) - Allow dropping multiple files
    ///&#10;
    ///• className (string) - HTML class name of the component
    ///&#10;
    ///• className_active (string) - HTML class name of the component while active
    ///&#10;
    ///• className_reject (string) - HTML class name of the component if rejected
    ///&#10;
    ///• className_disabled (string) - HTML class name of the component if disabled
    ///&#10;
    ///• style (record; default {}) - CSS styles to apply
    ///&#10;
    ///• style_active (record; default {
    ///    borderStyle: 'solid',
    ///    borderColor: '#6c6',
    ///    backgroundColor: '#eee',
    ///}) - CSS styles to apply while active
    ///&#10;
    ///• style_reject (record; default {
    ///    borderStyle: 'solid',
    ///    borderColor: '#c66',
    ///    backgroundColor: '#eee',
    ///}) - CSS styles if rejected
    ///&#10;
    ///• style_disabled (record; default {
    ///    opacity: 0.5,
    ///}) - CSS styles if disabled
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    type Prop =
        | ClassName of string
        | Contents of UploadContents
        | Filename of UploadFileName
        | LastModified of UploadLastModified
        | Accept of string
        | Disabled of bool
        | DisableClick of bool
        | MaxSize of int
        | MinSize of int
        | Multiple of bool
        | ClassNameActive of string
        | ClassNameReject of string
        | ClassNameDisabled of string
        | Style of DashComponentStyle
        | StyleActive of obj
        | StyleReject of obj
        | StyleDisabled of obj
        | LoadingState of LoadingState
        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | ClassName p            -> "className", box p
            | Contents p             -> "contents"           , UploadContents.convert p
            | Filename p             -> "filename"           , UploadFileName.convert p
            | LastModified p         -> "last_modified"      , UploadLastModified.convert p
            | Accept p               -> "accept"             , box p
            | Disabled p             -> "disabled"           , box p
            | DisableClick p         -> "disable_click"      , box p
            | MaxSize p              -> "max_size"           , box p
            | MinSize p              -> "min_size"           , box p
            | Multiple p             -> "multiple"           , box p
            | ClassNameActive p      -> "className_active"   , box p
            | ClassNameReject p      -> "className_reject"   , box p
            | ClassNameDisabled p    -> "className_disabled" , box p
            | Style p                -> "style"              , box p
            | StyleActive p          -> "style_active"       , box p
            | StyleReject p          -> "style_reject"       , box p
            | StyleDisabled p        -> "style_disabled"     , box p
            | LoadingState p         -> "loading_state"      , box p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The contents of the uploaded file as a binary string
        ///</summary>
        static member contents(p: UploadContents) = Prop(Contents p)

        ///<summary>
        ///The name of the file(s) that was(were) uploaded.
        ///Note that this does not include the path of the file
        ///(for security reasons).
        ///</summary>
        static member filename(p: UploadFileName) = Prop(Filename p)

        ///<summary>
        ///The last modified date of the file that was uploaded in unix time
        ///(seconds since 1970).
        ///</summary>
        static member lastModified(p: UploadLastModified) = Prop(LastModified p)

        ///<summary>
        ///Allow specific types of files.
        ///See https://github.com/okonet/attr-accept for more information.
        ///Keep in mind that mime type determination is not reliable across
        ///platforms. CSV files, for example, are reported as text/plain
        ///under macOS but as application/vnd.ms-excel under Windows.
        ///In some cases there might not be a mime type set at all.
        ///See: https://github.com/react-dropzone/react-dropzone/issues/276
        ///</summary>
        static member accept(p: string) = Prop(Accept p)
        ///<summary>
        ///Enable/disable the upload component entirely
        ///</summary>
        static member disabled(p: bool) = Prop(Disabled p)
        ///<summary>
        ///Disallow clicking on the component to open the file dialog
        ///</summary>
        static member disableClick(p: bool) = Prop(DisableClick p)
        ///<summary>
        ///Maximum file size in bytes. If &#96;-1&#96;, then infinite
        ///</summary>
        static member maxSize(p: int) = Prop(MaxSize p)
        ///<summary>
        ///Minimum file size in bytes
        ///</summary>
        static member minSize(p: int) = Prop(MinSize p)
        ///<summary>
        ///Allow dropping multiple files
        ///</summary>
        static member multiple(p: bool) = Prop(Multiple p)
        ///<summary>
        ///HTML class name of the component
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///HTML class name of the component while active
        ///</summary>
        static member classNameActive(p: string) = Prop(ClassNameActive p)
        ///<summary>
        ///HTML class name of the component if rejected
        ///</summary>
        static member classNameReject(p: string) = Prop(ClassNameReject p)
        ///<summary>
        ///HTML class name of the component if disabled
        ///</summary>
        static member classNameDisabled(p: string) = Prop(ClassNameDisabled p)
        ///<summary>
        ///CSS styles to apply
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///CSS styles to apply while active
        ///</summary>
        static member styleActive(p: obj) = Prop(StyleActive p)
        ///<summary>
        ///CSS styles if rejected
        ///</summary>
        static member styleReject(p: obj) = Prop(StyleReject p)
        ///<summary>
        ///CSS styles if disabled
        ///</summary>
        static member styleDisabled(p: obj) = Prop(StyleDisabled p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = Prop(LoadingState p)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) = Children([ Html.Html.text value ])
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
    ///Upload components allow your app to accept user-uploaded files via drag'n'drop
    ///</summary>
    type Upload() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?contents: string,
                ?filename: string,
                ?last_modified: string,
                ?accept: string,
                ?disabled: string,
                ?disable_click: string,
                ?max_size: string,
                ?min_size: string,
                ?multiple: string,
                ?className: string,
                ?className_active: string,
                ?className_reject: string,
                ?className_disabled: string,
                ?style: string,
                ?style_active: string,
                ?style_reject: string,
                ?style_disabled: string,
                ?loading_state: string
            ) =
            (fun (t: Upload) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "contents" contents
                DynObj.setValueOpt props "filename" filename
                DynObj.setValueOpt props "last_modified" last_modified
                DynObj.setValueOpt props "accept" accept
                DynObj.setValueOpt props "disabled" disabled
                DynObj.setValueOpt props "disable_click" disable_click
                DynObj.setValueOpt props "max_size" max_size
                DynObj.setValueOpt props "min_size" min_size
                DynObj.setValueOpt props "multiple" multiple
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "className_active" className_active
                DynObj.setValueOpt props "className_reject" className_reject
                DynObj.setValueOpt props "className_disabled" className_disabled
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "style_active" style_active
                DynObj.setValueOpt props "style_reject" style_reject
                DynObj.setValueOpt props "style_disabled" style_disabled
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Upload"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?contents: string,
                ?filename: string,
                ?last_modified: string,
                ?accept: string,
                ?disabled: string,
                ?disable_click: string,
                ?max_size: string,
                ?min_size: string,
                ?multiple: string,
                ?className: string,
                ?className_active: string,
                ?className_reject: string,
                ?className_disabled: string,
                ?style: string,
                ?style_active: string,
                ?style_reject: string,
                ?style_disabled: string,
                ?loading_state: string
            ) =
            Upload.applyMembers
                (id,
                 children,
                 ?contents = contents,
                 ?filename = filename,
                 ?last_modified = last_modified,
                 ?accept = accept,
                 ?disabled = disabled,
                 ?disable_click = disable_click,
                 ?max_size = max_size,
                 ?min_size = min_size,
                 ?multiple = multiple,
                 ?className = className,
                 ?className_active = className_active,
                 ?className_reject = className_reject,
                 ?className_disabled = className_disabled,
                 ?style = style,
                 ?style_active = style_active,
                 ?style_reject = style_reject,
                 ?style_disabled = style_disabled,
                 ?loading_state = loading_state)
                (Upload())

    ///<summary>
    ///Upload components allow your app to accept user-uploaded files via drag'n'drop
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• contents (string | list with values of type: string) - The contents of the uploaded file as a binary string
    ///&#10;
    ///• filename (string | list with values of type: string) - The name of the file(s) that was(were) uploaded.
    ///Note that this does not include the path of the file
    ///(for security reasons).
    ///&#10;
    ///• last_modified (number | list with values of type: number) - The last modified date of the file that was uploaded in unix time
    ///(seconds since 1970).
    ///&#10;
    ///• children (a list of or a singular dash component, string or number | string) - Contents of the upload component
    ///&#10;
    ///• accept (string) - Allow specific types of files.
    ///See https://github.com/okonet/attr-accept for more information.
    ///Keep in mind that mime type determination is not reliable across
    ///platforms. CSV files, for example, are reported as text/plain
    ///under macOS but as application/vnd.ms-excel under Windows.
    ///In some cases there might not be a mime type set at all.
    ///See: https://github.com/react-dropzone/react-dropzone/issues/276
    ///&#10;
    ///• disabled (boolean; default false) - Enable/disable the upload component entirely
    ///&#10;
    ///• disable_click (boolean; default false) - Disallow clicking on the component to open the file dialog
    ///&#10;
    ///• max_size (number; default -1) - Maximum file size in bytes. If &#96;-1&#96;, then infinite
    ///&#10;
    ///• min_size (number; default 0) - Minimum file size in bytes
    ///&#10;
    ///• multiple (boolean; default false) - Allow dropping multiple files
    ///&#10;
    ///• className (string) - HTML class name of the component
    ///&#10;
    ///• className_active (string) - HTML class name of the component while active
    ///&#10;
    ///• className_reject (string) - HTML class name of the component if rejected
    ///&#10;
    ///• className_disabled (string) - HTML class name of the component if disabled
    ///&#10;
    ///• style (record; default {}) - CSS styles to apply
    ///&#10;
    ///• style_active (record; default {
    ///    borderStyle: 'solid',
    ///    borderColor: '#6c6',
    ///    backgroundColor: '#eee',
    ///}) - CSS styles to apply while active
    ///&#10;
    ///• style_reject (record; default {
    ///    borderStyle: 'solid',
    ///    borderColor: '#c66',
    ///    backgroundColor: '#eee',
    ///}) - CSS styles if rejected
    ///&#10;
    ///• style_disabled (record; default {
    ///    opacity: 0.5,
    ///}) - CSS styles if disabled
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///</summary>
    let upload (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Upload.init (id, children)

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
