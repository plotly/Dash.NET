namespace Dash.NET.CSharp.DCC

open System
open System.Runtime.InteropServices


type IUnwrap =
    abstract Unwrap: unit -> obj

//module ComponentPropTypes =
    
type InputType = private WrappedInputType of Dash.NET.ComponentPropTypes.InputType with
    static member internal Wrap (v : Dash.NET.ComponentPropTypes.InputType) = WrappedInputType v
    static member internal Unwrap (v : InputType) = match v with WrappedInputType value -> value

    static member Text () = Dash.NET.ComponentPropTypes.InputType.Text |> InputType.Wrap
    static member Number () = Dash.NET.ComponentPropTypes.InputType.Number |> InputType.Wrap
    static member Password () = Dash.NET.ComponentPropTypes.InputType.Password |> InputType.Wrap
    static member Email () = Dash.NET.ComponentPropTypes.InputType.Email |> InputType.Wrap
    static member Range () = Dash.NET.ComponentPropTypes.InputType.Range |> InputType.Wrap
    static member Search () = Dash.NET.ComponentPropTypes.InputType.Search |> InputType.Wrap
    static member Tel () = Dash.NET.ComponentPropTypes.InputType.Tel |> InputType.Wrap
    static member Url () = Dash.NET.ComponentPropTypes.InputType.Url |> InputType.Wrap
    static member Hidden () = Dash.NET.ComponentPropTypes.InputType.Hidden |> InputType.Wrap

type InputMode = private WrappedInputMode of Dash.NET.ComponentPropTypes.InputMode with
    static member internal Wrap (v : Dash.NET.ComponentPropTypes.InputMode) = WrappedInputMode v
    static member internal Unwrap (v : InputMode) = match v with WrappedInputMode value -> value
        
    static member Verbatim () = Dash.NET.ComponentPropTypes.InputMode.Verbatim |> InputMode.Wrap
    static member Latin () = Dash.NET.ComponentPropTypes.InputMode.Latin |> InputMode.Wrap
    static member LatinName () = Dash.NET.ComponentPropTypes.InputMode.LatinName |> InputMode.Wrap
    static member LatinProse () = Dash.NET.ComponentPropTypes.InputMode.LatinProse |> InputMode.Wrap
    static member FullWidthLatin () = Dash.NET.ComponentPropTypes.InputMode.FullWidthLatin |> InputMode.Wrap
    static member Kana () = Dash.NET.ComponentPropTypes.InputMode.Kana |> InputMode.Wrap
    static member Katakana () = Dash.NET.ComponentPropTypes.InputMode.Katakana |> InputMode.Wrap
    static member Numeric () = Dash.NET.ComponentPropTypes.InputMode.Numeric |> InputMode.Wrap
    static member Tel () = Dash.NET.ComponentPropTypes.InputMode.Tel |> InputMode.Wrap
    static member Email () = Dash.NET.ComponentPropTypes.InputMode.Email |> InputMode.Wrap
    static member Url () = Dash.NET.ComponentPropTypes.InputMode.Url |> InputMode.Wrap

// SpellCheckOptions can be exposed as a bool

//type LoadingState = private WrappedLoadingState of Dash.NET.ComponentPropTypes.LoadingState with
//    static member internal Unwrap (v : LoadingState) : Dash.NET.ComponentPropTypes.LoadingState = match v with WrappedLoadingState v -> v
//    static member Init (isLoading : bool, [<Optional>] propName : string, [<Optional>] componentName : string) =
//        guardAgainstNull "isLoading" isLoading
//        Dash.NET.ComponentPropTypes.LoadingState.init (isLoading, ?propName = Option.ofObj propName, ?componentName = Option.ofObj componentName) |> WrappedLoadingState

[<CLIMutable>] 
type LoadingState =
    {
        isLoading: bool
        propName: string
        componentName: string
    }
    static member Unwrap  (v : LoadingState) : Dash.NET.ComponentPropTypes.LoadingState = Dash.NET.ComponentPropTypes.LoadingState.init (v.isLoading, ?propName = Option.ofObj v.propName, ?componentName = Option.ofObj v.componentName)

module LoadingState =
    let Init (isLoading: bool, [<Optional>] propName: string, [<Optional>] componentName: string) : LoadingState =
        guardAgainstNull "isLoading" isLoading
        { isLoading = isLoading; propName = propName; componentName = componentName }


type PersistenceTypeOptions = private WrappedPersistenceTypeOptions of Dash.NET.ComponentPropTypes.PersistenceTypeOptions with
    static member internal Wrap (v : Dash.NET.ComponentPropTypes.PersistenceTypeOptions) = WrappedPersistenceTypeOptions v
    static member internal Unwrap (v : PersistenceTypeOptions) = match v with WrappedPersistenceTypeOptions value -> value
        
    static member Local () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Local |> PersistenceTypeOptions.Wrap
    static member Session () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Session |> PersistenceTypeOptions.Wrap
    static member Memory () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Memory |> PersistenceTypeOptions.Wrap
        
//type DropdownOption = private WrappedDropdownOption of Dash.NET.ComponentPropTypes.DropdownOption with
//    static member internal Unwrap (v : DropdownOption) = match v with WrappedDropdownOption v -> v
//    static member Init (label : IConvertible, value : IConvertible, [<Optional>] disabled : Nullable<bool>, [<Optional>] title : string) =
//        guardAgainstNull "label" label
//        guardAgainstNull "value" value
//        Dash.NET.ComponentPropTypes.DropdownOption.init (label, value, ?disabled = Option.ofNullable disabled, ?title = Option.ofObj title) |> WrappedDropdownOption

[<CLIMutable>] 
type DropdownOption<'a, 'b when 'a :> IConvertible and 'b :> IConvertible> =
    {
        label: 'a
        value: 'b
        disabled: Nullable<bool>
        title: string
    }
    static member Unwrap  (v : DropdownOption<'a, 'b>) : Dash.NET.ComponentPropTypes.DropdownOption = Dash.NET.ComponentPropTypes.DropdownOption.init (v.label, v.value, ?disabled = Option.ofNullable v.disabled, ?title = Option.ofObj v.title)

module DropdownOption =
    let Init<'a, 'b when 'a :> IConvertible and 'b :> IConvertible> (label:'a, value:'b, [<Optional>] disabled: Nullable<bool>, [<Optional>] title: string) : DropdownOption<'a, 'b> =
        guardAgainstNull "label" label
        guardAgainstNull "value" value
        { label = label; value = value; disabled = disabled; title = title }

//type RadioItemsOption = private WrappedRadioItemsOption of Dash.NET.ComponentPropTypes.RadioItemsOption with
//    static member Unwrap (v : RadioItemsOption) : Dash.NET.ComponentPropTypes.RadioItemsOption = match v with WrappedRadioItemsOption v -> v
//    static member Init (label : IConvertible, value : IConvertible, [<Optional>] disabled : Nullable<bool>) =
//        guardAgainstNull "label" label
//        guardAgainstNull "value" value
//        Dash.NET.ComponentPropTypes.RadioItemsOption.init (label, value, ?disabled = Option.ofNullable disabled) |> WrappedRadioItemsOption
//    interface IUnwrap with
//        member x.Unwrap () = RadioItemsOption.Unwrap x |> box

open DynamicObj

[<CLIMutable>] 
type RadioItemsOption<'a, 'b when 'a :> IConvertible and 'b :> IConvertible> =
    {
        label : 'a
        value : 'b
        disabled : Nullable<bool>
    }
    with
    //static member Init(label:'a, value:'b, [<Optional>]disabled:Nullable<bool>) = { label = label; value = value; disabled = disabled}
    static member Unwrap  (v : RadioItemsOption<'a, 'b>) : Dash.NET.ComponentPropTypes.RadioItemsOption = Dash.NET.ComponentPropTypes.RadioItemsOption.init (v.label, v.value, ?disabled = Option.ofNullable v.disabled)

module RadioItemsOption =
    let Init<'a, 'b when 'a :> IConvertible and 'b :> IConvertible> (label: 'a, value: 'b, [<Optional>] disabled: Nullable<bool>) : RadioItemsOption<'a, 'b> =
        guardAgainstNull "label" label
        guardAgainstNull "value" value
        { label = label; value = value; disabled = disabled }

        
//type TabColors = private WrappedTabColors of Dash.NET.ComponentPropTypes.TabColors with
//    static member internal Unwrap (v : TabColors) : Dash.NET.ComponentPropTypes.TabColors = match v with WrappedTabColors v -> v
//    static member Init ([<Optional>] border : string, [<Optional>] primary : string, [<Optional>] background : string) =
//        Dash.NET.ComponentPropTypes.TabColors.init (?border = Option.ofObj border, ?primary = Option.ofObj primary, ?background = Option.ofObj background) |> WrappedTabColors

[<CLIMutable>] 
type TabColors =
    {
        border: string
        primary: string
        background: string
    }
    static member Unwrap  (v : TabColors) : Dash.NET.ComponentPropTypes.TabColors = Dash.NET.ComponentPropTypes.TabColors.init (?border = Option.ofObj v.border, ?primary = Option.ofObj v.primary, ?background = Option.ofObj v.background)

module TabColors =
    let Init ([<Optional>] border: string, [<Optional>] primary: string, [<Optional>] background: string) : TabColors =
        { border = border; primary = primary; background = background }


//type ChecklistOption = private WrappedChecklistOption of Dash.NET.ComponentPropTypes.ChecklistOption with
//    static member internal Unwrap (v : ChecklistOption) : Dash.NET.ComponentPropTypes.ChecklistOption = match v with WrappedChecklistOption v -> v
//    static member Init (label : IConvertible, value : IConvertible, [<Optional>] disabled : Nullable<bool>) =
//        guardAgainstNull "label" label
//        guardAgainstNull "value" value
//        Dash.NET.ComponentPropTypes.ChecklistOption.init (label, value, ?disabled = Option.ofNullable disabled) |> WrappedChecklistOption

[<CLIMutable>] 
type ChecklistOption<'a, 'b when 'a :> IConvertible and 'b :> IConvertible> =
    {
        label: 'a
        value: 'b
        disabled: Nullable<bool>
        title: string
    }
    static member Unwrap  (v : ChecklistOption<'a, 'b>) : Dash.NET.ComponentPropTypes.ChecklistOption = Dash.NET.ComponentPropTypes.ChecklistOption.init (v.label, v.value, ?disabled = Option.ofNullable v.disabled)

module ChecklistOption =
    let Init<'a, 'b when 'a :> IConvertible and 'b :> IConvertible> (label:'a, value:'b, [<Optional>] disabled: Nullable<bool>, [<Optional>] title: string) : ChecklistOption<'a, 'b> =
        guardAgainstNull "label" label
        guardAgainstNull "value" value
        { label = label; value = value; disabled = disabled; title = title }