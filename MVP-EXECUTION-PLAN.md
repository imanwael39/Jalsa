# JALSA MVP EXECUTION PLAN

> **Date:** June 27, 2026
> **Author:** Technical Program Manager
> **Status:** Active
> **Sprint Cadence:** 1 week per sprint

---

## PHASE 1 — CURRENT PROJECT STATUS SUMMARY

### Completion Dashboard

| Layer | Completion | Status |
|---|---|---|
| **Frontend** | 42% | Partial — Auth + Patients + Exercises + Dashboard done; Sessions/Reports/Chat pages exist but have no working backend |
| **Backend** | 52% | Partial — Auth + Patients + Exercises + Dashboard + AI services done; **NO SessionController**, **NO ReportController**, **NO ChatController** |
| **Database** | 60% | All 24 tables migrated and functional on SQL Server |
| **AI Features** | 35% | Services exist (ReportGeneration, Summarization, ChatAI, CrisisDetection, OCR, Embedding) but no controllers/endpoints to invoke them properly |
| **Testing** | 15% | ~21 unit tests across 5 test files; no integration tests |

### Critical Blockers 🔥

1. **No SessionController** — Frontend session pages will 404 on every API call. Sessions are the CORE clinical feature.
2. **No ReportController** — AI report generation has no API endpoint. Frontend reports routes are empty `[]`.
3. **No ChatController** — SignalR ChatHub exists but no REST endpoints for history/creation. Frontend chatbot routes are empty `[]`.
4. **AI endpoints unsecured** — `AiController` and `IntakeController` have no `[Authorize]` attribute.
5. **PatientService misplaced** — Lives in `Jalsa.API/Services` instead of Application layer.

### Existing Blockers

- Session frontend calls `/sessions/*` (no `/api/` prefix) — mismatch with backend route patterns
- Reports and Chat frontend routes are empty arrays — no pages exist
- `EmailNotificationService` is a stub (`Console.WriteLine`)

### Technical Debt ⚠

- `Galsa_DBDbContext` misspelled (should be `Jalsa`)
- JWT key hardcoded in `appsettings.json` (not in secrets/env)
- `ExerciseController` catches all exceptions and exposes `ex.Message` — information leak
- `IntakeController` directly injects `Galsa_DBDbContext` — violates Clean Architecture
- VectorStore does cosine similarity in C# memory, not via pgvector

---

## PHASE 2 — DEFINE MVP

### MVP Principle

> **A therapist can log in, manage patients, create/view sessions with notes, assign exercises, see a dashboard, and generate a basic AI report.**

### Module Classification

| Module | Current Status | MVP Classification | Reason |
|---|---|---|---|
| **Authentication** | ✅ 75% done | **Required ✅** | Foundation for all access |
| **Patient Management** | ✅ 65% done | **Required ✅** | Core data entity |
| **Session Management** | ❌ 15% (no backend) | **Required ✅** | Core clinical workflow |
| **Exercise Management** | ✅ 70% done | **Required ✅** | Core feature |
| **Dashboard** | ✅ 60% done | **Required ✅** | Therapist overview |
| **AI Reports (Basic)** | ⚠ 30% (no controller) | **Required ✅** | Key differentiator |
| **Notifications (In-App)** | ❌ 10% (stub) | **Post-MVP 🚀** | Not blocking core workflow |
| **Chat Support** | ⚠ 25% (no frontend) | **Post-MVP 🚀** | Complex; can launch without |
| **Voice Memo STT** | ⚠ Domain model only | **Post-MVP 🚀** | Nice-to-have for MVP |
| **PDF Export** | ❌ 0% | **Post-MVP 🚀** | Not needed for initial demo |
| **Admin Panel** | ❌ 0% | **Post-MVP 🚀** | Single-admin setup sufficient |
| **Semantic Search** | ⚠ VectorStore exists | **Post-MVP 🚀** | Can add after core works |
| **Account Lockout** | ❌ 0% | **Post-MVP 🚀** | Security hardening later |

### MVP SUCCESS CRITERIA — Users Can:

1. Register as Therapist, Login with JWT
2. Create, view, edit, archive, restore patients
3. Fill intake form for a patient
4. Create sessions with notes (observations, interventions, patient response, homework, next goals)
5. View session history per patient
6. Assign exercises to patients, set due dates
7. View exercise list, extend due dates
8. View dashboard with patient counts, session counts, exercise completion rate, assessment trends
9. Generate an AI draft referral report for a patient
10. View assessment results for a patient

---

## PHASE 3 — MVP TASK BREAKDOWN STRUCTURE

### Backend Tasks

| ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---|---|---|---|---|---|---|---|---|
| **BE-SES-01** | Session DTOs | Sessions | Create `SessionCreateDto`, `SessionUpdateDto`, `SessionViewDto`, `SessionNoteDto` | None | Critical | Small | Backend | Not Started |
| **BE-SES-02** | Session Service | Sessions | Implement `ISessionService` with CRUD + GetByPatientId | BE-SES-01 | Critical | Medium | Backend | Not Started |
| **BE-SES-03** | Session Controller | Sessions | Create `SessionController` at `/api/sessions` with full CRUD endpoints, `[Authorize(Roles="Therapist")]` | BE-SES-02 | Critical | Medium | Backend | Not Started |
| **BE-SES-04** | Session Note endpoints | Sessions | Add session note CRUD within session (GET/POST note for session) | BE-SES-03 | Critical | Medium | Backend | Not Started |
| **BE-RPT-01** | Report Controller | Reports | Create `ReportController` at `/api/reports` — POST generate draft, GET list, GET by ID | Existing AI services | Critical | Medium | Backend | Not Started |
| **BE-RPT-02** | Report DTOs | Reports | Create `ReportGenerateDto`, `ReportViewDto`, `ReportVersionDto` | None | Critical | Small | Backend | Not Started |
| **BE-RPT-03** | Report Service | Reports | Implement `IReportService` wrapping `ReportGenerationService` + DB persistence | BE-RPT-02, BE-RPT-01 | Critical | Medium | Backend | Not Started |
| **BE-AUTH-01** | Profile endpoint | Auth | Add `GET /api/auth/profile` and `PUT /api/auth/profile` endpoints | None | High | Small | Backend | Not Started |
| **BE-AUTH-02** | Secure AI endpoints | Auth | Add `[Authorize]` to `AiController` and `IntakeController` | None | Critical | Small | Backend | Not Started |
| **BE-PAT-01** | Intake form CRUD endpoint | Patients | Add `GET /api/patient/{id}/intake` and `POST /api/patient/{id}/intake` | None | High | Medium | Backend | Not Started |
| **BE-PAT-02** | Assessment CRUD endpoint | Patients | Create basic assessment GET/POST endpoints for patient assessments | None | High | Medium | Backend | Not Started |
| **BE-EXE-01** | Fix ExerciseController errors | Exercises | Replace catch-all exception handling with proper error responses; add `[Authorize]` role validation | None | High | Small | Backend | Not Started |

### Frontend Tasks

| ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---|---|---|---|---|---|---|---|---|
| **FE-SES-01** | Fix session API endpoints | Sessions | Update `api-endpoints.ts` sessions to use `/api/sessions` prefix matching backend | BE-SES-03 | Critical | Small | Frontend | Not Started |
| **FE-SES-02** | Connect session pages to API | Sessions | Wire session-list, session-form, session-detail to `SessionService` | FE-SES-01 | Critical | Medium | Frontend | Not Started |
| **FE-SES-03** | Fix session form submission | Sessions | Ensure session form creates/updates via backend API | FE-SES-02 | Critical | Medium | Frontend | Not Started |
| **FE-RPT-01** | Create reports routes | Reports | Define `REPORTS_ROUTES` with list, generate, detail pages | BE-RPT-01 | Critical | Small | Frontend | Not Started |
| **FE-RPT-02** | Create report list page | Reports | Page showing all referral reports for therapist | BE-RPT-01 | Critical | Medium | Frontend | Not Started |
| **FE-RPT-03** | Create report generate page | Reports | Button to trigger AI report generation + display draft | BE-RPT-01 | Critical | Medium | Frontend | Not Started |
| **FE-RPT-04** | Create report service | Reports | `ReportService` calling backend report endpoints | FE-RPT-01 | Critical | Small | Frontend | Not Started |
| **FE-PAT-01** | Fix intake form API connection | Patients | Wire intake form page to backend intake endpoints | BE-PAT-01 | High | Medium | Frontend | Not Started |
| **FE-AUTH-01** | Connect profile page to API | Auth | Wire profile component to `GET /api/auth/profile` | BE-AUTH-01 | High | Small | Frontend | Not Started |
| **FE-DASH-01** | Fix dashboard chart labels | Dashboard | Add Arabic labels to assessment trend and session frequency charts | None | Medium | Small | Frontend | Not Started |

### Infrastructure Tasks

| ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---|---|---|---|---|---|---|---|---|
| **INF-001** | Fix CORS for SignalR | Infra | Add SignalR to CORS policy (currently only HTTP) | None | High | Small | Backend | Not Started |
| **INF-002** | Move JWT secret to env vars | Security | Remove hardcoded JWT key from `appsettings.json`, use environment variable | None | High | Small | Backend | Not Started |

### Testing Tasks

| ID | Task Name | Module | Description | Dependencies | Priority | Effort | Owner | Status |
|---|---|---|---|---|---|---|---|---|
| **TEST-001** | Session service tests | Sessions | Unit tests for SessionService CRUD | BE-SES-02 | High | Medium | Backend | Not Started |
| **TEST-002** | Session controller tests | Sessions | Integration tests for SessionController endpoints | BE-SES-03 | High | Medium | Backend | Not Started |
| **TEST-003** | Report service tests | Reports | Unit tests for ReportService | BE-RPT-03 | High | Medium | Backend | Not Started |

---

## PHASE 4 — DEPENDENCY MAP

```
AUTH (Complete) ─────────────────────────────────────────────────┐
  ├─ JWT integration ✓                                           │
  ├─ Route Guards ✓                                              │
  └─ Protected pages ✓                                           │
                                                                 │
PATIENT MANAGEMENT (65%) ────────────────────────────────────────┤
  ├─ Patient CRUD ✓                                              │
  ├─ Intake Form CRUD ──→ BE-PAT-01 ──→ FE-PAT-01               │
  └─ Assessment CRUD ──→ BE-PAT-02                              │
                                                                 │
SESSION MANAGEMENT (15%) ◄── CRITICAL BLOCKER                    │
  ├─ Session DTOs ──→ BE-SES-01 ──→ BE-SES-02 ──→ BE-SES-03   │
  │                                              ↓               │
  │                                    BE-SES-04 (Note endpoints)│
  │                                              ↓               │
  │                              FE-SES-01 ──→ FE-SES-02 ──→ FE-SES-03
  │                                                                │
EXERCISE MANAGEMENT (70%) ────────────────────────────────────────┤
  └─ Fix error handling ──→ BE-EXE-01                            │
                                                                 │
DASHBOARD (60%) ──────────────────────────────────────────────────┤
  └─ Fix chart labels ──→ FE-DASH-01                             │
                                                                 │
AI REPORTS (30%) ◄── CRITICAL BLOCKER                            │
  ├─ Report DTOs ──→ BE-RPT-02 ──→ BE-RPT-03                    │
  ├─ Report Controller ──→ BE-RPT-01 ──→ FE-RPT-01/02/03/04     │
  └─ Secure endpoints ──→ BE-AUTH-02                             │
                                                                 │
AUTH EXTRAS                                                      │
  ├─ Profile endpoint ──→ BE-AUTH-01 ──→ FE-AUTH-01              │
  └─ Secure AI endpoints ──→ BE-AUTH-02                          │
                                                                 │
INFRASTRUCTURE                                                   │
  ├─ CORS SignalR ──→ INF-001                                   │
  └─ JWT env vars ──→ INF-002                                   │
```

### Critical Path Items

```
BE-SES-01 → BE-SES-02 → BE-SES-03 → FE-SES-01 → FE-SES-02 → FE-SES-03
BE-RPT-02 → BE-RPT-03 → BE-RPT-01 → FE-RPT-01 → FE-RPT-02/03
```

---

## PHASE 5 — MVP SPRINT PLAN

### SPRINT 1 — Foundation Fixes & Session Backend (Week 1)

**Goals:**
- Fix critical security issues
- Build complete Session CRUD backend
- Fix frontend API endpoint mismatches

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| BE-AUTH-02 | Add `[Authorize]` to AiController and IntakeController | Backend | 1h |
| INF-002 | Move JWT secret to environment variables | Backend | 2h |
| BE-EXE-01 | Fix ExerciseController error handling | Backend | 3h |
| BE-SES-01 | Create Session DTOs | Backend | 3h |
| BE-SES-02 | Implement SessionService | Backend | 6h |
| BE-SES-03 | Create SessionController with CRUD | Backend | 6h |
| BE-SES-04 | Add session note endpoints | Backend | 5h |
| FE-SES-01 | Fix session API endpoint prefix in `api-endpoints.ts` | Frontend | 1h |

**Expected Deliverables:**
- Session CRUD API fully functional at `/api/sessions`
- Session notes API functional at `/api/sessions/{id}/note`
- AI endpoints secured with `[Authorize]`
- Exercise error handling improved

**Dependencies:** None (can start immediately)

**Risks:**
- Session entity model may need adjustment based on existing DB schema
- Frontend session model may not match new DTOs

**Definition of Done:**
- [ ] `POST /api/sessions` creates a session (Therapist role required)
- [ ] `GET /api/sessions` returns session list
- [ ] `GET /api/sessions/{id}` returns session detail with note
- [ ] `PUT /api/sessions/{id}` updates session
- [ ] `DELETE /api/sessions/{id}` soft-deletes session
- [ ] `POST /api/sessions/{id}/note` creates/updates session note
- [ ] `GET /api/sessions/patient/{patientId}` returns patient's sessions
- [ ] All session endpoints require Therapist role
- [ ] AiController requires authentication

---

### SPRINT 2 — Session Frontend & Reports Backend (Week 2)

**Goals:**
- Connect frontend session pages to working backend
- Build Report generation backend
- Connect profile page

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| FE-SES-01 | Fix session API endpoints (already done Sprint 1) | Frontend | — |
| FE-SES-02 | Connect session pages to SessionService | Frontend | 6h |
| FE-SES-03 | Fix session form create/edit flow | Frontend | 5h |
| BE-RPT-02 | Create Report DTOs | Backend | 3h |
| BE-RPT-03 | Implement ReportService | Backend | 5h |
| BE-RPT-01 | Create ReportController | Backend | 4h |
| BE-AUTH-01 | Add profile GET/PUT endpoints | Backend | 3h |
| FE-AUTH-01 | Connect profile page to API | Frontend | 2h |
| INF-001 | Fix CORS for SignalR | Backend | 1h |

**Expected Deliverables:**
- Session list, form, and detail pages fully functional
- Report generation API: `POST /api/reports/generate/{patientId}`
- Report list/detail API: `GET /api/reports`, `GET /api/reports/{id}`
- Profile page shows real user data

**Dependencies:** Sprint 1 complete (Session backend, Auth fixes)

**Risks:**
- Report generation depends on OpenAI API availability
- Session frontend models may need updating to match backend DTOs

**Definition of Done:**
- [ ] Session list page loads sessions from backend
- [ ] Session form creates/edits sessions via API
- [ ] Session detail shows note content
- [ ] `POST /api/reports/generate/{patientId}` returns AI-generated report draft
- [ ] Report list page shows generated reports
- [ ] Profile page displays therapist info
- [ ] Profile update saves to backend

---

### SPRINT 3 — Reports Frontend & Integration (Week 3)

**Goals:**
- Complete reports frontend
- Fix remaining frontend-backend integrations
- Assessment and intake form connections

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| FE-RPT-01 | Define report routes | Frontend | 1h |
| FE-RPT-04 | Create ReportService | Frontend | 3h |
| FE-RPT-02 | Build report list page | Frontend | 5h |
| FE-RPT-03 | Build report generate/view page | Frontend | 6h |
| BE-PAT-01 | Intake form CRUD endpoint | Backend | 5h |
| BE-PAT-02 | Assessment CRUD endpoint | Backend | 5h |
| FE-PAT-01 | Connect intake form to backend | Frontend | 4h |
| TEST-001 | Session service unit tests | Backend | 4h |
| TEST-002 | Session controller integration tests | Backend | 4h |

**Expected Deliverables:**
- Reports feature fully functional end-to-end
- Intake forms save to backend
- Assessments viewable from patient detail
- Core session tests written

**Dependencies:** Sprint 2 complete

**Risks:**
- Assessment model may need DTO refinement
- Report UI needs Arabic disclaimer text

**Definition of Done:**
- [ ] Report routes (`/reports`) navigate correctly
- [ ] Report list page shows all reports
- [ ] Generate button triggers AI and displays draft
- [ ] Intake form saves to backend
- [ ] Assessment list displays on patient detail page
- [ ] Session unit tests pass
- [ ] Session controller tests pass

---

### SPRINT 4 — Stabilization & MVP Release (Week 4)

**Goals:**
- Bug fixes and edge cases
- Final integration testing
- UI polish and Arabic labels
- Deployment preparation

**Tasks:**

| ID | Task | Owner | Effort |
|---|---|---|---|
| FE-DASH-01 | Fix dashboard chart Arabic labels | Frontend | 3h |
| TEST-003 | Report service tests | Backend | 4h |
| ALL | End-to-end smoke testing | Full Stack | 8h |
| ALL | Fix all identified bugs | Full Stack | 8h |
| ALL | Responsive UI review | Frontend | 4h |
| ALL | Error handling review | Full Stack | 4h |
| INF-003 | Environment configuration for staging | Backend | 3h |

**Expected Deliverables:**
- All MVP features working end-to-end
- All critical bugs fixed
- Arabic UI labels on charts
- Smoke tests passing

**Dependencies:** Sprints 1-3 complete

**Risks:**
- Integration bugs between frontend and backend
- OpenAI API rate limits during testing

**Definition of Done:**
- [ ] Login → Patient List → Create Patient → Create Session → Assign Exercise → View Dashboard → Generate Report — all work
- [ ] No critical bugs
- [ ] Charts show Arabic labels
- [ ] Responsive on tablet and desktop
- [ ] All smoke tests pass

---

## PHASE 6 — MVP ROADMAP

### Week 1: Foundation & Session Backend

| Day | Backend | Frontend |
|---|---|---|
| Mon | Secure AI endpoints (BE-AUTH-02), Move JWT to env vars (INF-002) | Fix session API prefix (FE-SES-01) |
| Tue | Create Session DTOs (BE-SES-01), Fix ExerciseController (BE-EXE-01) | Fix session model interfaces if needed |
| Wed | Implement SessionService (BE-SES-02) | — |
| Thu | Create SessionController (BE-SES-03) | — |
| Fri | Add session note endpoints (BE-SES-04), CORS fix (INF-001) | Begin connecting session pages |

### Week 2: Session Frontend & Reports Backend

| Day | Backend | Frontend |
|---|---|---|
| Mon | Create Report DTOs (BE-RPT-02) | Connect session list page (FE-SES-02) |
| Tue | Implement ReportService (BE-RPT-03) | Connect session form (FE-SES-03) |
| Wed | Create ReportController (BE-RPT-01) | Connect session detail page |
| Thu | Add profile endpoints (BE-AUTH-01) | Connect profile page (FE-AUTH-01) |
| Fri | Integration testing | Session pages end-to-end testing |

### Week 3: Reports Frontend & Integrations

| Day | Backend | Frontend |
|---|---|---|
| Mon | Intake form CRUD (BE-PAT-01) | Create report routes + service (FE-RPT-01, FE-RPT-04) |
| Tue | Assessment CRUD (BE-PAT-02) | Build report list page (FE-RPT-02) |
| Wed | Session tests (TEST-001) | Build report generate page (FE-RPT-03) |
| Thu | Session controller tests (TEST-002) | Connect intake form (FE-PAT-01) |
| Fri | Report tests (TEST-003) | Reports integration testing |

### Week 4: Stabilization & Release

| Day | Backend | Frontend |
|---|---|---|
| Mon | Bug fixes from testing | Fix chart Arabic labels (FE-DASH-01) |
| Tue | Environment config (INF-003) | Responsive UI review |
| Wed | End-to-end smoke testing (Full Team) | — |
| Thu | Bug fixes (Full Team) | — |
| Fri | Final review, MVP release sign-off | — |

---

## PHASE 7 — MVP RISK REPORT

| Risk | Impact | Probability | Mitigation |
|---|---|---|---|
| Session API returns wrong shape for frontend | High | High | Align DTOs with existing frontend models before building |
| OpenAI API unavailable during demo | Medium | Medium | Use mock response fallback; test with real key before demo |
| Report generation takes >30s | Medium | Low | Add timeout + loading indicator; optimize prompt |
| Frontend session pages hardcoded to wrong URLs | High | High | Fixed in Sprint 1 (FE-SES-01) |
| Assessment/intake form models don't match DB | Medium | Medium | Review migration schema before building endpoints |
| CORS blocks SignalR connection | High | Low | Fixed in Sprint 1 (INF-001) |
| JWT token expiry causes session loss | Medium | Low | Refresh token flow already implemented |
| Team member unavailable during sprint | High | Medium | Cross-train on session and report modules |

---

## PHASE 8 — TEAM EXECUTION PLAN

### Backend Team (2 members)

**Member 1 (Backend Lead):**
- Sprint 1: BE-SES-01, BE-SES-02, BE-SES-03, BE-SES-04 (Session backend)
- Sprint 2: BE-RPT-02, BE-RPT-03, BE-RPT-01 (Report backend)
- Sprint 3: BE-PAT-01, BE-PAT-02 (Intake + Assessment endpoints)
- Sprint 4: Testing, bug fixes

**Member 2 (Backend):**
- Sprint 1: BE-AUTH-02, INF-002, BE-EXE-01, INF-001 (Security + fixes)
- Sprint 2: BE-AUTH-01 (Profile endpoints)
- Sprint 3: TEST-001, TEST-002, TEST-003 (Testing)
- Sprint 4: Environment config, bug fixes

### Frontend Team (2 members)

**Member 3 (Frontend Lead):**
- Sprint 1: FE-SES-01 (Fix endpoints)
- Sprint 2: FE-SES-02, FE-SES-03 (Session pages)
- Sprint 3: FE-RPT-01, FE-RPT-04, FE-RPT-02, FE-RPT-03 (Reports)
- Sprint 4: FE-DASH-01 (Chart labels), responsive review

**Member 4 (Frontend):**
- Sprint 1: Support session model updates
- Sprint 2: FE-AUTH-01 (Profile), support session pages
- Sprint 3: FE-PAT-01 (Intake form connection)
- Sprint 4: Bug fixes, UI polish

### Shared Tasks (Full Stack)

- Sprint 3-4: Integration testing
- Sprint 4: End-to-end smoke testing, deployment prep

---

## PHASE 9 — MVP RELEASE CHECKLIST

### Authentication ✅
- [ ] Login works for Therapist role
- [ ] Login works for Patient role
- [ ] Registration creates Therapist account
- [ ] JWT token injected in API calls
- [ ] Refresh token rotation works
- [ ] Password reset flow works
- [ ] Profile page shows user data
- [ ] Role-based route guards prevent unauthorized access

### Patient Management ✅
- [ ] Patient list loads with search/filter
- [ ] Create patient form works
- [ ] Edit patient form works
- [ ] Patient detail shows all info
- [ ] Archive/restore patient works
- [ ] Intake form saves to backend
- [ ] Assessments display on patient detail

### Session Management ✅
- [ ] Session list loads for patient
- [ ] Create session form works
- [ ] Edit session form works
- [ ] Session detail shows note content
- [ ] Session note fields save correctly
- [ ] Session history shows chronological list
- [ ] Sessions require Therapist role

### Exercise Management ✅
- [ ] Exercise list loads for therapist
- [ ] Assign exercise form works
- [ ] Patient can view assigned exercises
- [ ] Exercise due date can be extended
- [ ] Error messages are user-friendly

### Dashboard ✅
- [ ] Patient count displays
- [ ] Session count displays
- [ ] Exercise completion rate displays
- [ ] Assessment trend chart renders
- [ ] Session frequency chart renders
- [ ] Arabic labels on charts

### AI Reports ✅
- [ ] Generate button triggers AI report
- [ ] Report draft displays correctly
- [ ] Report list shows all reports
- [ ] Report detail view works
- [ ] Arabic disclaimer present on reports

### Technical Quality
- [ ] No 500 errors on any page
- [ ] No console errors in browser
- [ ] Loading spinners on async operations
- [ ] Error messages are Arabic
- [ ] Responsive on desktop and tablet
- [ ] All API calls have error handling
- [ ] No sensitive data in console logs

---

## PHASE 10 — FINAL EXECUTIVE REPORT

### JALSA MVP EXECUTION PLAN — Summary

| Metric | Value |
|---|---|
| **Current Status** | ~38% complete |
| **Estimated MVP Completion** | ~78% after 4 sprints |
| **Estimated Remaining Effort** | 4 weeks (1 sprint/week) |
| **Total Tasks** | 25 tasks (12 Backend, 10 Frontend, 3 Infra/Testing) |
| **Team Size** | 4 developers (2 BE, 2 FE) |

### Critical Blockers

1. 🔥 **No SessionController** — Build in Sprint 1
2. 🔥 **No ReportController** — Build in Sprint 2
3. 🔥 **AI endpoints unsecured** — Fix in Sprint 1
4. 🔥 **Frontend session API prefix mismatch** — Fix in Sprint 1

### MVP Scope

| Included ✅ | Excluded (Post-MVP) 🚀 |
|---|---|
| Login/Register/Profile | Chat support system |
| Patient CRUD + Intake + Assessments | Voice memo STT (Whisper) |
| Session CRUD + Notes | PDF export |
| Exercise CRUD + Patient view | In-app notifications |
| Dashboard with charts | Admin panel |
| AI Report generation (basic) | Semantic search UI |
| Role-based access control | Account lockout |
| Error handling + loading states | Rate limiting |

### Sprint Plan

| Sprint | Focus | Key Deliverables |
|---|---|---|
| **Sprint 1** (Week 1) | Foundation fixes + Session backend | Session API, security fixes |
| **Sprint 2** (Week 2) | Session frontend + Reports backend | Session pages, Report API |
| **Sprint 3** (Week 3) | Reports frontend + integrations | Reports pages, Intake/Assessment |
| **Sprint 4** (Week 4) | Stabilization + MVP release | Bug fixes, testing, polish |

### Major Risks

1. Session DTO alignment between frontend and backend models
2. OpenAI API availability during development and demo
3. Report generation performance (>30s for complex patients)

### Post-MVP Features

1. Chat support with SignalR + crisis detection
2. Voice memo upload with Whisper STT
3. PDF export for referral reports
4. In-app notification system
5. Admin user management panel
6. Semantic search for session notes
7. Account lockout after failed attempts
8. Rate limiting on API endpoints
9. Langfuse LLM observability
10. Sentry error monitoring

### Recommended Immediate Next Actions

1. **Start Sprint 1 today** — Begin with BE-AUTH-02 (secure AI endpoints) and BE-SES-01 (Session DTOs) as they have zero dependencies
2. **Align session DTOs with frontend models** — Read `frontend/src/app/core/models/session.model.ts` and ensure backend DTOs match before building SessionService
3. **Review Session entity in DB** — Check `Sessions` and `SessionNotes` table schema from migration to ensure DTOs map correctly
4. **Set up environment variables** — Create `.env` or `appsettings.Development.json` with JWT secret, OpenAI key, and connection string before Sprint 1 ends
5. **Daily standup at sprint start** — Coordinate backend/frontend alignment on API contracts before implementation begins

---

*Document generated from Jalsa Project Audit Report. All recommendations are based on actual codebase analysis with evidence referenced to specific file paths and line numbers.*
