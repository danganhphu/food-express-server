# Set shell for Windows OSs:
set windows-shell := ["pwsh.exe", "-NoLogo", "-Command"]

set dotenv-load := true

alias upgrade-v := upgrade-all-version
alias b := build
alias r := run

upgrade-all-version:
  dotnet-outdated food-express.slnx -u

build:
  dotnet build food-express.slnx

run:
  dotnet run --project .\src\Aspire\FoodExpress.AppHost\FoodExpress.AppHost.csproj