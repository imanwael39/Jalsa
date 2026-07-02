# Jalsa - Arabic Mental Health Clinic Management System

## Project Overview

**Jalsa (جلسة)** is a fully Arabic-language, RTL web-based clinic management system for licensed psychological therapists in Egypt and the Gulf region. ITI Capstone 2026, Group 6, .NET Track (5 members).

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular 21.2 (Standalone Components), TypeScript 5.9, Bootstrap 5.3 RTL, Tailwind CSS 4.1, Chart.js + ng2-charts, ngx-quill, SignalR client (@microsoft/signalr 10) |
| Backend | ASP.NET Core 8 (net8.0), C# 12, Entity Framework Core, FluentValidation, Hangfire |
| Database | SQL Server (34 tables, 1 migration) |
| AI | Azure OpenAI SDK (GPT-4o chat, Whisper STT, text-embedding-ada-002), raw SDK calls (no Semantic Kernel) |
| Auth | JWT Bearer (HS256, 1h access + 7d refresh token rotation), BCrypt password hashing |
| Real-time | SignalR WebSockets (ChatHub) |
| Testing | Frontend: Vitest 4.0 + @analogjs/vite-plugin-angular; Backend: xUnit + Moq + FluentAssertions |
| CI/CD | GitHub Actions (`.github/workflows/ci.yml`) — build + test on push/PR to develop/main |
| Background Jobs | Hangfire (ExerciseReminderJob daily at 9:00 AM) |

## Architecture

- **Frontend**: Core/Shared/Features structure with lazy-loaded routes
- **Backend**: Clean Architecture — Domain → Application → Infrastructure → API
- **State**: Angular Signals, hand-rolled per-domain state services (list + selected-item + loading + error + computed); no NgRx, no shared base class
- **Styling**: Bootstrap 5.3 RTL + Tailwind CSS 4.1 + CSS custom properties (design tokens in `styles/variables.css`)
- **Components**: All standalone, `OnPush` change detection
- **Font**: Tajawal (primary Arabic), Cairo + IBM Plex Sans Arabic (fallbacks)
- **Color Palette**: Professional Blue (#2563eb primary)

## Key Directories

```
frontend/src/app/
├── core/           # services/ (16), guards/, interceptors/, models/, state/, api/
├── shared/         # components/ (13), layouts/ (5), directives/, pipes/, validators/
├── features/       # auth, patients, sessions, exercises, reports, dashboard, chatbot
├── app.routes.ts   # Root routes with lazy loading
└── app.config.ts   # Providers: interceptors, router, animations, charts

backend/
├── Jalsa.API/          # Controllers (11), DTOs/Auth, Hubs, Services (Auth + 11 AI), HealthChecks
├── Jalsa.Domain/       # 33 entity models across 8 subfolders
├── Jalsa.Application/  # Services (6), DTOs (23), Interfaces, Validators (3)
├── Jalsa.Infrastructure/ # JalsaDbContext (34 DbSets), Migrations (1), Repositories (8)
└── Jalsa.Tests/        # 7 test classes (51 tests passing)
```

## Current Status (2026-06-28)

| Module | Frontend | Backend | Overall |
|--------|----------|---------|---------|
| Auth | Done (login, register, profile, forgot/reset pwd, /forbidden page) | Done (8 endpoints + JWT + refresh) | 98% |
| Patients | Done (list, CRUD, detail, intake, assessments) | Done (CRUD + intake + assessments + OCR) | 90% |
| Sessions | Done (list, CRUD, detail + notes + voice recorder) | Done (CRUD + notes + Whisper STT + AI summary) | 95% |
| Exercises | Done (list, assign, my-exercises) | Done (CRUD + logs + extend + reminder job) | 90% |
| Dashboard | Done (charts, stats cards) | Done (ProgressController) | 90% |
| AI Reports | Done (list, generate, detail, export HTML, reject) | Done (generate, CRUD, approve, reject, export) | 95% |
| Chatbot | Done (chat-list, chat-room + SignalR real-time) | Done (ChatController + ChatHub mapped) | 85% |
| Notifications | Done (header bell, dropdown, polling every 30s) | Done (NotificationController + DB records) | 90% |
| Tests | 270 tests (Vitest, 24 spec files) | 51 tests (xUnit, 7 test classes) | 90% |

### Build Status

- **Backend**: Builds clean (0 errors), 51 tests passing
- **Frontend**: Builds clean, 270 tests passing
- **CI/CD**: GitHub Actions active — parallel backend (.NET 8) and frontend (Node 22) jobs

## Known Issues

### Active Issues

| # | Issue | Severity | Details |
|---|-------|----------|---------|
| 1 | **Intake image endpoint mismatch** | Low | Frontend: `/api/patient/{id}/intake/image` vs Backend: `/api/patient/{id}/intake/{intakeFormId}/ocr` (different URL shape) |
| 2 | **Only Exercise validators** | Low | FluentValidation only covers 3 Exercise DTOs; all other DTOs use DataAnnotations only |
| 3 | **No integration/E2E tests** | Medium | Backend tests are unit-only (mocked); no controller or integration tests |
| 4 | **Mock API still enabled in dev** | Low | `environment.ts` has `enableMockApi: true` — may mask real API issues during development |

### Fixed Issues (complete log)

<details>
<summary>Click to expand fixed issues</summary>

1. ~~Frontend build broken~~ — Fixed: `npm install`
2. ~~No SessionController~~ — Fixed: Full CRUD + notes at `/api/sessions`
3. ~~No ReportController~~ — Fixed: AI generation + versioning at `/api/reports`
4. ~~AI endpoints unsecured~~ — Fixed: `[Authorize(Roles = "Therapist")]` on AiController + IntakeController
5. ~~Session API URL mismatch~~ — Fixed: All frontend endpoints use `/api/` prefix
6. ~~ExerciseController info leak~~ — Fixed: Removed try-catch, uses global handler
7. ~~JWT key hardcoded~~ — Fixed: Moved to appsettings.Development.json + startup validation
8. ~~IntakeController violates Clean Architecture~~ — Fixed: Uses IUnitOfWork + IntakeService
9. ~~No IntakeController CRUD~~ — Fixed: GET/POST /api/patient/{id}/intake + submit
10. ~~No AssessmentController~~ — Fixed: GET/POST /api/patient/{id}/assessments
11. ~~No Profile endpoints~~ — Fixed: GET/PUT /api/auth/profile
12. ~~Dashboard chart labels in English~~ — Fixed: Arabic labels
13. ~~Reports frontend routes empty~~ — Fixed: list, generate, detail pages
14. ~~Ownership check broken (4 services)~~ — Fixed: `ResolveTherapistIdAsync`
15. ~~Registration ignores fullName/licenseNumber~~ — Fixed: RegisterDto updated
16. ~~Dashboard analytics labels in English~~ — Fixed: `ar-EG` CultureInfo
17. ~~Session duration shows "min"~~ — Fixed: Changed to "دقيقة"
18. ~~`Galsa_DBDbContext` misspelled~~ — Fixed: Renamed to `JalsaDbContext` across 18 files
19. ~~`PatientService` in API layer~~ — Fixed: Moved to Application layer
20. ~~No frontend tests~~ — Fixed: 270 Vitest tests passing
21. ~~No CI/CD~~ — Fixed: GitHub Actions workflow
22. ~~No ChatController~~ — Fixed: Full REST API at `/api/chat` (conversations, history, send, close)
23. ~~ChatHub not mapped~~ — Fixed: `app.MapHub<ChatHub>("/chatHub")` added to Program.cs
24. ~~Chatbot frontend empty~~ — Fixed: chat-list + chat-room pages with SignalR real-time
25. ~~No in-app notifications~~ — Fixed: NotificationController + InAppNotificationService + header bell with dropdown
26. ~~`sessions/{id}/voice` no backend~~ — Fixed: `POST /api/sessions/{id}/voice` (Whisper STT, saves VoiceMemo)
27. ~~`sessions/{id}/summary` no backend~~ — Fixed: `GET /api/sessions/{id}/summary` (GPT-4o session summary)
28. ~~`reports/{id}/reject` no backend~~ — Fixed: `POST /api/reports/{id}/reject`
29. ~~`reports/{id}/export` no backend~~ — Fixed: `GET /api/reports/{id}/export` (Arabic RTL HTML download)
30. ~~Patient role stuck on login page~~ — Fixed: Role-aware redirect in login + `/forbidden` page
31. ~~`/forbidden` route missing~~ — Fixed: `ForbiddenComponent` registered at `/forbidden`, also role-aware `goHome()`
32. ~~Empty test file (UnitTest1.cs)~~ — Fixed: Deleted placeholder
33. ~~Backend tests at 28~~ — Fixed: Added SessionServiceTests + SessionControllerTests → 51 tests

</details>

## What Ships

**Completed MVP + Post-MVP**: Login/Register/Profile, Patient CRUD + Intake + Assessments, Session CRUD + Notes + Voice STT + AI Summary, Exercise CRUD + Logging, Dashboard with charts + stats, AI Report generation + approval + reject + HTML export, Role-based access (with working `/forbidden` page), Chatbot UI + REST API + SignalR real-time, In-app notification bell + polling

**Remaining (Post-MVP)**: Admin panel, Semantic search UI, Account lockout, Rate limiting, Binary PDF export (HTML export shipped instead)

## API Reference

### Controllers (11 total)

| Controller | Route | Auth | Endpoints |
|-----------|-------|------|-----------|
| AuthController | `/api/auth` | Mixed | `POST register`, `POST login`, `POST refresh`, `POST revoke`, `POST forgot-password`, `POST reset-password`, `GET profile` ★, `PUT profile` ★ |
| PatientController | `/api/patient` | ★ | `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `PATCH /{id}/archive`, `PATCH /{id}/restore`, `DELETE /{id}` |
| SessionController | `/api/sessions` | ★ Therapist | `POST`, `GET /{id}`, `GET /patient/{patientId}`, `PUT /{id}`, `DELETE /{id}`, `POST /{id}/note`, `GET /{id}/note`, `POST /{id}/voice` (Whisper STT), `GET /{id}/summary` (GPT-4o) |
| ExerciseController | `/api/exercises` | ★ | Therapist: `GET`, `GET /{id}`, `GET /patient/{patientId}`, `POST`, `PUT /{id}`, `DELETE /{id}`, `PUT /{id}/extend`; Patient: `GET /my`, `POST /log`, `GET /my/logs` |
| ReportController | `/api/reports` | ★ Therapist | `POST /generate`, `GET /{id}`, `GET /patient/{patientId}`, `PUT /{id}`, `POST /{id}/approve`, `POST /{id}/reject`, `GET /{id}/export`, `DELETE /{id}` |
| AssessmentController | `/api/patient/{id}/assessments` | ★ Therapist | `GET`, `POST` (template auto-resolution) |
| IntakeController | `/api/patient/{id}/intake` | ★ Therapist | `GET`, `POST` (save), `POST /submit`, `POST /{intakeFormId}/ocr` |
| AiController | `/api/ai` | ★ Therapist | `POST /summarize/{patientId}`, `POST /report-draft/{patientId}` |
| ProgressController | `/api/progress` | ★ Therapist | `GET /dashboard` |
| ChatController | `/api/chat` | ★ | `GET /conversations`, `GET /{conversationId}/history`, `POST /conversations` (idempotent), `PATCH /conversations/{conversationId}/close`, `POST /send` |
| NotificationController | `/api/notifications` | ★ | `GET` (last 50 + unreadCount), `PATCH /{id}/read`, `PATCH /read-all` |

★ = `[Authorize]` required

### Remaining Frontend→Backend Gaps

| Frontend Endpoint | Status |
|-------------------|--------|
| `POST /api/patient/{id}/intake/image` | Mismatch — backend is `POST /api/patient/{id}/intake/{intakeFormId}/ocr` |

### SignalR Hubs

| Hub | Route | Methods |
|-----|-------|---------|
| ChatHub | `/chatHub` | `SendMessage(conversationId, patientId, message)`, `JoinConversation(conversationId)` |

### Backend Services

**Application Layer (Clean Architecture):**
| Service | Interface | Purpose |
|---------|-----------|---------|
| PatientService | IPatientService | Patient CRUD + filtering + archive |
| SessionService | ISessionService | Session CRUD + notes |
| ExerciseService | IExerciseService | Exercise CRUD + logging + due date extension |
| ReportService | IReportService | Report generation + versioning + approval |
| AssessmentService | IAssessmentService | Assessment CRUD + template resolution |
| IntakeService | IIntakeService | Intake form CRUD + submission |
| ProgressService | IProgressService | Dashboard analytics |
| EmailNotificationService | INotificationService | Email + in-app notification DB persistence |

**API Layer (Auth + AI):**
| Service | Purpose |
|---------|---------|
| AuthService | JWT generation, password hashing, refresh tokens |
| EmailService | SMTP email delivery |
| SttService | Whisper STT transcription via Azure OpenAI AudioClient |
| ChatAiService | OpenAI conversation generation |
| ConversationMemoryService | Conversation context storage |
| CrisisDetectionService | Crisis keyword/pattern detection |
| EmbeddingService | OpenAI embedding API |
| VectorStore | SQL-based vector search |
| OcrService | Azure Computer Vision OCR |
| ReportGenerationService | AI report drafting |
| SummarizationService | Patient note + session summarization (GPT-4o) |

### Domain Model (33 entities, 34 DbSets)

| Category | Entities |
|----------|----------|
| Identity (5) | User, Role, UserRole, RefreshToken, PasswordResetToken |
| Clinic (3) | Clinic, Therapist, TherapistClinic |
| Patient (4) | Patient, PatientInvitation, IntakeForm, IntakeFormOcrExtraction |
| Session (5) | Session, SessionNote, SessionEmbedding, VoiceMemo, Assessment |
| Assessment (3) | AssessmentTemplate, AssessmentQuestion, AssessmentResponse |
| Exercise (2) | Exercise, ExerciseLog |
| Chat/AI (5) | ChatConversation, ChatMessage, AiChatLog, CrisisAlert, AiArtifact |
| Reports (2) | ReferralReport, ReportVersion |
| System (4) | UploadedFile, Notification, AuditLog, SystemSetting, AiReportGenerationLog |

## Frontend Structure

### Features (7 active)

| Feature | Pages | Key Components |
|---------|-------|----------------|
| Auth | login, register, forgot-password, reset-password, profile, **forbidden** | — |
| Patients | patient-list, patient-form, patient-detail, intake-form, assessment | — |
| Sessions | session-landing, session-list, session-form, session-detail | summary, voice-recorder |
| Exercises | exercise-list, assign-exercise, patient-exercise | — |
| Reports | report-landing, report-list, report-generate, report-detail | — |
| Dashboard | dashboard | — |
| Chatbot | **chat-list, chat-room** | — |

### Shared Components (13)

Form: Button, Input, Textarea, Checkbox, Radio, Select
Data: Table (+ column-cell directive), Pagination, StatsCard, EmptyState
Feedback: Spinner, Modal, Toast/ToastContainer
Charts: BarChart, LineChart

### Layouts (5)

MainLayout (sidebar + header + footer + router-outlet), AuthLayout, Sidebar, Header, Footer

### Core Services (16)

auth, patient, session, exercise, dashboard, report, notification, loading, app-state, navigation, exercise-state, patient-state, session-state, dashboard-state, report-state, **in-app-notification**

### Interceptors (3) + Guards (2)

- `authInterceptor` — Adds Bearer token (skips /refresh)
- `errorInterceptor` — Arabic error messages, 401 auto-logout
- `loadingInterceptor` — Spinner toggle
- `authGuard` — Redirects unauthenticated to /auth/login
- `roleGuard(roles[])` — Factory guard, redirects to /forbidden (page exists)

### Patient Role Routing

- Login redirects **Patient → `/exercises/my-exercises`** (not /dashboard which requires Therapist/Admin)
- `/forbidden` route registered in `app.routes.ts`, serves `ForbiddenComponent`
- `ForbiddenComponent.goHome()` is role-aware: Patient → /exercises/my-exercises, others → /dashboard

### Environment Config

| Env | API URL | Mock API |
|-----|---------|----------|
| Development | `http://localhost:5014` | **true** (known issue — may mask real API calls) |
| Staging | `https://staging-api.jalsa.com/api` | false |
| Production | `https://api.jalsa.com/api` | false |

## Test Coverage

### Frontend (270 tests, 24 spec files)

- Core: app, http-client, auth guard, role guard, 3 interceptors, 3 state services, state-utils, app-state, exercise service, loading, notification
- Features: assessment, intake-form, patient-detail, patient-form
- Shared: checkbox, input, radio, select, textarea

### Backend (51 tests, 7 test classes)

- ExerciseServiceTests, ProgressControllerTests, ProgressServiceTests, AssessmentServiceTests, IntakeServiceTests, **SessionServiceTests**, **SessionControllerTests** (uses EF InMemory)
- Helper: AsyncQueryProvider (test infrastructure)

## Development Rules

1. No direct push to main/develop — every change on a branch, PR required
2. Never commit/merge without explicit written permission from the project owner
3. Follow Clean Architecture — no business logic in controllers
4. All DB changes via migrations only
5. All components standalone with `OnPush` change detection
6. RTL-first — use logical CSS properties (`margin-inline-start`, etc.)
7. All UI text in Arabic
8. No `any` type in TypeScript — use proper types
9. Services expose Signals as `asReadonly()`
10. Components never call `HttpClientService` directly

## Branch & Commit Convention

Branch: `YYYY-MM-DD_Mustafa_TaskName`
Commits: `feat:`, `fix:`, `docs:`, `style:`, `refactor:`, `perf:`, `test:`, `chore:`

## Key Reference Docs

| Doc | Path | Purpose |
|-----|------|---------|
| SRS v1.0 | `docs/SRS/Jalsa_SRS_v1.0.md` | Full requirements spec (7 modules, data model, use cases) |
| MVP Plan | `MVP-EXECUTION-PLAN.md` | Sprint breakdown, task IDs, dependency map |
| Sprint Tasks | `Sprints/Sprint1-4.md` | Per-sprint task details |
| Dev Rules | `docs/DevelopmentRules.md` | Core development rules |
| Instructions | `instructions.md` | AI development workflow |
| Frontend Tasks | `Tasks/Frontend/*.md` | Detailed frontend task breakdowns (9 files) |
| ADRs | `docs/architecture/adr-001-007.md` | Architecture Decision Records (7 ADRs) |
| Coding Standards | `docs/architecture/coding-standards.md` | TypeScript/Angular conventions |
| Folder Structure | `docs/architecture/folder-structure.md` | Directory organization |
| Routing | `docs/architecture/routing-strategy.md` | Route config and guards |
| State Mgmt | `docs/architecture/state-management.md` | Signals + Services pattern |
| Design System | `docs/design-system/README.md` | Tokens, typography, components, RTL |
| Frontend Instructions | `docs/Instructions/FrontendInstructions.md` | Mandatory Angular constraints for AI agents |

## SRS Module Summary (docs/SRS/Jalsa_SRS_v1.0.md)

The SRS defines 7 modules. Current implementation status vs spec:

| SRS Module | Specified Features | Implemented | Gap |
|------------|-------------------|-------------|-----|
| 1. Auth | Register, login, JWT, refresh, roles, lockout, profile | All except lockout; /forbidden page added | Account lockout (Post-MVP) |
| 2. Patient Mgmt | CRUD, intake, assessments (PHQ-9/GAD-7/BDI), OCR, search, archive | All core features | Summary chips partial |
| 3. Session Notes | Linked sessions, structured fields, Whisper STT, auto-save, embeddings, semantic search | CRUD + notes + **Whisper STT** + **AI summary** | Auto-save, embeddings, semantic search (Post-MVP) |
| 4. Exercise Tracking | Assign, log status, reflections, progress bars, notifications, deactivate/extend | All + **in-app notifications** | — |
| 5. Dashboard | Assessment line charts, session bar charts, exercise donut, today's stats | All implemented | — |
| 6. AI Reports | One-click generate, context agent, structured Arabic report, edit, PDF, versioning | Generate + edit + versioning + **reject** + **HTML export** | Binary PDF (Post-MVP) |
| 7. Chatbot | Arabic GPT chat, crisis detection, therapist alerts, memory, audit logging | **ChatController + ChatHub + chat-list + chat-room UI + real-time SignalR** | Crisis alert UI, therapist push notifications (Post-MVP) |

## Mock Data

Development mock JSON files in `frontend/src/assets/mocks/`: dashboard, exercises, patients, reports, sessions
