# RealEstate Technical Test – Clean Architecture API (.NET 8 + SQL Server)

API to manage Real-Estate Properties with the services requested by the exercise:
- Create Property
- Update Property
- Change Price
- Add Image
- List Properties with filters, sorting & pagination
It uses a Clean-ish modular layout matching the project references you asked to keep, plus structured logging. Unit tests cover validators and handlers with Moq.

## Tech Stack

- .NET 8 (ASP.NET Core Web API)
- SQL Server (on-prem or Docker)
- EF Core 8 (DbContext + LINQ queries)
- FluentValidation (validators per use case)
- NUnit + Moq + FluentAssertions (tests)
- Swagger / OpenAPI (built-in)
- Docker (API + SQL), optional Azure DevOps pipeline YAML

## Projects

- **01.Api**: ASP.NET Core Web API (Swagger enabled).
- **02.Application**: Use cases and validators (placeholder for now).
- **03.Domain**: Domain entities (placeholder for now).
- **04.Infrastructure**: EF Core DbContext and DI.
- **05.Shared**: Shared primitives (placeholder for now).
- **06.Test**: NUnit tests project (placeholder for now).

## Database Model

Tables follow the diagram provided plus a few pragmatic fields:

 - Property: IdProperty (PK), Name, Address, Price (CHECK Price>0), CodeInternal (UNIQUE), Year, IdOwner (FK), CreatedAt, UpdatedAt
 - PropertyImage: IdPropertyImage (PK), IdProperty (FK), File, Enabled, CreatedAt
 - PropertyTrace: IdPropertyTrace (PK), IdProperty (FK), DateSale, Name, Value, Tax
 - Owner: IdOwner (PK), Name, Address, Photo, Birthday

Indexes: IX_Property_Price, IX_Property_Year, IX_Property_CreatedAt,
IX_PropertyImage (IdProperty, Enabled), IX_PropertyTrace (IdProperty, DateSale).

- Added CreatedAt/UpdatedAt and Enabled to match the note “add fields as you see fit”.

Note: The schema.sql si located at `RealEstateTechnicalTest\db\schema.sql`

### How to Run Locally

Prerequisites
- .NET 8 SDK
- SQL Server running (local instance, e.g. .\SQLEXPRESS or default)
- Visual Studio 2022 or VS Code + C# Dev Kit

Create Database & Schema
- Run your SQL script in SSMS (or sqlcmd) against your instance to create RealEstateDB and the tables.

Configure Connection String

Edit `01.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "RealEstate": "Server=DESKTOP-D867T7P\\SQLEXPRESS;Database=RealEstateDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }
}
```
Run

dotnet restore
dotnet build -c Debug
dotnet run --project RealEstateTechnicalTest

### Endpoints

- `GET /v1/properties` — filters: `name,address,codeInternal,ownerId,minPrice,maxPrice,minYear,maxYear`, sort: `price|-price|year|-year|createdAt|-createdAt`, paging: `page,pageSize`. Adds `X-Total-Count`.
- `POST /v1/properties` — create property.
- `PUT /v1/properties/{id}` — update fields.
- `PATCH /v1/properties/{id}/price` — change price.
- `POST /v1/properties/{id}/images` — add image.

### Testing (NUnit)

dotnet test -c Debug
dotnet test -c Debug --logger "trx;LogFileName=test-results.trx"

### How to Run with Docker (My extra)

Files
- RealEstateTechnicalTest/Dockerfile – builds & runs API
- RealEstateTechnicalTest/appsettings.Docker.json – connection to the SQL container
- docker-compose.yml – orchestrates sql + api

Compose Up
- docker compose up --build

