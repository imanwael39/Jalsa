# State Management Strategy

## Overview

Jalsa uses **Angular Signals** combined with **RxJS Services** for state management. This approach avoids the overhead of NgRx while providing sufficient reactivity and type safety for the application's complexity.

## State Management Layers

| State Type | Approach | Example |
|---|---|---|
| **Local/Component State** | Angular Signals (`signal()`, `computed()`) | Form values, UI visibility, local loading flags |
| **Shared/Feature State** | Services + Signals (`signal()` in root services) | Patient list, selected patient, session draft |
| **Application State** | `AppStateService` with Signals | Global loading indicator, notifications, theme |
| **Server/API State** | Services that call APIs and update Signals | Patient data from API, session data |
| **URL/Route State** | Angular Router (params, query params, data) | Patient ID, page, search filters |

## Principles

### 1. Signals for Synchronous State

- Use `signal()` for state that is read and written within the same session.
- Use `computed()` for derived state that depends on other signals.
- Signals are read-only when exposed publicly (`asReadonly()`).

```typescript
@Injectable({ providedIn: 'root' })
export class PatientService {
    private patientsSignal = signal<Patient[]>([]);
    readonly patients = this.patientsSignal.asReadonly();

    readonly activePatients = computed(() =>
        this.patientsSignal().filter(p => !p.isArchived)
    );
}
```

### 2. RxJS for Async Operations

- Use RxJS `Observable` for API calls (via `HttpClientService`).
- Convert API responses to Signals when the data reaches the service.
- Use `takeUntil`, `takeUntilDestroyed`, or `async` pipe — never subscribe without cleanup.

```typescript
loadPatients(): void {
    this.http.get<PaginatedResponse<Patient>>(API.patients.base)
        .pipe(takeUntilDestroyed())
        .subscribe({
            next: (data) => this.patientsSignal.set(data.items),
        });
}
```

### 3. No NgRx

Angular Signals + services are sufficient for this project's complexity. If the project grows significantly and cross-cutting state concerns become unmanageable, NgRx can be introduced later.

### 4. AppStateService

Global application state (loading indicators, notifications, theme) is managed through a single `AppStateService`:

```typescript
@Injectable({ providedIn: 'root' })
export class AppStateService {
    private loadingSignal = signal(false);
    readonly isLoading = this.loadingSignal.asReadonly();

    private notificationsSignal = signal<Notification[]>([]);
    readonly notifications = this.notificationsSignal.asReadonly();

    showLoading(): void { this.loadingSignal.set(true); }
    hideLoading(): void { this.loadingSignal.set(false); }
}
```

## Data Flow

```
Component (template)
    ↑ Signals (readonly)
Service (state management + API calls)
    ↑ HTTP calls via HttpClientService
API Endpoints (api-endpoints.ts)
    ↑ Interceptors (auth, error, loading)
Backend / Mock (dev)
```

## Rules

1. **Components never call `HttpClientService` directly** — always delegate to a feature service.
2. **Services expose Signals as readonly** — components cannot mutate state directly.
3. **Avoid storing API data in component state** — store it in services, reference via Signals.
4. **Use `computed()` for derived state** — never derive state manually in methods.
