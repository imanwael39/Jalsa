# PROGRESS

## Current Sprint
**Sprint 4** — Stabilization & MVP Release

## Current Branch
`2026-06-30_Iman_sprint-4`

## Current Task
**DOC-003** — Add Sentry error monitoring (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| DEP-001 | Create CI/CD pipeline | **Completed** |
| DEP-002 | Staging environment config | **Completed** |
| DOC-001 | Create backend README | **Completed** |
| DOC-002 | Add Langfuse LLM observability | **Completed** |
| DOC-003 | Add Sentry error monitoring | **Completed** |
| POLISH-001 | End-to-end smoke testing | Not Started |
| POLISH-002 | Responsive UI review | Not Started |
| POLISH-003 | Bug fixes from testing | Not Started |

## Completion Percentage
Sprint 4: 5/8 (62.5%)

## Completed Tasks
1. **DEP-001** — Created GitHub Actions CI/CD pipeline at `.github/workflows/ci.yml`
2. **DEP-002** — Created environment-specific appsettings for Staging and Production
3. **DOC-001** — Created `backend/README.md` with setup, architecture, and API docs
4. **DOC-002** — Added Langfuse LLM observability across 4 AI services
5. **DOC-003** — Added Sentry error monitoring (frontend + backend):
   - **Backend**: Added `Sentry.AspNetCore` 4.12.1 NuGet package. Configured `builder.WebHost.UseSentry()` in Program.cs with DSN from config, environment-aware, traces sample rate 0.2, PII disabled. Added `app.UseSentryTracing()` middleware. Added `Sentry` section to `appsettings.json` (empty DSN = disabled by default).
   - **Frontend**: Added `@sentry/angular` npm package. Initialized Sentry in `main.ts` (conditional on `sentryDsn` being set). Added `ErrorHandler` provider using `Sentry.createErrorHandler()` and `TraceService` in `app.config.ts`. Added `sentryDsn` field to `Environment` model and all 3 environment files (dev, staging, prod — empty by default).
   - Backend build: 0 errors, 0 warnings; 154/154 tests pass
   - Frontend build: clean (pre-existing CSS budget + quill warnings); 270/270 tests pass

## Remaining Tasks
POLISH-001, POLISH-002, POLISH-003

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings) — Release configuration
- **Frontend**: Build clean — production configuration

## Unit Test Results
- **Backend**: 154/154 passed
- **Frontend**: 270/270 passed (24 test files)

## Integration Test Results
N/A

## Manual Verification Results
- Verified `Sentry.AspNetCore` package installed and listed in csproj
- Verified `@sentry/angular` package installed in frontend
- Verified Sentry initializes conditionally (empty DSN = no-op)
- Verified ErrorHandler provider registered in app.config.ts
- Verified backend middleware pipeline order correct (UseSentryTracing after UseRouting)
- Verified all environment files updated with `sentryDsn` field

## Regression Test Results
- Backend: 154/154 tests pass — no regressions
- Frontend: 270/270 tests pass — no regressions

## Known Issues
- Pre-existing: 1 non-fatal error in `error.interceptor.spec.ts` (unrelated to Sentry changes)
- Pre-existing: 2 CSS budget warnings (patient-detail, chat-room)

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**POLISH-001** — End-to-end smoke testing
