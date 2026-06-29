# PROGRESS

## Current Sprint
**Sprint 2** — High Priority

## Current Branch
`2026-06-29_Iman_sprint-2`

## Current Task
**SEC-001** — Fix CORS for staging/production (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| SEC-001 | Fix CORS for staging/production | **Completed** |
| SEC-002 | Protect Hangfire dashboard | Not Started |
| SEC-003 | Move JWT key to env vars | Not Started |
| TEST-001 | Write AuthService unit tests | Not Started |
| TEST-002 | Write PatientService unit tests | Not Started |
| TEST-003 | Write ChatController integration tests | Not Started |
| TEST-004 | Write NotificationController tests | Not Started |
| FIX-012 | Add therapist ownership check to ExerciseController | Not Started |
| FIX-013 | Add missing DB indexes | Not Started |

## Completion Percentage
Sprint 2: 1/9 (11%)

## Completed Tasks
1. **SEC-001** — Replaced hardcoded `http://localhost:4200` CORS origin in `Program.cs` with configurable `CorsOrigins` array from `appsettings.json`. Added `CorsOrigins` section to `appsettings.json` with `http://localhost:4200` as default. Staging/production can now override origins via their own appsettings or environment variables.

## Remaining Tasks
SEC-002, SEC-003, TEST-001, TEST-002, TEST-003, TEST-004, FIX-012, FIX-013

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings)

## Unit Test Results
- **Backend**: 51/51 passed

## Integration Test Results
N/A

## Manual Verification Results
- `appsettings.json` now contains `"CorsOrigins": [ "http://localhost:4200" ]`
- `Program.cs` reads origins from configuration with fallback to `http://localhost:4200`
- CORS policy uses `WithOrigins(corsOrigins)` with the configured array

## Regression Test Results
- Backend: 51/51 tests passed, build clean

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**SEC-002** — Protect Hangfire dashboard
