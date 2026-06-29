# PROGRESS

## Current Sprint
**Sprint 2** — High Priority

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**SEC-003** — Move JWT key to env vars (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| SEC-001 | Fix CORS for staging/production | **Completed** |
| SEC-002 | Protect Hangfire dashboard | **Completed** |
| SEC-003 | Move JWT key to env vars | **Completed** |
| TEST-001 | Write AuthService unit tests | Not Started |
| TEST-002 | Write PatientService unit tests | Not Started |
| TEST-003 | Write ChatController integration tests | Not Started |
| TEST-004 | Write NotificationController tests | Not Started |
| FIX-012 | Add therapist ownership check to ExerciseController | Not Started |
| FIX-013 | Add missing DB indexes | Not Started |

## Completion Percentage
Sprint 2: 3/9 (33%)

## Completed Tasks
1. **SEC-001** — Replaced hardcoded `http://localhost:4200` CORS origin in `Program.cs` with configurable `CorsOrigins` array from `appsettings.json`. Added `CorsOrigins` section to `appsettings.json` with `http://localhost:4200` as default. Staging/production can now override origins via their own appsettings or environment variables.
2. **SEC-002** — Created `HangfireAuthorizationFilter` implementing `IDashboardAuthorizationFilter` that requires authenticated users with Admin role. Applied filter to `UseHangfireDashboard` via `DashboardOptions.Authorization` in `Program.cs`.
3. **SEC-003** — Removed hardcoded JWT key from `appsettings.Development.json`. Added `UserSecretsId` to `Jalsa.API.csproj` enabling `dotnet user-secrets`. JWT key must now come from environment variable (`Jwt__Key`), `.env` file (loaded via DotNetEnv), or user secrets. Base `appsettings.json` retains placeholder with startup validation.

## Remaining Tasks
TEST-001, TEST-002, TEST-003, TEST-004, FIX-012, FIX-013

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 51/51 passed

## Integration Test Results
N/A

## Manual Verification Results
- `appsettings.Development.json` no longer contains any JWT key
- `appsettings.json` retains `"REPLACE_WITH_ENV_VAR_OR_USER_SECRETS"` placeholder
- `Jalsa.API.csproj` now has `UserSecretsId` for `dotnet user-secrets` support
- Program.cs startup validation rejects placeholder/missing key
- DotNetEnv loads `.env` at startup; `.env` is gitignored
- Three ways to provide key: env var `Jwt__Key`, `.env` file, or `dotnet user-secrets`

## Regression Test Results
- Backend: 51/51 tests passed, build clean

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**TEST-001** — Write AuthService unit tests
