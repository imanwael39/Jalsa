
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
