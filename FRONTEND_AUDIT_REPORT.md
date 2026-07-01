# Jalsa Frontend Audit Report

**Audit type:** Read-only senior Angular architect / QA / UX audit
**Date:** 2026-06-30
**Scope:** `frontend/` (Angular 21.2 standalone, RTL Arabic clinic management)
**Mode:** Discovery & reporting only — no code modified.

> **Note on evidence quality:** All issues below are grounded in specific files/lines that were read in full. Where a claim could not be verified (notably the production build, see Phase 2), this is stated explicitly rather than guessed.

---

## 1. Executive Summary

The Jalsa frontend is a well-structured Angular 21.2 standalone application with strong architectural discipline: 100% of components use `OnPush` change detection, there are **zero `any` types** in app code (rule #8 fully respected), subscriptions consistently use `takeUntilDestroyed`, routing is lazy-loaded, and role/auth guards are correctly applied to every feature route. Code quality at the page level is high — most pages have loading, error, empty, and skeleton states with Arabic copy and ARIA attributes.

However, several real issues exist. The most serious are a **self-service "Therapist" role selection at registration** (privilege escalation for a clinical app), a **completely absent JWT refresh-token flow** (the `refreshToken()` method is never wired to any interceptor, so every access-token expiry logs the user out), and a **brand/design-system mismatch** where the `--primary` token is blue `#2563EB` instead of the documented Healthcare Green `#0F6E56`. There are also navigation/UX dead-ends for the Patient role, a missing report "reject" capability in the frontend (contradicting project docs), and stale documentation (the intake/OCR endpoint mismatch is in fact already resolved in code).

**Build status could not be verified** — the isolated Linux sandbox never finished booting across the entire session, so `ng build` / `npm install` could not be executed. Static analysis found no obviously build-breaking constructs (all template/pipe/component imports resolve to existing files, including `status-ar.pipe.ts`), but a clean-compile claim cannot be made from static review alone.

---

## 2. Project Structure Map

### Routing tree (lazy-loaded)
```
'' (AuthLayout)         /auth
                          /login, /register, /forgot-password, /reset-password
                        /auth/profile                [authGuard]
'' (MainLayout)         [authGuard]
                          /dashboard                 [authGuard, roleGuard(Therapist,Admin)]
                          /patients/**               [authGuard, roleGuard(Therapist,Admin)]
                          /sessions/**               [authGuard, roleGuard(Therapist,Admin)]
                          /exercises                 [authGuard]
                             ''  (list)              [roleGuard(Therapist,Admin)]
                             assign                  [roleGuard(Therapist,Admin)]
                             my-exercises            [roleGuard(Patient)]
                          /reports/**                [authGuard, roleGuard(Therapist,Admin)]
                          /chatbot, /chatbot/:id     [authGuard]   (no roleGuard)
                          '' -> redirect dashboard (pathMatch full)
/forbidden               (ForbiddenComponent)
'**' -> redirect auth/login
```

### Core services (11 in core/services + http-client + state)
auth, patient, session, exercise, report, dashboard, navigation, notification (toast), in-app-notification (polling), loading, app-state. HTTP wrapper: `core/api/http-client.service.ts`. State (signals): patient-state, session-state, exercise-state, dashboard-state, report-state, app-state. **No `chat.service.ts`** — chat HTTP is done inline inside chatbot components.

### Guards / interceptors
- `authGuard` (functional), `roleGuard(roles[])` (factory) — both correct.
- Interceptors registered in `app.config.ts`: `authInterceptor`, `errorInterceptor`, `loadingInterceptor`. **No refresh interceptor.**

### Shared (13 components, 5 layouts, 3 pipes, 2 directives, 1 validator)
Components: button, input, textarea, checkbox, radio, select, table (+column-cell directive), pagination, stats-card, empty-state, spinner, modal, toast/toast-container, bar-chart, line-chart. Layouts: main-layout, auth-layout, sidebar, header, footer. Pipes: date-ago, truncate, **status-ar** (exists). Directives: click-outside, role. Validator: password-match.

### Environments
`environment.ts` (dev): `apiUrl: http://localhost:5014`, `signalRHubUrl: .../chatHub`, **`enableMockApi: false`** (see Phase notes — this contradicts CLAUDE.md which claims `true`).

---

## 3. Critical Issues

### C-1. Self-service "Therapist" role selection at registration (privilege escalation)
- **File:** `frontend/src/app/features/auth/pages/register/register.component.html` (lines 66–75); `register.component.ts` (line 34, default `role: 'Therapist'`).
- **Root cause:** The registration form exposes a `<select formControlName="role">` with `<option value="Therapist">أخصائي نفسي</option>` and `<option value="Patient">مريض</option>`, and the value is sent verbatim to `POST /api/auth/register`. Any anonymous visitor can self-register as a Therapist and obtain clinician-level access to patient PHI.
- **Evidence:**
  ```html
  <select class="select" formControlName="role">
      <option value="Therapist">أخصائي نفسي</option>
      <option value="Patient">مريض</option>
  </select>
  ```
- **Suggested fix:** Do not let the client choose a privileged role. Either default all public registrations to `Patient` (or an unprivileged "pending" role) and require admin/invitation-based elevation to Therapist with license verification, or gate Therapist creation behind an admin-only flow. (Backend must also reject client-supplied privileged roles — out of audit scope but flagged.)

### C-2. JWT refresh-token rotation is never wired — every access-token expiry logs the user out
- **Files:** `core/services/auth.service.ts` (lines 82–89 `refreshToken()`), `core/interceptors/error.interceptor.ts` (lines 17–20), `core/interceptors/auth.interceptor.ts` (lines 8–10), `app.config.ts` (line 14).
- **Root cause:** `AuthService.refreshToken()` exists but **no interceptor or guard ever calls it.** `errorInterceptor` handles `401` by immediately calling `authService.logout()` and redirecting to login. `authInterceptor` only skips token attachment for `/refresh`. So when the 1h access token expires, the next request 401s and the user is logged out even though a valid 7d refresh token is in `localStorage`. The documented "refresh token rotation" is effectively dead on the frontend.
- **Evidence (error.interceptor.ts):**
  ```ts
  if (error.status === 401) {
      authService.logout();
      router.navigate(['/auth/login']);
      notification.error('البريد الإلكتروني أو كلمة المرور غير صحيحة');
  }
  ```
- **Suggested fix:** Implement a refresh interceptor that, on `401` (excluding `/login` and `/refresh`), calls `refreshToken()` once, queues concurrent requests, retries them with the new token, and only logs out if refresh itself fails. Use a single in-flight refresh subject to avoid the concurrent-401 race.

### C-3. Build / compile not verified (Phase 2 could not be executed)
- **Root cause (environmental, not a code defect):** The isolated Linux sandbox (`mcp__workspace__bash`) returned "Workspace still starting" on every attempt throughout the session (~14 tries spanning several minutes), so `npm install`, `ng build`, and `ng lint` could **not** be run.
- **Evidence:** every bash invocation returned `Workspace still starting. The isolated Linux environment is booting…`.
- **What static analysis could confirm:** No `any` types; all `@Component` files declare `OnPush`; the one import most likely to break the build (`StatusArPipe` from `shared/pipes/status-ar.pipe` in `report-detail.ts`) resolves — `status-ar.pipe.ts` exists. No missing-file imports were found among files read.
- **Severity rationale:** Marked Critical only because an unverifiable build is a release-blocking unknown; there is **no evidence of an actual build failure.** This should be re-run in a working environment before sign-off: `cd frontend && npm install && npx ng build` and `npm run lint`.

---

## 4. High Issues

### H-1. Patient role sees navigation links that immediately bounce to /forbidden
- **File:** `core/services/navigation.service.ts` (lines 18–25), `app.routes.ts` (lines 42–44), `dashboard.routes.ts` (line 7).
- **Root cause:** The sidebar nav item `{ label: 'لوحة التحكم', route: '/dashboard' }` has **no `roles` restriction**, so it renders for Patients. `/dashboard` is guarded by `roleGuard(['Therapist','Admin'])`. A Patient clicking it is redirected to `/forbidden`. Additionally the MainLayout root `''` redirects unconditionally to `dashboard`, so a Patient navigating to `/` is sent to dashboard → roleGuard → `/forbidden` (no redirect loop, but a dead-end rather than their `my-exercises` home).
- **Evidence (navigation.service.ts):**
  ```ts
  { label: 'لوحة التحكم', icon: 'bi-grid', route: '/dashboard' },   // no roles → shown to Patient
  ```
- **Note:** The "المحادثات" (chatbot) nav item *is* restricted to `['Therapist','Admin','Patient']`, but the `/chatbot` routes themselves only apply `authGuard` (no roleGuard), so role gating there is by menu visibility only.
- **Suggested fix:** Add `roles: ['Therapist','Admin']` to the dashboard nav item, and make the MainLayout `''` redirect role-aware (Patient → `exercises/my-exercises`). Login already redirects Patients correctly (`login.component.ts` line 70), but direct navigation to `/` does not.

### H-2. Misleading global 401 message ("wrong email or password") on session expiry
- **File:** `core/interceptors/error.interceptor.ts` (line 20).
- **Root cause:** Every `401` from any endpoint surfaces the toast `'البريد الإلكتروني أو كلمة المرور غير صحيحة'` (wrong email/password). A 401 caused by an expired token mid-session tells the user their password is wrong, which is confusing and erodes trust.
- **Evidence:** `notification.error('البريد الإلكتروني أو كلمة المرور غير صحيحة');` inside the generic `status === 401` branch.
- **Suggested fix:** Scope the "wrong credentials" message to the login request only (the login component already shows its own credential error at `login.component.ts` 75–78); for other 401s show a neutral "انتهت الجلسة، يرجى تسجيل الدخول مجدداً". Best combined with C-2 (refresh flow).

### H-3. Report "reject" capability missing in frontend (contradicts project docs)
- **Files:** `core/services/report.service.ts` (no `rejectReport`), `features/reports/pages/report-detail/report-detail.ts` (only approve/delete/export).
- **Root cause:** Backend exposes `POST /api/reports/{id}/reject` and `api-endpoints.ts` even defines `reports.reject` (line 40), but **no service method and no UI** call it. `ReportService` has `approveReport` but no `rejectReport`; `report-detail.ts` implements `approveReport`, `deleteReport`, `exportReport` only. CLAUDE.md lists reject as "Done (… reject)" for the frontend — this is inaccurate.
- **Evidence:** `report.service.ts` ends at `exportReport` (line 48–50); there is no `reject` anywhere in the service or component.
- **Suggested fix:** Add `rejectReport(id)` to `ReportService` using `API.reports.reject(id)`, and surface a reject action (with a reason field, matching the backend contract) in `report-detail`.

### H-4. SignalR incoming-message sender mis-mapped; therapist messages render as "Patient"
- **File:** `features/chatbot/pages/chat-room/chat-room.component.ts` (lines 116–126).
- **Root cause:** The `ReceiveMessage` handler maps senders with `senderType: sender === 'AI' ? 'AI' : 'Patient'`. Any non-AI sender (including a Therapist, or the user's own echoed message) is labelled `'Patient'`, so message alignment/attribution in the UI is wrong for multi-party conversations.
- **Evidence:**
  ```ts
  senderType: sender === 'AI' ? 'AI' : 'Patient',
  ```
- **Suggested fix:** Map the actual sender string to the correct `senderType` (`'Therapist' | 'Patient' | 'AI'`) rather than collapsing everything non-AI to Patient.

---

## 5. Medium Issues

### M-1. Primary brand color contradicts the documented Healthcare Green design system
- **File:** `frontend/src/styles/variables.css` (lines 10–21, 219).
- **Root cause:** `--primary: #2563EB` (blue) and the entire `--primary-*` ramp is blue; CLAUDE.md / design-system docs specify Healthcare Green `#0F6E56`. The dashboard charts likewise hardcode Bootstrap blue/green (`dashboard.component.ts` lines 37, 42 use `#0d6efd`, `#198754`), and `ForbiddenComponent` fallback uses `#2563EB`. The shipped product is blue-themed, not the documented green brand.
- **Evidence:** `--primary: #2563EB;` … `--sidebar-active-border: #2563EB;`
- **Suggested fix:** Reconcile tokens with the intended brand. If green is correct, update the `--primary-*` ramp to the `#0F6E56` family and replace hardcoded chart hex with token references.

### M-2. CLAUDE.md known-issue #1 (intake/image endpoint mismatch) is stale — already fixed in code
- **Files:** `core/api/api-endpoints.ts` (line 18), `core/services/patient.service.ts` (lines 57–68), `features/patients/pages/intake-form/intake-form.ts` (lines 96–98).
- **Root cause:** The frontend now calls `API.patients.intakeOcr(patientId, intakeFormId)` = `/api/patient/{id}/intake/{intakeFormId}/ocr`, which **matches** the backend. The documented "frontend calls `/intake/image`" mismatch no longer exists. `uploadIntakeImage` even requires a saved `intakeFormId` first (intake-form.ts 90–95). Documentation is out of date.
- **Suggested fix:** Update CLAUDE.md to remove/close known issue #1.

### M-3. `enableMockApi` documentation mismatch
- **File:** `frontend/src/environments/environment.ts` (line 9).
- **Root cause:** CLAUDE.md (known issue #4 and the env table) states dev has `enableMockApi: true`. The actual file has `enableMockApi: false`. Either the flag was changed without updating docs, or there is a second env file in play. No mock-API consumer of this flag was found among the services read (all hit real endpoints via `HttpClientService`), so the flag may be vestigial.
- **Suggested fix:** Correct the docs; if `enableMockApi` is unused, remove it from the `Environment` model.

### M-4. Dead / mismatched session voice endpoints in `SessionService`
- **File:** `core/services/session.service.ts` (lines 57–63).
- **Root cause:** `getVoiceMemos` issues `GET /api/sessions/{id}/voice` and `deleteVoiceMemo` issues `DELETE /api/sessions/{id}/voice/{memoId}`, but the documented backend only supports `POST /api/sessions/{id}/voice`. These two methods have no matching backend route (would 404/405). They are also not obviously called from any component read.
- **Evidence:**
  ```ts
  getVoiceMemos(sessionId: string) { return this.http.get<VoiceMemo[]>(`${API.sessions.byId(sessionId)}/voice`); }
  deleteVoiceMemo(...) { return this.http.delete<void>(`${API.sessions.byId(sessionId)}/voice/${voiceMemoId}`); }
  ```
- **Suggested fix:** Remove the dead methods or implement the corresponding backend routes; confirm callers first.

### M-5. Chat HTTP performed inline in components instead of a dedicated service (pattern/layering)
- **Files:** `features/chatbot/pages/chat-list/chat-list.component.ts` (line 44), `chat-room.component.ts` (lines 87–88, 160–166).
- **Root cause:** There is no `chat.service.ts`. Components inject `HttpClientService` directly and call chat endpoints inline. `chat-list` even **hardcodes** the URL string `'/api/chat/conversations'` (line 44) instead of using `API.chat.conversations`, diverging from the project's centralized-endpoint convention. (This is the wrapper service, not raw `HttpClient`, so it is not a strict rule #10 violation, but it bypasses the service layer used everywhere else.)
- **Evidence:** `this.http.get<ChatConversation[]>('/api/chat/conversations')`
- **Suggested fix:** Introduce a `ChatService` that owns all chat REST + SignalR calls and uses the `API.chat.*` constants; have components depend on it.

### M-6. Deprecated `.toPromise()` and hardcoded token key in chat-room
- **File:** `features/chatbot/pages/chat-room/chat-room.component.ts` (lines 111, 160–166).
- **Root cause:** Uses RxJS `.toPromise()` (deprecated, removed in newer RxJS) for the REST fallback send, and reads the JWT via `localStorage.getItem('jalsa_token')` with a hardcoded key string that duplicates the private `TOKEN_KEY` in `AuthService`. If the key constant ever changes, SignalR auth silently breaks.
- **Evidence:** `accessTokenFactory: () => localStorage.getItem('jalsa_token') ?? ''` and `.post(...).toPromise()`.
- **Suggested fix:** Use `firstValueFrom(...)` instead of `.toPromise()`; obtain the token via `authService.getToken()` rather than a literal key.

### M-7. Native search input uses non-existent `ariaLabel` attribute (no accessible name)
- **File:** `features/patients/pages/patient-list/patient-list.html` (line 18).
- **Root cause:** A native `<input type="search">` has `ariaLabel="بحث في المرضى"`. `ariaLabel` is a *component @Input* name, not a valid HTML attribute — on a native element it is ignored, so the search box has no accessible name. (The same camelCase is fine where it is bound to `app-button`/`app-input` components.)
- **Evidence:** `<input type="search" ... ariaLabel="بحث في المرضى" />`
- **Suggested fix:** Use `aria-label="بحث في المرضى"` on native elements.

---

## 6. Low Issues

### L-1. `getRoleLabel()` duplicated verbatim in two components
- **Files:** `shared/layouts/header/header.component.ts` (80–88) and `shared/layouts/sidebar/sidebar.component.ts` (36–44) — identical role→Arabic-label switch. **Fix:** extract to a shared pipe/util.

### L-2. Hardcoded chart colors instead of design tokens
- **File:** `features/dashboard/pages/dashboard/dashboard.component.ts` (37, 42, 53). Hex literals `#0d6efd`, `#198754`, `#ffc107`, `#dc3545`. **Fix:** reference CSS variables / a shared palette constant.

### L-3. Some physical CSS properties remain in shared component styles
- **Files:** `auth-layout.component.css`, `checkbox.component.css`, `spinner.component.css`, `radio.component.css`, `empty-state.component.css` (10 occurrences of `left/right/margin-left/right` etc.). Most are decorative (spinner rotation, absolute icon positioning) and not necessarily RTL-incorrect, but they violate the "logical properties first" rule (#6). The global stylesheet correctly uses `border-inline-start` (good). **Fix:** review each; convert layout-affecting ones to logical properties.

### L-4. `app.ts` (root) has no explicit `OnPush`
- **File:** `app.ts`. Every other component sets `OnPush`; the root shell does not. Negligible impact (it is a thin shell), but inconsistent with rule #5. **Fix:** add `changeDetection: OnPush` for consistency.

### L-5. `setToken`/`setRefreshToken` store JWT + refresh token in `localStorage`
- **File:** `core/services/auth.service.ts` (132–142). `localStorage` is XSS-readable. Common for SPAs and arguably acceptable, but for a PHI/clinical app consider httpOnly cookies or at least documenting the threat model. Flagged as Low/informational, not a confirmed exploit.

### L-6. Registration success uses a fixed 2s `setTimeout` redirect
- **File:** `features/auth/pages/register/register.component.ts` (100–102). Minor UX nit — a manual "go to login" affordance would be more robust than a timed redirect.

### L-7. `$any($event.target)` in template
- **File:** `patient-list.html` (17). Uses template `$any` to read `.value`; allowed but a typing escape hatch. **Fix:** use a typed event helper or `(input)` with `$event.target as HTMLInputElement` pattern via a method.

---

## 7. Scores (out of 100)

| Score | Value | Justification |
|-------|-------|---------------|
| **Frontend Health** | **78** | Excellent architectural hygiene (0 `any`, 100% OnPush, consistent `takeUntilDestroyed`, lazy routes, guards on every feature). Pulled down by C-1 (role escalation), C-2 (no working refresh flow), and an unverifiable build (C-3). No confirmed build break found statically. |
| **UI** | **74** | Clean, consistent component library; good Arabic RTL via Bootstrap RTL + logical properties in global CSS. Major deduction for the brand/design-token mismatch (M-1: blue vs documented green) and hardcoded chart colors; minor physical-CSS leftovers. |
| **UX** | **75** | Strong page-level UX: skeleton loaders, error states with retry, empty states, ARIA on layout and buttons. Deductions for Patient navigation dead-ends (H-1), misleading 401 messaging (H-2), and missing report-reject action (H-3). |
| **Performance** | **84** | Fully lazy-loaded features, OnPush everywhere, `takeUntilDestroyed` prevents leaks, sensible 5-min dashboard auto-refresh with `filter` guard, production budgets defined (700kB warn / 1MB error). No eager feature imports found. Bundle size itself unverified (build did not run). |
| **Architecture** | **82** | Clean Core/Shared/Features separation, centralized `API` endpoint map, signal-based state services exposing `asReadonly()`, single HTTP wrapper. Deductions: chat bypasses the service layer with an inline hardcoded URL (M-5), duplicated logic (L-1), and the refresh-flow gap (C-2) is an architectural omission. |

---

## 8. Appendix

### A. Build / lint command output
**Not available.** The `mcp__workspace__bash` sandbox never finished booting; every invocation (≈14 attempts across the session) returned:
```
Workspace still starting. The isolated Linux environment is booting in the background (usually 10–30 seconds). Try again shortly.
```
`npm install`, `npx ng build`, `npm run build`, and `npx ng lint` could therefore not be executed. **Recommended re-run in a working shell:**
```
cd frontend
npm install
npx ng build            # or: npm run build   (defaultConfiguration = production)
npm run lint            # eslint . --ext .ts,.html
```
Static substitute checks performed instead: no `any` types in app code; all 48 component files declare `ChangeDetectionStrategy.OnPush`; the highest-risk import (`status-ar.pipe`) resolves to an existing file.

### B. Files read in full
- Config/build: `frontend/package.json`, `frontend/angular.json`, `frontend/src/environments/environment.ts`, `frontend/src/styles.css`, `frontend/src/styles/variables.css` (grepped)
- App shell: `app.routes.ts`, `app.config.ts`, `shared/layouts/main-layout/main-layout.component.html`
- Core/api: `core/api/api-endpoints.ts`, `core/api/http-client.service.ts`
- Guards/interceptors: `core/guards/auth.guard.ts`, `core/guards/role.guard.ts`, `core/interceptors/auth.interceptor.ts`, `core/interceptors/error.interceptor.ts`
- Core services: `auth.service.ts`, `patient.service.ts`, `session.service.ts`, `report.service.ts`, `exercise.service.ts`, `dashboard.service.ts`, `notification.service.ts`, `in-app-notification.service.ts`, `navigation.service.ts`
- Feature routes: `auth.routes.ts`, `dashboard.routes.ts`, `patients.routes.ts`, `sessions.routes.ts`, `reports.routes.ts`, `exercises.routes.ts`, `chatbot.routes.ts`
- Feature components: `auth/login.component.ts`, `auth/register.component.ts` (+ `.html`), `auth/forbidden.component.ts`, `patients/patient-form.ts`, `patients/intake-form.ts`, `patients/patient-list.html`, `sessions/session-form.ts`, `reports/report-generate.ts`, `reports/report-detail.ts`, `exercises/assign-exercise.component.ts`, `dashboard/dashboard.component.ts`, `chatbot/chat-list.component.ts`, `chatbot/chat-room.component.ts`
- Layouts: `header.component.ts`, `sidebar.component.ts`
- Structural inventory via Glob/Grep across `frontend/src/app/**` (TS/HTML/CSS, `@Component`/OnPush counts, `any`, physical CSS props, subscriptions, HttpClient/SignalR usage, pipe existence).

### C. Verified-good (not issues)
- 0 `any` types in app code (rule #8).
- 48/48 components use `OnPush` (rule #5); only `app.ts` shell omits it (L-4).
- Subscriptions consistently piped through `takeUntilDestroyed` (no leak pattern found in components).
- Every feature route applies `authGuard` (+ `roleGuard` where appropriate).
- `ForbiddenComponent.goHome()` is genuinely role-aware (Patient → my-exercises, else dashboard).
- Login redirect is role-aware for Patients.
- Intake OCR endpoint now matches backend (known-issue #1 resolved).
- Forms (login, register, patient, intake, assign-exercise, report-generate, session) apply validators, display errors in templates, handle submit errors, and disable submit via a `loading`/`generating` signal while in flight.
