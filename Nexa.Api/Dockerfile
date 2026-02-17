FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Nexa.Api ./Nexa.Api
WORKDIR /app/Nexa.Api

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/Nexa.Api/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Nexa.Api.dll"]