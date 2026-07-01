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
