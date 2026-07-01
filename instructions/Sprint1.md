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
