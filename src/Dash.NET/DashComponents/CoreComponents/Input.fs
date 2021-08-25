namespace Dash.NET.DCC

open Dash.NET
open System
open Plotly.NET
open ComponentPropTypes

///<summary>
///A basic HTML input control for entering text, numbers, or passwords.
///Note that checkbox and radio types are supported through
///the Checklist and RadioItems component. Dates, times, and file uploads
///are also supported through separate components.
///</summary>
[<RequireQualifiedAccess>]
module Input =
    ///<summary>
    ///• value (string | number) - The value of the input
    ///&#10;
    ///• style (record) - The input's inline styles
    ///&#10;
    ///• className (string) - The class of the input element
    ///&#10;
    ///• debounce (boolean; default false) - If true, changes to input will be sent back to the Dash server only on enter or when losing focus.
    ///If it's false, it will sent the value back on every change.
    ///&#10;
    ///• type (value equal to: 'text', 'number', 'password', 'email', 'range', 'search', 'tel', 'url', 'hidden'; default text) - The type of control to render.
    ///&#10;
    ///• autoComplete (string) - This attribute indicates whether the value of the control can be automatically completed by the browser.
    ///&#10;
    ///• autoFocus (value equal to: 'autoFocus', 'autofocus', 'AUTOFOCUS' | boolean) - The element should be automatically focused after the page loaded.
    ///autoFocus is an HTML boolean attribute - it is enabled by a boolean or
    ///'autoFocus'. Alternative capitalizations &#96;autofocus&#96; &amp; &#96;AUTOFOCUS&#96;
    ///are also acccepted.
    ///&#10;
    ///• disabled (value equal to: 'disabled', 'DISABLED' | boolean) - If true, the input is disabled and can't be clicked on.
    ///disabled is an HTML boolean attribute - it is enabled by a boolean or
    ///'disabled'. Alternative capitalizations &#96;DISABLED&#96;
    ///&#10;
    ///• inputMode (value equal to: 'verbatim', 'latin', 'latin-name', 'latin-prose', 'full-width-latin', 'kana', 'katakana', 'numeric', 'tel', 'email', 'url') - Provides a hint to the browser as to the type of data that might be
    ///entered by the user while editing the element or its contents.
    ///&#10;
    ///• list (string) - Identifies a list of pre-defined options to suggest to the user.
    ///The value must be the id of a &lt;datalist&gt; element in the same document.
    ///The browser displays only options that are valid values for this
    ///input element.
    ///This attribute is ignored when the type attribute's value is
    ///hidden, checkbox, radio, file, or a button type.
    ///&#10;
    ///• max (string | number) - The maximum (numeric or date-time) value for this item, which must not be less than its minimum (min attribute) value.
    ///&#10;
    ///• maxLength (string | number) - If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the maximum number of characters (in UTF-16 code units) that the user can enter. For other control types, it is ignored. It can exceed the value of the size attribute. If it is not specified, the user can enter an unlimited number of characters. Specifying a negative number results in the default behavior (i.e. the user can enter an unlimited number of characters). The constraint is evaluated only when the value of the attribute has been changed.
    ///&#10;
    ///• min (string | number) - The minimum (numeric or date-time) value for this item, which must not be greater than its maximum (max attribute) value.
    ///&#10;
    ///• minLength (string | number) - If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the minimum number of characters (in Unicode code points) that the user can enter. For other control types, it is ignored.
    ///&#10;
    ///• multiple (boolean) - This Boolean attribute indicates whether the user can enter more than one value. This attribute applies when the type attribute is set to email or file, otherwise it is ignored.
    ///&#10;
    ///• name (string) - The name of the control, which is submitted with the form data.
    ///&#10;
    ///• pattern (string) - A regular expression that the control's value is checked against. The pattern must match the entire value, not just some subset. Use the title attribute to describe the pattern to help the user. This attribute applies when the value of the type attribute is text, search, tel, url, email, or password, otherwise it is ignored. The regular expression language is the same as JavaScript RegExp algorithm, with the 'u' parameter that makes it treat the pattern as a sequence of unicode code points. The pattern is not surrounded by forward slashes.
    ///&#10;
    ///• placeholder (string | number) - A hint to the user of what can be entered in the control . The placeholder text must not contain carriage returns or line-feeds. Note: Do not use the placeholder attribute instead of a &lt;label&gt; element, their purposes are different. The &lt;label&gt; attribute describes the role of the form element (i.e. it indicates what kind of information is expected), and the placeholder attribute is a hint about the format that the content should take. There are cases in which the placeholder attribute is never displayed to the user, so the form must be understandable without it.
    ///&#10;
    ///• readOnly (boolean | value equal to: 'readOnly', 'readonly', 'READONLY') - This attribute indicates that the user cannot modify the value of the control. The value of the attribute is irrelevant. If you need read-write access to the input value, do not add the "readonly" attribute. It is ignored if the value of the type attribute is hidden, range, color, checkbox, radio, file, or a button type (such as button or submit).
    ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
    ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
    ///are also acccepted.
    ///&#10;
    ///• required (value equal to: 'required', 'REQUIRED' | boolean) - This attribute specifies that the user must fill in a value before submitting a form. It cannot be used when the type attribute is hidden, image, or a button type (submit, reset, or button). The :optional and :required CSS pseudo-classes will be applied to the field as appropriate.
    ///required is an HTML boolean attribute - it is enabled by a boolean or
    ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
    ///are also acccepted.
    ///&#10;
    ///• selectionDirection (string) - The direction in which selection occurred. This is "forward" if the selection was made from left-to-right in an LTR locale or right-to-left in an RTL locale, or "backward" if the selection was made in the opposite direction. On platforms on which it's possible this value isn't known, the value can be "none"; for example, on macOS, the default direction is "none", then as the user begins to modify the selection using the keyboard, this will change to reflect the direction in which the selection is expanding.
    ///&#10;
    ///• selectionEnd (string) - The offset into the element's text content of the last selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
    ///&#10;
    ///• selectionStart (string) - The offset into the element's text content of the first selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
    ///&#10;
    ///• size (string) - The initial size of the control. This value is in pixels unless the value of the type attribute is text or password, in which case it is an integer number of characters. Starting in, this attribute applies only when the type attribute is set to text, search, tel, url, email, or password, otherwise it is ignored. In addition, the size must be greater than zero. If you do not specify a size, a default value of 20 is used.' simply states "the user agent should ensure that at least that many characters are visible", but different characters can have different widths in certain fonts. In some browsers, a certain string with x characters will not be entirely visible even if size is defined to at least x.
    ///&#10;
    ///• spellCheck (value equal to: 'true', 'false' | boolean) - Setting the value of this attribute to true indicates that the element needs to have its spelling and grammar checked. The value default indicates that the element is to act according to a default behavior, possibly based on the parent element's own spellcheck value. The value false indicates that the element should not be checked.
    ///&#10;
    ///• step (string | number; default any) - Works with the min and max attributes to limit the increments at which a numeric or date-time value can be set. It can be the string any or a positive floating point number. If this attribute is not set to any, the control accepts only values at multiples of the step value greater than the minimum.
    ///&#10;
    ///• n_submit (number; default 0) - Number of times the &#96;Enter&#96; key was pressed while the input had focus.
    ///&#10;
    ///• n_submit_timestamp (number; default -1) - Last time that &#96;Enter&#96; was pressed.
    ///&#10;
    ///• n_blur (number; default 0) - Number of times the input lost focus.
    ///&#10;
    ///• n_blur_timestamp (number; default -1) - Last time the input lost focus.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///&#10;
    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
    ///hasn't changed from its previous value, a &#96;value&#96; that the user has
    ///changed while using the app will keep that change, as long as
    ///the new &#96;value&#96; also matches what was given originally.
    ///Used in conjunction with &#96;persistence_type&#96;.
    ///&#10;
    ///• persisted_props (list with values of type: value equal to: 'value'; default ['value']) - Properties whose user interactions will persist after refreshing the
    ///component or the page. Since only &#96;value&#96; is allowed this prop can
    ///normally be ignored.
    ///&#10;
    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///</summary>
    type Prop =
        | Value of IConvertible
        | Style of DashComponentStyle
        | ClassName of string
        | Debounce of bool
        | Type of InputType
        | AutoComplete of string
        | AutoFocus of bool
        | Disabled of bool
        | Mode of InputMode
        | List of string
        | Max of IConvertible
        | MaxLength of IConvertible
        | Min of IConvertible
        | MinLength of IConvertible
        | Multiple of bool
        | Name of string
        | Pattern of string
        | Placeholder of IConvertible
        | ReadOnly of bool
        | Required of bool
        | SelectionDirection of string
        | SelectionEnd of string
        | SelectionStart of string
        | Size of string
        | Spellcheck of SpellCheckOptions
        | Step of IConvertible
        | NSubmit of IConvertible
        | NSubmitTimestamp of IConvertible
        | NBlur of IConvertible
        | NBlurTimestamp of IConvertible
        | SetProps of obj
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions
        static member toDynamicMemberDef (prop:Prop) =
            match prop with
            | Value p               -> "value", box p
            | Style p               -> "style", box p
            | ClassName p           -> "className", box p
            | Debounce p            -> "debounce", box p
            | Type p                -> "type", InputType.convert p
            | AutoComplete p        -> "autoComplete", box p
            | AutoFocus p           -> "autoFocus", box p
            | Disabled p            -> "disabled", box p
            | Mode p                -> "inputMode", InputMode.convert p
            | List p                -> "list", box p
            | Max p                 -> "max", box p
            | MaxLength p           -> "maxLength", box p
            | Min p                 -> "min", box p
            | MinLength p           -> "minLength", box p
            | Multiple p            -> "multiple", box p
            | Name p                -> "name", box p
            | Pattern p             -> "pattern", box p
            | Placeholder p         -> "placeholder", box p
            | ReadOnly p            -> "readOnly", box p
            | Required p            -> "required", box p
            | SelectionDirection p  -> "selectionDirection", box p 
            | SelectionEnd p        -> "selectionEnd", box p
            | SelectionStart p      -> "selectionStart", box p
            | Size p                -> "size", box p
            | Spellcheck p          -> "spellCheck", SpellCheckOptions.convert p
            | Step p                -> "step", box p
            | NSubmit p             -> "n_submit", box p
            | NSubmitTimestamp p    -> "n_submit_timestamp", box p
            | NBlur p               -> "n_blur", box p
            | NBlurTimestamp p      -> "n_blur_timestamp", box p
            | SetProps p            -> "setProps", box p
            | LoadingState p        -> "loading_state", box p
            | Persistence p         -> "persistence", box p
            | PersistedProps p      -> "persisted_props", box p
            | PersistenceType p     -> "persistence_type", PersistenceTypeOptions.convert p

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The value of the input
        ///</summary>
        static member value(p: IConvertible) = Prop(Value p)
        ///<summary>
        ///The input's inline styles
        ///</summary>
        static member style(p: DashComponentStyle) = Prop(Style p)
        ///<summary>
        ///The class of the input element
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///If true, changes to input will be sent back to the Dash server only on enter or when losing focus.
        ///If it's false, it will sent the value back on every change.
        ///</summary>
        static member debounce(p: bool) = Prop(Debounce p)
        ///<summary>
        ///The type of control to render.
        ///</summary>
        static member inputType(p: InputType) = Prop(Type p)
        ///<summary>
        ///This attribute indicates whether the value of the control can be automatically completed by the browser.
        ///</summary>
        static member autoComplete(p: string) = Prop(AutoComplete p)

        ///<summary>
        ///The element should be automatically focused after the page loaded.
        ///autoFocus is an HTML boolean attribute - it is enabled by a boolean or
        ///'autoFocus'. Alternative capitalizations &#96;autofocus&#96; &amp; &#96;AUTOFOCUS&#96;
        ///are also acccepted.
        ///</summary>
        static member autoFocus(p: bool) = Prop(AutoFocus p)

        ///<summary>
        ///If true, the input is disabled and can't be clicked on.
        ///disabled is an HTML boolean attribute - it is enabled by a boolean or
        ///'disabled'. Alternative capitalizations &#96;DISABLED&#96;
        ///</summary>
        static member disabled(p: bool) = Prop(Disabled p)
        ///<summary>
        ///Provides a hint to the browser as to the type of data that might be
        ///entered by the user while editing the element or its contents.
        ///</summary>
        static member mode(p: InputMode) = Prop(Mode p)
        ///<summary>
        ///Identifies a list of pre-defined options to suggest to the user.
        ///The value must be the id of a &lt;datalist&gt; element in the same document.
        ///The browser displays only options that are valid values for this
        ///input element.
        ///This attribute is ignored when the type attribute's value is
        ///hidden, checkbox, radio, file, or a button type.
        ///</summary>
        static member list(p: string) = Prop(List p)
        ///<summary>
        ///The maximum (numeric or date-time) value for this item, which must not be less than its minimum (min attribute) value.
        ///</summary>
        static member max(p: IConvertible) = Prop(Max p)
        ///<summary>
        ///If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the maximum number of characters (in UTF-16 code units) that the user can enter. For other control types, it is ignored. It can exceed the value of the size attribute. If it is not specified, the user can enter an unlimited number of characters. Specifying a negative number results in the default behavior (i.e. the user can enter an unlimited number of characters). The constraint is evaluated only when the value of the attribute has been changed.
        ///</summary>
        static member maxLength(p: IConvertible) = Prop(MaxLength p)
        ///<summary>
        ///The minimum (numeric or date-time) value for this item, which must not be greater than its maximum (max attribute) value.
        ///</summary>
        static member min(p: IConvertible) = Prop(Min p)
        ///<summary>
        ///If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the minimum number of characters (in Unicode code points) that the user can enter. For other control types, it is ignored.
        ///</summary>
        static member minLength(p: IConvertible) = Prop(MinLength p)

        ///<summary>
        ///This Boolean attribute indicates whether the user can enter more than one value. This attribute applies when the type attribute is set to email or file, otherwise it is ignored.
        ///</summary>
        static member multiple(p: bool) = Prop(Multiple p)
        ///<summary>
        ///The name of the control, which is submitted with the form data.
        ///</summary>
        static member name(p: string) = Prop(Name p)
        ///<summary>
        ///A regular expression that the control's value is checked against. The pattern must match the entire value, not just some subset. Use the title attribute to describe the pattern to help the user. This attribute applies when the value of the type attribute is text, search, tel, url, email, or password, otherwise it is ignored. The regular expression language is the same as JavaScript RegExp algorithm, with the 'u' parameter that makes it treat the pattern as a sequence of unicode code points. The pattern is not surrounded by forward slashes.
        ///</summary>
        static member pattern(p: string) = Prop(Pattern p)

        ///<summary>
        ///A hint to the user of what can be entered in the control . The placeholder text must not contain carriage returns or line-feeds. Note: Do not use the placeholder attribute instead of a &lt;label&gt; element, their purposes are different. The &lt;label&gt; attribute describes the role of the form element (i.e. it indicates what kind of information is expected), and the placeholder attribute is a hint about the format that the content should take. There are cases in which the placeholder attribute is never displayed to the user, so the form must be understandable without it.
        ///</summary>
        static member placeholder(p: IConvertible) = Prop(Placeholder p)

        ///<summary>
        ///This attribute indicates that the user cannot modify the value of the control. The value of the attribute is irrelevant. If you need read-write access to the input value, do not add the "readonly" attribute. It is ignored if the value of the type attribute is hidden, range, color, checkbox, radio, file, or a button type (such as button or submit).
        ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
        ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
        ///are also acccepted.
        ///</summary>
        static member readOnly(p: bool) = Prop(ReadOnly p)

        ///<summary>
        ///This attribute specifies that the user must fill in a value before submitting a form. It cannot be used when the type attribute is hidden, image, or a button type (submit, reset, or button). The :optional and :required CSS pseudo-classes will be applied to the field as appropriate.
        ///required is an HTML boolean attribute - it is enabled by a boolean or
        ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
        ///are also acccepted.
        ///</summary>
        static member required(p: bool) = Prop(Required p)
        ///<summary>
        ///The direction in which selection occurred. This is "forward" if the selection was made from left-to-right in an LTR locale or right-to-left in an RTL locale, or "backward" if the selection was made in the opposite direction. On platforms on which it's possible this value isn't known, the value can be "none"; for example, on macOS, the default direction is "none", then as the user begins to modify the selection using the keyboard, this will change to reflect the direction in which the selection is expanding.
        ///</summary>
        static member selectionDirection(p: string) = Prop(SelectionDirection p)
        ///<summary>
        ///The offset into the element's text content of the last selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
        ///</summary>
        static member selectionEnd(p: string) = Prop(SelectionEnd p)
        ///<summary>
        ///The offset into the element's text content of the first selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
        ///</summary>
        static member selectionStart(p: string) = Prop(SelectionStart p)
        ///<summary>
        ///The initial size of the control. This value is in pixels unless the value of the type attribute is text or password, in which case it is an integer number of characters. Starting in, this attribute applies only when the type attribute is set to text, search, tel, url, email, or password, otherwise it is ignored. In addition, the size must be greater than zero. If you do not specify a size, a default value of 20 is used.' simply states "the user agent should ensure that at least that many characters are visible", but different characters can have different widths in certain fonts. In some browsers, a certain string with x characters will not be entirely visible even if size is defined to at least x.
        ///</summary>
        static member size(p: string) = Prop(Size p)

        ///<summary>
        ///Setting the value of this attribute to true indicates that the element needs to have its spelling and grammar checked. The value default indicates that the element is to act according to a default behavior, possibly based on the parent element's own spellcheck value. The value false indicates that the element should not be checked.
        ///</summary>
        static member spellCheck(p: SpellCheckOptions) = Prop(Spellcheck p)
        ///<summary>
        ///Works with the min and max attributes to limit the increments at which a numeric or date-time value can be set. It can be the string any or a positive floating point number. If this attribute is not set to any, the control accepts only values at multiples of the step value greater than the minimum.
        ///</summary>
        static member step(p: IConvertible) = Prop(Step p)
        ///<summary>
        ///Number of times the &#96;Enter&#96; key was pressed while the input had focus.
        ///</summary>
        static member nSubmit(p: IConvertible) = Prop(NSubmit p)
        ///<summary>
        ///Last time that &#96;Enter&#96; was pressed.
        ///</summary>
        static member nSubmitTimestamp(p: IConvertible) = Prop(NSubmitTimestamp p)
        ///<summary>
        ///Number of times the input lost focus.
        ///</summary>
        static member nBlur(p: IConvertible) = Prop(NBlur p)
        ///<summary>
        ///Last time the input lost focus.
        ///</summary>
        static member nBlurTimestamp(p: IConvertible) = Prop(NBlurTimestamp p)
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = Prop(LoadingState p)
        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: IConvertible) = Prop(Persistence p)
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps(p: string []) = Prop(PersistedProps p)
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType(p: PersistenceTypeOptions) = Prop(PersistenceType p)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) = Children([ Html.Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: DashComponent) = Children([ value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: list<DashComponent>) = Children(value)
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

    ///<summary>
    ///A basic HTML input control for entering text, numbers, or passwords.
    ///Note that checkbox and radio types are supported through
    ///the Checklist and RadioItems component. Dates, times, and file uploads
    ///are also supported through separate components.
    ///</summary>
    type Input() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?value: string,
                ?style: string,
                ?className: string,
                ?debounce: string,
                ?``type``: string,
                ?autoComplete: string,
                ?autoFocus: string,
                ?disabled: string,
                ?inputMode: string,
                ?list: string,
                ?max: string,
                ?maxLength: string,
                ?min: string,
                ?minLength: string,
                ?multiple: string,
                ?name: string,
                ?pattern: string,
                ?placeholder: string,
                ?readOnly: string,
                ?required: string,
                ?selectionDirection: string,
                ?selectionEnd: string,
                ?selectionStart: string,
                ?size: string,
                ?spellCheck: string,
                ?step: string,
                ?n_submit: string,
                ?n_submit_timestamp: string,
                ?n_blur: string,
                ?n_blur_timestamp: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            (fun (t: Input) ->
                let props = DashComponentProps()
                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                DynObj.setValueOpt props "value" value
                DynObj.setValueOpt props "style" style
                DynObj.setValueOpt props "className" className
                DynObj.setValueOpt props "debounce" debounce
                DynObj.setValueOpt props "type" ``type``
                DynObj.setValueOpt props "autoComplete" autoComplete
                DynObj.setValueOpt props "autoFocus" autoFocus
                DynObj.setValueOpt props "disabled" disabled
                DynObj.setValueOpt props "inputMode" inputMode
                DynObj.setValueOpt props "list" list
                DynObj.setValueOpt props "max" max
                DynObj.setValueOpt props "maxLength" maxLength
                DynObj.setValueOpt props "min" min
                DynObj.setValueOpt props "minLength" minLength
                DynObj.setValueOpt props "multiple" multiple
                DynObj.setValueOpt props "name" name
                DynObj.setValueOpt props "pattern" pattern
                DynObj.setValueOpt props "placeholder" placeholder
                DynObj.setValueOpt props "readOnly" readOnly
                DynObj.setValueOpt props "required" required
                DynObj.setValueOpt props "selectionDirection" selectionDirection
                DynObj.setValueOpt props "selectionEnd" selectionEnd
                DynObj.setValueOpt props "selectionStart" selectionStart
                DynObj.setValueOpt props "size" size
                DynObj.setValueOpt props "spellCheck" spellCheck
                DynObj.setValueOpt props "step" step
                DynObj.setValueOpt props "n_submit" n_submit
                DynObj.setValueOpt props "n_submit_timestamp" n_submit_timestamp
                DynObj.setValueOpt props "n_blur" n_blur
                DynObj.setValueOpt props "n_blur_timestamp" n_blur_timestamp
                DynObj.setValueOpt props "loading_state" loading_state
                DynObj.setValueOpt props "persistence" persistence
                DynObj.setValueOpt props "persisted_props" persisted_props
                DynObj.setValueOpt props "persistence_type" persistence_type
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Input"
                t)

        static member init
            (
                id: string,
                children: seq<DashComponent>,
                ?value: string,
                ?style: string,
                ?className: string,
                ?debounce: string,
                ?``type``: string,
                ?autoComplete: string,
                ?autoFocus: string,
                ?disabled: string,
                ?inputMode: string,
                ?list: string,
                ?max: string,
                ?maxLength: string,
                ?min: string,
                ?minLength: string,
                ?multiple: string,
                ?name: string,
                ?pattern: string,
                ?placeholder: string,
                ?readOnly: string,
                ?required: string,
                ?selectionDirection: string,
                ?selectionEnd: string,
                ?selectionStart: string,
                ?size: string,
                ?spellCheck: string,
                ?step: string,
                ?n_submit: string,
                ?n_submit_timestamp: string,
                ?n_blur: string,
                ?n_blur_timestamp: string,
                ?loading_state: string,
                ?persistence: string,
                ?persisted_props: string,
                ?persistence_type: string
            ) =
            Input.applyMembers
                (id,
                 children,
                 ?value = value,
                 ?style = style,
                 ?className = className,
                 ?debounce = debounce,
                 ?``type`` = ``type``,
                 ?autoComplete = autoComplete,
                 ?autoFocus = autoFocus,
                 ?disabled = disabled,
                 ?inputMode = inputMode,
                 ?list = list,
                 ?max = max,
                 ?maxLength = maxLength,
                 ?min = min,
                 ?minLength = minLength,
                 ?multiple = multiple,
                 ?name = name,
                 ?pattern = pattern,
                 ?placeholder = placeholder,
                 ?readOnly = readOnly,
                 ?required = required,
                 ?selectionDirection = selectionDirection,
                 ?selectionEnd = selectionEnd,
                 ?selectionStart = selectionStart,
                 ?size = size,
                 ?spellCheck = spellCheck,
                 ?step = step,
                 ?n_submit = n_submit,
                 ?n_submit_timestamp = n_submit_timestamp,
                 ?n_blur = n_blur,
                 ?n_blur_timestamp = n_blur_timestamp,
                 ?loading_state = loading_state,
                 ?persistence = persistence,
                 ?persisted_props = persisted_props,
                 ?persistence_type = persistence_type)
                (Input())

    ///<summary>
    ///A basic HTML input control for entering text, numbers, or passwords.
    ///Note that checkbox and radio types are supported through
    ///the Checklist and RadioItems component. Dates, times, and file uploads
    ///are also supported through separate components.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• value (string | number) - The value of the input
    ///&#10;
    ///• style (record) - The input's inline styles
    ///&#10;
    ///• className (string) - The class of the input element
    ///&#10;
    ///• debounce (boolean; default false) - If true, changes to input will be sent back to the Dash server only on enter or when losing focus.
    ///If it's false, it will sent the value back on every change.
    ///&#10;
    ///• type (value equal to: 'text', 'number', 'password', 'email', 'range', 'search', 'tel', 'url', 'hidden'; default text) - The type of control to render.
    ///&#10;
    ///• autoComplete (string) - This attribute indicates whether the value of the control can be automatically completed by the browser.
    ///&#10;
    ///• autoFocus (value equal to: 'autoFocus', 'autofocus', 'AUTOFOCUS' | boolean) - The element should be automatically focused after the page loaded.
    ///autoFocus is an HTML boolean attribute - it is enabled by a boolean or
    ///'autoFocus'. Alternative capitalizations &#96;autofocus&#96; &amp; &#96;AUTOFOCUS&#96;
    ///are also acccepted.
    ///&#10;
    ///• disabled (value equal to: 'disabled', 'DISABLED' | boolean) - If true, the input is disabled and can't be clicked on.
    ///disabled is an HTML boolean attribute - it is enabled by a boolean or
    ///'disabled'. Alternative capitalizations &#96;DISABLED&#96;
    ///&#10;
    ///• inputMode (value equal to: 'verbatim', 'latin', 'latin-name', 'latin-prose', 'full-width-latin', 'kana', 'katakana', 'numeric', 'tel', 'email', 'url') - Provides a hint to the browser as to the type of data that might be
    ///entered by the user while editing the element or its contents.
    ///&#10;
    ///• list (string) - Identifies a list of pre-defined options to suggest to the user.
    ///The value must be the id of a &lt;datalist&gt; element in the same document.
    ///The browser displays only options that are valid values for this
    ///input element.
    ///This attribute is ignored when the type attribute's value is
    ///hidden, checkbox, radio, file, or a button type.
    ///&#10;
    ///• max (string | number) - The maximum (numeric or date-time) value for this item, which must not be less than its minimum (min attribute) value.
    ///&#10;
    ///• maxLength (string | number) - If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the maximum number of characters (in UTF-16 code units) that the user can enter. For other control types, it is ignored. It can exceed the value of the size attribute. If it is not specified, the user can enter an unlimited number of characters. Specifying a negative number results in the default behavior (i.e. the user can enter an unlimited number of characters). The constraint is evaluated only when the value of the attribute has been changed.
    ///&#10;
    ///• min (string | number) - The minimum (numeric or date-time) value for this item, which must not be greater than its maximum (max attribute) value.
    ///&#10;
    ///• minLength (string | number) - If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the minimum number of characters (in Unicode code points) that the user can enter. For other control types, it is ignored.
    ///&#10;
    ///• multiple (boolean) - This Boolean attribute indicates whether the user can enter more than one value. This attribute applies when the type attribute is set to email or file, otherwise it is ignored.
    ///&#10;
    ///• name (string) - The name of the control, which is submitted with the form data.
    ///&#10;
    ///• pattern (string) - A regular expression that the control's value is checked against. The pattern must match the entire value, not just some subset. Use the title attribute to describe the pattern to help the user. This attribute applies when the value of the type attribute is text, search, tel, url, email, or password, otherwise it is ignored. The regular expression language is the same as JavaScript RegExp algorithm, with the 'u' parameter that makes it treat the pattern as a sequence of unicode code points. The pattern is not surrounded by forward slashes.
    ///&#10;
    ///• placeholder (string | number) - A hint to the user of what can be entered in the control . The placeholder text must not contain carriage returns or line-feeds. Note: Do not use the placeholder attribute instead of a &lt;label&gt; element, their purposes are different. The &lt;label&gt; attribute describes the role of the form element (i.e. it indicates what kind of information is expected), and the placeholder attribute is a hint about the format that the content should take. There are cases in which the placeholder attribute is never displayed to the user, so the form must be understandable without it.
    ///&#10;
    ///• readOnly (boolean | value equal to: 'readOnly', 'readonly', 'READONLY') - This attribute indicates that the user cannot modify the value of the control. The value of the attribute is irrelevant. If you need read-write access to the input value, do not add the "readonly" attribute. It is ignored if the value of the type attribute is hidden, range, color, checkbox, radio, file, or a button type (such as button or submit).
    ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
    ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
    ///are also acccepted.
    ///&#10;
    ///• required (value equal to: 'required', 'REQUIRED' | boolean) - This attribute specifies that the user must fill in a value before submitting a form. It cannot be used when the type attribute is hidden, image, or a button type (submit, reset, or button). The :optional and :required CSS pseudo-classes will be applied to the field as appropriate.
    ///required is an HTML boolean attribute - it is enabled by a boolean or
    ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
    ///are also acccepted.
    ///&#10;
    ///• selectionDirection (string) - The direction in which selection occurred. This is "forward" if the selection was made from left-to-right in an LTR locale or right-to-left in an RTL locale, or "backward" if the selection was made in the opposite direction. On platforms on which it's possible this value isn't known, the value can be "none"; for example, on macOS, the default direction is "none", then as the user begins to modify the selection using the keyboard, this will change to reflect the direction in which the selection is expanding.
    ///&#10;
    ///• selectionEnd (string) - The offset into the element's text content of the last selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
    ///&#10;
    ///• selectionStart (string) - The offset into the element's text content of the first selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
    ///&#10;
    ///• size (string) - The initial size of the control. This value is in pixels unless the value of the type attribute is text or password, in which case it is an integer number of characters. Starting in, this attribute applies only when the type attribute is set to text, search, tel, url, email, or password, otherwise it is ignored. In addition, the size must be greater than zero. If you do not specify a size, a default value of 20 is used.' simply states "the user agent should ensure that at least that many characters are visible", but different characters can have different widths in certain fonts. In some browsers, a certain string with x characters will not be entirely visible even if size is defined to at least x.
    ///&#10;
    ///• spellCheck (value equal to: 'true', 'false' | boolean) - Setting the value of this attribute to true indicates that the element needs to have its spelling and grammar checked. The value default indicates that the element is to act according to a default behavior, possibly based on the parent element's own spellcheck value. The value false indicates that the element should not be checked.
    ///&#10;
    ///• step (string | number; default any) - Works with the min and max attributes to limit the increments at which a numeric or date-time value can be set. It can be the string any or a positive floating point number. If this attribute is not set to any, the control accepts only values at multiples of the step value greater than the minimum.
    ///&#10;
    ///• n_submit (number; default 0) - Number of times the &#96;Enter&#96; key was pressed while the input had focus.
    ///&#10;
    ///• n_submit_timestamp (number; default -1) - Last time that &#96;Enter&#96; was pressed.
    ///&#10;
    ///• n_blur (number; default 0) - Number of times the input lost focus.
    ///&#10;
    ///• n_blur_timestamp (number; default -1) - Last time the input lost focus.
    ///&#10;
    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
    ///&#10;
    ///• persistence (boolean | string | number) - Used to allow user interactions in this component to be persisted when
    ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
    ///hasn't changed from its previous value, a &#96;value&#96; that the user has
    ///changed while using the app will keep that change, as long as
    ///the new &#96;value&#96; also matches what was given originally.
    ///Used in conjunction with &#96;persistence_type&#96;.
    ///&#10;
    ///• persisted_props (list with values of type: value equal to: 'value'; default ['value']) - Properties whose user interactions will persist after refreshing the
    ///component or the page. Since only &#96;value&#96; is allowed this prop can
    ///normally be ignored.
    ///&#10;
    ///• persistence_type (value equal to: 'local', 'session', 'memory'; default local) - Where persisted user changes will be stored:
    ///memory: only kept in memory, reset on page refresh.
    ///local: window.localStorage, data is kept after the browser quit.
    ///session: window.sessionStorage, data is cleared once the browser quit.
    ///</summary>
    let input (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Input.init (id, children)

        let componentProps =
            match t.TryGetTypedValue<DashComponentProps> "props" with
            | Some (p) -> p
            | None -> DashComponentProps()

        Seq.iter
            (fun (prop: Prop) ->
                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
                DynObj.setValue componentProps fieldName boxedProp)
            props

        DynObj.setValue t "props" componentProps
        t :> DashComponent
