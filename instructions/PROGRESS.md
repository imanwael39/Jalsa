# PROGRESS

## Current Sprint
**Sprint 2** — High Priority

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**FIX-013** — Add missing DB indexes (Completed)

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
| FIX-013 | Add missing DB indexes | **Completed** |

## Completion Percentage
Sprint 2: 9/9 (100%) ✅

## Completed Tasks
1. **SEC-001** — Replaced hardcoded `http://localhost:4200` CORS origin in `Program.cs` with configurable `CorsOrigins` array from `appsettings.json`. Added `CorsOrigins` section to `appsettings.json` with `http://localhost:4200` as default. Staging/production can now override origins via their own appsettings or environment variables.
2. **SEC-002** — Created `HangfireAuthorizationFilter` implementing `IDashboardAuthorizationFilter` that requires authenticated users with Admin role. Applied filter to `UseHangfireDashboard` via `DashboardOptions.Authorization` in `Program.cs`.
3. **SEC-003** — Removed hardcoded JWT key from `appsettings.Development.json`. Added `UserSecretsId` to `Jalsa.API.csproj` enabling `dotnet user-secrets`. JWT key must now come from environment variable (`Jwt__Key`), `.env` file (loaded via DotNetEnv), or user secrets. Base `appsettings.json` retains placeholder with startup validation.
4. **TEST-001** — Created `AuthServiceTests.cs` with 15 unit tests using EF Core InMemory provider. Tests cover: Register (success, duplicate email, patient role, invalid role), Login (success, wrong password, non-existent user, inactive user), Refresh Token (valid rotation, invalid token, revoked token), Revoke Token (success, already revoked), JWT claims validation, BCrypt password hashing.
5. **TEST-002** — Created `PatientServiceTests.cs` with 15 unit tests using Moq. Tests cover: Create (success, no therapist profile), GetById (own, other therapist, non-existent), GetAll (own patients only, status filter, search by name), Update (success, ownership check), Archive/Restore (success, ownership check), Delete (success, ownership check).
6. **TEST-003** — Created `ChatControllerTests.cs` with 11 controller tests using Moq. Tests cover: GetConversations (returns list, filters by patientId), GetHistory (success, 404), CreateConversation (201 created, patient not found 404), CloseConversation (success, 404), Send (success with Therapist role, 404, Patient role sets correct senderType).
7. **TEST-004** — Created `NotificationControllerTests.cs` with 7 controller tests using Moq. Tests cover: GetNotifications (returns list, unread filter, empty list), MarkAsRead (success, 404), MarkAllAsRead (returns count, zero unread).
8. **FIX-012** — Added therapist ownership checks to all ExerciseController therapist endpoints. Updated `IExerciseService` to accept `Guid userId` on therapist methods (Create, Update, Delete, GetById, GetAll, GetByPatientId, ExtendDueDate). Added `ResolveTherapistIdAsync` and `ValidatePatientOwnershipAsync` to `ExerciseService` (same pattern as PatientService). `GetAllAsync` now filters exercises to only those belonging to the therapist's patients. Added `GetCurrentUserId()` helper to controller. Patient endpoints (my, log, my/logs) remain unchanged — they use the non-ownership overload. Updated ExerciseServiceTests from 6 to 15 tests covering ownership validation for all methods.
9. **FIX-013** — Added explicit `HasIndex()` calls in `JalsaDbContext` Fluent API for `Sessions.PatientId`, `Exercises.PatientId`, `ChatMessages.ConversationId`, and `Notifications.RecipientUserId`. Investigation confirmed these indexes already exist in the database — EF Core created them automatically from FK relationships in the InitialCreate migration. The explicit Fluent API calls make the intent clear and ensure indexes survive convention changes. No new migration needed since indexes are already present in the database schema.

## Remaining Tasks
None — Sprint 2 is complete.

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 108/108 passed

## Integration Test Results
N/A

## Manual Verification Results
- Verified InitialCreate migration already contains: IX_Sessions_PatientId (line 1095), IX_Exercises_PatientId (line 967), IX_ChatMessages_ConversationId (line 940), IX_Notifications_RecipientUserId (line 982)
- DbContext Fluent API now explicitly declares all 4 indexes
- No migration needed — indexes already exist in database

## Regression Test Results
- Backend: 108/108 tests passed, build clean
- All pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**Sprint 3** — Begin with TEST-005 (Write integration tests)
