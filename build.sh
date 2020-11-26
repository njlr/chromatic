#!/usr/bin/env bash

dotnet build
dotnet fable ./chromatic/chromatic.fsproj
