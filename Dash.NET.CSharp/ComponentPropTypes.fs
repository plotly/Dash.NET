namespace Dash.NET.CSharp.DCC

open System
open System.Runtime.InteropServices


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

type LoadingState = private WrappedLoadingState of Dash.NET.ComponentPropTypes.LoadingState with
    static member internal Convert (v : LoadingState) : Dash.NET.ComponentPropTypes.LoadingState = match v with WrappedLoadingState v -> v
    static member public Init (isLoading : bool, [<Optional>] propName : string, [<Optional>] componentName : string) =
        guardAgainstNull "isLoading" isLoading
        Dash.NET.ComponentPropTypes.LoadingState.init (isLoading, ?propName = Option.ofObj propName, ?componentName = Option.ofObj componentName) |> WrappedLoadingState


type PersistenceTypeOptions = private WrappedPersistenceTypeOptions of Dash.NET.ComponentPropTypes.PersistenceTypeOptions with
    static member internal Wrap (v : Dash.NET.ComponentPropTypes.PersistenceTypeOptions) = WrappedPersistenceTypeOptions v
    static member internal Unwrap (v : PersistenceTypeOptions) = match v with WrappedPersistenceTypeOptions value -> value
        
    static member Local () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Local |> PersistenceTypeOptions.Wrap
    static member Session () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Session |> PersistenceTypeOptions.Wrap
    static member Memory () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Memory |> PersistenceTypeOptions.Wrap
        
type DropdownOption = private WrappedDropdownOption of Dash.NET.ComponentPropTypes.DropdownOption with
    static member internal Convert (v : DropdownOption) = match v with WrappedDropdownOption v -> v
    static member public Init (label : IConvertible, value : IConvertible, [<Optional>] disabled : Nullable<bool>, [<Optional>] title : string) =
        guardAgainstNull "label" label
        guardAgainstNull "value" value
        Dash.NET.ComponentPropTypes.DropdownOption.init (label, value, ?disabled = Option.ofNullable disabled, ?title = Option.ofObj title) |> WrappedDropdownOption

type RadioItemsOption = private WrappedRadioItemsOption of Dash.NET.ComponentPropTypes.RadioItemsOption with
    static member internal Convert (v : RadioItemsOption) : Dash.NET.ComponentPropTypes.RadioItemsOption = match v with WrappedRadioItemsOption v -> v
    static member public Init (label : IConvertible, value : IConvertible, [<Optional>] disabled : Nullable<bool>) =
        guardAgainstNull "label" label
        guardAgainstNull "value" value
        Dash.NET.ComponentPropTypes.RadioItemsOption.init (label, value, ?disabled = Option.ofNullable disabled) |> WrappedRadioItemsOption
        
type TabColors = private WrappedTabColors of Dash.NET.ComponentPropTypes.TabColors with
    static member internal Convert (v : TabColors) : Dash.NET.ComponentPropTypes.TabColors = match v with WrappedTabColors v -> v
    static member internal Init ([<Optional>] border : string, [<Optional>] primary : string, [<Optional>] background : string) : Dash.NET.ComponentPropTypes.TabColors =
        Dash.NET.ComponentPropTypes.TabColors.init (?border = Option.ofObj border, ?primary = Option.ofObj primary, ?background = Option.ofObj background)

type ChecklistOption = private WrappedChecklistOption of Dash.NET.ComponentPropTypes.ChecklistOption with
    static member internal Convert (v : ChecklistOption) : Dash.NET.ComponentPropTypes.ChecklistOption = match v with WrappedChecklistOption v -> v
    static member internal Init (label : IConvertible, value : IConvertible, [<Optional>] disabled : Nullable<bool>) =
        guardAgainstNull "label" label
        guardAgainstNull "value" value
        Dash.NET.ComponentPropTypes.ChecklistOption.init (label, value, ?disabled = Option.ofNullable disabled)