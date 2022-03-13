#!/usr/bin/env bash

rm -rf ./*/bin ./*/obj ./build

VERSION=$(cat VERSION)-$(date +%s)

dotnet.exe publish BarLauncher.UnitConverter.Wox/BarLauncher.UnitConverter.Wox.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.UnitConverter.Wox/bin/Debug/net48/publish; zip -r ../../../../../../../BarLauncher-UnitConverter-${VERSION}.wox .)

dotnet.exe publish BarLauncher.UnitConverter.Flow.Launcher/BarLauncher.UnitConverter.Flow.Launcher.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.UnitConverter.Flow.Launcher/bin/Debug/net5.0-windows/publish; zip -r ../../../../../../../BarLauncher-UnitConverter-${VERSION}.zip .)
