# HospedagemAPI-JWT-Identity

A sample ASP.NET Core 3.1 Web API for lodging management that uses JWT authentication and ASP.NET Identity backed by in-memory databases.

## Overview

This solution demonstrates:
- ASP.NET Core 3.1 Web API
- JWT Bearer token authentication
- ASP.NET Core Identity with role-based authorization
- In-memory Entity Framework Core databases for both Identity and application data
- A simple accommodation (`Acomodacao`) management API

## Solution structure

- `HospedagemAPI/` - API host project
- `Business/` - application business logic, security helpers, and services
- `Domain/` - domain models
- `Persistence/` - EF Core database contexts

## Key features

- `/login` endpoint for user authentication
- JWT token issuance with RSA signing and configured issuer/audience
- Protected `/acomodacoes` endpoints requiring `Bearer` authentication
- In-memory data persistence for quick local testing
- Initial seeded users and roles on startup

## Requirements

- .NET Core SDK 3.1
- Visual Studio 2019 or later / `dotnet` CLI

## Setup

1. Open the solution in Visual Studio or use the CLI:
   ```bash
   cd c:\Projects\DotNet\HospedagemAPI-JWT-Identity
   dotnet restore
   dotnet build
   ```

2. Run the API project:
   ```bash
   dotnet run --project HospedagemAPI\HospedagemAPI.csproj
   ```

3. The API starts on the configured host and port. By default, HTTPS is enabled.

## API endpoints

### Authenticate and get JWT

POST `/login`

Request body example:

```json
{
  "UserID": "admin_hospedagemAPI",
  "Password": "AdminHospedagemAPI01!"
}
```

Successful response example:

```json
{
  "authenticated": true,
  "created": "2026-05-28 12:34:56",
  "expiration": "2026-05-28 12:35:56",
  "accessToken": "<jwt-token>",
  "refreshToken": null,
  "message": "Ok"
}
```

### List accommodations

GET `/acomodacoes`

- Requires `Authorization: Bearer <token>` header
- Returns all accommodations ordered by name

### Add accommodation

POST `/acomodacoes`

- Requires `Authorization: Bearer <token>` header
- Accepts an `Acomodacao` JSON payload

Example request body:

```json
{
  "Nome": "Suíte Luxo",
  "Tipo": "Quarto",
  "Diaria": 250.00,
  "Area": "35m²",
  "Local": "Centro"
}
```

Response is a `Resultado` object containing validation information.

## Seeded credentials

The application seeds an in-memory Identity database at startup with:

- Valid admin user:
  - `UserName`: `admin_hospedagemAPI`
  - `Password`: `AdminHospedagemAPI01!`
  - Role: `Acesso-HospedagemAPI`

- Invalid user (no access role):
  - `UserName`: `usrinvalido_hospedagemAPI`
  - `Password`: `UsrInvHospedagemAPI01!`

Only the role-bound admin user can successfully authenticate and access the protected API endpoints.

## Token configuration

Configured in `HospedagemAPI/appsettings.json`:
- `Audience`: `Clients-HospedagemAPI`
- `Issuer`: `HospedagemAPI`
- `Seconds`: `60`

## Notes

- The project uses `Microsoft.EntityFrameworkCore.InMemory` for easy testing and does not persist data between runs.
- Token expiration is short by default; adjust `Seconds` in `appsettings.json` for longer-lived tokens.
- The API currently does not expose registration or refresh-token endpoints.
