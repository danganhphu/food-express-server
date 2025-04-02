# Set shell for Windows OSs:
set windows-shell := ["pwsh.exe", "-NoLogo", "-Command"]

set dotenv-load := true

alias upgrade-v := upgrade-all-version
alias b := build
alias r := run
alias ss := setup-secrets

upgrade-all-version:
  dotnet-outdated food-express.slnx -u
  
restore:
  dotnet restore
  dotnet tool restore
    
setup-secrets:
  cd src/Aspire/FoodExpress.AppHost && \
  dotnet user-secrets set "Parameters:postgres-username" "postgres" && \
  dotnet user-secrets set "Parameters:postgres-password" "postgres" && \
  dotnet user-secrets set "Parameters:keycloak-username" "admin" && \
  dotnet user-secrets set "Parameters:keycloak-password" "admin"
    
build:
  dotnet build food-express.slnx

run: restore setup-secrets
  dotnet run --project src/Aspire/FoodExpress.AppHost/FoodExpress.AppHost.csproj