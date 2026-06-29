# PROGRESS

## Current Sprint
**Sprint 1** — Critical for MVP (Must Complete)

## Current Branch
`2026-06-29_Iman_sprint-1`

## Current Task
**FIX-002** — Fix Report reject endpoint (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| FIX-001 | Fix SignalR token key | **Completed** |
| FIX-002 | Fix Report reject endpoint | **Completed** |
| FIX-003 | Fix CrisisDetection exception handling | Not Started |
| FIX-004 | Add authGuard to chat routes | Not Started |
| FIX-005 | Add authGuard to profile route | Not Started |
| FIX-006 | Add role restriction to PatientController | Not Started |
| FIX-007 | Fix ChatHub authorization | Not Started |
| FIX-008 | Fix ChatController architecture | Not Started |
| FIX-009 | Fix NotificationController architecture | Not Started |
| FIX-010 | Fix SessionController voice memo | Not Started |
| FIX-011 | Fix 4 shared components to standalone | Not Started |

## Completion Percentage
Sprint 1: 2/11 (18%)

## Completed Tasks
1. **FIX-001** — Changed `localStorage.getItem('access_token')` to `localStorage.getItem('jalsa_token')` in `chat-room.component.ts:111` to match AuthService's TOKEN_KEY constant.
2. **FIX-002** — Added `RejectAsync` to `IReportService` and `ReportService`, updated `ReportController.Reject` to persist status change to DB instead of returning a fake response.

## Remaining Tasks
FIX-003, FIX-004, FIX-005, FIX-006, FIX-007, FIX-008, FIX-009, FIX-010, FIX-011

## Build Status
- **Frontend**: Build clean (0 errors, warnings only — CSS budget + ESM)
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Frontend**: 270/270 passed (1 pre-existing error in error.interceptor.spec.ts — unrelated)
- **Backend**: 51/51 passed

## Integration Test Results
N/A

## Manual Verification Results
- `IReportService` now declares `RejectAsync(Guid id, Guid therapistId)`
- `ReportService.RejectAsync` sets `Status = "Rejected"`, updates timestamp, persists via UoW
- `ReportController.Reject` calls `_reportService.RejectAsync` and returns full `ReportViewDto`
- Pattern matches existing `ApproveAsync` implementation

## Regression Test Results
- Backend: 51/51 tests passed
- Backend build: 0 errors, 0 warnings

## Known Issues
1. Pre-existing: `error.interceptor.spec.ts` has 1 failing error (Arabic message mismatch) — unrelated to current work
2. CSS budget warnings on 4 component stylesheets — pre-existing

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**FIX-003** — Fix CrisisDetection exception handling (Backend task)
