FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN apt-get update \
    && apt-get install -y --no-install-recommends nodejs npm \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /src
COPY ["BitcoinMarketLoader.Web/BitcoinMarketLoader.Web.csproj", "BitcoinMarketLoader.Web/"]
COPY ["BitcoinMarketLoader.Web/package.json", "BitcoinMarketLoader.Web/package-lock.json", "BitcoinMarketLoader.Web/"]
RUN dotnet restore "BitcoinMarketLoader.Web/BitcoinMarketLoader.Web.csproj"
RUN npm ci --prefix "BitcoinMarketLoader.Web"
COPY . .
WORKDIR "/src/BitcoinMarketLoader.Web"
RUN dotnet build "./BitcoinMarketLoader.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BitcoinMarketLoader.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitcoinMarketLoader.Web.dll"]
