# PROGRESS

## Current Sprint
**Sprint 3** — Quality & Hardening

## Current Branch
`2026-06-29_Iman_sprint-3`

## Current Task
**QUAL-006** — Add rate limiting (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| TEST-005 | Write controller tests for all 11 controllers | **Completed** |
| QUAL-001 | Add FluentValidation for all DTOs | **Completed** |
| QUAL-002 | Fix namespace typo Repositores→Repositories | **Completed** |
| QUAL-003 | Extract GetCurrentUserId to base controller | **Completed** |
| QUAL-004 | Move inline DTOs to proper files | **Completed** |
| QUAL-005 | Account lockout | **Completed** |
| QUAL-006 | Rate limiting | **Completed** |

## Completion Percentage
Sprint 3: 7/7 (100%)

## Completed Tasks
1. **TEST-005** — Created 7 new controller test files covering all 11 controllers. Total test count: 150 (up from 108).
2. **QUAL-001** — Created 11 new FluentValidation validators across 6 modules: Patient (PatientCreateDtoValidator, PatientUpdateDtoValidator), Session (SessionCreateDtoValidator, SessionUpdateDtoValidator, SessionNoteDtoValidator), Report (ReportGenerateDtoValidator, ReportUpdateDtoValidator), Intake (IntakeFormSaveDtoValidator), Assessment (AssessmentCreateDtoValidator), Chat (CreateConversationDtoValidator, SendMessageDtoValidator). All validators follow the existing pattern (Exercise validators). Auto-discovered by `AddValidatorsFromAssemblyContaining` in Program.cs. Total validators: 14 (3 existing Exercise + 11 new).
3. **QUAL-002** — Verified namespace typo `Repositores` does NOT exist in the codebase. All 18+ repository-related files already use correct spelling `Repositories`. No changes needed. Build clean, 149/149 tests pass.
4. **QUAL-003** — Created `BaseController.cs` with `GetCurrentUserId()` method. Updated 9 controllers to inherit from `BaseController` instead of `ControllerBase`: AuthController, PatientController, SessionController, ReportController, NotificationController, IntakeController, AssessmentController, ChatController, ExerciseController. Removed duplicate `GetCurrentUserId()` from all 9 controllers. Removed unused `using System.Security.Claims` and `using Jalsa.API.Exceptions` where no longer needed. Build clean, 149/149 tests pass.
5. **QUAL-004** — Moved 4 inline DTOs from controllers to proper files:
   - `ExtendDueDateRequest` → `Jalsa.Application/DTOs/Exercise/ExtendDueDateRequest.cs`
   - `OcrRequest` → `Jalsa.Application/DTOs/Intake/OcrRequest.cs`
   - `SummarizeRequest` → `Jalsa.API/DTOs/AI/AiRequestDtos.cs`
   - `ReportDraftRequest` → `Jalsa.API/DTOs/AI/AiRequestDtos.cs`
   Updated 3 controllers (ExerciseController, AiController, IntakeController) to use new DTOs. Updated AiControllerTests to use new namespace. Build clean, 149/149 tests pass.
6. **QUAL-005** — Added account lockout to AuthService.LoginAsync:
   - Added `FailedLoginAttempts` (int) and `LockoutEnd` (DateTime?) to User entity
   - Created EF Core migration `AddAccountLockoutFields`
   - Added Fluent API configuration in JalsaDbContext
   - Implemented lockout logic: 5 failed attempts → 15 min lockout
   - Successful login resets counter
   - Added 5 tests: increment counter, lock after 5 attempts, locked returns 403, reset on success, expired lockout allows login
   - Build clean, 154/154 tests pass.
7. **QUAL-006** — Added rate limiting middleware:
   - Added `Microsoft.AspNetCore.RateLimiting` imports
   - Configured two FixedWindowLimiter policies: "general" (100 req/min) and "ai" (10 req/min)
   - Added `[EnableRateLimiting("general")]` to BaseController (9 controllers) and ProgressController
   - Added `[EnableRateLimiting("ai")]` to AiController
   - Added `app.UseRateLimiter()` middleware with 429 JSON response
   - Build clean, 154/154 tests pass.

## Remaining Tasks
None — Sprint 3 complete. Next: Sprint 4 tasks (CI/CD, staging config, documentation, monitoring, smoke testing)

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 149/149 passed

## Integration Test Results
N/A

## Manual Verification Results
- Created 3 new DTO files in proper locations
- Removed all inline DTOs from 3 controllers
- Updated test file to use new namespace
- No inline DTO definitions remain in any controller

## Regression Test Results
- Backend: 149/149 tests passed, build clean
- All pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
Reduced — eliminated inline DTO duplication across 3 controllers

## Next Recommended Task
**QUAL-005** — Add account lockout
