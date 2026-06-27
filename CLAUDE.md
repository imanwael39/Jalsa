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

## Current Status (2026-06-27)

| Module | Frontend | Backend | Overall |
|--------|----------|---------|---------|
| Auth | 75% | 75% | 75% |
| Patients | 65% | 65% | 65% |
| Sessions | UI wired | API done | 65% |
| Exercises | 70% | 70% | 70% |
| Dashboard | UI wired | API done | 65% |
| AI Reports | UI wired | API done | 65% |
| Chatbot | Routes empty | Hub only, NO controller | 25% BLOCKED |

### Build Status

- **Backend**: Builds clean (0 warnings, 0 errors)
- **Frontend**: Builds clean (after `npm install`)

## Bugs Fixed

1. ~~**Frontend build broken**~~ — Fixed: `npm install`
2. ~~**No SessionController**~~ — Fixed: Full CRUD + notes at `/api/sessions`
3. ~~**No ReportController**~~ — Fixed: AI generation + versioning at `/api/reports`
4. ~~**AI endpoints unsecured**~~ — Fixed: `[Authorize(Roles = "Therapist")]` on AiController + IntakeController
5. ~~**Session API URL mismatch**~~ — Fixed: All frontend endpoints now use `/api/` prefix
6. ~~**ExerciseController info leak**~~ — Fixed: Removed try-catch, uses global handler
7. ~~**JWT key hardcoded**~~ — Fixed: Moved to appsettings.Development.json + startup validation
8. ~~**IntakeController violates Clean Architecture**~~ — Fixed: Uses IUnitOfWork

## Remaining Issues (Post-MVP)

1. **No ChatController** — REST endpoints missing for chat history (post-MVP per scope)
2. **`Galsa_DBDbContext` misspelled** — Should be `Jalsa` (risky migration rename, defer)
3. **`PatientService` misplaced** — Lives in API layer instead of Application layer (high churn, defer)
4. **Reports and Chatbot frontend routes are empty arrays** — No pages wired

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
- Frontend endpoint constants: `core/api/api-endpoints.ts`
- Auth: JWT in `localStorage`, `Authorization: Bearer <token>` header via `authInterceptor`
- Error handling: Global `errorInterceptor`, redirect to login on 401

## Mock Data

Development mock JSON files in `frontend/src/assets/mocks/`: dashboard, exercises, patients, reports, sessions
