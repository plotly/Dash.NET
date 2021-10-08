namespace Dash.NET.DCC

open System
open DynamicObj
open Dash.NET
open Dash.NET.Common

///<summary>
///A basic HTML textarea for entering multiline text.
///</summary>
[<RequireQualifiedAccess>]
module Textarea =
    type PropName =
        | Value
        | AutoFocus
        | Cols
        | Disabled
        | Form
        | MaxLength
        | MinLength
        | Name
        | Placeholder
        | ReadOnly
        | Required
        | Rows
        | Wrap
        | AccessKey
        | ClassName
        | ContentEditable
        | ContextMenu
        | Dir
        | Draggable
        | Hidden
        | Lang
        | SpellCheck
        | Style
        | TabIndex
        | Title
        | NBlur
        | NBlurTimestamp
        | NClicks
        | NClicksTimestamp
        | LoadingState
        | Persistence
        | PersistedProps
        | PersistenceType

        member this.toString () =
            match this with
            | Value             -> "value"
            | AutoFocus         -> "autoFocus"
            | Cols              -> "cols"
            | Disabled          -> "disabled"
            | Form              -> "form"
            | MaxLength         -> "maxLength"
            | MinLength         -> "minLength"
            | Name              -> "name"
            | Placeholder       -> "placeholder"
            | ReadOnly          -> "readOnly"
            | Required          -> "required"
            | Rows              -> "rows"
            | Wrap              -> "wrap"
            | AccessKey         -> "accessKey"
            | ClassName         -> "className"
            | ContentEditable   -> "contentEditable"
            | ContextMenu       -> "contextMenu"
            | Dir               -> "dir"
            | Draggable         -> "draggable"
            | Hidden            -> "hidden"
            | Lang              -> "lang"
            | SpellCheck        -> "spellCheck"
            | Style             -> "style"
            | TabIndex          -> "tabIndex"
            | Title             -> "title"
            | NBlur             -> "n_blur"
            | NBlurTimestamp    -> "n_blur_timestamp"
            | NClicks           -> "n_clicks"
            | NClicksTimestamp  -> "n_clicks_timestamp"
            | LoadingState      -> "loading_state"
            | Persistence       -> "persistence"
            | PersistedProps    -> "persisted_props"
            | PersistenceType   -> "persistence_type"

    ///<summary>
    ///• value (string) - The value of the textarea
    ///&#10;
    ///• autoFocus (string) - The element should be automatically focused after the page loaded.
    ///&#10;
    ///• cols (string | number) - Defines the number of columns in a textarea.
    ///&#10;
    ///• disabled (string | boolean) - Indicates whether the user can interact with the element.
    ///&#10;
    ///• form (string) - Indicates the form that is the owner of the element.
    ///&#10;
    ///• maxLength (string | number) - Defines the maximum number of characters allowed in the element.
    ///&#10;
    ///• minLength (string | number) - Defines the minimum number of characters allowed in the element.
    ///&#10;
    ///• name (string) - Name of the element. For example used by the server to identify the fields in form submits.
    ///&#10;
    ///• placeholder (string) - Provides a hint to the user of what can be entered in the field.
    ///&#10;
    ///• readOnly (boolean | value equal to: 'readOnly', 'readonly', 'READONLY') - Indicates whether the element can be edited.
    ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
    ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
    ///are also acccepted.
    ///&#10;
    ///• required (value equal to: 'required', 'REQUIRED' | boolean) - Indicates whether this element is required to fill out or not.
    ///required is an HTML boolean attribute - it is enabled by a boolean or
    ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
    ///are also acccepted.
    ///&#10;
    ///• rows (string | number) - Defines the number of rows in a text area.
    ///&#10;
    ///• wrap (string) - Indicates whether the text should be wrapped.
    ///&#10;
    ///• accessKey (string) - Defines a keyboard shortcut to activate or add focus to the element.
    ///&#10;
    ///• className (string) - Often used with CSS to style elements with common properties.
    ///&#10;
    ///• contentEditable (string | boolean) - Indicates whether the element's content is editable.
    ///&#10;
    ///• contextMenu (string) - Defines the ID of a &lt;menu&gt; element which will serve as the element's context menu.
    ///&#10;
    ///• dir (string) - Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left)
    ///&#10;
    ///• draggable (value equal to: 'true', 'false' | boolean) - Defines whether the element can be dragged.
    ///&#10;
    ///• hidden (string) - Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
    ///&#10;
    ///• lang (string) - Defines the language used in the element.
    ///&#10;
    ///• spellCheck (value equal to: 'true', 'false' | boolean) - Indicates whether spell checking is allowed for the element.
    ///&#10;
    ///• style (record) - Defines CSS styles which will override styles previously set.
    ///&#10;
    ///• tabIndex (string | number) - Overrides the browser's default tab order and follows the one specified instead.
    ///&#10;
    ///• title (string) - Text to be displayed in a tooltip when hovering over the element.
    ///&#10;
    ///• n_blur (number; default 0) - Number of times the textarea lost focus.
    ///&#10;
    ///• n_blur_timestamp (number; default -1) - Last time the textarea lost focus.
    ///&#10;
    ///• n_clicks (number; default 0) - Number of times the textarea has been clicked.
    ///&#10;
    ///• n_clicks_timestamp (number; default -1) - Last time the textarea was clicked.
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
        | Value of string
        | AutoFocus of string
        | Cols of int
        | Disabled of bool
        | Form of string
        | MaxLength of int
        | MinLength of int
        | Name of string
        | Placeholder of string
        | ReadOnly of bool
        | Required of bool
        | Rows of int
        | Wrap of string
        | AccessKey of string
        | ClassName of string
        | ContentEditable of bool
        | ContextMenu of string
        | Dir of string
        | Draggable of bool
        | Hidden of string
        | Lang of string
        | SpellCheck of bool
        | Style of obj
        | TabIndex of IConvertible
        | Title of string
        | NBlur of IConvertible
        | NBlurTimestamp of IConvertible
        | NClicks of IConvertible
        | NClicksTimestamp of IConvertible
        | LoadingState of LoadingState
        | Persistence of IConvertible
        | PersistedProps of string []
        | PersistenceType of PersistenceTypeOptions

        static member convert = function
            | Value             p -> box p
            | AutoFocus         p -> box p
            | Cols              p -> box p
            | Disabled          p -> box p
            | Form              p -> box p
            | MaxLength         p -> box p
            | MinLength         p -> box p
            | Name              p -> box p
            | Placeholder       p -> box p
            | ReadOnly          p -> box p
            | Required          p -> box p
            | Rows              p -> box p
            | Wrap              p -> box p
            | AccessKey         p -> box p
            | ClassName         p -> box p
            | ContentEditable   p -> box p
            | ContextMenu       p -> box p
            | Dir               p -> box p
            | Draggable         p -> box p
            | Hidden            p -> box p
            | Lang              p -> box p
            | SpellCheck        p -> box p
            | Style             p -> box p
            | TabIndex          p -> box p
            | Title             p -> box p
            | NBlur             p -> box p
            | NBlurTimestamp    p -> box p
            | NClicks           p -> box p
            | NClicksTimestamp  p -> box p
            | LoadingState      p -> box p
            | Persistence       p -> box p
            | PersistedProps    p -> box p
            | PersistenceType   p -> PersistenceTypeOptions.convert p

        static member toPropName = function
            | Value             _ -> PropName.Value
            | AutoFocus         _ -> PropName.AutoFocus
            | Cols              _ -> PropName.Cols
            | Disabled          _ -> PropName.Disabled
            | Form              _ -> PropName.Form
            | MaxLength         _ -> PropName.MaxLength
            | MinLength         _ -> PropName.MinLength
            | Name              _ -> PropName.Name
            | Placeholder       _ -> PropName.Placeholder
            | ReadOnly          _ -> PropName.ReadOnly
            | Required          _ -> PropName.Required
            | Rows              _ -> PropName.Rows
            | Wrap              _ -> PropName.Wrap
            | AccessKey         _ -> PropName.AccessKey
            | ClassName         _ -> PropName.ClassName
            | ContentEditable   _ -> PropName.ContentEditable
            | ContextMenu       _ -> PropName.ContextMenu
            | Dir               _ -> PropName.Dir
            | Draggable         _ -> PropName.Draggable
            | Hidden            _ -> PropName.Hidden
            | Lang              _ -> PropName.Lang
            | SpellCheck        _ -> PropName.SpellCheck
            | Style             _ -> PropName.Style
            | TabIndex          _ -> PropName.TabIndex
            | Title             _ -> PropName.Title
            | NBlur             _ -> PropName.NBlur
            | NBlurTimestamp    _ -> PropName.NBlurTimestamp
            | NClicks           _ -> PropName.NClicks
            | NClicksTimestamp  _ -> PropName.NClicksTimestamp
            | LoadingState      _ -> PropName.LoadingState
            | Persistence       _ -> PropName.Persistence
            | PersistedProps    _ -> PropName.PersistedProps
            | PersistenceType   _ -> PropName.PersistenceType

        static member toDynamicMemberDef prop =
            prop |> Prop.toPropName |> fun cp -> cp.toString()
            , Prop.convert prop

    ///<summary>
    ///A list of children or a property for this dash component
    ///</summary>
    type Attr =
        | Prop of Prop
        | Children of list<DashComponent>
        ///<summary>
        ///The value of the textarea
        ///</summary>
        static member value(p: string) = Prop(Value p)
        ///<summary>
        ///The element should be automatically focused after the page loaded.
        ///</summary>
        static member autoFocus(p: string) = Prop(AutoFocus p)
        ///<summary>
        ///Defines the number of columns in a textarea.
        ///</summary>
        static member cols(p: int) = Prop(Cols p)
        ///<summary>
        ///Indicates whether the user can interact with the element.
        ///</summary>
        static member disabled(p: bool) = Prop(Disabled p)
        ///<summary>
        ///Indicates the form that is the owner of the element.
        ///</summary>
        static member form(p: string) = Prop(Form p)
        ///<summary>
        ///Defines the maximum number of characters allowed in the element.
        ///</summary>
        static member maxLength(p: int) = Prop(MaxLength p)
        ///<summary>
        ///Defines the minimum number of characters allowed in the element.
        ///</summary>
        static member minLength(p: int) = Prop(MinLength p)
        ///<summary>
        ///Name of the element. For example used by the server to identify the fields in form submits.
        ///</summary>
        static member name(p: string) = Prop(Name p)
        ///<summary>
        ///Provides a hint to the user of what can be entered in the field.
        ///</summary>
        static member placeholder(p: string) = Prop(Placeholder p)
        ///<summary>
        ///Indicates whether the element can be edited.
        ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
        ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
        ///are also acccepted.
        ///</summary>
        static member readOnly(p: bool) = Prop(ReadOnly p)
        ///<summary>
        ///Indicates whether this element is required to fill out or not.
        ///required is an HTML boolean attribute - it is enabled by a boolean or
        ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
        ///are also acccepted.
        ///</summary>
        static member required(p: bool) = Prop(Required p)
        ///<summary>
        ///Defines the number of rows in a text area.
        ///</summary>
        static member rows(p: int) = Prop(Rows p)
        ///<summary>
        ///Indicates whether the text should be wrapped.
        ///</summary>
        static member wrap(p: string) = Prop(Wrap p)
        ///<summary>
        ///Defines a keyboard shortcut to activate or add focus to the element.
        ///</summary>
        static member accessKey(p: string) = Prop(AccessKey p)
        ///<summary>
        ///Often used with CSS to style elements with common properties.
        ///</summary>
        static member className(p: string) = Prop(ClassName p)
        ///<summary>
        ///Indicates whether the element's content is editable.
        ///</summary>
        static member contentEditable(p: bool) = Prop(ContentEditable p)
        ///<summary>
        ///Defines the ID of a &lt;menu&gt; element which will serve as the element's context menu.
        ///</summary>
        static member contextMenu(p: string) = Prop(ContextMenu p)
        ///<summary>
        ///Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left)
        ///</summary>
        static member dir(p: string) = Prop(Dir p)
        ///<summary>
        ///Defines whether the element can be dragged.
        ///</summary>
        static member draggable(p: bool) = Prop(Draggable p)
        ///<summary>
        ///Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
        ///</summary>
        static member hidden(p: string) = Prop(Hidden p)
        ///<summary>
        ///Defines the language used in the element.
        ///</summary>
        static member lang(p: string) = Prop(Lang p)
        ///<summary>
        ///Indicates whether spell checking is allowed for the element.
        ///</summary>
        static member spellCheck(p: bool) = Prop(SpellCheck p)
        ///<summary>
        ///Defines CSS styles which will override styles previously set.
        ///</summary>
        static member style(p: seq<Css.Style>) = Prop(Style(DashComponentStyle.fromCssStyle p))
        ///<summary>
        ///Overrides the browser's default tab order and follows the one specified instead.
        ///</summary>
        static member tabIndex(p: string) = Prop(TabIndex (p :> IConvertible))
        ///<summary>
        ///Overrides the browser's default tab order and follows the one specified instead.
        ///</summary>
        static member tabIndex(p: IConvertible) = Prop(TabIndex p)
        ///<summary>
        ///Text to be displayed in a tooltip when hovering over the element.
        ///</summary>
        static member title(p: string) = Prop(Title p)
        ///<summary>
        ///Number of times the textarea lost focus.
        ///</summary>
        static member nBlur(p: IConvertible) = Prop(NBlur p)
        ///<summary>
        ///Last time the textarea lost focus.
        ///</summary>
        static member nBlurTimestamp(p: IConvertible) = Prop(NBlurTimestamp p)
        ///<summary>
        ///Number of times the textarea has been clicked.
        ///</summary>
        static member nClicks(p: IConvertible) = Prop(NClicks p)
        ///<summary>
        ///Last time the textarea was clicked.
        ///</summary>
        static member nClicksTimestamp(p: IConvertible) = Prop(NClicksTimestamp p)
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
        static member persistence(p: bool) = Prop(Persistence p)

        ///<summary>
        ///Used to allow user interactions in this component to be persisted when
        ///the component - or the page - is refreshed. If &#96;persisted&#96; is truthy and
        ///hasn't changed from its previous value, a &#96;value&#96; that the user has
        ///changed while using the app will keep that change, as long as
        ///the new &#96;value&#96; also matches what was given originally.
        ///Used in conjunction with &#96;persistence_type&#96;.
        ///</summary>
        static member persistence(p: string) = Prop(Persistence p)

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
        static member children(value: int) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: string) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: float) = Children([ Html.text value ])
        ///<summary>
        ///The child or children of this dash component
        ///</summary>
        static member children(value: Guid) = Children([ Html.text value ])
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
    ///A basic HTML textarea for entering multiline text.
    ///</summary>
    type Textarea() =
        inherit DashComponent()
        static member applyMembers
            (
                id: string,
                children: seq<DashComponent>,
                ?value,
                ?autoFocus,
                ?cols,
                ?disabled,
                ?form,
                ?maxLength,
                ?minLength,
                ?name,
                ?placeholder,
                ?readOnly,
                ?required,
                ?rows,
                ?wrap,
                ?accessKey,
                ?className,
                ?contentEditable,
                ?contextMenu,
                ?dir,
                ?draggable,
                ?hidden,
                ?lang,
                ?spellCheck,
                ?style,
                ?tabIndex,
                ?title,
                ?nBlur,
                ?nBlurTimestamp,
                ?nClicks,
                ?nClicksTimestamp,
                ?loadingState,
                ?persistence,
                ?persistedProps,
                ?persistenceType
            ) =
            (fun (t: Textarea) ->
                let props = DashComponentProps()
                let setPropValueOpt prop =
                    DynObj.setPropValueOpt props Prop.convert prop

                DynObj.setValue props "id" id
                DynObj.setValue props "children" children
                setPropValueOpt Value value
                setPropValueOpt AutoFocus autoFocus
                setPropValueOpt Cols cols
                setPropValueOpt Disabled disabled
                setPropValueOpt Form form
                setPropValueOpt MaxLength maxLength
                setPropValueOpt MinLength minLength
                setPropValueOpt Name name
                setPropValueOpt Placeholder placeholder
                setPropValueOpt ReadOnly readOnly
                setPropValueOpt Required required
                setPropValueOpt Rows rows
                setPropValueOpt Wrap wrap
                setPropValueOpt AccessKey accessKey
                setPropValueOpt ClassName className
                setPropValueOpt ContentEditable contentEditable
                setPropValueOpt ContextMenu contextMenu
                setPropValueOpt Dir dir
                setPropValueOpt Draggable draggable
                setPropValueOpt Hidden hidden
                setPropValueOpt Lang lang
                setPropValueOpt SpellCheck spellCheck
                setPropValueOpt Style style
                setPropValueOpt TabIndex tabIndex
                setPropValueOpt Title title
                setPropValueOpt NBlur nBlur
                setPropValueOpt NBlurTimestamp nBlurTimestamp
                setPropValueOpt NClicks nClicks
                setPropValueOpt NClicksTimestamp nClicksTimestamp
                setPropValueOpt LoadingState loadingState
                setPropValueOpt Persistence persistence
                setPropValueOpt PersistedProps persistedProps
                setPropValueOpt PersistenceType persistenceType
                DynObj.setValue t "namespace" "dash_core_components"
                DynObj.setValue t "props" props
                DynObj.setValue t "type" "Textarea"
                t)

        static member init
            (
                id,
                children,
                ?value,
                ?autoFocus,
                ?cols,
                ?disabled,
                ?form,
                ?maxLength,
                ?minLength,
                ?name,
                ?placeholder,
                ?readOnly,
                ?required,
                ?rows,
                ?wrap,
                ?accessKey,
                ?className,
                ?contentEditable,
                ?contextMenu,
                ?dir,
                ?draggable,
                ?hidden,
                ?lang,
                ?spellCheck,
                ?style,
                ?tabIndex,
                ?title,
                ?nBlur,
                ?nBlurTimestamp,
                ?nClicks,
                ?nClicksTimestamp,
                ?loadingState,
                ?persistence,
                ?persistedProps,
                ?persistenceType
            ) =
            Textarea.applyMembers
                (id,
                 children,
                 ?value = value,
                 ?autoFocus = autoFocus,
                 ?cols = cols,
                 ?disabled = disabled,
                 ?form = form,
                 ?maxLength = maxLength,
                 ?minLength = minLength,
                 ?name = name,
                 ?placeholder = placeholder,
                 ?readOnly = readOnly,
                 ?required = required,
                 ?rows = rows,
                 ?wrap = wrap,
                 ?accessKey = accessKey,
                 ?className = className,
                 ?contentEditable = contentEditable,
                 ?contextMenu = contextMenu,
                 ?dir = dir,
                 ?draggable = draggable,
                 ?hidden = hidden,
                 ?lang = lang,
                 ?spellCheck = spellCheck,
                 ?style = style,
                 ?tabIndex = tabIndex,
                 ?title = title,
                 ?nBlur = nBlur,
                 ?nBlurTimestamp = nBlurTimestamp,
                 ?nClicks = nClicks,
                 ?nClicksTimestamp = nClicksTimestamp,
                 ?loadingState = loadingState,
                 ?persistence = persistence,
                 ?persistedProps = persistedProps,
                 ?persistenceType = persistenceType)
                (Textarea())

    ///<summary>
    ///A basic HTML textarea for entering multiline text.
    ///&#10;
    ///Properties:
    ///&#10;
    ///• id (string) - The ID of this component, used to identify dash components
    ///in callbacks. The ID needs to be unique across all of the
    ///components in an app.
    ///&#10;
    ///• value (string) - The value of the textarea
    ///&#10;
    ///• autoFocus (string) - The element should be automatically focused after the page loaded.
    ///&#10;
    ///• cols (string | number) - Defines the number of columns in a textarea.
    ///&#10;
    ///• disabled (string | boolean) - Indicates whether the user can interact with the element.
    ///&#10;
    ///• form (string) - Indicates the form that is the owner of the element.
    ///&#10;
    ///• maxLength (string | number) - Defines the maximum number of characters allowed in the element.
    ///&#10;
    ///• minLength (string | number) - Defines the minimum number of characters allowed in the element.
    ///&#10;
    ///• name (string) - Name of the element. For example used by the server to identify the fields in form submits.
    ///&#10;
    ///• placeholder (string) - Provides a hint to the user of what can be entered in the field.
    ///&#10;
    ///• readOnly (boolean | value equal to: 'readOnly', 'readonly', 'READONLY') - Indicates whether the element can be edited.
    ///readOnly is an HTML boolean attribute - it is enabled by a boolean or
    ///'readOnly'. Alternative capitalizations &#96;readonly&#96; &amp; &#96;READONLY&#96;
    ///are also acccepted.
    ///&#10;
    ///• required (value equal to: 'required', 'REQUIRED' | boolean) - Indicates whether this element is required to fill out or not.
    ///required is an HTML boolean attribute - it is enabled by a boolean or
    ///'required'. Alternative capitalizations &#96;REQUIRED&#96;
    ///are also acccepted.
    ///&#10;
    ///• rows (string | number) - Defines the number of rows in a text area.
    ///&#10;
    ///• wrap (string) - Indicates whether the text should be wrapped.
    ///&#10;
    ///• accessKey (string) - Defines a keyboard shortcut to activate or add focus to the element.
    ///&#10;
    ///• className (string) - Often used with CSS to style elements with common properties.
    ///&#10;
    ///• contentEditable (string | boolean) - Indicates whether the element's content is editable.
    ///&#10;
    ///• contextMenu (string) - Defines the ID of a &lt;menu&gt; element which will serve as the element's context menu.
    ///&#10;
    ///• dir (string) - Defines the text direction. Allowed values are ltr (Left-To-Right) or rtl (Right-To-Left)
    ///&#10;
    ///• draggable (value equal to: 'true', 'false' | boolean) - Defines whether the element can be dragged.
    ///&#10;
    ///• hidden (string) - Prevents rendering of given element, while keeping child elements, e.g. script elements, active.
    ///&#10;
    ///• lang (string) - Defines the language used in the element.
    ///&#10;
    ///• spellCheck (value equal to: 'true', 'false' | boolean) - Indicates whether spell checking is allowed for the element.
    ///&#10;
    ///• style (record) - Defines CSS styles which will override styles previously set.
    ///&#10;
    ///• tabIndex (string | number) - Overrides the browser's default tab order and follows the one specified instead.
    ///&#10;
    ///• title (string) - Text to be displayed in a tooltip when hovering over the element.
    ///&#10;
    ///• n_blur (number; default 0) - Number of times the textarea lost focus.
    ///&#10;
    ///• n_blur_timestamp (number; default -1) - Last time the textarea lost focus.
    ///&#10;
    ///• n_clicks (number; default 0) - Number of times the textarea has been clicked.
    ///&#10;
    ///• n_clicks_timestamp (number; default -1) - Last time the textarea was clicked.
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
    let textarea (id: string) (attrs: list<Attr>) =
        let props, children =
            List.fold
                (fun (props, children) (a: Attr) ->
                    match a with
                    | Prop (prop) -> prop :: props, children
                    | Children (child) -> props, child @ children)
                ([], [])
                attrs

        let t = Textarea.init (id, children)

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
