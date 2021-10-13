namespace Dash.NET.CSharp.DCC

open System

module ComponentPropTypes =
    
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

    type LoadingState = 
        {
            IsLoading : bool
            PropName : string
            ComponentName : string
        }
        with static member internal Convert (v : LoadingState) : Dash.NET.ComponentPropTypes.LoadingState = Dash.NET.ComponentPropTypes.LoadingState.init (v.IsLoading, v.PropName, v.ComponentName)

    type PersistenceTypeOptions = private WrappedPersistenceTypeOptions of Dash.NET.ComponentPropTypes.PersistenceTypeOptions with
        static member internal Wrap (v : Dash.NET.ComponentPropTypes.PersistenceTypeOptions) = WrappedPersistenceTypeOptions v
        static member internal Unwrap (v : PersistenceTypeOptions) = match v with WrappedPersistenceTypeOptions value -> value
        
        static member Local () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Local |> PersistenceTypeOptions.Wrap
        static member Session () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Session |> PersistenceTypeOptions.Wrap
        static member Memory () = Dash.NET.ComponentPropTypes.PersistenceTypeOptions.Memory |> PersistenceTypeOptions.Wrap
        
    type DropdownOption = 
        {
            Label:IConvertible
            Value:IConvertible
            Disabled:bool
            Title:string
        }
        static member internal Convert (v : DropdownOption) : Dash.NET.ComponentPropTypes.DropdownOption = Dash.NET.ComponentPropTypes.DropdownOption.init (v.Label, v.Value, v.Disabled, v.Title)

    type RadioItemsOption = 
        {
            Label:IConvertible
            Value:IConvertible
            Disabled:bool
        }
        static member internal Convert (v : RadioItemsOption) : Dash.NET.ComponentPropTypes.RadioItemsOption = Dash.NET.ComponentPropTypes.RadioItemsOption.init (v.Label, v.Value, v.Disabled)
        
    type TabColors = 
        {
            Border : string
            Primary : string
            Background : string
        }
        static member internal Convert (v : TabColors) : Dash.NET.ComponentPropTypes.TabColors = Dash.NET.ComponentPropTypes.TabColors.init (v.Border, v.Primary, v.Background)

    type ChecklistOption =
        {
            Label:IConvertible
            Value:IConvertible
            Disabled: bool
        }
        static member internal Convert (v : ChecklistOption) : Dash.NET.ComponentPropTypes.ChecklistOption = Dash.NET.ComponentPropTypes.ChecklistOption.init (v.Label, v.Value, v.Disabled)