rm -rf */bin */obj

dotnet.exe build BarLauncher-UnitConverter.sln
dotnet.exe publish BarLauncher.UnitConverter.Wox/BarLauncher.UnitConverter.Wox.csproj -c Release
dotnet.exe publish BarLauncher.UnitConverter.Flow.Launcher/BarLauncher.UnitConverter.Flow.Launcher.csproj -c Release -r win-x64

