dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:"coveragereport" -reporttypes:Html -classfilters:-Blackjack.Model.Migrations*
start coveragereport\index.html
