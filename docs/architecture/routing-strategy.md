# Routing Strategy

## Overview

Jalsa uses Angular's built-in router with **lazy loading for all features**. Routes are configured using functional guards (`CanActivateFn`) and standalone component routing patterns.

## Route Configuration

### Root Routes (`app.routes.ts`)

The root route file defines the top-level layout structure and lazy-loads all features:

```typescript
export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes').then(m => m.authRoutes),
    },
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: 'dashboard',
                loadChildren: () => import('./features/dashboard/dashboard.routes')
                    .then(m => m.dashboardRoutes),
            },
            {
                path: 'patients',
                loadChildren: () => import('./features/patients/patients.routes')
                    .then(m => m.patientsRoutes),
            },
            // ... other features
        ],
    },
    { path: '**', redirectTo: '/dashboard' },
];
```

### Feature Routes (`*.routes.ts`)

Each feature has its own route configuration file with child routes:

```typescript
// features/patients/patients.routes.ts
export const patientsRoutes: Routes = [
    {
        path: '',
        component: PatientListComponent,
    },
    {
        path: 'new',
        component: PatientFormComponent,
    },
    {
        path: ':id',
        component: PatientDetailComponent,
    },
    {
        path: ':id/edit',
        component: PatientFormComponent,
    },
    {
        path: ':id/intake',
        component: IntakeFormComponent,
    },
];
```

## Route Guards

- **Authentication Guard (`authGuard`):** Checks if the user is authenticated. Redirects to `/auth/login` if not.
- **Role Guard (`roleGuard`):** Factory function that returns a `CanActivateFn` checking for specific roles. Redirects to `/forbidden` if the user lacks permission.

```typescript
export const authGuard: CanActivateFn = () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    if (auth.isAuthenticated()) return true;
    return router.parseUrl('/auth/login');
};

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
    return () => {
        const auth = inject(AuthService);
        const router = inject(Router);
        if (auth.hasAnyRole(allowedRoles)) return true;
        return router.parseUrl('/forbidden');
    };
};
```

## Route Parameters and Data

| Purpose | Mechanism | Example |
|---|---|---|
| Entity IDs | Path params (`:id`) | `/patients/:id` |
| Search/filter | Query params (`?q=`) | `/patients?search=ahmed&page=2` |
| Static page config | Route `data` | `{ title: 'Patient List' }` |
| Pre-fetched data | Route resolvers | `{ resolve: { patient: patientResolver } }` |

## Lazy Loading Rules

1. **Every feature is lazy-loaded** — never import a feature module directly.
2. **Feature routes live in their own file** — `<feature>.routes.ts` inside the feature folder.
3. **Each page component is a route target** — pages are standalone components.
4. **Guards are applied at the route level** — never inside components.

## Planned Route Map

| Path | Feature | Guard | Description |
|---|---|---|---|
| `/auth/login` | Auth | — | Login page |
| `/auth/register` | Auth | — | Registration page |
| `/auth/reset-password` | Auth | — | Password reset |
| `/dashboard` | Dashboard | `authGuard` | Main dashboard |
| `/patients` | Patients | `authGuard` | Patient list |
| `/patients/:id` | Patients | `authGuard` | Patient details |
| `/patients/:id/edit` | Patients | `authGuard` | Edit patient |
| `/patients/:id/intake` | Patients | `authGuard` | Intake form |
| `/sessions` | Sessions | `authGuard` | Session list |
| `/sessions/new` | Sessions | `authGuard` | New session |
| `/sessions/:id` | Sessions | `authGuard` | Session detail |
| `/exercises` | Exercises | `authGuard` | Exercise library |
| `/exercises/assign` | Exercises | `authGuard` | Assign exercise |
| `/reports` | Reports | `authGuard` | Report list |
| `/reports/generate` | Reports | `authGuard` | Generate report |
| `/reports/:id` | Reports | `authGuard` | Report edit/detail |
| `/chat/:sessionId` | Chatbot | `authGuard` | Chat view |
| `/chat/monitor` | Chatbot | `authGuard`, `roleGuard(['Therapist'])` | Chat monitoring |
