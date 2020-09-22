namespace Dash.NET.DCC_DSL

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module Input =

    type InputProps =
        | Value of IConvertible
        | Style of DashComponentStyle
        | ClassName of string
        | Debounce of bool
        | Type of InputType
        | AutoComplete of string
        | AutoFocus of bool
        | Disabled of bool
        | Mode of InputMode
        | List of string
        | Max of IConvertible
        | MaxLength of IConvertible
        | Min of IConvertible
        | MinLength of IConvertible
        | Multiple of bool
        | Name of string
        | Pattern of string
        | Placeholder of IConvertible
        | ReadOnly of bool
        | Required of bool
        | SelectionDirection of string
        | SelectionEnd of string
        | SelectionStart of string
        | Size of string
        | Spellcheck of SpellCheckOptions
        | Step of IConvertible
        | NSubmit of IConvertible
        | NSubmitTimestamp of IConvertible
        | NBlur of IConvertible
        | NBlurTimestamp of IConvertible
        | SetPtops of obj
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions
        static member toDynamicMemberDef (prop:InputProps) =
            match prop with
            | Value p               -> "value", box p
            | Style p               -> "style", box p
            | ClassName p           -> "className", box p
            | Debounce p            -> "debounce", box p
            | Type p                -> "type", InputType.convert p
            | AutoComplete p        -> "autoComplete", box p
            | AutoFocus p           -> "autoFocus", box p
            | Disabled p            -> "disabled", box p
            | Mode p                -> "inputMode", InputMode.convert p
            | List p                -> "list", box p
            | Max p                 -> "max", box p
            | MaxLength p           -> "maxLength", box p
            | Min p                 -> "min", box p
            | MinLength p           -> "minLength", box p
            | Multiple p            -> "multiple", box p
            | Name p                -> "name", box p
            | Pattern p             -> "pattern", box p
            | Placeholder p         -> "placeholder", box p
            | ReadOnly p            -> "readOnly", box p
            | Required p            -> "required", box p
            | SelectionDirection p  -> "selectionDirection", box p 
            | SelectionEnd p        -> "selectionEnd", box p
            | SelectionStart p      -> "selectionStart", box p
            | Size p                -> "size", box p
            | Spellcheck p          -> "spellCheck", SpellCheckOptions.convert p
            | Step p                -> "step", box p
            | NSubmit p             -> "n_submit", box p
            | NSubmitTimestamp p    -> "n_submit_timestamp", box p
            | NBlur p               -> "n_blur", box p
            | NBlurTimestamp p      -> "n_blur_timestamp", box p
            | SetPtops p            -> "setProps", box p
            | LoadingState p        -> "loading_state", box p
            | Persistence p         -> "persistence", box p
            | PersistedProps p      -> "persisted_props", box p
            | PersistenceType p     -> "persistence_type", PersistenceTypeOptions.convert p

    type Input() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?Value               : IConvertible,
                ?Style               : DashComponentStyle ,
                ?ClassName           : string,
                ?Debounce            : bool,
                ?Type                : InputType,
                ?AutoComplete        : string,
                ?AutoFocus           : bool,
                ?Disabled            : bool,
                ?Mode                : InputMode,
                ?List                : string,
                ?Max                 : IConvertible,
                ?MaxLength           : IConvertible,
                ?Min                 : IConvertible,
                ?MinLength           : IConvertible,
                ?Multiple            : bool,
                ?Name                : string,
                ?Pattern             : string,
                ?Placeholder         : IConvertible,
                ?ReadOnly            : bool,
                ?Required            : bool,
                ?SelectionDirection  : string,
                ?SelectionEnd        : string,
                ?SelectionStart      : string,
                ?Size                : string,
                ?Spellcheck          : SpellCheckOptions,
                ?Step                : IConvertible,
                ?NSubmit             : IConvertible,
                ?NSubmitTimestamp    : IConvertible,
                ?NBlur               : IConvertible,
                ?NBlurTimestamp      : IConvertible,
                ?SetProps            : obj,
                ?LoadingState        : LoadingState,
                ?Persistence         : IConvertible,
                ?PersistedProps      : string [],
                ?PersistenceType     : PersistenceTypeOptions
            ) =
            (
                fun (i:Input) -> 

                    let props = DashComponentProps()

                    id                  |> DynObj.setValue props "id"
                    children            |> DynObj.setValue props "children"
                    Value               |> DynObj.setValueOpt props "value"
                    Style               |> DynObj.setValueOpt props "style"
                    ClassName           |> DynObj.setValueOpt props "className"
                    Debounce            |> DynObj.setValueOpt props "debounce"
                    Type                |> DynObj.setValueOptBy props "type" InputType.convert
                    AutoComplete        |> DynObj.setValueOpt props "autoComplete"
                    AutoFocus           |> DynObj.setValueOpt props "autoFocus"
                    Disabled            |> DynObj.setValueOpt props "disabled"
                    Mode                |> DynObj.setValueOptBy props "inputMode" InputMode.convert 
                    List                |> DynObj.setValueOpt props "list"
                    Max                 |> DynObj.setValueOpt props "max"
                    MaxLength           |> DynObj.setValueOpt props "maxLength"
                    Min                 |> DynObj.setValueOpt props "min"
                    MinLength           |> DynObj.setValueOpt props "minLength"
                    Multiple            |> DynObj.setValueOpt props "multiple"
                    Name                |> DynObj.setValueOpt props "name"
                    Pattern             |> DynObj.setValueOpt props "pattern"
                    Placeholder         |> DynObj.setValueOpt props "placeholder"
                    ReadOnly            |> DynObj.setValueOpt props "readOnly"
                    Required            |> DynObj.setValueOpt props "required"
                    SelectionDirection  |> DynObj.setValueOpt props "selectionDirection"
                    SelectionEnd        |> DynObj.setValueOpt props "selectionEnd"
                    SelectionStart      |> DynObj.setValueOpt props "selectionStart"
                    Size                |> DynObj.setValueOpt props "size"
                    Spellcheck          |> DynObj.setValueOptBy props "spellCheck" SpellCheckOptions.convert 
                    Step                |> DynObj.setValueOpt props "step"
                    NSubmit             |> DynObj.setValueOpt props "n_submit"
                    NSubmitTimestamp    |> DynObj.setValueOpt props "n_submit_timestamp"
                    NBlur               |> DynObj.setValueOpt props "n_blur"
                    NBlurTimestamp      |> DynObj.setValueOpt props "n_blur_timestamp"
                    SetProps            |> DynObj.setValueOpt props "setProps"
                    LoadingState        |> DynObj.setValueOpt props "loading_state"
                    Persistence         |> DynObj.setValueOpt props "persistence"
                    PersistedProps      |> DynObj.setValueOpt props "persisted_props"
                    PersistenceType     |> DynObj.setValueOptBy props "persistence_type" PersistenceTypeOptions.convert

                    DynObj.setValue i "namespace" "dash_core_components"
                    DynObj.setValue i "props" props
                    DynObj.setValue i "type" "Input"

                    i

            )
        static member init 
            (
                id,
                children,
                ?Value,
                ?Style,
                ?ClassName,
                ?Debounce,
                ?Type,
                ?AutoComplete,
                ?AutoFocus,
                ?Disabled,
                ?Mode,
                ?List,
                ?Max,
                ?MaxLength,
                ?Min,
                ?MinLength,
                ?Multiple,
                ?Name,
                ?Pattern,
                ?Placeholder,
                ?ReadOnly,
                ?Required,
                ?SelectionDirection,
                ?SelectionEnd,
                ?SelectionStart,
                ?Size,
                ?Spellcheck,
                ?Step,
                ?NSubmit,
                ?NSubmitTimestamp,
                ?NBlur,
                ?NBlurTimestamp,
                ?SetPtops,
                ?LoadingState,
                ?Persistence,
                ?PersistedProps,
                ?PersistenceType        
            ) = 
                Input()
                |> Input.applyMembers 
                    (
                        id,
                        children,
                        ?Value               = Value             ,
                        ?Style               = Style             ,
                        ?ClassName           = ClassName         ,
                        ?Debounce            = Debounce          ,
                        ?Type                = Type              ,
                        ?AutoComplete        = AutoComplete      ,
                        ?AutoFocus           = AutoFocus         ,
                        ?Disabled            = Disabled          ,
                        ?Mode                = Mode              ,
                        ?List                = List              ,
                        ?Max                 = Max               ,
                        ?MaxLength           = MaxLength         ,
                        ?Min                 = Min               ,
                        ?MinLength           = MinLength         ,
                        ?Multiple            = Multiple          ,
                        ?Name                = Name              ,
                        ?Pattern             = Pattern           ,
                        ?Placeholder         = Placeholder       ,
                        ?ReadOnly            = ReadOnly          ,
                        ?Required            = Required          ,
                        ?SelectionDirection  = SelectionDirection,
                        ?SelectionEnd        = SelectionEnd      ,
                        ?SelectionStart      = SelectionStart    ,
                        ?Size                = Size              ,
                        ?Spellcheck          = Spellcheck        ,
                        ?Step                = Step              ,
                        ?NSubmit             = NSubmit           ,
                        ?NSubmitTimestamp    = NSubmitTimestamp  ,
                        ?NBlur               = NBlur             ,
                        ?NBlurTimestamp      = NBlurTimestamp    ,
                        ?SetProps            = SetPtops          ,
                        ?LoadingState        = LoadingState      ,
                        ?Persistence         = Persistence       ,
                        ?PersistedProps      = PersistedProps    ,
                        ?PersistenceType     = PersistenceType   
                    )

    let input (id:string) (props:seq<InputProps>) (children:seq<DashComponent>) =
        let i = Input.init(id,children)
        let componentProps = 
            match (i.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> InputProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue i "props" 
        i :> DashComponent