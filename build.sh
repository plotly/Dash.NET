#!/bin/sh
dotnet restore
dotnet build --no-restore
dotnet test --no-build
