# PROGRESS

## Current Sprint
**Sprint 2** — High Priority

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**FIX-012** — Add therapist ownership check to ExerciseController (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| SEC-001 | Fix CORS for staging/production | **Completed** |
| SEC-002 | Protect Hangfire dashboard | **Completed** |
| SEC-003 | Move JWT key to env vars | **Completed** |
| TEST-001 | Write AuthService unit tests | **Completed** |
| TEST-002 | Write PatientService unit tests | **Completed** |
| TEST-003 | Write ChatController integration tests | **Completed** |
| TEST-004 | Write NotificationController tests | **Completed** |
| FIX-012 | Add therapist ownership check to ExerciseController | **Completed** |
| FIX-013 | Add missing DB indexes | Not Started |

## Completion Percentage
Sprint 2: 8/9 (89%)

## Completed Tasks
1. **SEC-001** — Replaced hardcoded `http://localhost:4200` CORS origin in `Program.cs` with configurable `CorsOrigins` array from `appsettings.json`. Added `CorsOrigins` section to `appsettings.json` with `http://localhost:4200` as default. Staging/production can now override origins via their own appsettings or environment variables.
2. **SEC-002** — Created `HangfireAuthorizationFilter` implementing `IDashboardAuthorizationFilter` that requires authenticated users with Admin role. Applied filter to `UseHangfireDashboard` via `DashboardOptions.Authorization` in `Program.cs`.
3. **SEC-003** — Removed hardcoded JWT key from `appsettings.Development.json`. Added `UserSecretsId` to `Jalsa.API.csproj` enabling `dotnet user-secrets`. JWT key must now come from environment variable (`Jwt__Key`), `.env` file (loaded via DotNetEnv), or user secrets. Base `appsettings.json` retains placeholder with startup validation.
4. **TEST-001** — Created `AuthServiceTests.cs` with 15 unit tests using EF Core InMemory provider. Tests cover: Register (success, duplicate email, patient role, invalid role), Login (success, wrong password, non-existent user, inactive user), Refresh Token (valid rotation, invalid token, revoked token), Revoke Token (success, already revoked), JWT claims validation, BCrypt password hashing.
5. **TEST-002** — Created `PatientServiceTests.cs` with 15 unit tests using Moq. Tests cover: Create (success, no therapist profile), GetById (own, other therapist, non-existent), GetAll (own patients only, status filter, search by name), Update (success, ownership check), Archive/Restore (success, ownership check), Delete (success, ownership check).
6. **TEST-003** — Created `ChatControllerTests.cs` with 11 controller tests using Moq. Tests cover: GetConversations (returns list, filters by patientId), GetHistory (success, 404), CreateConversation (201 created, patient not found 404), CloseConversation (success, 404), Send (success with Therapist role, 404, Patient role sets correct senderType).
7. **TEST-004** — Created `NotificationControllerTests.cs` with 7 controller tests using Moq. Tests cover: GetNotifications (returns list, unread filter, empty list), MarkAsRead (success, 404), MarkAllAsRead (returns count, zero unread).
8. **FIX-012** — Added therapist ownership checks to all ExerciseController therapist endpoints. Updated `IExerciseService` to accept `Guid userId` on therapist methods (Create, Update, Delete, GetById, GetAll, GetByPatientId, ExtendDueDate). Added `ResolveTherapistIdAsync` and `ValidatePatientOwnershipAsync` to `ExerciseService` (same pattern as PatientService). `GetAllAsync` now filters exercises to only those belonging to the therapist's patients. Added `GetCurrentUserId()` helper to controller. Patient endpoints (my, log, my/logs) remain unchanged — they use the non-ownership overload. Updated ExerciseServiceTests from 6 to 15 tests covering ownership validation for all methods.

## Remaining Tasks
FIX-013

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 108/108 passed (99 pre-existing + 9 new/updated ExerciseServiceTests)

## Integration Test Results
N/A

## Manual Verification Results
- All 15 ExerciseServiceTests pass individually
- CreateAsync validates patient belongs to therapist before creating exercise
- UpdateAsync/DeleteAsync/GetByIdAsync verify ownership via exercise → patient → therapist chain
- GetAllAsync filters to only exercises belonging to therapist's own patients
- GetByPatientIdAsync (with userId) validates patient ownership
- GetByPatientIdAsync (without userId) works for patient endpoints without ownership check
- ExtendDueDateAsync validates ownership before modifying due date
- Unauthorized access throws UnauthorizedAccessException
- Missing therapist profile throws UnauthorizedAccessException

## Regression Test Results
- Backend: 108/108 tests passed, build clean
- All 99 pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**FIX-013** — Add missing DB indexes
