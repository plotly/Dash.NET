# How to use the tool
```
USAGE: Dash.NET.ComponentGeneration.exe [--help] --name <name> --shortname <name> --componentdirectory <folder>
                                        --description <description> --author <name> [--metadata <meta>]
                                        [--version <version>] [--dashversion <version>] [--outputdirectory <folder>]
                                        [--addfile <source>] [--ignore <regex>] [--disabledefaultignore]
                                        [--publishtonuget <apiKey>] [--verbose]

REQUIRED:
    --name, -n <name>                       Name of the component or group of components, required
    --shortname, -s <name>                  Name of the exported javascript namespace of the component, required
    --componentdirectory, -d <folder>       Folder containing the component, required
    --description, -e <description>         A short description of the component, required
    --author, -a <name>                     Author of the component, there can be more than one of these, at least one required

OPTIONS:
    --metadata, -m <meta>                   React docgen metadata.json file, defaults to '<component_folder>/metadata.json'
    --version, -v <version>                 The version of the compenent, defaults to 1.0.0
    --dashversion, -vd <version>            The version of Dash.NET to use, defaults to '0.1.0-alpha9'
    --outputdirectory, -o <folder>          Directory to create the F# project folder in, defaults to ./
    --addfile, -f <source>                  Additional local source file to include, there can be more than one of these, 
                                            defaults to none
    --ignore, -i <regex>                    Ignore folders and file paths that match this regex, by default this includes
                                            "__pycache__" and ".*\.py", there can be more than one of these, defaults to none
    --disabledefaultignore, -ddi            Don't ignore "__pycache__" and ".*\.py" by default
    --publishtonuget, -p <apiKey>           Publish this package straight to nuget with this API key, defaults to not publishing. Can
                                            be published later using "dotnet nuget push <path-to-component>/bin/Release/<component-name>.<component-version>.nupkg 
                                            --api-key <api-key> --source https://api.nuget.org/v3/index.json"
    --verbose                               Print all logs
    --help                                  display a list of options.
```


# Basics of AST (Abstract Syntax Tree) based code generation
This is a basic overview of AST based code generation. It covers the most commonly used
concepts in F# syntax and how they are generated using the tools in the `FSharp.Compiler`
and `FsAst` libraries, as well as a handful of useful-to-know quirks of the system.

`FsAst` provides useful abstractions for the built-in `FSharp.Compiler` AST elements. 
`FsAst.Rcd` defines record types that correspond with the classes in `FSharp.Compiler` 
and can be easily converted back and forth, allowing for much more F# friendly construction.
Additionally `FsAst.Create` provides default constructors for the most common use cases of
these records, making things even easier.

Most of the examples given in this document will be using the convenient default constructors
provided by `FsAst.Create`. However not all elements of the AST are supported by this, only
the most common ones. For the elements that don't have default constructors you will have to 
instantiate the records manually. In some very rare cases `FsAst.Rcd` wont provide a record
for an element either, in those cases you have to create it through the classes provided by
`FSharp.Compiler`.

**A useful note**: It is possible to create invalid F# code through the AST. Certain things
not all pattern types working in all cases where patterns can be used, or things not
automatically adding parentheses when they would be required for the parser are things you
need to be carful about when creating an AST.

# Concepts / Terms
## Identifier
An identifier is a name used to refer to any type of variable/function/module/type/etc
in F#. For example:

```fsharp
let foo: string = "Hello World"
```

The identifier for this variable would just be `foo`. The identifier is also used to refer
to the variable later.

```fsharp
let foo = bar
```

In this case both `foo` and `bar` are identifiers.

There are 3 different types of identifier when using `FsAst`, and which one you use depends 
on where in the AST the identifier is used. 

1. `Ident` - A simple identifier, like the one above. Can just be created with `Ident.Create`.
2. `LongIdent` - Multiple identifiers connected by dots to refer to a nested entity. The type is just an alias for `Ident list`. Can also be created with `Ident.CreateLong`
3. `LongIdentWithDots` - A `LongIdent` with an optional dot at the end. Created with `LongIdentWithDots.Create` (There are also a couple of other constructors in `LongIdentWithDots` that take differently typed arguments for convenience)

The easiest way to know which one to use it to just look at 
the signature of whatever you are passing it in to.

## Pattern
A pattern is most commonly used to specify a variable [Identifier](#identifier) and type.
Take for example:

```fsharp
let foo: string = "Hello World"
```

In this case the pattern is "`foo: string`". 

Patterns can be nested to identify functions and arguments:

```fsharp
let foo (bar: int) (baz: string) = ...
```

In this case "`bar: int`" and "`baz: string`" are both patterns, but the whole 
"`foo (bar: int) (baz: string)`" is also a pattern.

Patterns are also used to transform or match on data. Examples of different type of 
transforming patterns:

```fsharp
// Cons pattern - x::xs
let x::xs = [1; 2; 3]
// Tuple pattern - (a, b)
let (a, b) = ("hello","world")

match n with 
// List pattern - []
| [] -> 0
// List pattern - [foo] (note: the "when" expression is not part of the pattern)
| [foo] when foo = 0 -> 1
// Wildcard pattern - _
| _ -> 2
```

A list of the types of patterns can be found [here](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/pattern-matching)

Most patterns are defined additively. The base of a pattern is the wildcard pattern, defined 
in `FsAst` with `SynPatRcd.CreateWild`. The wildcard pattern by itself will just be written
as "`_`".

From there you can specify an identifier with `SynPatRcd.CreateNamed`, a type with
`SynPatRcd.CreateTyped`, etc. You can then combine these with a function identifier to create 
a function pattern with `SynPatRcd.CreateLongIdent`, adding parentheses with `SynPatRcd.CreateParen`. An example:

```fsharp
// foo: string
let named = SynPatRcd.CreateNamed(Ident.Create "foo", SynPatRcd.CreateWild)
let typed = SynPatRcd.CreateTyped(named, SynType.Create "string")
```

Not all pattern types have a nice constructor in `FsAst` yet, in those cases you can create 
the pattern record manually. The record names all follow the pattern "`SynPat<Name>Rcd`" and
are converted to a `SynPatRcd` with the corresponding union case. For example:

```fsharp
// ?foo: string (used for optional parameters in class methods)
let optional = SynPatRcd.OptionalVal {Id = Ident.Create "foo"; Range = range.Zero}
let typed = SynPatRcd.CreateTyped(named, SynType.Create "string")
```

This is also an example of an uncommon type of pattern that does not build off of the 
wildcard pattern.

**Note**: For `Range = range.Zero`, range doesn't really matter when converting an AST to
code with Fantomas for code generation, so it is safe to always set it to `range.Zero`.

There is also a different kind of pattern called a "Simple Pattern", most commonly used in 
lambda expressions. It is largely the same as a normal pattern but only supports the `ident:
type` pattern type. It is constructed like:

```fsharp
SynSimplePat.CreateTyped (Ident.Create "x", SynType.Create "string"))
```

## Expression
Expressions are the core of functionality in F#. Anything that provides, acts on or otherwise
transforms data ([Patterns](#pattern) not included) are expressions. For example:

```fsharp
let foo: string = "Hello World"
```

The expression in this statement is `"Hello World"`. 

**Note**: If the `let` is within an expression then *technically* the entire statement is an 
expression, however this is not the case if the let is a module declaration or in a type 
declaration. See the corresponding sections of [Structure of An AST](#structure-of-an-ast) 
for the different versions of `let`.

There are too many types of expressions to cover in this document, so here are some notable 
examples of their construction:

```fsharp
// ()
SynExpr.CreateUnit

// foo
SynExpr.CreateIdentString "foo"

// "bar"
SynExpr.CreateConstString "bar"

// foo "bar"
SynExpr.CreateApp(SynExpr.CreateIdentString "foo", SynExpr.CreateConstString "bar")

// (fun (x: string) -> x)
let argPat = SynSimplePat.CreateTyped (Ident.Create "x", SynType.Create "string"))
let args = SynSimplePats.SimplePats ([argPat], range.Zero) // wrapper for a list of argument patterns
let expr = SynExpr.CreateIdentString "x"
SynExpr.Lambda (false, false, args, expr, None, range.Zero)

// foo()
// bar()
let foo = SynExpr.CreateApp(SynExpr.CreateIdentString "foo", SynExpr.CreateUnit)
let bar = SynExpr.CreateApp(SynExpr.CreateIdentString "bar", SynExpr.CreateUnit)
SynExpr.Sequential (DebugPointAtSequential.Both, false, foo, bar, range.Zero)

// let x = "Hello World"
// x
let returnX = SynExpr.CreateIdentString "x"
let binding =
    { SynBindingRcd.Let with
        Pattern = SynPatRcd.CreateNamed(Ident.Create "x", SynPatRcd.CreateWild)
        Expr = SynExpr.CreateConstString "Hello World" }
SynExpr.LetOrUse (false, false, [ binding.FromRcd ], nextExpr, range.Zero )
// interestingly because a let can't be the last expression in a sequence it has to be defined
// with a reference to the next expression in the sequence.
// .FromRcd is needed because this function takes the FSharp.Compiler version of binding, not
// the FsAst one, so it has to be converted.
 
// match foo with 
// | null -> true
// | _ -> false
let clauses =
  [ SynMatchClause.Clause 
      ( (SynPatRcd.CreateNull).FromRcd, 
        None, 
        SynExpr.CreateIdentString "true", 
        range.Zero,
         DebugPointForTarget.No )
    SynMatchClause.Clause 
      ( (SynPatRcd.CreateWild).FromRcd,
        None, 
        SynExpr.CreateIdentString "false", 
        range.Zero, 
        DebugPointForTarget.No ) ]
SynExpr.CreateMatch (SynExpr.CreateIdentString "foo", clauses)
```

## Binding
A binding is an association between a [Pattern](#pattern) and an [Expression](#expression). For example:

```fsharp
let foo: string = "Hello World"
```
the binding would be `foo: string = "Hello World"`.

`FsAst` provides a binding with empty defaults that you can then modify using a record `with`.
The most common binding definitions will look something like:

```fsharp
// ///Some documentation string
// let _ = ()
{ SynBindingRcd.Let with //Defaulted binding
    XmlDoc = PreXmlDoc.Create "Some documentation string" 
    Pattern = SynPatRcd.CreateWild
    Expr = SynExpr.CreateUnit }
```

## Component Information
Component info is similar to a [Binding](#binding), but for things like type definitions and 
modules. the component info record is where you define the identifier, xml documentation, as 
well as any attributes and generics. An example:

```fsharp
// [<RequireQualifiedAccess>]
// type TypeName() = ...
{ SynComponentInfoRcd.Create (Ident.Create "TypeName") with
    XmlDoc = PreXmlDoc.Create "Some documentation string"
    Attributes = SynAttributeList.Create [SynAttribute.Create "RequireQualifiedAccess"]
```

# Structure of an AST
## Parsed File Input
At the root of the AST is the `ParsedImplFileInputRcd`. The most important Fields of this are
generally going to be the file name and the root namespace/module it contains:

```fsharp
ParsedImplFileInputRcd
    .CreateFs("filename")
    .AddModule(someModuleOrNamespace)
```

However this is also where you can define pragmas and hash directives.

## Namespace Or Module
The namespace or module is the root of the actual code inside the 
[Parsed File Input](#parsed-file-input). This represents the `namespace` or `module` declaration 
at the top of the file.

```fsharp
// namespace Some.Namespace.Identifier
let namespaceOrModule = SynModuleOrNamespaceRcd.CreateNamespace (Ident.CreateLong "Some.Namespace.Identifier")

// module Some.Module.Identifier
let namespaceOrModule = SynModuleOrNamespaceRcd.CreateModule (Ident.CreateLong "Some.Module.Identifier")
```

from there you can add module declarations with:

```fsharp
namespaceOrModule.AddDeclarations [ (*list of module declarations*) ]
```

## Module Declarations
Module declarations are anything set or defined within a module. They are created with the
type `SynModuleDecl`. The following are some of the most common module declarations you will 
need:

### Nested Module
A nested module is how you would create modules within your root 
[Module of Namespace](#module-or-namespace), or how you would create a module within a nested 
module itself.

```fsharp
// module ModuleName =
SynModuleDecl.CreateNestedModule 
  ( SynComponentInfoRcd.Create (Ident.Create "ModuleName"),
    [ (*list of module declarations*) ] )
```

### Open Declaration
An open declaration is how you would open an external library

```fsharp
// open System
SynModuleDecl.CreateOpen "System"
```

### Type Declaration
A normal type declaration is used to create an object class.

```fsharp
// type TypeName =
SynModuleDecl.CreateType
  ( SynComponentInfoRcd.Create (Ident.Create "TypeName"),
    [ (*list of member definitions*) ] )
```

For creating the members of a type see [Member Definitions](#member-definitions)

**Note**: Special types like Records and Discriminated Unions are created with a 
different type of type declaration, see [Simple Type Declaration](#simple-type-declaration)

### Simple Type Declaration
A simple type declaration is used to declare special types like Records and Discriminated Unions.

```fsharp
// type SimpleType =
SynModuleDecl.CreateSimpleType
  ( SynComponentInfoRcd.Create (Ident.Create "SimpleType"),
    SynTypeDefnSimpleReprRcd.None,
    [ (*list of member definitions*) ] )
```

Which type of simple type you create depends on which `SynTypeDefnSimpleReprRcd` case you pass
into it. See [Simple Type Definitions](#simple-type-definitions) for examples.

### Let Declaration
A let declaration lets you define a module constant. `SynModuleDecl.CreateLet` takes in a list
of bindings so you can define multiple at once.

```fsharp
// ///Some documentation string
// let _ = ()
SynModuleDecl.CreateLet
  [ { SynBindingRcd.Let with
        XmlDoc = PreXmlDoc.Create "Some documentation string" 
        Pattern = SynPatRcd.CreateWild
        Expr = SynExpr.CreateUnit } ]
```

## Simple Type Definitions
### Discriminated Union
```fsharp
// type UnionType =
//     | UnionCase of v:string
let unionCases =
  [ SynUnionCaseRcd.Create 
      ( (Ident.Create "UnionCase"), 
        (SynUnionCaseType.Create [ SynFieldRcd.Create(Ident.Create "v", SynType.Create "string") ]) ) ]
    |> SynTypeDefnSimpleReprUnionRcd.Create 

SynModuleDecl.CreateSimpleType
  ( SynComponentInfoRcd.Create (Ident.Create "UnionType"),
    SynTypeDefnSimpleReprRcd.Union unionCases,
    [ (*list of member definitions*) ] )
```
### Record
```fsharp
// type RecordType =
//   { v: string }
let recordFields =
  [ SynFieldRcd.Create(Ident.Create "v", SynType.Create "string") ]
    |> SynTypeDefnSimpleReprRecordRcd.Create

SynModuleDecl.CreateSimpleType
  ( SynComponentInfoRcd.Create (Ident.Create "RecordType"),
    SynTypeDefnSimpleReprRcd.Record recordFields,
    [ (*list of member definitions*) ] )
```

## Member Definitions
### Implicit Constructor
```fsharp
// type TypeName() =
SynModuleDecl.CreateType
  ( SynComponentInfoRcd.Create (Ident.Create "TypeName"),
    [ SynMemberDefn.CreateImplicitCtor() ] )
```

### Type Inheritance
```fsharp
// type TypeName() =
//     inherit OtherType()
let inheritType = 
    (SynExpr.CreateIdentString "OtherType", SynExpr.CreateUnit)
    |> SynExpr.CreateApp
    |> SynType.StaticConstantExpr

SynModuleDecl.CreateType
  ( SynComponentInfoRcd.Create (Ident.Create "TypeName"),
    [ SynMemberDefn.CreateImplicitCtor()
      SynMemberDefn.Inherit (inheritType, range.Zero), None, range.Zero) ] )
```

### Let Declaration
```fsharp
// type TypeName =
//     let v = ()
let binding =
    { SynBindingRcd.Let with
        Pattern = SynPatRcd.CreateNamed(Ident.Create "v", SynPatRcd.CreateWild)
        Expr = SynExpr.CreateUnit }

SynModuleDecl.CreateType
  ( SynComponentInfoRcd.Create (Ident.Create "TypeName"),
    [ SynMemberDefn.LetBindings([binding], false, false, range.Zero) ] )
```

### Member
```fsharp
// type TypeName =
//     static member v = ()
let binding =
    { SynBindingRcd.Let with
        Pattern = SynPatRcd.CreateNamed(Ident.Create "v", SynPatRcd.CreateWild)
        Expr = SynExpr.CreateUnit }

SynModuleDecl.CreateType
  ( SynComponentInfoRcd.Create (Ident.Create "TypeName"),
    [ SynMemberDefn.CreateStaticMember binding ] )
```

# Misc Notes
### Named Parameters in C# style method
Interestingly, when creating an application of a C# style method and you want to use the syntax

```fsharp
SomeObject.SomeMethod
    ( argument1,
      argument2,
      NamedArgument = argument3 )
```

In the assignment of the named parameter (`NamedArgument = argument3`), the `=` is treated like
an infix function application, and the whole line is just an expression:

```fsharp
SynExpr.CreateApp
  ( SynExpr.CreateIdentString "NamedArgument",
    SynExpr.CreateApp
      ( SynExpr.CreateIdentString "=", 
        SynExpr.CreateIdentString "argument3" ) )
```