module Dash.NET.DashTable

open System
open DynamicObj
open Dash.NET
open Dash.NET.Common
open Newtonsoft.Json

[<Literal>]
let CdnLink = "https://unpkg.com/dash-table@4.12.1/dash_table/bundle.js"

///<summary>
///Dash DataTable is an interactive table component designed for
///designed for viewing, editing, and exploring large datasets.
///DataTable is rendered with standard, semantic HTML &lt;table/&gt; markup,
///which makes it accessible, responsive, and easy to style. This
///component was written from scratch in React.js specifically for the
///Dash community. Its API was designed to be ergonomic and its behavior
///is completely customizable through its properties.
///</summary>
[<RequireQualifiedAccess>]
module DataTable =
    type DUConverter () =
        inherit JsonConverter ()
        override _.WriteJson(writer, value, serializer) =
            if isNull value then writer.WriteNull()
            else serializer.Serialize(writer, NameCase.fromDiscriminatedUnion value);

        override _.ReadJson(reader, objectType, existingValue, serializer) =
            let duConverter = new Converters.DiscriminatedUnionConverter()
            duConverter.ReadJson(reader, objectType, existingValue, serializer)

        override _.CanConvert(objectType) =
            let duConverter = new Converters.DiscriminatedUnionConverter()
            duConverter.CanConvert(objectType)

    ///<summary>
    ///value equal to: 'first', 'last'
    ///</summary>
    type ColumnTogglePosition =
        | First
        | Last

    type TogglableColumn =
        | Position of ColumnTogglePosition
        | Bool of bool
        | MergedMultiHeader of bool list

    ///<summary>
    ///value equal to: 'odd', 'even'
    ///</summary>
    type Alternate =
        | Odd
        | Even

    ///<summary>
    ///number | list with values of type: number | value equal to: 'odd', 'even'
    ///</summary>
    type SelectRowBy =
        | Index of int
        | Indices of int list
        | Alternate of Alternate

    ///<summary>
    ///value equal to: 'any', 'numeric', 'text', 'datetime'
    ///&#10;
    ///The data-type provides support for per column typing and allows for data
    ///validation and coercion.
    ///'numeric': represents both floats and ints.
    ///'text': represents a string.
    ///'datetime': a string representing a date or date-time, in the form:
    ///  'YYYY-MM-DD HH:MM:SS.ssssss' or some truncation thereof. Years must
    ///  have 4 digits, unless you use &#96;validation.allow_YY: true&#96;. Also
    ///  accepts 'T' or 't' between date and time, and allows timezone info
    ///  at the end. To convert these strings to Python &#96;datetime&#96; objects,
    ///  use &#96;dateutil.parser.isoparse&#96;. In R use &#96;parse_iso_8601&#96; from the
    ///  &#96;parsedate&#96; library.
    ///  WARNING: these parsers do not work with 2-digit years, if you use
    ///  &#96;validation.allow_YY: true&#96; and do not coerce to 4-digit years.
    ///  And parsers that do work with 2-digit years may make a different
    ///  guess about the century than we make on the front end.
    ///'any': represents any type of data.
    ///Defaults to 'any' if undefined.
    ///</summary>
    type ColumnType =
        | Any
        | Numeric
        | Text
        | Datetime

    ///<summary>
    ///string | list with values of type: string
    ///</summary>
    type StyleColumn =
        | Id of string
        | Ids of string list

    ///<summary>
    ///record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'header_index: number | list with values of type: number | value equal to: 'odd', 'even' (optional)', 'column_editable: boolean (optional)' (optional)'
    ///</summary>
    type ConditionalHeaderStyle =
        {
            [<JsonProperty("column_id"); JsonConverter(typeof<DUConverter>)>] ColumnId: StyleColumn option
            [<JsonProperty("column_type"); JsonConverter(typeof<DUConverter>)>] ColumnType: ColumnType option
            [<JsonProperty("header_index"); JsonConverter(typeof<DUConverter>)>] HeaderIndex: SelectRowBy option
            [<JsonProperty("column_editable"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] ColumnEditable: bool option
        }

    ///<summary>
    ///record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'column_editable: boolean (optional)' (optional)'
    ///</summary>
    type ConditionalFilterStyle =
        {
            [<JsonProperty("column_id"); JsonConverter(typeof<DUConverter>)>] ColumnId: StyleColumn option
            [<JsonProperty("column_type"); JsonConverter(typeof<DUConverter>)>] ColumnType: ColumnType option
            [<JsonProperty("column_editable"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] ColumnEditable: bool option
        }

    ///<summary>
    ///value equal to: 'active', 'selected'
    ///</summary>
    type DataStyleState =
        | Active
        | Selected

    ///<summary>
    ///record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'filter_query: string (optional)', 'state: value equal to: 'active', 'selected' (optional)', 'row_index: number | value equal to: 'odd', 'even' | list with values of type: number (optional)', 'column_editable: boolean (optional)' (optional)'
    ///</summary>
    type ConditionalDataStyle =
        {
            [<JsonProperty("column_id"); JsonConverter(typeof<DUConverter>)>] ColumnId: StyleColumn option
            [<JsonProperty("column_type"); JsonConverter(typeof<DUConverter>)>] ColumnType: ColumnType option
            [<JsonProperty("filter_query"); JsonConverter(typeof<Json.OptionConverter<string>>)>] FilterQuery: string option
            [<JsonProperty("state"); JsonConverter(typeof<DUConverter>)>] State: DataStyleState option
            [<JsonProperty("row_index"); JsonConverter(typeof<DUConverter>)>] RowIndex: SelectRowBy option
            [<JsonProperty("column_editable"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] ColumnEditable: bool option
        }

    ///<summary>
    ///record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)' (optional)'
    ///</summary>
    type ConditionalCellStyle =
        {
            [<JsonProperty("column_id"); JsonConverter(typeof<DUConverter>)>] ColumnId: StyleColumn option
            [<JsonProperty("column_type"); JsonConverter(typeof<DUConverter>)>] ColumnType: ColumnType option
        }

    ///<summary>
    ///value equal to: 'asc', 'desc'
    ///</summary>
    type SortDirection =
        | Asc
        | Desc
        | AscDesc

    ///<summary>
    ///record with the fields: 'column_id: string (required)', 'direction: value equal to: 'asc', 'desc' (required)'
    ///</summary>
    type SortBy =
        {
            [<JsonProperty("column_id")>] ColumnId: string
            [<JsonProperty("direction"); JsonConverter(typeof<DUConverter>)>] Direction: SortDirection
        }

    ///<summary>
    ///value equal to: 'single', 'multi'
    ///</summary>
    type SortMode =
        | Single
        | Multi

    ///<summary>
    ///value equal to: 'custom', 'native', 'none'
    ///</summary>
    type MaybeActionType =
        | Custom
        | Native
        | None

    ///<summary>
    ///value equal to: 'sensitive', 'insensitive'
    ///</summary>
    type FilterOptionCase =
        | Sensitive
        | Insensitive

    ///<summary>
    ///record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)'
    ///</summary>
    type FilterOption =
        {
            [<JsonProperty("case"); JsonConverter(typeof<DUConverter>)>] Case: FilterOptionCase option
        }

    ///<summary>
    ///value equal to: 'and', 'or'
    ///</summary>
    type FilterActionOperator =
        | And
        | Or

    ///<summary>
    ///value equal to: 'custom', 'native'
    ///</summary>
    type FilterActionType =
        | Custom
        | Native

    ///<summary>
    ///record with the fields: 'type: value equal to: 'custom', 'native' (required)', 'operator: value equal to: 'and', 'or' (optional)'
    ///</summary>
    type ConditionalFilterAction =
        {
            [<JsonProperty("type"); JsonConverter(typeof<DUConverter>)>] Type: FilterActionType
            [<JsonProperty("operator"); JsonConverter(typeof<DUConverter>)>] Operator: FilterActionOperator option
        }

    ///<summary>
    ///value equal to: 'custom', 'native', 'none' | record with the fields: 'type: value equal to: 'custom', 'native' (required)', 'operator: value equal to: 'and', 'or' (optional)'
    ///</summary>
    type FilterAction =
        | Type of MaybeActionType
        | Conditional of ConditionalFilterAction

    ///<summary>
    ///value equal to: 'text', 'markdown'
    ///&#10;
    ///refers to the type of tooltip syntax used
    ///for the tooltip generation. Can either be &#96;markdown&#96;
    ///or &#96;text&#96;. Defaults to &#96;text&#96;.
    ///</summary>
    type TooltipValueType =
        | Text
        | Markdown

    ///<summary>
    ///record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'
    ///</summary>
    type TooltipValue =
        {
            [<JsonProperty("value")>] Value: string
            [<JsonProperty("type"); JsonConverter(typeof<DUConverter>)>] Type: TooltipValueType option
            [<JsonProperty("delay"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Delay: int option
            [<JsonProperty("duration"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Duration: int option
        }

    ///<summary>
    ///value equal to: 'null' | string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'
    ///</summary>
    type MaybeTooltip =
        | Text of string
        | Value of TooltipValue
        | Null

    ///<summary>
    ///string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)' | list with values of type: value equal to: 'null' | string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'
    ///</summary>
    type HeaderTooltip =
        | Text of string
        | Value of TooltipValue
        | Values of MaybeTooltip list

    ///<summary>
    ///string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'
    ///</summary>
    type DataTooltip =
        | Text of string
        | Value of TooltipValue

    ///<summary>
    ///record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)', 'row_index: number | value equal to: 'odd', 'even' (optional)'
    ///&#10;
    ///The &#96;if&#96; refers to the condition that needs to be fulfilled
    ///in order for the associated tooltip configuration to be
    ///used. If multiple conditions are defined, all conditions
    ///must be met for the tooltip to be used by a cell.
    ///</summary>
    type TooltipCondition =
        {
            [<JsonProperty("column_id"); JsonConverter(typeof<Json.OptionConverter<string>>)>] ColumnId: string option
            [<JsonProperty("filter_query"); JsonConverter(typeof<Json.OptionConverter<string>>)>] FilterQuery: string option
            [<JsonProperty("row_index"); JsonConverter(typeof<DUConverter>)>] RowIndex: SelectRowBy option
        }

    ///<summary>
    ///record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'if: record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)', 'row_index: number | value equal to: 'odd', 'even' (optional)' (required)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'
    ///</summary>
    type ConditionalTooltip =
        {
            [<JsonProperty("value")>] Value: string
            [<JsonProperty("if"); JsonConverter(typeof<Json.OptionConverter<TooltipCondition>>)>] If: TooltipCondition option
            [<JsonProperty("type"); JsonConverter(typeof<DUConverter>)>] Type: TooltipValueType option
            [<JsonProperty("delay"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Delay: int option
            [<JsonProperty("duration"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Duration: int option
        }

    ///<summary>
    ///value equal to: 'both', 'data', 'header'
    ///&#10;
    ///Refers to whether the tooltip will be shown
    ///only on data or headers. Can be &#96;both&#96;, &#96;data&#96;, &#96;header&#96;.
    ///Defaults to &#96;both&#96;.
    ///</summary>
    type TooltipUseWith =
        | Both
        | Data
        | Header

    ///<summary>
    ///record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'use_with: value equal to: 'both', 'data', 'header' (optional)', 'value: string (required)'
    ///</summary>
    type UseWithTooltipValue =
        {
            [<JsonProperty("value")>] Value: string
            [<JsonProperty("use_with"); JsonConverter(typeof<DUConverter>)>] UseWith: TooltipUseWith option
            [<JsonProperty("type"); JsonConverter(typeof<DUConverter>)>] Type: TooltipValueType option
            [<JsonProperty("delay"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Delay: int option
            [<JsonProperty("duration"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Duration: int option
        }

    ///<summary>
    ///string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'use_with: value equal to: 'both', 'data', 'header' (optional)', 'value: string (required)'
    ///</summary>
    type ColumnTooltip =
        | Text of string
        | UseWithValue of UseWithTooltipValue

    ///<summary>
    ///record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)'
    ///</summary>
    type DropdownCondition =
        {
            [<JsonProperty("column_id"); JsonConverter(typeof<Json.OptionConverter<string>>)>] ColumnId: string option
            [<JsonProperty("filter_query"); JsonConverter(typeof<Json.OptionConverter<string>>)>] FilterQuery: string option
        }

    ///<summary>
    ///record with the fields: 'label: string (required)', 'value: number | string | boolean (required)'
    ///</summary>
    type DropdownOption =
        {
            [<JsonProperty("label")>] Label: string
            [<JsonProperty("value")>] Value: IConvertible
        }

    ///<summary>
    ///record with the fields: 'clearable: boolean (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'
    ///</summary>
    type Dropdown =
        {
            [<JsonProperty("options")>] Options: DropdownOption list
            [<JsonProperty("clearable"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] Clearable: bool option
        }

    ///<summary>
    ///record with the fields: 'clearable: boolean (optional)', 'if: record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)' (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'
    ///</summary>
    type ConditionalDropdown =
        {
            [<JsonProperty("clearable"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] Clearable: bool option
            [<JsonProperty("if"); JsonConverter(typeof<Json.OptionConverter<DropdownCondition>>)>] If: DropdownCondition option
            [<JsonProperty("options"); JsonConverter(typeof<Json.OptionConverter<DropdownOption>>)>] Options: DropdownOption list
        }

    ///<summary>
    ///record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)'
    ///</summary>
    type Cell =
        {
            [<JsonProperty("row"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Row: int option
            [<JsonProperty("column"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Column: int option
            [<JsonProperty("row_id"); JsonConverter(typeof<Json.OptionConverter<IConvertible>>)>] RowId: IConvertible option
            [<JsonProperty("column_id"); JsonConverter(typeof<Json.OptionConverter<string>>)>] ColumnId: string option
        }

    ///<summary>
    ///value equal to: 'single', 'multi', 'false'
    ///</summary>
    type Select =
        | Single
        | Multi
        | False

    ///• fixed_columns/fixed_headers (record with the fields: 'data: value equal to: '0' (optional)', 'headers: value equal to: 'false' (optional)' | record with the fields: 'data: number (optional)', 'headers: value equal to: 'true' (required)'; default {
    type Fixed =
        {
            [<JsonProperty("data"); JsonConverter(typeof<Json.OptionConverter<uint>>)>] Data: uint option
            [<JsonProperty("headers"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] Headers: bool option
        }

    ///<summary>
    ///value equal to: 'none', 'ids', 'names', 'display'
    ///</summary>
    type MaybeExportHeaders =
        | None
        | Ids
        | Names
        | Display

    ///<summary>
    ///value equal to: 'csv', 'xlsx', 'none'
    ///</summary>
    type MaybeExportFormat =
        | Csv
        | Xlsx
        | None

    ///<summary>
    ///value equal to: 'all', 'visible'
    ///</summary>
    type ExportColumns =
        | All
        | Visible

    ///<summary>
    ///record with the fields: 'selector: string (required)', 'rule: string (required)'
    ///</summary>
    type Css =
        {
            [<JsonProperty("selector")>] Selector: string
            [<JsonProperty("rule")>] Rule: string
        }

    ///<summary>
    ///value equal to: '_blank', '_parent', '_self', '_top'
    ///</summary>
    type LinkBehaviour =
        | Blank
        | Parent
        | Self
        | Top

    ///<summary>
    ///string | value equal to: '_blank', '_parent', '_self', '_top'
    ///&#10;
    ///(default: '_blank').  The link's behavior (_blank opens the link in a
    ///new tab, _parent opens the link in the parent frame, _self opens the link in the
    ///current tab, and _top opens the link in the top frame) or a string
    ///</summary>
    type LinkTarget =
        | Text of string
        | Behaviour of LinkBehaviour

    ///<summary>
    ///record with the fields: 'link_target: string | value equal to: '_blank', '_parent', '_self', '_top' (optional)', 'html: boolean (optional)'
    ///</summary>
    type MarkdownOptions =
        {
            [<JsonProperty("link_target"); JsonConverter(typeof<DUConverter>)>] LinkTarget: LinkTarget option
            [<JsonProperty("html"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] Html: bool option
        }

    ///<summary>
    ///record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)'
    ///</summary>
    type LocaleFormat =
        {
            [<JsonProperty("symbol"); JsonConverter(typeof<Json.OptionConverter<string list>>)>] Symbol: string list option
            [<JsonProperty("decimal"); JsonConverter(typeof<Json.OptionConverter<string>>)>] Decimal: string option
            [<JsonProperty("group"); JsonConverter(typeof<Json.OptionConverter<string>>)>] Group: string option
            [<JsonProperty("grouping"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Grouping: int list option
            [<JsonProperty("numerals"); JsonConverter(typeof<Json.OptionConverter<string list>>)>] Numerals: string list option
            [<JsonProperty("percent"); JsonConverter(typeof<Json.OptionConverter<string>>)>] Percent: string option
            [<JsonProperty("separate_4digits"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] Separate4digits: bool option
        }

    ///<summary>
    ///record with the fields: 'allow_null: boolean (optional)', 'default: boolean | number | string | record | list (optional)', 'allow_YY: boolean (optional)'
    ///&#10;
    ///The &#96;validation&#96; options for user input processing that can accept, reject or apply a
    ///default value.
    ///</summary>
    type ColumnValidation =
        {
            [<JsonProperty("allow_null"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] AllowNull: bool option
            [<JsonProperty("default"); JsonConverter(typeof<Json.OptionConverter<obj>>)>] Default: obj option
            [<JsonProperty("allow_YY"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] AllowYY: bool option
        }

    ///<summary>
    ///value equal to: 'accept', 'default', 'reject'
    ///&#10;
    ///(default 'reject'):  What to do with the value if the action fails.
    /// 'accept': use the invalid value;
    /// 'default': replace the provided value with &#96;validation.default&#96;;
    /// 'reject': do not modify the existing value.
    ///</summary>
    type FailureAction =
        | Accept
        | Default
        | Reject

    ///<summary>
    ///value equal to: 'coerce', 'none', 'validate'
    ///&#10;
    ///(default 'coerce'):  'none': do not validate data;
    /// 'coerce': check if the data corresponds to the destination type and
    /// attempts to coerce it into the destination type if not;
    /// 'validate': check if the data corresponds to the destination type (no coercion).
    ///</summary>
    type MaybeOnChangeAction =
        | Coerce
        | None
        | Validate

    ///<summary>
    ///record with the fields: 'action: value equal to: 'coerce', 'none', 'validate' (optional)', 'failure: value equal to: 'accept', 'default', 'reject' (optional)'
    ///&#10;
    ///The &#96;on_change&#96; behavior of the column for user-initiated modifications.
    ///</summary>
    type ColumnOnChange =
        {
            [<JsonProperty("action"); JsonConverter(typeof<DUConverter>)>] Action: MaybeOnChangeAction option
            [<JsonProperty("failure"); JsonConverter(typeof<DUConverter>)>] Failure: FailureAction option
        }

    ///<summary>
    ///value equal to: 'input', 'dropdown', 'markdown'
    ///&#10;
    ///The &#96;presentation&#96; to use to display data. Markdown can be used on
    ///columns with type 'text'.  See 'dropdown' for more info.
    ///Defaults to 'input' for ['datetime', 'numeric', 'text', 'any'].
    ///</summary>
    type ColumnPresentation =
        | Input
        | Dropdown
        | Markdown
            
    ///<summary>
    ///record with the fields: 'locale: record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)' (optional)', 'nully: boolean | number | string | record | list (optional)', 'prefix: number (optional)', 'specifier: string (optional)'
    ///&#10;
    ///The formatting applied to the column's data.
    ///This prop is derived from the [d3-format](https://github.com/d3/d3-format) library specification. Apart from
    ///being structured slightly differently (under a single prop), the usage is the same.
    ///See also dash_table.FormatTemplate.  It contains helper functions for typical number formats.
    ///</summary>
    type ColumnFormat =
        {
            [<JsonProperty("locale"); JsonConverter(typeof<Json.OptionConverter<LocaleFormat>>)>] Locale: LocaleFormat option
            [<JsonProperty("nully"); JsonConverter(typeof<Json.OptionConverter<obj>>)>] Nully: obj option
            [<JsonProperty("prefix"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Prefix: int option
            [<JsonProperty("specifier"); JsonConverter(typeof<Json.OptionConverter<string>>)>] Specifier: string option
        }

    ///<summary>
    ///value equal to: 'first', 'last' | boolean | list with values of type: boolean
    ///&#10;
    ///If true, the user can select the column by clicking on the checkbox or radio button
    ///in the column. If there are multiple header rows, true will display the input
    ///on each row.
    ///If &#96;last&#96;, the input will only appear on the last header row. If &#96;first&#96; it will only
    ///appear on the first header row. These are respectively shortcut equivalents to
    ///&#96;[false, ..., false, true]&#96; and &#96;[true, false, ..., false]&#96;.
    ///If there are merged, multi-header columns then you can choose which column header
    ///row to display the input in by supplying an array of booleans.
    ///For example, &#96;[true, false]&#96; will display the &#96;selectable&#96; input on the first row,
    ///but now on the second row.
    ///If the &#96;selectable&#96; input appears on a merged columns, then clicking on that input
    ///will select *all* of the merged columns associated with it.
    ///The table-level prop &#96;column_selectable&#96; is used to determine the type of column
    ///selection to use.
    ///</summary>
    type ColumnSelectable = TogglableColumn

    ///<summary>
    ///value equal to: 'first', 'last' | boolean | list with values of type: boolean
    ///&#10;
    ///If true, the user can rename the column by clicking on the &#96;rename&#96;
    ///action button on the column. If there are multiple header rows, true
    ///will display the action button on each row.
    ///If &#96;last&#96;, the &#96;rename&#96; action button will only appear on the last header
    ///row. If &#96;first&#96; it will only appear on the first header row. These
    ///are respectively shortcut equivalents to &#96;[false, ..., false, true]&#96; and
    ///&#96;[true, false, ..., false]&#96;.
    ///If there are merged, multi-header columns then you can choose
    ///which column header row to display the &#96;rename&#96; action button in by
    ///supplying an array of booleans.
    ///For example, &#96;[true, false]&#96; will display the &#96;rename&#96; action button
    ///on the first row, but not the second row.
    ///If the &#96;rename&#96; action button appears on a merged column, then clicking
    ///on that button will rename *all* of the merged columns associated with it.
    ///</summary>
    type ColumnRenamable = TogglableColumn

    ///<summary>
    ///value equal to: 'first', 'last' | boolean | list with values of type: boolean
    ///&#10;
    ///If true, the user can hide the column by clicking on the &#96;hide&#96;
    ///action button on the column. If there are multiple header rows, true
    ///will display the action button on each row.
    ///If &#96;last&#96;, the &#96;hide&#96; action button will only appear on the last header
    ///row. If &#96;first&#96; it will only appear on the first header row. These
    ///are respectively shortcut equivalents to &#96;[false, ..., false, true]&#96; and
    ///&#96;[true, false, ..., false]&#96;.
    ///If there are merged, multi-header columns then you can choose
    ///which column header row to display the &#96;hide&#96; action button in by
    ///supplying an array of booleans.
    ///For example, &#96;[true, false]&#96; will display the &#96;hide&#96; action button
    ///on the first row, but not the second row.
    ///If the &#96;hide&#96; action button appears on a merged column, then clicking
    ///on that button will hide *all* of the merged columns associated with it.
    ///</summary>
    type ColumnHideable = TogglableColumn

    ///<summary>
    ///Sensitifity value equal to: 'sensitive', 'insensitive'
    ///</summary>
    type FilterSensitifity =
        | Sensitive
        | Insensitive

    ///<summary>
    ///record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)'
    ///&#10;
    ///There are two &#96;filter_options&#96; props in the table.
    ///This is the column-level filter_options prop and there is
    ///also the table-level &#96;filter_options&#96; prop.
    ///These props determine whether the applicable filter relational
    ///operators will default to &#96;sensitive&#96; or &#96;insensitive&#96; comparison.
    ///If the column-level &#96;filter_options&#96; prop is set it overrides
    ///the table-level &#96;filter_options&#96; prop for that column.
    ///</summary>
    type ColumnFilterOption =
        {
            [<JsonProperty("case"); JsonConverter(typeof<DUConverter>)>] Case: FilterSensitifity option
        }

    ///<summary>
    ///value equal to: 'first', 'last' | boolean | list with values of type: boolean
    ///&#10;
    ///If true, the user can remove the column by clicking on the &#96;delete&#96;
    ///action button on the column. If there are multiple header rows, true
    ///will display the action button on each row.
    ///If &#96;last&#96;, the &#96;delete&#96; action button will only appear on the last header
    ///row. If &#96;first&#96; it will only appear on the first header row. These
    ///are respectively shortcut equivalents to &#96;[false, ..., false, true]&#96; and
    ///&#96;[true, false, ..., false]&#96;.
    ///If there are merged, multi-header columns then you can choose
    ///which column header row to display the &#96;delete&#96; action button in by
    ///supplying an array of booleans.
    ///For example, &#96;[true, false]&#96; will display the &#96;delete&#96; action button
    ///on the first row, but not the second row.
    ///If the &#96;delete&#96; action button appears on a merged column, then clicking
    ///on that button will remove *all* of the merged columns associated with it.
    ///</summary>
    type ColumnDeletable = TogglableColumn

    ///<summary>
    ///value equal to: 'first', 'last' | boolean | list with values of type: boolean
    ///&#10;
    ///If true, the user can clear the column by clicking on the &#96;clear&#96;
    ///action button on the column. If there are multiple header rows, true
    ///will display the action button on each row.
    ///If &#96;last&#96;, the &#96;clear&#96; action button will only appear on the last header
    ///row. If &#96;first&#96; it will only appear on the first header row. These
    ///are respectively shortcut equivalents to &#96;[false, ..., false, true]&#96; and
    ///&#96;[true, false, ..., false]&#96;.
    ///If there are merged, multi-header columns then you can choose
    ///which column header row to display the &#96;clear&#96; action button in by
    ///supplying an array of booleans.
    ///For example, &#96;[true, false]&#96; will display the &#96;clear&#96; action button
    ///on the first row, but not the second row.
    ///If the &#96;clear&#96; action button appears on a merged column, then clicking
    ///on that button will clear *all* of the merged columns associated with it.
    ///Unlike &#96;column.deletable&#96;, this action does not remove the column(s)
    ///from the table. It only removed the associated entries from &#96;data&#96;.
    ///</summary>
    type ColumnClearable = TogglableColumn

    ///<summary>
    ///record with the fields: 'clearable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'deletable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'editable: boolean (optional)', 'filter_options: record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)' (optional)', 'hideable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'renamable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'selectable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'format: record with the fields: 'locale: record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)' (optional)', 'nully: boolean | number | string | record | list (optional)', 'prefix: number (optional)', 'specifier: string (optional)' (optional)', 'id: string (required)', 'name: string | list with values of type: string (required)', 'presentation: value equal to: 'input', 'dropdown', 'markdown' (optional)', 'on_change: record with the fields: 'action: value equal to: 'coerce', 'none', 'validate' (optional)', 'failure: value equal to: 'accept', 'default', 'reject' (optional)' (optional)', 'sort_as_null: list with values of type: string | number | boolean (optional)', 'validation: record with the fields: 'allow_null: boolean (optional)', 'default: boolean | number | string | record | list (optional)', 'allow_YY: boolean (optional)' (optional)', 'type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)'
    ///</summary>
    type Column =
        {
            [<JsonProperty("name")>] Name: string list
            [<JsonProperty("id")>] Id: string
            [<JsonProperty("type"); JsonConverter(typeof<DUConverter>)>] Type: ColumnType option
            [<JsonProperty("clearable"); JsonConverter(typeof<DUConverter>)>] Clearable: ColumnClearable option
            [<JsonProperty("deletable"); JsonConverter(typeof<DUConverter>)>] Deletable: ColumnDeletable option
            [<JsonProperty("editable"); JsonConverter(typeof<Json.OptionConverter<bool>>)>] Editable: bool option
            [<JsonProperty("filter_options"); JsonConverter(typeof<Json.OptionConverter<ColumnFilterOption>>)>] FilterOptions: ColumnFilterOption option
            [<JsonProperty("hideable"); JsonConverter(typeof<DUConverter>)>] Hideable: ColumnHideable option
            [<JsonProperty("renamable"); JsonConverter(typeof<DUConverter>)>] Renamable: ColumnRenamable option
            [<JsonProperty("selectable"); JsonConverter(typeof<DUConverter>)>] Selectable: ColumnSelectable option
            [<JsonProperty("format"); JsonConverter(typeof<Json.OptionConverter<ColumnFormat>>)>] Format: ColumnFormat option
            [<JsonProperty("presentation"); JsonConverter(typeof<Json.OptionConverter<ColumnPresentation>>)>] Presentation: ColumnPresentation option
            [<JsonProperty("on_change"); JsonConverter(typeof<Json.OptionConverter<ColumnOnChange>>)>] OnChange: ColumnOnChange option
            [<JsonProperty("sort_as_null"); JsonConverter(typeof<Json.OptionConverter<IConvertible list>>)>] SortAsNull: IConvertible list option
            [<JsonProperty("validation"); JsonConverter(typeof<Json.OptionConverter<ColumnValidation>>)>] Validation: ColumnValidation option
        }
        static member create name id =
            {
                Name = [ name ]
                Id = id
                Type = Option.None
                Clearable = Option.None
                Deletable = Option.None
                Editable = Option.None
                FilterOptions = Option.None
                Hideable = Option.None
                Renamable = Option.None
                Selectable = Option.None
                Format = Option.None
                Presentation = Option.None
                OnChange = Option.None
                SortAsNull = Option.None
                Validation = Option.None
            }

    ///<summary>
    ///record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)'
    ///</summary>
    type ActiveCell =
        {
            [<JsonProperty("row"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Row: int option
            [<JsonProperty("column"); JsonConverter(typeof<Json.OptionConverter<int>>)>] Column: int option
            [<JsonProperty("row_id"); JsonConverter(typeof<Json.OptionConverter<IConvertible>>)>] RowId: IConvertible option
            [<JsonProperty("column_id"); JsonConverter(typeof<Json.OptionConverter<string>>)>] ColumnId: string option
        }

    ///<summary>
    ///• active_cell (record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)') - The row and column indices and IDs of the currently active cell.
    ///&#96;row_id&#96; is only returned if the data rows have an &#96;id&#96; key.
    ///&#10;
    ///• columns (list with values of type: record with the fields: 'clearable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'deletable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'editable: boolean (optional)', 'filter_options: record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)' (optional)', 'hideable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'renamable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'selectable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'format: record with the fields: 'locale: record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)' (optional)', 'nully: boolean | number | string | record | list (optional)', 'prefix: number (optional)', 'specifier: string (optional)' (optional)', 'id: string (required)', 'name: string | list with values of type: string (required)', 'presentation: value equal to: 'input', 'dropdown', 'markdown' (optional)', 'on_change: record with the fields: 'action: value equal to: 'coerce', 'none', 'validate' (optional)', 'failure: value equal to: 'accept', 'default', 'reject' (optional)' (optional)', 'sort_as_null: list with values of type: string | number | boolean (optional)', 'validation: record with the fields: 'allow_null: boolean (optional)', 'default: boolean | number | string | record | list (optional)', 'allow_YY: boolean (optional)' (optional)', 'type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)') - Columns describes various aspects about each individual column.
    ///&#96;name&#96; and &#96;id&#96; are the only required parameters.
    ///&#10;
    ///• include_headers_on_copy_paste (boolean; default false) - If true, headers are included when copying from the table to different
    ///tabs and elsewhere. Note that headers are ignored when copying from the table onto itself and
    ///between two tables within the same tab.
    ///&#10;
    ///• locale_format (record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)') - The localization specific formatting information applied to all columns in the table.
    ///This prop is derived from the [d3.formatLocale](https://github.com/d3/d3-format#formatLocale) data structure specification.
    ///When left unspecified, each individual nested prop will default to a pre-determined value.
    ///&#10;
    ///• markdown_options (record with the fields: 'link_target: string | value equal to: '_blank', '_parent', '_self', '_top' (optional)', 'html: boolean (optional)'; default {
    ///    link_target: '_blank',
    ///    html: false
    ///}) - The &#96;markdown_options&#96; property allows customization of the markdown cells behavior.
    ///&#10;
    ///• css (list with values of type: record with the fields: 'selector: string (required)', 'rule: string (required)'; default []) - The &#96;css&#96; property is a way to embed CSS selectors and rules
    ///onto the page.
    ///We recommend starting with the &#96;style_*&#96; properties
    ///before using this &#96;css&#96; property.
    ///Example:
    ///[
    ///    {"selector": ".dash-spreadsheet", "rule": 'font-family: "monospace"'}
    ///]
    ///&#10;
    ///• data (list with values of type: record) - The contents of the table.
    ///The keys of each item in data should match the column IDs.
    ///Each item can also have an 'id' key, whose value is its row ID. If there
    ///is a column with ID='id' this will display the row ID, otherwise it is
    ///just used to reference the row for selections, filtering, etc.
    ///Example:
    ///[
    ///     {'column-1': 4.5, 'column-2': 'montreal', 'column-3': 'canada'},
    ///     {'column-1': 8, 'column-2': 'boston', 'column-3': 'america'}
    ///]
    ///&#10;
    ///• data_previous (list with values of type: record) - The previous state of &#96;data&#96;. &#96;data_previous&#96;
    ///has the same structure as &#96;data&#96; and it will be updated
    ///whenever &#96;data&#96; changes, either through a callback or
    ///by editing the table.
    ///This is a read-only property: setting this property will not
    ///have any impact on the table.
    ///&#10;
    ///• data_timestamp (number) - The unix timestamp when the data was last edited.
    ///Use this property with other timestamp properties
    ///(such as &#96;n_clicks_timestamp&#96; in &#96;dash_html_components&#96;)
    ///to determine which property has changed within a callback.
    ///&#10;
    ///• editable (boolean; default false) - If True, then the data in all of the cells is editable.
    ///When &#96;editable&#96; is True, particular columns can be made
    ///uneditable by setting &#96;editable&#96; to &#96;False&#96; inside the &#96;columns&#96;
    ///property.
    ///If False, then the data in all of the cells is uneditable.
    ///When &#96;editable&#96; is False, particular columns can be made
    ///editable by setting &#96;editable&#96; to &#96;True&#96; inside the &#96;columns&#96;
    ///property.
    ///&#10;
    ///• end_cell (record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)') - When selecting multiple cells
    ///(via clicking on a cell and then shift-clicking on another cell),
    ///&#96;end_cell&#96; represents the row / column coordinates and IDs of the cell
    ///in one of the corners of the region.
    ///&#96;start_cell&#96; represents the coordinates of the other corner.
    ///&#10;
    ///• export_columns (value equal to: 'all', 'visible'; default visible) - Denotes the columns that will be used in the export data file.
    ///If &#96;all&#96;, all columns will be used (visible + hidden). If &#96;visible&#96;,
    ///only the visible columns will be used. Defaults to &#96;visible&#96;.
    ///&#10;
    ///• export_format (value equal to: 'csv', 'xlsx', 'none'; default none) - Denotes the type of the export data file,
    ///Defaults to &#96;'none'&#96;
    ///&#10;
    ///• export_headers (value equal to: 'none', 'ids', 'names', 'display') - Denotes the format of the headers in the export data file.
    ///If &#96;'none'&#96;, there will be no header. If &#96;'display'&#96;, then the header
    ///of the data file will be be how it is currently displayed. Note that
    ///&#96;'display'&#96; is only supported for &#96;'xlsx'&#96; export_format and will behave
    ///like &#96;'names'&#96; for &#96;'csv'&#96; export format. If &#96;'ids'&#96; or &#96;'names'&#96;,
    ///then the headers of data file will be the column id or the column
    ///names, respectively
    ///&#10;
    ///• fill_width (boolean; default true) - &#96;fill_width&#96; toggles between a set of CSS for two common behaviors:
    ///True: The table container's width will grow to fill the available space;
    ///False: The table container's width will equal the width of its content.
    ///&#10;
    ///• hidden_columns (list with values of type: string) - List of columns ids of the columns that are currently hidden.
    ///See the associated nested prop &#96;columns.hideable&#96;.
    ///&#10;
    ///• is_focused (boolean) - If True, then the &#96;active_cell&#96; is in a focused state.
    ///&#10;
    ///• merge_duplicate_headers (boolean) - If True, then column headers that have neighbors with duplicate names
    ///will be merged into a single cell.
    ///This will be applied for single column headers and multi-column
    ///headers.
    ///&#10;
    ///• fixed_columns (record with the fields: 'data: value equal to: '0' (optional)', 'headers: value equal to: 'false' (optional)' | record with the fields: 'data: number (optional)', 'headers: value equal to: 'true' (required)'; default {
    ///    headers: false,
    ///    data: 0
    ///}) - &#96;fixed_columns&#96; will "fix" the set of columns so that
    ///they remain visible when scrolling horizontally across
    ///the unfixed columns. &#96;fixed_columns&#96; fixes columns
    ///from left-to-right.
    ///If &#96;headers&#96; is False, no columns are fixed.
    ///If &#96;headers&#96; is True, all operation columns (see &#96;row_deletable&#96; and &#96;row_selectable&#96;)
    ///are fixed. Additional data columns can be fixed by
    ///assigning a number to &#96;data&#96;.
    ///Note that fixing columns introduces some changes to the
    ///underlying markup of the table and may impact the
    ///way that your columns are rendered or sized.
    ///View the documentation examples to learn more.
    ///&#10;
    ///• fixed_rows (record with the fields: 'data: value equal to: '0' (optional)', 'headers: value equal to: 'false' (optional)' | record with the fields: 'data: number (optional)', 'headers: value equal to: 'true' (required)'; default {
    ///    headers: false,
    ///    data: 0
    ///}) - &#96;fixed_rows&#96; will "fix" the set of rows so that
    ///they remain visible when scrolling vertically down
    ///the table. &#96;fixed_rows&#96; fixes rows
    ///from top-to-bottom, starting from the headers.
    ///If &#96;headers&#96; is False, no rows are fixed.
    ///If &#96;headers&#96; is True, all header and filter rows (see &#96;filter_action&#96;) are
    ///fixed. Additional data rows can be fixed by assigning
    ///a number to &#96;data&#96;.  Note that fixing rows introduces some changes to the
    ///underlying markup of the table and may impact the
    ///way that your columns are rendered or sized.
    ///View the documentation examples to learn more.
    ///&#10;
    ///• column_selectable (value equal to: 'single', 'multi', 'false'; default false) - If &#96;single&#96;, then the user can select a single column or group
    ///of merged columns via the radio button that will appear in the
    ///header rows.
    ///If &#96;multi&#96;, then the user can select multiple columns or groups
    ///of merged columns via the checkbox that will appear in the header
    ///rows.
    ///If false, then the user will not be able to select columns and no
    ///input will appear in the header rows.
    ///When a column is selected, its id will be contained in &#96;selected_columns&#96;
    ///and &#96;derived_viewport_selected_columns&#96;.
    ///&#10;
    ///• row_deletable (boolean) - If True, then a &#96;x&#96; will appear next to each &#96;row&#96;
    ///and the user can delete the row.
    ///&#10;
    ///• cell_selectable (boolean; default true) - If True (default), then it is possible to click and navigate
    ///table cells.
    ///&#10;
    ///• row_selectable (value equal to: 'single', 'multi', 'false'; default false) - If &#96;single&#96;, then the user can select a single row
    ///via a radio button that will appear next to each row.
    ///If &#96;multi&#96;, then the user can select multiple rows
    ///via a checkbox that will appear next to each row.
    ///If false, then the user will not be able to select rows
    ///and no additional UI elements will appear.
    ///When a row is selected, its index will be contained
    ///in &#96;selected_rows&#96;.
    ///&#10;
    ///• selected_cells (list with values of type: record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)'; default []) - &#96;selected_cells&#96; represents the set of cells that are selected,
    ///as an array of objects, each item similar to &#96;active_cell&#96;.
    ///Multiple cells can be selected by holding down shift and
    ///clicking on a different cell or holding down shift and navigating
    ///with the arrow keys.
    ///&#10;
    ///• selected_rows (list with values of type: number; default []) - &#96;selected_rows&#96; contains the indices of rows that
    ///are selected via the UI elements that appear when
    ///&#96;row_selectable&#96; is &#96;'single'&#96; or &#96;'multi'&#96;.
    ///&#10;
    ///• selected_columns (list with values of type: string; default []) - &#96;selected_columns&#96; contains the ids of columns that
    ///are selected via the UI elements that appear when
    ///&#96;column_selectable&#96; is &#96;'single' or 'multi'&#96;.
    ///&#10;
    ///• selected_row_ids (list with values of type: string | number; default []) - &#96;selected_row_ids&#96; contains the ids of rows that
    ///are selected via the UI elements that appear when
    ///&#96;row_selectable&#96; is &#96;'single'&#96; or &#96;'multi'&#96;.
    ///&#10;
    ///• start_cell (record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)') - When selecting multiple cells
    ///(via clicking on a cell and then shift-clicking on another cell),
    ///&#96;start_cell&#96; represents the [row, column] coordinates of the cell
    ///in one of the corners of the region.
    ///&#96;end_cell&#96; represents the coordinates of the other corner.
    ///&#10;
    ///• style_as_list_view (boolean; default false) - If True, then the table will be styled like a list view
    ///and not have borders between the columns.
    ///&#10;
    ///• page_action (value equal to: 'custom', 'native', 'none'; default native) - &#96;page_action&#96; refers to a mode of the table where
    ///not all of the rows are displayed at once: only a subset
    ///are displayed (a "page") and the next subset of rows
    ///can viewed by clicking "Next" or "Previous" buttons
    ///at the bottom of the page.
    ///Pagination is used to improve performance: instead of
    ///rendering all of the rows at once (which can be expensive),
    ///we only display a subset of them.
    ///With pagination, we can either page through data that exists
    ///in the table (e.g. page through &#96;10,000&#96; rows in &#96;data&#96; &#96;100&#96; rows at a time)
    ///or we can update the data on-the-fly with callbacks
    ///when the user clicks on the "Previous" or "Next" buttons.
    ///These modes can be toggled with this &#96;page_action&#96; parameter:
    ///&#96;'native'&#96;: all data is passed to the table up-front, paging logic is
    ///handled by the table;
    ///&#96;'custom'&#96;: data is passed to the table one page at a time, paging logic
    ///is handled via callbacks;
    ///&#96;'none'&#96;: disables paging, render all of the data at once.
    ///&#10;
    ///• page_current (number; default 0) - &#96;page_current&#96; represents which page the user is on.
    ///Use this property to index through data in your callbacks with
    ///backend paging.
    ///&#10;
    ///• page_count (number) - &#96;page_count&#96; represents the number of the pages in the
    ///paginated table. This is really only useful when performing
    ///backend pagination, since the front end is able to use the
    ///full size of the table to calculate the number of pages.
    ///&#10;
    ///• page_size (number; default 250) - &#96;page_size&#96; represents the number of rows that will be
    ///displayed on a particular page when &#96;page_action&#96; is &#96;'custom'&#96; or &#96;'native'&#96;
    ///&#10;
    ///• dropdown (dict with values of type: record with the fields: 'clearable: boolean (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'; default {}) - &#96;dropdown&#96; specifies dropdown options for different columns.
    ///Each entry refers to the column ID.
    ///The &#96;clearable&#96; property defines whether the value can be deleted.
    ///The &#96;options&#96; property refers to the &#96;options&#96; of the dropdown.
    ///&#10;
    ///• dropdown_conditional (list with values of type: record with the fields: 'clearable: boolean (optional)', 'if: record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)' (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'; default []) - &#96;dropdown_conditional&#96; specifies dropdown options in various columns and cells.
    ///This property allows you to specify different dropdowns
    ///depending on certain conditions. For example, you may
    ///render different "city" dropdowns in a row depending on the
    ///current value in the "state" column.
    ///&#10;
    ///• dropdown_data (list with values of type: dict with values of type: record with the fields: 'clearable: boolean (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'; default []) - &#96;dropdown_data&#96; specifies dropdown options on a row-by-row, column-by-column basis.
    ///Each item in the array corresponds to the corresponding dropdowns for the &#96;data&#96; item
    ///at the same index. Each entry in the item refers to the Column ID.
    ///&#10;
    ///• tooltip (dict with values of type: string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'use_with: value equal to: 'both', 'data', 'header' (optional)', 'value: string (required)'; default {}) - &#96;tooltip&#96; is the column based tooltip configuration applied to all rows. The key is the column
    /// id and the value is a tooltip configuration.
    ///Example: {i: {'value': i, 'use_with: 'both'} for i in df.columns}
    ///&#10;
    ///• tooltip_conditional (list with values of type: record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'if: record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)', 'row_index: number | value equal to: 'odd', 'even' (optional)' (required)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'; default []) - &#96;tooltip_conditional&#96; represents the tooltip shown
    ///for different columns and cells.
    ///This property allows you to specify different tooltips
    ///depending on certain conditions. For example, you may have
    ///different tooltips in the same column based on the value
    ///of a certain data property.
    ///Priority is from first to last defined conditional tooltip
    ///in the list. Higher priority (more specific) conditional
    ///tooltips should be put at the beginning of the list.
    ///&#10;
    ///• tooltip_data (list with values of type: dict with values of type: string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'; default []) - &#96;tooltip_data&#96; represents the tooltip shown
    ///for different columns and cells.
    ///A list of dicts for which each key is
    ///a column id and the value is a tooltip configuration.
    ///&#10;
    ///• tooltip_header (dict with values of type: string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)' | list with values of type: value equal to: 'null' | string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'; default {}) - &#96;tooltip_header&#96; represents the tooltip shown
    ///for each header column and optionally each header row.
    ///Example to show long column names in a tooltip: {i: i for i in df.columns}.
    ///Example to show different column names in a tooltip: {'Rep': 'Republican', 'Dem': 'Democrat'}.
    ///If the table has multiple rows of headers, then use a list as the value of the
    ///&#96;tooltip_header&#96; items.
    ///&#10;
    ///• tooltip_delay (number; default 350) - &#96;tooltip_delay&#96; represents the table-wide delay in milliseconds before
    ///the tooltip is shown when hovering a cell. If set to &#96;None&#96;, the tooltip
    ///will be shown immediately.
    ///Defaults to 350.
    ///&#10;
    ///• tooltip_duration (number; default 2000) - &#96;tooltip_duration&#96; represents the table-wide duration in milliseconds
    ///during which the tooltip will be displayed when hovering a cell. If
    ///set to &#96;None&#96;, the tooltip will not disappear.
    ///Defaults to 2000.
    ///&#10;
    ///• filter_query (string; default ) - If &#96;filter_action&#96; is enabled, then the current filtering
    ///string is represented in this &#96;filter_query&#96;
    ///property.
    ///&#10;
    ///• filter_action (value equal to: 'custom', 'native', 'none' | record with the fields: 'type: value equal to: 'custom', 'native' (required)', 'operator: value equal to: 'and', 'or' (optional)'; default none) - The &#96;filter_action&#96; property controls the behavior of the &#96;filtering&#96; UI.
    ///If &#96;'none'&#96;, then the filtering UI is not displayed.
    ///If &#96;'native'&#96;, then the filtering UI is displayed and the filtering
    ///logic is handled by the table. That is, it is performed on the data
    ///that exists in the &#96;data&#96; property.
    ///If &#96;'custom'&#96;, then the filtering UI is displayed but it is the
    ///responsibility of the developer to program the filtering
    ///through a callback (where &#96;filter_query&#96; or &#96;derived_filter_query_structure&#96; would be the input
    ///and &#96;data&#96; would be the output).
    ///&#10;
    ///• filter_options (record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)'; default {}) - There are two &#96;filter_options&#96; props in the table.
    ///This is the table-level filter_options prop and there is
    ///also the column-level &#96;filter_options&#96; prop.
    ///These props determine whether the applicable filter relational
    ///operators will default to &#96;sensitive&#96; or &#96;insensitive&#96; comparison.
    ///If the column-level &#96;filter_options&#96; prop is set it overrides
    ///the table-level &#96;filter_options&#96; prop for that column.
    ///&#10;
    ///• sort_action (value equal to: 'custom', 'native', 'none'; default none) - The &#96;sort_action&#96; property enables data to be
    ///sorted on a per-column basis.
    ///If &#96;'none'&#96;, then the sorting UI is not displayed.
    ///If &#96;'native'&#96;, then the sorting UI is displayed and the sorting
    ///logic is handled by the table. That is, it is performed on the data
    ///that exists in the &#96;data&#96; property.
    ///If &#96;'custom'&#96;, the the sorting UI is displayed but it is the
    ///responsibility of the developer to program the sorting
    ///through a callback (where &#96;sort_by&#96; would be the input and &#96;data&#96;
    ///would be the output).
    ///Clicking on the sort arrows will update the
    ///&#96;sort_by&#96; property.
    ///&#10;
    ///• sort_mode (value equal to: 'single', 'multi'; default single) - Sorting can be performed across multiple columns
    ///(e.g. sort by country, sort within each country,
    /// sort by year) or by a single column.
    ///NOTE - With multi-column sort, it's currently
    ///not possible to determine the order in which
    ///the columns were sorted through the UI.
    ///See [https://github.com/plotly/dash-table/issues/170](https://github.com/plotly/dash-table/issues/170)
    ///&#10;
    ///• sort_by (list with values of type: record with the fields: 'column_id: string (required)', 'direction: value equal to: 'asc', 'desc' (required)'; default []) - &#96;sort_by&#96; describes the current state
    ///of the sorting UI.
    ///That is, if the user clicked on the sort arrow
    ///of a column, then this property will be updated
    ///with the column ID and the direction
    ///(&#96;asc&#96; or &#96;desc&#96;) of the sort.
    ///For multi-column sorting, this will be a list of
    ///sorting parameters, in the order in which they were
    ///clicked.
    ///&#10;
    ///• sort_as_null (list with values of type: string | number | boolean; default []) - An array of string, number and boolean values that are treated as &#96;None&#96;
    ///(i.e. ignored and always displayed last) when sorting.
    ///This value will be used by columns without &#96;sort_as_null&#96;.
    ///Defaults to &#96;[]&#96;.
    ///&#10;
    ///• style_table (record; default {}) - CSS styles to be applied to the outer &#96;table&#96; container.
    ///This is commonly used for setting properties like the
    ///width or the height of the table.
    ///&#10;
    ///• style_cell (record) - CSS styles to be applied to each individual cell of the table.
    ///This includes the header cells, the &#96;data&#96; cells, and the filter
    ///cells.
    ///&#10;
    ///• style_data (record) - CSS styles to be applied to each individual data cell.
    ///That is, unlike &#96;style_cell&#96;, it excludes the header and filter cells.
    ///&#10;
    ///• style_filter (record) - CSS styles to be applied to the filter cells.
    ///Note that this may change in the future as we build out a
    ///more complex filtering UI.
    ///&#10;
    ///• style_header (record) - CSS styles to be applied to each individual header cell.
    ///That is, unlike &#96;style_cell&#96;, it excludes the &#96;data&#96; and filter cells.
    ///&#10;
    ///• style_cell_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)' (optional)'; default []) - Conditional CSS styles for the cells.
    ///This can be used to apply styles to cells on a per-column basis.
    ///&#10;
    ///• style_data_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'filter_query: string (optional)', 'state: value equal to: 'active', 'selected' (optional)', 'row_index: number | value equal to: 'odd', 'even' | list with values of type: number (optional)', 'column_editable: boolean (optional)' (optional)'; default []) - Conditional CSS styles for the data cells.
    ///This can be used to apply styles to data cells on a per-column basis.
    ///&#10;
    ///• style_filter_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'column_editable: boolean (optional)' (optional)'; default []) - Conditional CSS styles for the filter cells.
    ///This can be used to apply styles to filter cells on a per-column basis.
    ///&#10;
    ///• style_header_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'header_index: number | list with values of type: number | value equal to: 'odd', 'even' (optional)', 'column_editable: boolean (optional)' (optional)'; default []) - Conditional CSS styles for the header cells.
    ///This can be used to apply styles to header cells on a per-column basis.
    ///&#10;
    ///• virtualization (boolean; default false) - This property tells the table to use virtualization when rendering.
    ///Assumptions are that:
    ///the width of the columns is fixed;
    ///the height of the rows is always the same; and
    ///runtime styling changes will not affect width and height vs. first rendering
    ///&#10;
    ///• derived_filter_query_structure (record) - This property represents the current structure of
    ///&#96;filter_query&#96; as a tree structure. Each node of the
    ///query structure has:
    ///type (string; required):
    ///  'open-block',
    ///  'logical-operator',
    ///  'relational-operator',
    ///  'unary-operator', or
    ///  'expression';
    ///subType (string; optional):
    ///  'open-block': '()',
    ///  'logical-operator': '&amp;&amp;', '||',
    ///  'relational-operator': '=', '&gt;=', '&gt;', '&lt;=', '&lt;', '!=', 'contains',
    ///  'unary-operator': '!', 'is bool', 'is even', 'is nil', 'is num', 'is object', 'is odd', 'is prime', 'is str',
    ///  'expression': 'value', 'field';
    ///value (any):
    ///  'expression, value': passed value,
    ///  'expression, field': the field/prop name.
    ///block (nested query structure; optional).
    ///left (nested query structure; optional).
    ///right (nested query structure; optional).
    ///If the query is invalid or empty, the &#96;derived_filter_query_structure&#96; will
    ///be &#96;None&#96;.
    ///&#10;
    ///• derived_viewport_data (list with values of type: record; default []) - This property represents the current state of &#96;data&#96;
    ///on the current page. This property will be updated
    ///on paging, sorting, and filtering.
    ///&#10;
    ///• derived_viewport_indices (list with values of type: number; default []) - &#96;derived_viewport_indices&#96; indicates the order in which the original
    ///rows appear after being filtered, sorted, and/or paged.
    ///&#96;derived_viewport_indices&#96; contains indices for the current page,
    ///while &#96;derived_virtual_indices&#96; contains indices across all pages.
    ///&#10;
    ///• derived_viewport_row_ids (list with values of type: string | number; default []) - &#96;derived_viewport_row_ids&#96; lists row IDs in the order they appear
    ///after being filtered, sorted, and/or paged.
    ///&#96;derived_viewport_row_ids&#96; contains IDs for the current page,
    ///while &#96;derived_virtual_row_ids&#96; contains IDs across all pages.
    ///&#10;
    ///• derived_viewport_selected_columns (list with values of type: string) - &#96;derived_viewport_selected_columns&#96; contains the ids of the
    ///&#96;selected_columns&#96; that are not currently hidden.
    ///&#10;
    ///• derived_viewport_selected_rows (list with values of type: number; default []) - &#96;derived_viewport_selected_rows&#96; represents the indices of the
    ///&#96;selected_rows&#96; from the perspective of the &#96;derived_viewport_indices&#96;.
    ///&#10;
    ///• derived_viewport_selected_row_ids (list with values of type: string | number; default []) - &#96;derived_viewport_selected_row_ids&#96; represents the IDs of the
    ///&#96;selected_rows&#96; on the currently visible page.
    ///&#10;
    ///• derived_virtual_data (list with values of type: record; default []) - This property represents the visible state of &#96;data&#96;
    ///across all pages after the front-end sorting and filtering
    ///as been applied.
    ///&#10;
    ///• derived_virtual_indices (list with values of type: number; default []) - &#96;derived_virtual_indices&#96; indicates the order in which the original
    ///rows appear after being filtered and sorted.
    ///&#96;derived_viewport_indices&#96; contains indices for the current page,
    ///while &#96;derived_virtual_indices&#96; contains indices across all pages.
    ///&#10;
    ///• derived_virtual_row_ids (list with values of type: string | number; default []) - &#96;derived_virtual_row_ids&#96; indicates the row IDs in the order in which
    ///they appear after being filtered and sorted.
    ///&#96;derived_viewport_row_ids&#96; contains IDs for the current page,
    ///while &#96;derived_virtual_row_ids&#96; contains IDs across all pages.
    ///&#10;
    ///• derived_virtual_selected_rows (list with values of type: number; default []) - &#96;derived_virtual_selected_rows&#96; represents the indices of the
    /// &#96;selected_rows&#96; from the perspective of the &#96;derived_virtual_indices&#96;.
    ///&#10;
    ///• derived_virtual_selected_row_ids (list with values of type: string | number; default []) - &#96;derived_virtual_selected_row_ids&#96; represents the IDs of the
    ///&#96;selected_rows&#96; as they appear after filtering and sorting,
    ///across all pages.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///&#10;
    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
    ///hasn't changed from its previous value, any &#96;persisted_props&#96; that the
    ///user has changed while using the app will keep those changes, as long as
    ///the new prop value also matches what was given originally.
    ///Used in conjunction with &#96;persistence_type&#96; and &#96;persisted_props&#96;.
    ///&#10;
    ///• persisted_props (list with values of type: value equal to: 'columns_name', 'data', 'filter_query', 'hidden_columns', 'selected_columns', 'selected_rows', 'sort_by'; default [
    ///    'columns_name',
    ///    'filter_query',
    ///    'hidden_columns',
    ///    'selected_columns',
    ///    'selected_rows',
    ///    'sort_by'
    ///]) - Properties whose user interactions will persist after refreshing the
    ///component or the page.
    ///&#10;
    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///</summary>
    type Prop =
        | ActiveCell of ActiveCell
        | Columns of seq<Column>
        | IncludeHeadersOnCopyPaste of bool
        | LocaleFormat of LocaleFormat
        | MarkdownOptions of MarkdownOptions
        | Css of seq<Css>
        | Data of seq<obj>
        | DataPrevious of seq<obj>
        | DataTimestamp of int64
        | Editable of bool
        | EndCell of Cell
        | ExportColumns of ExportColumns
        | ExportFormat of MaybeExportFormat
        | ExportHeaders of MaybeExportHeaders
        | FillWidth of bool
        | HiddenColumns of seq<string>
        | IsFocused of bool
        | MergeDuplicateHeaders of bool
        | FixedColumns of Fixed
        | FixedRows of Fixed
        | ColumnSelectable of Select
        | RowDeletable of bool
        | CellSelectable of bool
        | RowSelectable of Select
        | SelectedCells of seq<Cell>
        | SelectedRows of seq<int>
        | SelectedColumns of seq<string>
        | SelectedRowIds of seq<IConvertible>
        | StartCell of Cell
        | StyleAsListView of bool
        | PageAction of MaybeActionType
        | PageCurrent of int
        | PageCount of int
        | PageSize of int
        | Dropdown of Map<string, Dropdown>
        | DropdownConditional of seq<ConditionalDropdown>
        | DropdownData of seq<Map<string, Dropdown>>
        | Tooltip of Map<string, ColumnTooltip>
        | TooltipConditional of seq<ConditionalTooltip>
        | TooltipData of seq<Map<string, DataTooltip>>
        | TooltipHeader of Map<string, HeaderTooltip>
        | TooltipDelay of int
        | TooltipDuration of int
        | FilterQuery of string
        | FilterAction of FilterAction
        | FilterOptions of FilterOption
        | SortAction of MaybeActionType
        | SortMode of SortMode
        | SortBy of seq<SortBy>
        | SortAsNull of seq<IConvertible>
        | StyleTable of DashComponentStyle
        | StyleCell of DashComponentStyle
        | StyleData of DashComponentStyle
        | StyleFilter of DashComponentStyle
        | StyleHeader of DashComponentStyle
        | StyleCellConditional of seq<ConditionalCellStyle>
        | StyleDataConditional of seq<ConditionalDataStyle>
        | StyleFilterConditional of seq<ConditionalFilterStyle>
        | StyleHeaderConditional of seq<ConditionalHeaderStyle>
        | Virtualization of bool
        | DerivedFilterQueryStructure of obj
        | DerivedViewportData of seq<obj>
        | DerivedViewportIndices of seq<int>
        | DerivedViewportRowIds of seq<IConvertible>
        | DerivedViewportSelectedColumns of seq<string>
        | DerivedViewportSelectedRows of seq<int>
        | DerivedViewportSelectedRowIds of seq<IConvertible>
        | DerivedVirtualData of seq<obj>
        | DerivedVirtualIndices of seq<int>
        | DerivedVirtualRowIds of seq<IConvertible>
        | DerivedVirtualSelectedRows of seq<int>
        | DerivedVirtualSelectedRowIds of seq<IConvertible>
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions

        static member convert = function
            | ActiveCell p -> box p
            | Columns p -> box p
            | IncludeHeadersOnCopyPaste p -> box p
            | LocaleFormat p -> box p
            | MarkdownOptions p -> box p
            | Css p -> box p
            | Data p -> box p
            | DataPrevious p -> box p
            | DataTimestamp p -> box p
            | Editable p -> box p
            | EndCell p -> box p
            | ExportColumns p -> Convert.fromDiscriminatedUnion p
            | ExportFormat p -> Convert.fromDiscriminatedUnion p
            | ExportHeaders p -> Convert.fromDiscriminatedUnion p
            | FillWidth p -> box p
            | HiddenColumns p -> box p
            | IsFocused p -> box p
            | MergeDuplicateHeaders p -> box p
            | FixedColumns p -> box p
            | FixedRows p -> box p
            | ColumnSelectable p -> Convert.fromDiscriminatedUnion p
            | RowDeletable p -> box p
            | CellSelectable p -> box p
            | RowSelectable p -> Convert.fromDiscriminatedUnion p
            | SelectedCells p -> box p
            | SelectedRows p -> box p
            | SelectedColumns p -> box p
            | SelectedRowIds p -> box p
            | StartCell p -> box p
            | StyleAsListView p -> box p
            | PageAction p -> Convert.fromDiscriminatedUnion p
            | PageCurrent p -> box p
            | PageCount p -> box p
            | PageSize p -> box p
            | Dropdown p -> box p
            | DropdownConditional p -> box p
            | DropdownData p -> box p
            | Tooltip p -> Convert.fromMappedDiscriminatedUnions p
            | TooltipConditional p -> box p
            | TooltipData p -> p |> Seq.map Convert.fromMappedDiscriminatedUnions |> box
            | TooltipHeader p -> Convert.fromMappedDiscriminatedUnions p
            | TooltipDelay p -> box p
            | TooltipDuration p -> box p
            | FilterQuery p -> box p
            | FilterAction p -> Convert.fromDiscriminatedUnion p
            | FilterOptions p ->  box p
            | SortAction p -> Convert.fromDiscriminatedUnion p
            | SortMode p -> Convert.fromDiscriminatedUnion p
            | SortBy p -> box p
            | SortAsNull p -> box p
            | StyleTable p -> box p
            | StyleCell p -> box p
            | StyleData p -> box p
            | StyleFilter p -> box p
            | StyleHeader p -> box p
            | StyleCellConditional p -> box p
            | StyleDataConditional p -> box p
            | StyleFilterConditional p -> box p
            | StyleHeaderConditional p -> box p
            | Virtualization p -> box p
            | DerivedFilterQueryStructure p -> box p
            | DerivedViewportData p -> box p
            | DerivedViewportIndices p -> box p
            | DerivedViewportRowIds p -> box p
            | DerivedViewportSelectedColumns p -> box p
            | DerivedViewportSelectedRows p -> box p
            | DerivedViewportSelectedRowIds p -> box p
            | DerivedVirtualData p -> box p
            | DerivedVirtualIndices p -> box p
            | DerivedVirtualRowIds p -> box p
            | DerivedVirtualSelectedRows p -> box p
            | DerivedVirtualSelectedRowIds p -> box p
            | LoadingState p -> box p
            | Persistence p -> box p
            | PersistedProps p -> box p
            | PersistenceType p -> Convert.fromDiscriminatedUnion p

        static member toDynamicMemberDef(prop: Prop) =
            prop |> Prop.toDynamicMemberPropName, Prop.convert prop

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of DashComponent list
        ///<summary>
        ///The row and column indices and IDs of the currently active cell.
        ///&#96;row_id&#96; is only returned if the data rows have an &#96;id&#96; key.
        ///</summary>
        static member activeCell(p: ActiveCell) = Prop(ActiveCell p)
        ///<summary>
        ///Columns describes various aspects about each individual column.
        ///&#96;name&#96; and &#96;id&#96; are the only required parameters.
        ///</summary>
        static member columns(p: #seq<Column>) = Prop(Columns p)
        ///<summary>
        ///If true, headers are included when copying from the table to different
        ///tabs and elsewhere. Note that headers are ignored when copying from the table onto itself and
        ///between two tables within the same tab.
        ///</summary>
        static member includeHeadersOnCopyPaste(p: bool) = Prop(IncludeHeadersOnCopyPaste p)
        ///<summary>
        ///The localization specific formatting information applied to all columns in the table.
        ///This prop is derived from the [d3.formatLocale](https://github.com/d3/d3-format#formatLocale) data structure specification.
        ///When left unspecified, each individual nested prop will default to a pre-determined value.
        ///</summary>
        static member localeFormat(p: LocaleFormat) = Prop(LocaleFormat p)
        ///<summary>
        ///The &#96;markdown_options&#96; property allows customization of the markdown cells behavior.
        ///</summary>
        static member markdownOptions(p: MarkdownOptions) = Prop(MarkdownOptions p)
        ///<summary>
        ///The &#96;css&#96; property is a way to embed CSS selectors and rules
        ///onto the page.
        ///We recommend starting with the &#96;style_*&#96; properties
        ///before using this &#96;css&#96; property.
        ///Example:
        ///[
        ///    {"selector": ".dash-spreadsheet", "rule": 'font-family: "monospace"'}
        ///]
        ///</summary>
        static member css(p: #seq<Css>) = Prop(Css p)
        ///<summary>
        ///The contents of the table.
        ///The keys of each item in data should match the column IDs.
        ///Each item can also have an 'id' key, whose value is its row ID. If there
        ///is a column with ID='id' this will display the row ID, otherwise it is
        ///just used to reference the row for selections, filtering, etc.
        ///Example:
        ///[
        ///     {'column-1': 4.5, 'column-2': 'montreal', 'column-3': 'canada'},
        ///     {'column-1': 8, 'column-2': 'boston', 'column-3': 'america'}
        ///]
        ///</summary>
        static member data(p: #seq<obj> ) = Prop(Data p)
        ///<summary>
        ///The previous state of &#96;data&#96;. &#96;data_previous&#96;
        ///has the same structure as &#96;data&#96; and it will be updated
        ///whenever &#96;data&#96; changes, either through a callback or
        ///by editing the table.
        ///This is a read-only property: setting this property will not
        ///have any impact on the table.
        ///</summary>
        static member dataPrevious(p: #seq<obj>) = Prop(DataPrevious p)
        ///<summary>
        ///The unix timestamp when the data was last edited.
        ///Use this property with other timestamp properties
        ///(such as &#96;n_clicks_timestamp&#96; in &#96;dash_html_components&#96;)
        ///to determine which property has changed within a callback.
        ///</summary>
        static member dataTimestamp(p: int64) = Prop(DataTimestamp p)
        ///<summary>
        ///If True, then the data in all of the cells is editable.
        ///When &#96;editable&#96; is True, particular columns can be made
        ///uneditable by setting &#96;editable&#96; to &#96;False&#96; inside the &#96;columns&#96;
        ///property.
        ///If False, then the data in all of the cells is uneditable.
        ///When &#96;editable&#96; is False, particular columns can be made
        ///editable by setting &#96;editable&#96; to &#96;True&#96; inside the &#96;columns&#96;
        ///property.
        ///</summary>
        static member editable(p: bool) = Prop(Editable p)
        ///<summary>
        ///When selecting multiple cells
        ///(via clicking on a cell and then shift-clicking on another cell),
        ///&#96;end_cell&#96; represents the row / column coordinates and IDs of the cell
        ///in one of the corners of the region.
        ///&#96;start_cell&#96; represents the coordinates of the other corner.
        ///</summary>
        static member endCell(p: Cell) = Prop(EndCell p)
        ///<summary>
        ///Denotes the columns that will be used in the export data file.
        ///If &#96;all&#96;, all columns will be used (visible + hidden). If &#96;visible&#96;,
        ///only the visible columns will be used. Defaults to &#96;visible&#96;.
        ///</summary>
        static member exportColumns(p: ExportColumns) = Prop(ExportColumns p)
        ///<summary>
        ///Denotes the type of the export data file,
        ///Defaults to &#96;'none'&#96;
        ///</summary>
        static member exportFormat(p: MaybeExportFormat) = Prop(ExportFormat p)
        ///<summary>
        ///Denotes the format of the headers in the export data file.
        ///If &#96;'none'&#96;, there will be no header. If &#96;'display'&#96;, then the header
        ///of the data file will be be how it is currently displayed. Note that
        ///&#96;'display'&#96; is only supported for &#96;'xlsx'&#96; export_format and will behave
        ///like &#96;'names'&#96; for &#96;'csv'&#96; export format. If &#96;'ids'&#96; or &#96;'names'&#96;,
        ///then the headers of data file will be the column id or the column
        ///names, respectively
        ///</summary>
        static member exportHeaders(p: MaybeExportHeaders) = Prop(ExportHeaders p)
        ///<summary>
        ///&#96;fill_width&#96; toggles between a set of CSS for two common behaviors:
        ///True: The table container's width will grow to fill the available space;
        ///False: The table container's width will equal the width of its content.
        ///</summary>
        static member fillWidth(p: bool) = Prop(FillWidth p)
        ///<summary>
        ///List of columns ids of the columns that are currently hidden.
        ///See the associated nested prop &#96;columns.hideable&#96;.
        ///</summary>
        static member hiddenColumns(p: #seq<string>) = Prop(HiddenColumns p)
        ///<summary>
        ///If True, then the &#96;active_cell&#96; is in a focused state.
        ///</summary>
        static member isFocused(p: bool) = Prop(IsFocused p)
        ///<summary>
        ///If True, then column headers that have neighbors with duplicate names
        ///will be merged into a single cell.
        ///This will be applied for single column headers and multi-column
        ///headers.
        ///</summary>
        static member mergeDuplicateHeaders(p: bool) = Prop(MergeDuplicateHeaders p)
        ///<summary>
        ///&#96;fixed_columns&#96; will "fix" the set of columns so that
        ///they remain visible when scrolling horizontally across
        ///the unfixed columns. &#96;fixed_columns&#96; fixes columns
        ///from left-to-right.
        ///If &#96;headers&#96; is False, no columns are fixed.
        ///If &#96;headers&#96; is True, all operation columns (see &#96;row_deletable&#96; and &#96;row_selectable&#96;)
        ///are fixed. Additional data columns can be fixed by
        ///assigning a number to &#96;data&#96;.
        ///Note that fixing columns introduces some changes to the
        ///underlying markup of the table and may impact the
        ///way that your columns are rendered or sized.
        ///View the documentation examples to learn more.
        ///</summary>
        static member fixedColumns(p: Fixed) = Prop(FixedColumns p)
        ///<summary>
        ///&#96;fixed_rows&#96; will "fix" the set of rows so that
        ///they remain visible when scrolling vertically down
        ///the table. &#96;fixed_rows&#96; fixes rows
        ///from top-to-bottom, starting from the headers.
        ///If &#96;headers&#96; is False, no rows are fixed.
        ///If &#96;headers&#96; is True, all header and filter rows (see &#96;filter_action&#96;) are
        ///fixed. Additional data rows can be fixed by assigning
        ///a number to &#96;data&#96;.  Note that fixing rows introduces some changes to the
        ///underlying markup of the table and may impact the
        ///way that your columns are rendered or sized.
        ///View the documentation examples to learn more.
        ///</summary>
        static member fixedRows(p: Fixed) = Prop(FixedRows p)
        ///<summary>
        ///If &#96;single&#96;, then the user can select a single column or group
        ///of merged columns via the radio button that will appear in the
        ///header rows.
        ///If &#96;multi&#96;, then the user can select multiple columns or groups
        ///of merged columns via the checkbox that will appear in the header
        ///rows.
        ///If false, then the user will not be able to select columns and no
        ///input will appear in the header rows.
        ///When a column is selected, its id will be contained in &#96;selected_columns&#96;
        ///and &#96;derived_viewport_selected_columns&#96;.
        ///</summary>
        static member columnSelectable(p: Select) = Prop(ColumnSelectable p)
        ///<summary>
        ///If True, then a &#96;x&#96; will appear next to each &#96;row&#96;
        ///and the user can delete the row.
        ///</summary>
        static member rowDeletable(p: bool) = Prop(RowDeletable p)
        ///<summary>
        ///If True (default), then it is possible to click and navigate
        ///table cells.
        ///</summary>
        static member cellSelectable(p: bool) = Prop(CellSelectable p)
        ///<summary>
        ///If &#96;single&#96;, then the user can select a single row
        ///via a radio button that will appear next to each row.
        ///If &#96;multi&#96;, then the user can select multiple rows
        ///via a checkbox that will appear next to each row.
        ///If false, then the user will not be able to select rows
        ///and no additional UI elements will appear.
        ///When a row is selected, its index will be contained
        ///in &#96;selected_rows&#96;.
        ///</summary>
        static member rowSelectable(p: Select) = Prop(RowSelectable p)
        ///<summary>
        ///&#96;selected_cells&#96; represents the set of cells that are selected,
        ///as an array of objects, each item similar to &#96;active_cell&#96;.
        ///Multiple cells can be selected by holding down shift and
        ///clicking on a different cell or holding down shift and navigating
        ///with the arrow keys.
        ///</summary>
        static member selectedCells(p: #seq<Cell>) = Prop(SelectedCells p)
        ///<summary>
        ///&#96;selected_rows&#96; contains the indices of rows that
        ///are selected via the UI elements that appear when
        ///&#96;row_selectable&#96; is &#96;'single'&#96; or &#96;'multi'&#96;.
        ///</summary>
        static member selectedRows(p: #seq<int>) = Prop(SelectedRows p)
        ///<summary>
        ///&#96;selected_columns&#96; contains the ids of columns that
        ///are selected via the UI elements that appear when
        ///&#96;column_selectable&#96; is &#96;'single' or 'multi'&#96;.
        ///</summary>
        static member selectedColumns(p: #seq<string>) = Prop(SelectedColumns p)
        ///<summary>
        ///&#96;selected_row_ids&#96; contains the ids of rows that
        ///are selected via the UI elements that appear when
        ///&#96;row_selectable&#96; is &#96;'single'&#96; or &#96;'multi'&#96;.
        ///</summary>
        static member selectedRowIds(p: #seq<IConvertible>) = Prop(SelectedRowIds p)
        ///<summary>
        ///When selecting multiple cells
        ///(via clicking on a cell and then shift-clicking on another cell),
        ///&#96;start_cell&#96; represents the [row, column] coordinates of the cell
        ///in one of the corners of the region.
        ///&#96;end_cell&#96; represents the coordinates of the other corner.
        ///</summary>
        static member startCell(p: Cell) = Prop(StartCell p)
        ///<summary>
        ///If True, then the table will be styled like a list view
        ///and not have borders between the columns.
        ///</summary>
        static member styleAsListView(p: bool) = Prop(StyleAsListView p)
        ///<summary>
        ///&#96;page_action&#96; refers to a mode of the table where
        ///not all of the rows are displayed at once: only a subset
        ///are displayed (a "page") and the next subset of rows
        ///can viewed by clicking "Next" or "Previous" buttons
        ///at the bottom of the page.
        ///Pagination is used to improve performance: instead of
        ///rendering all of the rows at once (which can be expensive),
        ///we only display a subset of them.
        ///With pagination, we can either page through data that exists
        ///in the table (e.g. page through &#96;10,000&#96; rows in &#96;data&#96; &#96;100&#96; rows at a time)
        ///or we can update the data on-the-fly with callbacks
        ///when the user clicks on the "Previous" or "Next" buttons.
        ///These modes can be toggled with this &#96;page_action&#96; parameter:
        ///&#96;'native'&#96;: all data is passed to the table up-front, paging logic is
        ///handled by the table;
        ///&#96;'custom'&#96;: data is passed to the table one page at a time, paging logic
        ///is handled via callbacks;
        ///&#96;'none'&#96;: disables paging, render all of the data at once.
        ///</summary>
        static member pageAction(p: MaybeActionType) = Prop(PageAction p)
        ///<summary>
        ///&#96;page_current&#96; represents which page the user is on.
        ///Use this property to index through data in your callbacks with
        ///backend paging.
        ///</summary>
        static member pageCurrent(p: int) = Prop(PageCurrent p)
        ///<summary>
        ///&#96;page_count&#96; represents the number of the pages in the
        ///paginated table. This is really only useful when performing
        ///backend pagination, since the front end is able to use the
        ///full size of the table to calculate the number of pages.
        ///</summary>
        static member pageCount(p: int) = Prop(PageCount p)
        ///<summary>
        ///&#96;page_size&#96; represents the number of rows that will be
        ///displayed on a particular page when &#96;page_action&#96; is &#96;'custom'&#96; or &#96;'native'&#96;
        ///</summary>
        static member pageSize(p: int) = Prop(PageSize p)
        ///<summary>
        ///&#96;dropdown&#96; specifies dropdown options for different columns.
        ///Each entry refers to the column ID.
        ///The &#96;clearable&#96; property defines whether the value can be deleted.
        ///The &#96;options&#96; property refers to the &#96;options&#96; of the dropdown.
        ///</summary>
        static member dropdown(p: Map<string, Dropdown>) = Prop(Dropdown p)
        ///<summary>
        ///&#96;dropdown_conditional&#96; specifies dropdown options in various columns and cells.
        ///This property allows you to specify different dropdowns
        ///depending on certain conditions. For example, you may
        ///render different "city" dropdowns in a row depending on the
        ///current value in the "state" column.
        ///</summary>
        static member dropdownConditional(p: #seq<ConditionalDropdown>) = Prop(DropdownConditional p)
        ///<summary>
        ///&#96;dropdown_data&#96; specifies dropdown options on a row-by-row, column-by-column basis.
        ///Each item in the array corresponds to the corresponding dropdowns for the &#96;data&#96; item
        ///at the same index. Each entry in the item refers to the Column ID.
        ///</summary>
        static member dropdownData(p: seq<Map<string, Dropdown>>) = Prop(DropdownData p)
        ///<summary>
        ///&#96;tooltip&#96; is the column based tooltip configuration applied to all rows. The key is the column
        /// id and the value is a tooltip configuration.
        ///Example: {i: {'value': i, 'use_with: 'both'} for i in df.columns}
        ///</summary>
        static member tooltip(p: Map<string, ColumnTooltip>) = Prop(Tooltip p)
        ///<summary>
        ///&#96;tooltip_conditional&#96; represents the tooltip shown
        ///for different columns and cells.
        ///This property allows you to specify different tooltips
        ///depending on certain conditions. For example, you may have
        ///different tooltips in the same column based on the value
        ///of a certain data property.
        ///Priority is from first to last defined conditional tooltip
        ///in the list. Higher priority (more specific) conditional
        ///tooltips should be put at the beginning of the list.
        ///</summary>
        static member tooltipConditional(p: #seq<ConditionalTooltip>) = Prop(TooltipConditional p)
        ///<summary>
        ///&#96;tooltip_data&#96; represents the tooltip shown
        ///for different columns and cells.
        ///A list of dicts for which each key is
        ///a column id and the value is a tooltip configuration.
        ///</summary>
        static member tooltipData(p: #seq<Map<string, DataTooltip>>) = Prop(TooltipData p)
        ///<summary>
        ///&#96;tooltip_header&#96; represents the tooltip shown
        ///for each header column and optionally each header row.
        ///Example to show long column names in a tooltip: {i: i for i in df.columns}.
        ///Example to show different column names in a tooltip: {'Rep': 'Republican', 'Dem': 'Democrat'}.
        ///If the table has multiple rows of headers, then use a list as the value of the
        ///&#96;tooltip_header&#96; items.
        ///</summary>
        static member tooltipHeader(p: Map<string, HeaderTooltip>) = Prop(TooltipHeader p)
        ///<summary>
        ///&#96;tooltip_delay&#96; represents the table-wide delay in milliseconds before
        ///the tooltip is shown when hovering a cell. If set to &#96;None&#96;, the tooltip
        ///will be shown immediately.
        ///Defaults to 350.
        ///</summary>
        static member tooltipDelay(p: int) = Prop(TooltipDelay p)
        ///<summary>
        ///&#96;tooltip_duration&#96; represents the table-wide duration in milliseconds
        ///during which the tooltip will be displayed when hovering a cell. If
        ///set to &#96;None&#96;, the tooltip will not disappear.
        ///Defaults to 2000.
        ///</summary>
        static member tooltipDuration(p: int) = Prop(TooltipDuration p)
        ///<summary>
        ///If &#96;filter_action&#96; is enabled, then the current filtering
        ///string is represented in this &#96;filter_query&#96;
        ///property.
        ///</summary>
        static member filterQuery(p: string) = Prop(FilterQuery p)
        ///<summary>
        ///The &#96;filter_action&#96; property controls the behavior of the &#96;filtering&#96; UI.
        ///If &#96;'none'&#96;, then the filtering UI is not displayed.
        ///If &#96;'native'&#96;, then the filtering UI is displayed and the filtering
        ///logic is handled by the table. That is, it is performed on the data
        ///that exists in the &#96;data&#96; property.
        ///If &#96;'custom'&#96;, then the filtering UI is displayed but it is the
        ///responsibility of the developer to program the filtering
        ///through a callback (where &#96;filter_query&#96; or &#96;derived_filter_query_structure&#96; would be the input
        ///and &#96;data&#96; would be the output).
        ///</summary>
        static member filterAction(p: MaybeActionType) = Prop(FilterAction(FilterAction.Type p))
        ///<summary>
        ///The &#96;filter_action&#96; property controls the behavior of the &#96;filtering&#96; UI.
        ///If &#96;'none'&#96;, then the filtering UI is not displayed.
        ///If &#96;'native'&#96;, then the filtering UI is displayed and the filtering
        ///logic is handled by the table. That is, it is performed on the data
        ///that exists in the &#96;data&#96; property.
        ///If &#96;'custom'&#96;, then the filtering UI is displayed but it is the
        ///responsibility of the developer to program the filtering
        ///through a callback (where &#96;filter_query&#96; or &#96;derived_filter_query_structure&#96; would be the input
        ///and &#96;data&#96; would be the output).
        ///</summary>
        static member filterAction(p: ConditionalFilterAction) = Prop(FilterAction(FilterAction.Conditional p))
        ///<summary>
        ///There are two &#96;filter_options&#96; props in the table.
        ///This is the table-level filter_options prop and there is
        ///also the column-level &#96;filter_options&#96; prop.
        ///These props determine whether the applicable filter relational
        ///operators will default to &#96;sensitive&#96; or &#96;insensitive&#96; comparison.
        ///If the column-level &#96;filter_options&#96; prop is set it overrides
        ///the table-level &#96;filter_options&#96; prop for that column.
        ///</summary>
        static member filterOptions(p: FilterOption) = Prop(FilterOptions p)
        ///<summary>
        ///The &#96;sort_action&#96; property enables data to be
        ///sorted on a per-column basis.
        ///If &#96;'none'&#96;, then the sorting UI is not displayed.
        ///If &#96;'native'&#96;, then the sorting UI is displayed and the sorting
        ///logic is handled by the table. That is, it is performed on the data
        ///that exists in the &#96;data&#96; property.
        ///If &#96;'custom'&#96;, the the sorting UI is displayed but it is the
        ///responsibility of the developer to program the sorting
        ///through a callback (where &#96;sort_by&#96; would be the input and &#96;data&#96;
        ///would be the output).
        ///Clicking on the sort arrows will update the
        ///&#96;sort_by&#96; property.
        ///</summary>
        static member sortAction(p: MaybeActionType) = Prop(SortAction p)
        ///<summary>
        ///Sorting can be performed across multiple columns
        ///(e.g. sort by country, sort within each country,
        /// sort by year) or by a single column.
        ///NOTE - With multi-column sort, it's currently
        ///not possible to determine the order in which
        ///the columns were sorted through the UI.
        ///See [https://github.com/plotly/dash-table/issues/170](https://github.com/plotly/dash-table/issues/170)
        ///</summary>
        static member sortMode(p: SortMode) = Prop(SortMode p)
        ///<summary>
        ///&#96;sort_by&#96; describes the current state
        ///of the sorting UI.
        ///That is, if the user clicked on the sort arrow
        ///of a column, then this property will be updated
        ///with the column ID and the direction
        ///(&#96;asc&#96; or &#96;desc&#96;) of the sort.
        ///For multi-column sorting, this will be a list of
        ///sorting parameters, in the order in which they were
        ///clicked.
        ///</summary>
        static member sortBy(p: #seq<SortBy>) = Prop(SortBy p)
        ///<summary>
        ///An array of string, number and boolean values that are treated as &#96;None&#96;
        ///(i.e. ignored and always displayed last) when sorting.
        ///This value will be used by columns without &#96;sort_as_null&#96;.
        ///Defaults to &#96;[]&#96;.
        ///</summary>
        static member sortAsNull(p: #seq<IConvertible>) = Prop(SortAsNull p)
        ///<summary>
        ///CSS styles to be applied to the outer &#96;table&#96; container.
        ///This is commonly used for setting properties like the
        ///width or the height of the table.
        ///</summary>
        static member styleTable(p: seq<Css.Style>) = Prop(StyleTable(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///CSS styles to be applied to each individual cell of the table.
        ///This includes the header cells, the &#96;data&#96; cells, and the filter
        ///cells.
        ///</summary>
        static member styleCell(p: seq<Css.Style>) = Prop(StyleCell(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///CSS styles to be applied to each individual data cell.
        ///That is, unlike &#96;style_cell&#96;, it excludes the header and filter cells.
        ///</summary>
        static member styleData(p: seq<Css.Style>) = Prop(StyleData(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///CSS styles to be applied to the filter cells.
        ///Note that this may change in the future as we build out a
        ///more complex filtering UI.
        ///</summary>
        static member styleFilter(p: seq<Css.Style>) = Prop(StyleFilter(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///CSS styles to be applied to each individual header cell.
        ///That is, unlike &#96;style_cell&#96;, it excludes the &#96;data&#96; and filter cells.
        ///</summary>
        static member styleHeader(p: seq<Css.Style>) = Prop(StyleHeader(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Conditional CSS styles for the cells.
        ///This can be used to apply styles to cells on a per-column basis.
        ///</summary>
        static member styleCellConditional(p: #seq<ConditionalCellStyle>) = Prop(StyleCellConditional p)
        ///<summary>
        ///Conditional CSS styles for the data cells.
        ///This can be used to apply styles to data cells on a per-column basis.
        ///</summary>
        static member styleDataConditional(p: #seq<ConditionalDataStyle>) = Prop(StyleDataConditional p)
        ///<summary>
        ///Conditional CSS styles for the filter cells.
        ///This can be used to apply styles to filter cells on a per-column basis.
        ///</summary>
        static member styleFilterConditional(p: #seq<ConditionalFilterStyle>) = Prop(StyleFilterConditional p)
        ///<summary>
        ///Conditional CSS styles for the header cells.
        ///This can be used to apply styles to header cells on a per-column basis.
        ///</summary>
        static member styleHeaderConditional(p: #seq<ConditionalHeaderStyle>) = Prop(StyleHeaderConditional p)
        ///<summary>
        ///This property tells the table to use virtualization when rendering.
        ///Assumptions are that:
        ///the width of the columns is fixed;
        ///the height of the rows is always the same; and
        ///runtime styling changes will not affect width and height vs. first rendering
        ///</summary>
        static member virtualization(p: bool) = Prop(Virtualization p)
        ///<summary>
        ///This property represents the current structure of
        ///&#96;filter_query&#96; as a tree structure. Each node of the
        ///query structure has:
        ///type (string; required):
        ///  'open-block',
        ///  'logical-operator',
        ///  'relational-operator',
        ///  'unary-operator', or
        ///  'expression';
        ///subType (string; optional):
        ///  'open-block': '()',
        ///  'logical-operator': '&amp;&amp;', '||',
        ///  'relational-operator': '=', '&gt;=', '&gt;', '&lt;=', '&lt;', '!=', 'contains',
        ///  'unary-operator': '!', 'is bool', 'is even', 'is nil', 'is num', 'is object', 'is odd', 'is prime', 'is str',
        ///  'expression': 'value', 'field';
        ///value (any):
        ///  'expression, value': passed value,
        ///  'expression, field': the field/prop name.
        ///block (nested query structure; optional).
        ///left (nested query structure; optional).
        ///right (nested query structure; optional).
        ///If the query is invalid or empty, the &#96;derived_filter_query_structure&#96; will
        ///be &#96;None&#96;.
        ///</summary>
        static member derivedFilterQueryStructure(p: obj) = Prop(DerivedFilterQueryStructure p)
        ///<summary>
        ///This property represents the current state of &#96;data&#96;
        ///on the current page. This property will be updated
        ///on paging, sorting, and filtering.
        ///</summary>
        static member derivedViewportData(p: #seq<obj>) = Prop(DerivedViewportData p)
        ///<summary>
        ///&#96;derived_viewport_indices&#96; indicates the order in which the original
        ///rows appear after being filtered, sorted, and/or paged.
        ///&#96;derived_viewport_indices&#96; contains indices for the current page,
        ///while &#96;derived_virtual_indices&#96; contains indices across all pages.
        ///</summary>
        static member derivedViewportIndices(p: #seq<int>) = Prop(DerivedViewportIndices p)
        ///<summary>
        ///&#96;derived_viewport_row_ids&#96; lists row IDs in the order they appear
        ///after being filtered, sorted, and/or paged.
        ///&#96;derived_viewport_row_ids&#96; contains IDs for the current page,
        ///while &#96;derived_virtual_row_ids&#96; contains IDs across all pages.
        ///</summary>
        static member derivedViewportRowIds(p: #seq<IConvertible>) = Prop(DerivedViewportRowIds p)
        ///<summary>
        ///&#96;derived_viewport_selected_columns&#96; contains the ids of the
        ///&#96;selected_columns&#96; that are not currently hidden.
        ///</summary>
        static member derivedViewportSelectedColumns(p: #seq<string>) =
            Prop(DerivedViewportSelectedColumns p)
        ///<summary>
        ///&#96;derived_viewport_selected_rows&#96; represents the indices of the
        ///&#96;selected_rows&#96; from the perspective of the &#96;derived_viewport_indices&#96;.
        ///</summary>
        static member derivedViewportSelectedRows(p: #seq<int>) = Prop(DerivedViewportSelectedRows p)
        ///<summary>
        ///&#96;derived_viewport_selected_row_ids&#96; represents the IDs of the
        ///&#96;selected_rows&#96; on the currently visible page.
        ///</summary>
        static member derivedViewportSelectedRowIds(p: #seq<IConvertible>) = Prop(DerivedViewportSelectedRowIds p)
        ///<summary>
        ///This property represents the visible state of &#96;data&#96;
        ///across all pages after the front-end sorting and filtering
        ///as been applied.
        ///</summary>
        static member derivedVirtualData(p: #seq<obj>) = Prop(DerivedVirtualData p)
        ///<summary>
        ///&#96;derived_virtual_indices&#96; indicates the order in which the original
        ///rows appear after being filtered and sorted.
        ///&#96;derived_viewport_indices&#96; contains indices for the current page,
        ///while &#96;derived_virtual_indices&#96; contains indices across all pages.
        ///</summary>
        static member derivedVirtualIndices(p: #seq<int>) = Prop(DerivedVirtualIndices p)
        ///<summary>
        ///&#96;derived_virtual_row_ids&#96; indicates the row IDs in the order in which
        ///they appear after being filtered and sorted.
        ///&#96;derived_viewport_row_ids&#96; contains IDs for the current page,
        ///while &#96;derived_virtual_row_ids&#96; contains IDs across all pages.
        ///</summary>
        static member derivedVirtualRowIds(p: #seq<IConvertible>) = Prop(DerivedVirtualRowIds p)
        ///<summary>
        ///&#96;derived_virtual_selected_rows&#96; represents the indices of the
        /// &#96;selected_rows&#96; from the perspective of the &#96;derived_virtual_indices&#96;.
        ///</summary>
        static member derivedVirtualSelectedRows(p: #seq<int>) = Prop(DerivedVirtualSelectedRows p)
        ///<summary>
        ///&#96;derived_virtual_selected_row_ids&#96; represents the IDs of the
        ///&#96;selected_rows&#96; as they appear after filtering and sorting,
        ///across all pages.
        ///</summary>
        static member derivedVirtualSelectedRowIds(p: #seq<IConvertible>) = Prop(DerivedVirtualSelectedRowIds p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = Prop(LoadingState p)
        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, any &#96;persisted_props&#96; that the
        ///user has changed while using the app will keep those changes, as long as
        ///the new prop value also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96; and &#96;persisted_props&#96;.
        ///</summary>
        static member persistence(p: IConvertible) = Prop(Persistence p)
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page.
        ///</summary>
        static member persistedProps(p: string []) = Prop(PersistedProps p)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = Children [ Html.text value ] 
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children [ Html.text value ]
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children [ Html.text value ]
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Guid) = Children [ Html.text value ]
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: DashComponent) = Children [ value ]
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: DashComponent list) = Children(value)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

    ///<summary>
    ///Dash DataTable is an interactive table component designed for
    ///designed for viewing, editing, and exploring large datasets.
    ///DataTable is rendered with standard, semantic HTML &lt;table/&gt; markup,
    ///which makes it accessible, responsive, and easy to style. This
    ///component was written from scratch in React.js specifically for the
    ///Dash community. Its API was designed to be ergonomic and its behavior
    ///is completely customizable through its properties.
    ///</summary>
    type DataTable() =
        inherit DashComponent()

        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?activeCell,
                ?columns,
                ?includeHeadersOnCopyPaste,
                ?localeFormat,
                ?markdownOptions,
                ?css,
                ?data,
                ?dataPrevious,
                ?dataTimestamp,
                ?editable,
                ?endCell,
                ?exportColumns,
                ?exportFormat,
                ?exportHeaders,
                ?fillWidth,
                ?hiddenColumns,
                ?isFocused,
                ?mergeDuplicateHeaders,
                ?fixedColumns,
                ?fixedRows,
                ?columnSelectable,
                ?rowDeletable,
                ?cellSelectable,
                ?rowSelectable,
                ?selectedCells,
                ?selectedRows,
                ?selectedColumns,
                ?selectedRowIds,
                ?startCell,
                ?styleAsListView,
                ?pageAction,
                ?pageCurrent,
                ?pageCount,
                ?pageSize,
                ?dropdown,
                ?dropdownConditional,
                ?dropdownData,
                ?tooltip,
                ?tooltipConditional,
                ?tooltipData,
                ?tooltipHeader,
                ?tooltipDelay,
                ?tooltipDuration,
                ?filterQuery,
                ?filterAction,
                ?filterOptions,
                ?sortAction,
                ?sortMode,
                ?sortBy,
                ?sortAsNull,
                ?styleTable,
                ?styleCell,
                ?styleData,
                ?styleFilter,
                ?styleHeader,
                ?styleCellConditional,
                ?styleDataConditional,
                ?styleFilterConditional,
                ?styleHeaderConditional,
                ?virtualization,
                ?derivedFilterQueryStructure,
                ?derivedViewportData,
                ?derivedViewportIndices,
                ?derivedViewportRowIds,
                ?derivedViewportSelectedColumns,
                ?derivedViewportSelectedRows,
                ?derivedViewportSelectedRowIds,
                ?derivedVirtualData,
                ?derivedVirtualIndices,
                ?derivedVirtualRowIds,
                ?derivedVirtualSelectedRows,
                ?derivedVirtualSelectedRowIds,
                ?loadingState,
                ?persistence,
                ?persistedProps,
                ?persistenceType
            ) =
            fun (dataTable: DataTable) ->
                let props = DashComponentProps()
                let inline setValueOpt prop =
                    DynObj.setValueOpt props prop Prop.convert

                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                setValueOpt ActiveCell activeCell
                setValueOpt Columns columns
                setValueOpt IncludeHeadersOnCopyPaste includeHeadersOnCopyPaste
                setValueOpt LocaleFormat localeFormat
                setValueOpt MarkdownOptions markdownOptions
                setValueOpt Css css
                setValueOpt Data data
                setValueOpt DataPrevious dataPrevious
                setValueOpt DataTimestamp dataTimestamp
                setValueOpt Editable editable
                setValueOpt EndCell endCell
                setValueOpt ExportColumns exportColumns
                setValueOpt ExportFormat exportFormat
                setValueOpt ExportHeaders exportHeaders
                setValueOpt FillWidth fillWidth
                setValueOpt HiddenColumns hiddenColumns
                setValueOpt IsFocused isFocused
                setValueOpt MergeDuplicateHeaders mergeDuplicateHeaders
                setValueOpt FixedColumns fixedColumns
                setValueOpt FixedRows fixedRows
                setValueOpt ColumnSelectable columnSelectable
                setValueOpt RowDeletable rowDeletable
                setValueOpt CellSelectable cellSelectable
                setValueOpt RowSelectable rowSelectable
                setValueOpt SelectedCells selectedCells
                setValueOpt SelectedRows selectedRows
                setValueOpt SelectedColumns selectedColumns
                setValueOpt SelectedRowIds selectedRowIds
                setValueOpt StartCell startCell
                setValueOpt StyleAsListView styleAsListView
                setValueOpt PageAction pageAction
                setValueOpt PageCurrent pageCurrent
                setValueOpt PageCount pageCount
                setValueOpt PageSize pageSize
                setValueOpt Dropdown dropdown
                setValueOpt DropdownConditional dropdownConditional
                setValueOpt DropdownData dropdownData
                setValueOpt Tooltip tooltip
                setValueOpt TooltipConditional tooltipConditional
                setValueOpt TooltipData tooltipData
                setValueOpt TooltipHeader tooltipHeader
                setValueOpt TooltipDelay tooltipDelay
                setValueOpt TooltipDuration tooltipDuration
                setValueOpt FilterQuery filterQuery
                setValueOpt FilterAction filterAction
                setValueOpt FilterOptions filterOptions
                setValueOpt SortAction sortAction
                setValueOpt SortMode sortMode
                setValueOpt SortBy sortBy
                setValueOpt SortAsNull sortAsNull
                setValueOpt StyleTable styleTable
                setValueOpt StyleCell styleCell
                setValueOpt StyleData styleData
                setValueOpt StyleFilter styleFilter
                setValueOpt StyleHeader styleHeader
                setValueOpt StyleCellConditional styleCellConditional
                setValueOpt StyleDataConditional styleDataConditional
                setValueOpt StyleFilterConditional styleFilterConditional
                setValueOpt StyleHeaderConditional styleHeaderConditional
                setValueOpt Virtualization virtualization
                setValueOpt DerivedFilterQueryStructure derivedFilterQueryStructure
                setValueOpt DerivedViewportData derivedViewportData
                setValueOpt DerivedViewportIndices derivedViewportIndices
                setValueOpt DerivedViewportRowIds derivedViewportRowIds
                setValueOpt DerivedViewportSelectedColumns derivedViewportSelectedColumns
                setValueOpt DerivedViewportSelectedRows derivedViewportSelectedRows
                setValueOpt DerivedViewportSelectedRowIds derivedViewportSelectedRowIds
                setValueOpt DerivedVirtualData derivedVirtualData
                setValueOpt DerivedVirtualIndices derivedVirtualIndices
                setValueOpt DerivedVirtualRowIds derivedVirtualRowIds
                setValueOpt DerivedVirtualSelectedRows derivedVirtualSelectedRows
                setValueOpt DerivedVirtualSelectedRowIds derivedVirtualSelectedRowIds
                setValueOpt LoadingState loadingState
                setValueOpt Persistence persistence
                setValueOpt PersistedProps persistedProps
                setValueOpt PersistenceType persistenceType
                DynObj.setValue dataTable "namespace" "dash_table"
                DynObj.setValue dataTable "props" props
                DynObj.setValue dataTable "type" "DataTable"
                dataTable

        static member init
            (
                id,
                children,
                ?activeCell,
                ?columns,
                ?includeHeadersOnCopyPaste,
                ?localeFormat,
                ?markdownOptions,
                ?css,
                ?data,
                ?dataPrevious,
                ?dataTimestamp,
                ?editable,
                ?endCell,
                ?exportColumns,
                ?exportFormat,
                ?exportHeaders,
                ?fillWidth,
                ?hiddenColumns,
                ?isFocused,
                ?mergeDuplicateHeaders,
                ?fixedColumns,
                ?fixedRows,
                ?columnSelectable,
                ?rowDeletable,
                ?cellSelectable,
                ?rowSelectable,
                ?selectedCells,
                ?selectedRows,
                ?selectedColumns,
                ?selectedRowIds,
                ?startCell,
                ?styleAsListView,
                ?pageAction,
                ?pageCurrent,
                ?pageCount,
                ?pageSize,
                ?dropdown,
                ?dropdownConditional,
                ?dropdownData,
                ?tooltip,
                ?tooltipConditional,
                ?tooltipData,
                ?tooltipHeader,
                ?tooltipDelay,
                ?tooltipDuration,
                ?filterQuery,
                ?filterAction,
                ?filterOptions,
                ?sortAction,
                ?sortMode,
                ?sortBy,
                ?sortAsNull,
                ?styleTable,
                ?styleCell,
                ?styleData,
                ?styleFilter,
                ?styleHeader,
                ?styleCellConditional,
                ?styleDataConditional,
                ?styleFilterConditional,
                ?styleHeaderConditional,
                ?virtualization,
                ?derivedFilterQueryStructure,
                ?derivedViewportData,
                ?derivedViewportIndices,
                ?derivedViewportRowIds,
                ?derivedViewportSelectedColumns,
                ?derivedViewportSelectedRows,
                ?derivedViewportSelectedRowIds,
                ?derivedVirtualData,
                ?derivedVirtualIndices,
                ?derivedVirtualRowIds,
                ?derivedVirtualSelectedRows,
                ?derivedVirtualSelectedRowIds,
                ?loadingState,
                ?persistence,
                ?persistedProps,
                ?persistenceType
            ) =
            DataTable.applyMembers (
                id,
                children,
                ?activeCell = activeCell,
                ?columns = columns,
                ?includeHeadersOnCopyPaste = includeHeadersOnCopyPaste,
                ?localeFormat = localeFormat,
                ?markdownOptions = markdownOptions,
                ?css = css,
                ?data = data,
                ?dataPrevious = dataPrevious,
                ?dataTimestamp = dataTimestamp,
                ?editable = editable,
                ?endCell = endCell,
                ?exportColumns = exportColumns,
                ?exportFormat = exportFormat,
                ?exportHeaders = exportHeaders,
                ?fillWidth = fillWidth,
                ?hiddenColumns = hiddenColumns,
                ?isFocused = isFocused,
                ?mergeDuplicateHeaders = mergeDuplicateHeaders,
                ?fixedColumns = fixedColumns,
                ?fixedRows = fixedRows,
                ?columnSelectable = columnSelectable,
                ?rowDeletable = rowDeletable,
                ?cellSelectable = cellSelectable,
                ?rowSelectable = rowSelectable,
                ?selectedCells = selectedCells,
                ?selectedRows = selectedRows,
                ?selectedColumns = selectedColumns,
                ?selectedRowIds = selectedRowIds,
                ?startCell = startCell,
                ?styleAsListView = styleAsListView,
                ?pageAction = pageAction,
                ?pageCurrent = pageCurrent,
                ?pageCount = pageCount,
                ?pageSize = pageSize,
                ?dropdown = dropdown,
                ?dropdownConditional = dropdownConditional,
                ?dropdownData = dropdownData,
                ?tooltip = tooltip,
                ?tooltipConditional = tooltipConditional,
                ?tooltipData = tooltipData,
                ?tooltipHeader = tooltipHeader,
                ?tooltipDelay = tooltipDelay,
                ?tooltipDuration = tooltipDuration,
                ?filterQuery = filterQuery,
                ?filterAction = filterAction,
                ?filterOptions = filterOptions,
                ?sortAction = sortAction,
                ?sortMode = sortMode,
                ?sortBy = sortBy,
                ?sortAsNull = sortAsNull,
                ?styleTable = styleTable,
                ?styleCell = styleCell,
                ?styleData = styleData,
                ?styleFilter = styleFilter,
                ?styleHeader = styleHeader,
                ?styleCellConditional = styleCellConditional,
                ?styleDataConditional = styleDataConditional,
                ?styleFilterConditional = styleFilterConditional,
                ?styleHeaderConditional = styleHeaderConditional,
                ?virtualization = virtualization,
                ?derivedFilterQueryStructure = derivedFilterQueryStructure,
                ?derivedViewportData = derivedViewportData,
                ?derivedViewportIndices = derivedViewportIndices,
                ?derivedViewportRowIds = derivedViewportRowIds,
                ?derivedViewportSelectedColumns = derivedViewportSelectedColumns,
                ?derivedViewportSelectedRows = derivedViewportSelectedRows,
                ?derivedViewportSelectedRowIds = derivedViewportSelectedRowIds,
                ?derivedVirtualData = derivedVirtualData,
                ?derivedVirtualIndices = derivedVirtualIndices,
                ?derivedVirtualRowIds = derivedVirtualRowIds,
                ?derivedVirtualSelectedRows = derivedVirtualSelectedRows,
                ?derivedVirtualSelectedRowIds = derivedVirtualSelectedRowIds,
                ?loadingState = loadingState,
                ?persistence = persistence,
                ?persistedProps = persistedProps,
                ?persistenceType = persistenceType
            ) (DataTable())

    ///<summary>
    ///Dash DataTable is an interactive table component designed for
    ///designed for viewing, editing, and exploring large datasets.
    ///DataTable is rendered with standard, semantic HTML &lt;table/&gt; markup,
    ///which makes it accessible, responsive, and easy to style. This
    ///component was written from scratch in React.js specifically for the
    ///Dash community. Its API was designed to be ergonomic and its behavior
    ///is completely customizable through its properties.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• active_cell (record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)') - The row and column indices and IDs of the currently active cell.
    ///&#96;row_id&#96; is only returned if the data rows have an &#96;id&#96; key.
    ///&#10;
    ///• columns (list with values of type: record with the fields: 'clearable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'deletable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'editable: boolean (optional)', 'filter_options: record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)' (optional)', 'hideable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'renamable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'selectable: value equal to: 'first', 'last' | boolean | list with values of type: boolean (optional)', 'format: record with the fields: 'locale: record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)' (optional)', 'nully: boolean | number | string | record | list (optional)', 'prefix: number (optional)', 'specifier: string (optional)' (optional)', 'id: string (required)', 'name: string | list with values of type: string (required)', 'presentation: value equal to: 'input', 'dropdown', 'markdown' (optional)', 'on_change: record with the fields: 'action: value equal to: 'coerce', 'none', 'validate' (optional)', 'failure: value equal to: 'accept', 'default', 'reject' (optional)' (optional)', 'sort_as_null: list with values of type: string | number | boolean (optional)', 'validation: record with the fields: 'allow_null: boolean (optional)', 'default: boolean | number | string | record | list (optional)', 'allow_YY: boolean (optional)' (optional)', 'type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)') - Columns describes various aspects about each individual column.
    ///&#96;name&#96; and &#96;id&#96; are the only required parameters.
    ///&#10;
    ///• include_headers_on_copy_paste (boolean; default false) - If true, headers are included when copying from the table to different
    ///tabs and elsewhere. Note that headers are ignored when copying from the table onto itself and
    ///between two tables within the same tab.
    ///&#10;
    ///• locale_format (record with the fields: 'symbol: list with values of type: string (optional)', 'decimal: string (optional)', 'group: string (optional)', 'grouping: list with values of type: number (optional)', 'numerals: list with values of type: string (optional)', 'percent: string (optional)', 'separate_4digits: boolean (optional)') - The localization specific formatting information applied to all columns in the table.
    ///This prop is derived from the [d3.formatLocale](https://github.com/d3/d3-format#formatLocale) data structure specification.
    ///When left unspecified, each individual nested prop will default to a pre-determined value.
    ///&#10;
    ///• markdown_options (record with the fields: 'link_target: string | value equal to: '_blank', '_parent', '_self', '_top' (optional)', 'html: boolean (optional)'; default {
    ///    link_target: '_blank',
    ///    html: false
    ///}) - The &#96;markdown_options&#96; property allows customization of the markdown cells behavior.
    ///&#10;
    ///• css (list with values of type: record with the fields: 'selector: string (required)', 'rule: string (required)'; default []) - The &#96;css&#96; property is a way to embed CSS selectors and rules
    ///onto the page.
    ///We recommend starting with the &#96;style_*&#96; properties
    ///before using this &#96;css&#96; property.
    ///Example:
    ///[
    ///    {"selector": ".dash-spreadsheet", "rule": 'font-family: "monospace"'}
    ///]
    ///&#10;
    ///• data (list with values of type: record) - The contents of the table.
    ///The keys of each item in data should match the column IDs.
    ///Each item can also have an 'id' key, whose value is its row ID. If there
    ///is a column with ID='id' this will display the row ID, otherwise it is
    ///just used to reference the row for selections, filtering, etc.
    ///Example:
    ///[
    ///     {'column-1': 4.5, 'column-2': 'montreal', 'column-3': 'canada'},
    ///     {'column-1': 8, 'column-2': 'boston', 'column-3': 'america'}
    ///]
    ///&#10;
    ///• data_previous (list with values of type: record) - The previous state of &#96;data&#96;. &#96;data_previous&#96;
    ///has the same structure as &#96;data&#96; and it will be updated
    ///whenever &#96;data&#96; changes, either through a callback or
    ///by editing the table.
    ///This is a read-only property: setting this property will not
    ///have any impact on the table.
    ///&#10;
    ///• data_timestamp (number) - The unix timestamp when the data was last edited.
    ///Use this property with other timestamp properties
    ///(such as &#96;n_clicks_timestamp&#96; in &#96;dash_html_components&#96;)
    ///to determine which property has changed within a callback.
    ///&#10;
    ///• editable (boolean; default false) - If True, then the data in all of the cells is editable.
    ///When &#96;editable&#96; is True, particular columns can be made
    ///uneditable by setting &#96;editable&#96; to &#96;False&#96; inside the &#96;columns&#96;
    ///property.
    ///If False, then the data in all of the cells is uneditable.
    ///When &#96;editable&#96; is False, particular columns can be made
    ///editable by setting &#96;editable&#96; to &#96;True&#96; inside the &#96;columns&#96;
    ///property.
    ///&#10;
    ///• end_cell (record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)') - When selecting multiple cells
    ///(via clicking on a cell and then shift-clicking on another cell),
    ///&#96;end_cell&#96; represents the row / column coordinates and IDs of the cell
    ///in one of the corners of the region.
    ///&#96;start_cell&#96; represents the coordinates of the other corner.
    ///&#10;
    ///• export_columns (value equal to: 'all', 'visible'; default visible) - Denotes the columns that will be used in the export data file.
    ///If &#96;all&#96;, all columns will be used (visible + hidden). If &#96;visible&#96;,
    ///only the visible columns will be used. Defaults to &#96;visible&#96;.
    ///&#10;
    ///• export_format (value equal to: 'csv', 'xlsx', 'none'; default none) - Denotes the type of the export data file,
    ///Defaults to &#96;'none'&#96;
    ///&#10;
    ///• export_headers (value equal to: 'none', 'ids', 'names', 'display') - Denotes the format of the headers in the export data file.
    ///If &#96;'none'&#96;, there will be no header. If &#96;'display'&#96;, then the header
    ///of the data file will be be how it is currently displayed. Note that
    ///&#96;'display'&#96; is only supported for &#96;'xlsx'&#96; export_format and will behave
    ///like &#96;'names'&#96; for &#96;'csv'&#96; export format. If &#96;'ids'&#96; or &#96;'names'&#96;,
    ///then the headers of data file will be the column id or the column
    ///names, respectively
    ///&#10;
    ///• fill_width (boolean; default true) - &#96;fill_width&#96; toggles between a set of CSS for two common behaviors:
    ///True: The table container's width will grow to fill the available space;
    ///False: The table container's width will equal the width of its content.
    ///&#10;
    ///• hidden_columns (list with values of type: string) - List of columns ids of the columns that are currently hidden.
    ///See the associated nested prop &#96;columns.hideable&#96;.
    ///&#10;
    ///• id (string) - The ID of the table.
    ///&#10;
    ///• is_focused (boolean) - If True, then the &#96;active_cell&#96; is in a focused state.
    ///&#10;
    ///• merge_duplicate_headers (boolean) - If True, then column headers that have neighbors with duplicate names
    ///will be merged into a single cell.
    ///This will be applied for single column headers and multi-column
    ///headers.
    ///&#10;
    ///• fixed_columns (record with the fields: 'data: value equal to: '0' (optional)', 'headers: value equal to: 'false' (optional)' | record with the fields: 'data: number (optional)', 'headers: value equal to: 'true' (required)'; default {
    ///    headers: false,
    ///    data: 0
    ///}) - &#96;fixed_columns&#96; will "fix" the set of columns so that
    ///they remain visible when scrolling horizontally across
    ///the unfixed columns. &#96;fixed_columns&#96; fixes columns
    ///from left-to-right.
    ///If &#96;headers&#96; is False, no columns are fixed.
    ///If &#96;headers&#96; is True, all operation columns (see &#96;row_deletable&#96; and &#96;row_selectable&#96;)
    ///are fixed. Additional data columns can be fixed by
    ///assigning a number to &#96;data&#96;.
    ///Note that fixing columns introduces some changes to the
    ///underlying markup of the table and may impact the
    ///way that your columns are rendered or sized.
    ///View the documentation examples to learn more.
    ///&#10;
    ///• fixed_rows (record with the fields: 'data: value equal to: '0' (optional)', 'headers: value equal to: 'false' (optional)' | record with the fields: 'data: number (optional)', 'headers: value equal to: 'true' (required)'; default {
    ///    headers: false,
    ///    data: 0
    ///}) - &#96;fixed_rows&#96; will "fix" the set of rows so that
    ///they remain visible when scrolling vertically down
    ///the table. &#96;fixed_rows&#96; fixes rows
    ///from top-to-bottom, starting from the headers.
    ///If &#96;headers&#96; is False, no rows are fixed.
    ///If &#96;headers&#96; is True, all header and filter rows (see &#96;filter_action&#96;) are
    ///fixed. Additional data rows can be fixed by assigning
    ///a number to &#96;data&#96;.  Note that fixing rows introduces some changes to the
    ///underlying markup of the table and may impact the
    ///way that your columns are rendered or sized.
    ///View the documentation examples to learn more.
    ///&#10;
    ///• column_selectable (value equal to: 'single', 'multi', 'false'; default false) - If &#96;single&#96;, then the user can select a single column or group
    ///of merged columns via the radio button that will appear in the
    ///header rows.
    ///If &#96;multi&#96;, then the user can select multiple columns or groups
    ///of merged columns via the checkbox that will appear in the header
    ///rows.
    ///If false, then the user will not be able to select columns and no
    ///input will appear in the header rows.
    ///When a column is selected, its id will be contained in &#96;selected_columns&#96;
    ///and &#96;derived_viewport_selected_columns&#96;.
    ///&#10;
    ///• row_deletable (boolean) - If True, then a &#96;x&#96; will appear next to each &#96;row&#96;
    ///and the user can delete the row.
    ///&#10;
    ///• cell_selectable (boolean; default true) - If True (default), then it is possible to click and navigate
    ///table cells.
    ///&#10;
    ///• row_selectable (value equal to: 'single', 'multi', 'false'; default false) - If &#96;single&#96;, then the user can select a single row
    ///via a radio button that will appear next to each row.
    ///If &#96;multi&#96;, then the user can select multiple rows
    ///via a checkbox that will appear next to each row.
    ///If false, then the user will not be able to select rows
    ///and no additional UI elements will appear.
    ///When a row is selected, its index will be contained
    ///in &#96;selected_rows&#96;.
    ///&#10;
    ///• selected_cells (list with values of type: record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)'; default []) - &#96;selected_cells&#96; represents the set of cells that are selected,
    ///as an array of objects, each item similar to &#96;active_cell&#96;.
    ///Multiple cells can be selected by holding down shift and
    ///clicking on a different cell or holding down shift and navigating
    ///with the arrow keys.
    ///&#10;
    ///• selected_rows (list with values of type: number; default []) - &#96;selected_rows&#96; contains the indices of rows that
    ///are selected via the UI elements that appear when
    ///&#96;row_selectable&#96; is &#96;'single'&#96; or &#96;'multi'&#96;.
    ///&#10;
    ///• selected_columns (list with values of type: string; default []) - &#96;selected_columns&#96; contains the ids of columns that
    ///are selected via the UI elements that appear when
    ///&#96;column_selectable&#96; is &#96;'single' or 'multi'&#96;.
    ///&#10;
    ///• selected_row_ids (list with values of type: string | number; default []) - &#96;selected_row_ids&#96; contains the ids of rows that
    ///are selected via the UI elements that appear when
    ///&#96;row_selectable&#96; is &#96;'single'&#96; or &#96;'multi'&#96;.
    ///&#10;
    ///• start_cell (record with the fields: 'row: number (optional)', 'column: number (optional)', 'row_id: string | number (optional)', 'column_id: string (optional)') - When selecting multiple cells
    ///(via clicking on a cell and then shift-clicking on another cell),
    ///&#96;start_cell&#96; represents the [row, column] coordinates of the cell
    ///in one of the corners of the region.
    ///&#96;end_cell&#96; represents the coordinates of the other corner.
    ///&#10;
    ///• style_as_list_view (boolean; default false) - If True, then the table will be styled like a list view
    ///and not have borders between the columns.
    ///&#10;
    ///• page_action (value equal to: 'custom', 'native', 'none'; default native) - &#96;page_action&#96; refers to a mode of the table where
    ///not all of the rows are displayed at once: only a subset
    ///are displayed (a "page") and the next subset of rows
    ///can viewed by clicking "Next" or "Previous" buttons
    ///at the bottom of the page.
    ///Pagination is used to improve performance: instead of
    ///rendering all of the rows at once (which can be expensive),
    ///we only display a subset of them.
    ///With pagination, we can either page through data that exists
    ///in the table (e.g. page through &#96;10,000&#96; rows in &#96;data&#96; &#96;100&#96; rows at a time)
    ///or we can update the data on-the-fly with callbacks
    ///when the user clicks on the "Previous" or "Next" buttons.
    ///These modes can be toggled with this &#96;page_action&#96; parameter:
    ///&#96;'native'&#96;: all data is passed to the table up-front, paging logic is
    ///handled by the table;
    ///&#96;'custom'&#96;: data is passed to the table one page at a time, paging logic
    ///is handled via callbacks;
    ///&#96;'none'&#96;: disables paging, render all of the data at once.
    ///&#10;
    ///• page_current (number; default 0) - &#96;page_current&#96; represents which page the user is on.
    ///Use this property to index through data in your callbacks with
    ///backend paging.
    ///&#10;
    ///• page_count (number) - &#96;page_count&#96; represents the number of the pages in the
    ///paginated table. This is really only useful when performing
    ///backend pagination, since the front end is able to use the
    ///full size of the table to calculate the number of pages.
    ///&#10;
    ///• page_size (number; default 250) - &#96;page_size&#96; represents the number of rows that will be
    ///displayed on a particular page when &#96;page_action&#96; is &#96;'custom'&#96; or &#96;'native'&#96;
    ///&#10;
    ///• dropdown (dict with values of type: record with the fields: 'clearable: boolean (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'; default {}) - &#96;dropdown&#96; specifies dropdown options for different columns.
    ///Each entry refers to the column ID.
    ///The &#96;clearable&#96; property defines whether the value can be deleted.
    ///The &#96;options&#96; property refers to the &#96;options&#96; of the dropdown.
    ///&#10;
    ///• dropdown_conditional (list with values of type: record with the fields: 'clearable: boolean (optional)', 'if: record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)' (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'; default []) - &#96;dropdown_conditional&#96; specifies dropdown options in various columns and cells.
    ///This property allows you to specify different dropdowns
    ///depending on certain conditions. For example, you may
    ///render different "city" dropdowns in a row depending on the
    ///current value in the "state" column.
    ///&#10;
    ///• dropdown_data (list with values of type: dict with values of type: record with the fields: 'clearable: boolean (optional)', 'options: list with values of type: record with the fields: 'label: string (required)', 'value: number | string | boolean (required)' (required)'; default []) - &#96;dropdown_data&#96; specifies dropdown options on a row-by-row, column-by-column basis.
    ///Each item in the array corresponds to the corresponding dropdowns for the &#96;data&#96; item
    ///at the same index. Each entry in the item refers to the Column ID.
    ///&#10;
    ///• tooltip (dict with values of type: string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'use_with: value equal to: 'both', 'data', 'header' (optional)', 'value: string (required)'; default {}) - &#96;tooltip&#96; is the column based tooltip configuration applied to all rows. The key is the column
    /// id and the value is a tooltip configuration.
    ///Example: {i: {'value': i, 'use_with: 'both'} for i in df.columns}
    ///&#10;
    ///• tooltip_conditional (list with values of type: record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'if: record with the fields: 'column_id: string (optional)', 'filter_query: string (optional)', 'row_index: number | value equal to: 'odd', 'even' (optional)' (required)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'; default []) - &#96;tooltip_conditional&#96; represents the tooltip shown
    ///for different columns and cells.
    ///This property allows you to specify different tooltips
    ///depending on certain conditions. For example, you may have
    ///different tooltips in the same column based on the value
    ///of a certain data property.
    ///Priority is from first to last defined conditional tooltip
    ///in the list. Higher priority (more specific) conditional
    ///tooltips should be put at the beginning of the list.
    ///&#10;
    ///• tooltip_data (list with values of type: dict with values of type: string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'; default []) - &#96;tooltip_data&#96; represents the tooltip shown
    ///for different columns and cells.
    ///A list of dicts for which each key is
    ///a column id and the value is a tooltip configuration.
    ///&#10;
    ///• tooltip_header (dict with values of type: string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)' | list with values of type: value equal to: 'null' | string | record with the fields: 'delay: number (optional)', 'duration: number (optional)', 'type: value equal to: 'text', 'markdown' (optional)', 'value: string (required)'; default {}) - &#96;tooltip_header&#96; represents the tooltip shown
    ///for each header column and optionally each header row.
    ///Example to show long column names in a tooltip: {i: i for i in df.columns}.
    ///Example to show different column names in a tooltip: {'Rep': 'Republican', 'Dem': 'Democrat'}.
    ///If the table has multiple rows of headers, then use a list as the value of the
    ///&#96;tooltip_header&#96; items.
    ///&#10;
    ///• tooltip_delay (number; default 350) - &#96;tooltip_delay&#96; represents the table-wide delay in milliseconds before
    ///the tooltip is shown when hovering a cell. If set to &#96;None&#96;, the tooltip
    ///will be shown immediately.
    ///Defaults to 350.
    ///&#10;
    ///• tooltip_duration (number; default 2000) - &#96;tooltip_duration&#96; represents the table-wide duration in milliseconds
    ///during which the tooltip will be displayed when hovering a cell. If
    ///set to &#96;None&#96;, the tooltip will not disappear.
    ///Defaults to 2000.
    ///&#10;
    ///• filter_query (string; default ) - If &#96;filter_action&#96; is enabled, then the current filtering
    ///string is represented in this &#96;filter_query&#96;
    ///property.
    ///&#10;
    ///• filter_action (value equal to: 'custom', 'native', 'none' | record with the fields: 'type: value equal to: 'custom', 'native' (required)', 'operator: value equal to: 'and', 'or' (optional)'; default none) - The &#96;filter_action&#96; property controls the behavior of the &#96;filtering&#96; UI.
    ///If &#96;'none'&#96;, then the filtering UI is not displayed.
    ///If &#96;'native'&#96;, then the filtering UI is displayed and the filtering
    ///logic is handled by the table. That is, it is performed on the data
    ///that exists in the &#96;data&#96; property.
    ///If &#96;'custom'&#96;, then the filtering UI is displayed but it is the
    ///responsibility of the developer to program the filtering
    ///through a callback (where &#96;filter_query&#96; or &#96;derived_filter_query_structure&#96; would be the input
    ///and &#96;data&#96; would be the output).
    ///&#10;
    ///• filter_options (record with the field: 'case: value equal to: 'sensitive', 'insensitive' (optional)'; default {}) - There are two &#96;filter_options&#96; props in the table.
    ///This is the table-level filter_options prop and there is
    ///also the column-level &#96;filter_options&#96; prop.
    ///These props determine whether the applicable filter relational
    ///operators will default to &#96;sensitive&#96; or &#96;insensitive&#96; comparison.
    ///If the column-level &#96;filter_options&#96; prop is set it overrides
    ///the table-level &#96;filter_options&#96; prop for that column.
    ///&#10;
    ///• sort_action (value equal to: 'custom', 'native', 'none'; default none) - The &#96;sort_action&#96; property enables data to be
    ///sorted on a per-column basis.
    ///If &#96;'none'&#96;, then the sorting UI is not displayed.
    ///If &#96;'native'&#96;, then the sorting UI is displayed and the sorting
    ///logic is handled by the table. That is, it is performed on the data
    ///that exists in the &#96;data&#96; property.
    ///If &#96;'custom'&#96;, the the sorting UI is displayed but it is the
    ///responsibility of the developer to program the sorting
    ///through a callback (where &#96;sort_by&#96; would be the input and &#96;data&#96;
    ///would be the output).
    ///Clicking on the sort arrows will update the
    ///&#96;sort_by&#96; property.
    ///&#10;
    ///• sort_mode (value equal to: 'single', 'multi'; default single) - Sorting can be performed across multiple columns
    ///(e.g. sort by country, sort within each country,
    /// sort by year) or by a single column.
    ///NOTE - With multi-column sort, it's currently
    ///not possible to determine the order in which
    ///the columns were sorted through the UI.
    ///See [https://github.com/plotly/dash-table/issues/170](https://github.com/plotly/dash-table/issues/170)
    ///&#10;
    ///• sort_by (list with values of type: record with the fields: 'column_id: string (required)', 'direction: value equal to: 'asc', 'desc' (required)'; default []) - &#96;sort_by&#96; describes the current state
    ///of the sorting UI.
    ///That is, if the user clicked on the sort arrow
    ///of a column, then this property will be updated
    ///with the column ID and the direction
    ///(&#96;asc&#96; or &#96;desc&#96;) of the sort.
    ///For multi-column sorting, this will be a list of
    ///sorting parameters, in the order in which they were
    ///clicked.
    ///&#10;
    ///• sort_as_null (list with values of type: string | number | boolean; default []) - An array of string, number and boolean values that are treated as &#96;None&#96;
    ///(i.e. ignored and always displayed last) when sorting.
    ///This value will be used by columns without &#96;sort_as_null&#96;.
    ///Defaults to &#96;[]&#96;.
    ///&#10;
    ///• style_table (record; default {}) - CSS styles to be applied to the outer &#96;table&#96; container.
    ///This is commonly used for setting properties like the
    ///width or the height of the table.
    ///&#10;
    ///• style_cell (record) - CSS styles to be applied to each individual cell of the table.
    ///This includes the header cells, the &#96;data&#96; cells, and the filter
    ///cells.
    ///&#10;
    ///• style_data (record) - CSS styles to be applied to each individual data cell.
    ///That is, unlike &#96;style_cell&#96;, it excludes the header and filter cells.
    ///&#10;
    ///• style_filter (record) - CSS styles to be applied to the filter cells.
    ///Note that this may change in the future as we build out a
    ///more complex filtering UI.
    ///&#10;
    ///• style_header (record) - CSS styles to be applied to each individual header cell.
    ///That is, unlike &#96;style_cell&#96;, it excludes the &#96;data&#96; and filter cells.
    ///&#10;
    ///• style_cell_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)' (optional)'; default []) - Conditional CSS styles for the cells.
    ///This can be used to apply styles to cells on a per-column basis.
    ///&#10;
    ///• style_data_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'filter_query: string (optional)', 'state: value equal to: 'active', 'selected' (optional)', 'row_index: number | value equal to: 'odd', 'even' | list with values of type: number (optional)', 'column_editable: boolean (optional)' (optional)'; default []) - Conditional CSS styles for the data cells.
    ///This can be used to apply styles to data cells on a per-column basis.
    ///&#10;
    ///• style_filter_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'column_editable: boolean (optional)' (optional)'; default []) - Conditional CSS styles for the filter cells.
    ///This can be used to apply styles to filter cells on a per-column basis.
    ///&#10;
    ///• style_header_conditional (list with values of type: record with the field: 'if: record with the fields: 'column_id: string | list with values of type: string (optional)', 'column_type: value equal to: 'any', 'numeric', 'text', 'datetime' (optional)', 'header_index: number | list with values of type: number | value equal to: 'odd', 'even' (optional)', 'column_editable: boolean (optional)' (optional)'; default []) - Conditional CSS styles for the header cells.
    ///This can be used to apply styles to header cells on a per-column basis.
    ///&#10;
    ///• virtualization (boolean; default false) - This property tells the table to use virtualization when rendering.
    ///Assumptions are that:
    ///the width of the columns is fixed;
    ///the height of the rows is always the same; and
    ///runtime styling changes will not affect width and height vs. first rendering
    ///&#10;
    ///• derived_filter_query_structure (record) - This property represents the current structure of
    ///&#96;filter_query&#96; as a tree structure. Each node of the
    ///query structure has:
    ///type (string; required):
    ///  'open-block',
    ///  'logical-operator',
    ///  'relational-operator',
    ///  'unary-operator', or
    ///  'expression';
    ///subType (string; optional):
    ///  'open-block': '()',
    ///  'logical-operator': '&amp;&amp;', '||',
    ///  'relational-operator': '=', '&gt;=', '&gt;', '&lt;=', '&lt;', '!=', 'contains',
    ///  'unary-operator': '!', 'is bool', 'is even', 'is nil', 'is num', 'is object', 'is odd', 'is prime', 'is str',
    ///  'expression': 'value', 'field';
    ///value (any):
    ///  'expression, value': passed value,
    ///  'expression, field': the field/prop name.
    ///block (nested query structure; optional).
    ///left (nested query structure; optional).
    ///right (nested query structure; optional).
    ///If the query is invalid or empty, the &#96;derived_filter_query_structure&#96; will
    ///be &#96;None&#96;.
    ///&#10;
    ///• derived_viewport_data (list with values of type: record; default []) - This property represents the current state of &#96;data&#96;
    ///on the current page. This property will be updated
    ///on paging, sorting, and filtering.
    ///&#10;
    ///• derived_viewport_indices (list with values of type: number; default []) - &#96;derived_viewport_indices&#96; indicates the order in which the original
    ///rows appear after being filtered, sorted, and/or paged.
    ///&#96;derived_viewport_indices&#96; contains indices for the current page,
    ///while &#96;derived_virtual_indices&#96; contains indices across all pages.
    ///&#10;
    ///• derived_viewport_row_ids (list with values of type: string | number; default []) - &#96;derived_viewport_row_ids&#96; lists row IDs in the order they appear
    ///after being filtered, sorted, and/or paged.
    ///&#96;derived_viewport_row_ids&#96; contains IDs for the current page,
    ///while &#96;derived_virtual_row_ids&#96; contains IDs across all pages.
    ///&#10;
    ///• derived_viewport_selected_columns (list with values of type: string) - &#96;derived_viewport_selected_columns&#96; contains the ids of the
    ///&#96;selected_columns&#96; that are not currently hidden.
    ///&#10;
    ///• derived_viewport_selected_rows (list with values of type: number; default []) - &#96;derived_viewport_selected_rows&#96; represents the indices of the
    ///&#96;selected_rows&#96; from the perspective of the &#96;derived_viewport_indices&#96;.
    ///&#10;
    ///• derived_viewport_selected_row_ids (list with values of type: string | number; default []) - &#96;derived_viewport_selected_row_ids&#96; represents the IDs of the
    ///&#96;selected_rows&#96; on the currently visible page.
    ///&#10;
    ///• derived_virtual_data (list with values of type: record; default []) - This property represents the visible state of &#96;data&#96;
    ///across all pages after the front-end sorting and filtering
    ///as been applied.
    ///&#10;
    ///• derived_virtual_indices (list with values of type: number; default []) - &#96;derived_virtual_indices&#96; indicates the order in which the original
    ///rows appear after being filtered and sorted.
    ///&#96;derived_viewport_indices&#96; contains indices for the current page,
    ///while &#96;derived_virtual_indices&#96; contains indices across all pages.
    ///&#10;
    ///• derived_virtual_row_ids (list with values of type: string | number; default []) - &#96;derived_virtual_row_ids&#96; indicates the row IDs in the order in which
    ///they appear after being filtered and sorted.
    ///&#96;derived_viewport_row_ids&#96; contains IDs for the current page,
    ///while &#96;derived_virtual_row_ids&#96; contains IDs across all pages.
    ///&#10;
    ///• derived_virtual_selected_rows (list with values of type: number; default []) - &#96;derived_virtual_selected_rows&#96; represents the indices of the
    /// &#96;selected_rows&#96; from the perspective of the &#96;derived_virtual_indices&#96;.
    ///&#10;
    ///• derived_virtual_selected_row_ids (list with values of type: string | number; default []) - &#96;derived_virtual_selected_row_ids&#96; represents the IDs of the
    ///&#96;selected_rows&#96; as they appear after filtering and sorting,
    ///across all pages.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///&#10;
    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
    ///hasn't changed from its previous value, any &#96;persisted_props&#96; that the
    ///user has changed while using the app will keep those changes, as long as
    ///the new prop value also matches what was given originally.
    ///Used in conjunction with &#96;persistence_type&#96; and &#96;persisted_props&#96;.
    ///&#10;
    ///• persisted_props (list with values of type: value equal to: 'columns_name', 'data', 'filter_query', 'hidden_columns', 'selected_columns', 'selected_rows', 'sort_by'; default [
    ///    'columns_name',
    ///    'filter_query',
    ///    'hidden_columns',
    ///    'selected_columns',
    ///    'selected_rows',
    ///    'sort_by'
    ///]) - Properties whose user interactions will persist after refreshing the
    ///component or the page.
    ///&#10;
    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///</summary>
    let dataTable (id: string) (attrs: Attr list) =
        let props, children =
            attrs
            |> List.fold (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop prop -> prop :: props, children
                    | Children child -> props, child @ children
            ) ([], [])

        let dataTable = DataTable.init (id, children)

        let componentProps =
            match dataTable.TryGetTypedValue<DashComponentProps> "props" with
            | Option.Some p -> p
            | Option.None -> DashComponentProps()

        props
        |> Seq.iter (fun (prop: Prop) ->
            let fieldName, boxedProp = Prop.toDynamicMemberDef prop
            DynObj.setValue componentProps fieldName boxedProp
        )

        DynObj.setValue dataTable "props" componentProps
        dataTable :> DashComponent
