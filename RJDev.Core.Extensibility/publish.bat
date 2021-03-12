dotnet pack -c Release
set /p version=Package version: 
dotnet nuget push bin\Release\RJDev.Core.Extensibility.%version%.nupkg --api-key %NUGET_RJDEV_API_KEY% --source https://api.nuget.org/v3/index.json