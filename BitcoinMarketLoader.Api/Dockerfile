FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BitcoinMarketLoader.Api/BitcoinMarketLoader.Api.csproj", "BitcoinMarketLoader.Api/"]
RUN dotnet restore "BitcoinMarketLoader.Api/BitcoinMarketLoader.Api.csproj"
COPY . .
WORKDIR "/src/BitcoinMarketLoader.Api"
RUN dotnet build "./BitcoinMarketLoader.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BitcoinMarketLoader.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitcoinMarketLoader.Api.dll"]
