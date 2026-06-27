
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
