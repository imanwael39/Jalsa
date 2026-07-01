# JALSA MVP EXECUTION PLAN

> **Date:** June 29, 2026
> **Author:** Senior Technical Program Manager
> **Status:** Active
> **Sprint Cadence:** 1 week per sprint (4 sprints total)
> **Based On:** `JALSA-AUDIT-REPORT.md` (June 29, 2026 — every finding evidence-based)

---

## PHASE 1 — CURRENT PROJECT STATUS SUMMARY

### Completion Dashboard

| Layer | Completion | Status |
|-------|-----------|--------|
| **Frontend** | 92% | All 7 feature modules implemented with real API connections; 28 screens built; 4 shared components missing `standalone: true` |
| **Backend** | 88% | All 11 controllers functional; architecture violations in 3 controllers (Chat, Notification, Session voice memo) |
| **Database** | 95% | 34 DbSets, single clean migration, proper indexes; missing 4 performance indexes |
| **AI Features** | 85% | 8 AI services implemented; vector search is in-memory cosine similarity |
| **Testing** | 45% | 51 backend + 24 frontend spec files; no integration or E2E tests |
| **Documentation** | 80% | SRS, architecture, sprint plans exist; no backend README |
| **Overall** | **82%** | **MVP functional with critical issues requiring fixes** |

### Critical Blockers 🔥

| # | Issue | Severity | File | Effort |
|---|-------|----------|------|--------|
| 1 | **SignalR token key mismatch** — frontend reads `access_token`, stores `jalsa_token` | 🔴 Critical | `chat-room.component.ts:111` | 1h |
| 2 | **ChatController bypasses Clean Architecture** — uses DbContext directly | 🔴 Critical | `ChatController.cs` | 4h |
| 3 | **NotificationController bypasses Clean Architecture** | 🔴 Critical | `NotificationController.cs` | 3h |
| 4 | **ReportController.Reject is a no-op stub** — doesn't persist | 🔴 Critical | `ReportController.cs:67-73` | 2h |
| 5 | **CrisisDetectionService swallows exceptions** — returns `IsCrisis=true` on AI failure | 🔴 Critical | `CrisisDetectionService.cs:55-63` | 2h |
| 6 | **ChatHub has no authorization** — any user can impersonate any patient | 🔴 Critical | `ChatHub.cs:30` | 3h |
| 7 | **PatientController has no role restriction** — Patients can manage other Patients | 🟡 High | `PatientController.cs:12` | 1h |
| 8 | **3 shared components missing `standalone: true`** | 🟡 High | Pagination, EmptyState, Spinner, Modal | 2h |

### Major Risks

| # | Risk | Impact | Probability |
|---|------|--------|-------------|
| 1 | **No CI/CD pipeline** — `.github/workflows/` does not exist | High | High |
| 2 | **No integration tests** — all 51 backend tests are unit-only with mocked dependencies | High | High |
| 3 | **VectorStore loads ALL embeddings into memory** for cosine similarity — O(n) per query | High | Medium |
| 4 | **CORS hardcoded to localhost:4200** — won't work in staging/production | Medium | High |
| 5 | **JWT key hardcoded in appsettings.Development.json** — security risk | Medium | High |
| 6 | **Hangfire dashboard unprotected** — anyone can access at `/hangfire` | Medium | Medium |

### Technical Debt

| # | Debt | Impact | Effort to Fix |
|---|------|--------|---------------|
| 1 | `GetCurrentUserId()` duplicated in 7 controllers | Code duplication | Small |
| 2 | Namespace typo `Repositores` (missing 'i') across Application layer | Consistency | Medium (18+ files) |
| 3 | DTOs defined inside controller files (Exercise, Intake, Ai) | Clean Architecture | Small |
| 4 | `PatientService` originally in API layer, now in Application — incomplete migration of Chat/Notification | Architecture | Medium |
| 5 | `EmailNotificationService` sends email via `Console.WriteLine` when SMTP not configured | Dev-only | Small |
| 6 | No FluentValidation for Patient, Session, Report, Intake, or Assessment DTOs | Validation gap | Medium |
| 7 | `OcrService` hardcodes `Confidence = 0.85m` instead of computing from AI | Accuracy | Small |
| 8 | `ConversationMemoryService` loads ALL patient artifacts for similarity search | Performance | Large |

---

## PHASE 2 — DEFINE MVP

### MVP Principle

> **A therapist can log in, manage patients, create/view sessions with notes, assign exercises, see a dashboard, generate AI reports, and use the chatbot — all with proper security and architecture.**

### Module Classification

| Module | Current Status | MVP Classification | Reason |
|--------|---------------|-------------------|--------|
| **Authentication** | ✅ 98% | **Required ✅** | Foundation — only missing account lockout |
| **Patient Management** | ✅ 92% | **Required ✅** | Core entity — needs role restriction |
| **Session Management** | ✅ 95% | **Required ✅** | Core clinical workflow — minor architecture fix |
| **Exercise Management** | ✅ 91% | **Required ✅** | Core feature — needs ownership check |
| **Dashboard** | ✅ 92% | **Required ✅** | Therapist overview — complete |
| **AI Reports** | ✅ 92% | **Required ✅** | Key differentiator — reject endpoint is stub |
| **Chat System** | ⚠ 82% | **Required ✅** | Safety-critical — token bug + no auth on hub |
| **Notifications (In-App)** | ✅ 87% | **Required ✅** | In-app bell works — architecture fix needed |
| **Voice Memo STT** | ✅ Implemented | **Post-MVP 🚀** | Already working, minor architecture fix |
| **PDF Export** | ❌ 0% | **Post-MVP 🚀** | HTML export shipped; PDF is enhancement |
| **Admin Panel** | ❌ 0% | **Post-MVP 🚀** | Single-admin setup sufficient |
| **Semantic Search** | ⚠ VectorStore exists | **Post-MVP 🚀** | In-memory; can add pgvector later |
| **Account Lockout** | ❌ 0% | **Post-MVP 🚀** | Security hardening |
| **Rate Limiting** | ❌ 0% | **Post-MVP 🚀** | Security hardening |
| **CI/CD Pipeline** | ❌ 0% | **Post-MVP 🚀** | Quality assurance — deploy manually first |

### MVP SUCCESS CRITERIA — Users Can:

1. Register as Therapist, Login with JWT, view/edit profile
2. Create, view, edit, archive, restore patients (Therapist-only access)
3. Fill intake form for a patient (with OCR support)
4. Take assessments (PHQ-9, GAD-7, BDI) for a patient
5. Create sessions with structured notes (observations, interventions, patient response, homework, goals)
6. Upload voice memos with Whisper STT transcription
7. View session history per patient
8. Assign exercises to patients, set due dates, extend due dates
9. View exercise list, log completion/partial/skipped
10. View dashboard with patient counts, session counts, exercise completion rate, assessment trends
11. Generate an AI draft referral report, approve or reject it, export as HTML
12. Use between-session chatbot with crisis detection
13. Receive in-app notifications for exercise reminders

---

## PHASE 3 — MVP TASK BREAKDOWN STRUCTURE

### Critical for MVP (Must Complete) — Sprint 1

| Task ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---------|-----------|--------|-------------|-------------|----------|--------|-------|--------|
| **FIX-001** | Fix SignalR token key | Chat | Change `access_token` to `jalsa_token` in `chat-room.component.ts:111` | None | 🔴 Critical | 1h | Frontend | Not Started |
| **FIX-002** | Fix Report reject endpoint | Reports | Implement actual rejection persistence in `ReportController.Reject` (`ReportController.cs:67-73`) | None | 🔴 Critical | 2h | Backend | Not Started |
| **FIX-003** | Fix CrisisDetection exception handling | AI | Don't return `IsCrisis=true` on AI failure (`CrisisDetectionService.cs:55-63`) | None | 🔴 Critical | 2h | Backend | Not Started |
| **FIX-004** | Add authGuard to chat routes | Chat | Add `authGuard` to `/chatbot` and `/chatbot/:id` routes | None | 🔴 Critical | 1h | Frontend | Not Started |
| **FIX-005** | Add authGuard to profile route | Auth | Add `authGuard` to `/auth/profile` route | None | 🔴 Critical | 1h | Frontend | Not Started |
| **FIX-006** | Add role restriction to PatientController | Patients | Add `[Authorize(Roles="Therapist")]` (`PatientController.cs:12`) | None | 🔴 Critical | 1h | Backend | Not Started |
| **FIX-007** | Fix ChatHub authorization | Chat | Validate user identity on `SendMessage` (`ChatHub.cs:30`) | None | 🔴 Critical | 3h | Backend | Not Started |
| **FIX-008** | Fix ChatController architecture | Chat | Extract service layer from DbContext usage (`ChatController.cs`) | None | 🔴 Critical | 4h | Backend | Not Started |
| **FIX-009** | Fix NotificationController architecture | Notifications | Extract service layer from DbContext usage (`NotificationController.cs`) | None | 🔴 Critical | 3h | Backend | Not Started |
| **FIX-010** | Fix SessionController voice memo | Sessions | Use UoW instead of direct DbContext (`SessionController.cs:117-128`) | None | 🔴 Critical | 1h | Backend | Not Started |
| **FIX-011** | Fix 4 shared components to standalone | Frontend | Pagination, EmptyState, Spinner, Modal — add `standalone: true` | None | 🟡 High | 2h | Frontend | Not Started |

### High Priority — Sprint 2

| Task ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---------|-----------|--------|-------------|-------------|----------|--------|-------|--------|
| **SEC-001** | Fix CORS for staging/production | Infra | Replace hardcoded `localhost:4200` with configurable origins (`Program.cs`) | None | 🟡 High | 2h | Backend | Not Started |
| **SEC-002** | Protect Hangfire dashboard | Infra | Add `[Authorize(Roles="Admin")]` to Hangfire endpoint (`Program.cs`) | None | 🟡 High | 1h | Backend | Not Started |
| **SEC-003** | Move JWT key to env vars | Security | Remove hardcoded key from `appsettings.Development.json` | None | 🟡 High | 2h | Backend | Not Started |
| **TEST-001** | Write AuthService unit tests | Testing | JWT generation, refresh token rotation, password hashing | None | 🟡 High | 4h | Backend | Not Started |
| **TEST-002** | Write PatientService unit tests | Testing | CRUD + ownership checks | None | 🟡 High | 4h | Backend | Not Started |
| **TEST-003** | Write ChatController integration tests | Testing | REST endpoints with real DB | None | 🟡 High | 4h | Backend | Not Started |
| **TEST-004** | Write NotificationController tests | Testing | Mark read, mark all read | None | 🟡 High | 3h | Backend | Not Started |
| **FIX-012** | Add therapist ownership check to ExerciseController | Exercises | Verify therapist owns exercise before modify | None | 🟡 High | 2h | Backend | Not Started |
| **FIX-013** | Add missing DB indexes | Database | `Sessions.PatientId`, `Exercises.PatientId`, `ChatMessages.ConversationId`, `Notifications.UserId` | None | 🟡 High | 1h | Backend | Not Started |

### Medium Priority — Sprint 3

| Task ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---------|-----------|--------|-------------|-------------|----------|--------|-------|--------|
| **TEST-005** | Write integration tests | Testing | Controller tests with real DB for all 11 controllers | None | 🟡 Medium | 8h | Backend | Not Started |
| **QUAL-001** | Add FluentValidation for all DTOs | Validation | Patient, Session, Report, Intake, Assessment DTOs | None | 🟡 Medium | 6h | Backend | Not Started |
| **QUAL-002** | Fix namespace typo `Repositores` | Code Quality | Rename to `Repositories` across all 18+ files | None | 🟡 Medium | 2h | Backend | Not Started |
| **QUAL-003** | Extract `GetCurrentUserId()` to base controller | Code Quality | Reduce duplication across 7 controllers | None | 🟡 Medium | 1h | Backend | Not Started |
| **QUAL-004** | Move inline DTOs to proper folders | Code Quality | ExerciseController, IntakeController, AiController | None | 🟡 Medium | 2h | Backend | Not Started |
| **QUAL-005** | Add account lockout | Security | 5 failed attempts → 15 min lockout (`AuthService.cs`) | None | 🟡 Medium | 3h | Backend | Not Started |
| **QUAL-006** | Add rate limiting | Security | 100 req/min per user, 10 AI/min | None | 🟡 Medium | 3h | Backend | Not Started |

### Low Priority — Sprint 4

| Task ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---------|-----------|--------|-------------|-------------|----------|--------|-------|--------|
| **DEP-001** | Create CI/CD pipeline | DevOps | GitHub Actions for build + test | None | 🟢 Low | 4h | DevOps | Not Started |
| **DEP-002** | Staging environment config | DevOps | Environment-specific appsettings | None | 🟢 Low | 3h | Backend | Not Started |
| **DOC-001** | Create backend README | Documentation | Setup, architecture, API docs | None | 🟢 Low | 2h | Backend | Not Started |
| **DOC-002** | Add Langfuse LLM observability | Observability | Log all AI calls | None | 🟢 Low | 4h | Backend | Not Started |
| **DOC-003** | Add Sentry error monitoring | Observability | Frontend + backend | None | 🟢 Low | 3h | Full Stack | Not Started |
| **POLISH-001** | End-to-end smoke testing | Quality | Full workflow validation | All fixes done | 🟢 Low | 8h | Full Stack | Not Started |
| **POLISH-002** | Responsive UI review | Frontend | Tablet + desktop check | All fixes done | 🟢 Low | 4h | Frontend | Not Started |
| **POLISH-003** | Bug fixes from testing | Full Stack | All identified issues | All fixes done | 🟢 Low | 6h | Full Stack | Not Started |

---

## PHASE 4 — DEPENDENCY MAP

```
SPRINT 1 — Critical Fixes (No dependencies, can all start immediately)
├── FIX-001: SignalR token key (Frontend, 1h)
├── FIX-004: authGuard chat routes (Frontend, 1h)
├── FIX-005: authGuard profile route (Frontend, 1h)
├── FIX-011: Standalone components (Frontend, 2h)
├── FIX-002: Report reject endpoint (Backend, 2h)
├── FIX-003: CrisisDetection exceptions (Backend, 2h)
├── FIX-006: PatientController role restriction (Backend, 1h)
├── FIX-010: SessionController voice memo UoW (Backend, 1h)
├── FIX-007: ChatHub authorization (Backend, 3h)
├── FIX-008: ChatController architecture (Backend, 4h)
└── FIX-009: NotificationController architecture (Backend, 3h)

SPRINT 2 — Security & Testing (After Sprint 1)
├── SEC-001: CORS config (Backend, 2h)
├── SEC-002: Hangfire auth (Backend, 1h)
├── SEC-003: JWT env vars (Backend, 2h)
├── FIX-012: Exercise ownership check (Backend, 2h)
├── FIX-013: DB indexes (Backend, 1h)
├── TEST-001: AuthService tests (Backend, 4h)
├── TEST-002: PatientService tests (Backend, 4h)
├── TEST-003: ChatController tests (Backend, 4h)
└── TEST-004: NotificationController tests (Backend, 3h)

SPRINT 3 — Quality & Hardening (After Sprint 2)
├── TEST-005: Integration tests (Backend, 8h)
├── QUAL-001: FluentValidation (Backend, 6h)
├── QUAL-002: Namespace typo fix (Backend, 2h)
├── QUAL-003: Base controller extraction (Backend, 1h)
├── QUAL-004: Inline DTOs move (Backend, 2h)
├── QUAL-005: Account lockout (Backend, 3h)
└── QUAL-006: Rate limiting (Backend, 3h)

SPRINT 4 — Polish & Deploy (After Sprint 3)
├── DEP-001: CI/CD pipeline (DevOps, 4h)
├── DEP-002: Staging config (Backend, 3h)
├── DOC-001: Backend README (Backend, 2h)
├── DOC-002: Langfuse (Backend, 4h)
├── DOC-003: Sentry (Full Stack, 3h)
├── POLISH-001: Smoke testing (Full Stack, 8h)
├── POLISH-002: Responsive review (Frontend, 4h)
└── POLISH-003: Bug fixes (Full Stack, 6h)
```

### Critical Path Items

```
FIX-008 (ChatController architecture) → TEST-003 (ChatController tests)
FIX-009 (NotificationController architecture) → TEST-004 (NotificationController tests)
FIX-006 (PatientController role) → TEST-002 (PatientService tests)
FIX-011 (Standalone components) → POLISH-002 (Responsive review)
```

---

## PHASE 5 — MVP SPRINT PLAN

### SPRINT 1 — Critical Fixes (Week 1)

**Goals:**
- Fix all 8 critical blockers identified in audit
- Resolve security vulnerabilities (ChatHub auth, PatientController roles)
- Fix architecture violations (Chat, Notification, Session controllers)
- Fix frontend bugs (SignalR token, missing guards, non-standalone components)

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| FIX-001 | Fix SignalR token key (`access_token` → `jalsa_token`) | Frontend | 1h |
| FIX-004 | Add `authGuard` to chat routes | Frontend | 1h |
| FIX-005 | Add `authGuard` to profile route | Frontend | 1h |
| FIX-011 | Fix 4 shared components to `standalone: true` | Frontend | 2h |
| FIX-002 | Fix Report reject endpoint to persist | Backend | 2h |
| FIX-003 | Fix CrisisDetection exception handling | Backend | 2h |
| FIX-006 | Add `[Authorize(Roles="Therapist")]` to PatientController | Backend | 1h |
| FIX-010 | Fix SessionController voice memo — use UoW | Backend | 1h |
| FIX-007 | Add ChatHub authorization (validate user identity) | Backend | 3h |
| FIX-008 | Extract ChatController service layer | Backend | 4h |
| FIX-009 | Extract NotificationController service layer | Backend | 3h |

**Expected Deliverables:**
- All critical security vulnerabilities patched
- All architecture violations resolved
- All frontend bugs fixed
- Chat system fully functional with proper auth

**Dependencies:** None (can start immediately)

**Risks:**
- ChatController architecture fix may require new service interfaces and implementations
- ChatHub auth fix may affect existing SignalR connection flow

**Definition of Done:**
- [ ] `FIX-001`: SignalR connects successfully in chat room
- [ ] `FIX-002`: `POST /api/reports/{id}/reject` persists rejection status
- [ ] `FIX-003`: CrisisDetection returns `IsCrisis=false` on AI failure
- [ ] `FIX-004`: `/chatbot` redirects to login when unauthenticated
- [ ] `FIX-005`: `/auth/profile` redirects to login when unauthenticated
- [ ] `FIX-006`: Patient role cannot access `/api/patient` endpoints
- [ ] `FIX-007`: ChatHub validates `Context.User.Identity` on connect
- [ ] `FIX-008`: ChatController uses `IChatService` instead of DbContext
- [ ] `FIX-009`: NotificationController uses `INotificationService` instead of DbContext
- [ ] `FIX-010`: SessionController voice memo uses UoW pattern
- [ ] `FIX-011`: All shared components have `standalone: true`
- [ ] All existing 51 backend tests still pass
- [ ] All existing 24 frontend specs still pass

---

### SPRINT 2 — Security & Testing (Week 2)

**Goals:**
- Harden security (CORS, Hangfire, JWT, ownership checks)
- Add database performance indexes
- Write unit tests for untested critical services

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| SEC-001 | Fix CORS for staging/production | Backend | 2h |
| SEC-002 | Protect Hangfire dashboard | Backend | 1h |
| SEC-003 | Move JWT key to environment variables | Backend | 2h |
| FIX-012 | Add therapist ownership check to ExerciseController | Backend | 2h |
| FIX-013 | Add missing DB indexes (Sessions, Exercises, ChatMessages, Notifications) | Backend | 1h |
| TEST-001 | Write AuthService unit tests (JWT, refresh, password) | Backend | 4h |
| TEST-002 | Write PatientService unit tests (CRUD + ownership) | Backend | 4h |
| TEST-003 | Write ChatController integration tests | Backend | 4h |
| TEST-004 | Write NotificationController tests | Backend | 3h |

**Expected Deliverables:**
- All security configurations production-ready
- Database optimized with proper indexes
- Critical auth and patient services have test coverage
- Chat and notification APIs tested

**Dependencies:** Sprint 1 complete

**Risks:**
- CORS changes may require frontend environment config updates
- Integration tests need InMemory DB or TestContainers setup

**Definition of Done:**
- [ ] `SEC-001`: CORS allows staging/production origins (configurable)
- [ ] `SEC-002`: `/hangfire` returns 403 for non-Admin users
- [ ] `SEC-003`: JWT key read from `Environment.GetEnvironmentVariable()`
- [ ] `FIX-012`: Therapist cannot modify exercises belonging to another therapist
- [ ] `FIX-013`: All 4 new indexes exist in migration
- [ ] `TEST-001`: AuthService tests cover login, register, refresh, password reset
- [ ] `TEST-002`: PatientService tests cover CRUD + ownership isolation
- [ ] `TEST-003`: ChatController tests cover conversations, history, send, close
- [ ] `TEST-004`: NotificationController tests cover list, read, read-all
- [ ] All new tests pass
- [ ] Total backend tests: 51 + ~25 new = ~76 tests

---

### SPRINT 3 — Quality & Hardening (Week 3)

**Goals:**
- Write integration tests for all controllers
- Add FluentValidation for all DTOs
- Fix code quality issues (namespace typo, duplication, inline DTOs)
- Add account lockout and rate limiting

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| TEST-005 | Write integration tests for all 11 controllers | Backend | 8h |
| QUAL-001 | Add FluentValidation for Patient, Session, Report, Intake, Assessment DTOs | Backend | 6h |
| QUAL-002 | Fix namespace typo `Repositores` → `Repositories` | Backend | 2h |
| QUAL-003 | Extract `GetCurrentUserId()` to base controller | Backend | 1h |
| QUAL-004 | Move inline DTOs to proper folders | Backend | 2h |
| QUAL-005 | Add account lockout (5 attempts → 15 min) | Backend | 3h |
| QUAL-006 | Add rate limiting (100 req/min, 10 AI/min) | Backend | 3h |

**Expected Deliverables:**
- All controllers have integration tests
- All DTOs validated with FluentValidation
- Code quality issues resolved
- Security hardening complete

**Dependencies:** Sprint 2 complete

**Risks:**
- Namespace typo fix may touch 18+ files — merge conflict risk
- Integration tests may reveal hidden bugs

**Definition of Done:**
- [ ] `TEST-005`: Integration tests for Auth, Patient, Session, Exercise, Report, Assessment, Intake, Ai, Progress, Chat, Notification controllers
- [ ] `QUAL-001`: FluentValidation rules for all DTOs with Arabic error messages
- [ ] `QUAL-002`: All files use `Repositories` namespace (grep confirms no `Repositores`)
- [ ] `QUAL-003`: Base controller has `GetCurrentUserId()` method
- [ ] `QUAL-004`: No DTOs defined inside controller files
- [ ] `QUAL-005`: 6th failed login returns 423 Locked
- [ ] `QUAL-006`: 101st request in 1 minute returns 429 Too Many Requests
- [ ] All tests pass (target: ~100+ total)

---

### SPRINT 4 — Polish & Deploy (Week 4)

**Goals:**
- Set up CI/CD pipeline
- Configure staging environment
- End-to-end smoke testing
- UI polish and bug fixes
- Create deployment documentation

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| DEP-001 | Create GitHub Actions CI/CD pipeline | DevOps | 4h |
| DEP-002 | Create staging environment config | Backend | 3h |
| DOC-001 | Create backend README | Backend | 2h |
| DOC-002 | Add Langfuse LLM observability | Backend | 4h |
| DOC-003 | Add Sentry error monitoring | Full Stack | 3h |
| POLISH-001 | End-to-end smoke testing | Full Stack | 8h |
| POLISH-002 | Responsive UI review (tablet + desktop) | Frontend | 4h |
| POLISH-003 | Fix all identified bugs | Full Stack | 6h |

**Expected Deliverables:**
- CI/CD pipeline builds and tests on push
- Staging environment fully configured
- All smoke tests passing
- All UI responsive and polished
- Monitoring and observability active

**Dependencies:** Sprints 1-3 complete

**Risks:**
- Integration bugs may surface during full E2E testing
- OpenAI API rate limits during extended testing sessions

**Definition of Done:**
- [ ] `DEP-001`: GitHub Actions runs build + test on PR to main/develop
- [ ] `DEP-002`: Staging config with proper CORS, JWT, connection strings
- [ ] `DOC-001`: Backend README with setup instructions
- [ ] `DOC-002`: Langfuse logs all GPT-4o calls
- [ ] `DOC-003`: Sentry captures frontend + backend errors
- [ ] `POLISH-001`: Full workflow passes: Login → Patient → Session → Exercise → Dashboard → Report → Chat
- [ ] `POLISH-002`: All pages responsive on iPad (768px) and desktop (1920px)
- [ ] `POLISH-003`: Zero critical bugs, zero console errors
- [ ] All ~100+ tests pass

---

## PHASE 6 — MVP ROADMAP

### Week 1: Critical Fixes

| Day | Frontend | Backend |
|-----|----------|---------|
| **Mon** | FIX-001: SignalR token key (1h) | FIX-006: PatientController role restriction (1h) |
| | FIX-004: authGuard chat routes (1h) | FIX-010: SessionController voice memo UoW (1h) |
| | FIX-005: authGuard profile route (1h) | FIX-003: CrisisDetection exception handling (2h) |
| **Tue** | FIX-011: Standalone components (2h) | FIX-002: Report reject endpoint (2h) |
| **Wed** | Verify all frontend fixes | FIX-007: ChatHub authorization (3h) |
| **Thu** | — | FIX-008: ChatController architecture (4h) |
| **Fri** | Integration testing of all fixes | FIX-009: NotificationController architecture (3h) |

### Week 2: Security & Testing

| Day | Frontend | Backend |
|-----|----------|---------|
| **Mon** | Support CORS config changes | SEC-001: CORS config (2h) |
| | | SEC-002: Hangfire auth (1h) |
| | | SEC-003: JWT env vars (2h) |
| **Tue** | — | FIX-012: Exercise ownership check (2h) |
| | | FIX-013: DB indexes (1h) |
| **Wed** | — | TEST-001: AuthService tests (4h) |
| **Thu** | — | TEST-002: PatientService tests (4h) |
| **Fri** | Verify all security fixes | TEST-003: ChatController tests (4h) |
| | | TEST-004: NotificationController tests (3h) |

### Week 3: Quality & Hardening

| Day | Frontend | Backend |
|-----|----------|---------|
| **Mon** | Support validation changes | QUAL-002: Namespace typo fix (2h) |
| | | QUAL-003: Base controller extraction (1h) |
| | | QUAL-004: Inline DTOs move (2h) |
| **Tue** | — | QUAL-001: FluentValidation (6h) |
| **Wed** | — | QUAL-005: Account lockout (3h) |
| | | QUAL-006: Rate limiting (3h) |
| **Thu** | — | TEST-005: Integration tests (8h — may spill to Fri) |
| **Fri** | Verify all quality fixes | TEST-005: Integration tests (continued) |

### Week 4: Polish & Deploy

| Day | Frontend | Backend |
|-----|----------|---------|
| **Mon** | POLISH-002: Responsive review (4h) | DEP-001: CI/CD pipeline (4h) |
| **Tue** | Bug fixes from responsive review | DEP-002: Staging config (3h) |
| **Wed** | POLISH-001: Smoke testing (Full Team) | DOC-001: Backend README (2h) |
| **Thu** | POLISH-003: Bug fixes (Full Team) | DOC-002: Langfuse (4h) |
| **Fri** | Final review | DOC-003: Sentry (3h) |
| | MVP release sign-off | MVP release sign-off |

---

## PHASE 7 — MVP RISK REPORT

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| ChatController architecture fix breaks existing chat functionality | High | Medium | Write tests before refactoring; test SignalR connection end-to-end after each change |
| ChatHub auth fix breaks existing SignalR connections | High | Medium | Test with both authenticated and unauthenticated users; verify token validation flow |
| Namespace typo fix causes merge conflicts | Medium | High | Do all namespace fixes in single commit; coordinate with all team members |
| Integration tests reveal hidden bugs in controllers | Medium | Medium | Allocate buffer time in Sprint 3; prioritize critical path tests first |
| OpenAI API rate limits during testing | Medium | Low | Use mock responses for unit tests; test with real API only for integration tests |
| CORS changes break local development | Medium | Medium | Keep `localhost:4200` in development config; only restrict in staging/production |
| Account lockout causes support burden | Low | Medium | Document lockout policy; add admin unlock endpoint (post-MVP) |
| Rate limiting blocks legitimate heavy usage | Low | Low | Set generous limits (100 req/min); monitor before tightening |

---

## PHASE 8 — TEAM EXECUTION PLAN

### Backend Team (2 members)

**Member 1 (Backend Lead — Architecture & Security):**
- Sprint 1: FIX-007 (ChatHub auth), FIX-008 (ChatController architecture), FIX-009 (NotificationController architecture)
- Sprint 2: SEC-001 (CORS), SEC-002 (Hangfire), SEC-003 (JWT env vars), FIX-012 (Exercise ownership)
- Sprint 3: QUAL-001 (FluentValidation), QUAL-005 (Account lockout), QUAL-006 (Rate limiting)
- Sprint 4: DEP-001 (CI/CD), DEP-002 (Staging config), DOC-001 (README)

**Member 2 (Backend — Testing & Fixes):**
- Sprint 1: FIX-002 (Report reject), FIX-003 (CrisisDetection), FIX-006 (PatientController), FIX-010 (Session voice memo)
- Sprint 2: FIX-013 (DB indexes), TEST-001 (AuthService tests), TEST-002 (PatientService tests), TEST-003 (ChatController tests), TEST-004 (NotificationController tests)
- Sprint 3: TEST-005 (Integration tests), QUAL-002 (Namespace typo), QUAL-003 (Base controller), QUAL-004 (Inline DTOs)
- Sprint 4: DOC-002 (Langfuse), POLISH-003 (Bug fixes)

### Frontend Team (1-2 members)

**Member 3 (Frontend Lead):**
- Sprint 1: FIX-001 (SignalR token), FIX-004 (authGuard chat), FIX-005 (authGuard profile), FIX-011 (Standalone components)
- Sprint 2: Support CORS config, verify security fixes
- Sprint 3: Support validation changes, verify FluentValidation
- Sprint 4: POLISH-002 (Responsive review), POLISH-001 (Smoke testing), POLISH-003 (Bug fixes)

### DevOps / Full Stack

- Sprint 4: DEP-001 (CI/CD), DOC-003 (Sentry)

### Daily Coordination

- **Sprint 1-2:** Backend team works independently (no frontend dependencies for fixes)
- **Sprint 3:** Backend validates DTOs with frontend team before deploying FluentValidation
- **Sprint 4:** Full team coordinates on smoke testing and bug fixes

---

## PHASE 9 — MVP RELEASE CHECKLIST

### Authentication ✅
- [ ] Login works for Therapist role
- [ ] Login works for Patient role
- [ ] Registration creates Therapist account with license number
- [ ] JWT token injected in all API calls
- [ ] Refresh token rotation works
- [ ] Password reset via email OTP works
- [ ] Profile page shows user data (GET/PUT)
- [ ] Role-based route guards prevent unauthorized access
- [ ] `/auth/profile` requires authentication
- [ ] Account lockout after 5 failed attempts

### Patient Management ✅
- [ ] Patient list loads with search/filter
- [ ] Create patient form works
- [ ] Edit patient form works
- [ ] Patient detail shows all info
- [ ] Archive/restore patient works
- [ ] Intake form saves to backend
- [ ] OCR upload works
- [ ] Assessments display on patient detail
- [ ] Only Therapists can manage patients (role restriction)

### Session Management ✅
- [ ] Session list loads for patient
- [ ] Create session form works
- [ ] Edit session form works
- [ ] Session detail shows note content
- [ ] Session note fields save correctly
- [ ] Session history shows chronological list
- [ ] Voice memo upload with Whisper STT works
- [ ] AI session summary works
- [ ] Sessions require Therapist role

### Exercise Management ✅
- [ ] Exercise list loads for therapist
- [ ] Assign exercise form works
- [ ] Patient can view assigned exercises
- [ ] Patient can log completion/partial/skipped
- [ ] Exercise due date can be extended
- [ ] Therapist ownership check works
- [ ] Error messages are user-friendly

### Dashboard ✅
- [ ] Patient count displays
- [ ] Session count displays
- [ ] Exercise completion rate displays
- [ ] Assessment trend chart renders
- [ ] Session frequency chart renders (Arabic labels)
- [ ] Auto-refresh every 5 minutes works

### AI Reports ✅
- [ ] Generate button triggers AI report
- [ ] Report draft displays correctly
- [ ] Report list shows all reports
- [ ] Report detail view works
- [ ] Approve workflow works
- [ ] Reject workflow persists (not stub)
- [ ] HTML export works
- [ ] Arabic disclaimer present on reports

### Chat System ✅
- [ ] Chat list loads conversations
- [ ] Chat room opens with history
- [ ] Real-time messaging via SignalR works
- [ ] SignalR token authentication works
- [ ] ChatHub validates user identity
- [ ] Crisis detection works (with proper exception handling)
- [ ] Chat requires authentication (authGuard)

### Notifications ✅
- [ ] Notification bell shows in header
- [ ] Dropdown shows recent notifications
- [ ] Mark read works
- [ ] Mark all read works
- [ ] Unread count badge accurate
- [ ] Polling every 30 seconds works

### Security ✅
- [ ] CORS configured for staging/production
- [ ] Hangfire dashboard protected
- [ ] JWT key in environment variables
- [ ] Rate limiting active
- [ ] No sensitive data in console logs
- [ ] All API endpoints have proper authorization

### Technical Quality
- [ ] No 500 errors on any page
- [ ] No console errors in browser
- [ ] Loading spinners on async operations
- [ ] Error messages are Arabic
- [ ] Responsive on tablet (768px) and desktop (1920px)
- [ ] All API calls have error handling
- [ ] CI/CD pipeline runs on PR
- [ ] ~100+ tests all passing
- [ ] Integration tests cover all controllers

---

## PHASE 10 — FINAL EXECUTIVE REPORT

# JALSA MVP EXECUTION PLAN

### Current Status
**82%** (per audit report dated June 29, 2026)

### Estimated MVP Completion
**96%** after 4 sprints of focused work

### Estimated Remaining Effort
**4 weeks** (1 sprint/week, 4-5 team members)

### Critical Blockers

1. 🔴 **SignalR token key mismatch** — Chat broken in production (1h fix)
2. 🔴 **ChatHub has no authorization** — Security vulnerability (3h fix)
3. 🔴 **ChatController/NotificationController bypass Clean Architecture** — Architecture violations (7h total)
4. 🔴 **ReportController.Reject is a no-op** — Feature incomplete (2h fix)
5. 🔴 **CrisisDetectionService swallows exceptions** — Safety issue (2h fix)
6. 🔴 **PatientController has no role restriction** — Security vulnerability (1h fix)
7. 🟡 **No CI/CD pipeline** — No automated quality gates
8. 🟡 **No integration tests** — API contract bugs undetected

### MVP Scope

| Included ✅ | Excluded (Post-MVP) 🚀 |
|---|---|
| Login/Register/Profile | PDF export for reports |
| Patient CRUD + Intake + Assessments + OCR | Semantic search UI |
| Session CRUD + Notes + Voice STT + AI Summary | Session note auto-save |
| Exercise CRUD + Logging + Reminders | Session embedding generation |
| Dashboard with charts (Arabic labels) | Patient summary chips |
| AI Report generation + approve + reject + HTML export | 90-day absence warning |
| Chat system with SignalR + crisis detection | Admin panel |
| In-app notifications | Audit log recording |
| Role-based access control | Langfuse/Sentry observability |
| Account lockout + Rate limiting | pgvector semantic search |

### Sprint Plan

| Sprint | Focus | Key Deliverables | Est. Completion |
|---|---|---|---|
| **Sprint 1** (Week 1) | Critical Fixes | Security patches, architecture violations, frontend bugs | 86% |
| **Sprint 2** (Week 2) | Security & Testing | CORS, Hangfire, JWT, ownership checks, unit tests | 90% |
| **Sprint 3** (Week 3) | Quality & Hardening | FluentValidation, integration tests, code quality, lockout, rate limiting | 94% |
| **Sprint 4** (Week 4) | Polish & Deploy | CI/CD, staging config, monitoring, smoke testing, bug fixes | 96% |

### Remaining Tasks

| Category | Tasks | Total Effort |
|----------|-------|-------------|
| Critical Fixes (Sprint 1) | 11 tasks | ~23h |
| Security & Testing (Sprint 2) | 9 tasks | ~23h |
| Quality & Hardening (Sprint 3) | 7 tasks | ~25h |
| Polish & Deploy (Sprint 4) | 8 tasks | ~30h |
| **Total** | **35 tasks** | **~101h** |

### Major Risks

1. ChatController refactoring may break existing functionality
2. Namespace typo fix may cause merge conflicts
3. Integration tests may reveal hidden bugs
4. OpenAI API rate limits during testing
5. CORS changes may affect local development

### Post-MVP Features

1. PDF export for referral reports
2. Semantic search for session notes (pgvector)
3. Session note auto-save (60s drafts)
4. Session embedding generation
5. Patient summary chips (last session, total sessions, active exercises)
6. 90-day absence warning
7. Admin user management panel
8. Audit log recording
9. Langfuse LLM observability
10. Sentry error monitoring

### Recommended Immediate Next Actions

1. **Start Sprint 1 TODAY** — Begin with FIX-001 (SignalR token, 1h), FIX-004/005 (authGuards, 2h), and FIX-006 (PatientController, 1h) — these are independent 1h fixes that can be done in parallel by frontend and backend
2. **Assign team members** — Need at least 2 backend developers and 1-2 frontend developers for 4 weeks
3. **Set up branch protection** — Before any fixes, ensure no direct pushes to main/develop
4. **Review ChatController architecture** — Read `ChatController.cs` and plan the service extraction before coding (biggest Sprint 1 task)
5. **Create test infrastructure** — Set up InMemory DB or TestContainers for integration tests before Sprint 3

---

*Document generated from JALSA-AUDIT-REPORT.md (June 29, 2026). All findings evidence-based with specific file paths and line numbers. No assumptions made.*
