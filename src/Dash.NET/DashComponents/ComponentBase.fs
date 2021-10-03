namespace Dash.NET

open Newtonsoft.Json
open System.Runtime.InteropServices
open System
open DynamicObj

type LoadableComponentDefinition = 
    { ComponentName: string
      ComponentJavascript: string list }

type DashComponent
    (
        [<Optional;DefaultParameterValue(false)>]IsRawString:bool
    ) = 
    inherit DynamicObj()
    new() = DashComponent(false)
    [<JsonIgnore()>] member internal _.IsRawString = IsRawString
    
    static member internal transformChildren (children:seq<DashComponent>) =

        let getInnerHTML (c:DashComponent) = 
            match c.TryGetTypedValue<string>("innerHTML") with
            | Some s -> s
            | None -> ""
        
        children
        |> List.ofSeq 
        |> List.map (fun comp ->
            if comp.IsRawString then
                getInnerHTML comp |> box
            else
                box comp 
        )

type DashComponentProps() = inherit DynamicObj()

[<AutoOpen>]
module ComponentPropTypes = 

    type InputType =
        | Text
        | Number
        | Password
        | Email
        | Range
        | Search
        | Tel
        | Url
        | Hidden
        static member toString = function
            | Text -> "text"
            | Number -> "number"
            | Password -> "password"
            | Email -> "email"
            | Range -> "range"
            | Search -> "search"
            | Tel -> "tel"
            | Url -> "url"
            | Hidden -> "hidden"
        static member convert = InputType.toString >> box

    type InputMode = 
        | Verbatim
        | Latin
        | LatinName
        | LatinProse
        | FullWidthLatin
        | Kana
        | Katakana
        | Numeric
        | Tel
        | Email
        | Url
        static member toString = function
            | Verbatim          -> "verbatim"
            | Latin             -> "latin"
            | LatinName         -> "latin-name"
            | LatinProse        -> "latin-prose"
            | FullWidthLatin    -> "full-width-latin"
            | Kana              -> "kana"
            | Katakana          -> "katakana"
            | Numeric           -> "numeric"
            | Tel               -> "tel"
            | Email             -> "email"
            | Url               -> "url"
        static member convert = InputMode.toString >> box
        
    type SpellCheckOptions =
        | True
        | False
        static member toString = function
            | True -> "true"
            | False -> "false"
        static member convert = SpellCheckOptions.toString >> box

    type PersistenceTypeOptions =
        | Local
        | Session
        | Memory
        static member toString = function
            | Local     -> "local"
            | Session   -> "session"
            | Memory    -> "memory"
        static member convert = PersistenceTypeOptions.toString >> box

    type LoadingState () =
        inherit DynamicObj()
        static member create 
            (
                isLoading       : bool,
                ?PropName       : string,
                ?ComponentName  : string
            ) =
                let ls = LoadingState()

                isLoading       |> DynObj.setValue ls "isLoading"
                PropName        |> DynObj.setValueOpt ls "propName"
                ComponentName   |> DynObj.setValueOpt ls "componentName"

                ls
       

    type DropdownOption () =
        inherit DynamicObj()
        static member create 
            (
                label:IConvertible,
                value:IConvertible,
                ?Disabled:bool,
                ?Title:string
            ) =
                let dro = DropdownOption()

                label   |> DynObj.setValue dro "label"
                value   |> DynObj.setValue dro "value"
                Disabled|> DynObj.setValueOpt dro "disabled"
                Title   |> DynObj.setValueOpt dro "title"

                dro

    type RadioItemsOption () =
        inherit DynamicObj()
        static member create 
            (
                label:IConvertible,
                value:IConvertible,
                ?Disabled:bool
            ) =
                let dro = DropdownOption()

                label   |> DynObj.setValue dro "label"
                value   |> DynObj.setValue dro "value"
                Disabled|> DynObj.setValueOpt dro "disabled"

                dro

    type TabColors () =
        inherit DynamicObj()
        static member create 
            (
                ?Border      :string,
                ?Primary     :string,
                ?Background  :string
            ) =
                let tc = DropdownOption()

                Border    |> DynObj.setValueOpt tc "border"
                Primary   |> DynObj.setValueOpt tc "primary"
                Background|> DynObj.setValueOpt tc "background"

                tc
      

    type ChecklistOption = RadioItemsOption

type ComponentProperty =
    | Children
    | Value
    | N_Clicks
    | CustomProperty of string

    static member toPropertyName (ctp:ComponentProperty) =
        match ctp with
        | Children      -> "children"
        | Value         -> "value"
        | N_Clicks      -> "n_clicks"
        | CustomProperty p  -> p