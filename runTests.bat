dotnet test --collect:"XPlat Code Coverage"
coverlet Test\bin\Debug\net8.0\Test.dll --target "dotnet" --targetargs "test Test/Test.csproj --no-build"
reportgenerator -reports:coverage.cobertura.xml -targetdir:"coveragereport" -reporttypes:Html
start coveragereport\index.htm