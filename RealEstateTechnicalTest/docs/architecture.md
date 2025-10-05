# Architecture (current topology)

> NOTE: This layout intentionally follows the project references currently configured in the solution.

	/RealEstateTechnicalTest.sln
	/RealEstateTechnicalTest → API (ASP.NET Core)
	/Application → Use cases (handlers/validators), depends on Infrastructure + Shared
	/Domain → Entities and rules, depends on Shared
	/Infrastructure → EF Core (DbContext, mappings), ports (interfaces), DTOs and queries, repos/adapters, depends on Domain
	/Shared → (reserved) common primitives (future)

## Dependency flow (as implemented)
- **API** → uses **Application** and **Shared**.
- **Application** → depends on **Infrastructure** (for ports/DTO/Query) and **Shared**.
- **Infrastructure** → depends on **Domain** (entities) and holds **ports** (e.g., `IPropertyRepository`) and data contracts used by Application.
- **Domain** → independent (entities only), depends on **Shared** if needed.

### Why ports live in Infrastructure here?
To respect the current project references without changing them. Application consumes repository **interfaces** exposed by Infrastructure and calls their implementations via DI (also registered in Infrastructure).

## Current scope
- Endpoints:
  - `GET /v1/properties` with **filters**, **sorting**, **pagination**. Adds `X-Total-Count` header.
- EF Core DbContext with minimal mappings reflecting the given schema.
- Use case:
  - `PropertyListHandler` (+ `PropertyListValidator`).
- Repository:
  - `PropertyRepository` using EF Core and projections to lightweight DTOs.

## Next steps
- Commands: **Create**, **Update**, **ChangePrice**, **AddImage**.
- Error handling via **ProblemDetails** and logging.
- Unit tests (NUnit).
- Docker compose (API+SQL).
- CI pipeline template (optional).

## API surface (v0.3)
- `GET /v1/properties` (filters/sort/pagination)
- `POST /v1/properties` (create)
- `PUT /v1/properties/{id}` (update)
- `PATCH /v1/properties/{id}/price` (change price)
- `POST /v1/properties/{id}/images` (add image)

DTOs for requests are defined in Infrastructure. Handlers with validation and logging live in Application. Repositories adapt EF Core in Infrastructure. Controllers stay thin and map DTOs to use cases, returning proper status codes.
