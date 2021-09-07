namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json

/////<summary>
/////DatePickerSingle is a tailor made component designed for selecting
/////a single day off of a calendar.
/////The DatePicker integrates well with the Python datetime module with the
/////startDate and endDate being returned in a string format suitable for
/////creating datetime objects.
/////This component is based off of Airbnb's react-dates react component
/////which can be found here: https://github.com/airbnb/react-dates
/////</summary>
//[<RequireQualifiedAccess>]
//module DatePickerSingle =
//    ///<summary>
//    ///value equal to: 'local', 'session', 'memory'
//    ///</summary>
//    type PersistenceTypeType =
//        | Local
//        | Session
//        | Memory
//        static member convert(this) =
//            box (
//                match this with
//                | Local -> "local"
//                | Session -> "session"
//                | Memory -> "memory"
//            )

//    ///<summary>
//    ///value equal to: 'date'
//    ///</summary>
//    type PersistedPropsTypeType =
//        | Date
//        static member convert(this) =
//            box (
//                match this with
//                | Date -> "date"
//            )

//    ///<summary>
//    ///list with values of type: value equal to: 'date'
//    ///</summary>
//    type PersistedPropsType =
//        | PersistedPropsType of list<PersistedPropsTypeType>
//        static member convert(this) =
//            box (
//                match this with
//                | PersistedPropsType (v) ->
//                    List.map (fun (i: PersistedPropsTypeType) -> box (i |> PersistedPropsTypeType.convert)) v
//            )

//    ///<summary>
//    ///boolean | string | number
//    ///</summary>
//    type PersistenceType =
//        | Bool of bool
//        | String of string
//        | IConvertible of IConvertible
//        static member convert(this) =
//            match this with
//            | Bool (v) -> box v
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)'
//    ///</summary>
//    type LoadingStateType =
//        { IsLoading: bool
//          PropName: string
//          ComponentName: string }
//        static member convert(this) =
//            box
//                {| is_loading = this.IsLoading
//                   prop_name = this.PropName
//                   component_name = this.ComponentName |}

//    ///<summary>
//    ///value equal to: '0', '1', '2', '3', '4', '5', '6'
//    ///</summary>
//    type FirstDayOfWeekType =
//        | Prop0
//        | Prop1
//        | Prop2
//        | Prop3
//        | Prop4
//        | Prop5
//        | Prop6
//        static member convert(this) =
//            box (
//                match this with
//                | Prop0 -> "0"
//                | Prop1 -> "1"
//                | Prop2 -> "2"
//                | Prop3 -> "3"
//                | Prop4 -> "4"
//                | Prop5 -> "5"
//                | Prop6 -> "6"
//            )

//    ///<summary>
//    ///value equal to: 'vertical', 'horizontal'
//    ///</summary>
//    type CalendarOrientationType =
//        | Vertical
//        | Horizontal
//        static member convert(this) =
//            box (
//                match this with
//                | Vertical -> "vertical"
//                | Horizontal -> "horizontal"
//            )

//    ///<summary>
//    ///• date (string) - Specifies the starting date for the component, best practice is to pass
//    ///value via datetime object
//    ///&#10;
//    ///• min_date_allowed (string) - Specifies the lowest selectable date for the component.
//    ///Accepts datetime.datetime objects or strings
//    ///in the format 'YYYY-MM-DD'
//    ///&#10;
//    ///• max_date_allowed (string) - Specifies the highest selectable date for the component.
//    ///Accepts datetime.datetime objects or strings
//    ///in the format 'YYYY-MM-DD'
//    ///&#10;
//    ///• initial_visible_month (string) - Specifies the month that is initially presented when the user
//    ///opens the calendar. Accepts datetime.datetime objects or strings
//    ///in the format 'YYYY-MM-DD'
//    ///&#10;
//    ///• day_size (number; default 39) - Size of rendered calendar days, higher number
//    ///means bigger day size and larger calendar overall
//    ///&#10;
//    ///• calendar_orientation (value equal to: 'vertical', 'horizontal'; default horizontal) - Orientation of calendar, either vertical or horizontal.
//    ///Valid options are 'vertical' or 'horizontal'.
//    ///&#10;
//    ///• is_RTL (boolean; default false) - Determines whether the calendar and days operate
//    ///from left to right or from right to left
//    ///&#10;
//    ///• placeholder (string) - Text that will be displayed in the input
//    ///box of the date picker when no date is selected.
//    ///Default value is 'Start Date'
//    ///&#10;
//    ///• reopen_calendar_on_clear (boolean; default false) - If True, the calendar will automatically open when cleared
//    ///&#10;
//    ///• number_of_months_shown (number; default 1) - Number of calendar months that are shown when calendar is opened
//    ///&#10;
//    ///• with_portal (boolean; default false) - If True, calendar will open in a screen overlay portal,
//    ///not supported on vertical calendar
//    ///&#10;
//    ///• with_full_screen_portal (boolean; default false) - If True, calendar will open in a full screen overlay portal, will
//    ///take precedent over 'withPortal' if both are set to True,
//    ///not supported on vertical calendar
//    ///&#10;
//    ///• first_day_of_week (value equal to: '0', '1', '2', '3', '4', '5', '6'; default 0) - Specifies what day is the first day of the week, values must be
//    ///from [0, ..., 6] with 0 denoting Sunday and 6 denoting Saturday
//    ///&#10;
//    ///• stay_open_on_select (boolean; default false) - If True the calendar will not close when the user has selected a value
//    ///and will wait until the user clicks off the calendar
//    ///&#10;
//    ///• show_outside_days (boolean; default true) - If True the calendar will display days that rollover into
//    ///the next month
//    ///&#10;
//    ///• month_format (string) - Specifies the format that the month will be displayed in the calendar,
//    ///valid formats are variations of "MM YY". For example:
//    ///"MM YY" renders as '05 97' for May 1997
//    ///"MMMM, YYYY" renders as 'May, 1997' for May 1997
//    ///"MMM, YY" renders as 'Sep, 97' for September 1997
//    ///&#10;
//    ///• display_format (string) - Specifies the format that the selected dates will be displayed
//    ///valid formats are variations of "MM YY DD". For example:
//    ///"MM YY DD" renders as '05 10 97' for May 10th 1997
//    ///"MMMM, YY" renders as 'May, 1997' for May 10th 1997
//    ///"M, D, YYYY" renders as '07, 10, 1997' for September 10th 1997
//    ///"MMMM" renders as 'May' for May 10 1997
//    ///&#10;
//    ///• disabled (boolean; default false) - If True, no dates can be selected.
//    ///&#10;
//    ///• clearable (boolean; default false) - Whether or not the dropdown is "clearable", that is, whether or
//    ///not a small "x" appears on the right of the dropdown that removes
//    ///the selected value.
//    ///&#10;
//    ///• style (record) - CSS styles appended to wrapper div
//    ///&#10;
//    ///• className (string) - Appends a CSS class to the wrapper div component.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///&#10;
//    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
//    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//    ///hasn't changed from its previous value, a &#96;date&#96; that the user has
//    ///changed while using the app will keep that change, as long as
//    ///the new &#96;date&#96; also matches what was given originally.
//    ///Used in conjunction with &#96;persistence_type&#96;.
//    ///&#10;
//    ///• persisted_props (list with values of type: value equal to: 'date'; default ['date']) - Properties whose user interactions will persist after refreshing the
//    ///component or the page. Since only &#96;date&#96; is allowed this prop can
//    ///normally be ignored.
//    ///&#10;
//    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
//    ///memory: only kept in memory, reset on page refresh.
//    ///local: window.localStorage, data is kept after the browser quit.
//    ///session: window.sessionStorage, data is cleared once the browser quit.
//    ///</summary>
//    type Prop =
//        | Date of string
//        | MinDateAllowed of string
//        | MaxDateAllowed of string
//        | InitialVisibleMonth of string
//        | DaySize of IConvertible
//        | CalendarOrientation of CalendarOrientationType
//        | IsRTL of bool
//        | Placeholder of string
//        | ReopenCalendarOnClear of bool
//        | NumberOfMonthsShown of IConvertible
//        | WithPortal of bool
//        | WithFullScreenPortal of bool
//        | FirstDayOfWeek of FirstDayOfWeekType
//        | StayOpenOnSelect of bool
//        | ShowOutsideDays of bool
//        | MonthFormat of string
//        | DisplayFormat of string
//        | Disabled of bool
//        | Clearable of bool
//        | Style of obj
//        | ClassName of string
//        | LoadingState of LoadingStateType
//        | Persistence of PersistenceType
//        | PersistedProps of PersistedPropsType
//        | PersistenceType of PersistenceTypeType
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Date (p) -> "date", box p
//            | MinDateAllowed (p) -> "min_date_allowed", box p
//            | MaxDateAllowed (p) -> "max_date_allowed", box p
//            | InitialVisibleMonth (p) -> "initial_visible_month", box p
//            | DaySize (p) -> "day_size", box p
//            | CalendarOrientation (p) -> "calendar_orientation", p |> CalendarOrientationType.convert
//            | IsRTL (p) -> "is_RTL", box p
//            | Placeholder (p) -> "placeholder", box p
//            | ReopenCalendarOnClear (p) -> "reopen_calendar_on_clear", box p
//            | NumberOfMonthsShown (p) -> "number_of_months_shown", box p
//            | WithPortal (p) -> "with_portal", box p
//            | WithFullScreenPortal (p) -> "with_full_screen_portal", box p
//            | FirstDayOfWeek (p) -> "first_day_of_week", p |> FirstDayOfWeekType.convert
//            | StayOpenOnSelect (p) -> "stay_open_on_select", box p
//            | ShowOutsideDays (p) -> "show_outside_days", box p
//            | MonthFormat (p) -> "month_format", box p
//            | DisplayFormat (p) -> "display_format", box p
//            | Disabled (p) -> "disabled", box p
//            | Clearable (p) -> "clearable", box p
//            | Style (p) -> "style", box p
//            | ClassName (p) -> "className", box p
//            | LoadingState (p) -> "loading_state", p |> LoadingStateType.convert
//            | Persistence (p) -> "persistence", p |> PersistenceType.convert
//            | PersistedProps (p) -> "persisted_props", p |> PersistedPropsType.convert
//            | PersistenceType (p) -> "persistence_type", p |> PersistenceTypeType.convert

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///Specifies the starting date for the component, best practice is to pass
//        ///value via datetime object
//        ///</summary>
//        static member date(p: string) = Prop(Date p)
//        ///<summary>
//        ///Specifies the lowest selectable date for the component.
//        ///Accepts datetime.datetime objects or strings
//        ///in the format 'YYYY-MM-DD'
//        ///</summary>
//        static member minDateAllowed(p: string) = Prop(MinDateAllowed p)
//        ///<summary>
//        ///Specifies the highest selectable date for the component.
//        ///Accepts datetime.datetime objects or strings
//        ///in the format 'YYYY-MM-DD'
//        ///</summary>
//        static member maxDateAllowed(p: string) = Prop(MaxDateAllowed p)
//        ///<summary>
//        ///Specifies the month that is initially presented when the user
//        ///opens the calendar. Accepts datetime.datetime objects or strings
//        ///in the format 'YYYY-MM-DD'
//        ///</summary>
//        static member initialVisibleMonth(p: string) = Prop(InitialVisibleMonth p)
//        ///<summary>
//        ///Size of rendered calendar days, higher number
//        ///means bigger day size and larger calendar overall
//        ///</summary>
//        static member daySize(p: IConvertible) = Prop(DaySize p)
//        ///<summary>
//        ///Orientation of calendar, either vertical or horizontal.
//        ///Valid options are 'vertical' or 'horizontal'.
//        ///</summary>
//        static member calendarOrientation(p: CalendarOrientationType) = Prop(CalendarOrientation p)
//        ///<summary>
//        ///Determines whether the calendar and days operate
//        ///from left to right or from right to left
//        ///</summary>
//        static member isRTL(p: bool) = Prop(IsRTL p)
//        ///<summary>
//        ///Text that will be displayed in the input
//        ///box of the date picker when no date is selected.
//        ///Default value is 'Start Date'
//        ///</summary>
//        static member placeholder(p: string) = Prop(Placeholder p)
//        ///<summary>
//        ///If True, the calendar will automatically open when cleared
//        ///</summary>
//        static member reopenCalendarOnClear(p: bool) = Prop(ReopenCalendarOnClear p)
//        ///<summary>
//        ///Number of calendar months that are shown when calendar is opened
//        ///</summary>
//        static member numberOfMonthsShown(p: IConvertible) = Prop(NumberOfMonthsShown p)
//        ///<summary>
//        ///If True, calendar will open in a screen overlay portal,
//        ///not supported on vertical calendar
//        ///</summary>
//        static member withPortal(p: bool) = Prop(WithPortal p)
//        ///<summary>
//        ///If True, calendar will open in a full screen overlay portal, will
//        ///take precedent over 'withPortal' if both are set to True,
//        ///not supported on vertical calendar
//        ///</summary>
//        static member withFullScreenPortal(p: bool) = Prop(WithFullScreenPortal p)
//        ///<summary>
//        ///Specifies what day is the first day of the week, values must be
//        ///from [0, ..., 6] with 0 denoting Sunday and 6 denoting Saturday
//        ///</summary>
//        static member firstDayOfWeek(p: FirstDayOfWeekType) = Prop(FirstDayOfWeek p)
//        ///<summary>
//        ///If True the calendar will not close when the user has selected a value
//        ///and will wait until the user clicks off the calendar
//        ///</summary>
//        static member stayOpenOnSelect(p: bool) = Prop(StayOpenOnSelect p)
//        ///<summary>
//        ///If True the calendar will display days that rollover into
//        ///the next month
//        ///</summary>
//        static member showOutsideDays(p: bool) = Prop(ShowOutsideDays p)
//        ///<summary>
//        ///Specifies the format that the month will be displayed in the calendar,
//        ///valid formats are variations of "MM YY". For example:
//        ///"MM YY" renders as '05 97' for May 1997
//        ///"MMMM, YYYY" renders as 'May, 1997' for May 1997
//        ///"MMM, YY" renders as 'Sep, 97' for September 1997
//        ///</summary>
//        static member monthFormat(p: string) = Prop(MonthFormat p)
//        ///<summary>
//        ///Specifies the format that the selected dates will be displayed
//        ///valid formats are variations of "MM YY DD". For example:
//        ///"MM YY DD" renders as '05 10 97' for May 10th 1997
//        ///"MMMM, YY" renders as 'May, 1997' for May 10th 1997
//        ///"M, D, YYYY" renders as '07, 10, 1997' for September 10th 1997
//        ///"MMMM" renders as 'May' for May 10 1997
//        ///</summary>
//        static member displayFormat(p: string) = Prop(DisplayFormat p)
//        ///<summary>
//        ///If True, no dates can be selected.
//        ///</summary>
//        static member disabled(p: bool) = Prop(Disabled p)
//        ///<summary>
//        ///Whether or not the dropdown is "clearable", that is, whether or
//        ///not a small "x" appears on the right of the dropdown that removes
//        ///the selected value.
//        ///</summary>
//        static member clearable(p: bool) = Prop(Clearable p)
//        ///<summary>
//        ///CSS styles appended to wrapper div
//        ///</summary>
//        static member style(p: obj) = Prop(Style p)
//        ///<summary>
//        ///Appends a CSS class to the wrapper div component.
//        ///</summary>
//        static member className(p: string) = Prop(ClassName p)
//        ///<summary>
//        ///Object that holds the loading state object coming from dash-renderer
//        ///</summary>
//        static member loadingState(p: LoadingStateType) = Prop(LoadingState p)
//        ///<summary>
//        ///Used to allow user interactions in this component to be persisted when
//        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//        ///hasn't changed from its previous value, a &#96;date&#96; that the user has
//        ///changed while using the app will keep that change, as long as
//        ///the new &#96;date&#96; also matches what was given originally.
//        ///Used in conjunction with &#96;persistence_type&#96;.
//        ///</summary>
//        static member persistence(p: bool) =
//            Prop(Persistence(PersistenceType.Bool p))

//        ///<summary>
//        ///Used to allow user interactions in this component to be persisted when
//        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//        ///hasn't changed from its previous value, a &#96;date&#96; that the user has
//        ///changed while using the app will keep that change, as long as
//        ///the new &#96;date&#96; also matches what was given originally.
//        ///Used in conjunction with &#96;persistence_type&#96;.
//        ///</summary>
//        static member persistence(p: string) =
//            Prop(Persistence(PersistenceType.String p))

//        ///<summary>
//        ///Used to allow user interactions in this component to be persisted when
//        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//        ///hasn't changed from its previous value, a &#96;date&#96; that the user has
//        ///changed while using the app will keep that change, as long as
//        ///the new &#96;date&#96; also matches what was given originally.
//        ///Used in conjunction with &#96;persistence_type&#96;.
//        ///</summary>
//        static member persistence(p: IConvertible) =
//            Prop(Persistence(PersistenceType.IConvertible p))

//        ///<summary>
//        ///Properties whose user interactions will persist after refreshing the
//        ///component or the page. Since only &#96;date&#96; is allowed this prop can
//        ///normally be ignored.
//        ///</summary>
//        static member persistedProps(p: PersistedPropsType) = Prop(PersistedProps p)
//        ///<summary>
//        ///Where persisted user changes will be stored:
//        ///memory: only kept in memory, reset on page refresh.
//        ///local: window.localStorage, data is kept after the browser quit.
//        ///session: window.sessionStorage, data is cleared once the browser quit.
//        ///</summary>
//        static member persistenceType(p: PersistenceTypeType) = Prop(PersistenceType p)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: int) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: string) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: float) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: System.Guid) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: DashComponent) = Children([ value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: list<DashComponent>) = Children(value)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

//    ///<summary>
//    ///DatePickerSingle is a tailor made component designed for selecting
//    ///a single day off of a calendar.
//    ///The DatePicker integrates well with the Python datetime module with the
//    ///startDate and endDate being returned in a string format suitable for
//    ///creating datetime objects.
//    ///This component is based off of Airbnb's react-dates react component
//    ///which can be found here: https://github.com/airbnb/react-dates
//    ///</summary>
//    type DatePickerSingle() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?date: string,
//                ?minDateAllowed: string,
//                ?maxDateAllowed: string,
//                ?initialVisibleMonth: string,
//                ?daySize: IConvertible,
//                ?calendarOrientation: CalendarOrientationType,
//                ?isRTL: bool,
//                ?placeholder: string,
//                ?reopenCalendarOnClear: bool,
//                ?numberOfMonthsShown: IConvertible,
//                ?withPortal: bool,
//                ?withFullScreenPortal: bool,
//                ?firstDayOfWeek: FirstDayOfWeekType,
//                ?stayOpenOnSelect: bool,
//                ?showOutsideDays: bool,
//                ?monthFormat: string,
//                ?displayFormat: string,
//                ?disabled: bool,
//                ?clearable: bool,
//                ?style: obj,
//                ?className: string,
//                ?loadingState: LoadingStateType,
//                ?persistence: PersistenceType,
//                ?persistedProps: PersistedPropsType,
//                ?persistenceType: PersistenceTypeType
//            ) =
//            (fun (t: DatePickerSingle) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "date" (date |> Option.map box)
//                DynObj.setValueOpt props "minDateAllowed" (minDateAllowed |> Option.map box)
//                DynObj.setValueOpt props "maxDateAllowed" (maxDateAllowed |> Option.map box)
//                DynObj.setValueOpt props "initialVisibleMonth" (initialVisibleMonth |> Option.map box)
//                DynObj.setValueOpt props "daySize" (daySize |> Option.map box)
//                DynObj.setValueOpt
//                    props
//                    "calendarOrientation"
//                    (calendarOrientation |> Option.map CalendarOrientationType.convert)

//                DynObj.setValueOpt props "isRTL" (isRTL |> Option.map box)
//                DynObj.setValueOpt props "placeholder" (placeholder |> Option.map box)
//                DynObj.setValueOpt props "reopenCalendarOnClear" (reopenCalendarOnClear |> Option.map box)
//                DynObj.setValueOpt props "numberOfMonthsShown" (numberOfMonthsShown |> Option.map box)
//                DynObj.setValueOpt props "withPortal" (withPortal |> Option.map box)
//                DynObj.setValueOpt props "withFullScreenPortal" (withFullScreenPortal |> Option.map box)
//                DynObj.setValueOpt props "firstDayOfWeek" (firstDayOfWeek |> Option.map FirstDayOfWeekType.convert)
//                DynObj.setValueOpt props "stayOpenOnSelect" (stayOpenOnSelect |> Option.map box)
//                DynObj.setValueOpt props "showOutsideDays" (showOutsideDays |> Option.map box)
//                DynObj.setValueOpt props "monthFormat" (monthFormat |> Option.map box)
//                DynObj.setValueOpt props "displayFormat" (displayFormat |> Option.map box)
//                DynObj.setValueOpt props "disabled" (disabled |> Option.map box)
//                DynObj.setValueOpt props "clearable" (clearable |> Option.map box)
//                DynObj.setValueOpt props "style" (style |> Option.map box)
//                DynObj.setValueOpt props "className" (className |> Option.map box)
//                DynObj.setValueOpt props "loadingState" (loadingState |> Option.map LoadingStateType.convert)
//                DynObj.setValueOpt props "persistence" (persistence |> Option.map PersistenceType.convert)
//                DynObj.setValueOpt props "persistedProps" (persistedProps |> Option.map PersistedPropsType.convert)
//                DynObj.setValueOpt props "persistenceType" (persistenceType |> Option.map PersistenceTypeType.convert)
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "DatePickerSingle"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?date: string,
//                ?minDateAllowed: string,
//                ?maxDateAllowed: string,
//                ?initialVisibleMonth: string,
//                ?daySize: IConvertible,
//                ?calendarOrientation: CalendarOrientationType,
//                ?isRTL: bool,
//                ?placeholder: string,
//                ?reopenCalendarOnClear: bool,
//                ?numberOfMonthsShown: IConvertible,
//                ?withPortal: bool,
//                ?withFullScreenPortal: bool,
//                ?firstDayOfWeek: FirstDayOfWeekType,
//                ?stayOpenOnSelect: bool,
//                ?showOutsideDays: bool,
//                ?monthFormat: string,
//                ?displayFormat: string,
//                ?disabled: bool,
//                ?clearable: bool,
//                ?style: obj,
//                ?className: string,
//                ?loadingState: LoadingStateType,
//                ?persistence: PersistenceType,
//                ?persistedProps: PersistedPropsType,
//                ?persistenceType: PersistenceTypeType
//            ) =
//            DatePickerSingle.applyMembers
//                (id,
//                 children,
//                 ?date = date,
//                 ?minDateAllowed = minDateAllowed,
//                 ?maxDateAllowed = maxDateAllowed,
//                 ?initialVisibleMonth = initialVisibleMonth,
//                 ?daySize = daySize,
//                 ?calendarOrientation = calendarOrientation,
//                 ?isRTL = isRTL,
//                 ?placeholder = placeholder,
//                 ?reopenCalendarOnClear = reopenCalendarOnClear,
//                 ?numberOfMonthsShown = numberOfMonthsShown,
//                 ?withPortal = withPortal,
//                 ?withFullScreenPortal = withFullScreenPortal,
//                 ?firstDayOfWeek = firstDayOfWeek,
//                 ?stayOpenOnSelect = stayOpenOnSelect,
//                 ?showOutsideDays = showOutsideDays,
//                 ?monthFormat = monthFormat,
//                 ?displayFormat = displayFormat,
//                 ?disabled = disabled,
//                 ?clearable = clearable,
//                 ?style = style,
//                 ?className = className,
//                 ?loadingState = loadingState,
//                 ?persistence = persistence,
//                 ?persistedProps = persistedProps,
//                 ?persistenceType = persistenceType)
//                (DatePickerSingle())

//    ///<summary>
//    ///DatePickerSingle is a tailor made component designed for selecting
//    ///a single day off of a calendar.
//    ///The DatePicker integrates well with the Python datetime module with the
//    ///startDate and endDate being returned in a string format suitable for
//    ///creating datetime objects.
//    ///This component is based off of Airbnb's react-dates react component
//    ///which can be found here: https://github.com/airbnb/react-dates
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• date (string) - Specifies the starting date for the component, best practice is to pass
//    ///value via datetime object
//    ///&#10;
//    ///• min_date_allowed (string) - Specifies the lowest selectable date for the component.
//    ///Accepts datetime.datetime objects or strings
//    ///in the format 'YYYY-MM-DD'
//    ///&#10;
//    ///• max_date_allowed (string) - Specifies the highest selectable date for the component.
//    ///Accepts datetime.datetime objects or strings
//    ///in the format 'YYYY-MM-DD'
//    ///&#10;
//    ///• initial_visible_month (string) - Specifies the month that is initially presented when the user
//    ///opens the calendar. Accepts datetime.datetime objects or strings
//    ///in the format 'YYYY-MM-DD'
//    ///&#10;
//    ///• day_size (number; default 39) - Size of rendered calendar days, higher number
//    ///means bigger day size and larger calendar overall
//    ///&#10;
//    ///• calendar_orientation (value equal to: 'vertical', 'horizontal'; default horizontal) - Orientation of calendar, either vertical or horizontal.
//    ///Valid options are 'vertical' or 'horizontal'.
//    ///&#10;
//    ///• is_RTL (boolean; default false) - Determines whether the calendar and days operate
//    ///from left to right or from right to left
//    ///&#10;
//    ///• placeholder (string) - Text that will be displayed in the input
//    ///box of the date picker when no date is selected.
//    ///Default value is 'Start Date'
//    ///&#10;
//    ///• reopen_calendar_on_clear (boolean; default false) - If True, the calendar will automatically open when cleared
//    ///&#10;
//    ///• number_of_months_shown (number; default 1) - Number of calendar months that are shown when calendar is opened
//    ///&#10;
//    ///• with_portal (boolean; default false) - If True, calendar will open in a screen overlay portal,
//    ///not supported on vertical calendar
//    ///&#10;
//    ///• with_full_screen_portal (boolean; default false) - If True, calendar will open in a full screen overlay portal, will
//    ///take precedent over 'withPortal' if both are set to True,
//    ///not supported on vertical calendar
//    ///&#10;
//    ///• first_day_of_week (value equal to: '0', '1', '2', '3', '4', '5', '6'; default 0) - Specifies what day is the first day of the week, values must be
//    ///from [0, ..., 6] with 0 denoting Sunday and 6 denoting Saturday
//    ///&#10;
//    ///• stay_open_on_select (boolean; default false) - If True the calendar will not close when the user has selected a value
//    ///and will wait until the user clicks off the calendar
//    ///&#10;
//    ///• show_outside_days (boolean; default true) - If True the calendar will display days that rollover into
//    ///the next month
//    ///&#10;
//    ///• month_format (string) - Specifies the format that the month will be displayed in the calendar,
//    ///valid formats are variations of "MM YY". For example:
//    ///"MM YY" renders as '05 97' for May 1997
//    ///"MMMM, YYYY" renders as 'May, 1997' for May 1997
//    ///"MMM, YY" renders as 'Sep, 97' for September 1997
//    ///&#10;
//    ///• display_format (string) - Specifies the format that the selected dates will be displayed
//    ///valid formats are variations of "MM YY DD". For example:
//    ///"MM YY DD" renders as '05 10 97' for May 10th 1997
//    ///"MMMM, YY" renders as 'May, 1997' for May 10th 1997
//    ///"M, D, YYYY" renders as '07, 10, 1997' for September 10th 1997
//    ///"MMMM" renders as 'May' for May 10 1997
//    ///&#10;
//    ///• disabled (boolean; default false) - If True, no dates can be selected.
//    ///&#10;
//    ///• clearable (boolean; default false) - Whether or not the dropdown is "clearable", that is, whether or
//    ///not a small "x" appears on the right of the dropdown that removes
//    ///the selected value.
//    ///&#10;
//    ///• style (record) - CSS styles appended to wrapper div
//    ///&#10;
//    ///• className (string) - Appends a CSS class to the wrapper div component.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///&#10;
//    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
//    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//    ///hasn't changed from its previous value, a &#96;date&#96; that the user has
//    ///changed while using the app will keep that change, as long as
//    ///the new &#96;date&#96; also matches what was given originally.
//    ///Used in conjunction with &#96;persistence_type&#96;.
//    ///&#10;
//    ///• persisted_props (list with values of type: value equal to: 'date'; default ['date']) - Properties whose user interactions will persist after refreshing the
//    ///component or the page. Since only &#96;date&#96; is allowed this prop can
//    ///normally be ignored.
//    ///&#10;
//    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
//    ///memory: only kept in memory, reset on page refresh.
//    ///local: window.localStorage, data is kept after the browser quit.
//    ///session: window.sessionStorage, data is cleared once the browser quit.
//    ///</summary>
//    let datePickerSingle (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = DatePickerSingle.init (id, children)

//        let componentProps =
//            match t.TryGetTypedValue<DashComponentProps> "props" with
//            | Some (p) -> p
//            | None -> DashComponentProps()

//        Seq.iter
//            (fun (prop: Prop) ->
//                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
//                DynObj.setValue componentProps fieldName boxedProp)
//            props

//        DynObj.setValue t "props" componentProps
//        t :> DashComponent
