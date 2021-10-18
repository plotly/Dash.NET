namespace Dash.NET.DashTable

type FormatTemplate() =
    static member money(decimals, ?sign) =
        Format.helper (
            group = Group.Yes,
            precision = decimals,
            scheme = Fixed,
            symbol = Symbol.Yes,
            sign = (sign |> Option.defaultValue Default)
        )
        
    static member percentage(decimals, ?rounded) =
        Format.helper (
            scheme = (match rounded |> Option.defaultValue false with true -> PercentageRounded | false -> Percentage),
            precision = decimals
        )

