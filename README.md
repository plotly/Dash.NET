![](docs/img/logo_title.svg)

[![](https://img.shields.io/nuget/vpre/Dash.NET)](https://www.nuget.org/packages/Dash.NET/)

Dash.NET is a .NET interface to [Dash](https://plotly.com/dash/) - the most downloaded framework for building ML &amp; data science web apps - written in F#. Built on top of [Plotly.NET](https://github.com/plotly/Plotly.NET), React and asp.netcore (via Giraffe), Dash.NET ties modern UI elements like dropdowns, sliders, and graphs directly to your analytical .NET code.

This library is under heavy development. Things might break. However, Dash.NET has a stable core and has already been used for non trivial applications ([example1](https://github.com/CSBiology/TMEA), [example2](https://github.com/TRR175/ExploreKinetics)). The current development goal is to implement all targets set in the [beta roadmap](https://github.com/plotly/Dash.NET/issues/4), where you can also see a summary of the state of the project.

The documentation is WIP as well.

<!-- TOC -->

- [Installation](#installation)
- [Documentation](#documentation)
- [Development](#development)
    - [build](#build)
    - [docs](#docs)
    - [release](#release)

<!-- /TOC -->


## Installation

Get the latest preview package via nuget: [![](https://img.shields.io/nuget/vpre/Dash.NET)](https://www.nuget.org/packages/Dash.NET/)

Use the `dotnet new` template: 

`dotnet new -i Dash.NET.Template::*` 

(watch out, this template might not use the latest Dash.NET package, take a look at the referenced version and update if needed )

## Documentation

The landing page of our docs contains everything to get you started fast, check it out [📖 here](http://plotly.github.io/Dash.NET/) 

## Development

_Note:_ The `release` and `prerelease` build targets assume that there is a `NUGET_KEY` environment variable that contains a valid Nuget.org API key.

### build

Check the [build.fsx file](https://github.com/plotly/Dash.NET/blob/dev/build.fsx) to take a look at the  build targets. Here are some examples:

```shell
# Windows

# Build only
./build.cmd

# Full release buildchain: build, test, pack, build the docs, push a git tag, publsih thze nuget package, release the docs
./build.cmd -t release

# The same for prerelease versions:
./build.cmd -t prerelease


# Linux/mac

# Build only
build.sh

# Full release buildchain: build, test, pack, build the docs, push a git tag, publsih thze nuget package, release the docs
build.sh -t release

# The same for prerelease versions:
build.sh -t prerelease

```

### docs

The docs are contained in `.fsx` and `.md` files in the `docs` folder. To develop docs on a local server with hot reload, run the following in the root of the project:

```shell
# Windows
./build.cmd -t watchdocs

# Linux/mac
./build.sh -t watchdocs
```


### release

Library license
===============

The library is available under the [MIT license](https://github.com/plotly/Dash.NET/blob/dev/LICENSE).