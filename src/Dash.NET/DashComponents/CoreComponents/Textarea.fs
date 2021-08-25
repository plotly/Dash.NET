namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json
//open System.Collections.Generic

/////<summary>
/////A basic HTML textarea for entering multiline text.
/////</summary>
//[<RequireQualifiedAccess>]
//module Textarea =
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
//    ///value equal to: 'value'
//    ///</summary>
//    type PersistedPropsTypeType =
//        | Value
//        member this.Convert() =
//            match this with
//            | Value -> "value"

//    ///<summary>
//    ///list with values of type: value equal to: 'value'
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
//    ///string | number
//    ///</summary>
//    type TabIndexType =
//        | String of string
//        | IConvertible of IConvertible
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///value equal to: 'true', 'false'
//    ///</summary>
//    type SpellCheckTypeCase0Type =
//        | True
//        | False
//        member this.Convert() =
//            match this with
//            | True -> "true"
//            | False -> "false"

//    ///<summary>
//    ///value equal to: 'true', 'false' | boolean
//    ///</summary>
//    type SpellCheckType =
//        | SpellCheckTypeCase0Type of SpellCheckTypeCase0Type
//        | Bool of bool
//        member this.Convert() =
//            match this with
//            | SpellCheckTypeCase0Type (v) -> box (v.Convert())
//            | Bool (v) -> box v

//    ///<summary>
//    ///value equal to: 'true', 'false'
//    ///</summary>
//    type DraggableTypeCase0Type =
//        | True
//        | False
//        member this.Convert() =
//            match this with
//            | True -> "true"
//            | False -> "false"

//    ///<summary>
//    ///value equal to: 'true', 'false' | boolean
//    ///</summary>
//    type DraggableType =
//        | DraggableTypeCase0Type of DraggableTypeCase0Type
//        | Bool of bool
//        member this.Convert() =
//            match this with
//            | DraggableTypeCase0Type (v) -> box (v.Convert())
//            | Bool (v) -> box v

//    ///<summary>
//    ///string | boolean
//    ///</summary>
//    type ContentEditableType =
//        | String of string
//        | Bool of bool
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | Bool (v) -> box v

//    ///<summary>
//    ///string | number
//    ///</summary>
//    type RowsType =
//        | String of string
//        | IConvertible of IConvertible
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///value equal to: 'required', 'REQUIRED'
//    ///</summary>
//    type RequiredTypeCase0Type =
//        | Required
//        | REQUIRED
//        member this.Convert() =
//            match this with
//            | Required -> "required"
//            | REQUIRED -> "REQUIRED"

//    ///<summary>
//    ///value equal to: 'required', 'REQUIRED' | boolean
//    ///</summary>
//    type RequiredType =
//        | RequiredTypeCase0Type of RequiredTypeCase0Type
//        | Bool of bool
//        member this.Convert() =
//            match this with
//            | RequiredTypeCase0Type (v) -> box (v.Convert())
//            | Bool (v) -> box v

//    ///<summary>
//    ///value equal to: 'readOnly', 'readonly', 'READONLY'
//    ///</summary>
//    type ReadOnlyTypeCase1Type =
//        | ReadOnly
//        | Readonly
//        | READONLY
//        member this.Convert() =
//            match this with
//            | ReadOnly -> "readOnly"
//            | Readonly -> "readonly"
//            | READONLY -> "READONLY"

//    ///<summary>
//    ///boolean | value equal to: 'readOnly', 'readonly', 'READONLY'
//    ///</summary>
//    type ReadOnlyType =
//        | Bool of bool
//        | ReadOnlyTypeCase1Type of ReadOnlyTypeCase1Type
//        member this.Convert() =
//            match this with
//            | Bool (v) -> box v
//            | ReadOnlyTypeCase1Type (v) -> box (v.Convert())

//    ///<summary>
//    ///string | number
//    ///</summary>
//    type MinLengthType =
//        | String of string
//        | IConvertible of IConvertible
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///string | number
//    ///</summary>
//    type MaxLengthType =
//        | String of string
//        | IConvertible of IConvertible
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///string | boolean
//    ///</summary>
//    type DisabledType =
//        | String of string
//        | Bool of bool
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | Bool (v) -> box v

//    ///<summary>
//    ///string | number
//    ///</summary>
//    type ColsType =
//        | String of string
//        | IConvertible of IConvertible
//        member this.Convert() =
//            match this with
//            | String (v) -> box v
//            | IConvertible (v) -> box v

//    ///<summary>
//    ///• value (string) - The value of the textarea
//    ///&#10;
//    ///• autoFocus (string) - The element should be automatically focused after the page loaded.
//    ///&#10;
//    ///• cols (string | number) - Defines the number of columns in a textarea.
//    ///&#10;
//    ///• disabled (string | boolean) - Indicates whether the user can interact with the element.
//    ///&#10;
//    ///• form (string) - Indicates the form that is the owner of the element.
//    ///&#10;
//    ///• maxLength (string | number) - Defines the maximum number of characters allowed in the element.
//    ///&#10;
//    ///• minLength (string | number) - Defines the minimum number of characters allowed in the element.
//    ///&#10;
//    ///• name (string) - Name of the element. For example used by the server to identify the fields in form submits.
//    ///&#10;
//    ///• placeholder (string) - Provides a hint to the user of what can be entered in the field.
//    ///&#10;
//    ///• readOnly (boolean | value equal to: 'readOnly', 'readonly', 'READONLY') - Indicates whether the element can be edited.
//    ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
//    ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
//    ///are also acccepted.
//    ///&#10;
//    ///• required (value equal to: 'required', 'REQUIRED' | boolean) - Indicates whether this element is required to fill out or not.
//    ///required is an HTML boolean attribute - it is enabled by a boolean or
//    ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
//    ///are also acccepted.
//    ///&#10;
//    ///• rows (string | number) - Defines the number of rows in a text area.
//    ///&#10;
//    ///• wrap (string) - Indicates whether the text should be wrapped.
//    ///&#10;
//    ///• accessKey (string) - Defines a keyboard shortcut to activate or add focus to the element.
//    ///&#10;
//    ///• className (string) - Often used with CSS to style elements with common properties.
//    ///&#10;
//    ///• contentEditable (string | boolean) - Indicates whether the element's content is editable.
//    ///&#10;
//    ///• contextMenu (string) - Defines the ID of a &lt;menu&gt; element which will serve as the element's context menu.
//    ///&#10;
//    ///• dir (string) - Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left)
//    ///&#10;
//    ///• draggable (value equal to: 'true', 'false' | boolean) - Defines whether the element can be dragged.
//    ///&#10;
//    ///• hidden (string) - Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
//    ///&#10;
//    ///• lang (string) - Defines the language used in the element.
//    ///&#10;
//    ///• spellCheck (value equal to: 'true', 'false' | boolean) - Indicates whether spell checking is allowed for the element.
//    ///&#10;
//    ///• style (record) - Defines CSS styles which will override styles previously set.
//    ///&#10;
//    ///• tabIndex (string | number) - Overrides the browser's default tab order and follows the one specified instead.
//    ///&#10;
//    ///• title (string) - Text to be displayed in a tooltip when hovering over the element.
//    ///&#10;
//    ///• n_blur (number; default 0) - Number of times the textarea lost focus.
//    ///&#10;
//    ///• n_blur_timestamp (number; default -1) - Last time the textarea lost focus.
//    ///&#10;
//    ///• n_clicks (number; default 0) - Number of times the textarea has been clicked.
//    ///&#10;
//    ///• n_clicks_timestamp (number; default -1) - Last time the textarea was clicked.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///&#10;
//    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
//    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//    ///hasn't changed from its previous value, a &#96;value&#96; that the user has
//    ///changed while using the app will keep that change, as long as
//    ///the new &#96;value&#96; also matches what was given originally.
//    ///Used in conjunction with &#96;persistence_type&#96;.
//    ///&#10;
//    ///• persisted_props (list with values of type: value equal to: 'value'; default ['value']) - Properties whose user interactions will persist after refreshing the
//    ///component or the page. Since only &#96;value&#96; is allowed this prop can
//    ///normally be ignored.
//    ///&#10;
//    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
//    ///memory: only kept in memory, reset on page refresh.
//    ///local: window.localStorage, data is kept after the browser quit.
//    ///session: window.sessionStorage, data is cleared once the browser quit.
//    ///</summary>
//    type Prop =
//        | Value of string
//        | AutoFocus of string
//        | Cols of ColsType
//        | Disabled of DisabledType
//        | Form of string
//        | MaxLength of MaxLengthType
//        | MinLength of MinLengthType
//        | Name of string
//        | Placeholder of string
//        | ReadOnly of ReadOnlyType
//        | Required of RequiredType
//        | Rows of RowsType
//        | Wrap of string
//        | AccessKey of string
//        | ClassName of string
//        | ContentEditable of ContentEditableType
//        | ContextMenu of string
//        | Dir of string
//        | Draggable of DraggableType
//        | Hidden of string
//        | Lang of string
//        | SpellCheck of SpellCheckType
//        | Style of obj
//        | TabIndex of TabIndexType
//        | Title of string
//        | NBlur of IConvertible
//        | NBlurTimestamp of IConvertible
//        | NClicks of IConvertible
//        | NClicksTimestamp of IConvertible
//        | LoadingState of LoadingStateType
//        | Persistence of PersistenceType
//        | PersistedProps of PersistedPropsType
//        | PersistenceType of PersistenceTypeType
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Value (p) -> "value", box p
//            | AutoFocus (p) -> "autoFocus", box p
//            | Cols (p) -> "cols", box (p.Convert())
//            | Disabled (p) -> "disabled", box (p.Convert())
//            | Form (p) -> "form", box p
//            | MaxLength (p) -> "maxLength", box (p.Convert())
//            | MinLength (p) -> "minLength", box (p.Convert())
//            | Name (p) -> "name", box p
//            | Placeholder (p) -> "placeholder", box p
//            | ReadOnly (p) -> "readOnly", box (p.Convert())
//            | Required (p) -> "required", box (p.Convert())
//            | Rows (p) -> "rows", box (p.Convert())
//            | Wrap (p) -> "wrap", box p
//            | AccessKey (p) -> "accessKey", box p
//            | ClassName (p) -> "className", box p
//            | ContentEditable (p) -> "contentEditable", box (p.Convert())
//            | ContextMenu (p) -> "contextMenu", box p
//            | Dir (p) -> "dir", box p
//            | Draggable (p) -> "draggable", box (p.Convert())
//            | Hidden (p) -> "hidden", box p
//            | Lang (p) -> "lang", box p
//            | SpellCheck (p) -> "spellCheck", box (p.Convert())
//            | Style (p) -> "style", box p
//            | TabIndex (p) -> "tabIndex", box (p.Convert())
//            | Title (p) -> "title", box p
//            | NBlur (p) -> "n_blur", box p
//            | NBlurTimestamp (p) -> "n_blur_timestamp", box p
//            | NClicks (p) -> "n_clicks", box p
//            | NClicksTimestamp (p) -> "n_clicks_timestamp", box p
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
//        ///The value of the textarea
//        ///</summary>
//        static member value(p: string) = Prop(Value p)
//        ///<summary>
//        ///The element should be automatically focused after the page loaded.
//        ///</summary>
//        static member autoFocus(p: string) = Prop(AutoFocus p)
//        ///<summary>
//        ///Defines the number of columns in a textarea.
//        ///</summary>
//        static member cols(p: string) = Prop(Cols(ColsType.String p))
//        ///<summary>
//        ///Defines the number of columns in a textarea.
//        ///</summary>
//        static member cols(p: IConvertible) = Prop(Cols(ColsType.IConvertible p))
//        ///<summary>
//        ///Indicates whether the user can interact with the element.
//        ///</summary>
//        static member disabled(p: string) = Prop(Disabled(DisabledType.String p))
//        ///<summary>
//        ///Indicates whether the user can interact with the element.
//        ///</summary>
//        static member disabled(p: bool) = Prop(Disabled(DisabledType.Bool p))
//        ///<summary>
//        ///Indicates the form that is the owner of the element.
//        ///</summary>
//        static member form(p: string) = Prop(Form p)
//        ///<summary>
//        ///Defines the maximum number of characters allowed in the element.
//        ///</summary>
//        static member maxLength(p: string) = Prop(MaxLength(MaxLengthType.String p))
//        ///<summary>
//        ///Defines the maximum number of characters allowed in the element.
//        ///</summary>
//        static member maxLength(p: IConvertible) =
//            Prop(MaxLength(MaxLengthType.IConvertible p))

//        ///<summary>
//        ///Defines the minimum number of characters allowed in the element.
//        ///</summary>
//        static member minLength(p: string) = Prop(MinLength(MinLengthType.String p))
//        ///<summary>
//        ///Defines the minimum number of characters allowed in the element.
//        ///</summary>
//        static member minLength(p: IConvertible) =
//            Prop(MinLength(MinLengthType.IConvertible p))

//        ///<summary>
//        ///Name of the element. For example used by the server to identify the fields in form submits.
//        ///</summary>
//        static member name(p: string) = Prop(Name p)
//        ///<summary>
//        ///Provides a hint to the user of what can be entered in the field.
//        ///</summary>
//        static member placeholder(p: string) = Prop(Placeholder p)
//        ///<summary>
//        ///Indicates whether the element can be edited.
//        ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
//        ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
//        ///are also acccepted.
//        ///</summary>
//        static member readOnly(p: bool) = Prop(ReadOnly(ReadOnlyType.Bool p))
//        ///<summary>
//        ///Indicates whether the element can be edited.
//        ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
//        ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
//        ///are also acccepted.
//        ///</summary>
//        static member readOnly(p: ReadOnlyTypeCase1Type) =
//            Prop(ReadOnly(ReadOnlyType.ReadOnlyTypeCase1Type p))

//        ///<summary>
//        ///Indicates whether this element is required to fill out or not.
//        ///required is an HTML boolean attribute - it is enabled by a boolean or
//        ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
//        ///are also acccepted.
//        ///</summary>
//        static member required(p: RequiredTypeCase0Type) =
//            Prop(Required(RequiredType.RequiredTypeCase0Type p))

//        ///<summary>
//        ///Indicates whether this element is required to fill out or not.
//        ///required is an HTML boolean attribute - it is enabled by a boolean or
//        ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
//        ///are also acccepted.
//        ///</summary>
//        static member required(p: bool) = Prop(Required(RequiredType.Bool p))
//        ///<summary>
//        ///Defines the number of rows in a text area.
//        ///</summary>
//        static member rows(p: string) = Prop(Rows(RowsType.String p))
//        ///<summary>
//        ///Defines the number of rows in a text area.
//        ///</summary>
//        static member rows(p: IConvertible) = Prop(Rows(RowsType.IConvertible p))
//        ///<summary>
//        ///Indicates whether the text should be wrapped.
//        ///</summary>
//        static member wrap(p: string) = Prop(Wrap p)
//        ///<summary>
//        ///Defines a keyboard shortcut to activate or add focus to the element.
//        ///</summary>
//        static member accessKey(p: string) = Prop(AccessKey p)
//        ///<summary>
//        ///Often used with CSS to style elements with common properties.
//        ///</summary>
//        static member className(p: string) = Prop(ClassName p)
//        ///<summary>
//        ///Indicates whether the element's content is editable.
//        ///</summary>
//        static member contentEditable(p: string) =
//            Prop(ContentEditable(ContentEditableType.String p))

//        ///<summary>
//        ///Indicates whether the element's content is editable.
//        ///</summary>
//        static member contentEditable(p: bool) =
//            Prop(ContentEditable(ContentEditableType.Bool p))

//        ///<summary>
//        ///Defines the ID of a &lt;menu&gt; element which will serve as the element's context menu.
//        ///</summary>
//        static member contextMenu(p: string) = Prop(ContextMenu p)
//        ///<summary>
//        ///Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left)
//        ///</summary>
//        static member dir(p: string) = Prop(Dir p)
//        ///<summary>
//        ///Defines whether the element can be dragged.
//        ///</summary>
//        static member draggable(p: DraggableTypeCase0Type) =
//            Prop(Draggable(DraggableType.DraggableTypeCase0Type p))

//        ///<summary>
//        ///Defines whether the element can be dragged.
//        ///</summary>
//        static member draggable(p: bool) = Prop(Draggable(DraggableType.Bool p))
//        ///<summary>
//        ///Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
//        ///</summary>
//        static member hidden(p: string) = Prop(Hidden p)
//        ///<summary>
//        ///Defines the language used in the element.
//        ///</summary>
//        static member lang(p: string) = Prop(Lang p)
//        ///<summary>
//        ///Indicates whether spell checking is allowed for the element.
//        ///</summary>
//        static member spellCheck(p: SpellCheckTypeCase0Type) =
//            Prop(SpellCheck(SpellCheckType.SpellCheckTypeCase0Type p))

//        ///<summary>
//        ///Indicates whether spell checking is allowed for the element.
//        ///</summary>
//        static member spellCheck(p: bool) = Prop(SpellCheck(SpellCheckType.Bool p))
//        ///<summary>
//        ///Defines CSS styles which will override styles previously set.
//        ///</summary>
//        static member style(p: obj) = Prop(Style p)
//        ///<summary>
//        ///Overrides the browser's default tab order and follows the one specified instead.
//        ///</summary>
//        static member tabIndex(p: string) = Prop(TabIndex(TabIndexType.String p))
//        ///<summary>
//        ///Overrides the browser's default tab order and follows the one specified instead.
//        ///</summary>
//        static member tabIndex(p: IConvertible) =
//            Prop(TabIndex(TabIndexType.IConvertible p))

//        ///<summary>
//        ///Text to be displayed in a tooltip when hovering over the element.
//        ///</summary>
//        static member title(p: string) = Prop(Title p)
//        ///<summary>
//        ///Number of times the textarea lost focus.
//        ///</summary>
//        static member nBlur(p: IConvertible) = Prop(NBlur p)
//        ///<summary>
//        ///Last time the textarea lost focus.
//        ///</summary>
//        static member nBlurTimestamp(p: IConvertible) = Prop(NBlurTimestamp p)
//        ///<summary>
//        ///Number of times the textarea has been clicked.
//        ///</summary>
//        static member nClicks(p: IConvertible) = Prop(NClicks p)
//        ///<summary>
//        ///Last time the textarea was clicked.
//        ///</summary>
//        static member nClicksTimestamp(p: IConvertible) = Prop(NClicksTimestamp p)
//        ///<summary>
//        ///Object that holds the loading state object coming from dash-renderer
//        ///</summary>
//        static member loadingState(p: LoadingStateType) = Prop(LoadingState p)
//        ///<summary>
//        ///Used to allow user interactions in this component to be persisted when
//        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
//        ///changed while using the app will keep that change, as long as
//        ///the new &#96;value&#96; also matches what was given originally.
//        ///Used in conjunction with &#96;persistence_type&#96;.
//        ///</summary>
//        static member persistence(p: bool) =
//            Prop(Persistence(PersistenceType.Bool p))

//        ///<summary>
//        ///Used to allow user interactions in this component to be persisted when
//        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
//        ///changed while using the app will keep that change, as long as
//        ///the new &#96;value&#96; also matches what was given originally.
//        ///Used in conjunction with &#96;persistence_type&#96;.
//        ///</summary>
//        static member persistence(p: string) =
//            Prop(Persistence(PersistenceType.String p))

//        ///<summary>
//        ///Used to allow user interactions in this component to be persisted when
//        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
//        ///changed while using the app will keep that change, as long as
//        ///the new &#96;value&#96; also matches what was given originally.
//        ///Used in conjunction with &#96;persistence_type&#96;.
//        ///</summary>
//        static member persistence(p: IConvertible) =
//            Prop(Persistence(PersistenceType.IConvertible p))

//        ///<summary>
//        ///Properties whose user interactions will persist after refreshing the
//        ///component or the page. Since only &#96;value&#96; is allowed this prop can
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
//    ///A basic HTML textarea for entering multiline text.
//    ///</summary>
//    type Textarea() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?value: string,
//                ?autoFocus: string,
//                ?cols: string,
//                ?disabled: string,
//                ?form: string,
//                ?maxLength: string,
//                ?minLength: string,
//                ?name: string,
//                ?placeholder: string,
//                ?readOnly: string,
//                ?required: string,
//                ?rows: string,
//                ?wrap: string,
//                ?accessKey: string,
//                ?className: string,
//                ?contentEditable: string,
//                ?contextMenu: string,
//                ?dir: string,
//                ?draggable: string,
//                ?hidden: string,
//                ?lang: string,
//                ?spellCheck: string,
//                ?style: string,
//                ?tabIndex: string,
//                ?title: string,
//                ?n_blur: string,
//                ?n_blur_timestamp: string,
//                ?n_clicks: string,
//                ?n_clicks_timestamp: string,
//                ?loading_state: string,
//                ?persistence: string,
//                ?persisted_props: string,
//                ?persistence_type: string
//            ) =
//            (fun (t: Textarea) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "value" value
//                DynObj.setValueOpt props "autoFocus" autoFocus
//                DynObj.setValueOpt props "cols" cols
//                DynObj.setValueOpt props "disabled" disabled
//                DynObj.setValueOpt props "form" form
//                DynObj.setValueOpt props "maxLength" maxLength
//                DynObj.setValueOpt props "minLength" minLength
//                DynObj.setValueOpt props "name" name
//                DynObj.setValueOpt props "placeholder" placeholder
//                DynObj.setValueOpt props "readOnly" readOnly
//                DynObj.setValueOpt props "required" required
//                DynObj.setValueOpt props "rows" rows
//                DynObj.setValueOpt props "wrap" wrap
//                DynObj.setValueOpt props "accessKey" accessKey
//                DynObj.setValueOpt props "className" className
//                DynObj.setValueOpt props "contentEditable" contentEditable
//                DynObj.setValueOpt props "contextMenu" contextMenu
//                DynObj.setValueOpt props "dir" dir
//                DynObj.setValueOpt props "draggable" draggable
//                DynObj.setValueOpt props "hidden" hidden
//                DynObj.setValueOpt props "lang" lang
//                DynObj.setValueOpt props "spellCheck" spellCheck
//                DynObj.setValueOpt props "style" style
//                DynObj.setValueOpt props "tabIndex" tabIndex
//                DynObj.setValueOpt props "title" title
//                DynObj.setValueOpt props "n_blur" n_blur
//                DynObj.setValueOpt props "n_blur_timestamp" n_blur_timestamp
//                DynObj.setValueOpt props "n_clicks" n_clicks
//                DynObj.setValueOpt props "n_clicks_timestamp" n_clicks_timestamp
//                DynObj.setValueOpt props "loading_state" loading_state
//                DynObj.setValueOpt props "persistence" persistence
//                DynObj.setValueOpt props "persisted_props" persisted_props
//                DynObj.setValueOpt props "persistence_type" persistence_type
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "Textarea"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?value: string,
//                ?autoFocus: string,
//                ?cols: string,
//                ?disabled: string,
//                ?form: string,
//                ?maxLength: string,
//                ?minLength: string,
//                ?name: string,
//                ?placeholder: string,
//                ?readOnly: string,
//                ?required: string,
//                ?rows: string,
//                ?wrap: string,
//                ?accessKey: string,
//                ?className: string,
//                ?contentEditable: string,
//                ?contextMenu: string,
//                ?dir: string,
//                ?draggable: string,
//                ?hidden: string,
//                ?lang: string,
//                ?spellCheck: string,
//                ?style: string,
//                ?tabIndex: string,
//                ?title: string,
//                ?n_blur: string,
//                ?n_blur_timestamp: string,
//                ?n_clicks: string,
//                ?n_clicks_timestamp: string,
//                ?loading_state: string,
//                ?persistence: string,
//                ?persisted_props: string,
//                ?persistence_type: string
//            ) =
//            Textarea.applyMembers
//                (id,
//                 children,
//                 ?value = value,
//                 ?autoFocus = autoFocus,
//                 ?cols = cols,
//                 ?disabled = disabled,
//                 ?form = form,
//                 ?maxLength = maxLength,
//                 ?minLength = minLength,
//                 ?name = name,
//                 ?placeholder = placeholder,
//                 ?readOnly = readOnly,
//                 ?required = required,
//                 ?rows = rows,
//                 ?wrap = wrap,
//                 ?accessKey = accessKey,
//                 ?className = className,
//                 ?contentEditable = contentEditable,
//                 ?contextMenu = contextMenu,
//                 ?dir = dir,
//                 ?draggable = draggable,
//                 ?hidden = hidden,
//                 ?lang = lang,
//                 ?spellCheck = spellCheck,
//                 ?style = style,
//                 ?tabIndex = tabIndex,
//                 ?title = title,
//                 ?n_blur = n_blur,
//                 ?n_blur_timestamp = n_blur_timestamp,
//                 ?n_clicks = n_clicks,
//                 ?n_clicks_timestamp = n_clicks_timestamp,
//                 ?loading_state = loading_state,
//                 ?persistence = persistence,
//                 ?persisted_props = persisted_props,
//                 ?persistence_type = persistence_type)
//                (Textarea())

//    ///<summary>
//    ///A basic HTML textarea for entering multiline text.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - The ID of this component, used to identify dash components
//    ///in callbacks. The ID needs to be unique across all of the
//    ///components in an app.
//    ///&#10;
//    ///• value (string) - The value of the textarea
//    ///&#10;
//    ///• autoFocus (string) - The element should be automatically focused after the page loaded.
//    ///&#10;
//    ///• cols (string | number) - Defines the number of columns in a textarea.
//    ///&#10;
//    ///• disabled (string | boolean) - Indicates whether the user can interact with the element.
//    ///&#10;
//    ///• form (string) - Indicates the form that is the owner of the element.
//    ///&#10;
//    ///• maxLength (string | number) - Defines the maximum number of characters allowed in the element.
//    ///&#10;
//    ///• minLength (string | number) - Defines the minimum number of characters allowed in the element.
//    ///&#10;
//    ///• name (string) - Name of the element. For example used by the server to identify the fields in form submits.
//    ///&#10;
//    ///• placeholder (string) - Provides a hint to the user of what can be entered in the field.
//    ///&#10;
//    ///• readOnly (boolean | value equal to: 'readOnly', 'readonly', 'READONLY') - Indicates whether the element can be edited.
//    ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
//    ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
//    ///are also acccepted.
//    ///&#10;
//    ///• required (value equal to: 'required', 'REQUIRED' | boolean) - Indicates whether this element is required to fill out or not.
//    ///required is an HTML boolean attribute - it is enabled by a boolean or
//    ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
//    ///are also acccepted.
//    ///&#10;
//    ///• rows (string | number) - Defines the number of rows in a text area.
//    ///&#10;
//    ///• wrap (string) - Indicates whether the text should be wrapped.
//    ///&#10;
//    ///• accessKey (string) - Defines a keyboard shortcut to activate or add focus to the element.
//    ///&#10;
//    ///• className (string) - Often used with CSS to style elements with common properties.
//    ///&#10;
//    ///• contentEditable (string | boolean) - Indicates whether the element's content is editable.
//    ///&#10;
//    ///• contextMenu (string) - Defines the ID of a &lt;menu&gt; element which will serve as the element's context menu.
//    ///&#10;
//    ///• dir (string) - Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left)
//    ///&#10;
//    ///• draggable (value equal to: 'true', 'false' | boolean) - Defines whether the element can be dragged.
//    ///&#10;
//    ///• hidden (string) - Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
//    ///&#10;
//    ///• lang (string) - Defines the language used in the element.
//    ///&#10;
//    ///• spellCheck (value equal to: 'true', 'false' | boolean) - Indicates whether spell checking is allowed for the element.
//    ///&#10;
//    ///• style (record) - Defines CSS styles which will override styles previously set.
//    ///&#10;
//    ///• tabIndex (string | number) - Overrides the browser's default tab order and follows the one specified instead.
//    ///&#10;
//    ///• title (string) - Text to be displayed in a tooltip when hovering over the element.
//    ///&#10;
//    ///• n_blur (number; default 0) - Number of times the textarea lost focus.
//    ///&#10;
//    ///• n_blur_timestamp (number; default -1) - Last time the textarea lost focus.
//    ///&#10;
//    ///• n_clicks (number; default 0) - Number of times the textarea has been clicked.
//    ///&#10;
//    ///• n_clicks_timestamp (number; default -1) - Last time the textarea was clicked.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///&#10;
//    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
//    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
//    ///hasn't changed from its previous value, a &#96;value&#96; that the user has
//    ///changed while using the app will keep that change, as long as
//    ///the new &#96;value&#96; also matches what was given originally.
//    ///Used in conjunction with &#96;persistence_type&#96;.
//    ///&#10;
//    ///• persisted_props (list with values of type: value equal to: 'value'; default ['value']) - Properties whose user interactions will persist after refreshing the
//    ///component or the page. Since only &#96;value&#96; is allowed this prop can
//    ///normally be ignored.
//    ///&#10;
//    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
//    ///memory: only kept in memory, reset on page refresh.
//    ///local: window.localStorage, data is kept after the browser quit.
//    ///session: window.sessionStorage, data is cleared once the browser quit.
//    ///</summary>
//    let textarea (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = Textarea.init (id, children)

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
