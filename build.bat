@REM Open CMD as Admin
@REM mklink /d "C:\development\Rimworld-Together\Builder\Result\3005289691\Current\Assemblies" "C:\development\Rimworld-Together\Source\Client\bin\Release\net472"
@REM mklink /d "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\Rimworld Together Dev" "C:\development\Rimworld-Together\Builder\Result\3005289691"


docker run --rm -it ^
-w /App ^
-v %cd%/Source:/App/Source ^
--entrypoint "dotnet" ^
mcr.microsoft.com/dotnet/sdk:7.0 ^
build Source/Client/GameClient.csproj --configuration Release /property:WarningLevel=0
