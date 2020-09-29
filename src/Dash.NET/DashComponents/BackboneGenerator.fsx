open System
open System.IO

let htmlBackbone = File.ReadAllText (__SOURCE_DIRECTORY__ + "/htmlComponentBackbone.template")
let componentBackbone = File.ReadAllText (__SOURCE_DIRECTORY__ + "/componentBackbone.template")

[<AutoOpen>]
module Keywords =
    ///https://docs.microsoft.com/de-de/dotnet/fsharp/language-reference/keyword-reference
    let keywordsAndReserved = 
        set [
            "abstract"
            "and"
            "let"
            "as"
            "assert"
            "base"
            "begin"
            "class"
            "default"
            "delegate"
            "do"
            "done"
            "downcast"
            "downto"
            "elif"
            "else"
            "end"
            "exception"
            "extern"
            "false"
            "finally"
            "fixed"
            "for"
            "fun"
            "function"
            "global"
            "if"
            "in"
            "inherit"
            "inline"
            "interface"
            "internal"
            "lazy"
            "let"
            "let!"
            "match"
            "match!"
            "member"
            "module"
            "mutable"
            "namespace"
            "new"
            "null"
            "of"
            "open"
            "or"
            "override"
            "private"
            "public"
            "rec"
            "return"
            "return!"
            "static"
            "struct"
            "then"
            "to"
            "true"
            "try"
            "type"
            "upcast"
            "use"
            "use!"
            "val"
            "void"
            "when"
            "while"
            "with"
            //reserved due to being kw in OCaml
            "asr"
            "land"
            "lor"
            "lsl"
            "lsr"
            "lxor"
            "mod"
            "sig"
            //reserved for future use
            "atomic"
            "break"
            "checked"
            "component"
            "const"
            "constraint"
            "constructor"
            "continue"
            "eager"
            "event"
            "external"
            "functor"
            "include"
            "method"
            "mixin"
            "object"
            "parallel"
            "process"
            "protected"
            "pure"
            "sealed"
            "tailcall"
            "trait"
            "virtual"
            "volatile"
        ]

    let preventKWUsage (s:string) =
        if keywordsAndReserved.Contains(s) then
            sprintf "_%s" s
        else
            s

module String =
    let replace (old:string) (_new:string) (s:string) = s.Replace(old,_new)
    let write path (s:string) = File.WriteAllText(path,s)

type ComponentType =
    | HTMLComponent
    | DashComponent
    static member toBackbone = function
        | HTMLComponent -> htmlBackbone
        | DashComponent -> componentBackbone

type ComponentParameters = 
    {
        Type: ComponentType
        ComponentName:             string
        CamelCaseComponentName:    string
        ComponentChar:             string
        ComponentNamespace:        string
        ComponentType:             string
        LibraryNamespace:          string
    }
    static member create (_type:ComponentType) (componentName:string) (componentNameSpace:string) (componentType:string) (libraryNameSpace:string) =
        {
            Type=_type
            ComponentName               = preventKWUsage componentName
            CamelCaseComponentName      = preventKWUsage (sprintf "%c%s" (Char.ToLowerInvariant(componentName.[0])) (componentName.Substring(1)))
            ComponentChar               = preventKWUsage (componentName.ToLower().Substring(0,1))
            ComponentNamespace          = preventKWUsage componentNameSpace
            ComponentType               = preventKWUsage componentType
            LibraryNamespace            = preventKWUsage libraryNameSpace
    }


let generateComponentTemplateFile path (parameters:ComponentParameters) =
    parameters.Type
    |> ComponentType.toBackbone
    |> String.replace "{ComponentName}"             parameters.ComponentName
    |> String.replace "{camelCaseComponentName}"    parameters.CamelCaseComponentName
    |> String.replace "{ComponentChar}"             parameters.ComponentChar
    |> String.replace "{ComponentNamespace}"        parameters.ComponentNamespace
    |> String.replace "{ComponentType}"             parameters.ComponentType
    |> String.replace "{LibraryNamespace}"          parameters.LibraryNamespace
    |> String.write path 

let htmlComponents =
    [
        "A"
        "Abbr"
        "Acronym"
        "Address"
        "Area"
        "Article"
        "Aside"
        "Audio"
        "B"
        "Base"
        "Basefont"
        "Bdi"
        "Bdo"
        "Big"
        "Blink"
        "Blockquote"
        "Br"
        "Button"
        "Canvas"
        "Caption"
        "Center"
        "Cite"
        "Code"
        "Col"
        "Colgroup"
        "Command"
        "Content"
        "Data"
        "Datalist"
        "Dd"
        "Del"
        "Details"
        "Dfn"
        "Dialog"
        "Div"
        "Dl"
        "Dt"
        "Element"
        "Em"
        "Embed"
        "Fieldset"
        "Figcaption"
        "Figure"
        "Font"
        "Footer"
        "Form"
        "Frame"
        "Frameset"
        "H1"
        "H2"
        "H3"
        "H4"
        "H5"
        "H6"
        "Header"
        "Hgroup"
        "Hr"
        "I"
        "Iframe"
        "Img"
        "Ins"
        "Isindex"
        "Kbd"
        "Keygen"
        "Label"
        "Legend"
        "Li"
        "Link"
        "Listing"
        "Main"
        "MapEl"
        "Mark"
        "Marquee"
        "Meta"
        "Meter"
        "Multicol"
        "Nav"
        "Nextid"
        "Nobr"
        "Noscript"
        "ObjectEl"
        "Ol"
        "Optgroup"
        "Option"
        "Output"
        "P"
        "Param"
        "Picture"
        "Plaintext"
        "Pre"
        "Progress"
        "Q"
        "Rb"
        "Rp"
        "Rt"
        "Rtc"
        "Ruby"
        "S"
        "Samp"
        "Script"
        "Section"
        "Select"
        "Shadow"
        "Slot"
        "Small"
        "Source"
        "Spacer"
        "Span"
        "Strike"
        "Strong"
        "Sub"
        "Summary"
        "Sup"
        "Table"
        "Tbody"
        "Td"
        "Template"
        "Textarea"
        "Tfoot"
        "Th"
        "Thead"
        "Time"
        "Title"
        "Tr"
        "Track"
        "U"
        "Ul"
        "Var"
        "Video"
        "Wbr"
        "Xmp"
    ]
    |> List.map (fun cName ->
        ComponentParameters.create 
            ComponentType.HTMLComponent 
            cName
            "dash_html_components" 
            cName
            "Dash.NET.HTML" 
    )

htmlComponents
|> List.iter (fun (_component:ComponentParameters) ->
    _component |> generateComponentTemplateFile (__SOURCE_DIRECTORY__ + (sprintf "/HTMLComponents/%s.fs" _component.ComponentName))
)
