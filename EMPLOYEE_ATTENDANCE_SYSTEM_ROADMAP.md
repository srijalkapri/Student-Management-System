# Employee Attendance System — Project Roadmap & Technical Blueprint

**Document purpose:** Client-facing technical roadmap for building a secure, scalable employee attendance platform (~50 employees) by integrating your existing **SMART-PRESENCE** face detection/recognition pipeline with a **.NET 8** backend (Clean Architecture) and two portals: **Admin** and **Employee**.

**Reference:** `FACE_DETECTION_RECOGNITION.md` (MTCNN + InceptionResnetV1 / FaceNet pipeline)

---

## 1. Executive Summary

| Item | Decision |
|------|----------|
| **Problem** | Manual or unreliable attendance tracking for ~50 employees |
| **Solution** | Web/mobile attendance with **face-based check-in/check-out**, admin oversight, and audit trails |
| **Users** | **Admin** (HR/manager) + **Employee** (self-service attendance & profile) |
| **Scale (initial)** | 50 employees, single office, single shift model (extensible) |
| **Core differentiator** | Proven face pipeline (detect → align → embed → match) already built in SMART-PRESENCE |
| **Backend** | ASP.NET Core 8 API (reuse patterns from your current CRUD/SMS project) |
| **Auth** | JWT + role-based access (`Admin`, `Employee`) + optional admin approval on registration |

**What you are selling:** Not “a login app with a camera” — a **controlled biometric attendance system** with enrollment, verification, audit logs, and configurable match thresholds.

---

## 2. Client Requirements → System Mapping

| Client need | System feature |
|-------------|----------------|
| Record employee attendance | Check-in / check-out with timestamp, status, source (`face`, `manual_override`) |
| ~50 employees | Single-tenant DB; in-memory or Redis embedding index is sufficient at this scale |
| Face recognition | Reuse MTCNN + InceptionResnetV1 (512-dim embeddings, Euclidean match, threshold ~0.9) |
| Basic employee info | Employee profile: name, employee code, department, photo, contact, join date, active flag |
| Two portals only | **Admin portal** + **Employee portal** (no third role in v1) |
| Login / register | Username/password JWT; admin approves new employees before face enrollment |
| Trust & compliance | Audit logs, no raw password storage, encrypted embeddings optional, retention policy |

---

## 3. Recommended Architecture (Sell This to the Client)

### 3.1 High-level diagram

```text
┌─────────────────────────────────────────────────────────────────┐
│  Frontend (React / Flutter / Blazor — your choice)              │
│  • Admin dashboard    • Employee check-in camera UI              │
└────────────────────────────┬────────────────────────────────────┘
                             │ HTTPS + JWT
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│  .NET 8 API (Clean Architecture)                                 │
│  • Auth (login/register/approve)                               │
│  • Employee CRUD (admin)                                       │
│  • Attendance records & reports                                │
│  • Enrollment orchestration                                    │
└────────────┬───────────────────────────────┬────────────────────┘
             │                               │
             ▼                               ▼
┌────────────────────────┐    ┌────────────────────────────────┐
│  PostgreSQL            │    │  Face Recognition Service       │
│  Users, Employees,     │    │  (Recommended: Python sidecar)  │
│  Attendance, Audit,    │    │  MTCNN + FaceNet from existing  │
│  Embedding metadata    │    │  SMART-PRESENCE pipeline        │
└────────────────────────┘    └────────────────────────────────┘
```

### 3.2 Integration strategy (recommended for v1)

**Option A — Python face microservice (recommended)**

| Pros | Cons |
|------|------|
| Reuse `face_pipeline.py` as-is | Two deployable services |
| Fastest time-to-market | Network hop per scan |
| Same accuracy you already tested | Need service-to-service auth |

**.NET calls Python** for:

- `POST /enroll` — generate embedding from profile photo
- `POST /recognize` — detect + match from camera frame
- `POST /reload-index` — rebuild embedding cache after admin verifies employee

**Option B — Native .NET (v2 / enterprise pitch)**

Port pipeline to ONNX + FaceAiSharp / FaceRecognitionDotNet. Higher upfront cost; single deployable artifact.

**Client message:** “Phase 1 uses our proven recognition engine; Phase 2 can consolidate into one stack if you need air-gapped deployment.”

---

## 4. Reuse From Your Current .NET Project (CRUD/SMS)

Your existing codebase is a strong foundation. **Reuse these patterns**, rename domains:

| Existing (SMS) | New (Attendance) |
|----------------|------------------|
| `User` + `UserStatus` (Pending/Approved/Rejected) | Same — employee self-register → admin approves |
| `AuthService` + JWT | Same — roles: `Admin`, `Employee` |
| `SuperAdmin` seed | `Admin` seed user |
| `AccessLogMiddleware` | API audit trail |
| `TeacherPortal` / `StudentPortal` | `AdminPortal` / `EmployeePortal` |
| Clean Architecture layers | `Domain` → `Application` → `Infrastructure` → `Web` |
| FluentValidation + `ServiceResponse<T>` | Keep |
| PostgreSQL + EF Core migrations | Keep |
| CORS + env-based config | Keep |

**Do not copy:** Students, grades, exams, teachers — replace with `Employee`, `Department`, `AttendanceRecord`, `FaceEnrollment`.

---

## 5. Face Pipeline (From SMART-PRESENCE) — What Must Be Preserved

### 5.1 Pipeline (non-negotiable for quality)

```text
JPEG from camera
  → MTCNN detect (prob >= 0.85)
  → Align 160×160
  → InceptionResnetV1 → 512-dim embedding (L2 normalized)
  → Euclidean distance vs enrolled index
  → if distance <= threshold (default 0.9) → match else "Unknown"
```

### 5.2 Enrollment rules

- Only employees with `IsVerified = true` and a **profile photo** enter the embedding index.
- On admin verify / photo update → rebuild that employee’s embedding.
- Store embedding in DB as `byte[]` or `float[512]` + optional in-memory index for speed.

### 5.3 Attendance scan rules

| Rule | Why |
|------|-----|
| Reject `Unknown` faces | Prevents buddy punching |
| Log confidence + distance | Audit & dispute resolution |
| One check-in per day (configurable) | Business rule |
| Check-out only if checked in | Data integrity |
| Optional: reject if confidence too low | Reduce false positives |

### 5.4 What NOT to ship

- OpenCV Haar fallback for recognition (detection-only → always Unknown).
- Storing raw camera images forever without policy (privacy risk).

---

## 6. Portal Breakdown

### 6.1 Admin portal (must-have)

| Module | Features |
|--------|----------|
| **Dashboard** | Today present/absent/late, recent check-ins |
| **Employees** | CRUD, activate/deactivate, assign department |
| **Approvals** | Approve/reject self-registrations (reuse auth flow) |
| **Face enrollment** | Upload/approve photo → trigger embedding build |
| **Attendance** | Daily/monthly view, filter by employee/department/date |
| **Reports** | Export CSV/PDF (monthly summary) |
| **Settings** | Recognition threshold, work hours, grace minutes |
| **Audit** | Who changed what; failed recognition attempts |

### 6.2 Employee portal (must-have)

| Module | Features |
|--------|----------|
| **Check-in / Check-out** | Camera capture → API scan |
| **My attendance** | History, hours this week/month |
| **My profile** | View/edit allowed fields; photo update (re-enrollment pending admin) |
| **Status** | Today: not checked in / checked in / checked out |

**v1 scope control:** Employee portal does **not** need full HR features — attendance + profile is enough.

---

## 7. Data Model (Minimum Viable)

### 7.1 Core entities

```text
User
  Id, Username, PasswordHash, Role (Admin|Employee), Status, CreatedAt

Employee
  Id, UserId, EmployeeCode, FullName, Email, Phone,
  DepartmentId, ProfilePhotoPath, IsVerified, IsActive,
  FaceEmbedding (optional byte[]), EnrolledAt

Department
  Id, Name

AttendanceRecord
  Id, EmployeeId, Date, CheckInUtc, CheckOutUtc,
  Status (Present|Late|Absent|HalfDay),
  CheckInMethod (Face|Manual), CheckOutMethod,
  CheckInConfidence, CheckOutConfidence,
  CreatedAt

AttendanceSettings
  FaceRecognitionThreshold (default 0.9),
  DetectionMinProbability (default 0.85),
  WorkStartTime, WorkEndTime, GraceMinutes

RecognitionAuditLog
  Id, EmployeeId?, ImageHash (not full image), MatchedName,
  Distance, Confidence, WasSuccessful, IpAddress, CreatedAtUtc
```

### 7.2 Indexes (performance)

- `AttendanceRecord (EmployeeId, Date)` — unique per day if one record per day
- `Employee (EmployeeCode)` — unique
- `Employee (IsVerified, IsActive)` — for index rebuild queries

---

## 8. API Design (New Endpoints)

Base: `/api` — all protected except login/register.

### 8.1 Auth (reuse pattern)

| Method | Endpoint | Role |
|--------|----------|------|
| POST | `/api/Auth/Login` | Anonymous |
| POST | `/api/Auth/Register` | Anonymous |
| GET | `/api/Auth/PendingUsers` | Admin |
| POST | `/api/Auth/Approve/{userId}` | Admin |
| POST | `/api/Auth/Reject/{userId}` | Admin |
| GET | `/api/Auth/Me` | Any |

### 8.2 Admin

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET/POST/PUT | `/api/Admin/Employees` | Employee CRUD |
| POST | `/api/Admin/Employees/{id}/Verify` | Approve photo → enroll face |
| GET | `/api/Admin/Attendance` | Filtered attendance list |
| GET | `/api/Admin/Reports/Monthly` | Report data |
| PUT | `/api/Admin/Settings` | Threshold, work hours |

### 8.3 Employee

| Method | Endpoint | Purpose |
|--------|----------|---------|
| POST | `/api/EmployeePortal/CheckIn` | `multipart/form-data` image |
| POST | `/api/EmployeePortal/CheckOut` | `multipart/form-data` image |
| GET | `/api/EmployeePortal/MyAttendance` | Own history |
| GET | `/api/EmployeePortal/TodayStatus` | Current day state |
| GET | `/api/EmployeePortal/Profile` | Own profile |

### 8.4 Face service (internal or Python)

| Method | Endpoint | Purpose |
|--------|----------|---------|
| POST | `/internal/face/enroll` | Photo → embedding |
| POST | `/internal/face/recognize` | Photo → `{ employeeId, name, confidence, box }` |
| POST | `/internal/face/reload-index` | Rebuild cache |

**.NET** validates JWT before calling internal face service with **API key** or mTLS.

---

## 9. Security (Client Selling Points)

This section is critical for winning the deal.

### 9.1 Authentication & authorization

| Control | Implementation |
|---------|----------------|
| Password storage | BCrypt (already in your stack) |
| API auth | JWT Bearer, short expiry (e.g. 60 min) |
| Role separation | `[Authorize(Roles = "Admin")]` vs `Employee` |
| Registration gate | Admin approval before attendance allowed |
| HTTPS only | TLS in production; HSTS |

### 9.2 Biometric & privacy

| Risk | Mitigation |
|------|------------|
| Face data sensitivity | Store **embeddings**, not reusable photos for matching (optional: delete photo after enroll) |
| GDPR / local law | Document purpose, retention period, employee consent checkbox on register |
| Spoofing (photo of photo) | Phase 2: liveness detection; Phase 1: good lighting + admin enrollment review |
| False match | Configurable threshold; log distance; admin can tune |
| Failed attempts | Rate limit scan endpoints per IP/user |
| Audit trail | `RecognitionAuditLog` without storing full images (hash only) |

### 9.3 Infrastructure

| Control | Implementation |
|---------|----------------|
| Secrets | Env vars / Azure Key Vault — never in git |
| CORS | Explicit frontend origin only |
| File upload | Max size (e.g. 2 MB), JPEG only, virus scan optional |
| Service-to-service | API key between .NET ↔ Python face service |
| DB | Encrypted connection (SSL), least-privilege DB user |

### 9.4 What to promise the client

- “Passwords are hashed; we never store plain text.”
- “Face matching uses mathematical embeddings, not sharing photos with third parties.”
- “Every check-in attempt is logged for accountability.”
- “Only verified employees can mark attendance.”
- “Admin controls who is active and enrolled.”

---

## 10. Performance (50 Employees)

At 50 employees, this system is **lightweight** if designed correctly.

| Concern | Target | Approach |
|---------|--------|----------|
| Recognition latency | &lt; 2–3 s per scan on CPU | Lazy-load models once; keep embedding index in memory |
| Index size | 50 × 512 floats ≈ 100 KB | Trivial — in-memory dictionary |
| Concurrent check-ins | Morning rush ~10–20 in 10 min | Stateless API + connection pooling |
| DB | PostgreSQL | Indexed queries; paginate reports |
| Image upload | Compress client-side JPEG | Limit resolution (e.g. 640×480) |
| Python GPU | Optional | CPU is fine for 50; GPU for &gt;200 or real-time video |

**Benchmark before demo:** 10 consecutive scans &lt; 3 s each on target hardware.

---

## 11. Frontend Options

| Option | Best for |
|--------|----------|
| **React (Vite)** | You already use this; fastest if reusing SMS frontend patterns |
| **Flutter** | Mobile-first check-in kiosk + single codebase |
| **Blazor** | All-.NET shop, smaller team |

**Must-have UI:**

- Camera capture (`getUserMedia`) → JPEG blob → `FormData` POST
- Clear feedback: “Recognized as X” / “Unknown face” / “Already checked in”
- Admin tables with filters and export

---

## 12. Phased Roadmap (What to Build & When)

### Phase 0 — Discovery & setup (3–5 days)

- [ ] Confirm client: work hours, late rules, one vs multiple check-ins per day
- [ ] Confirm hardware: employee phones vs fixed office tablet
- [ ] New solution `Attendance.sln` or fork CRUD template
- [ ] PostgreSQL + Docker Compose (.NET API + Python face service + DB)
- [ ] Strip SMS-specific modules; keep auth/middleware/DI patterns

### Phase 1 — MVP (2–3 weeks) — **sell this first**

- [ ] Auth: Admin + Employee roles, register, approve
- [ ] Employee CRUD (admin)
- [ ] Profile photo upload + admin verify → enroll embedding
- [ ] Python face service wired (enroll + recognize)
- [ ] Check-in / check-out APIs
- [ ] Employee: today status + my attendance
- [ ] Admin: today dashboard + attendance list
- [ ] Basic audit log for scans
- [ ] Deploy to staging for client UAT

**MVP exit criteria:** 5 test employees can check in/out via face; admin sees records.

### Phase 2 — Production hardening (1–2 weeks)

- [ ] Rate limiting on scan endpoints
- [ ] Monthly report + CSV export
- [ ] Settings UI (threshold, work hours)
- [ ] Email notifications (optional: late alert)
- [ ] Production deploy + backup strategy
- [ ] Employee consent + privacy notice page

### Phase 3 — Enhancements (optional upsell)

- [ ] Liveness detection (anti-spoof)
- [ ] Multi-branch / multi-location
- [ ] Native .NET face pipeline (remove Python sidecar)
- [ ] Mobile app (Flutter)
- [ ] Integration with payroll export
- [ ] Shift scheduling

---

## 13. Deployment Architecture (Production)

```text
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│   Vercel /   │     │ Render /     │     │ Neon /       │
│   Frontend   │────▶│ .NET API     │────▶│ PostgreSQL   │
└──────────────┘     └──────┬───────┘     └──────────────┘
                            │
                            ▼
                     ┌──────────────┐
                     │ Python Face  │
                     │ Service      │
                     │ (Railway/    │
                     │  same VPS)   │
                     └──────────────┘
```

| Component | Suggested host |
|-----------|----------------|
| Frontend | Vercel / Netlify |
| .NET API | Render / Azure App Service |
| PostgreSQL | Neon / Azure Database |
| Face service | Same VPS as API or separate container with GPU optional |

**Env vars (production):**

- `ConnectionStrings__PostgreSQLConnection`
- `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience`
- `Cors__AllowedOrigins`
- `FaceService__BaseUrl`, `FaceService__ApiKey`

---

## 14. Testing Strategy

| Layer | What to test |
|-------|----------------|
| Unit | Attendance rules (late, duplicate check-in, checkout without check-in) |
| Integration | .NET → Python face service contract |
| Recognition | 5+ enrolled faces, 3 unknown faces, 2 borderline threshold cases |
| Security | JWT role enforcement, unapproved user cannot scan |
| Load | 20 sequential scans without memory leak on Python service |
| UAT | Client signs off with real employees in office lighting |

---

## 15. Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Poor office lighting | Failed recognition | Enrollment in same environment; UI lighting hints |
| Buddy punching with photo | Fraud | Liveness (phase 2); admin spot checks; audit log |
| False positive match | Wrong attendance | Tune threshold; log distance; manual admin correction |
| Python service down | No check-in | Health check + fallback “contact admin” message |
| Privacy concerns | Legal/trust | Consent, retention policy, minimal image storage |
| Scope creep | Delayed delivery | Lock MVP to 2 portals + check-in/out only |

---

## 16. Client Deliverables Checklist (Proposal Package)

When selling, include:

- [ ] This roadmap (scope + phases)
- [ ] Live demo: enroll 1 face → check-in → admin dashboard
- [ ] Security one-pager (section 9 summary)
- [ ] Timeline: MVP 2–3 weeks, production 4–5 weeks total
- [ ] Support plan: 30-day bug fix window post go-live
- [ ] Optional maintenance retainer (monthly hosting + threshold tuning)

---

## 17. Must-Contain Feature List (Do Not Skip)

### Functional

- [ ] Admin login
- [ ] Employee self-register + admin approval
- [ ] Employee profile with photo
- [ ] Face enrollment after admin verification
- [ ] Face-based check-in and check-out
- [ ] Unknown face rejection
- [ ] Attendance history (employee + admin views)
- [ ] Admin dashboard (today’s attendance)
- [ ] Configurable recognition threshold

### Non-functional

- [ ] HTTPS
- [ ] JWT auth
- [ ] Role-based APIs
- [ ] Audit log for recognition attempts
- [ ] Response time acceptable on office Wi‑Fi
- [ ] Database backups
- [ ] Environment-based secrets

---

## 18. Where to Focus Your Energy (Priority Order)

1. **Face pipeline integration** — prove enroll + recognize end-to-end (biggest technical risk).
2. **Attendance business rules** — check-in/out logic must be bulletproof.
3. **Admin approval + enrollment flow** — no attendance without verified face.
4. **Auth reuse from CRUD project** — saves a week.
5. **Simple React camera UI** — one screen for employee check-in.
6. **Admin attendance table** — client’s main daily view.
7. **Reports & polish** — after MVP is accepted.
8. **Liveness / native .NET** — only after paid phase 2.

---

## 19. Suggested Solution Structure (.NET)

```text
Attendance.Domain/
  Models/ Employee, AttendanceRecord, Department, AttendanceSettings, ...
Attendance.Application/
  DTOs/, Interfaces/, Services/, Validators/
Attendance.Infrastructure/
  Persistence/, Repositories/, Migrations/
Attendance.Web/
  Controllers/ AuthController, AdminController, EmployeePortalController
  Middleware/ AccessLog, RequestLogging (reuse)
Attendance.FaceService/          (optional separate Python repo)
  face_pipeline.py, app.py, requirements.txt
```

---

## 20. Quick Start — Next Actions for You

1. **Fork or new repo** from CRUD Clean Architecture template.
2. **Spin up Python face service** from SMART-PRESENCE `face_pipeline.py`.
3. **Prove integration:** .NET POST image → Python → returns `employeeId` + confidence.
4. **Implement `Employee` + `AttendanceRecord`** entities and check-in service.
5. **Build employee camera page** (React) + admin attendance table.
6. **Demo to client** with 3–5 enrolled test users.
7. **Sign MVP scope** → Phase 1 delivery.

---

## 21. Appendix — Mapping SMART-PRESENCE → Employee System

| SMART-PRESENCE (Student) | Employee Attendance |
|--------------------------|---------------------|
| `Student` | `Employee` |
| `roll_number` | `EmployeeCode` |
| `is_verified` | `IsVerified` |
| `image` (profile photo) | `ProfilePhotoPath` |
| `_embedding_index` | DB + in-memory cache |
| `AttendanceSettings.face_recognition_threshold` | Same setting |
| `POST /api/checkin/` | `POST /api/EmployeePortal/CheckIn` |
| `POST /api/checkout/` | `POST /api/EmployeePortal/CheckOut` |
| `POST /api/scan/` | Optional unified scan endpoint |

---

## 22. Appendix — Sample Check-In Flow (Sequence)

```text
Employee opens app → Camera → Capture JPEG
  → POST /api/EmployeePortal/CheckIn (Bearer token + image)
    → .NET validates user is approved Employee
    → .NET forwards image to Face Service
    → Face Service: detect → embed → match
    → Returns { employeeId, confidence, distance }
    → .NET verifies employeeId matches logged-in user (anti-spoof proxy)
    → Creates/updates AttendanceRecord (CheckInUtc)
    → Writes RecognitionAuditLog
  → Response: { success, checkInTime, message }
```

**Important:** Even if face matches, consider requiring `matchedEmployeeId == currentUser.EmployeeId` so one person cannot check in for another if both are enrolled.

---

*Document version: 1.0 — for client proposal and internal development planning.*
