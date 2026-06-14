# Bitcoin Market Loader

Bitcoin Market Loader is a .NET 10 application that retrieves BTC/EUR market
ticks from CoinDesk, stores them, and presents them through a REST API and a
Blazor web interface.

Stored ticks use EUR as their source currency. API responses can be returned in
EUR or converted to CZK using exchange rates from the Czech National Bank.

## Features

- Periodic retrieval and persistence of the latest BTC/EUR market tick
- SQL Server persistence through Entity Framework Core
- Optional in-memory repository for lightweight runs
- EUR and CZK display currencies
- Paged market tick REST API
- Market tick notes
- Single and batch tick deletion
- Blazor UI with paging, currency switching, auto-refresh, note editing, and
  delete mode
- Swagger API documentation
- NLog file logging, with additional console logging in Development
- Docker Compose configurations for normal and fully mocked local runs

## Solution Structure

| Project | Responsibility |
| --- | --- |
| `BitcoinMarketLoader.Domain` | Market domain models and enums |
| `BitcoinMarketLoader.Application` | Application services, currency conversion, and polling logic |
| `BitcoinMarketLoader.Infrastructure` | CoinDesk/CNB clients, EF Core, SQL Server, and repositories |
| `BitcoinMarketLoader.Api` | REST API, Swagger, health endpoint, and polling background host |
| `BitcoinMarketLoader.Web` | Blazor web interface |
| `BitcoinMarketLoader.UnitTests` | Unit and model configuration tests |

## Requirements

For local .NET development:

- .NET 10 SDK
- SQL Server, unless using the in-memory repository

For containerized runs:

- Docker
- Docker Compose

## Configuration

Configuration can be provided through `appsettings.json`, environment-specific
files, user secrets, or environment variables. Nested environment variable
names use double underscores, for example:

```
CoinDeskClient__ApiKey=your-api-key
```

### API Settings

| Setting | Description                                                                                                        |
| --- |--------------------------------------------------------------------------------------------------------------------|
| `ConnectionStrings:SqlServer` | SQL Server connection string                                                                                       |
| `useInMemoryRepositories` | Use non-persistent in-memory storage when `true`                                                                   |
| `Database:ApplyMigrations` | Apply pending EF Core migrations during API startup                                                                |
| `MarketTickPolling:IntervalSeconds` | Polling interval; `0` or a negative value disables polling                                                         |
| `CoinDeskClient:BaseUrl` | CoinDesk server base URL                                                                                           |
| `CoinDeskClient:ApiKey` | CoinDesk API Key - required when running agains real server                                                        |
| `CoinDeskClient:OverrideTickId` | Testing only - generate unique CCSEQ values for repeated mocked responses                                          |
| `CnbApiClient:BaseUrl` | Czech National Bank API base URL                                                                                   |
| `ExchangeRateConfig:CacheDurationInHours` | Historical exchange-rate cache duration                                                                            |
| `ExchangeRateConfig:CurrentDayCacheDurationInMinutes` | Current-day exchange-rate cache duration - should be shorter, because it can change when ČNB publishes a new rates |

Do not commit production API keys, passwords, or connection strings. Prefer
environment variables or a secret manager.

### Web Settings

| Setting | Description |
| --- | --- |
| `BitcoinMarketApi:BaseUrl` | Base URL of `BitcoinMarketLoader.Api` |

## Run Locally with .NET

Configure `BitcoinMarketLoader.Api/appsettings.Development.json` for your local
SQL Server and CoinDesk endpoint. The current Development configuration expects:

- SQL Server on `localhost:1433`
- MockServer on `localhost:1080`
- Automatic migrations enabled

Start the API:

```bash
dotnet run --project BitcoinMarketLoader.Api
```

Start the web application in another terminal:

```bash
dotnet run --project BitcoinMarketLoader.Web
```

Default local URLs:

- Web: `http://localhost:5158`
- API: `http://localhost:5131`
- Swagger: `http://localhost:5131/swagger`
- Health: `http://localhost:5131/health`

## Run with Docker Compose

The base Compose configuration runs the API and web projects. It uses the
in-memory repository and disables polling by default:

```bash
docker compose up --build
```

Stop the stack:

```bash
docker compose down
```

Available URLs:

- Web: `http://localhost:5158`
- API: `http://localhost:5131`

Useful environment overrides:

```bash
USE_IN_MEMORY_REPOSITORIES=false \
APPLY_DATABASE_MIGRATIONS=true \
MARKET_TICK_POLLING_INTERVAL_SECONDS=60 \
COINDESK_API_KEY=your-api-key \
docker compose up --build
```

When disabling the in-memory repository, also provide
`ConnectionStrings__SqlServer` through a Compose override file or by updating
the service environment in `compose.yaml`.

## Local Mock Testing Stack

`compose.mock.yaml` provides a complete local testing environment:

- MockServer on port `1080`
- SQL Server on port `1433`
- API on port `5131`
- Web application on port `5158`

The API waits for MockServer and SQL Server to become healthy. It then applies
EF Core migrations before becoming healthy itself. The web application starts
after the API is healthy.

MockServer loads its CoinDesk expectation from:

```text
mockserver/coindesk-expectations.json
```

Run the testing stack:

```bash
docker compose -f compose.mock.yaml up --build
```

Open:

```text
http://localhost:5158
```

The mock configuration returns the same CoinDesk payload repeatedly. The API
sets `CoinDeskClient:OverrideTickId` to `true`, so each fetched tick receives a
unique CCSEQ and can be persisted.

Change the polling interval:

```bash
MARKET_TICK_POLLING_INTERVAL_SECONDS=5 \
docker compose -f compose.mock.yaml up --build
```

Stop the testing stack and remove its containers:

```bash
docker compose -f compose.mock.yaml down
```

The SQL Server container currently has no persistent volume, so its database is
removed with the container. The credentials in `compose.mock.yaml` are
development-only and must not be used for deployment.

## Entity Framework Migrations

Create a migration:

```bash
dotnet ef migrations add MigrationName \
  --project BitcoinMarketLoader.Infrastructure \
  --startup-project BitcoinMarketLoader.Api \
  --context MainDbContext
```

Apply migrations manually:

```bash
dotnet ef database update \
  --project BitcoinMarketLoader.Infrastructure \
  --startup-project BitcoinMarketLoader.Api \
  --context MainDbContext
```

For deployment, enable startup migration application:

```bash
Database__ApplyMigrations=true
```

Startup migrations are skipped when `useInMemoryRepositories=true`.

## Tests

Run all unit tests:

```bash
dotnet test BitcoinMarketLoader.UnitTests
```

Build the complete solution:

```bash
dotnet build BitcoinMarketLoader.sln
```

## Logging

The API uses NLog.

- Production-style configuration writes dated files under `logs/`.
- Development writes to both the console and a dedicated development log file.

The active files are:

```text
BitcoinMarketLoader.Api/NLog.config
BitcoinMarketLoader.Api/NLog.Development.config
```
