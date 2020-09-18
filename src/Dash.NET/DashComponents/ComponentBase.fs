namespace Dash.NET

open Newtonsoft.Json
open Plotly.NET

type DashComponent() = inherit DynamicObj()

type DashComponentStyle() = inherit DynamicObj()

type DashComponentProps() = inherit DynamicObj()

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

    type LoadingState = 
        {
            IsLoading : bool
            PropName : string
            ComponentName : string
        }
        static member create isLoading propName componentName = {IsLoading=isLoading; PropName=propName; ComponentName=componentName}

    type PersistenceTypeOptions =
        | Local
        | Session
        | Memory
        static member toString = function
            | Local     -> "local"
            | Session   -> "session"
            | Memory    -> "memory"
        static member convert = PersistenceTypeOptions.toString >> box

module HTMLPropTypes =

    type HTMLProps =
        | ClassName of string
    
        static member toDynamicMemberDef (prop:HTMLProps) =
            match prop with
            | ClassName p -> "className", box p