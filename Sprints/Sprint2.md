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
