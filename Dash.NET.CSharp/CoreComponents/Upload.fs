namespace Dash.NET.CSharp.DCC

open System
open ComponentPropTypes
open Dash.NET.CSharp.ComponentStyle
///<summary>
///Upload components allow your app to accept user-uploaded files via drag'n'drop
///</summary>
[<RequireQualifiedAccess>]
module Upload =

    // Original attr
    type internal OAttr = Dash.NET.DCC.Upload.Attr


    type UploadContents = private WrappedUploadContents of Dash.NET.DCC.Upload.UploadContents with
        static member internal Wrap (attr : Dash.NET.DCC.Upload.UploadContents) = WrappedUploadContents attr
        static member internal Unwrap (attr : UploadContents) = match attr with | WrappedUploadContents attr -> attr

        static member SingleContent (value : string) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Upload.UploadContents.SingleContent value |> UploadContents.Wrap
        static member MultipleContent ([<ParamArray>] value : string array) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            Dash.NET.DCC.Upload.UploadContents.MultipleContent (value |> List.ofArray) |> UploadContents.Wrap


    type UploadFileName = private WrappedUploadFileName of Dash.NET.DCC.Upload.UploadFileName with
        static member internal Wrap (attr : Dash.NET.DCC.Upload.UploadFileName) = WrappedUploadFileName attr
        static member internal Unwrap (attr : UploadFileName) = match attr with | WrappedUploadFileName attr -> attr

        static member SingleFile (value : string) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Upload.UploadFileName.SingleFile value |> UploadFileName.Wrap
        static member MultipleFiles ([<ParamArray>] value : string array) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            Dash.NET.DCC.Upload.UploadFileName.MultipleFiles (value |> List.ofArray) |> UploadFileName.Wrap
        
        
    type UploadLastModified = private WrappedUploadLastModified of Dash.NET.DCC.Upload.UploadLastModified with
        static member internal Wrap (attr : Dash.NET.DCC.Upload.UploadLastModified) = WrappedUploadLastModified attr
        static member internal Unwrap (attr : UploadLastModified) = match attr with | WrappedUploadLastModified attr -> attr
        
        static member SingleTimeStamp (value : int) = 
            guardAgainstNull "value" value
            Dash.NET.DCC.Upload.UploadLastModified.SingleTimeStamp value |> UploadLastModified.Wrap
        static member MultipleTimeStamps ([<ParamArray>] value : int array) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            Dash.NET.DCC.Upload.UploadLastModified.MultipleTimeStamps (value |> List.ofArray) |> UploadLastModified.Wrap

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Upload.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Upload.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///The contents of the uploaded file as a binary string
        ///</summary>
        static member contents(p: UploadContents) = 
            guardAgainstNull "p" p
            OAttr.contents (p |> UploadContents.Unwrap) |> Attr.Wrap

        ///<summary>
        ///The name of the file(s) that was(were) uploaded.
        ///Note that this does not include the path of the file
        ///(for security reasons).
        ///</summary>
        static member filename(p: UploadFileName) = 
            guardAgainstNull "p" p
            OAttr.filename (p |> UploadFileName.Unwrap) |> Attr.Wrap

        ///<summary>
        ///The last modified date of the file that was uploaded in unix time
        ///(seconds since 1970).
        ///</summary>
        static member lastModified(p: UploadLastModified) = 
            guardAgainstNull "p" p
            OAttr.lastModified (p |> UploadLastModified.Unwrap) |> Attr.Wrap

        ///<summary>
        ///Allow specific types of files.
        ///See https://github.com/okonet/attr-accept for more information.
        ///Keep in mind that mime type determination is not reliable across
        ///platforms. CSV files, for example, are reported as text/plain
        ///under macOS but as application/vnd.ms-excel under Windows.
        ///In some cases there might not be a mime type set at all.
        ///See: https://github.com/react-dropzone/react-dropzone/issues/276
        ///</summary>
        static member accept(p: string) = 
            guardAgainstNull "p" p
            OAttr.accept p |> Attr.Wrap
        ///<summary>
        ///Enable/disable the upload component entirely
        ///</summary>
        static member disabled(p: bool) = 
            guardAgainstNull "p" p
            OAttr.disabled p |> Attr.Wrap
        ///<summary>
        ///Disallow clicking on the component to open the file dialog
        ///</summary>
        static member disableClick(p: bool) = 
            guardAgainstNull "p" p
            OAttr.disableClick p |> Attr.Wrap
        ///<summary>
        ///Maximum file size in bytes. If &#96;-1&#96;, then infinite
        ///</summary>
        static member maxSize(p: int) = 
            guardAgainstNull "p" p
            OAttr.maxSize p |> Attr.Wrap
        ///<summary>
        ///Minimum file size in bytes
        ///</summary>
        static member minSize(p: int) = 
            guardAgainstNull "p" p
            OAttr.minSize p |> Attr.Wrap
        ///<summary>
        ///Allow dropping multiple files
        ///</summary>
        static member multiple(p: bool) = 
            guardAgainstNull "p" p
            OAttr.multiple p |> Attr.Wrap
        ///<summary>
        ///HTML class name of the component
        ///</summary>
        static member className(p: string) = 
            guardAgainstNull "p" p
            OAttr.className p |> Attr.Wrap
        ///<summary>
        ///HTML class name of the component while active
        ///</summary>
        static member classNameActive(p: string) = 
            guardAgainstNull "p" p
            OAttr.classNameActive p |> Attr.Wrap
        ///<summary>
        ///HTML class name of the component if rejected
        ///</summary>
        static member classNameReject(p: string) = 
            guardAgainstNull "p" p
            OAttr.classNameReject p |> Attr.Wrap
        ///<summary>
        ///HTML class name of the component if disabled
        ///</summary>
        static member classNameDisabled(p: string) = 
            guardAgainstNull "p" p
            OAttr.classNameDisabled p |> Attr.Wrap
        ///<summary>
        ///CSS styles to apply
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Html.Style>) = 
            guardAgainstNull "p" p
            p |> Array.iter (guardAgainstNull "p")
            OAttr.style (p |> Array.map Dash.NET.CSharp.Html.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///CSS styles to apply while active
        ///</summary>
        static member styleActive(p: obj) = 
            guardAgainstNull "p" p
            OAttr.styleActive p |> Attr.Wrap
        ///<summary>
        ///CSS styles if rejected
        ///</summary>
        static member styleReject(p: obj) = 
            guardAgainstNull "p" p
            OAttr.styleReject p |> Attr.Wrap
        ///<summary>
        ///CSS styles if disabled
        ///</summary>
        static member styleDisabled(p: obj) = 
            guardAgainstNull "p" p
            OAttr.styleDisabled p |> Attr.Wrap
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = 
            guardAgainstNull "p" p
            OAttr.loadingState (p |> LoadingState.Convert) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Guid) = 
            guardAgainstNull "value" value
            OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Dash.NET.CSharp.Html.DashComponent) = 
            guardAgainstNull "value" value
            OAttr.children (value |> Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children([<ParamArray>] value: array<Dash.NET.CSharp.Html.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Array.iter (guardAgainstNull "value")
            OAttr.children (value |> Array.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<Dash.NET.CSharp.Html.DashComponent>) = 
            guardAgainstNull "value" value
            value |> Seq.iter (guardAgainstNull "value")
            OAttr.children (value |> Seq.map Dash.NET.CSharp.Html.DashComponent.Unwrap) |> Attr.Wrap

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
    let upload (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Upload.upload id (attrs |> List.ofArray |> List.map Attr.Unwrap)