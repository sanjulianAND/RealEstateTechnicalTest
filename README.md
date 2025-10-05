# RealEstateTechnicalTest

Clean architecture scaffold for the technical test.  
This first commit only includes the solution layout and a **/ping** endpoint that checks SQL Server connectivity.

## Projects

- **01.Api**: ASP.NET Core Web API (Swagger enabled).
- **02.Application**: Use cases and validators (placeholder for now).
- **03.Domain**: Domain entities (placeholder for now).
- **04.Infrastructure**: EF Core DbContext and DI.
- **05.Shared**: Shared primitives (placeholder for now).
- **06.Test**: NUnit tests project (placeholder for now).

## Requirements

- .NET 8 SDK
- SQL Server instance available
- Visual Studio 2022 (or `dotnet` CLI)

## Configure connection string

Edit `01.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "RealEstate": "Server=DESKTOP-D867T7P\\SQLEXPRESS;Database=RealEstateDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
