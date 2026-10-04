FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/MigrationService/MigrationService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/src/MigrationService/Migrations ./Migrations
ENTRYPOINT ["dotnet", "MigrationService.dll"]
