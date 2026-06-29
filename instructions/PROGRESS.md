# PROGRESS

## Current Sprint
**Sprint 1** — Critical for MVP (Must Complete)

## Current Branch
`2026-06-29_Iman_sprint-1`

## Current Task
**FIX-008** — Fix ChatController architecture (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| FIX-001 | Fix SignalR token key | **Completed** |
| FIX-002 | Fix Report reject endpoint | **Completed** |
| FIX-003 | Fix CrisisDetection exception handling | **Completed** |
| FIX-004 | Add authGuard to chat routes | **Completed** |
| FIX-005 | Add authGuard to profile route | **Completed** |
| FIX-006 | Add role restriction to PatientController | **Completed** |
| FIX-007 | Fix ChatHub authorization | **Completed** |
| FIX-008 | Fix ChatController architecture | **Completed** |
| FIX-009 | Fix NotificationController architecture | Not Started |
| FIX-010 | Fix SessionController voice memo | Not Started |
| FIX-011 | Fix 4 shared components to standalone | Not Started |

## Completion Percentage
Sprint 1: 8/11 (73%)

## Completed Tasks
1. **FIX-001** — Changed `localStorage.getItem('access_token')` to `localStorage.getItem('jalsa_token')` in `chat-room.component.ts:111` to match AuthService's TOKEN_KEY constant.
2. **FIX-002** — Added `RejectAsync` to `IReportService` and `ReportService`, updated `ReportController.Reject` to persist status change to DB instead of returning a fake response.
3. **FIX-003** — Fixed CrisisDetectionService to parse AI response JSON and only return `IsCrisis=true` when AI confirms. On AI failure or non-crisis AI response, returns `IsCrisis=false`.
4. **FIX-004** — Added `canActivate: [authGuard]` to both chatbot child routes (`''` and `':id'`) in `chatbot.routes.ts`.
5. **FIX-005** — Added `canActivate: [authGuard]` to the profile route in `auth.routes.ts`. This route was completely unprotected since it sits outside the AuthLayout children and the `/auth` parent has no guard.
6. **FIX-006** — Changed `[Authorize]` to `[Authorize(Roles = "Therapist")]` on `PatientController` to restrict patient management to therapists only.
7. **FIX-007** — Added `[Authorize]` to ChatHub class, added user identity validation via JWT claims in both `SendMessage` and `JoinConversation`, added conversation existence check in `SendMessage`.
8. **FIX-008** — Extracted ChatService from ChatController. Created IChatService interface, ChatService implementation using IUnitOfWork, 5 Chat DTOs. Controller now injects IChatService instead of JalsaDbContext.

## Remaining Tasks
FIX-009, FIX-010, FIX-011

## Build Status
- **Frontend**: Build clean (0 errors, warnings only — CSS budget + ESM)
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Frontend**: 270/270 passed (1 pre-existing error in error.interceptor.spec.ts — unrelated)
- **Backend**: 51/51 passed

## Integration Test Results
N/A

## Manual Verification Results
- ChatController no longer imports JalsaDbContext or Microsoft.EntityFrameworkCore
- ChatController injects IChatService (Clean Architecture compliant)
- ChatService uses IUnitOfWork + IGenericRepository pattern (matches SessionService, ReportService, etc.)
- 5 DTOs created: ConversationViewDto, ChatMessageViewDto, ChatHistoryDto, CreateConversationDto, SendMessageDto
- Inline request records removed from controller (moved to proper DTO files)
- Service registered as AddScoped in Program.cs
- All existing API endpoints preserved with same routes and behavior

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
**FIX-009** — Fix NotificationController architecture (Backend task)
