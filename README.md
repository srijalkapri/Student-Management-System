# Student Management System (SMS)

Backend API for a school Student Management System, built with **ASP.NET Core 8** and **Clean Architecture**.

**Live API:** https://student-management-system-tonm.onrender.com  
**Live Frontend:** https://front-end-sms.vercel.app  

---

## Architecture

| Project | Layer | Responsibility |
|---------|--------|----------------|
| `CRUD.Domain` | Domain | Entities (`Student`, `Teacher`, `User`, etc.) |
| `CRUD.Application` | Application | DTOs, services, interfaces, validators |
| `CRUD.Infrastructure` | Infrastructure | EF Core, PostgreSQL, repositories, migrations |
| `CRUD.Web` | Presentation | Controllers, JWT auth, middleware, Swagger |

```text
CRUD.Web → Application + Infrastructure
Infrastructure → Application + Domain
Application → Domain
Domain → (no dependencies)
```

---

## Features

- JWT authentication (register → admin approve/reject)
- Role-based access: **SuperAdmin**, **Teacher**, **Student**
- Admin CRUD: students, teachers, grades, subjects, exams, assignments
- Teacher / Student portals (scoped “me” data)
- Soft delete
- Access logs (PostgreSQL)
- Request/response file logging (local `Logs/`)
- Health check: `GET /health`

---

## Tech stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core + Npgsql (PostgreSQL)
- FluentValidation
- JWT Bearer
- Serilog (request/response files)
- Docker (Render deploy)

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (local) or Neon (cloud)
- Optional: Docker

---

## Getting started (local)

### 1. Clone and restore

```powershell
git clone https://github.com/srijalkapri/Student-Management-System.git
cd Student-Management-System
dotnet restore
```

### 2. Connection string

Edit `CRUD.Web/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "PostgreSQLConnection": "Host=localhost;Port=5432;Database=CrudDB;Username=postgres;Password=YOUR_PASSWORD;SSL Mode=Prefer"
  }
}
```

### 3. Apply migrations

```powershell
dotnet ef database update --project CRUD.Infrastructure --startup-project CRUD.Web
```

### 4. Run

```powershell
dotnet run --project CRUD.Web
```

- Swagger: http://localhost:5271/swagger
- Default SuperAdmin credentials are in `appsettings.json` — change the password after first login

---

## Configuration

| File | When used |
|------|-----------|
| `appsettings.json` | Shared defaults |
| `appsettings.Development.json` | Local (overrides) |
| `appsettings.Production.json` | Production defaults |
| Render env vars | Secrets (DB, JWT, CORS) — preferred in prod |

---

## Main API areas

| Area | Controller |
|------|------------|
| Auth / approve / reject | `AuthController` |
| Admin students / teachers / grades / … | `*Controller` (SuperAdmin) |
| Teacher “me” APIs | `TeacherPortalController` |
| Student “me” APIs | `StudentPortalController` |

---

## Branches

| Branch | Purpose |
|--------|---------|
| `master` | Development / demo |
| `clean_arch` | Production deploy (Render) |

---

## Deployment

See **[DEPLOY.md](DEPLOY.md)** for Neon + Render + Vercel steps.

Quick overview:

1. **Neon** — PostgreSQL
2. **Render** — API (Docker, branch `clean_arch`)
3. **Vercel** — frontend (`VITE_API_URL` → Render URL)

---

## License

Private / educational project.
