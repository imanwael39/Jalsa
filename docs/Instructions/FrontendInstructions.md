# Frontend Instructions for AI Agents

This document defines the **mandatory constraints, patterns, and conventions** that all generated code must follow. Adhering to these rules ensures consistency across different AI sessions and models.

---

## 1. General Principles

- **Use Angular 17+ Standalone Architecture** – absolutely no NgModules. All components, directives, pipes, and guards must be standalone.
- **Use Functional Guards** – implement guards as `CanActivateFn` (with `inject()`). Avoid class‑based guards.
- **Use Functional Interceptors** – implement interceptors as `HttpInterceptorFn`.
- **State Management** – use **Angular Signals** for local/component state and **Services with RxJS** for shared/global state. **Do not use NgRx** unless explicitly requested.
- **Change Detection** – use `ChangeDetectionStrategy.OnPush` on every component.
- **Lazy Load all Features** – every feature must be lazy‑loaded via `loadChildren` in `app.routes.ts`.
- **Inject Dependencies** – use `inject()` inside functional guards and interceptors, and constructor injection for services.

---

## 2. File and Folder Structure

- Follow the approved folder structure:
```
src/app/
├── core/
│ ├── api/ # API endpoints, HTTP client service
│ ├── guards/ # Functional guards (auth, role)
│ ├── interceptors/ # Functional interceptors (auth, error, loading)
│ ├── models/ # DTO interfaces (Patient, Session, etc.)
│ ├── services/ # Feature services (AuthService, PatientService, etc.)
│ └── state/ # State management utilities and base classes
├── shared/
│ ├── components/ # Reusable UI components (Button, Input, Modal, etc.)
│ ├── directives/ # Directives (RoleDirective, ClickOutsideDirective)
│ ├── pipes/ # Pipes (TruncatePipe, DateAgoPipe)
│ └── layouts/ # Layout components (MainLayout, AuthLayout)
└── features/ # Lazy‑loaded features
├── auth/
├── patients/
├── sessions/
├── exercises/
├── reports/
├── dashboard/
└── chatbot/
```

- Each component/directive/pipe should be in its own folder with `component.ts`, `component.html`, `component.css`, and an `index.ts` barrel file.
- Use **kebab‑case** for file names (e.g., `patient-list.component.ts`).
- Use **PascalCase** for class names (e.g., `PatientListComponent`).
- Use **camelCase** for property and method names.

---

## 3. Component Implementation

- All components **must** be standalone.
- Use `@Component({ standalone: true })`.
- Use `ChangeDetectionStrategy.OnPush`.
- Use **plain CSS** with `.css` files – no SCSS, no Sass, no Less.
- Prefer **host binding** (`@HostBinding`) for applying CSS classes based on component inputs.
- Use **content projection** (`ng-content`) where appropriate.
- Emit events using `@Output()`, never mutate inputs directly.
- For form components, implement `ControlValueAccessor` and provide `NG_VALUE_ACCESSOR`.
- Use **computed signals** for derived state, and **effects** only when side‑effects are necessary.

### Component Template:
```typescript
@Component({
  selector: 'app-button',
  standalone: true,
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonComponent {
  @Input() variant: 'primary' | 'secondary' = 'primary';
  @Output() clicked = new EventEmitter<void>();
  // ...
}
```
---
## 4. Service Implementation

- Services must be provided in root (`providedIn: 'root`').

- Use Signals for reactive state within services.
 
- Use `HttpClientService` (the custom wrapper) for all API calls – never inject HttpClient directly.
 
- Handle errors using `catchError` and rethrow or notify via `NotificationService`.

## Service Example:
```
@Injectable({ providedIn: 'root' })
export class PatientService {
  private patientsSignal = signal<Patient[]>([]);
  readonly patients = this.patientsSignal.asReadonly();

  constructor(private http: HttpClientService) {}

  loadPatients(): void {
    this.http.get<Patient[]>(API.patients.base).subscribe({
      next: (data) => this.patientsSignal.set(data),
      error: (err) => this.notificationService.error('Failed to load patients'),
    });
  }
}
```
---
## 5. Guard Implementation
- Must be functional (`CanActivateFn`).
- Use `inject()` to get dependencies.
- Return `true` or `UrlTree` (redirect).

## Auth Guard:
```
export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);
  if (auth.isAuthenticated()) return true;
  return router.parseUrl('/auth/login');
};
```
## Role Guard (higher‑order):
```
export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    if (auth.hasAnyRole(allowedRoles)) return true;
    return router.parseUrl('/forbidden');
  };
};
```
---
## 6. Interceptor Implementation
- Must be functional (`HttpInterceptorFn`).
- Use `inject()` for services.
- Always clone the request before modifying.\
- For auth interceptor: add `Authorization` header with token.
- For error interceptor: catch 401, 403, 500 and show notifications via `NotificationService`.
- For loading interceptor: use `LoadingService` to show/hide global loading.

## Interceptor Example:
```
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.getToken();
  if (token) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }
  return next(req);
};
```
---
## 7. Styling and RTL
- Use plain CSS – no preprocessors allowed.
- Use Bootstrap 5 with RTL support.
- Import Bootstrap and Bootstrap RTL in `src/styles.css` in the correct order.
- Define CSS variables in `src/styles.css` for the design system (colours, spacing, typography, etc.).
- Use utility classes (.`m-`, `.p-`, .`d-flex`, etc.) from Bootstrap instead of custom CSS where possible.
- For margins/paddings, use logical properties (e.g., `margin-inline-start`) or Bootstrap’s RTL‑aware classes (.`ms`-, .`me`-).
- Always test components in both RTL and LTR modes.

## Example: Global CSS Variables
```
/* src/styles.css */
:root {
  --primary: #0F172A;
  --secondary: #3B82F6;
  --success: #22C55E;
  --danger: #EF4444;
  --font-family-base: 'Cairo', 'Inter', sans-serif;
  --spacer: 1rem;
  --border-radius: 0.375rem;
  --box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}
```
---
## 8. State Management
- Component State: Use signal and computed.
- Shared State: Use services with signal exposed as readonly.
- API State: Use services that call APIs and update signals.
- Global State: Use AppStateService for loading, notifications, theme, etc.<br>
**DO NOT** use NgRx, Redux, or any external state management library unless explicitly requested.
---
## 9. API Integration
- Always use `HttpClientService` (from `core/api`).
- Never inject `HttpClient` directly.
- All endpoints must be defined in `core/api/api`-endpoints.ts.
- Use the `API` constant to reference endpoints.
- Handle errors via the `errorInterceptor` – do not duplicate error handling in services.
---
## 10. Testing
- Write unit tests for all components, services, pipes, directives, and guards.
- Use Jasmine and Karma (or Jest if configured).
- Test component interactions via `TestBed` and `ComponentFixture`.
- Mock services using `jasmine.createSpyObj()` or `ng-mocks`.
- Aim for ≥ 80% code coverage.
- Write integration tests for critical user flows (e.g., login, patient CRUD).
- Use Cypress or Playwright  for E2E tests (smoke suite).
---
## 11. Accessibility
- Follow **WCAG 2.1 AA** guidelines.
- Use semantic HTML (e.g., `<button>`, `<nav>`, `<main>`).
- Add `aria-label`, `role`, and `aria-describedby` where necessary.
- Ensure **keyboard navigation** works (Tab, Enter, Escape).
- Maintain **colour contrast** ratios (≥ 4.5:1 for normal text).
- Use `visually-hidden` class for screen‑reader‑only text.
---
## 12. Security
- Store **JWT tokens** in `localStorage` (with appropriate CSP and XSS protections).
- Logout on 401 response (handled by interceptor).
- Role‑based access enforced via `roleGuard` and `*appRole` directive.
- Sanitise any user‑generated content before rendering (Angular’s DomSanitizer).
- Do not expose sensitive data in client‑side logs.
---
## 13. Performance
- Use lazy loading for all features.
- Use OnPush change detection.
- Use trackBy in `*ngFor` loops.
- Use **Signals** for fine‑grained reactivity.
- Optimise images (use responsive formats, lazy loading).
- Minify and bundle with Angular CLI optimisations.
- Run Lighthouse audits and aim for scores ≥ 90.
---
## 14. AI‑Specific Constraints
- Do not generate class‑based guards – always use functional.
- Do not generate NgModules – all components are standalone.
- Do not use `any` type – use proper types and interfaces.
- Do not use ``var`` – use `const` or `let`.
- Do not use `subscribe` without cleanup – prefer `async` `pipe`, `takeUntil`, or `takeUntilDestroyed`.
- Do not import `CommonModule` directly – use `AsyncPipe`, `NgIf`, `NgFor` individually.
- Always include `standalone: true `in components, directives, pipes.
- Always include `providedIn: 'root'` for services.
- Always use the custom `HttpClientService` instead of `HttpClient`.
- Always use the `API` constant for endpoints.
- Always follow the folder structure above.
- **Do not use SCSS, Sass, or Less** – use plain CSS only.
---
## 15. Commit Conventions
Use Conventional Commits:

- `feat: add patient list`
- `fix: correct login redirect`
- `docs: update API documentation`
- `style: format with Prettier`
- `refactor: change patient service to Signals`
- `perf: add trackBy to patient list`
- `test: add unit tests for auth guard`
---
## 16. Additional Tools
- ESLint – enforce Angular style guide.
- Prettier – automatic formatting.
- Husky – pre‑commit hooks for linting and formatting.
- commitlint – validate commit messages.
---
## 17. How to Use This Document
- Read this document before starting any task.
- Reference it while generating code.
- Follow every rule – deviations must be justified and documented.
- Review generated code against these rules.



