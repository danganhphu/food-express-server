set dotenv-load := true

build:
  dotnet build airline-suite.sln

run:
  dotnet run --project src\Aspire\AirlineSuite.AppHost\AirlineSuite.AppHost.csproj