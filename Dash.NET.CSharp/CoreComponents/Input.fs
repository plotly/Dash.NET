namespace Dash.NET.CSharp.DCC

open System
open ComponentPropTypes
open Dash.NET.CSharp.ComponentStyle

// Original attr
type internal OAttr = Dash.NET.DCC.Input.Attr

///<summary>
///A basic HTML input control for entering text, numbers, or passwords.
///Note that checkbox and radio types are supported through
///the Checklist and RadioItems component. Dates, times, and file uploads
///are also supported through separate components.
///</summary>
[<RequireQualifiedAccess>]
module Input =

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr = private WrappedAttr of Dash.NET.DCC.Input.Attr with
        static member internal Wrap (attr : Dash.NET.DCC.Input.Attr) = Attr.WrappedAttr attr
        static member internal Unwrap (attr : Attr) = match attr with | Attr.WrappedAttr attr -> attr

        ///<summary>
        ///The value of the input
        ///</summary>
        static member value(p: IConvertible) = OAttr.value p |> Attr.Wrap
        ///<summary>
        ///The input's inline styles
        ///</summary>
        static member style([<ParamArray>] p: array<Dash.NET.CSharp.Dsl.Style>) = OAttr.style (p |> Array.map Dash.NET.CSharp.Dsl.Style.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The class of the input element
        ///</summary>
        static member className(p: string) = OAttr.className p |> Attr.Wrap
        ///<summary>
        ///If true, changes to input will be sent back to the Dash server only on enter or when losing focus.
        ///If it's false, it will sent the value back on every change.
        ///</summary>
        static member debounce(p: bool) = OAttr.debounce p |> Attr.Wrap
        ///<summary>
        ///The type of control to render.
        ///</summary>
        static member inputType(p: InputType) = OAttr.inputType (p |> InputType.Unwrap) |> Attr.Wrap
        ///<summary>
        ///This attribute indicates whether the value of the control can be automatically completed by the browser.
        ///</summary>
        static member autoComplete(p: string) = OAttr.autoComplete p |> Attr.Wrap

        ///<summary>
        ///The element should be automatically focused after the page loaded.
        ///autoFocus is an HTML boolean attribute - it is enabled by a boolean or
        ///'autoFocus'. Alternative capitalizations &#96;autofocus&#96; &amp; &#96;AUTOFOCUS&#96;
        ///are also acccepted.
        ///</summary>
        static member autoFocus(p: bool) = OAttr.autoFocus p |> Attr.Wrap

        ///<summary>
        ///If true, the input is disabled and can't be clicked on.
        ///disabled is an HTML boolean attribute - it is enabled by a boolean or
        ///'disabled'. Alternative capitalizations &#96;DISABLED&#96;
        ///</summary>
        static member disabled(p: bool) = OAttr.disabled p |> Attr.Wrap
        ///<summary>
        ///Provides a hint to the browser as to the type of data that might be
        ///entered by the user while editing the element or its contents.
        ///</summary>
        static member mode(p: InputMode) = OAttr.mode (p |> InputMode.Unwrap) |> Attr.Wrap
        ///<summary>
        ///Identifies a list of pre-defined options to suggest to the user.
        ///The value must be the id of a &lt;datalist&gt; element in the same document.
        ///The browser displays only options that are valid values for this
        ///input element.
        ///This attribute is ignored when the type attribute's value is
        ///hidden, checkbox, radio, file, or a button type.
        ///</summary>
        static member list(p: string) = OAttr.list p |> Attr.Wrap
        ///<summary>
        ///The maximum (numeric or date-time) value for this item, which must not be less than its minimum (min attribute) value.
        ///</summary>
        static member max(p: IConvertible) = OAttr.max p |> Attr.Wrap
        ///<summary>
        ///If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the maximum number of characters (in UTF-16 code units) that the user can enter. For other control types, it is ignored. It can exceed the value of the size attribute. If it is not specified, the user can enter an unlimited number of characters. Specifying a negative number results in the default behavior (i.e. the user can enter an unlimited number of characters). The constraint is evaluated only when the value of the attribute has been changed.
        ///</summary>
        static member maxLength(p: IConvertible) = OAttr.maxLength p |> Attr.Wrap
        ///<summary>
        ///The minimum (numeric or date-time) value for this item, which must not be greater than its maximum (max attribute) value.
        ///</summary>
        static member min(p: IConvertible) = OAttr.min p |> Attr.Wrap
        ///<summary>
        ///If the value of the type attribute is text, email, search, password, tel, or url, this attribute specifies the minimum number of characters (in Unicode code points) that the user can enter. For other control types, it is ignored.
        ///</summary>
        static member minLength(p: IConvertible) = OAttr.minLength p |> Attr.Wrap

        ///<summary>
        ///This Boolean attribute indicates whether the user can enter more than one value. This attribute applies when the type attribute is set to email or file, otherwise it is ignored.
        ///</summary>
        static member multiple(p: bool) = OAttr.multiple p |> Attr.Wrap
        ///<summary>
        ///The name of the control, which is submitted with the form data.
        ///</summary>
        static member name(p: string) = OAttr.name p |> Attr.Wrap
        ///<summary>
        ///A regular expression that the control's value is checked against. The pattern must match the entire value, not just some subset. Use the title attribute to describe the pattern to help the user. This attribute applies when the value of the type attribute is text, search, tel, url, email, or password, otherwise it is ignored. The regular expression language is the same as JavaScript RegExp algorithm, with the 'u' parameter that makes it treat the pattern as a sequence of unicode code points. The pattern is not surrounded by forward slashes.
        ///</summary>
        static member pattern(p: string) = OAttr.pattern p |> Attr.Wrap

        ///<summary>
        ///A hint to the user of what can be entered in the control . The placeholder text must not contain carriage returns or line-feeds. Note: Do not use the placeholder attribute instead of a &lt;label&gt; element, their purposes are different. The &lt;label&gt; attribute describes the role of the form element (i.e. it indicates what kind of information is expected), and the placeholder attribute is a hint about the format that the content should take. There are cases in which the placeholder attribute is never displayed to the user, so the form must be understandable without it.
        ///</summary>
        static member placeholder(p: IConvertible) = OAttr.placeholder p |> Attr.Wrap

        ///<summary>
        ///This attribute indicates that the user cannot modify the value of the control. The value of the attribute is irrelevant. If you need read-write access to the input value, do not add the "readonly" attribute. It is ignored if the value of the type attribute is hidden, range, color, checkbox, radio, file, or a button type (such as button or submit).
        ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
        ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
        ///are also acccepted.
        ///</summary>
        static member readOnly(p: bool) = OAttr.readOnly p |> Attr.Wrap

        ///<summary>
        ///This attribute specifies that the user must fill in a value before submitting a form. It cannot be used when the type attribute is hidden, image, or a button type (submit, reset, or button). The :optional and :required CSS pseudo-classes will be applied to the field as appropriate.
        ///required is an HTML boolean attribute - it is enabled by a boolean or
        ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
        ///are also acccepted.
        ///</summary>
        static member required(p: bool) = OAttr.required p |> Attr.Wrap
        ///<summary>
        ///The direction in which selection occurred. This is "forward" if the selection was made from left-to-right in an LTR locale or right-to-left in an RTL locale, or "backward" if the selection was made in the opposite direction. On platforms on which it's possible this value isn't known, the value can be "none"; for example, on macOS, the default direction is "none", then as the user begins to modify the selection using the keyboard, this will change to reflect the direction in which the selection is expanding.
        ///</summary>
        static member selectionDirection(p: string) = OAttr.selectionDirection p |> Attr.Wrap
        ///<summary>
        ///The offset into the element's text content of the last selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
        ///</summary>
        static member selectionEnd(p: string) = OAttr.selectionEnd p |> Attr.Wrap
        ///<summary>
        ///The offset into the element's text content of the first selected character. If there's no selection, this value indicates the offset to the character following the current text input cursor position (that is, the position the next character typed would occupy).
        ///</summary>
        static member selectionStart(p: string) = OAttr.selectionStart p |> Attr.Wrap
        ///<summary>
        ///The initial size of the control. This value is in pixels unless the value of the type attribute is text or password, in which case it is an integer number of characters. Starting in, this attribute applies only when the type attribute is set to text, search, tel, url, email, or password, otherwise it is ignored. In addition, the size must be greater than zero. If you do not specify a size, a default value of 20 is used.' simply states "the user agent should ensure that at least that many characters are visible", but different characters can have different widths in certain fonts. In some browsers, a certain string with x characters will not be entirely visible even if size is defined to at least x.
        ///</summary>
        static member size(p: string) = OAttr.size p |> Attr.Wrap

        ///<summary>
        ///Setting the value of this attribute to true indicates that the element needs to have its spelling and grammar checked. The value default indicates that the element is to act according to a default behavior, possibly based on the parent element's own spellcheck value. The value false indicates that the element should not be checked.
        ///</summary>
        static member spellCheck(p: bool) = (if p then OAttr.spellCheck Dash.NET.ComponentPropTypes.SpellCheckOptions.True else OAttr.spellCheck Dash.NET.ComponentPropTypes.SpellCheckOptions.False) |> Attr.Wrap
        ///<summary>
        ///Works with the min and max attributes to limit the increments at which a numeric or date-time value can be set. It can be the string any or a positive floating point number. If this attribute is not set to any, the control accepts only values at multiples of the step value greater than the minimum.
        ///</summary>
        static member step(p: IConvertible) = OAttr.step p |> Attr.Wrap
        ///<summary>
        ///Number of times the &#96;Enter&#96; key was pressed while the input had focus.
        ///</summary>
        static member nSubmit(p: IConvertible) = OAttr.nSubmit p |> Attr.Wrap
        ///<summary>
        ///Last time that &#96;Enter&#96; was pressed.
        ///</summary>
        static member nSubmitTimestamp(p: IConvertible) = OAttr.nSubmitTimestamp p |> Attr.Wrap
        ///<summary>
        ///Number of times the input lost focus.
        ///</summary>
        static member nBlur(p: IConvertible) = OAttr.nBlur p |> Attr.Wrap
        ///<summary>
        ///Last time the input lost focus.
        ///</summary>
        static member nBlurTimestamp(p: IConvertible) = OAttr.nBlurTimestamp p |> Attr.Wrap
        ///<summary>
        ///Object that holds the loading state object coming from dash-renderer
        ///</summary>
        static member loadingState(p: LoadingState) = OAttr.loadingState (p |> LoadingState.Convert) |> Attr.Wrap
        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: IConvertible) = OAttr.persistence p |> Attr.Wrap
        ///<summary>
        ///Properties whose user interactions will persist after refreshing the
        ///component or the page. Since only &#96;value&#96; is allowed this prop can
        ///normally be ignored.
        ///</summary>
        static member persistedProps([<ParamArray>] p: string array) = OAttr.persistedProps p |> Attr.Wrap
        ///<summary>
        ///Where persisted user changes will be stored:
        ///memory: only kept in memory, reset on page refresh.
        ///local: window.localStorage, data is kept after the browser quit.
        ///session: window.sessionStorage, data is cleared once the browser quit.
        ///</summary>
        static member persistenceType(p: PersistenceTypeOptions) = OAttr.persistenceType (p |> PersistenceTypeOptions.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: int) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: System.Guid) = OAttr.children value |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Dash.NET.CSharp.Dsl.DashComponent) = OAttr.children (value |> Dash.NET.CSharp.Dsl.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children([<ParamArray>] value: array<Dash.NET.CSharp.Dsl.DashComponent>) = OAttr.children (value |> Array.map Dash.NET.CSharp.Dsl.DashComponent.Unwrap) |> Attr.Wrap
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: seq<Dash.NET.CSharp.Dsl.DashComponent>) = OAttr.children (value |> Seq.map Dash.NET.CSharp.Dsl.DashComponent.Unwrap) |> Attr.Wrap

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
    let input (id: string, [<ParamArray>] attrs: array<Attr>) = Dash.NET.DCC.Input.input id (attrs |> List.ofArray |> List.map Attr.Unwrap) |> Dash.NET.CSharp.Dsl.DashComponent.Wrap