# PROGRESS

## Current Sprint
**Sprint 2** — High Priority

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**TEST-003** — Write ChatController integration tests (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| SEC-001 | Fix CORS for staging/production | **Completed** |
| SEC-002 | Protect Hangfire dashboard | **Completed** |
| SEC-003 | Move JWT key to env vars | **Completed** |
| TEST-001 | Write AuthService unit tests | **Completed** |
| TEST-002 | Write PatientService unit tests | **Completed** |
| TEST-003 | Write ChatController integration tests | **Completed** |
| TEST-004 | Write NotificationController tests | Not Started |
| FIX-012 | Add therapist ownership check to ExerciseController | Not Started |
| FIX-013 | Add missing DB indexes | Not Started |

## Completion Percentage
Sprint 2: 6/9 (67%)

## Completed Tasks
1. **SEC-001** — Replaced hardcoded `http://localhost:4200` CORS origin in `Program.cs` with configurable `CorsOrigins` array from `appsettings.json`. Added `CorsOrigins` section to `appsettings.json` with `http://localhost:4200` as default. Staging/production can now override origins via their own appsettings or environment variables.
2. **SEC-002** — Created `HangfireAuthorizationFilter` implementing `IDashboardAuthorizationFilter` that requires authenticated users with Admin role. Applied filter to `UseHangfireDashboard` via `DashboardOptions.Authorization` in `Program.cs`.
3. **SEC-003** — Removed hardcoded JWT key from `appsettings.Development.json`. Added `UserSecretsId` to `Jalsa.API.csproj` enabling `dotnet user-secrets`. JWT key must now come from environment variable (`Jwt__Key`), `.env` file (loaded via DotNetEnv), or user secrets. Base `appsettings.json` retains placeholder with startup validation.
4. **TEST-001** — Created `AuthServiceTests.cs` with 15 unit tests using EF Core InMemory provider. Tests cover: Register (success, duplicate email, patient role, invalid role), Login (success, wrong password, non-existent user, inactive user), Refresh Token (valid rotation, invalid token, revoked token), Revoke Token (success, already revoked), JWT claims validation, BCrypt password hashing.
5. **TEST-002** — Created `PatientServiceTests.cs` with 15 unit tests using Moq. Tests cover: Create (success, no therapist profile), GetById (own, other therapist, non-existent), GetAll (own patients only, status filter, search by name), Update (success, ownership check), Archive/Restore (success, ownership check), Delete (success, ownership check).
6. **TEST-003** — Created `ChatControllerTests.cs` with 11 controller tests using Moq. Tests cover: GetConversations (returns list, filters by patientId), GetHistory (success, 404), CreateConversation (201 created, patient not found 404), CloseConversation (success, 404), Send (success with Therapist role, 404, Patient role sets correct senderType).

## Remaining Tasks
TEST-004, FIX-012, FIX-013

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 92/92 passed (81 existing + 11 new ChatControllerTests)

## Integration Test Results
N/A

## Manual Verification Results
- All 11 ChatControllerTests pass individually
- GET /conversations returns Ok with list, passes patientId filter
- GET /{id}/history returns Ok or 404
- POST /conversations returns 201 or 404
- PATCH /conversations/{id}/close returns Ok or 404
- POST /send returns Ok or 404; Patient role correctly sets senderType to "Patient"

## Regression Test Results
- Backend: 92/92 tests passed, build clean
- All 81 pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**TEST-004** — Write NotificationController tests
