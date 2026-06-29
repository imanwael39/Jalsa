# PROGRESS

## Current Sprint
**Sprint 1** — Critical for MVP (Must Complete)

## Current Branch
`2026-06-29_Iman_sprint-1`

## Current Task
**FIX-004** — Add authGuard to chat routes (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| FIX-001 | Fix SignalR token key | **Completed** |
| FIX-002 | Fix Report reject endpoint | **Completed** |
| FIX-003 | Fix CrisisDetection exception handling | **Completed** |
| FIX-004 | Add authGuard to chat routes | **Completed** |
| FIX-005 | Add authGuard to profile route | Not Started |
| FIX-006 | Add role restriction to PatientController | Not Started |
| FIX-007 | Fix ChatHub authorization | Not Started |
| FIX-008 | Fix ChatController architecture | Not Started |
| FIX-009 | Fix NotificationController architecture | Not Started |
| FIX-010 | Fix SessionController voice memo | Not Started |
| FIX-011 | Fix 4 shared components to standalone | Not Started |

## Completion Percentage
Sprint 1: 4/11 (36%)

## Completed Tasks
1. **FIX-001** — Changed `localStorage.getItem('access_token')` to `localStorage.getItem('jalsa_token')` in `chat-room.component.ts:111` to match AuthService's TOKEN_KEY constant.
2. **FIX-002** — Added `RejectAsync` to `IReportService` and `ReportService`, updated `ReportController.Reject` to persist status change to DB instead of returning a fake response.
3. **FIX-003** — Fixed CrisisDetectionService to parse AI response JSON and only return `IsCrisis=true` when AI confirms. On AI failure or non-crisis AI response, returns `IsCrisis=false`.
4. **FIX-004** — Added `canActivate: [authGuard]` to both chatbot child routes (`''` and `':id'`) in `chatbot.routes.ts`.

## Remaining Tasks
FIX-005, FIX-006, FIX-007, FIX-008, FIX-009, FIX-010, FIX-011

## Build Status
- **Frontend**: Build clean (0 errors, warnings only — CSS budget + ESM)
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Frontend**: 270/270 passed (1 pre-existing error in error.interceptor.spec.ts — unrelated)
- **Backend**: 51/51 passed

## Integration Test Results
N/A

## Manual Verification Results
- Both chatbot routes (`''` and `':id'`) now have `canActivate: [authGuard]`
- Parent route already had authGuard; child routes now have defense-in-depth
- Import path verified: `../../core/guards/auth.guard`

## Regression Test Results
- Frontend: 270/270 tests passed
- Frontend build: 0 errors

## Known Issues
1. Pre-existing: `error.interceptor.spec.ts` has 1 failing error (Arabic message mismatch) — unrelated to current work
2. CSS budget warnings on 4 component stylesheets — pre-existing

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**FIX-005** — Add authGuard to profile route (Frontend task)
