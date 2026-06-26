# Folder Structure

## Overview

The Jalsa frontend follows a modular folder structure organised around Angular 17 Standalone Components. The structure separates code into three main areas: **core** (singleton services, models, guards), **shared** (reusable UI components), and **features** (lazy-loaded feature modules).

## Top-Level Structure

```
src/
├── app/
│   ├── core/                          # Singleton services, guards, interceptors, models
│   │   ├── api/
│   │   │   ├── api-endpoints.ts      # All endpoint constants
│   │   │   └── http-client.service.ts # Base HTTP service
│   │   ├── guards/
│   │   │   ├── auth.guard.ts         # Authentication guard
│   │   │   └── role.guard.ts         # Role-based guard
│   │   ├── interceptors/
│   │   │   ├── auth.interceptor.ts   # Token injection
│   │   │   ├── error.interceptor.ts  # Global error handling
│   │   │   └── loading.interceptor.ts # Loading state
│   │   ├── models/                   # All DTO interfaces
│   │   │   ├── patient.model.ts
│   │   │   ├── auth.model.ts
│   │   │   ├── session.model.ts
│   │   │   ├── exercise.model.ts
│   │   │   ├── report.model.ts
│   │   │   ├── chat.model.ts
│   │   │   └── dashboard.model.ts
│   │   ├── services/                # Feature services
│   │   │   ├── auth.service.ts
│   │   │   ├── patient.service.ts
│   │   │   ├── session.service.ts
│   │   │   ├── exercise.service.ts
│   │   │   ├── report.service.ts
│   │   │   ├── chat.service.ts
│   │   │   └── dashboard.service.ts
│   │   └── state/                   # Application state
│   │       ├── app-state.service.ts
│   │       └── feature-state/       # Feature-specific state
│   │           ├── patient-state.service.ts
│   │           └── session-state.service.ts
│   │
│   ├── shared/                      # Reusable UI components
│   │   ├── components/
│   │   │   ├── button/
│   │   │   ├── input/
│   │   │   ├── modal/
│   │   │   ├── table/
│   │   │   ├── pagination/
│   │   │   ├── spinner/
│   │   │   └── toast/
│   │   ├── directives/
│   │   │   ├── role.directive.ts
│   │   │   └── click-outside.directive.ts
│   │   ├── pipes/
│   │   │   ├── date-ago.pipe.ts
│   │   │   └── truncate.pipe.ts
│   │   └── layouts/
│   │       ├── main-layout/
│   │       └── auth-layout/
│   │
│   ├── features/                   # Lazy-loaded features
│   │   ├── auth/
│   │   │   ├── pages/
│   │   │   │   ├── login/
│   │   │   │   ├── register/
│   │   │   │   ├── reset-password/
│   │   │   │   └── profile/
│   │   │   ├── components/
│   │   │   └── auth.routes.ts
│   │   ├── patients/
│   │   │   ├── pages/
│   │   │   │   ├── patient-list/
│   │   │   │   ├── patient-detail/
│   │   │   │   ├── patient-form/
│   │   │   │   ├── intake-form/
│   │   │   │   └── assessment/
│   │   │   └── patients.routes.ts
│   │   ├── sessions/
│   │   │   ├── pages/
│   │   │   │   ├── session-list/
│   │   │   │   ├── session-form/
│   │   │   │   └── session-detail/
│   │   │   ├── components/
│   │   │   │   ├── voice-recorder/
│   │   │   │   └── summary/
│   │   │   └── sessions.routes.ts
│   │   ├── exercises/
│   │   │   ├── pages/
│   │   │   │   ├── exercise-list/
│   │   │   │   ├── assign-exercise/
│   │   │   │   └── patient-exercise/
│   │   │   └── exercises.routes.ts
│   │   ├── reports/
│   │   │   ├── pages/
│   │   │   │   ├── report-list/
│   │   │   │   ├── generate-report/
│   │   │   │   └── report-edit/
│   │   │   └── reports.routes.ts
│   │   ├── dashboard/
│   │   │   ├── pages/
│   │   │   │   └── dashboard/
│   │   │   └── dashboard.routes.ts
│   │   └── chatbot/
│   │       ├── pages/
│   │       │   ├── chat/
│   │       │   └── chat-monitor/
│   │       ├── components/
│   │       └── chatbot.routes.ts
│   │
│   ├── app.routes.ts               # Root routes with lazy loading
│   └── app.config.ts               # Application configuration
│
├── assets/
│   ├── mocks/                     # Mock data for development
│   └── images/
│
├── environments/
│   ├── environment.ts              # Development environment
│   ├── environment.prod.ts         # Production environment
│   └── environment.staging.ts      # Staging environment
│
└── styles/
    ├── _variables.css              # CSS variables
    ├── _typography.css             # Typography styles
    ├── _utilities.css              # Utility classes
    └── styles.css                  # Main stylesheet
```

## Directory Responsibilities

### `core/`
Contains singleton services, models, guards, and interceptors used application-wide. Every file in `core/` is **provided in root** and loaded once.

- **`core/api/`** — Endpoint constants (`api-endpoints.ts`) and the base HTTP client service (`http-client.service.ts`). All feature services delegate API calls through `HttpClientService`.
- **`core/guards/`** — Functional route guards (`CanActivateFn`): `authGuard` for authentication, `roleGuard` (higher-order factory) for role-based access.
- **`core/interceptors/`** — Functional HTTP interceptors (`HttpInterceptorFn`): `authInterceptor` injects JWT tokens, `errorInterceptor` handles global errors, `loadingInterceptor` manages loading state.
- **`core/models/`** — TypeScript interfaces (DTOs) for all API request/response shapes. One file per domain entity.
- **`core/services/`** — Feature services that encapsulate business logic and API calls for each domain.
- **`core/state/`** — Application-wide state services (`AppStateService`) and feature-specific state services.

### `shared/`
Reusable UI components, directives, pipes, and layout components. These have **no business logic** and receive data via `@Input()`.

- **`shared/components/`** — Atomic/design-system components (button, input, modal, table, pagination, spinner, toast).
- **`shared/directives/`** — Attribute directives (`appRole`, `clickOutside`).
- **`shared/pipes/`** — Pure pipes (`dateAgo`, `truncate`).
- **`shared/layouts/`** — Full-page layout components (`MainLayout` with sidebar/header, `AuthLayout` for login/register).

### `features/`
Lazy-loaded feature modules. Each feature has its own route configuration file (`*.routes.ts`), pages, and optionally shared components scoped to that feature.

- **`features/<name>/pages/`** — Route-level page components. One folder per page.
- **`features/<name>/components/`** — Components used only within this feature.
- **`features/<name>/<name>.routes.ts`** — Feature route configuration, lazy-loaded via `loadChildren` in `app.routes.ts`.

## Naming Conventions

| Element       | Convention         | Example                     |
|---------------|--------------------|-----------------------------|
| Files         | `kebab-case`       | `patient-list.component.ts` |
| Classes       | `PascalCase`       | `PatientListComponent`      |
| Properties    | `camelCase`        | `selectedPatient`           |
| Selectors     | `app-` prefix      | `app-button`                |
| Interfaces    | `PascalCase`       | `CreatePatientRequest`      |

## Key Rules

1. **No NgModules** — All components, directives, and pipes are standalone.
2. **Lazy load all features** — Never import a feature directly in `app.routes.ts`; always use `loadChildren`.
3. **One folder per component** — Each component has its own folder with `.ts`, `.html`, `.css`, and an optional `index.ts` barrel file.
4. **Feature components stay in the feature** — If a component is only used in one feature, it lives inside that feature's `components/` folder.
