# Jalsa Frontend Defense Guide

Angular 21.2 standalone-component frontend for the Jalsa clinic management system. Prepared for the capstone defense — team lead / Frontend owner briefing.

---

## 1. Architecture

### 1.1 Core / Shared / Features split

The frontend at `frontend/src/app` is organized into three top-level folders, each with a distinct role:

- **`core/`** — singletons that exist once per app: the 21 injectable services (auth, patient, session, exercise, dashboard, report, notification, loading, app-state, navigation, in-app-notification, admin, ai, password-reset-state, and the entity-scoped domain services), the 13 signal-based state services, the HTTP gateway (`core/api/http-client.service.ts`), the 4 interceptors, and the 2 guards. Nothing here renders UI. Everything here is `providedIn: 'root'`, so Angular's DI tree-shakes and singleton-izes it automatically without needing a `CoreModule` import-once pattern (the classic Angular anti-pattern this structure avoids).
- **`shared/`** — dumb, reusable, presentational building blocks: the 13+ shared components (buttons, inputs, table, modal, toast, charts…), the 5 layout shells, directives, pipes, and validators. Nothing here knows about business logic or calls a domain service — it only takes `@Input()`/`input()` and emits `@Output()`/events.
- **`features/`** — the 7 lazy-loaded business modules (auth, patients, sessions, exercises, reports, dashboard, chatbot), each owning its own pages, its own routes file, and consuming `core` services + `shared` components.

**Why this split exists:** it enforces a strict dependency direction — `features` depends on `core` and `shared`, but `core` and `shared` never depend on `features`. That means core services and shared components can be tested and reasoned about in total isolation, feature modules can be lazy-loaded independently without pulling in unrelated business logic, and a reviewer (or a panel) can answer "where does X live" from the folder name alone: state and data access → `core`, visual primitives → `shared`, page-level business logic → `features`.

### 1.2 `app.config.ts` — every provider

```typescript
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        authInterceptor,
        errorInterceptor,
        refreshTokenInterceptor,
        loadingInterceptor,
      ])
    ),
    provideAnimations(),
    provideAppInitializer(() => {
      inject(AppStateService);
      return inject(AuthService).initializeAuth();
    }),
    { provide: ErrorHandler, useValue: Sentry.createErrorHandler({ showDialog: false }) },
    { provide: Sentry.TraceService, deps: [Router] },
  ],
};
```

| Provider | Purpose |
|---|---|
| `provideZoneChangeDetection({ eventCoalescing: true })` | Batches multiple DOM events firing in the same tick into a single change-detection pass — reduces redundant CD cycles even before Signals kick in. |
| `provideRouter(routes)` | Registers the root `Routes` array from `app.routes.ts`. |
| `provideHttpClient(withInterceptors([...]))` | Registers the functional interceptor chain, in explicit array order (order matters — see §4). |
| `provideAnimations()` | Enables Angular's animation engine for component transitions (modals, toasts, route transitions). |
| `provideAppInitializer(...)` | Runs **before the router activates the first route** — injects `AppStateService` (hydrates theme/sidebar from localStorage) and calls `AuthService.initializeAuth()`, which restores the user's session (roles, profile) from the stored JWT/refresh token before any guard evaluates `isAuthenticated()`. This is why `authGuard` never races against an unhydrated auth state on page refresh. |
| `ErrorHandler` → Sentry | Global uncaught-error handler wired to Sentry, without the intrusive "report this error" browser dialog. |
| `Sentry.TraceService` | Hooks Sentry performance tracing into `Router` navigation events. |

**Interceptor registration order is significant** — `withInterceptors([auth, error, refreshToken, loading])` runs in array order on the outbound leg and reverse order on the inbound (response) leg, i.e. `authInterceptor` attaches the token first, `loadingInterceptor`'s `finalize()` is the *last* thing to fire when a response returns. See §4 for the full request/response trace.

### 1.3 `app.routes.ts` — lazy routes and guards

```typescript
export const routes: Routes = [
  { path: 'auth', loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES) },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', loadChildren: () => import('./features/dashboard/dashboard.routes')... },
      { path: 'patients', loadChildren: () => import('./features/patients/patients.routes')... },
      { path: 'sessions', loadChildren: () => import('./features/sessions/sessions.routes')... },
      { path: 'exercises', loadChildren: () => import('./features/exercises/exercises.routes')... },
      { path: 'reports', loadChildren: () => import('./features/reports/reports.routes')... },
      { path: 'chatbot', loadChildren: () => import('./features/chatbot/chatbot.routes')... },
      { path: 'notifications', loadChildren: () => ... },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    ],
  },
  { path: 'forbidden', loadComponent: () => import('./features/auth/pages/forbidden/forbidden.component').then(m => m.ForbiddenComponent) },
  { path: '**', redirectTo: 'auth/login' },
];
```

Every feature under `''` is behind `loadChildren`/`loadComponent`, so its JS chunk is only downloaded on first navigation into it — the initial bundle only contains the app shell, router, and `MainLayoutComponent`.

**Two-tier guarding:**
1. `authGuard` sits once, at the `MainLayoutComponent` parent route — every authenticated feature inherits it, so a logged-out user is bounced to `/auth/login?returnUrl=<attempted>` before Angular even lazy-loads the feature chunk.
2. `roleGuard(roles: string[])` is applied **per feature route**, e.g. dashboard has two variants:
   ```typescript
   { path: '', canActivate: [authGuard, roleGuard(['Therapist'])], loadComponent: () => DashboardComponent },
   { path: 'patient', canActivate: [authGuard, roleGuard(['Patient'])], loadComponent: () => PatientDashboardComponent },
   ```
   This lets a single route segment (`/dashboard`) branch to a completely different page per role without a runtime `*ngIf` inside one giant component.

**Guard implementations:**

```typescript
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.isAuthenticated()) return true;
  router.navigate(['/auth/login'], { queryParams: { returnUrl: router.url } });
  return false;
};

export const roleGuard = (allowedRoles: string[]): CanActivateFn => () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (allowedRoles.some(role => authService.hasRole(role))) return true;
  router.navigate(['/forbidden']);
  return false;
};
```

The **Patient-role special case is not in the guard** — `roleGuard` always redirects to `/forbidden` on failure. The special-casing lives at the call sites that need to send a Patient somewhere useful *after* a successful login or after landing on `/forbidden`:

- `login.component.ts`, on successful auth:
  ```typescript
  const roles = this.authService.currentUser()?.roles ?? [];
  let target = this.returnUrl;
  if (roles.includes('Patient')) target = '/dashboard/patient';
  else if (roles.includes('Admin') && target === '/dashboard') target = '/admin/dashboard';
  this.router.navigateByUrl(target);
  ```
- `ForbiddenComponent.goHome()`:
  ```typescript
  goHome(): void {
    const roles = this.authService.currentUser()?.roles ?? [];
    if (roles.includes('Patient')) this.router.navigate(['/dashboard/patient']);
    else if (roles.includes('Admin')) this.router.navigate(['/admin/dashboard']);
    else this.router.navigate(['/dashboard']);
  }
  ```

This is a deliberate separation of concerns: **the guard's job is binary access control** (can this role see this route, yes/no); **the redirect-to-somewhere-useful logic is a UX decision that belongs to the page**, not the security boundary. Mixing them would mean every future role added to the system requires editing the guard itself instead of just the calling component.

---

## 2. State Management

### 2.1 Why hand-rolled signals instead of NgRx

Every domain gets its own `@Injectable({ providedIn: 'root' })` state service following the same shape: private writable `signal()`s, public `asReadonly()` views, and `computed()` derivations. There is **no shared base class** — each of the 13 state services (`PatientStateService`, `SessionStateService`, `ExerciseStateService`, `ReportStateService`, `DashboardStateService`, `PatientDashboardStateService`, `PatientProgressStateService`, `PatientAssessmentStateService`, `PatientSessionStateService`, and 4 admin state services) repeats the pattern independently:

```typescript
@Injectable({ providedIn: 'root' })
export class PatientStateService {
  private patientsSignal = signal<Patient[]>([]);
  private selectedPatientSignal = signal<Patient | null>(null);
  private loadingSignal = signal(false);
  private errorSignal = signal<string | null>(null);

  readonly patients = this.patientsSignal.asReadonly();
  readonly selectedPatient = this.selectedPatientSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();
  readonly patientCount = computed(() => this.patientsSignal().length);

  setPatients(patients: Patient[]): void { this.patientsSignal.set(patients); this.errorSignal.set(null); }
  addPatient(p: Patient): void { this.patientsSignal.update(list => [...list, p]); }
  updatePatient(p: Patient): void { this.patientsSignal.update(list => list.map(x => x.id === p.id ? p : x)); }
  removePatient(id: string): void { this.patientsSignal.update(list => list.filter(x => x.id !== id)); }
  selectPatient(p: Patient | null): void { this.selectedPatientSignal.set(p); }
  setLoading(v: boolean): void { this.loadingSignal.set(v); }
  setError(e: string | null): void { this.errorSignal.set(e); }
  reset(): void { this.patientsSignal.set([]); this.selectedPatientSignal.set(null); this.errorSignal.set(null); }
}
```

The list-focused services all follow `list + selectedItem + loading + error + computed count`; the dashboard-style services (`DashboardStateService`, `PatientDashboardStateService`) follow `summary + loading + error + lastLoadedAt`, with a `computed()` `isStale` flag against a 5-minute TTL instead of a `count`.

**Reasoning to defend (why not NgRx):**
- The app has no cross-cutting state that needs time-travel debugging, action replay, or a single global store — each domain's state is genuinely local to its feature and consumed by 2-4 components at most.
- Signals give the same benefits NgRx exists for (single source of truth, immutable updates, reactive derivation via `computed()`) with roughly a tenth of the boilerplate (no actions, reducers, effects, selectors, or `createFeature` ceremony) and with native Angular integration — no `async` pipe, no `Store` DI token, no `ofType()` RxJS operators.
- Angular 21's own direction (Signals as first-class reactive primitive, `input()`/`output()` moving off decorators) makes hand-rolled signal services the more "idiomatic Angular 21" choice than bringing in a third-party global-store library for a 5-person capstone team on a fixed timeline.

### 2.2 State service by state service

| Service | Signals | `computed()` | Primary consumers |
|---|---|---|---|
| `PatientStateService` | `patients`, `selectedPatient`, `loading`, `error` | `patientCount` | `patient-list`, `patient-form`, `patient-detail` |
| `SessionStateService` | `sessions`, `selectedSession`, `loading`, `error` | `sessionCount` | `session-list`, `session-form`, `session-detail` |
| `ExerciseStateService` | `exercises`, `logs`, `loading`, `error` | `logsByExercise` (groups `ExerciseLog[]` by `exerciseId` into a `Map`) | `exercise-list`, `assign-exercise`, `patient-exercise` |
| `ReportStateService` | `reports`, `selectedReport`, `loading`, `error` | `reportCount` | `report-list`, `report-generate`, `report-detail` |
| `DashboardStateService` | `summary`, `loading`, `error`, `lastLoadedAt` | `isStale` (5-min TTL) | `dashboard.component.ts` |
| `PatientDashboardStateService` | same shape, patient-scoped summary | `isStale` | `patient-dashboard.component.ts` |
| `PatientProgressStateService` | `progress`, `loading`, `error` | — | patient-facing progress page |
| `PatientAssessmentStateService` | `list`, `detail`, `loading`, `saving`, `error` (separates load vs. save states) | — | patient assessment fill-out flow |
| `PatientSessionStateService` | `sessions`, `selectedSession`, `loading`, `error` | — | patient appointment views |
| 4× Admin state services | entity list + `totalCount` + `loading` + `error` | — | admin panel pages |

### 2.3 Why components never call `HttpClientService` directly, and the one full data-flow trace

The rule exists so that (a) every domain has exactly one place that knows the REST shape, and (b) UI state lives in one signal-backed service that many components can share without prop-drilling or re-fetching. Concretely: `PatientService` (a stateless domain service) knows the endpoint URLs and DTO shapes; `PatientStateService` (a state service) holds the resulting signals; components inject both, call the domain service, and push the result into state.

**Full trace — loading the patient list on `/patients`:**

1. **Component** (`patient-list.ts`) — `ngOnInit()` calls `this.fetchPatients()`, which calls `this.patientService.getPatients({ page, pageSize, search })`.
2. **Domain service** (`core/services/patient.service.ts`) — builds the query params and calls `this.http.get<Patient[]>('/api/patient', params)` where `this.http` is the injected `HttpClientService`, not Angular's raw `HttpClient`.
3. **HTTP gateway** (`core/api/http-client.service.ts`) — prefixes `environment.apiUrl` and delegates to Angular's `HttpClient.get()`, returning an `Observable<T>`.
4. **Interceptor chain** (registered in `app.config.ts`, executed in this order on the outbound leg):
   - `authInterceptor` attaches `Authorization: Bearer <token>` (skips this for `/refresh`, since that call carries its own refresh token in the body, not a Bearer header).
   - `errorInterceptor` wraps the call in `catchError`, ready to map a failure to an Arabic toast.
   - `refreshTokenInterceptor` is the first to see a `401` on the way back and, if not already refreshing, calls `authService.refreshToken()`, retries the original request with the new token, and queues any other requests that 401 while a refresh is in flight.
   - `loadingInterceptor` calls `loadingService.show()` before the request and `loadingService.hide()` in `finalize()` regardless of outcome.
5. **Backend** — ASP.NET Core `PatientController` handles `GET /api/patient`, returns JSON.
6. **Back up the interceptor chain** (reverse order for the response) — `loadingInterceptor` hides the spinner, `refreshTokenInterceptor` passes a 2xx straight through, `errorInterceptor` passes a 2xx straight through (only maps failures), `authInterceptor` is a no-op on the response leg.
7. **Domain service** returns the typed `Patient[]` to the component.
8. **Component** calls `this.patientStateService.setPatients(result)`, which updates the private `patientsSignal` — every component that injects `PatientStateService` and reads `patients()` re-renders reactively (no manual subscription, no `async` pipe needed if the template reads the signal directly).

This chain is why the rule is enforced: skipping `HttpClientService` (as three of the chatbot components and `crisis-alerts-list.component.ts` currently do — see §10) means that call bypasses the single base-URL configuration point, though it still goes through the same interceptor chain since interceptors are registered against Angular's `HttpClient` itself, not against `HttpClientService`.

---

## 3. Core Services (21 total)

| Service | Responsibility |
|---|---|
| `AuthService` | JWT/refresh-token lifecycle, login/register/logout, `currentUser` signal, `hasRole()`/`hasAnyRole()`, profile CRUD, avatar upload, password change/reset/OTP flow. |
| `PatientService` | Patient CRUD, archive/restore, intake form save, assessment add/assign. Stateless — pushes results to `PatientStateService`. |
| `SessionService` | Session CRUD, session notes, AI summary fetch, patient reschedule/cancel request approve/reject. |
| `ExerciseService` | Exercise CRUD, due-date extension, patient completion logging, "my exercises"/"my logs" endpoints. |
| `DashboardService` | Therapist dashboard summary fetch. |
| `ReportService` | AI report generation, versioning, approve/reject, HTML export fetch. |
| `NotificationService` | In-memory toast queue — `show()/success()/error()/warning()/info()/dismiss()`, signal-backed `notifications` list, auto-dismiss timers. |
| `LoadingService` | Global spinner counter (`show()/hide()` reference-counted so overlapping requests don't flicker the spinner off early). |
| `AppStateService` | Theme (light/dark) and sidebar collapsed/expanded state, persisted to localStorage. |
| `NavigationService` | Role-filtered sidebar menu — `computed()` from `AuthService.currentUser().roles`. |
| `InAppNotificationService` | Server-backed notification bell — polling (`startPolling()/stopPolling()`) + SignalR push, `unreadCount` signal, mark-read/mark-all-read. |
| `AdminService` | Admin panel CRUD across users, doctors, patient accounts, audit logs, system settings. |
| `AiService` | Direct calls to `/api/ai/summarize/{patientId}` and `/api/ai/report-draft/{patientId}`. |
| `PasswordResetStateService` | Cross-page state (email + OTP) for the forgot→verify→reset password wizard, so the OTP page doesn't need the email re-typed. |
| `PatientDashboardService` | Patient-role dashboard summary fetch. |
| `PatientProgressService` | Patient-role progress metrics fetch. |
| `PatientAssessmentService` | Patient-role assessment list/detail/answer/submit. |
| `PatientSessionService` | Patient-role session list/detail/reschedule/cancel request. |
| `HttpClientService` | The single HTTP gateway — see below. |
| 2× guard functions (`authGuard`, `roleGuard`) | Not injectable services but co-located in `core/guards/`. |

### `http-client.service.ts` — the single HTTP gateway

```typescript
@Injectable({ providedIn: 'root' })
export class HttpClientService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  get<T>(endpoint: string, params?: HttpParams | Record<string, ...>): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}${endpoint}`, { params });
  }
  post<T>(endpoint: string, body: unknown, options?: { headers?: HttpHeaders }): Observable<T> { ... }
  put<T>(endpoint: string, body: unknown): Observable<T> { ... }
  patch<T>(endpoint: string, body: unknown): Observable<T> { ... }
  delete<T>(endpoint: string): Observable<T> { ... }
  upload<T>(endpoint: string, formData: FormData): Observable<T> { ... }
  blob(endpoint: string): Observable<Blob> { ... }
}
```

It exists purely to centralize `environment.apiUrl` prefixing and to give every domain service the same six HTTP verbs plus a multipart `upload()` and a `blob()` (used by the report HTML export). It is intentionally stateless and has zero error-handling logic — that responsibility is fully delegated to `errorInterceptor`, keeping the gateway a thin, boring pass-through.

---

## 4. Interceptors and Guards

### 4.1 `auth.interceptor.ts`

```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  if (req.url.includes('/refresh')) return next(req);
  const token = authService.getToken();
  if (token) {
    return next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
  }
  return next(req);
};
```

It skips `/refresh` because that endpoint authenticates with a **refresh token in the request body**, not a Bearer access token — attaching a (possibly expired) access token there would be meaningless and could even confuse the backend's refresh-token validation logic.

### 4.2 `refresh-token.interceptor.ts` — the rotation flow

```typescript
let isRefreshing = false;
const refreshedToken$ = new BehaviorSubject<string | null>(null);

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const isExempt = AUTH_EXEMPT_PATHS.some(p => req.url.includes(p)) || req.context.get(SKIP_REFRESH);
  if (isExempt) return next(req);

  return next(req).pipe(
    catchError(error => {
      if (!(error instanceof HttpErrorResponse) || error.status !== 401) return throwError(() => error);

      if (!isRefreshing) {
        isRefreshing = true;
        refreshedToken$.next(null);
        return authService.refreshToken().pipe(
          switchMap(response => {
            isRefreshing = false;
            refreshedToken$.next(response.token);
            return next(cloneWithToken(req, response.token));
          }),
          catchError(refreshError => {
            isRefreshing = false;
            authService.logout();
            return throwError(() => refreshError);
          })
        );
      }
      // A second (concurrent) 401 arrives while a refresh is already in flight — queue it.
      return refreshedToken$.pipe(
        filter((token): token is string => token !== null),
        take(1),
        switchMap(token => next(cloneWithToken(req, token)))
      );
    })
  );
};
```

**On refresh failure mid-request:** the first request that triggers the refresh calls `authService.logout()` and rethrows; any requests that were queued behind `refreshedToken$` are left waiting on a `BehaviorSubject` that will never emit a non-null value again for that refresh cycle — they simply never resolve via this path in the current implementation (there's no explicit `error` propagation to the queued requests on refresh failure, since `refreshedToken$.next()` is only ever called with a token or left at `null`). In practice this is rare because `logout()` navigates away and the queued requests' owning components are torn down before they'd surface a hung state, but it's worth flagging as an edge case if a panel probes it (see §9/§10).

`SKIP_REFRESH` is an `HttpContextToken` set on the retried clone so that a request that fails *again* after a successful refresh doesn't re-enter this interceptor and loop.

### 4.3 `error.interceptor.ts`

Maps every HTTP status to an Arabic `NotificationService` toast (401 → session expired + auto-logout + redirect to login; 403 → "ليس لديك صلاحية للقيام بهذه العملية"; 404, 409, 429, generic 4xx/5xx, and network-down `status === 0` all get distinct Arabic messages), then always rethrows via `throwError(() => error)` so the calling component can still add its own local handling (e.g. a form's `error` signal) on top of the global toast.

### 4.4 `loading.interceptor.ts`

```typescript
export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  if (req.url.includes('/refresh')) return next(req);
  loadingService.show();
  return next(req).pipe(finalize(() => loadingService.hide()));
};
```

`/refresh` is excluded so the silent background token refresh never flickers the global spinner. `LoadingService` internally reference-counts `show()`/`hide()` calls so that N overlapping requests don't hide the spinner after the first one finishes.

### 4.5 `authGuard` vs `roleGuard` — redirect behavior recap

- `authGuard`: unauthenticated → `/auth/login?returnUrl=<url>`.
- `roleGuard(roles)`: wrong role → `/forbidden` (always, no per-role branching in the guard itself).
- The Patient-specific "send them somewhere useful" logic lives in `login.component.ts`'s post-login redirect and in `ForbiddenComponent.goHome()`, both of which route Patient → `/dashboard/patient`, Admin → `/admin/dashboard`, Therapist → `/dashboard`. This is deliberately kept out of the guard (see §1.3 reasoning).

---

## 5. Shared Component Library

### 5.1 Form controls — the `ControlValueAccessor` pattern

Button, Input, Textarea, Checkbox, Radio, and Select all live in `shared/components/`. Button is presentational only (`variant`, `size`, `loading`, `disabled` inputs, `clicked` output) — it does **not** implement `ControlValueAccessor` since it never carries form value. The other five (Input, Textarea, Checkbox, Radio, Select) all implement the identical CVA contract, illustrated by Input:

```typescript
export class InputComponent implements ControlValueAccessor {
  value = '';
  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void { this.value = value ?? ''; }
  registerOnChange(fn: (value: string) => void): void { this.onChange = fn; }
  registerOnTouched(fn: () => void): void { this.onTouched = fn; }
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  onInput(event: Event): void {
    this.value = (event.target as HTMLInputElement).value;
    this.onChange(this.value);
    this.onTouched();
  }
  onBlur(event: FocusEvent): void { this.onTouched(); this.blur.emit(event); }
}
```

registered via a `NG_VALUE_ACCESSOR` provider with `forwardRef()` + `multi: true`, so any of these can be dropped into a reactive form with `formControlName` exactly like a native `<input>`. Checkbox's value type is `boolean`, Radio/Select are `string | number`, each typed accordingly — same four-method contract, different generic.

### 5.2 Data display

- **Table** (`table.component.ts`) — generic `TableComponent<T>`, takes a `columns: TableColumn[]` config (`key`, `label`, `sortable`, `width`, `align`) and a `data: T[]` array; emits `sort`, `rowClick`, `rowAction`. Custom per-column rendering is done via `@ContentChildren(ColumnCellDirective)` — a consumer declares `<ng-template appColumnCell="status">...</ng-template>` and the table looks up the matching template by `columnKey` at render time instead of hardcoding cell markup.
- **Pagination** — takes `totalItems`, `pageSize`, `currentPage`, `maxVisible`; computes `totalPages`, visible `pages` window, `isFirstPage`/`isLastPage`; emits `pageChange`.
- **StatsCard** — signal-`input()`-based metric tile (`label`, `value`, `icon`, `color`, `trend`, `trendLabel`) used across the dashboard.
- **EmptyState** — placeholder for empty lists (`title`, `message`, `icon`/`image`, optional `actionLabel` + `action` output).

### 5.3 Feedback

- **Spinner** — sm/md/lg, overlay or inline, Arabic default label "جاري التحميل...". Driven globally by `LoadingService.isLoading`.
- **Modal** — sm/lg/xl, backdrop-click and Escape-to-close (via `@HostListener`), `showModal`/`showModalChange` for two-way binding, `opened`/`closed` events.
- **Toast / ToastContainer** — `ToastComponent` renders a single dismissible, auto-timed (pausable on hover) alert; `ToastContainerComponent` injects `NotificationService`, reads its `notifications` signal, and renders the queue fixed to the RTL-correct corner using `inset-inline-end` (not `right`).

### 5.4 Charts

`BarChartComponent` and `LineChartComponent` both wrap **Chart.js directly** (instantiating `new Chart(canvasRef, config)` in `ngAfterViewInit`, not going through the `ng2-charts` component API, despite `ng2-charts` being a listed dependency — worth knowing if a panel asks specifically about `ng2-charts` usage). Both take `labels: string[]`, `datasets: ChartDataset<'bar'|'line'>[]`, `title`, `loading`, `height`, defer rendering until the canvas `ViewChild` exists, and `destroy()` the Chart.js instance in `ngOnDestroy` to avoid canvas leaks on repeated navigation. `features/dashboard/pages/dashboard/dashboard.component.ts` feeds them via `computed()` signals that re-derive dataset colors from CSS custom properties (`getComputedStyle(document.documentElement).getPropertyValue('--primary')`) so charts re-theme automatically when `AppStateService.theme()` flips light/dark.

### 5.5 Layouts

`MainLayoutComponent` composes Sidebar + Header + Footer + `<router-outlet>` and owns the mobile sidebar-collapse toggle via `AppStateService`; `AuthLayoutComponent` is a minimal shell for the six auth pages; `SidebarComponent` renders `NavigationService.menuItems()`; `HeaderComponent` hosts the notification bell (polling `InAppNotificationService`) and user dropdown; `FooterComponent` is static.

### 5.6 OnPush everywhere

17 of the 18 components under `shared/components/` (94%) explicitly set `changeDetection: ChangeDetectionStrategy.OnPush`; all five layout shells do too. This is safe without manual `markForCheck()` calls specifically **because** state is signal-backed: reading a signal inside a template automatically registers that template with the signal's dependency graph, and a signal `set()`/`update()` schedules exactly the components that read it for re-check — Angular's signal-aware change detection does the `markForCheck()` equivalent internally, so `OnPush` + signals removes the classic "I mutated an object in place and OnPush didn't notice" failure mode of `OnPush` + plain RxJS/mutable objects.

---

## 6. Features — the 7 modules

### Auth
Login, register, forgot-password, verify-otp, reset-password, profile, forbidden. Login does role-aware redirect after `AuthService.login()` succeeds (see §1.3). Register uses custom validators (`therapistRequiredForPatientValidator`, `passwordMatchValidator`, an uppercase/lowercase/digit regex) and loads the active therapist list for the Patient-role dropdown. Forgot→verify-OTP→reset is a three-page wizard bridged by `PasswordResetStateService` (in-memory, not localStorage, so it clears on refresh — intentional, forces the user to restart the flow rather than resume a stale OTP session). Profile handles name/email edit, password change, and avatar upload with client-side type/size validation (JPG/PNG/WEBP, ≤5 MB) before hitting the backend. Forbidden is the `roleGuard` failure landing page, with the role-aware `goHome()`.

### Patients
Patient-list (debounced 300ms search, `pageSize+1` fetch trick to detect "has next page" without a backend total count), patient-form (create requires password only if an email/login account is being provisioned — enforced by a custom cross-field validator; edit mode drops the password field entirely), patient-detail (tabbed: overview/sessions/exercises/reports/assessments, embeds the AI-summary button calling `AiService.summarizePatient()`), intake-form (structured psych-history fields; still carries dormant OCR-upload signals from the removed OCR feature — see §10), assessment (PHQ-9/GAD-7/DASS-21/PCL-5 template dropdown with two modes — "score now" vs. "assign to patient" — toggled via a reactive-form value-changes subscription that swaps required validators on/off).

### Sessions
Session-landing (empty-state prompting "select a patient first"), session-list, session-form (uses `ngx-quill`'s `QuillEditorComponent` for the rich-text note body, with a 2-second debounced auto-save on `content` value changes that PATCHes the note in the background while the therapist keeps typing), session-detail (embeds the `Summary` component and handles patient-initiated reschedule/cancel request approve/reject), and the `Summary` sub-component (`sessionId = input.required<string>()`, calls `SessionService.getSummary()` which hits the backend's Gateway-backed AI summarization endpoint).

### Exercises
Exercise-list (therapist's global view of all assigned exercises), assign-exercise (form with a custom `futureDateValidator` preventing past due-dates), patient-exercise (dual-role component: as a therapist-viewed tab it fetches by `patientId` override; as the patient's own "my exercises" view it calls the `/my` endpoint instead, since the patient-scoped API rejects the therapist-only by-patient-id route — this branching lives in one component rather than being duplicated across two).

### Reports
Report-landing (same "select a patient first" empty state as sessions), report-list (shows version count per report), report-generate (form with optional `therapistInstructions` free text + language selector, calls `ReportService.generateReport()` which triggers the backend's RAG + Gateway chat pipeline), report-detail (approve/reject workflow, and a client-side HTML→PDF export using `html2pdf.js`: the backend returns an Arabic RTL HTML blob, which is injected off-screen into the DOM, rendered via `html2canvas` at 2x scale, and saved as an A4 PDF — this is a **frontend-only** binary PDF path built on top of the backend's HTML export endpoint, distinct from the backend's own HTML-download feature).

### Dashboard
Single page with stats cards + three Chart.js-driven charts (assessment-score line trend, session-frequency line trend, exercise-completion bar breakdown), fed by `DashboardStateService` with a 5-minute-TTL auto-refresh (`interval()` + `switchMap` re-fetch, skipped while a fetch is already `loading`), and Arabic (`ar-EG`) locale formatting on all labels/dates.

### Chatbot
Chat-list (conversation list with unread tracking via a `localStorage`-persisted "last seen" map, live filtered by patient name search, role-aware "new conversation" patient picker for therapists) and chat-room, which is the one place SignalR is wired in directly:

```typescript
this.hubConnection = new signalR.HubConnectionBuilder()
  .withUrl(environment.signalRHubUrl, { accessTokenFactory: () => localStorage.getItem('jalsa_token') ?? '' })
  .withAutomaticReconnect()
  .build();

this.hubConnection.on('ReceiveMessageChunk', (delta: string) => { this.streamingText.update(prev => prev + delta); });
this.hubConnection.on('ReceiveMessage', (_sender, content) => { /* replaces the streaming bubble with the final AI message */ });
this.hubConnection.on('ReceiveHumanMessage', (incoming: ChatMessage) => { /* dedupes by id, appends */ });

await this.hubConnection.start();
await this.hubConnection.invoke('JoinConversation', this.conversationId());
```

Patients send messages via `hubConnection.invoke('SendMessage', conversationId, patientId, text)` when the connection is `Connected`; therapists (and any patient fallback if SignalR is down) send via a plain REST `POST /api/chat/send`, and the message shows up for everyone in the group via the `ReceiveHumanMessage` broadcast — so the REST path and the SignalR path both converge on the same rendered message list, deduplicated by message `id`. `ngOnDestroy` calls `hubConnection.stop()`.

---

## 7. RTL and Arabic-First Design

**Logical CSS properties** (`margin-inline-start`, `padding-inline-end`, `inset-inline-start`, `border-inline-start`, etc.) resolve to "left" or "right" based on the element's computed `direction`, so the same stylesheet works unmodified for both `dir="rtl"` and `dir="ltr"` — there's no need for a separate RTL stylesheet or a `[dir=rtl]` override for every spacing rule. Jalsa's `src/styles/utilities.css` defines its spacing utility classes (`.ms-*`, `.me-*`, `.ps-*`, `.pe-*`, `.start-*`, `.end-*`, `.border-start`, `.border-end`) exclusively in logical form. That said, the codebase is a **hybrid, not 100% pure**: physical `margin-left`/`margin-right` still appears in ~84 places at the component-CSS level (e.g. the checkbox component), so if a panel probes this, the honest answer is "utility-class layer is logical-property-clean; some component-level CSS still uses physical properties and would need a cleanup pass to be fully direction-agnostic" (see §10).

`<html lang="ar" dir="rtl">` is hardcoded in `index.html`. `src/styles/variables.css` defines the design tokens: Professional Blue primary scale (`--primary: #2563eb`, `--primary-hover: #1d4ed8`, plus a full `--primary-50` … `--primary-900` ramp), semantic colors (success `#10b981`, warning `#f59e0b`, danger `#ef4444`, info `#0ea5e9`), a 4px-based spacing scale (`--space-1` … `--space-9`), a type scale (`--text-xs` … `--text-2xl`), and a dark sidebar palette (`--sidebar-bg: #0f172a`). Font stack: `--font-family-base: 'Cairo', 'IBM Plex Sans Arabic', 'Tajawal', sans-serif`, loaded via Google Fonts with `<link rel="preconnect">` DNS/TLS pre-warming for weights 400/500/600/700 across all three families.

**Bootstrap RTL + Tailwind coexistence:** `styles.css` imports `bootstrap/dist/css/bootstrap.rtl.min.css` first (Bootstrap's own RTL-flipped build handles directional layout — grid, flex order, floats), then the project's own `variables.css`/`typography.css`/`utilities.css` load after it so the cascade lets custom tokens win over Bootstrap defaults. Tailwind 4.1 is wired in purely via the `@tailwindcss/postcss` PostCSS plugin (Tailwind 4's CSS-first config, no `tailwind.config.js`) and is used as a secondary micro-utility layer layered on top — Bootstrap owns structural/directional layout, Tailwind fills in one-off spacing/flex utility needs, and because import order puts Bootstrap first, there's no observed specificity fight between the two.

---

## 8. Testing

54 spec files, roughly 750-770 individual test cases (`describe`/`it` blocks), running under **Vitest 4.0** via **`@analogjs/vite-plugin-angular`**. That plugin is what makes this possible at all: Angular components normally require the Angular CLI's own build pipeline (ngc/AOT compiler) to process decorators and templates, which the standard Vite/Vitest toolchain doesn't understand out of the box. The plugin hooks into Vite's transform pipeline to compile Angular templates and decorator metadata on the fly, so `TestBed` and Angular's `ComponentFixture` work inside a pure Vitest run — no `ng test`/Karma/Jasmine needed.

```typescript
// vitest.config.ts
export default defineConfig({
  plugins: [angular()],
  test: { globals: true, environment: 'jsdom', include: ['src/**/*.spec.ts'], setupFiles: ['src/test-setup.ts'] },
});
```

Coverage confirmed present: `app.spec.ts`; `http-client.service.spec.ts`; `auth.guard.spec.ts`, `role.guard.spec.ts`; all 4 interceptors (`auth`, `error`, `loading`, `refresh-token`); state services `patient-state` (18 cases), `session-state` (16), `exercise-state` (7), `admin-users-state` (6); `app-state.service.spec.ts`; `exercise.service.spec.ts`; `loading.service.spec.ts`; `notification.service.spec.ts`; `navigation.service.spec.ts`; `auth.service.spec.ts`; `admin.service.spec.ts`; `in-app-notification.service.spec.ts`; the assessment, intake-form, patient-detail, and patient-form feature pages; and 5 of the 6 form components (input, textarea, checkbox, radio, select — **Button has no dedicated spec file**, a small gap worth naming proactively).

---

## 9. Likely Defense Questions

**1. "Why Signals instead of NgRx?"**
No cross-cutting global state that needs a single store, time-travel debugging, or action replay — each of the 13 state services is genuinely scoped to one domain and consumed by 2-4 components at most. Signals give single-source-of-truth + immutable updates + reactive derivation (`computed()`) with a fraction of NgRx's boilerplate (no actions/reducers/effects/selectors), and it's the direction Angular 21 itself is pushing (signal-based `input()`/`output()`, signal-aware `OnPush`). For a 5-person team on a fixed capstone timeline, NgRx's ceremony cost outweighs its benefit at this state-graph size.

**2. "Why no shared base state class across the 5+ entity state services?"**
Honest answer: it's duplication, and it's a known gap (see §10). It was a deliberate trade-off early on to keep each service simple and directly readable rather than introduce a generic `BaseStateService<T>` abstraction whose type-parameterization would have added indirection for a team still ramping up on Signals. The fix is straightforward (extract a generic base with `list/selected/loading/error` + `computed count`) and is on the post-MVP list.

**3. "How do you prevent stale state across two state services showing the same patient?"**
We don't, fully — this is a real gap. `PatientStateService.selectedPatient` and, say, an assessment or session view that also caches patient info independently can diverge if one is updated and the other isn't refreshed. In practice the blast radius is small because most patient-scoped views re-fetch via `patientService.getPatient(id)` on route entry rather than trusting a stale cross-service cache, but there's no reactive "single patient truth" today. A future fix would be a shared `computed()` selector layer or moving to a single normalized entity store.

**4. "What happens on token refresh failure mid-request?"**
The interceptor that triggered the refresh calls `authService.logout()` and rethrows — that request fails visibly. Requests that were *queued* behind that refresh (waiting on `refreshedToken$`) are left subscribed to a `BehaviorSubject` that never emits again for that cycle; they don't get an explicit error propagated to them in the current implementation. In practice `logout()` triggers a navigation to `/auth/login` and the components that issued those queued requests are torn down before a user notices a hang, but it's a known edge case, not an airtight guarantee (see §10).

**5. "Why is `enableMockApi` still `true` in dev — is that a risk?"**
It's flagged as a known issue in the project's own tracking doc. The risk is real: a developer running locally against the mock JSON fixtures in `frontend/src/assets/mocks/` can pass local manual testing while a real backend integration bug goes unnoticed until CI or staging. It's not a production risk (staging/production environments have it hardcoded `false`), but it should be flipped to `false` by default in dev, with a documented opt-in flag for offline/demo work instead.

**6. "Why do 3-4 chatbot components and `crisis-alerts-list.component.ts` call `HttpClientService` directly, breaking the stated rule?"**
The chatbot components' justification is real-time SignalR coupling — the chat-room and chat-list components manage a live hub connection alongside REST fallback calls (send, regenerate, close), and that tight coupling made it pragmatic to call the gateway directly rather than add a wrapper service that would just proxy calls through. `crisis-alerts-list.component.ts`, however, is a genuine rule violation with no such justification — it should have a `CrisisAlertService` extracted; that's a concrete, fixable item to raise proactively rather than have a panelist find it first.

**7. "Why OnPush everywhere, and doesn't that risk components missing updates?"**
OnPush plus Signals removes the classic failure mode (mutating an object in place and OnPush not noticing) because signal reads inside a template register that template with the signal, and any `set()`/`update()` schedules exactly the dependent views for re-check — this is the same mechanism as `markForCheck()` but automatic and precise, not a manual sledgehammer. The one place this could still bite is if a component reads a plain (non-signal) mutable object reference and mutates it in place instead of going through the domain's signal API — but the state-service pattern is designed specifically to prevent that by never exposing a mutable reference, only readonly signals.

**8. "Why does the report PDF export happen client-side with `html2pdf.js` instead of the backend generating a real PDF?"**
The backend ships an Arabic RTL HTML export endpoint (`GET /api/reports/{id}/export`) as the shipped MVP feature; a true binary PDF generator is explicitly listed as Post-MVP on the backend. The frontend's `html2pdf.js` approach (render the returned HTML off-screen, `html2canvas` snapshot at 2x scale, wrap in a jsPDF A4 doc) is a client-side stopgap that gets users a downloadable PDF today without waiting on a backend PDF library integration — the trade-off is that it's a canvas rasterization of HTML, not vector text, so the exported PDF is not text-selectable/searchable, which is worth naming as a known limitation.

**9. "Walk me through why `authGuard` doesn't race the app's auth-state hydration on a hard refresh."**
`provideAppInitializer()` in `app.config.ts` runs `AuthService.initializeAuth()` — which restores the user/roles from the stored JWT — and Angular's router will not activate the first route (and therefore will not evaluate `authGuard`) until all `APP_INITIALIZER`-equivalent providers resolve. So by the time `authGuard` runs, `authService.isAuthenticated()` already reflects the restored session, not an unhydrated default.

**10. "Why is FluentValidation only on 3 backend DTOs, and does the frontend assume more validation coverage than actually exists server-side?"**
Yes, partially — the frontend layers its own reactive-form `Validators` + custom validators (e.g. `passwordMatchValidator`, `futureDateValidator`, `therapistRequiredForPatientValidator`) on essentially every form, and `errorInterceptor` gracefully surfaces whatever the backend does return as `error.message`/`error.error`. But since only Exercise DTOs have FluentValidation server-side and everything else relies on DataAnnotations, there's a real gap where a malformed payload that slips past frontend validation (a bug, a direct API call, a race condition) might not get a clean, well-formed validation error back from most endpoints — DataAnnotations errors are shaped differently and less richly than FluentValidation's. The frontend's Arabic error mapping is generic enough (`'البيانات المدخلة غير صحيحة'` fallback) to not crash on an unexpected shape, but it also won't surface field-level detail the backend isn't producing.

**11. "How does the dual-role `patient-exercise` component avoid duplicating logic for therapist vs patient view?"**
One component, one `ngOnInit` branch: if a `patientIdOverride` input is set (therapist viewing a specific patient's tab), it calls `exerciseService.getExercisesByPatient(patientId)`; if not (patient viewing their own exercises), it calls `exerciseService.getMyExercises()`, because the patient-scoped backend endpoint doesn't accept an arbitrary patient ID (would 403). This keeps the completion-logging and reflection-note UI identical for both roles while only branching the initial fetch call.

**12. "Explain the SignalR reconnection story — what happens if the WebSocket drops mid-chat?"**
`.withAutomaticReconnect()` on the `HubConnectionBuilder` handles transient drops with SignalR's default backoff. If the connection can't be re-established (or hasn't started yet) when the patient tries to send, `dispatchMessage()` falls back to the REST `POST /api/chat/send` path — the same path therapists always use — so the conversation degrades to non-streaming REST rather than failing outright. The UI also optimistically appends the outgoing message locally before the hub round-trip completes, and reconciles/dedupes by message `id` when the `ReceiveHumanMessage` broadcast arrives, so a slow network doesn't make the UI feel unresponsive.

**13. "Why 5-minute cache TTL on dashboard state, and what happens if a therapist logs a session then immediately checks the dashboard?"**
`DashboardStateService.isStale` is a `computed()` checking `Date.now() - lastLoadedAt > 5min`; combined with an `interval()`-driven auto-refresh in `dashboard.component.ts`, this balances not hammering the analytics endpoint on every dashboard visit against staying reasonably current. If a therapist creates a session and immediately navigates to the dashboard, they will see stale numbers until the next 5-minute tick or a manual refresh — this is an accepted trade-off for an analytics view, not a live transactional one, but it's honest to note there's no active invalidation (e.g. the session-create flow doesn't call `dashboardState.reset()`).

**14. "Why lazy-load every feature route instead of eager-loading a smaller app?"**
Initial bundle size — a clinic-management SPA with 7 feature modules, ngx-quill, Chart.js, SignalR, and html2pdf.js as dependencies would produce a large first-load chunk if bundled eagerly. `loadChildren`/`loadComponent` on every feature route means the initial payload is just the router + `MainLayoutComponent` + auth feature (needed immediately for the login screen), and heavier dependencies like `ngx-quill` (sessions) or `html2pdf.js` (reports) only download when a user actually navigates into those features.

**15. "Is Tailwind actually needed given Bootstrap RTL is already handling layout — isn't that redundant?"**
Fair challenge. Bootstrap RTL is the primary structural/grid/flex-direction system; Tailwind's utility classes are used for one-off spacing, sizing, and visual tweaks that would otherwise require writing bespoke component CSS. It is a deliberate two-layer approach rather than a clean single-framework choice, and the honest trade-off is slightly larger CSS output and two utility-class vocabularies for new contributors to learn — but it avoids writing custom CSS for every minor layout tweak across ~40+ components, which was judged a bigger productivity win for a 5-person team than committing to one framework's utility set exclusively.

---

## 10. Known Gaps

- **No shared base state class.** All 13 state services (`PatientStateService`, `SessionStateService`, `ExerciseStateService`, `ReportStateService`, and the rest) hand-implement the identical `signal + asReadonly + computed` boilerplate independently. A generic `BaseStateService<T>` would remove real duplication; it wasn't extracted due to time constraints and a preference for directness while the team was still ramping up on Signals.
- **`enableMockApi: true` in `environment.ts` (dev only).** Real risk of masking backend integration bugs during local development, since Development points at mock JSON fixtures under `frontend/src/assets/mocks/` rather than always hitting the live API. Not a production risk (staging/prod are `false`), but should default to `false` locally with an explicit opt-in for offline demo work.
- **Rule violations on `HttpClientService` direct calls.** Chatbot's chat-room/chat-list components call `HttpClientService` directly, arguably justified by tight SignalR coupling; `crisis-alerts-list.component.ts` does too, with no such justification — a `CrisisAlertService` should be extracted.
- **Refresh-token queueing has no explicit failure propagation to queued requests.** If a refresh fails while other requests are queued waiting on it, those queued requests don't receive an explicit rethrown error — they're left subscribed to a `BehaviorSubject` that stops emitting for that cycle. Masked in practice by the resulting logout/navigation, but not an airtight contract.
- **No active cache invalidation between actions and dashboard state.** `DashboardStateService`'s 5-minute TTL is time-based only; creating a session/exercise/report doesn't proactively call `dashboardState.reset()`, so newly created data may not appear on the dashboard for up to 5 minutes.
- **Hybrid RTL implementation.** The shared utility-class layer (`utilities.css`) is cleanly logical-property-based, but ~84 physical `margin-left`/`margin-right` declarations remain at the component-CSS level (e.g. `checkbox.component.css`). Not broken today (the app is Arabic-only, RTL-only in practice), but not future-proof if an LTR/English mode were ever added.
- **Client-side PDF export is rasterized, not vector text.** `html2pdf.js` + `html2canvas` produces a PDF that is a canvas snapshot of the report HTML — not selectable/searchable/copyable text — because the backend doesn't yet ship a real PDF generator (explicitly Post-MVP).
- **Backend validation asymmetry the frontend partially compensates for.** Only Exercise DTOs use FluentValidation server-side; everything else relies on DataAnnotations. The frontend layers its own client-side validators and a generic Arabic fallback error message, but can't guarantee rich, field-level server error detail for non-Exercise endpoints.
- **Dormant OCR-related signals in `intake-form.ts`.** The backend removed OCR (`OcrService`, the intake-image endpoint, and the `OcrRequest` DTO) because the AI Gateway has no vision proxy, but the frontend's intake-form component still carries leftover `ocrLoading`/`ocrSuccess`/`selectedFileName` signal declarations and file-upload wiring from the removed feature — dead code that should be deleted, not just orphaned.
- **`Button` component has no dedicated spec file.** All five other form controls (Input, Textarea, Checkbox, Radio, Select) have full Vitest coverage; Button does not, despite being the most widely used shared component in the app.
- **No shared "single patient truth" across state services.** `PatientStateService.selectedPatient` and patient-scoped data cached in other state services (assessment, session) can diverge if one is updated without the other refreshing — most views work around this by re-fetching on route entry rather than trusting a cross-service cache, but there's no reactive guarantee of consistency.
