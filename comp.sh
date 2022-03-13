rm -rf */bin */obj */build build

dotnet.exe publish BarLauncher.UnitConverter.Wox/BarLauncher.UnitConverter.Wox.csproj -c Debug
dotnet.exe publish BarLauncher.UnitConverter.Flow.Launcher/BarLauncher.UnitConverter.Flow.Launcher.csproj -c Debug -r win-x64

