#!/usr/bin/env bash

dotnet restore
dotnet tool restore
dotnet paket install

cd ./demo
yarn install --pure-lockfile
cd -
