//---
//ComponentName: Dropdown
//camelCaseComponentName: dropdown
//ComponentChar: d
//ComponentNamespace: dash_core_components
//ComponentType: Dropdown
//LibraryNamespace: Dash.NET.DCC
//---

namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

[<RequireQualifiedAccess>]
module Dropdown =

    type DropdownValue =
        | SingleValue of IConvertible
        | MultipleValues of seq<IConvertible>
        static member convert = function
            | SingleValue    p -> box p
            | MultipleValues p -> box p

    type DropdownProps =
        | ClassName of string
        | Options of seq<DropdownOption>
        | Value of DropdownValue
        | OptionHeight of float
        | Clearable of bool
        | Disabled of bool
        | Multi of bool
        | Placeholder of string
        | Searchable of bool
        | SearchValue of bool
        | Style of DashComponentStyle
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions

        static member toDynamicMemberDef (prop:DropdownProps) =
            match prop with
            | ClassName p       -> "className", box p
            | Options p         -> "options", box p
            | Value p           -> "value", p |> DropdownValue.convert
            | OptionHeight p    -> "optionHeight", box p
            | Clearable p       -> "clearable", box p
            | Disabled p        -> "disabled", box p
            | Multi p           -> "multi", box p
            | Placeholder p     -> "placeholder", box p
            | Searchable p      -> "searchable", box p
            | SearchValue p     -> "search_value", box p
            | Style p           -> "style", box p
            | LoadingState p    -> "loading_state", box p
            | Persistence p     -> "persistence", box p
            | PersistedProps p  -> "persisted_props", box p
            | PersistenceType p -> "persistence_type", PersistenceTypeOptions.convert p

    type Dropdown() =
        inherit DashComponent()
        static member applyMembers
            (
                id : string,
                children : seq<DashComponent>,
                ?ClassName : string,
                ?Options : seq<DropdownOption>,
                ?Value : DropdownValue,
                ?OptionHeight : float,
                ?Clearable : bool,
                ?Disabled : bool,
                ?Multi : bool,
                ?Placeholder : string,
                ?Searchable : bool,
                ?SearchValue : bool,
                ?Style : DashComponentStyle,
                ?LoadingState : LoadingState,
                ?Persistence : IConvertible,
                ?PersistedProps : string [],
                ?PersistenceType : PersistenceTypeOptions
            ) =
            (
                fun (d:Dropdown) -> 

                    let props = DashComponentProps()

                    id |> DynObj.setValue props "id"
                    children |> DynObj.setValue props "children"
                    ClassName       |> DynObj.setValueOpt props "className"
                    Options         |> DynObj.setValueOpt props "options"
                    Value           |> DynObj.setValueOpt props "value"
                    OptionHeight    |> DynObj.setValueOpt props "optionHeight"
                    Clearable       |> DynObj.setValueOpt props "clearable"
                    Disabled        |> DynObj.setValueOpt props "disabled"
                    Multi           |> DynObj.setValueOpt props "multi"
                    Placeholder     |> DynObj.setValueOpt props "placeholder"
                    Searchable      |> DynObj.setValueOpt props "searchable"
                    SearchValue     |> DynObj.setValueOpt props "search_value"
                    Style           |> DynObj.setValueOpt props "style"
                    LoadingState    |> DynObj.setValueOpt props "loading_state"
                    Persistence     |> DynObj.setValueOpt props "persistence"
                    PersistedProps  |> DynObj.setValueOpt props "persisted_props"
                    PersistenceType |> DynObj.setValueOpt props "persistence_type"

                    DynObj.setValue d "namespace" "dash_core_components"
                    DynObj.setValue d "props" props
                    DynObj.setValue d "type" "Dropdown"

                    d

            )
        static member init 
            (
                id,
                children,
                ?ClassName,
                ?Options,
                ?Value,
                ?OptionHeight,
                ?Clearable,
                ?Disabled,
                ?Multi,
                ?Placeholder,
                ?Searchable,
                ?SearchValue ,
                ?Style,
                ?LoadingState,
                ?Persistence,
                ?PersistedProps,
                ?PersistenceType
            ) = 
                Dropdown()
                |> Dropdown.applyMembers 
                    (
                        id,
                        children,
                        ?ClassName       = ClassName,
                        ?Options         = Options,
                        ?Value           = Value,
                        ?OptionHeight    = OptionHeight,
                        ?Clearable       = Clearable,
                        ?Disabled        = Disabled,
                        ?Multi           = Multi,
                        ?Placeholder     = Placeholder,
                        ?Searchable      = Searchable,
                        ?SearchValue     = SearchValue ,
                        ?Style           = Style,
                        ?LoadingState    = LoadingState,
                        ?Persistence     = Persistence,
                        ?PersistedProps  = PersistedProps,
                        ?PersistenceType = PersistenceType
                    )

    let dropdown (id:string) (props:seq<DropdownProps>) (children:seq<DashComponent>) =
        let d = Dropdown.init(id,children)
        let componentProps = 
            match (d.TryGetTypedValue<DashComponentProps>("props")) with
            | Some p -> p
            | None -> DashComponentProps()
        props
        |> Seq.iter (fun prop ->
            let fieldName,boxedProp = prop |> DropdownProps.toDynamicMemberDef
            boxedProp |> DynObj.setValue componentProps fieldName
        )
        componentProps |> DynObj.setValue d "props" 
        d :> DashComponent