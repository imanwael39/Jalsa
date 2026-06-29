# PROGRESS

## Current Sprint
**Sprint 2** — High Priority

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**TEST-001** — Write AuthService unit tests (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| SEC-001 | Fix CORS for staging/production | **Completed** |
| SEC-002 | Protect Hangfire dashboard | **Completed** |
| SEC-003 | Move JWT key to env vars | **Completed** |
| TEST-001 | Write AuthService unit tests | **Completed** |
| TEST-002 | Write PatientService unit tests | Not Started |
| TEST-003 | Write ChatController integration tests | Not Started |
| TEST-004 | Write NotificationController tests | Not Started |
| FIX-012 | Add therapist ownership check to ExerciseController | Not Started |
| FIX-013 | Add missing DB indexes | Not Started |

## Completion Percentage
Sprint 2: 4/9 (44%)

## Completed Tasks
1. **SEC-001** — Replaced hardcoded `http://localhost:4200` CORS origin in `Program.cs` with configurable `CorsOrigins` array from `appsettings.json`. Added `CorsOrigins` section to `appsettings.json` with `http://localhost:4200` as default. Staging/production can now override origins via their own appsettings or environment variables.
2. **SEC-002** — Created `HangfireAuthorizationFilter` implementing `IDashboardAuthorizationFilter` that requires authenticated users with Admin role. Applied filter to `UseHangfireDashboard` via `DashboardOptions.Authorization` in `Program.cs`.
3. **SEC-003** — Removed hardcoded JWT key from `appsettings.Development.json`. Added `UserSecretsId` to `Jalsa.API.csproj` enabling `dotnet user-secrets`. JWT key must now come from environment variable (`Jwt__Key`), `.env` file (loaded via DotNetEnv), or user secrets. Base `appsettings.json` retains placeholder with startup validation.
4. **TEST-001** — Created `AuthServiceTests.cs` with 15 unit tests using EF Core InMemory provider. Tests cover: Register (success, duplicate email, patient role, invalid role), Login (success, wrong password, non-existent user, inactive user), Refresh Token (valid rotation, invalid token, revoked token), Revoke Token (success, already revoked), JWT claims validation, BCrypt password hashing.

## Remaining Tasks
TEST-002, TEST-003, TEST-004, FIX-012, FIX-013

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 66/66 passed (51 existing + 15 new AuthServiceTests)

## Integration Test Results
N/A

## Manual Verification Results
- All 15 AuthServiceTests pass individually
- Register tests verify user creation, therapist creation, role assignment, password hashing
- Login tests verify credential validation, inactive account rejection
- Token tests verify rotation (old revoked, new issued) and revocation
- JWT tests verify issuer, audience, email claims in generated token

## Regression Test Results
- Backend: 66/66 tests passed, build clean
- All 51 pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**TEST-002** — Write PatientService unit tests
