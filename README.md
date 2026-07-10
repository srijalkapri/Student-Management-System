# School Management CRUD API

A .NET 8 REST API for managing school operations — students, teachers, grades, subjects, exams, and user authentication — built with **Clean Architecture**, **PostgreSQL**, **JWT auth**, and configurable request/response logging.

---

## Features

- **Clean Architecture** — Domain, Application, Infrastructure, and Web layers
- **JWT authentication** — Bearer token–based API security
- **Role-based authorization** — `SuperAdmin` and `Teacher` roles
- **User registration & approval** — new users register; SuperAdmin approves or rejects
- **Full CRUD** — Students, Teachers, Grades, Subjects, GradeSubjects, GradeSubjectTeachers
- **Exam management** — schedules and sessions with bulk update support
- **Student promotion** — preview and promote students between grades
- **Soft delete** — restore support for students and teachers
- **Pagination** — paged list endpoints across major resources
- **FluentValidation** — request validation with consistent error responses
- **Access logging** — API access metadata stored in PostgreSQL (`AccessLogs`)
- **Request/response file logging** — Serilog-based logging with configurable rolling and file size limits
- **Swagger UI** — interactive API documentation in Development

---

## Tech Stack

| Layer | Technology |
|--------|------------|
| Runtime | .NET 8 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core 8 |
| Database | PostgreSQL |
| Auth | JWT Bearer |
| Validation | FluentValidation |
| Logging | Serilog (File + Map sinks) |
| API Docs | Swagger / Swashbuckle |

---

## Architecture
CRUD/ ├── CRUD.Domain/ # Entities and domain models ├── CRUD.Application/ # DTOs, services, interfaces, validators ├── CRUD.Infrastructure/ # EF Core, repositories, migrations └── CRUD.Web/ # API controllers, middleware, Program.cs


**Dependency flow:** `Web → Application → Domain` and `Infrastructure → Application`

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/) (local or remote)
- IDE: Visual Studio 2022, VS Code, or Cursor (with C# extension for debugging)

---

## Getting Started

### 1. Clone the repository

```bash
git clone <your-repo-url>
cd CRUD

### 2. Configure the database
Update CRUD.Web/appsettings.json (or use User Secrets / environment variables):


"ConnectionStrings": {
  "PostgreSQLConnection": "Host=localhost;Port=5432;Database=CrudDB;Username=postgres;Password=YOUR_PASSWORD"
}

3. Apply migrations (if needed)
dotnet ef database update --project CRUD.Infrastructure --startup-project CRUD.Web
The app also runs DbInitializer on startup to seed initial data (including SuperAdmin).

4. Run the API
dotnet run --project CRUD.Web
Or press F5 in VS Code/Cursor with the CRUD.Web (https) debug profile.

5. Open Swagger
HTTPS: https://localhost:7172/swagger
HTTP: http://localhost:5271/swagger
Authentication
Default SuperAdmin (seeded)
Configured in appsettings.json under SuperAdmin:

Field	Default
Username
superadmin
Password
(see appsettings — change in production)
Login flow
POST /api/Auth/Login with username and password
Copy the JWT token from the response
In Swagger, click Authorize and enter: Bearer <your-token>
Roles
Role	Access
SuperAdmin
Full access; user approval; teacher management
Teacher
Students, grades, subjects, exams (no teacher CRUD)
Auth endpoints
Method	Endpoint	Description
POST
/api/Auth/Login
Login (anonymous)
POST
/api/Auth/Register
Register (anonymous)
GET
/api/Auth/PendingUsers
List pending users (SuperAdmin)
POST
/api/Auth/Approve/{userId}
Approve user (SuperAdmin)
POST
/api/Auth/Reject/{userId}
Reject user (SuperAdmin)
GET
/api/Auth/Me
Current user profile
POST
/api/Auth/Logout
Logout
API Modules
Controller	Base Route	Main operations
Auth
/api/Auth
Login, register, approval
Student
/api/Student
CRUD, restore, promotion
Teacher
/api/Teacher
CRUD, restore, details
Grade
/api/Grade
CRUD, pagination
Subject
/api/Subject
CRUD, pagination
GradeSubject
/api/GradeSubject
Grade–subject mapping
GradeSubjectTeacher
/api/GradeSubjectTeacher
Teacher assignments
Exam
/api/Exam
Schedules & sessions
All controllers (except anonymous auth endpoints) require a valid JWT.

Logging
1. Access Log (database)
Writes API access metadata to PostgreSQL AccessLogs table (user, path, method, timestamp).

Config (appsettings.json):

"AccessLog": {
  "Enabled": true,
  "ExcludedPaths": [ "/swagger", "/health" ]
}
2. Request/Response Log (file)
Dedicated Serilog logger writes to CRUD.Web/Logs/:

Request starting
Executing endpoint
Request body / response body
Config:

"RequestResponseLogging": {
  "Enabled": true,
  "LogDirectory": "Logs",
  "FileNamePrefix": "RequestResponseLog_",
  "RollingMode": "Hour",
  "RollingIntervalMinutes": 2,
  "MaxFileSizeKb": 1024,
  "ExcludedPaths": [ "/swagger", "/health", "/api/Auth/Login" ]
}
Rolling modes
RollingMode	Behavior
Custom
Custom N-minute buckets (RollingIntervalMinutes)
Minute
New file every minute
Hour
New file every hour
Day
New file every day
File size rolling
MaxFileSizeKb triggers a new file when size is exceeded (within the same time bucket).

Note: Serilog’s built-in file size limit is a soft cap. A single large log entry (e.g. full response body) can push a file slightly over the configured limit before rolling. For a strict hard cap, a custom sink would be required.

Configuration Reference
Section	Purpose
ConnectionStrings:PostgreSQLConnection
Database connection
Jwt
Issuer, audience, signing key, token expiry
SuperAdmin
Initial admin seed credentials
AccessLog
DB access logging
RequestResponseLogging
File request/response logging
Security: Do not commit real passwords or JWT keys. Use User Secrets, environment variables, or Azure Key Vault in production.

Debugging (VS Code / Cursor)
Open the repo root in VS Code/Cursor
Install the C# extension
Use Run and Debug → CRUD.Web (https)
Press F5
Set breakpoints in controllers, services, or middleware
Call endpoints from Swagger (with JWT for protected routes)
Ensure only one API instance is running when debugging.

Response Format
API responses use a consistent wrapper:

{
  "success": true,
  "message": "Operation completed.",
  "data": { },
  "errors": []
}
Validation errors return 400 Bad Request with an errors array.

Project Structure (Web Layer)
CRUD.Web/
├── Controllers/          # API endpoints
├── Middleware/
│   ├── AccessLogMiddleware.cs
│   └── RequestResponseLoggingMiddleware.cs
├── Logging/
│   └── RequestResponseLoggerFactory.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
└── Program.cs
Middleware Pipeline
HTTPS Redirection
  → Authentication
  → Authorization
  → RequestResponseLoggingMiddleware
  → AccessLogMiddleware
  → Controllers
License
This project is for educational/internal use. Add your license here if needed.
