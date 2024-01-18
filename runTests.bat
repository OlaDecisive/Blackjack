dotnet test --collect:"XPlat Code Coverage"
coverlet TestModel\bin\Debug\net8.0\TestModel.dll --target "dotnet" --targetargs "test TestModel/TestModel.csproj --no-build" --format cobertura
coverlet TestWebApi\bin\Debug\net8.0\TestWebApi.dll --target "dotnet" --targetargs "test TestWebApi/TestWebApi.csproj --no-build" --format cobertura
reportgenerator -reports:coverage.cobertura.xml -targetdir:"coveragereport" -reporttypes:Html
start coveragereport\index.html