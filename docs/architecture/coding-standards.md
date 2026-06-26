# Coding Standards

## Language and Compiler

- **TypeScript:** Strict mode enabled (`strict: true` in `tsconfig.json`).
- **Target:** ES2022.
- **No `any` type** — use proper types and interfaces everywhere. Use `unknown` when the type is not known at compile time.

## Naming Conventions

| Element | Convention | Example |
|---|---|---|
| Variables | `camelCase` | `selectedPatient` |
| Functions/methods | `camelCase` | `getPatientById()` |
| Classes | `PascalCase` | `PatientListComponent` |
| Interfaces | `PascalCase` | `CreatePatientRequest` |
| Types | `PascalCase` | `PatientStatus` |
| Files | `kebab-case` | `patient-list.component.ts` |
| Component selectors | `app-` prefix with kebab-case | `app-button`, `app-patient-list` |
| Directives | `app` prefix + camelCase | `appRole`, `appClickOutside` |
| Pipes | `camelCase` | `dateAgo`, `truncate` |

## Component Standards

- **All components are standalone** — `@Component({ standalone: true })`.
- **Change detection:** Always use `ChangeDetectionStrategy.OnPush`.
- **Component files:** Each component in its own folder with `.ts`, `.html`, `.css` files.
- **File naming:** `<name>.component.ts`, `<name>.component.html`, `<name>.component.css`.
- **Input/Output:** Use `@Input()` and `@Output()` decorators. Never mutate inputs.
- **Host binding:** Use `@HostBinding` for applying CSS classes based on component state.

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
}
```

## Service Standards

- All services use `providedIn: 'root'`.
- Use Signals for reactive state: `signal()` for writable state, `computed()` for derived state.
- Expose Signals as readonly (`asReadonly()`) to consumers.
- Use `HttpClientService` for all API calls — never inject `HttpClient` directly.
- Use the `API` constant for endpoint paths.

## Guard Standards

- **Functional guards only** — use `CanActivateFn` with `inject()`.
- Never use class-based guards.
- Return `true` or `UrlTree` for redirects.

```typescript
export const authGuard: CanActivateFn = () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    if (auth.isAuthenticated()) return true;
    return router.parseUrl('/auth/login');
};
```

## Interceptor Standards

- **Functional interceptors only** — use `HttpInterceptorFn` with `inject()`.
- Always clone the request before modifying.
- Three interceptors: auth (JWT token), error (global handling), loading (spinner state).

## State Management

- Local state: `signal()` and `computed()`.
- Shared state: Services with `signal()` exposed as `asReadonly()`.
- Application state: `AppStateService`.
- No NgRx.

## RxJS Standards

- Prefer `async` pipe in templates over manual subscriptions.
- Use `takeUntil`, `takeUntilDestroyed`, or `async` pipe — never subscribe without cleanup.
- Use `pipe()` for operators. Chain operators for readability.
- Handle errors with `catchError` and rethrow or delegate to `NotificationService`.

```typescript
loadPatients(): void {
    this.http.get<PaginatedResponse<Patient>>(API.patients.base)
        .pipe(takeUntilDestroyed())
        .subscribe({
            next: (data) => this.patientsSignal.set(data.items),
            error: () => this.notification.error('Failed to load patients'),
        });
}
```

## Styling Standards

- **Plain CSS only** — no SCSS, Sass, or Less.
- Use CSS variables for design tokens (colours, spacing, typography).
- Use Bootstrap 5 utility classes (`.m-`, `.p-`, `.d-flex`) preferentially.
- Use logical properties (`margin-inline-start`, `padding-inline-end`) for RTL support.
- Follow the existing CSS variable naming in `styles.css`.

## Import Standards

- Never import `CommonModule` directly.
- Import individual Angular directives: `NgIf`, `NgFor`, `AsyncPipe`, `DatePipe`, etc.
- Use barrel files (`index.ts`) for component folders.

```typescript
@Component({
    standalone: true,
    imports: [NgIf, AsyncPipe, RouterLink],
})
```

## Linting and Formatting

- ESLint for TypeScript linting (Angular style guide).
- Prettier for automatic code formatting.
- Husky for pre-commit hooks (lint-staged).
- commitlint for commit message validation.

## Commit Conventions

Follow Conventional Commits:

- `feat:` — new feature
- `fix:` — bug fix
- `docs:` — documentation
- `style:` — formatting
- `refactor:` — code restructuring
- `perf:` — performance improvement
- `test:` — testing
- `chore:` — build/tooling

## Security

- JWT tokens stored in `localStorage` (with CSP and XSS protections).
- Logout on 401 (handled by `errorInterceptor`).
- Role-based access via `roleGuard` and `appRole` directive.
- Sanitize user-generated content with `DomSanitizer`.
- No sensitive data in client-side logs.
