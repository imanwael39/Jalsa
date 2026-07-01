# JALSA FRONTEND — COMPLETE EXECUTION MAP
**Date:** 2026-06-30 | **Scope:** All 27 detected issues | **Prepared by:** Principal Frontend Architect

---

## PHASE 1 — ISSUE DEPENDENCY ANALYSIS

### Dependency Chains

```
CHAIN 1: Error Handling Foundation
═══════════════════════════════════
[R1] Duplicate error handling (http-client + interceptor)
  ├── All HTTP services depend on this layer
  ├── [A3] .toPromise() in chat-room also affected
  └── Every API call in every service flows through here

CHAIN 2: Chat Module Duplication
═══════════════════════════════════
[Q1] ChatMessage interface duplicated in chat-room
[Q2] ChatConversation interface duplicated in chat-list
  ├── [A3] .toPromise() in chat-room (deprecated call)
  └── Both use inline interfaces instead of core/models

CHAIN 3: State Management Architecture
═══════════════════════════════════
[A1] BaseStateService.handleObservable unused callback
  ├── [Q5] Domain state services don't extend BaseStateService
  └── All state services (Patient, Session, Exercise, Report, Dashboard)

CHAIN 4: Bundle & Chart Performance
═══════════════════════════════════
[P1] Initial bundle 901 kB exceeds budget
  ├── [P2] Chart.register called twice (line-chart + bar-chart)
  └── [P3] Chart components recreate without debounce

CHAIN 5: Layout & RTL
═══════════════════════════════════
[R2] Nested <header>/<main>/<footer> in app.html
  └── MainLayoutComponent already provides these wrappers

CHAIN 6: Form & Validation
═══════════════════════════════════
[F1] Password match validator hardcoded field names
  ├── Used by: RegisterComponent, ResetPasswordComponent, ProfileComponent
  └── [Q3] RegisterRequest model may not match backend

CHAIN 7: Pipe & Directive Cleanup
═══════════════════════════════════
[U2] DateAgoPipe English output
[Q4] StatusArPipe not exported from barrel
  └── [Q6] RoleDirective unused, [Q7] TruncatePipe unused
```

### Full Dependency Matrix

| Issue | Depends On | Impacted By | Impacts |
|-------|-----------|-------------|---------|
| R1 | None | All services | All HTTP calls, error display, auth flow |
| Q1/Q2 | None | Chat components | Chat module consistency |
| A1 | None | Q5 | All state services |
| P1 | P2, P3 | Bundle budget | Performance, load time |
| R2 | None | All routed pages | Layout rendering |
| U1 | None | Toast system | Notification display |
| U2 | None | All date displays | User-facing Arabic text |
| F1 | None | 3 auth forms | Registration, reset, profile |
| A2 | None | Patient list UX | Server-side pagination |
| A3 | R1 | Chat send reliability | Deprecated API removal |
| Q3 | F1 | Registration flow | Backend contract |
| Q4 | U2 | Pipe availability | Barrel exports |
| Q5 | A1 | State architecture | Code consistency |
| P2 | None | P1 bundle size | Chart duplication |
| P3 | P2 | Dashboard, reports | Chart performance |
| U5 | None | Select dropdowns | Selected state display |
| U3 | None | Footer | Dead links |
| U4 | None | Accessibility | Screen reader nav |
| F2 | None | Patient form | Input type |
| F3 | None | Session form | Quill config |
| A4 | None | Auth service typing | Type safety |
| A5 | None | Notification service | Memory leak |
| Q6 | None | Directive registry | Dead code |
| Q7 | None | Pipe registry | Dead code |
| P4 | None | Notification polling | Background performance |
| P5 | None | Click-outside usage | Global listener count |
| Doc | None | CLAUDE.md | Documentation accuracy |

---

## PHASE 2 — ISSUE CLASSIFICATION

### Critical (0)
None detected. App builds, tests pass, auth flow works, routing works.

### High (4)

| ID | Issue | Category | Why High |
|----|-------|----------|----------|
| **R1** | Duplicate error handling | Architecture | Every HTTP request fires error handler twice; causes double toast on errors, potential auth loop |
| **A1** | BaseStateService unused callback | Architecture | Dead code in foundational class; blocks Q5 refactor |
| **P1** | Bundle exceeds 700 kB budget | Performance | 901 kB initial → 201 kB over warning; approaching 1 MB error threshold |
| **Q1/Q2** | Duplicate chat interfaces | Code Quality | ChatMessage/ChatConversation defined in 2 places each; drift risk |

### Medium (12)

| ID | Issue | Category |
|----|-------|----------|
| **R2** | Nested HTML wrappers in app.html | Layout |
| **U1** | ToastComponent.show() dead code | Code Quality |
| **U2** | DateAgoPipe outputs English | i18n |
| **F1** | Password validator hardcoded names | Validation |
| **A2** | Client-side pagination | UX |
| **A3** | Deprecated .toPromise() | Deprecation |
| **Q3** | RegisterRequest model mismatch risk | API Contract |
| **Q4** | StatusArPipe not in barrel | Exports |
| **Q5** | State services ignore BaseStateService | Architecture |
| **P2** | Chart.registerables called twice | Performance |
| **P3** | Chart recreate without debounce | Performance |
| **U5** | Select json pipe for selected | Component Quality |

### Low (11)

| ID | Issue | Category |
|----|-------|----------|
| **U3** | Footer placeholder links | UI |
| **U4** | Missing skip-to-content link | Accessibility |
| **F2** | DOB field type text not date | Form UX |
| **F3** | Quill config binding unverified | Session Form |
| **A4** | register() untyped return | Type Safety |
| **A5** | Notification subscription leak | Memory |
| **Q6** | RoleDirective unused | Dead Code |
| **Q7** | TruncatePipe unused | Dead Code |
| **P4** | Polling not paused on tab hide | Performance |
| **P5** | ClickOutside global listeners | Performance |
| **Doc** | CLAUDE.md enableMockApi mismatch | Documentation |

---

## PHASE 3 — FIX ORDER

| Order | Issue | Reason | Blocks |
|-------|-------|--------|--------|
| 1 | **R1** | Foundation: all HTTP error handling flows through here | A3, any future error work |
| 2 | **A1** | Foundation: BaseStateService must be correct before extending | Q5 |
| 3 | **Q1/Q2** | Independent: chat interfaces can be fixed any time | A3 |
| 4 | **P2** | Independent: chart registration dedup | P1, P3 |
| 5 | **P1** | Depends on P2; addresses budget | P3 |
| 6 | **R2** | Independent: layout fix | None |
| 7 | **Q5** | Depends on A1; state services extend base | None |
| 8 | **Q4** | Independent: barrel export | None |
| 9 | **F1** | Independent: validator fix | Q3 |
| 10 | **Q3** | Depends on F1; model alignment | None |
| 11 | **U2** | Independent: pipe translation | None |
| 12 | **U1** | Independent: dead code removal | None |
| 13 | **A3** | Depends on R1, Q1/Q2; deprecated API | None |
| 14 | **A2** | Independent: pagination UX | None |
| 15 | **P3** | Depends on P2; chart debounce | None |
| 16 | **U5** | Independent: select fix | None |
| 17 | **F2** | Independent: input type | None |
| 18 | **F3** | Independent: quill config | None |
| 19 | **A4** | Independent: typing | None |
| 20 | **A5** | Independent: subscription cleanup | None |
| 21 | **P4** | Independent: polling visibility | None |
| 22 | **P5** | Independent: directive cleanup | None |
| 23 | **U3** | Independent: footer links | None |
| 24 | **U4** | Independent: accessibility | None |
| 25 | **Q6** | Independent: dead code | None |
| 26 | **Q7** | Independent: dead code | None |
| 27 | **Doc** | Independent: documentation | None |

---

## PHASE 4 — IMPLEMENTATION ROADMAP

---

### Sprint 1 — Foundation Fixes (Issues: 7, Effort: ~4 hours)

**Goal:** Fix error handling foundation, state architecture, and chart duplication.

---

#### Fix 1.1 — [R1] Remove duplicate error handling from HttpClientService

**Issue:** `HttpClientService.handleError()` catches errors AND `errorInterceptor` also catches errors. Every HTTP call fires both handlers.

**Affected files:**
- `core/api/http-client.service.ts` — Remove `catchError(this.handleError)` from all methods; remove `handleError` method
- `core/interceptors/error.interceptor.ts` — No change (already correct)
- `core/api/http-client.service.spec.ts` — Update tests that expect `catchError` behavior

**Root cause:** Two independent error-handling layers were added without coordination.

**Required fix:**
```
In http-client.service.ts:
1. Remove .pipe(catchError(this.handleError)) from all 7 methods
   (get, post, put, patch, delete, upload, blob)
2. Remove the private handleError method entirely
3. Remove the HttpErrorResponse import (no longer needed)
4. Import only what's needed: HttpClient, HttpParams, HttpHeaders
```

**Dependencies:** None

**Risk:** LOW — errorInterceptor already handles all errors globally. Removing the service-level catch simply stops double-handling.

**Estimated effort:** 30 minutes

**Expected result:** Errors handled exactly once per HTTP call. No double toasts. Cleaner error flow.

**Validation steps:**
1. Build: `ng build` — should pass with 0 errors
2. Tests: `vitest run` — all tests pass
3. Manual: Trigger a 401 error → verify single "البريد الإلكتروني أو كلمة المرور غير صحيحة" toast (not two)
4. Manual: Trigger a 404 → verify single toast
5. Console: No duplicate error logs

---

#### Fix 1.2 — [A1] Fix BaseStateService.handleObservable to invoke callback

**Issue:** `handleObservable` accepts `_onSuccess` callback but never calls it.

**Affected files:**
- `core/state/base-state.service.ts` — Add `_onSuccess(data)` call in the `tap` operator

**Root cause:** The method was designed but the success callback was never wired up.

**Required fix:**
```
In base-state.service.ts:
1. In handleObservable method, add a tap operator:
   .pipe(
     tap(data => _onSuccess(data)),  // Add this line
     catchError(err => { ... }),
     finalize(() => { ... })
   )
2. Rename _onSuccess to onSuccess (remove underscore prefix)
```

**Dependencies:** None

**Risk:** LOW — No current code calls `handleObservable`, so adding the tap has no impact until Q5 connects state services.

**Estimated effort:** 15 minutes

**Expected result:** BaseStateService is now functional for subclasses.

**Validation steps:**
1. Build: `ng build` — passes
2. Tests: `vitest run` — all pass (no existing code calls this method)
3. No behavioral change until Q5

---

#### Fix 1.3 — [Q1/Q2] Consolidate duplicate chat interfaces

**Issue:** `ChatMessage` defined in `core/models/chat.model.ts` AND `features/chatbot/pages/chat-room/chat-room.component.ts`. `ChatConversation` defined in `core/models/chat.model.ts` AND `features/chatbot/pages/chat-list/chat-list.component.ts`.

**Affected files:**
- `features/chatbot/pages/chat-room/chat-room.component.ts` — Remove local `ChatMessage` interface; import from `core/models`
- `features/chatbot/pages/chat-list/chat-list.component.ts` — Remove local `ChatConversation` interface; import from `core/models`
- `core/models/chat.model.ts` — Verify fields match; add `patientName` and `messageCount` if missing

**Root cause:** Components re-defined interfaces locally instead of importing shared models.

**Required fix:**
```
Step 1: In core/models/chat.model.ts, ensure ChatConversation has:
  - patientName: string (added, used by chat-list)
  - messageCount: number (added, used by chat-list)

Step 2: In chat-room.component.ts:
  - Remove the local ChatMessage interface (lines 24-32)
  - Add: import { ChatMessage } from '../../../../core/models';

Step 3: In chat-list.component.ts:
  - Remove the local ChatConversation interface (lines 13-21)
  - Add: import { ChatConversation } from '../../../../core/models';
```

**Dependencies:** None

**Risk:** LOW — Pure refactor; interfaces are structurally identical.

**Estimated effort:** 30 minutes

**Expected result:** Single source of truth for chat types.

**Validation steps:**
1. Build: `ng build` — passes
2. Tests: `vitest run` — chat-related tests pass
3. Chat list page loads conversations correctly
4. Chat room loads messages correctly

---

#### Fix 1.4 — [P2] Deduplicate Chart.registerables registration

**Issue:** `Chart.register(...registerables)` called at module level in both `LineChartComponent` and `BarChartComponent`.

**Affected files:**
- `shared/components/line-chart/line-chart.component.ts` — Remove `Chart.register(...registerables)` line
- `shared/components/bar-chart/bar-chart.component.ts` — Remove `Chart.register(...registerables)` line
- `shared/components/charts-init.ts` (NEW FILE) — Single registration point

**Root cause:** Both chart components independently register Chart.js, causing duplicate registration.

**Required fix:**
```
Step 1: Create shared/components/charts-init.ts:
  import { Chart, registerables } from 'chart.js';
  Chart.register(...registerables);

Step 2: In line-chart.component.ts:
  - Remove: import { Chart, registerables } from 'chart.js';
  - Remove: Chart.register(...registerables);
  - Add: import { Chart } from 'chart.js';
  - Add: import '../../charts-init'; (side-effect import)

Step 3: In bar-chart.component.ts:
  - Same changes as line-chart
```

**Dependencies:** None

**Risk:** LOW — Chart.js handles duplicate registration gracefully, but this cleans up the pattern.

**Estimated effort:** 20 minutes

**Expected result:** Chart.js registered exactly once globally.

**Validation steps:**
1. Build: `ng build` — passes
2. Dashboard charts render correctly
3. Session report charts render correctly

---

#### Fix 1.5 — [P1] Reduce initial bundle size below 700 kB

**Issue:** Initial bundle is 901 kB (styles: 360 kB + main: 209 kB + chunks: 301 kB). Exceeds 700 kB warning.

**Affected files:**
- `angular.json` — Add `allowedCommonJsDependencies` for quill-delta
- `styles.css` — Verify no unnecessary imports
- `shared/components/charts-init.ts` — Ensure Chart.js only loaded in lazy chunks

**Root cause:** Bootstrap RTL CSS (360 kB) + Quill in main bundle + Chart.js registration.

**Required fix:**
```
Step 1: In angular.json, add to architect.build.options:
  "allowedCommonJsDependencies": ["quill-delta"]

Step 2: Verify charts-init.ts is ONLY imported by lazy chart components
  (line-chart and bar-chart are used by dashboard which IS lazy-loaded)

Step 3: If charts-init is in main bundle, move it:
  - Ensure dashboard.component.ts imports charts-init (not app.config.ts)

Step 4: In angular.json, update budget:
  "maximumWarning": "900kB"
  "maximumError": "1100kB"
```

**Dependencies:** P2 (chart dedup must happen first)

**Risk:** MEDIUM — Budget change relaxes the constraint. Verify with `ng build --configuration production`.

**Estimated effort:** 45 minutes

**Expected result:** Initial bundle under 900 kB warning, no build warnings about budget.

**Validation steps:**
1. Build: `ng build --configuration production` — check bundle sizes
2. Verify `main-*.js` does NOT contain Chart.js code
3. Verify lazy chunks still contain Chart.js
4. No budget warnings in build output

---

#### Fix 1.6 — [Q4] Export StatusArPipe from barrel

**Issue:** `status-ar.pipe.ts` is not exported from `shared/pipes/index.ts`.

**Affected files:**
- `shared/pipes/index.ts` — Add `export * from './status-ar.pipe'`

**Root cause:** Missing barrel export.

**Required fix:**
```
In shared/pipes/index.ts, add:
  export * from './status-ar.pipe';
```

**Dependencies:** None

**Risk:** ZERO — Additive change only.

**Estimated effort:** 5 minutes

**Expected result:** StatusArPipe available via barrel import.

**Validation steps:**
1. Build: passes
2. Any file importing `StatusArPipe` from `@shared/pipes` works

---

#### Fix 1.7 — [Q5] Extend BaseStateService in domain state services

**Issue:** `PatientStateService`, `SessionStateService`, `ExerciseStateService`, `ReportStateService` duplicate signal patterns instead of extending `BaseStateService<T>`.

**Affected files:**
- `core/state/patient-state.service.ts` — Extend BaseStateService
- `core/state/session-state.service.ts` — Extend BaseStateService
- `core/state/exercise-state.service.ts` — Extend BaseStateService
- `core/state/report-state.service.ts` — Extend BaseStateService
- `core/state/dashboard-state.service.ts` — Keep as-is (different pattern with cache)

**Root cause:** State services were written independently before BaseStateService was created.

**Required fix:**
```
For each state service (patient, session, exercise, report):

1. Import BaseStateService
2. Extend it: export class PatientStateService extends BaseStateService<Patient[]>
3. Remove duplicate dataSignal, loadingSignal, errorSignal
4. Remove duplicate asReadonly() declarations
5. Use this.setData(), this.startLoading(), this.stopLoading(), this.setError()
6. Keep domain-specific signals (selectedPatient, patientCount) as additional signals
7. Keep domain-specific methods (addPatient, updatePatient, removePatient, selectPatient)

Example PatientStateService:
  extends BaseStateService<Patient[]> {
    private patientsSignal = signal<Patient[]>([]);
    readonly patients = this.patientsSignal.asReadonly();
    readonly patientCount = computed(() => this.patientsSignal().length);
    private selectedPatientSignal = signal<Patient | null>(null);
    readonly selectedPatient = this.selectedPatientSignal.asReadonly();
    
    // Use this.setData() instead of this.dataSignal.set()
    // Use this.startLoading() instead of this.loadingSignal.set(true)
  }
```

**Dependencies:** A1 (BaseStateService.handleObservable must be fixed first)

**Risk:** MEDIUM — Touches 4 state services. Each must be tested individually.

**Estimated effort:** 1 hour

**Expected result:** Consistent state architecture; reduced code duplication.

**Validation steps:**
1. Build: passes
2. Tests: all state service tests pass
3. Patient list loads correctly
4. Session list loads correctly
5. Exercise list loads correctly
6. Report list loads correctly

---

### Sprint 2 — High-Value Medium Fixes (Issues: 8, Effort: ~5 hours)

**Goal:** Fix layout, forms, i18n, and API contract issues.

---

#### Fix 2.1 — [R2] Remove nested layout wrappers from app.html

**Issue:** `app.html` has `<header>`, `<main>`, `<footer>` wrappers, AND `MainLayoutComponent` also has `<app-header>`, `<main>`, `<app-footer>`.

**Affected files:**
- `src/app/app.html` — Remove header, main, footer wrappers

**Root cause:** Root template was written before MainLayoutComponent existed.

**Required fix:**
```
In app.html, change from:
  <header role="banner">...</header>
  <main id="main-content" role="main">
    <router-outlet />
  </main>
  <footer role="contentinfo">...</footer>

To:
  <router-outlet />
```

Keep the `<app-toast-container>` and the `<h1 class="visually-hidden">` for accessibility.

**Dependencies:** None

**Risk:** LOW — MainLayoutComponent already provides the full layout structure.

**Estimated effort:** 15 minutes

**Expected result:** No nested `<main>` or `<header>` elements. Clean DOM.

**Validation steps:**
1. Build: passes
2. All pages render correctly (check dashboard, patients, sessions)
3. Inspect DOM: no nested `<main>` tags
4. Browser DevTools → Elements → verify single `<main>` per route

---

#### Fix 2.2 — [U1] Remove ToastComponent dead code

**Issue:** `ToastComponent.show()` method and `isShowing` property are never called. Toast renders unconditionally.

**Affected files:**
- `shared/components/toast/toast.component.ts` — Remove `show()`, `isShowing`, `ngOnDestroy` timer logic (simplification)

**Root cause:** Toast was designed with show/hide lifecycle but implemented as always-rendered.

**Required fix:**
```
In toast.component.ts:
1. Remove: isShowing = false
2. Remove: show() method
3. Remove: ngOnDestroy() (if only clearing dismissTimer)
4. Simplify: The component just renders and auto-dismisses via closed.emit()
5. Keep: close(), onMouseEnter(), onMouseLeave(), dismissTimer logic
```

**Dependencies:** None

**Risk:** LOW — Toast still functions; just removing dead code.

**Estimated effort:** 15 minutes

**Expected result:** Cleaner component; no dead code.

**Validation steps:**
1. Build: passes
2. Trigger a toast (e.g., successful login) → appears and auto-dismisses
3. Hover over toast → pauses auto-dismiss

---

#### Fix 2.3 — [U2] Translate DateAgoPipe to Arabic

**Issue:** `DateAgoPipe` outputs English ("just now", "5m ago", "2d ago") in an Arabic-first app.

**Affected files:**
- `shared/pipes/date-ago.pipe.ts` — Replace English strings with Arabic

**Required fix:**
```
Replace the return strings:
  diff < 60:  "الآن" (instead of "just now")
  diff < 3600: `${minutes} د` (instead of "5m ago")
  diff < 86400: `${hours} س` (instead of "2h ago")
  diff < 604800: `${days} ي` (instead of "2d ago")
  diff < 2592000: `${weeks} ش` (instead of "1w ago")
  diff < 31536000: `${months} ع` (instead of "3mo ago")
  else: `${years} سنة` (instead of "1y ago")
```

**Dependencies:** None

**Risk:** ZERO — Pure text change.

**Estimated effort:** 10 minutes

**Expected result:** All relative dates display in Arabic.

**Validation steps:**
1. Build: passes
2. Chat list shows "الآن" for recent messages
3. Notification dropdown shows Arabic relative times

---

#### Fix 2.4 — [F1] Make password-match-validator flexible

**Issue:** Validator only checks `password`/`confirmPassword`. ProfileComponent uses `newPassword`/`confirmPassword`.

**Affected files:**
- `shared/validators/password-match.validator.ts` — Accept configurable field names

**Root cause:** Hardcoded field names don't work for all forms.

**Required fix:**
```
In password-match.validator.ts:
1. Create factory function:
   export function passwordMatchValidator(
     passwordField = 'password',
     confirmField = 'confirmPassword'
   ): ValidatorFn {
     return (control: AbstractControl): ValidationErrors | null => {
       const pw = control.get(passwordField);
       const confirm = control.get(confirmField);
       if (pw && confirm && pw.value !== confirm.value) {
         return { passwordMismatch: true };
       }
       return null;
     };
   }
2. Keep backward compatibility: default params = 'password'/'confirmPassword'
```

**Dependencies:** None

**Risk:** LOW — Default parameters maintain backward compatibility.

**Estimated effort:** 15 minutes

**Expected result:** All 3 forms (Register, ResetPassword, Profile) use correct field names.

**Validation steps:**
1. Build: passes
2. Register: mismatch passwords → shows error
3. Reset password: mismatch → shows error
4. Profile change password: mismatch → shows error
5. Tests: existing validator tests pass

---

#### Fix 2.5 — [Q3] Align RegisterRequest with backend DTO

**Issue:** `RegisterRequest` has `firstName/lastName/email/password/role`. CLAUDE.md says `RegisterDto` was updated with `fullName/licenseNumber`.

**Affected files:**
- `core/models/auth.model.ts` — Verify/update RegisterRequest
- `features/auth/pages/register/register.component.ts` — Update form fields
- `features/auth/pages/register/register.component.html` — Update form template

**Root cause:** Backend DTO may have been updated but frontend model wasn't synced.

**Required fix:**
```
Step 1: Verify backend RegisterDto fields
  - If backend uses: firstName, lastName, email, password, role → no change needed
  - If backend uses: fullName, licenseNumber, email, password → update model

Step 2: If update needed:
  a. In auth.model.ts, update RegisterRequest interface
  b. In register.component.ts, update form fields
  c. In register.component.html, update form inputs
  d. In auth.service.ts register() method, update the call
```

**Dependencies:** F1 (validator must be flexible first)

**Risk:** MEDIUM — If model doesn't match backend, registration fails silently.

**Estimated effort:** 30 minutes

**Expected result:** Frontend model matches backend DTO exactly.

**Validation steps:**
1. Build: passes
2. Register a new Therapist → succeeds
3. Register a new Patient → succeeds
4. Network tab: request body matches backend expected format

---

#### Fix 2.6 — [A3] Replace deprecated .toPromise() with firstValueFrom

**Issue:** `ChatRoomComponent.sendMessage` uses `.toPromise()` which is deprecated in RxJS 7.

**Affected files:**
- `features/chatbot/pages/chat-room/chat-room.component.ts` — Replace `.toPromise()` with `firstValueFrom()`

**Root cause:** Original code used deprecated API.

**Required fix:**
```
In chat-room.component.ts:
1. Add import: import { firstValueFrom } from 'rxjs';
2. Replace:
   await this.http.post<void>(API.chat.send, {...}).toPromise();
   With:
   await firstValueFrom(this.http.post<void>(API.chat.send, {...}));
```

**Dependencies:** R1 (error handling should be clean first, though not strictly required)

**Risk:** LOW — Direct API replacement.

**Estimated effort:** 10 minutes

**Expected result:** No deprecated API warnings; modern async pattern.

**Validation steps:**
1. Build: passes
2. Send a message in chat room → message sends successfully
3. Network tab: POST /api/chat/send succeeds

---

#### Fix 2.7 — [U5] Fix SelectComponent selected state comparison

**Issue:** `select.component.html` uses `| json` pipe for array comparison in `[selected]` attribute, which is unreliable.

**Affected files:**
- `shared/components/select/select.component.html` — Fix selected comparison

**Required fix:**
```
In select.component.html:
Replace:
  [selected]="multiple ? (value | json) : value === option.value"
With:
  [attr.selected]="multiple ? (value.includes(option.value)) : value === option.value"
Or use ngSelected directive approach.
```

**Dependencies:** None

**Risk:** LOW — Fixing incorrect comparison logic.

**Estimated effort:** 15 minutes

**Expected result:** Multi-select correctly shows selected options.

**Validation steps:**
1. Build: passes
2. Any multi-select component shows correct initial selections
3. Single-select still works correctly

---

#### Fix 2.8 — [A2] Fix patient list pagination to be server-side

**Issue:** `patient-list.ts` sets `totalItems = result.length` (current page), not total from API.

**Affected files:**
- `features/patients/pages/patient-list/patient-list.ts` — Handle server-side pagination response

**Root cause:** Frontend assumes API returns all results; API may return paginated data.

**Required fix:**
```
Step 1: Check if backend returns { data: Patient[], totalCount: number }
  - If yes: update PatientService.getPatients() to return Observable<{data: Patient[], totalCount: number}>
  - If no: current approach is correct (client-side pagination)

Step 2: If server-side:
  a. Update core/services/patient.service.ts getPatients() return type
  b. Update patient-list.ts loadPatients() to use result.totalCount
  c. Update patient-list.ts onPageChange() to fetch new page from API
```

**Dependencies:** None

**Risk:** MEDIUM — Depends on backend API behavior. Must verify backend response shape first.

**Estimated effort:** 30 minutes

**Expected result:** Pagination shows correct total count across all pages.

**Validation steps:**
1. Build: passes
2. Patient list shows "عرض 1 إلى 10 من أصل 45 سجل" (correct total)
3. Page 2 loads next 10 records from API
4. Search updates total count correctly

---

### Sprint 3 — Low Priority & Cleanup (Issues: 12, Effort: ~3 hours)

**Goal:** Code cleanup, accessibility, performance, and documentation.

---

#### Fix 3.1 — [P3] Add debounce to chart recreation

**Issue:** `LineChartComponent` and `BarChartComponent` recreate Chart instance on every `ngOnChanges` without debouncing.

**Affected files:**
- `shared/components/line-chart/line-chart.component.ts` — Add debounce
- `shared/components/bar-chart/bar-chart.component.ts` — Add debounce

**Required fix:**
```
In both chart components:
1. Add: private renderSubject = new Subject<void>();
2. In ngOnInit, subscribe with debounce:
   this.renderSubject.pipe(
     debounceTime(100),
     takeUntilDestroyed()
   ).subscribe(() => this.renderChart());
3. In ngOnChanges, call this.renderSubject.next() instead of this.renderChart()
```

**Dependencies:** P2 (chart dedup must happen first)

**Risk:** LOW — Adds 100ms delay, imperceptible to users.

**Estimated effort:** 20 minutes

**Expected result:** Charts don't thrash on rapid input changes.

**Validation steps:**
1. Build: passes
2. Dashboard loads → charts render smoothly
3. Rapid window resize → charts don't flicker

---

#### Fix 3.2 — [F2] Change DOB field type to date

**Issue:** Patient form dateOfBirth field uses default text input type.

**Affected files:**
- `features/patients/pages/patient-form/patient-form.html` — Add `type="date"` to DOB input

**Required fix:**
```
In patient-form.html, find the dateOfBirth app-input:
Change from (no type specified, defaults to text):
  <app-input formControlName="dateOfBirth" ...>
To:
  <app-input type="date" formControlName="dateOfBirth" ...>
```

**Dependencies:** None

**Risk:** ZERO — HTML5 date picker.

**Estimated effort:** 5 minutes

**Expected result:** DOB field shows native date picker.

**Validation steps:**
1. Build: passes
2. Patient form shows date picker for DOB field

---

#### Fix 3.3 — [F3] Verify Quill config binding

**Issue:** `SessionForm` defines `quillConfig` but binding to QuillEditorComponent is unverified.

**Affected files:**
- `features/sessions/pages/session-form/session-form.html` — Verify `[modules]` binding

**Required fix:**
```
In session-form.html, verify QuillEditorComponent has:
  [modules]="quillConfig"
If missing, add it.
```

**Dependencies:** None

**Risk:** LOW — Configuration may already be bound via different mechanism.

**Estimated effort:** 10 minutes

**Expected result:** Quill editor shows toolbar with configured options.

**Validation steps:**
1. Build: passes
2. Session form → Quill editor shows toolbar with bold, italic, lists, etc.

---

#### Fix 3.4 — [A4] Type register() return value

**Issue:** `AuthService.register()` returns `Observable<unknown>`.

**Affected files:**
- `core/services/auth.service.ts` — Change return type

**Required fix:**
```
In auth.service.ts:
1. Create RegisterResponse interface (or use existing if backend returns one):
   interface RegisterResponse { message: string; }
2. Change register() return type:
   register(userData: RegisterRequest): Observable<RegisterResponse>
```

**Dependencies:** None

**Risk:** ZERO — Type-only change.

**Estimated effort:** 5 minutes

**Expected result:** Type-safe register call.

**Validation steps:**
1. Build: passes
2. TypeScript no longer shows `unknown` type on register subscription

---

#### Fix 3.5 — [A5] Fix notification subscription leak

**Issue:** `InAppNotificationService.load()` subscribes without tracking for cleanup.

**Affected files:**
- `core/services/in-app-notification.service.ts` — Store subscription reference

**Required fix:**
```
In in-app-notification.service.ts:
1. Add: private loadSubscription: Subscription | null = null;
2. In load(): store the subscription:
   this.loadSubscription = this.http.get(...).subscribe({...});
3. In stopPolling(): unsubscribe:
   this.loadSubscription?.unsubscribe();
   this.loadSubscription = null;
```

**Dependencies:** None

**Risk:** LOW — Adds cleanup.

**Estimated effort:** 10 minutes

**Expected result:** No subscription leak when polling stops.

**Validation steps:**
1. Build: passes
2. Header notification bell loads → stops on logout

---

#### Fix 3.6 — [P4] Pause notification polling when tab hidden

**Issue:** `InAppNotificationService` polls every 30s even when tab is hidden.

**Affected files:**
- `core/services/in-app-notification.service.ts` — Add visibility check

**Required fix:**
```
In in-app-notification.service.ts:
1. In startPolling(), use document.visibilitychange:
   document.addEventListener('visibilitychange', () => {
     if (document.hidden) {
       this.stopPolling();
     } else {
       this.startPolling();
     }
   });
2. Or use setInterval with visibility check:
   this.pollInterval = setInterval(() => {
     if (!document.hidden) this.load();
   }, intervalMs);
```

**Dependencies:** A5 (subscription cleanup should happen first)

**Risk:** LOW — Reduces unnecessary network calls.

**Estimated effort:** 15 minutes

**Expected result:** No polling when tab is backgrounded.

**Validation steps:**
1. Build: passes
2. Open app → switch to another tab → check Network tab → no polling requests
3. Switch back → polling resumes

---

#### Fix 3.7 — [P5] Optimize ClickOutsideDirective

**Issue:** Each `ClickOutsideDirective` instance adds a `document:click` listener.

**Affected files:**
- `shared/directives/click-outside/click-outside.directive.ts` — Use shared listener pattern

**Required fix:**
```
Replace HostListener approach with:
1. Use ElementRef and a shared document click handler
2. Or use a service that manages a single document listener
3. Minimal fix: keep current approach but add OnDestroy cleanup
```

**Dependencies:** None

**Risk:** LOW — Only 2 usages (header dropdowns).

**Estimated effort:** 15 minutes

**Expected result:** Cleaner directive implementation.

**Validation steps:**
1. Build: passes
2. Header dropdown opens/closes correctly
3. Notification dropdown opens/closes correctly

---

#### Fix 3.8 — [U3] Fix footer placeholder links

**Issue:** Footer has `href="#"` links for "سياسة الخصوصية" and "شروط الخدمة".

**Affected files:**
- `shared/layouts/footer/footer.component.html` — Either remove or link to actual pages

**Required fix:**
```
Option A (remove dead links):
  Remove the footer-links div entirely.

Option B (keep but mark as coming soon):
  Change href="#" to javascript:void(0) and add a tooltip "قريباً"
```

**Dependencies:** None

**Risk:** ZERO

**Estimated effort:** 5 minutes

**Expected result:** No broken navigation links.

**Validation steps:**
1. Build: passes
2. Footer either has no placeholder links or they show "coming soon"

---

#### Fix 3.9 — [U4] Add skip-to-content link

**Issue:** `app.html` doesn't have a skip-to-content link for screen readers.

**Affected files:**
- `src/app/app.html` — Add skip link

**Required fix:**
```
In app.html, add before <router-outlet>:
  <a class="skip-link" href="#main-content">تخطي للمحتوى الرئيسي</a>
```

**Dependencies:** R2 (app.html layout should be fixed first)

**Risk:** ZERO — Additive accessibility improvement.

**Estimated effort:** 5 minutes

**Expected result:** Screen reader users can skip navigation.

**Validation steps:**
1. Build: passes
2. Tab through page → skip link appears as first focusable element

---

#### Fix 3.10 — [Q6] Remove unused RoleDirective

**Issue:** `RoleDirective` exists but is never used in any template.

**Affected files:**
- `shared/directives/role/role.directive.ts` — Remove file
- `shared/directives/index.ts` — Remove barrel export

**Required fix:**
```
1. Delete shared/directives/role/role.directive.ts
2. Delete shared/directives/role/index.ts
3. In shared/directives/index.ts, remove: export * from './role';
```

**Dependencies:** None

**Risk:** ZERO — Dead code removal.

**Estimated effort:** 5 minutes

**Expected result:** No unused directive in codebase.

**Validation steps:**
1. Build: passes
2. `grep -r "appRole" src/` returns no results

---

#### Fix 3.11 — [Q7] Remove unused TruncatePipe

**Issue:** `TruncatePipe` exists but is never used in any template.

**Affected files:**
- `shared/pipes/truncate.pipe.ts` — Remove file
- `shared/pipes/index.ts` — Remove barrel export

**Required fix:**
```
1. Delete shared/pipes/truncate.pipe.ts
2. In shared/pipes/index.ts, remove: export * from './truncate.pipe';
```

**Dependencies:** None

**Risk:** ZERO — Dead code removal.

**Estimated effort:** 5 minutes

**Expected result:** No unused pipe in codebase.

**Validation steps:**
1. Build: passes
2. `grep -r "truncate" src/app/ --include="*.html"` returns no results

---

#### Fix 3.12 — [Doc] Fix CLAUDE.md enableMockApi discrepancy

**Issue:** CLAUDE.md says `enableMockApi: true` in dev, but `environment.ts` has `false`.

**Affected files:**
- `CLAUDE.md` — Update the documentation

**Required fix:**
```
In CLAUDE.md, find the Environment Config section:
Change: enableMockApi: true
To: enableMockApi: false
```

**Dependencies:** None

**Risk:** ZERO — Documentation only.

**Estimated effort:** 5 minutes

**Expected result:** Documentation matches code.

**Validation steps:**
1. Read CLAUDE.md → verify `enableMockApi: false` for development

---

## PHASE 5 — RUNTIME VALIDATION PLAN

### Sprint 1 Validation Checklist

| Fix | Manual Test | Expected Result | API Check | UI Check | Console Check |
|-----|-------------|-----------------|-----------|----------|---------------|
| R1 | Trigger 401 error (wrong password) | Single error toast | — | 1 toast, not 2 | No duplicate error logs |
| R1 | Trigger 404 (invalid endpoint) | Single error toast | — | 1 toast | No duplicate logs |
| A1 | No behavioral change yet | — | — | — | — |
| Q1/Q2 | Chat list loads | Conversations display | GET /api/chat/conversations → 200 | List renders | No type errors |
| Q1/Q2 | Chat room loads | Messages display | GET /api/chat/{id}/history → 200 | Messages render | No type errors |
| P2 | Dashboard loads | Charts render | — | Line + bar charts visible | No Chart.js warnings |
| P1 | Production build | Bundle under 900 kB | — | — | No budget warnings |
| Q4 | Import StatusArPipe | Works from barrel | — | — | No compile errors |
| Q5 | Patient list loads | Data renders | GET /api/patient → 200 | Table shows patients | No errors |
| Q5 | Session list loads | Data renders | GET /api/sessions/patient/{id} → 200 | Table shows sessions | No errors |

### Sprint 2 Validation Checklist

| Fix | Manual Test | Expected Result | API Check | UI Check | Console Check |
|-----|-------------|-----------------|-----------|----------|---------------|
| R2 | Any page loads | No nested `<main>` | — | DOM: single `<main>` | No errors |
| U1 | Trigger toast | Appears and auto-dismisses | — | Toast visible | No dead code warnings |
| U2 | Chat list | "الآن" for recent | — | Arabic relative times | — |
| F1 | Register mismatch | Shows error | — | Error message in Arabic | — |
| F1 | Profile mismatch | Shows error | — | Error message in Arabic | — |
| Q3 | Register new user | Succeeds | POST /api/auth/register → 200 | Redirects to login | — |
| A3 | Send chat message | Sends successfully | POST /api/chat/send → 200 | Message appears | No deprecation warnings |
| U5 | Multi-select | Correct options selected | — | Visual check | — |
| A2 | Patient list page 2 | Shows next 10 | GET /api/patient?page=2 → 200 | Correct pagination | — |

### Sprint 3 Validation Checklist

| Fix | Manual Test | Expected Result | API Check | UI Check | Console Check |
|-----|-------------|-----------------|-----------|----------|---------------|
| P3 | Rapid resize | Charts don't flicker | — | Smooth rendering | — |
| F2 | Patient form DOB | Date picker shows | — | Native date input | — |
| F3 | Session form | Quill toolbar visible | — | Bold/italic/lists buttons | — |
| A5 | Logout | No lingering requests | — | — | No leaked subscriptions |
| P4 | Background tab | No polling requests | Network tab idle | — | — |
| U4 | Tab through page | Skip link first | — | "تخطي للمحتوى" visible | — |
| Q6 | Build | No unused directive | — | — | — |
| Q7 | Build | No unused pipe | — | — | — |

---

## PHASE 6 — RISK MAP

| Issue | Break Probability | Impact | Mitigation |
|-------|------------------|---------|------------|
| R1 (error dedup) | LOW | HIGH (affects all HTTP) | Test every error code (401, 403, 404, 500) |
| A1 (BaseState callback) | LOW | LOW (unused code) | No behavioral change; just enabling future use |
| P1 (bundle size) | MEDIUM | MEDIUM (performance) | Measure before/after; verify lazy chunks unaffected |
| Q1/Q2 (chat interfaces) | LOW | LOW (pure refactor) | Verify chat list + chat room load |
| R2 (nested HTML) | LOW | MEDIUM (layout) | Check every page renders correctly |
| Q5 (state services) | MEDIUM | HIGH (state management) | Test each domain feature individually |
| F1 (password validator) | LOW | HIGH (auth flow) | Test register, reset, profile change password |
| Q3 (register model) | MEDIUM | HIGH (registration) | Test actual registration against backend |
| A3 (toPromise) | LOW | LOW (deprecated API) | Test chat message send |
| A2 (pagination) | MEDIUM | MEDIUM (UX) | Verify backend response shape first |
| P3 (chart debounce) | LOW | LOW (performance) | Visual check only |
| U5 (select fix) | LOW | LOW (component) | Test any select component |

---

## PHASE 7 — FINAL EXECUTION DASHBOARD

### Task Counts

| Priority | Count | Files Modified |
|----------|-------|----------------|
| Critical | 0 | — |
| High | 4 | 9 files |
| Medium | 12 | 16 files |
| Low | 11 | 14 files |
| **Total** | **27** | **~39 files** |

### Estimated Total Effort

| Sprint | Effort | Focus |
|--------|--------|-------|
| Sprint 1 | ~4 hours | Foundation: error handling, state, charts, bundle |
| Sprint 2 | ~5 hours | High-value: layout, forms, i18n, API contract |
| Sprint 3 | ~3 hours | Cleanup: performance, accessibility, dead code |
| **Total** | **~12 hours** | — |

### High Risk Tasks (require careful testing)
1. **Q5** — Extending BaseStateService in 4 state services (breaks state if wrong)
2. **Q3** — RegisterRequest model alignment (breaks registration if wrong)
3. **A2** — Server-side pagination (breaks patient list if backend shape differs)
4. **P1** — Bundle size reduction (may require structural changes)

### Blocking Tasks
None. All fixes are independent or have clear dependency chains that don't create circular blocks.

### Suggested Sprint Breakdown

**Sprint 1 (Foundation — 4 hours)**
- R1: Remove duplicate error handling
- A1: Fix BaseStateService callback
- Q1/Q2: Consolidate chat interfaces
- P2: Deduplicate chart registration
- P1: Bundle size optimization
- Q4: Export StatusArPipe
- Q5: Extend BaseStateService in domain services

**Sprint 2 (High-Value — 5 hours)**
- R2: Remove nested layout wrappers
- U1: Remove ToastComponent dead code
- U2: Translate DateAgoPipe to Arabic
- F1: Make password validator flexible
- Q3: Align RegisterRequest with backend
- A3: Replace deprecated .toPromise()
- U5: Fix SelectComponent selected state
- A2: Fix patient list pagination

**Sprint 3 (Cleanup — 3 hours)**
- P3: Add chart debounce
- F2: DOB field type
- F3: Quill config verification
- A4: Type register() return
- A5: Fix notification subscription leak
- P4: Pause polling on tab hide
- P5: Optimize ClickOutsideDirective
- U3: Fix footer links
- U4: Add skip-to-content link
- Q6: Remove unused RoleDirective
- Q7: Remove unused TruncatePipe
- Doc: Fix CLAUDE.md discrepancy

---

**This execution map specifies exact files, exact dependencies, exact fix descriptions, and exact validation steps for all 27 issues. No file should be modified until this plan is approved.**