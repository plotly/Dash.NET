namespace Dash.NET.DashTable

open System
open Dash.NET

open Common

type Align =
    | Default
    | Left
    | Right
    | Center
    | RightSign
    static member convert = function
        | Default   -> ""
        | Left      -> "<"
        | Right     -> ">"
        | Center    -> "^"
        | RightSign -> "="
    static member parse = function
        | "<" -> Left
        | ">" -> Right
        | "^" -> Center
        | "=" -> RightSign
        | _ -> Default

type Group =
    | No
    | Yes
    static member convert = function
        | No    -> ""
        | Yes   -> ","
    static member parse = function
        | false -> No
        | true -> Yes

type Padding =
    | No
    | Yes
    static member convert = function
        | No    -> ""
        | Yes   -> "0"
    static member parse = function
        | false -> No
        | true -> Yes

type Prefix =
    | Yocto
    | Zepto
    | Atto
    | Femto
    | Pico
    | Nano
    | Micro
    | Milli
    | None
    | Kilo
    | Mega
    | Giga
    | Tera
    | Peta
    | Exa
    | Zetta
    | Yotta
    | Explicit of float
    static member convert = function
        | Yocto -> 10. ** -24. |> box
        | Zepto -> 10. ** -21. |> box
        | Atto  -> 10. ** -18. |> box
        | Femto -> 10. ** -15. |> box
        | Pico  -> 10. ** -12. |> box
        | Nano  -> 10. ** -9. |> box
        | Micro -> 10. ** -6. |> box
        | Milli -> 10. ** -3. |> box
        | None  -> null
        | Kilo  -> 10. ** 3. |> box
        | Mega  -> 10. ** 6. |> box
        | Giga  -> 10. ** 9. |> box
        | Tera  -> 10. ** 12. |> box
        | Peta  -> 10. ** 15. |> box
        | Exa   -> 10. ** 18. |> box
        | Zetta -> 10. ** 21. |> box
        | Yotta -> 10. ** 24. |> box
        | Explicit f -> box f

type Scheme =
    | Default
    | Decimal
    | DecimalInteger
    | DecimalOrExponent
    | DecimalSiPrefix
    | Exponent
    | Fixed
    | Percentage
    | PercentageRounded
    | Binary
    | Octal
    | LowerCaseHex
    | UpperCaseHex
    | Unicode
    static member convert = function
        | Default           -> ""
        | Decimal           -> "r"
        | DecimalInteger    -> "d"
        | DecimalOrExponent -> "g"
        | DecimalSiPrefix   -> "s"
        | Exponent          -> "e"
        | Fixed             -> "f"
        | Percentage        -> "%"
        | PercentageRounded -> "p"
        | Binary            -> "b"
        | Octal             -> "o"
        | LowerCaseHex      -> "x"
        | UpperCaseHex      -> "X"
        | Unicode           -> "c"
    static member parse = function
        | "r" -> Decimal
        | "d" -> DecimalInteger
        | "g" -> DecimalOrExponent
        | "s" -> DecimalSiPrefix
        | "e" -> Exponent
        | "f" -> Fixed
        | "%" -> Percentage
        | "p" -> PercentageRounded
        | "b" -> Binary
        | "o" -> Octal
        | "x" -> LowerCaseHex
        | "X" -> UpperCaseHex
        | "c" -> Unicode
        | _   -> Default

type Sign =
    | Default
    | Negative
    | Positive
    | Parantheses
    | Space
    static member convert = function
        | Default       -> ""
        | Negative      -> "-"
        | Positive      -> "+"
        | Parantheses   -> "("
        | Space         -> " "
    static member parse = function
        | "-" -> Negative
        | "+" -> Positive
        | "(" -> Parantheses
        | " " -> Space
        | _   -> Default

type Symbol =
    | No
    | Yes
    | Binary
    | Octal
    | Hex
    static member convert = function
        | No        -> ""
        | Yes       -> "$"
        | Binary    -> "#b"
        | Octal     -> "#o"
        | Hex       -> "#x"
    static member parse = function
        | "$"   -> Yes
        | "#b"  -> Binary
        | "#o"  -> Octal
        | "#x"  -> Hex
        | _     -> No

type Trim =
    | No
    | Yes
    static member convert = function
        | No    -> ""
        | Yes   -> "~"
    static member parse = function
        | false -> No
        | true -> Yes

type Specifier
        (
            ?align: Align,
            ?fill: char,
            ?group: Group,
            ?padding: Padding,
            ?paddingWidth: uint,
            ?precision: uint,
            ?sign: Sign,
            ?symbol: Symbol,
            ?trim: Trim,
            ?scheme: Scheme
        ) =

    member val Align = align with get, set
    member val Fill = fill with get, set
    member val Group = group with get, set
    member val Padding = padding with get, set
    member val PaddingWidth = paddingWidth with get, set
    member val Precision = precision with get, set
    member val Sign = sign with get, set
    member val Symbol = symbol with get, set
    member val Trim = trim with get, set
    member val Scheme = scheme with get, set

    member this.align p =
        this.Align <- Some p
        this

    member this.align p =
        p |> Align.parse |> this.align

    member this.fill (p: char) =
        this.Fill <- Some p
        this

    member this.group p =
        this.Group <- Some p
        this

    member this.group p =
        p |> Group.parse |> this.group

    member this.padding p =
        this.Padding <- Some p
        this

    member this.padding p =
        p |> Padding.parse |> this.padding

    member this.paddingWidth (p: uint) =
        this.PaddingWidth <- Some p
        this

    member this.precision (p: uint) =
        this.Precision <- Some p
        this

    member this.scheme p =
        this.Scheme <- Some p
        this

    member this.scheme p =
        p |> Scheme.parse |> this.scheme

    member this.sign p =
        this.Sign <- Some p
        this

    member this.sign p =
        p |> Sign.parse |> this.sign

    member this.symbol p =
        this.Symbol <- Some p
        this

    member this.symbol p =
        p |> Symbol.parse |> this.symbol

    member this.trim p =
        this.Trim <- Some p
        this

    member this.trim p =
        p |> Trim.parse |> this.trim

    member this.toString () =
        let convString conv = Option.map conv >> Option.defaultValue ""
        let isAligned = this.Align <> Some (Align.Default)
        Text.StringBuilder(if isAligned then (this.Fill |> convString string) else "")
                .Append(this.Align |> convString Align.convert)
                .Append(this.Sign |> convString Sign.convert)
                .Append(this.Symbol |> convString Symbol.convert)
                .Append(this.Padding |> convString Padding.convert)
                .Append(this.PaddingWidth |> convString string)
                .Append(this.Group |> convString Group.convert)
                .Append(this.Precision |> convString (sprintf ".%i"))
                .Append(this.Trim |> convString Trim.convert)
                .Append(this.Scheme |> convString Scheme.convert)
                .ToString()

module Locale =
    let private getSymbol (t: DataTable.Locale) =
        t.TryGetTypedValue<string list> DataTable.Locale.Prop.symbol
        |> Option.defaultValue [ ""; "" ]

    let private symbol (t: DataTable.Locale) (p: string list) =
        p |> setValue t DataTable.Locale.Prop.symbol

    let symbolPrefix (t: DataTable.Locale) (p: string) =
        t |> getSymbol |> replaceAt 0 p |> symbol t

    let symbolSuffix (t: DataTable.Locale) (p: string) =
        t |> getSymbol |> replaceAt 1 p |> symbol t

    let decimalDelimiter (t: DataTable.Locale) (p: char) =
        p |> setValue t DataTable.Locale.Prop.decimal

    let groupDelimiter (t: DataTable.Locale) (p: char) =
        p |> setValue t DataTable.Locale.Prop.group

    let groups (t: DataTable.Locale) (p: uint list) =
        match p with
        | [] -> failwith "expected groups to be an integer or a list of one or more integers"
        | groups -> groups |> List.map box |> setValue t DataTable.Locale.Prop.grouping

type Format () =
    inherit DataTable.Format()

    let mutable _specifier = Specifier ()

    member private this.apply () =
        _specifier.toString()
        |> setValue this DataTable.Format.Prop.specifier

    member this.align (p: Align) =
        _specifier <- p |> _specifier.align
        this.apply()

    member this.align (p: string) =
        _specifier <- p |> _specifier.align
        this.apply()

    member this.fill p =
        _specifier <- p |> _specifier.fill
        this.apply()

    member this.group (p: Group) =
        _specifier <- p |> _specifier.group
        this.apply()

    member this.group (p: bool) =
        _specifier <- p |> _specifier.group
        this.apply()

    member this.padding (p: Padding) =
        _specifier <- p |> _specifier.padding
        this.apply()

    member this.padding (p: bool) =
        _specifier <- p |> _specifier.padding
        this.apply()

    member this.paddingWidth (p: uint) =
        _specifier <- p |> _specifier.paddingWidth
        this.apply()

    member this.precision (p: uint) =
        _specifier <- p |> _specifier.precision
        this.apply()

    member this.scheme (p: Scheme) =
        _specifier <- p |> _specifier.scheme
        this.apply()

    member this.scheme (p: string) =
        _specifier <- p |> _specifier.scheme
        this.apply()

    member this.sign (p: Sign) =
        _specifier <- p |> _specifier.sign
        this.apply()

    member this.sign (p: string) =
        _specifier <- p |> _specifier.sign
        this.apply()

    member this.symbol (p: Symbol) =
        _specifier <- p |> _specifier.symbol
        this.apply()

    member this.symbol (p: string) =
        _specifier <- p |> _specifier.symbol
        this.apply()

    member this.trim (p: Trim) =
        _specifier <- p |> _specifier.trim
        this.apply()

    member this.trim (p: bool) =
        _specifier <- p |> _specifier.trim
        this.apply()

    // Locale
    member private this.locale (p: DataTable.Locale) =
        p |> setValue this DataTable.Format.Prop.locale

    member private this.getLocale () =
        DataTable.Format.Prop.locale
        |> this.TryGetTypedValue<DataTable.Locale>
        |> Option.defaultWith (fun () -> DataTable.Locale.init ())

    member this.symbolPrefix p =
        p |> Locale.symbolPrefix (this.getLocale()) |> this.locale

    member this.symbolSuffix p =
        p |> Locale.symbolSuffix (this.getLocale()) |> this.locale

    member this.decimalDelimiter p =
        p |> Locale.decimalDelimiter (this.getLocale()) |> this.locale

    member this.groupDelimiter p =
        p |> Locale.groupDelimiter(this.getLocale()) |> this.locale

    member this.groups (p : uint list) =
        p |> Locale.groups(this.getLocale()) |> this.locale

    member this.groups (p : uint) =
        [ p ] |> Locale.groups(this.getLocale()) |> this.locale

    //# Nully
    member this.nully (p: obj) =
        p |> setValue this DataTable.Format.Prop.nully

    //# Prefix
    member this.siPrefix (p: Prefix) =
        p |> Prefix.convert |> setValue this DataTable.Format.Prop.prefix

    static member helper
        (
            ?align,
            ?fill,
            ?group,
            ?padding,
            ?paddingWidth,
            ?precision,
            ?scheme,
            ?sign,
            ?symbol,
            ?trim,
            ?symbolPrefix,
            ?symbolSuffix,
            ?decimalDelimiter,
            ?groupDelimiter,
            ?groups,
            ?nully,
            ?siPrefix
        ) =
            let t =
                Format()
                |> withOptValue (fun t (v: Align) -> t.align v) align
                |> withOptValue (fun t -> t.fill) fill
                |> withOptValue (fun t (v: Group) -> t.group v) group
                |> withOptValue (fun t (v: Padding) -> t.padding v) padding
                |> withOptValue (fun t -> t.paddingWidth) paddingWidth
                |> withOptValue (fun t -> t.precision) precision
                |> withOptValue (fun t (v: Scheme) -> t.scheme v) scheme
                |> withOptValue (fun t (v: Sign) -> t.sign v) sign
                |> withOptValue (fun t (v: Symbol) -> t.symbol v) symbol
                |> withOptValue (fun t (v: Trim) -> t.trim v) trim
                |> withOptValue (fun t -> t.symbolPrefix) symbolPrefix
                |> withOptValue (fun t -> t.symbolSuffix) symbolSuffix
                |> withOptValue (fun t -> t.decimalDelimiter) decimalDelimiter
                |> withOptValue (fun t -> t.groupDelimiter) groupDelimiter
                |> withOptValue (fun t (v: uint list) -> t.groups v) groups
                |> withOptValue (fun t -> t.nully) nully
                |> withOptValue (fun t -> t.siPrefix) siPrefix
                |> fun t -> t.apply()
            t

