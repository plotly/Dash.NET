//---
//ComponentName: Upload
//camelCaseComponentName: upload
//ComponentChar: u
//ComponentNamespace: dash_core_components
//ComponentType: Upload
//---

namespace Dash.NET.DCC_DSL

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

//Not sure if these are needed, as these properties most likely only matter on runtime
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

[<RequireQualifiedAccess>]
module Upload =

   type UploadProps =
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
       | Style of obj
       | StyleActive of obj
       | StyleReject of obj
       | StyleDisabled of obj
       | LoadingState of LoadingState
       static member toDynamicMemberDef (prop:UploadProps) =
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

   type Upload() =
       inherit DashComponent()
       static member applyMembers
           (
               id                   : string,
               children             : seq<DashComponent>,
               ?ClassName           : string,
               ?Contents            : UploadContents,
               ?Filename            : UploadFileName,
               ?LastModified        : UploadLastModified,
               ?Accept              : string,
               ?Disabled            : bool,
               ?DisableClick        : bool,
               ?MaxSize             : int,
               ?MinSize             : int,
               ?Multiple            : bool,
               ?ClassNameActive     : string,
               ?ClassNameReject     : string,
               ?ClassNameDisabled   : string,
               ?Style               : obj,
               ?StyleActive         : obj,
               ?StyleReject         : obj,
               ?StyleDisabled       : obj,
               ?LoadingState        : LoadingState
           ) =
           (
               fun (u:Upload) -> 

                   let props = DashComponentProps()

                   id                  |> DynObj.setValue props "id"
                   children            |> DynObj.setValue props "children"
                   ClassName           |> DynObj.setValueOpt props "className"
                   Contents            |> DynObj.setValueOptBy props "contents" UploadContents.convert   
                   Filename            |> DynObj.setValueOptBy props "filename" UploadFileName.convert   
                   LastModified        |> DynObj.setValueOptBy props "last_modified" UploadLastModified.convert
                   Accept              |> DynObj.setValueOpt props "accept"            
                   Disabled            |> DynObj.setValueOpt props "disabled"          
                   DisableClick        |> DynObj.setValueOpt props "disable_click"     
                   MaxSize             |> DynObj.setValueOpt props "max_size"          
                   MinSize             |> DynObj.setValueOpt props "min_size"          
                   Multiple            |> DynObj.setValueOpt props "multiple"          
                   ClassNameActive     |> DynObj.setValueOpt props "className_active"  
                   ClassNameReject     |> DynObj.setValueOpt props "className_reject"  
                   ClassNameDisabled   |> DynObj.setValueOpt props "className_disabled"
                   Style               |> DynObj.setValueOpt props "style"             
                   StyleActive         |> DynObj.setValueOpt props "style_active"      
                   StyleReject         |> DynObj.setValueOpt props "style_reject"      
                   StyleDisabled       |> DynObj.setValueOpt props "style_disabled"    
                   LoadingState        |> DynObj.setValueOpt props "loading_state"     

                   DynObj.setValue u "namespace" "dash_core_components"
                   DynObj.setValue u "props" props
                   DynObj.setValue u "type" "Upload"

                   u

           )
       static member init 
           (
                id,
                children,
                ?ClassName,
                ?Contents         ,
                ?Filename         ,
                ?LastModified     ,
                ?Accept           ,
                ?Disabled         ,
                ?DisableClick     ,
                ?MaxSize          ,
                ?MinSize          ,
                ?Multiple         ,
                ?ClassNameActive  ,
                ?ClassNameReject  ,
                ?ClassNameDisabled,
                ?Style            ,
                ?StyleActive      ,
                ?StyleReject      ,
                ?StyleDisabled    ,
                ?LoadingState     
           ) = 
               Upload()
               |> Upload.applyMembers 
                   (
                        id,
                        children,
                        ?ClassName = ClassName,
                        ?Contents         = Contents         ,
                        ?Filename         = Filename         ,
                        ?LastModified     = LastModified     ,
                        ?Accept           = Accept           ,
                        ?Disabled         = Disabled         ,
                        ?DisableClick     = DisableClick     ,
                        ?MaxSize          = MaxSize          ,
                        ?MinSize          = MinSize          ,
                        ?Multiple         = Multiple         ,
                        ?ClassNameActive  = ClassNameActive  ,
                        ?ClassNameReject  = ClassNameReject  ,
                        ?ClassNameDisabled= ClassNameDisabled,
                        ?Style            = Style            ,
                        ?StyleActive      = StyleActive      ,
                        ?StyleReject      = StyleReject      ,
                        ?StyleDisabled    = StyleDisabled    ,
                        ?LoadingState     = LoadingState     
                   )

   let upload (id:string) (props:seq<UploadProps>) (children:seq<DashComponent>) =
       let u = Upload.init(id,children)
       let componentProps = 
           match (u.TryGetTypedValue<DashComponentProps>("props")) with
           | Some p -> p
           | None -> DashComponentProps()
       props
       |> Seq.iter (fun prop ->
           let fieldName,boxedProp = prop |> UploadProps.toDynamicMemberDef
           boxedProp |> DynObj.setValue componentProps fieldName
       )
       componentProps |> DynObj.setValue u "props" 
       u :> DashComponent