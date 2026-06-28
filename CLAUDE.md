# Jalsa - Arabic Mental Health Clinic Management System

## Project Overview

**Jalsa (جلسة)** is a fully Arabic-language, RTL web-based clinic management system for licensed psychological therapists in Egypt and the Gulf region. ITI Capstone 2026, Group 6, .NET Track (5 members).

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular 21 (Standalone Components), TypeScript 5.9, Bootstrap 5.3 RTL, Tailwind CSS 4, Chart.js + ng2-charts, ngx-quill, SignalR client |
| Backend | ASP.NET Core 8, C#, Entity Framework Core, FluentValidation, Hangfire |
| Database | SQL Server (24 tables migrated), pgvector planned |
| AI | Azure OpenAI (GPT-4o, Whisper, text-embedding-ada-002), Semantic Kernel |
| Auth | JWT Bearer (HS256, 1h access + 7d refresh token rotation) |
| Real-time | SignalR WebSockets |
| Testing | Frontend: Vitest; Backend: xUnit |
| CI/CD | GitHub Actions planned |

## Architecture

- **Frontend**: Core/Shared/Features structure with lazy-loaded routes
- **Backend**: Clean Architecture — Domain → Application → Infrastructure → API
- **State**: Angular Signals + Services (no NgRx)
- **Styling**: Bootstrap 5 RTL + CSS custom properties (design tokens in `variables.css`)
- **Components**: All standalone, `OnPush` change detection

## Key Directories

```
frontend/src/app/
├── core/           # Singleton services, guards, interceptors, models, state
├── shared/         # Reusable UI components, layouts, directives, pipes
├── features/       # Lazy-loaded: auth, patients, sessions, exercises, reports, dashboard, chatbot
├── app.routes.ts   # Root routes with lazy loading
└── app.config.ts   # Application configuration

backend/
├── Jalsa.API/          # Controllers, DTOs, Hubs, Services (Auth, AI)
├── Jalsa.Domain/       # Entity models (Identity, Patient, Session, Exercise, Assessment, Report, Chat, Crisis)
├── Jalsa.Application/  # Business logic, DTOs, Repositories interfaces, Validators
├── Jalsa.Infrastructure/ # EF Core DbContext, Migrations, Repository implementations
└── Jalsa.Tests/        # xUnit tests
```

## Current Status (2026-06-28)

| Module | Frontend | Backend | Overall |
|--------|----------|---------|---------|
| Auth | Done (login, register, profile, forgot/reset pwd) | Done (9 endpoints + JWT) | 95% |
| Patients | Done (list, CRUD, detail, intake, assessments) | Done (CRUD + intake + assessments) | 90% |
| Sessions | Done (list, CRUD, detail + notes) | Done (CRUD + notes) | 90% |
| Exercises | Done (list, assign, my-exercises) | Done (CRUD + logs + extend) | 90% |
| Dashboard | Done (charts, stats, auto-refresh) | Done (ProgressController) | 90% |
| AI Reports | Done (list, generate, detail) | Done (generate, CRUD, approve) | 85% |
| Chatbot | Routes empty, no pages | Hub only, NO controller | 25% BLOCKED — Post-MVP |
| Tests | — | 29 tests (5 files) passing | 70% |

### Build Status

- **Backend**: Builds clean (0 warnings, 0 errors), 29 tests passing
- **Frontend**: Builds clean

## Bugs Fixed

1. ~~**Frontend build broken**~~ — Fixed: `npm install`
2. ~~**No SessionController**~~ — Fixed: Full CRUD + notes at `/api/sessions`
3. ~~**No ReportController**~~ — Fixed: AI generation + versioning at `/api/reports`
4. ~~**AI endpoints unsecured**~~ — Fixed: `[Authorize(Roles = "Therapist")]` on AiController + IntakeController
5. ~~**Session API URL mismatch**~~ — Fixed: All frontend endpoints now use `/api/` prefix
6. ~~**ExerciseController info leak**~~ — Fixed: Removed try-catch, uses global handler
7. ~~**JWT key hardcoded**~~ — Fixed: Moved to appsettings.Development.json + startup validation
8. ~~**IntakeController violates Clean Architecture**~~ — Fixed: Uses IUnitOfWork + IntakeService
9. ~~**No IntakeController CRUD**~~ — Fixed: GET/POST /api/patient/{id}/intake + submit
10. ~~**No AssessmentController**~~ — Fixed: GET/POST /api/patient/{id}/assessments with template auto-resolution
11. ~~**No Profile endpoints**~~ — Fixed: GET/PUT /api/auth/profile bridging User + Therapist.FullName
12. ~~**Dashboard chart labels in English**~~ — Fixed: All labels translated to Arabic
13. ~~**Reports frontend routes empty**~~ — Fixed: list, generate, detail pages wired
14. ~~**Ownership check broken (4 services)**~~ — Fixed: Session/Intake/Assessment/Report services compared JWT User ID against Therapist entity ID — resolved via `ResolveTherapistIdAsync`
15. ~~**Registration ignores fullName/licenseNumber**~~ — Fixed: RegisterDto now accepts fullName, licenseNumber, specialization
16. ~~**Dashboard analytics labels in English**~~ — Fixed: Month/week labels use `ar-EG` CultureInfo
17. ~~**Session duration shows "min" (English)**~~ — Fixed: Changed to "دقيقة"

## Remaining Issues (Post-MVP)

1. **No ChatController** — REST endpoints missing for chat history (post-MVP per scope)
2. **Chatbot frontend routes empty** — No pages, no components (post-MVP)
3. **`Galsa_DBDbContext` misspelled** — Should be `Jalsa` (risky migration rename, defer)
4. **`PatientService` misplaced** — Lives in API layer instead of Application layer (high churn, defer)

## Next Steps (Priority Order)

### MVP Polish (Ship-Ready)
1. **End-to-end smoke test** — Run backend + frontend together, test login → patient CRUD → session → report flow manually
2. **Frontend error handling polish** — Ensure all API error responses show Arabic user-friendly messages
3. **Loading states & empty states** — Verify all pages handle loading/empty/error states gracefully
4. **Form validation messages in Arabic** — Some assessment/profile forms still show English validation text

### Post-MVP Enhancements
5. **Chatbot module** — ChatController REST endpoints + frontend chat UI (currently blocked/deferred)
6. **PDF export** — Report PDF generation and download
7. **Frontend tests** — Vitest unit tests for critical components (currently 0)
8. **`PatientService` refactor** — Move from API layer to Application layer
9. **`Galsa_DBDbContext` rename** — Fix misspelling (requires migration coordination)
10. **CI/CD pipeline** — GitHub Actions for build + test on PR

## Branch Convention

```
YYYY-MM-DD_Mustafa_TaskName
```

## Commit Convention

Conventional Commits: `feat:`, `fix:`, `docs:`, `style:`, `refactor:`, `perf:`, `test:`, `chore:`

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

## Key Reference Docs

| Doc | Path | Purpose |
|-----|------|---------|
| SRS v1.0 | `docs/SRS/Jalsa_SRS_v1.0.md` | Full requirements spec (7 modules, data model, use cases) |
| MVP Plan | `MVP-EXECUTION-PLAN.md` | Sprint breakdown, task IDs, dependency map |
| Sprint Tasks | `Sprints/Sprint1-4.md` | Per-sprint task details |
| Dev Rules | `docs/DevelopmentRules.md` | Core development rules |
| Instructions | `instructions.md` | AI development workflow |
| ADRs | `docs/architecture/adr-001-007.md` | Architecture Decision Records |
| Coding Standards | `docs/architecture/coding-standards.md` | TypeScript/Angular conventions |
| Folder Structure | `docs/architecture/folder-structure.md` | Directory organization |
| Routing | `docs/architecture/routing-strategy.md` | Route config and guards |
| State Mgmt | `docs/architecture/state-management.md` | Signals + Services pattern |
| Design System | `docs/design-system/README.md` | Tokens, typography, components, RTL |

## MVP Scope (What Ships)

**Included**: Login/Register/Profile, Patient CRUD + Intake + Assessments, Session CRUD + Notes, Exercise CRUD, Dashboard with charts, AI Report generation (basic), Role-based access

**Excluded (Post-MVP)**: Chat support, Voice memo STT, PDF export, In-app notifications, Admin panel, Semantic search UI, Account lockout, Rate limiting

## API Patterns

- Backend routes: `/api/[controller]` (e.g., `/api/patients`, `/api/auth`)
- Nested routes: `/api/patient/{id}/intake`, `/api/patient/{id}/assessments`
- Frontend endpoint constants: `core/api/api-endpoints.ts`
- Auth: JWT in `localStorage`, `Authorization: Bearer <token>` header via `authInterceptor`
- Error handling: Global `errorInterceptor`, redirect to login on 401

### Controllers (9 total)

| Controller | Route | Key Endpoints |
|-----------|-------|---------------|
| AuthController | `/api/auth` | register, login, refresh, revoke, forgot-password, reset-password, profile (GET/PUT) |
| PatientController | `/api/patients` | CRUD + archive/restore |
| SessionController | `/api/sessions` | CRUD + save-note/get-note |
| ExerciseController | `/api/exercises` | CRUD + extend-due-date, my-exercises, log-completion |
| ReportController | `/api/reports` | generate, CRUD, approve |
| AssessmentController | `/api/patient/{id}/assessments` | list, create (template auto-resolution) |
| IntakeController | `/api/patient/{id}/intake` | get, save, submit, OCR |
| AiController | `/api/ai` | summarize-patient, generate-report-draft |
| ProgressController | `/api/progress` | dashboard summary |

## Mock Data

Development mock JSON files in `frontend/src/assets/mocks/`: dashboard, exercises, patients, reports, sessions
