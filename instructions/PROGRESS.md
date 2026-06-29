# PROGRESS

## Current Sprint
**Sprint 3** — Quality & Hardening

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**TEST-005** — Write controller tests for all 11 controllers (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| TEST-005 | Write controller tests for all 11 controllers | **Completed** |
| QUAL-001 | Add FluentValidation for all DTOs | Pending |
| QUAL-002 | Fix namespace typo Repositores→Repositories | Pending |
| QUAL-003 | Extract GetCurrentUserId to base controller | Pending |
| QUAL-004 | Move inline DTOs to proper files | Pending |
| QUAL-005 | Account lockout | Pending |
| QUAL-006 | Rate limiting | Pending |

## Completion Percentage
Sprint 3: 1/7 (14%)

## Completed Tasks
1. **TEST-005** — Created 7 new controller test files: `AuthControllerTests.cs` (8 tests: register, login, refresh, revoke, forgot-password, reset-password, get-profile with UoW mocking, update-profile), `PatientControllerTests.cs` (7 tests: getById, getAll, create, update, archive, restore, delete), `ExerciseControllerTests.cs` (10 tests: getAll, getById, getByPatientId, create, update, delete, extendDueDate for therapist + getMyExercises, logCompletion, getMyLogs for patient), `ReportControllerTests.cs` (8 tests: generate with AI service, getById, getByPatientId, update, approve, reject, export HTML file, delete), `AssessmentControllerTests.cs` (2 tests: getByPatientId, create), `IntakeControllerTests.cs` (4 tests: getByPatientId, save, submit, runOcr with UoW mocking + NotFound), `AiControllerTests.cs` (2 tests: summarizePatient, generateReportDraft). All 11 controllers now have test coverage. Total test count: 150 (up from 108).

## Remaining Tasks
QUAL-001 through QUAL-006

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 150/150 passed

## Integration Test Results
N/A

## Manual Verification Results
- All 7 new test files compile and pass
- Existing 108 tests still pass (no regressions)
- All 11 controllers now have dedicated test files

## Regression Test Results
- Backend: 150/150 tests passed, build clean
- All pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**QUAL-001** — Add FluentValidation for all DTOs
