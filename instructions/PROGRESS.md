# PROGRESS

## Current Sprint
**Sprint 5** — Frontend Quality & Architecture Fixes

## Current Branch
`2026-06-30_Iman_sprint-5`

## Current Task
**Fix 1.3 — [Q1/Q2]** — Consolidate duplicate chat interfaces (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| R1 | Remove duplicate error handling from HttpClientService | **Completed** |
| A1 | Fix BaseStateService.handleObservable callback | **Completed** |
| Q1/Q2 | Consolidate duplicate chat interfaces | **Completed** |
| P2 | Deduplicate Chart.registerables registration | Not Started |
| P1 | Reduce initial bundle size below 900 kB warning | Not Started |
| Q4 | Export StatusArPipe from barrel | Not Started |
| Q5 | Extend BaseStateService in domain state services | Not Started |

## Completion Percentage
Sprint 5: 3/7 (43%)

## Completed Tasks
1. **R1** — Removed duplicate error handling from HttpClientService:
   - Removed `catchError(this.handleError)` from all 7 methods (get, post, put, patch, delete, upload, blob)
   - Removed the `private handleError` method entirely
   - Removed unused imports: `HttpErrorResponse`, `throwError`, `catchError`
   - Error handling now flows exclusively through `errorInterceptor` (single handler)

2. **A1** — Fixed BaseStateService.handleObservable callback:
   - Added `tap(data => onSuccess(data))` operator to invoke the success callback
   - Renamed `_onSuccess` parameter to `onSuccess` (removed underscore prefix for unused param)
   - Added `tap` to imports from `rxjs/operators`
   - No behavioral change yet (no current code calls `handleObservable`); enables Q5

3. **Q1/Q2** — Consolidated duplicate chat interfaces:
   - Updated `core/models/chat.model.ts`: added `patientName` and `messageCount` to `ChatConversation`; narrowed `senderType` from `string` to `'Patient' | 'AI' | 'Therapist'` on `ChatMessage`
   - Removed local `ChatMessage` interface from `chat-room.component.ts`; imports from `core/models`
   - Removed local `ChatConversation` interface from `chat-list.component.ts`; imports from `core/models`
   - Added `tokensUsed: null, latencyMs: null` to inline ChatMessage construction in chat-room

## Remaining Tasks
- P2: Deduplicate Chart.registerables registration
- P1: Reduce initial bundle size below 900 kB warning
- Q4: Export StatusArPipe from barrel
- Q5: Extend BaseStateService in domain state services

## Build Status
- **Frontend**: Build clean (0 errors, pre-existing warnings: bundle budget, quill-delta CommonJS)

## Unit Test Results
- **Frontend**: 435/435 passed (39 test files, 0 failures)

## Integration Test Results
N/A (no integration test infrastructure)

## Manual Verification Results
- Build passes with 0 errors
- All 435 tests pass
- `ChatMessage` and `ChatConversation` now defined only in `core/models/chat.model.ts`
- Both chat-room and chat-list import from the shared model barrel
- No local interface duplicates remain in chatbot feature

## Regression Test Results
- Frontend: 435/435 tests pass (no regressions)

## Known Issues
- Pre-existing: 3 CSS budget warnings (sidebar, patient-detail, chat-room) — cosmetic, not blocking
- Pre-existing: quill-delta CommonJS warning — third-party, not fixable
- Pre-existing: bundle size 901 kB exceeds 700 kB budget (addressed in P1)

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**Fix 1.4 — [P2]** — Deduplicate Chart.registerables registration
