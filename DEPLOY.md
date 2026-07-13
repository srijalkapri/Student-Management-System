# Free deploy guide: Neon (DB) + Render (API) + Vercel (Frontend)
#
# Frontend path: C:\Users\Srijal\OneDrive\Desktop\FrontEnd

## 1) Neon PostgreSQL (free)

1. Create account at https://neon.tech
2. Create a project and copy the connection string.
3. Format for this app (add SSL):

```
Host=ep-xxxx.region.aws.neon.tech;Database=neondb;Username=xxx;Password=xxx;SSL Mode=Require;Trust Server Certificate=true
```

4. From your PC (with that connection string set), apply migrations:

```powershell
cd C:\Users\Srijal\source\repos\CRUD
$env:ConnectionStrings__PostgreSQLConnection="Host=...;Database=neondb;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true"
dotnet ef database update --project CRUD.Infrastructure --startup-project CRUD.Web
```

---

## 2) Render Web Service (API)

1. Push this `CRUD` repo to GitHub.
2. https://render.com → **New** → **Web Service** → connect the repo.
3. Settings:
   - **Runtime:** Docker
   - **Dockerfile Path:** `Dockerfile` (repo root)
   - **Root Directory:** leave empty (repo root)
4. Environment variables:

| Key | Example |
|-----|---------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__PostgreSQLConnection` | Neon string from step 1 |
| `Jwt__Key` | long random secret (32+ chars) |
| `Jwt__Issuer` | `CRUDAPI` |
| `Jwt__Audience` | `CRUDAPIUser` |
| `Jwt__ExpiresInMinutes` | `60` |
| `SuperAdmin__Username` | `superadmin` |
| `SuperAdmin__Password` | strong password |
| `SuperAdmin__FullName` | `Super Administrator` |
| `Cors__AllowedOrigins__0` | `https://YOUR-APP.vercel.app` |
| `EnableSwagger` | `true` (optional, for testing) |

5. Deploy. Note your URL: `https://YOUR-API.onrender.com`
6. Test: `GET https://YOUR-API.onrender.com/health`

Free tier sleeps after idle; first request may be slow.

---

## 3) Vercel (Frontend)

Frontend folder: `C:\Users\Srijal\OneDrive\Desktop\FrontEnd`

1. Push FrontEnd to its own GitHub repo.
2. https://vercel.com → Import that repo.
3. Framework: Vite
4. Environment variable:

| Key | Value |
|-----|-------|
| `VITE_API_URL` | `https://YOUR-API.onrender.com` (no trailing slash) |

5. Deploy. Note URL: `https://YOUR-APP.vercel.app`
6. Go back to Render and set `Cors__AllowedOrigins__0` to that Vercel URL, then **redeploy** the API.

Local frontend still works without `VITE_API_URL` (Vite proxies `/api` → `https://localhost:7172`).

---

## 4) Go-live checklist

- [ ] Migrations applied on Neon
- [ ] Render health endpoint returns `{ "status": "ok" }`
- [ ] Login works from Vercel site
- [ ] CORS origin matches Vercel URL exactly (https, no trailing slash)
- [ ] Changed default SuperAdmin password and JWT key
- [ ] Teacher / Student portals work after approve flow

---

## Local Docker test (optional)

```powershell
cd C:\Users\Srijal\source\repos\CRUD
docker build -t crud-api .
docker run --rm -p 8080:8080 `
  -e ConnectionStrings__PostgreSQLConnection="Host=host.docker.internal;Port=5432;Database=CrudDB;Username=postgres;Password=...;SSL Mode=Prefer" `
  -e Jwt__Key="YourSuperSecretKeyHereMakeItLongEnoughForHS256" `
  -e Cors__AllowedOrigins__0="http://localhost:5173" `
  crud-api
```

Then open `http://localhost:8080/health`.
