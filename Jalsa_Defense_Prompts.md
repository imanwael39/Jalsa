# Jalsa Presentation Defense — Ready-to-Use Prompts

Two prompts below. Paste either one into an AI assistant (or use it as a personal study script) that has access to the Jalsa repository. Prompt 1 goes deep on the Frontend — your part. Prompt 2 gives you lead-level command of the whole system so you can answer any question thrown at your team.

---

## Prompt 1 — Frontend Deep Dive (for defending YOUR part)

```
You are helping me prepare to defend the Frontend implementation of "Jalsa," an Arabic
RTL clinic management system for psychological therapists (Angular 21.2 standalone
components, TypeScript 5.9, Bootstrap 5.3 RTL, Tailwind CSS 4.1, Chart.js/ng2-charts,
ngx-quill, SignalR client). I am the team lead and the Frontend owner, and I need to
be able to answer any technical question a panel might ask about it.

Go through the codebase at frontend/src/app and produce a defense-ready explanation
covering, in this order:

1. ARCHITECTURE
   - Explain the Core/Shared/Features folder split and why it exists.
   - Walk through app.config.ts: every provider registered (router, animations,
     interceptors, charts) and what each does.
   - Walk through app.routes.ts: lazy-loaded feature routes, and how authGuard and
     roleGuard(roles[]) gate them.

2. STATE MANAGEMENT
   - Explain the hand-rolled per-domain state pattern (list + selected-item + loading +
     error + computed) used instead of NgRx.
   - Go service-by-service through core/state/: patient-state, session-state,
     exercise-state, dashboard-state, report-state. For each: what signals it exposes
     as asReadonly(), what computed() values it derives, and which components consume it.
   - Explain why components never call HttpClientService directly, and trace one full
     data flow: component -> state service -> domain service -> HttpClientService ->
     interceptors -> backend.

3. CORE SERVICES (16 total in core/services/)
   - List each service and its single responsibility (auth, patient, session, exercise,
     dashboard, report, notification, loading, app-state, navigation, in-app-notification,
     plus the 5 state services).
   - Explain http-client.service.ts as the single HTTP gateway.

4. INTERCEPTORS AND GUARDS
   - auth.interceptor.ts: how it attaches the Bearer token, and why it skips /refresh.
   - refresh-token.interceptor.ts: the refresh-token rotation flow on 401.
   - error.interceptor.ts: Arabic error message mapping and 401 auto-logout.
   - loading.interceptor.ts: how it drives the global spinner.
   - authGuard vs roleGuard: redirect behavior, including the Patient-role special case
     (redirects to /exercises/my-exercises instead of /dashboard) and the /forbidden page.

5. SHARED COMPONENT LIBRARY (13 components)
   - Form controls: Button, Input, Textarea, Checkbox, Radio, Select — explain the
     shared API pattern (ControlValueAccessor / reactive forms integration).
   - Data display: Table (+ column-cell directive), Pagination, StatsCard, EmptyState.
   - Feedback: Spinner, Modal, Toast/ToastContainer.
   - Charts: BarChart, LineChart (Chart.js/ng2-charts wrappers) and how Dashboard uses them.
   - Explain OnPush change detection: why every component uses it, and how signals make
     that safe (no manual markForCheck needed).

6. FEATURES — walk each of the 7 feature modules and its pages, explaining the purpose
   and key logic of each page:
   - Auth: login, register, forgot-password, reset-password, profile, forbidden
   - Patients: patient-list, patient-form, patient-detail, intake-form, assessment
   - Sessions: session-landing, session-list, session-form, session-detail, summary
   - Exercises: exercise-list, assign-exercise, patient-exercise
   - Reports: report-landing, report-list, report-generate, report-detail
   - Dashboard: single dashboard page with charts + stats cards
   - Chatbot: chat-list, chat-room, with @microsoft/signalr client wiring to the
     backend's /chatHub for real-time messages

7. RTL AND ARABIC-FIRST DESIGN
   - Explain "logical CSS properties" (margin-inline-start etc.) and why they're
     mandatory instead of margin-left/right.
   - Design tokens in styles/variables.css, the Tajawal/Cairo/IBM Plex Sans Arabic font
     stack, and the Professional Blue (#2563eb) palette.
   - How Bootstrap 5.3 RTL and Tailwind CSS 4.1 coexist in the same app.

8. TESTING
   - 270 Vitest tests across 24 spec files. Name what's covered: app, http-client, auth
     guard, role guard, all 3 interceptors, 3 state services, state-utils, app-state,
     exercise service, loading, notification, assessment, intake-form, patient-detail,
     patient-form, and the 6 form components.
   - Explain @analogjs/vite-plugin-angular's role in running Angular tests under Vitest.

9. LIKELY DEFENSE QUESTIONS — generate 15 hard questions a panel could ask about the
   Frontend (e.g. "why signals instead of NgRx," "why no shared base state class,"
   "how do you prevent stale state across two state services showing the same patient,"
   "what happens on token refresh failure mid-request," "why is enableMockApi still
   true in dev and is that a risk") and give me strong, specific answers to each,
   citing the actual files/patterns above.

10. KNOWN GAPS — be upfront about what's incomplete or weak in the Frontend (e.g. no
    shared base state class causes duplication across 5 state services, mock API still
    enabled in dev, only 3 DTOs have FluentValidation on the backend which affects
    frontend error handling assumptions) so I can address them proactively instead of
    getting caught off guard.

Be specific and technical — reference real file paths, class names, and patterns from
the repo rather than generic Angular theory. Where relevant, quote short code snippets
to back up an explanation.
```

---

## Prompt 2 — Whole-Project Command (for you as Team Lead)

```
You are helping me, the team lead of "Jalsa" (an Arabic RTL clinic management system,
ITI Capstone 2026, Group 6, .NET Track, 5 members), build complete command of the
entire system — not just my Frontend part — so I can answer questions about any
teammate's work during our presentation defense.

Tech stack: Angular 21.2 frontend / ASP.NET Core 8 + C# 12 backend / SQL Server (34
tables) / EF Core / FluentValidation / Hangfire / JWT auth (HS256, 1h access + 7d
refresh rotation) / BCrypt / SignalR / an ITI student AI Gateway proxying Bedrock
Claude 3 Haiku (chat) and Titan Embed Text v2 (embeddings), no Semantic Kernel /
Langfuse for LLM observability / xUnit+Moq+FluentAssertions and Vitest for testing /
GitHub Actions CI.

Produce a lead-level briefing covering:

1. SYSTEM ARCHITECTURE
   - Explain Clean Architecture layering: Domain -> Application -> Infrastructure -> API.
   - What lives in each of the 4 backend projects (Jalsa.Domain, Jalsa.Application,
     Jalsa.Infrastructure, Jalsa.API) and why business logic must never sit in controllers.
   - How the Frontend and Backend integrate end to end for one full user action
     (e.g. therapist creates a session note) — from Angular component through
     interceptors, to the controller, service, repository, EF Core, SQL Server, and back.

2. DOMAIN MODEL — walk through all 33 entities grouped by category (Identity, Clinic,
   Patient, Session, Assessment, Exercise, Chat/AI, Reports, System) and explain the
   key relationships (e.g. Patient -> IntakeForm -> IntakeFormOcrExtraction, even
   though OCR extraction itself was removed; Session -> SessionNote -> SessionEmbedding;
   ReferralReport -> ReportVersion for versioning).

3. ALL 11 CONTROLLERS — for each, list its route, auth requirements, and endpoints:
   AuthController, PatientController, SessionController, ExerciseController,
   ReportController, AssessmentController, IntakeController, AiController,
   ProgressController, ChatController, NotificationController. Explain the ★=[Authorize]
   convention and where role restriction (Therapist-only) is applied.

4. AI SUBSYSTEM — explain the whole pipeline in detail since this is often the most
   heavily questioned part:
   - IGatewayClient and GatewayClient: raw HTTP calls to the ITI AI Gateway (no
     Semantic Kernel), proxying Bedrock Claude 3 Haiku for chat and Titan Embed Text
     v2 for embeddings.
   - ChatAiService, ConversationMemoryService, CrisisDetectionService,
     EmbeddingService, VectorStore (SQL-based vector search), ReportGenerationService,
     SummarizationService, PromptService (versioned prompts per task/language), and
     LangfuseObservabilityService.
   - Explain why OCR and Whisper STT were removed (gateway has no vision/audio proxy)
     and what functionality that cost the product (intake form image capture, session
     voice memos) — be ready to explain this as a deliberate, justified scope cut.
   - Trace the AI report generation flow end to end: AiController/ReportController ->
     ReportGenerationService -> RAG context via VectorStore -> GatewayClient chat call
     -> Langfuse logging -> versioned ReferralReport saved.

5. AUTH AND SECURITY
   - JWT flow: access token (1h) + refresh token rotation (7d), BCrypt hashing.
   - RefreshToken and PasswordResetToken entities and their lifecycle.
   - What's NOT done yet: account lockout, rate limiting (listed as Post-MVP gaps) —
     be ready to explain these as known, tracked gaps rather than oversights.

6. REAL-TIME (SignalR)
   - ChatHub at /chatHub: SendMessage and JoinConversation methods.
   - How it's mapped in Program.cs and consumed by the Angular chat-room component.

7. BACKGROUND JOBS
   - Hangfire's ExerciseReminderJob running daily at 9:00 AM — what it does and why
     it's needed for exercise adherence tracking.

8. DATA LAYER
   - JalsaDbContext with 34 DbSets, 1 migration, 8 repositories behind IUnitOfWork.
   - Why "all DB changes via migrations only" is enforced as a rule.

9. TESTING AND CI/CD
   - Backend: 51 xUnit tests across 7 classes (ExerciseServiceTests,
     ProgressControllerTests, ProgressServiceTests, AssessmentServiceTests,
     IntakeServiceTests, SessionServiceTests, SessionControllerTests), using EF
     InMemory and the AsyncQueryProvider test helper.
   - Frontend: 270 Vitest tests, 24 spec files.
   - GitHub Actions workflow: parallel backend (.NET 8) and frontend (Node 22) jobs on
     push/PR to develop/main.

10. PROJECT STATUS AND HONEST GAPS — summarize the module completion table (Auth 98%,
    Patients 90%, Sessions 95%, Exercises 90%, Dashboard 90%, AI Reports 95%,
    Chatbot 85%, Notifications 90%) and the known active issues: only Exercise DTOs
    have FluentValidation (others rely on DataAnnotations only), no integration/E2E
    tests (unit-only, mocked), mock API still enabled in frontend dev environment.
    Also state clearly what's explicitly Post-MVP and out of scope: Admin panel,
    semantic search UI, account lockout, rate limiting, binary PDF export (we ship
    HTML export instead).

11. LIKELY CROSS-CUTTING DEFENSE QUESTIONS — generate 15 hard questions a panel could
    ask that span team members' work (e.g. "how do you guarantee a therapist can only
    see their own patients," "what happens if the AI Gateway is down mid-report-generation,"
    "why SQL-based vector search instead of a dedicated vector DB," "how does crisis
    detection actually trigger an alert and who sees it," "what's your rollback story
    if a migration fails in production") and give me strong, specific answers, citing
    the real services/entities/flows above.

Be specific and technical, cite real class/file/entity names from the repo, and
organize the answer so I can use it as my personal briefing document before the
presentation.
```
