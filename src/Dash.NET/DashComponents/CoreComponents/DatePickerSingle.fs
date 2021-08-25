namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

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
//        member this.Convert() =
//            match this with
//            | Local -> "local"
//            | Session -> "session"
//            | Memory -> "memory"

//    ///<summary>
//    ///value equal to: 'date'
//    ///</summary>
//    type PersistedPropsTypeType =
//        | Date
//        member this.Convert() =
//            match this with
//            | Date -> "date"

//    ///<summary>
//    ///list with values of type: value equal to: 'date'
//    ///</summary>
//    type PersistedPropsType =
//        | PersistedPropsType of list<PersistedPropsTypeType>
//        member this.Convert() =
//            match this with
//            | PersistedPropsType (v) -> List.map (fun (i: PersistedPropsTypeType) -> box (i.Convert())) v

//    ///<summary>
//    ///boolean | string | number
//    ///</summary>
//    type PersistenceType =
//        | Bool of bool
//        | String of string
//        | IConvertible of IConvertible
//        member this.Convert() =
//            match this with
//            | Bool (v) -> box v
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)'
//    ///</summary>
//    type LoadingStateType =
//        { IsLoading: Option<bool>
//          PropName: Option<string>
//          ComponentName: Option<string> }
//        member this.Convert() =
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
//        member this.Convert() =
//            match this with
//            | Prop0 -> "0"
//            | Prop1 -> "1"
//            | Prop2 -> "2"
//            | Prop3 -> "3"
//            | Prop4 -> "4"
//            | Prop5 -> "5"
//            | Prop6 -> "6"

//    ///<summary>
//    ///value equal to: 'vertical', 'horizontal'
//    ///</summary>
//    type CalendarOrientationType =
//        | Vertical
//        | Horizontal
//        member this.Convert() =
//            match this with
//            | Vertical -> "vertical"
//            | Horizontal -> "horizontal"

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
//            | CalendarOrientation (p) -> "calendar_orientation", box (p.Convert())
//            | IsRTL (p) -> "is_RTL", box p
//            | Placeholder (p) -> "placeholder", box p
//            | ReopenCalendarOnClear (p) -> "reopen_calendar_on_clear", box p
//            | NumberOfMonthsShown (p) -> "number_of_months_shown", box p
//            | WithPortal (p) -> "with_portal", box p
//            | WithFullScreenPortal (p) -> "with_full_screen_portal", box p
//            | FirstDayOfWeek (p) -> "first_day_of_week", box (p.Convert())
//            | StayOpenOnSelect (p) -> "stay_open_on_select", box p
//            | ShowOutsideDays (p) -> "show_outside_days", box p
//            | MonthFormat (p) -> "month_format", box p
//            | DisplayFormat (p) -> "display_format", box p
//            | Disabled (p) -> "disabled", box p
//            | Clearable (p) -> "clearable", box p
//            | Style (p) -> "style", box p
//            | ClassName (p) -> "className", box p
//            | LoadingState (p) -> "loading_state", box (p.Convert())
//            | Persistence (p) -> "persistence", box (p.Convert())
//            | PersistedProps (p) -> "persisted_props", box (p.Convert())
//            | PersistenceType (p) -> "persistence_type", box (p.Convert())

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
//        static member children(value: int) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: string) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: float) = Children([ Html.Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: System.Guid) = Children([ Html.Html.text value ])
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
//                ?min_date_allowed: string,
//                ?max_date_allowed: string,
//                ?initial_visible_month: string,
//                ?day_size: string,
//                ?calendar_orientation: string,
//                ?is_RTL: string,
//                ?placeholder: string,
//                ?reopen_calendar_on_clear: string,
//                ?number_of_months_shown: string,
//                ?with_portal: string,
//                ?with_full_screen_portal: string,
//                ?first_day_of_week: string,
//                ?stay_open_on_select: string,
//                ?show_outside_days: string,
//                ?month_format: string,
//                ?display_format: string,
//                ?disabled: string,
//                ?clearable: string,
//                ?style: string,
//                ?className: string,
//                ?loading_state: string,
//                ?persistence: string,
//                ?persisted_props: string,
//                ?persistence_type: string
//            ) =
//            (fun (t: DatePickerSingle) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "date" date
//                DynObj.setValueOpt props "min_date_allowed" min_date_allowed
//                DynObj.setValueOpt props "max_date_allowed" max_date_allowed
//                DynObj.setValueOpt props "initial_visible_month" initial_visible_month
//                DynObj.setValueOpt props "day_size" day_size
//                DynObj.setValueOpt props "calendar_orientation" calendar_orientation
//                DynObj.setValueOpt props "is_RTL" is_RTL
//                DynObj.setValueOpt props "placeholder" placeholder
//                DynObj.setValueOpt props "reopen_calendar_on_clear" reopen_calendar_on_clear
//                DynObj.setValueOpt props "number_of_months_shown" number_of_months_shown
//                DynObj.setValueOpt props "with_portal" with_portal
//                DynObj.setValueOpt props "with_full_screen_portal" with_full_screen_portal
//                DynObj.setValueOpt props "first_day_of_week" first_day_of_week
//                DynObj.setValueOpt props "stay_open_on_select" stay_open_on_select
//                DynObj.setValueOpt props "show_outside_days" show_outside_days
//                DynObj.setValueOpt props "month_format" month_format
//                DynObj.setValueOpt props "display_format" display_format
//                DynObj.setValueOpt props "disabled" disabled
//                DynObj.setValueOpt props "clearable" clearable
//                DynObj.setValueOpt props "style" style
//                DynObj.setValueOpt props "className" className
//                DynObj.setValueOpt props "loading_state" loading_state
//                DynObj.setValueOpt props "persistence" persistence
//                DynObj.setValueOpt props "persisted_props" persisted_props
//                DynObj.setValueOpt props "persistence_type" persistence_type
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "DatePickerSingle"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?date: string,
//                ?min_date_allowed: string,
//                ?max_date_allowed: string,
//                ?initial_visible_month: string,
//                ?day_size: string,
//                ?calendar_orientation: string,
//                ?is_RTL: string,
//                ?placeholder: string,
//                ?reopen_calendar_on_clear: string,
//                ?number_of_months_shown: string,
//                ?with_portal: string,
//                ?with_full_screen_portal: string,
//                ?first_day_of_week: string,
//                ?stay_open_on_select: string,
//                ?show_outside_days: string,
//                ?month_format: string,
//                ?display_format: string,
//                ?disabled: string,
//                ?clearable: string,
//                ?style: string,
//                ?className: string,
//                ?loading_state: string,
//                ?persistence: string,
//                ?persisted_props: string,
//                ?persistence_type: string
//            ) =
//            DatePickerSingle.applyMembers
//                (id,
//                 children,
//                 ?date = date,
//                 ?min_date_allowed = min_date_allowed,
//                 ?max_date_allowed = max_date_allowed,
//                 ?initial_visible_month = initial_visible_month,
//                 ?day_size = day_size,
//                 ?calendar_orientation = calendar_orientation,
//                 ?is_RTL = is_RTL,
//                 ?placeholder = placeholder,
//                 ?reopen_calendar_on_clear = reopen_calendar_on_clear,
//                 ?number_of_months_shown = number_of_months_shown,
//                 ?with_portal = with_portal,
//                 ?with_full_screen_portal = with_full_screen_portal,
//                 ?first_day_of_week = first_day_of_week,
//                 ?stay_open_on_select = stay_open_on_select,
//                 ?show_outside_days = show_outside_days,
//                 ?month_format = month_format,
//                 ?display_format = display_format,
//                 ?disabled = disabled,
//                 ?clearable = clearable,
//                 ?style = style,
//                 ?className = className,
//                 ?loading_state = loading_state,
//                 ?persistence = persistence,
//                 ?persisted_props = persisted_props,
//                 ?persistence_type = persistence_type)
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
