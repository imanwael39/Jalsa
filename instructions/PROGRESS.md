# PROGRESS

## Current Sprint
**Sprint 4** — Stabilization & MVP Release

## Current Branch
`2026-06-30_Iman_sprint-4`

## Current Task
**DOC-002** — Add Langfuse LLM observability (Completed)

## Sprint Progress

| Task ID | Task Name | Status |
|---------|-----------|--------|
| DEP-001 | Create CI/CD pipeline | **Completed** |
| DEP-002 | Staging environment config | **Completed** |
| DOC-001 | Create backend README | **Completed** |
| DOC-002 | Add Langfuse LLM observability | **Completed** |
| DOC-003 | Add Sentry error monitoring | Not Started |
| POLISH-001 | End-to-end smoke testing | Not Started |
| POLISH-002 | Responsive UI review | Not Started |
| POLISH-003 | Bug fixes from testing | Not Started |

## Completion Percentage
Sprint 4: 4/8 (50%)

## Completed Tasks
1. **DEP-001** — Created GitHub Actions CI/CD pipeline at `.github/workflows/ci.yml`
2. **DEP-002** — Created environment-specific appsettings for Staging and Production
3. **DOC-001** — Created `backend/README.md` with setup, architecture, and API docs
4. **DOC-002** — Added Langfuse LLM observability:
   - Created `LangfuseSettings` config class (Enabled, BaseUrl, PublicKey, SecretKey)
   - Created `ILlmObservabilityService` interface with `LlmGenerationLog` model
   - Created `LangfuseObservabilityService` — logs traces + generations to Langfuse REST API
   - Integrated into 4 AI services: SummarizationService (2 methods), ReportGenerationService, ChatAiService, CrisisDetectionService
   - Each AI call now logs: name, model, input, output, token usage, latency, metadata
   - Disabled by default (`"Enabled": false` in appsettings) — enable by setting Langfuse keys
   - Registered via `AddHttpClient<>` in Program.cs
   - Failure-safe: Langfuse errors are logged as warnings, never break AI calls
   - Build clean, 154/154 tests pass

## Remaining Tasks
DOC-003, POLISH-001, POLISH-002, POLISH-003

## Build Status
- **Backend**: Build clean (0 errors, 0 warnings) — Release configuration

## Unit Test Results
- **Backend**: 154/154 passed

## Integration Test Results
N/A

## Manual Verification Results
- Verified all 4 AI services compile with observability injection
- Verified `LangfuseObservabilityService` correctly builds Langfuse ingestion payload
- Verified Langfuse config section added to `appsettings.json` (disabled by default)
- Verified `CrisisDetectionService` internal test constructor still works (nullable observability)

## Regression Test Results
- Backend: 154/154 tests pass — no regressions
- All pre-existing tests still pass

## Known Issues
None

## Blockers
None

## Technical Debt
None introduced

## Next Recommended Task
**DOC-003** — Add Sentry error monitoring
