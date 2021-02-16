:: CLEAN UP existing folders
del ..\bin\Release\netcoreapp3.1\win-x64\publish
del ..\bin\Release\netcoreapp3.1\linux-x64\publish
del ..\bin\Release\netcoreapp3.1\osx-x64\publish

::Do the publish

dotnet publish ../StructureWatch.csproj -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=%1 --version-suffix %1
dotnet publish ../StructureWatch.csproj -r linux-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=%1 --version-suffix %1
dotnet publish ../StructureWatch.csproj -r osx-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=%1 --version-suffix %1


:: TODO ZIPS
::DONE;
ECHO HOOORAY!
