
🧩 Jalsa – Phase 2: Core Architecture Setup Expanded Implementation Handbook · 8 Tasks · 40+ Subtasks
=====================================================================================================

🧩 Phase 2 – Core Architecture Setup
------------------------------------

**Purpose:** Build the core infrastructure of the application — API layer, model definitions, guards, interceptors, and foundational state management. This phase creates the shared services and utilities that every feature will depend on, ensuring consistency, security, and maintainability across the entire frontend.

📋 Tasks: 8 ⏱️ Total Effort: ~28 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 1 (Foundation Setup) must be complete 🎯 Deliverable: Complete core module with API layer, models, guards, interceptors, and state services

## FE-CORE-001API Layer – Endpoints & HTTP Client P0 High 6h ▾

### Task Information

*   **Task ID:** FE-CORE-001
*   **Task Name:** API Layer – Endpoints & HTTP Client
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-002 (Environment), FE-SETUP-004 (Folder structure)
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Create the central API layer: define all endpoint constants (`api-endpoints.ts`) and a base HTTP client service (`http-client.service.ts`) that wraps Angular's `HttpClient` with consistent error handling, environment configuration, and authentication headers.

### Business Purpose

Centralising API communication ensures that all backend calls are consistent, easy to update, and secure. It reduces duplication and makes the codebase more maintainable.

### Technical Purpose

Provide a typed, observable-based HTTP client that automatically uses the correct base URL from environment, includes authentication tokens (via interceptors later), and standardises error handling.

### Prerequisites

#### Concepts Required

*   Angular `HttpClient` and its methods (`get`, `post`, etc.)
*   Observable and RxJS operators (`pipe`, `catchError`, `throwError`)
*   Environment files and injection of environment variables
*   TypeScript interfaces and generics

#### Access Required

*   API contract document (from Phase 0) listing all endpoints.

### Dependencies

*   **FE-SETUP-002:** Environment files provide `apiUrl`.
*   **FE-SETUP-004:** Folder structure `core/api` exists.

### Inputs

*   List of all API endpoints and their methods (from API contract).
*   Environment configuration.

### Outputs

*   `core/api/api-endpoints.ts` – constants for all endpoints.
*   `core/api/http-client.service.ts` – base HTTP service.

### Detailed Workflow

#### Step 1: Create `api-endpoints.ts`

**What to do:** Define all API routes as constants, using a hierarchical structure and functions for dynamic segments.

**Why it is required:** Centralised endpoint definitions make it easy to update URLs when the backend changes.

**Expected result:** A complete and typed API endpoint object.


```typescript
// core/api/api-endpoints.ts
export const API = {
    auth: {
        login: '/auth/login',
        register: '/auth/register',
        refresh: '/auth/refresh',
        logout: '/auth/logout',
        profile: '/auth/profile',
    },
    patients: {
        base: '/patients',
        byId: (id: string) => `/patients/${id}`,
        archive: (id: string) => `/patients/${id}/archive`,
        restore: (id: string) => `/patients/${id}/restore`,
        intake: (id: string) => `/patients/${id}/intake`,
        intakeImage: (id: string) => `/patients/${id}/intake/image`,
        assessments: (id: string) => `/patients/${id}/assessments`,
    },
    sessions: {
        base: '/sessions',
        byPatient: (patientId: string) => `/sessions/patient/${patientId}`,
        byId: (id: string) => `/sessions/${id}`,
        voice: (id: string) => `/sessions/${id}/voice`,
        summary: (id: string) => `/sessions/${id}/summary`,
    },
    exercises: {
        base: '/exercises',
        assign: '/exercises/assign',
        byPatient: (patientId: string) => `/exercises/patient/${patientId}`,
        status: (id: string) => `/exercises/${id}/status`,
    },
    reports: {
        base: '/reports',
        generate: '/reports/generate',
        byId: (id: string) => `/reports/${id}`,
        approve: (id: string) => `/reports/${id}/approve`,
        reject: (id: string) => `/reports/${id}/reject`,
        export: (id: string) => `/reports/${id}/export`,
    },
    dashboard: {
        stats: '/dashboard/stats',
        trends: '/dashboard/trends',
    },
    chat: {
        history: (sessionId: string) => `/chat/${sessionId}/history`,
        send: '/chat/send',
    },
} as const;
```
**Note:** Use `as const` to ensure literal types.

#### Step 2: Create `http-client.service.ts`

**What to do:** Create a service that wraps `HttpClient` and automatically uses the base API URL from environment.

**Why it is required:** This abstraction ensures all HTTP calls use the correct base URL and can be extended with global logic.

```typescript
// core/api/http-client.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class HttpClientService {
    private baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) {}

    get<T>(endpoint: string, params?: HttpParams | { [param: string]: string | number | boolean | readonly (string | number | boolean)[] }): Observable<T> {
        return this.http.get<T>(`${this.baseUrl}${endpoint}`, { params })
            .pipe(catchError(this.handleError));
    }

    post<T>(endpoint: string, body: any | null, options?: { headers?: HttpHeaders }): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}${endpoint}`, body, options)
            .pipe(catchError(this.handleError));
    }

    put<T>(endpoint: string, body: any): Observable<T> {
        return this.http.put<T>(`${this.baseUrl}${endpoint}`, body)
            .pipe(catchError(this.handleError));
    }

    patch<T>(endpoint: string, body: any): Observable<T> {
        return this.http.patch<T>(`${this.baseUrl}${endpoint}`, body)
            .pipe(catchError(this.handleError));
    }

    delete<T>(endpoint: string): Observable<T> {
        return this.http.delete<T>(`${this.baseUrl}${endpoint}`)
            .pipe(catchError(this.handleError));
    }

    upload<T>(endpoint: string, formData: FormData): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}${endpoint}`, formData)
            .pipe(catchError(this.handleError));
    }

    private handleError(error: any): Observable<never> {
        // Centralised error handling – can be extended later
        console.error('HTTP Error:', error);
        return throwError(() => error);
    }
}
```
#### Step 3: Export API Barrel

Create `core/api/index.ts` to export everything from the API layer.

```typescript
// core/api/index.ts
export * from './api-endpoints';
export * from './http-client.service';
```
### Folder Structure

```
src/app/core/api/
├── api-endpoints.ts
├── http-client.service.ts
└── index.ts
```

### Files To Create

*   `core/api/api-endpoints.ts`
*   `core/api/http-client.service.ts`
*   `core/api/index.ts`

### CLI Commands

```bash
# Generate HTTP client service (optional, we can create manually)
ng g s core/api/http-client --skip-tests
# Then manually edit and add api-endpoints.ts
```

### Code Flow
```
Component → Feature Service → HttpClientService → HttpClient → Backend
```
### API Interaction Flow
```
API Endpoint Constant → HttpClientService.get(API.patients.base) → Observable<Patient\[\]>
``` 
### Concepts Required

*   HTTP client and observables
*   Environment variables
*   TypeScript generics
*   Error handling with RxJS

### Tools/Libraries Required

*   Angular HttpClient

### Angular Concepts Required

*   Dependency injection
*   Singleton services
*   Observable and `pipe`

### RxJS Concepts Required

*   `catchError`
*   `throwError`
*   Observable creation

### Security Considerations

*   The `HttpClientService` does not yet include authentication; that will be added via interceptors.
*   Never log sensitive data in error handling.

### Testing Steps

1.  Unit test `HttpClientService` with mock `HttpClient`.
2.  Verify that the base URL is correctly appended.
3.  Test error handling by simulating a failed request.

### Expected Deliverables

*   API endpoints file with all routes.
*   Base HTTP client service.
*   Barrel export.

### Acceptance Criteria

*   All endpoints from the API contract are defined.
*   `HttpClientService` methods work and return observables.
*   Error handling catches and rethrows errors.

### Common Mistakes

**Mistake 1:** Hardcoding the API base URL instead of using environment.  
**Fix:** Always use `environment.apiUrl`.

**Mistake 2:** Not using generics, losing type safety.  
**Fix:** Always specify `<T>` for response types.

### Edge Cases

*   If `environment.apiUrl` is empty, the service will call relative URLs; ensure it's set.
*   For uploads, the `upload` method uses `post` with `FormData`.

### Definition of Done

*   Both files are created and pass linting and tests.
*   All endpoints are defined.

### Frontend Architecture Notes

*   This layer is the single point of contact for all backend communication.
*   All feature services will depend on `HttpClientService`.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Always import `HttpClientService` from `@core/api` for API calls.

## FE-CORE-002Core Models (DTO Interfaces) P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-CORE-002
*   **Task Name:** Core Models (DTO Interfaces)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001 (API endpoints)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Define TypeScript interfaces for all Data Transfer Objects (DTOs) used in API requests and responses, based on the API contract. These models will be used across the application to ensure type safety and consistency.

### Business Purpose

Type‑safe models prevent bugs caused by mismatched data shapes, improve developer experience with autocompletion, and serve as documentation.

### Technical Purpose

Create a set of interfaces that represent the core entities: User, Patient, Session, Exercise, Report, ChatMessage, etc. These will be used in services and components.

### Prerequisites

*   API contract defines the structure of each entity.
*   Understanding of TypeScript interfaces and type aliases.

### Dependencies

*   **FE-CORE-001:** API endpoints exist; models complement them.

### Inputs

*   API contract (OpenAPI/Swagger or document) with DTO definitions.

### Outputs

*   Model files under `core/models/`.
*   Barrel export `core/models/index.ts`.

### Detailed Workflow

#### Step 1: Review API Contract DTOs

**What to do:** Extract all DTOs from the API contract, including request and response types.

**Why it is required:** Models must accurately reflect what the backend sends and receives.

**Expected result:** A list of DTOs with fields and types.

#### Step 2: Create Model Files

**What to do:** For each logical group, create a file (e.g., `user.model.ts`, `patient.model.ts`).

**Why it is required:** Grouping by domain improves organisation.

```typescript
// core/models/user.model.ts
export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
    createdAt: string; // ISO date string
    updatedAt: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    role: string;
}
```
```typescript
// core/models/patient.model.ts
export interface Patient {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    gender: 'Male' | 'Female' | 'Other';
    emergencyContact: string | null;
    notes: string | null;
    isArchived: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface CreatePatientRequest {
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    gender: 'Male' | 'Female' | 'Other';
    emergencyContact?: string;
    notes?: string;
}

export interface UpdatePatientRequest {
    firstName?: string;
    lastName?: string;
    email?: string;
    dateOfBirth?: string;
    gender?: 'Male' | 'Female' | 'Other';
    emergencyContact?: string;
    notes?: string;
}

export interface IntakeForm {
    id: string;
    patientId: string;
    reasonForVisit: string;
    medicalHistory: string;
    medications: string;
    allergies: string;
    otherInfo: string | null;
    imageUrl: string | null;
    extractedData: any | null;
    createdAt: string;
    updatedAt: string;
}

export interface Assessment {
    id: string;
    patientId: string;
    type: string; // e.g., 'PHQ-9', 'GAD-7'
    score: number;
    date: string;
    notes: string | null;
}

export interface AssessmentRequest {
    type: string;
    score: number;
    date: string;
    notes?: string;
}
```
```typescript
// core/models/session.model.ts
export interface Session {
    id: string;
    patientId: string;
    therapistId: string;
    date: string;
    content: string; // rich text
    status: 'Draft' | 'Completed' | 'Archived';
    voiceMemoUrl: string | null;
    aiSummary: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface CreateSessionRequest {
    patientId: string;
    date: string;
    content: string;
}

export interface UpdateSessionRequest {
    date?: string;
    content?: string;
    status?: 'Draft' | 'Completed' | 'Archived';
}
```
```typescript
// core/models/exercise.model.ts
export interface Exercise {
    id: string;
    name: string;
    description: string;
    category: string;
}

export interface ExerciseAssignment {
    id: string;
    patientId: string;
    exerciseId: string;
    exercise: Exercise;
    dueDate: string;
    status: 'Pending' | 'InProgress' | 'Completed';
    reflection: string | null;
    assignedAt: string;
}

export interface AssignExerciseRequest {
    patientId: string;
    exerciseId: string;
    dueDate: string;
}

export interface UpdateExerciseStatusRequest {
    status: 'Pending' | 'InProgress' | 'Completed';
    reflection?: string;
}
```
```typescript
// core/models/report.model.ts
export interface Report {
    id: string;
    patientId: string;
    title: string;
    content: string;
    status: 'Draft' | 'PendingReview' | 'Approved' | 'Rejected';
    generatedBy: string;
    reviewedBy: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface GenerateReportRequest {
    patientId: string;
}

export interface UpdateReportRequest {
    title?: string;
    content?: string;
}
```
```typescript
// core/models/chat.model.ts
export interface ChatMessage {
    id: string;
    sessionId: string;
    senderId: string;
    senderName: string;
    content: string;
    timestamp: string;
    isFromAI: boolean;
}

export interface SendChatMessageRequest {
    sessionId: string;
    message: string;
}
```
```typescript
// core/models/dashboard.model.ts
export interface DashboardStats {
    totalPatients: number;
    activePatients: number;
    totalSessions: number;
    completedSessions: number;
    pendingExercises: number;
    completedExercises: number;
    pendingReports: number;
}

export interface TrendData {
    date: string;
    value: number;
}
```
```typescript
// core/models/api-response.model.ts
export interface ApiResponse<T> {
    data: T;
    message: string;
    success: boolean;
    errors?: string[];
}
```






#### Step 3: Create Barrel Export

```typescript
// core/models/index.ts
export * from './user.model';
export * from './patient.model';
export * from './session.model';
export * from './exercise.model';
export * from './report.model';
export * from './chat.model';
export * from './dashboard.model';
export * from './api-response.model';
```

### Files To Create

*   `core/models/user.model.ts`
*   `core/models/patient.model.ts`
*   `core/models/session.model.ts`
*   `core/models/exercise.model.ts`
*   `core/models/report.model.ts`
*   `core/models/chat.model.ts`
*   `core/models/dashboard.model.ts`
*   `core/models/api-response.model.ts`
*   `core/models/index.ts`

### CLI Commands

None; manual creation.

### Concepts Required

*   TypeScript interfaces and types
*   Union types
*   Optional properties

### Tools/Libraries Required

*   None

### Testing Steps

1.  No runtime tests; but ensure TypeScript compiles without errors.
2.  Use the models in a dummy service to verify they align with mock data.

### Expected Deliverables

*   Complete set of model interfaces.

### Common Mistakes

**Mistake 1:** Using `any` for complex fields instead of defining proper types.  
**Fix:** Always define detailed interfaces.

**Mistake 2:** Mismatched field names with backend.  
**Fix:** Verify with the API contract or Swagger schema.

### Edge Cases

*   Dates: use ISO string (`string`) and convert to `Date` in services if needed.
*   Nullable fields: use `null` or `undefined` appropriately.

### Definition of Done

*   All model files created and exported.
*   No TypeScript errors.

### Frontend Architecture Notes

*   These models are used throughout the application; keep them in sync with backend changes.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Always import models from `@core/models` to maintain type safety.

## FE-CORE-003Core Services (Notification, Loading, etc.) P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-CORE-003
*   **Task Name:** Core Services (Notification, Loading, etc.)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001, FE-CORE-002
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Create core application‑wide services: `NotificationService` for user feedback (toasts/modals), `LoadingService` for global loading indicator, and `AppStateService` for global UI state (theme, etc.). These will be used throughout the app to provide consistent user experience.

### Business Purpose

Users need clear feedback for actions (success, error, loading). A consistent notification system improves usability and reduces confusion.

### Technical Purpose

Centralise UI feedback logic so that components don't have to implement their own toasts or loading indicators. Use Angular Signals for reactive state.

### Prerequisites

*   Understanding of Angular Services and Dependency Injection.
*   Familiarity with Signals (for `AppStateService`).

### Dependencies

*   **FE-SETUP-003:** Bootstrap is installed (for styling toasts).

### Inputs

*   UI design for notification types (success, error, warning, info).

### Outputs

*   `core/services/notification.service.ts`
*   `core/services/loading.service.ts`
*   `core/services/app-state.service.ts`
*   Barrel export `core/services/index.ts`

### Detailed Workflow

#### Step 1: Create `NotificationService`

**What to do:** Implement a service that can display toast notifications (using Bootstrap's toast component) or a snackbar.

**Why it is required:** Components can show notifications without knowing how they are rendered.

```typescript
// core/services/notification.service.ts
import { Injectable, signal } from '@angular/core';

export interface Notification {
    id: string;
    message: string;
    type: 'success' | 'error' | 'warning' | 'info';
    duration?: number; // in milliseconds
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
    private notificationsSignal = signal<Notification[]>([]);
    readonly notifications = this.notificationsSignal.asReadonly();

    show(message: string, type: Notification['type'] = 'info', duration: number = 5000) {
        const id = Date.now().toString(36) + Math.random().toString(36).slice(2);
        const notification: Notification = { id, message, type, duration };
        this.notificationsSignal.update(list => [...list, notification]);

        if (duration > 0) {
            setTimeout(() => this.dismiss(id), duration);
        }
    }

    success(message: string, duration?: number) {
        this.show(message, 'success', duration);
    }

    error(message: string, duration?: number) {
        this.show(message, 'error', duration);
    }

    warning(message: string, duration?: number) {
        this.show(message, 'warning', duration);
    }

    info(message: string, duration?: number) {
        this.show(message, 'info', duration);
    }

    dismiss(id: string) {
        this.notificationsSignal.update(list => list.filter(n => n.id !== id));
    }

    clear() {
        this.notificationsSignal.set([]);
    }
}
```
#### Step 2: Create `LoadingService`

**What to do:** Manage a global loading state.

**Why it is required:** A global spinner/overlay can be shown during HTTP requests.

```typescript
// core/services/loading.service.ts
import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
    private loadingSignal = signal<boolean>(false);
    readonly isLoading = this.loadingSignal.asReadonly();

    private loadingCount = 0;

    show() {
        this.loadingCount++;
        if (this.loadingCount === 1) {
            this.loadingSignal.set(true);
        }
    }

    hide() {
        this.loadingCount--;
        if (this.loadingCount <= 0) {
            this.loadingCount = 0;
            this.loadingSignal.set(false);
        }
    }

    reset() {
        this.loadingCount = 0;
        this.loadingSignal.set(false);
    }
}
```

#### Step 3: Create `AppStateService`

**What to do:** Manage global UI state like theme (RTL/LTR), sidebar visibility, etc. For now, keep it minimal.

```typescript
// core/services/app-state.service.ts
import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AppStateService {
    private themeSignal = signal<'light' | 'dark'>('light');
    readonly theme = this.themeSignal.asReadonly();

    private sidebarCollapsedSignal = signal<boolean>(false);
    readonly sidebarCollapsed = this.sidebarCollapsedSignal.asReadonly();

    toggleTheme() {
        this.themeSignal.update(current => current === 'light' ? 'dark' : 'light');
    }

    toggleSidebar() {
        this.sidebarCollapsedSignal.update(collapsed => !collapsed);
    }

    setSidebarCollapsed(collapsed: boolean) {
        this.sidebarCollapsedSignal.set(collapsed);
    }
}
```
#### Step 4: Create Barrel Export

```typescript
// core/services/index.ts
export * from './notification.service';
export * from './loading.service';
export * from './app-state.service';
```
### Files To Create

*   `core/services/notification.service.ts`
*   `core/services/loading.service.ts`
*   `core/services/app-state.service.ts`
*   `core/services/index.ts`

### CLI Commands

```bash
ng g s core/services/notification --skip-tests
ng g s core/services/loading --skip-tests
ng g s core/services/app-state --skip-tests
```

Then manually edit to match the code above.

### Concepts Required

*   Angular Signals
*   Service singletons

### Tools/Libraries Required

*   Bootstrap for styling (optional, but we'll use for toasts later).

### Angular Concepts Required

*   `providedIn: 'root'`
*   Signal API (`signal`, `asReadonly`, `update`, `set`)

### Testing Steps

1.  Test `NotificationService` by calling `success()` and verifying the signal updates.
2.  Test `LoadingService` by calling `show()` and `hide()` and checking `isLoading`.

### Expected Deliverables

*   Three core services.

### Common Mistakes

**Mistake 1:** Not using Signals for state, leading to manual change detection.  
**Fix:** Use Signals for reactive UI state.

**Mistake 2:** Forgetting to clean up loading count, causing loading to stay visible.  
**Fix:** Ensure `hide()` decrements correctly.

### Definition of Done

*   All services are implemented and exported.
*   They can be injected and used in components.

### Frontend Architecture Notes

*   These services form the foundation of user experience feedback.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `NotificationService` for all user messages, `LoadingService` for global loading.

## FE-CORE-004Functional Guards (Auth & Role) P0 High 4h ▾

### Task Information

*   **Task ID:** FE-CORE-004
*   **Task Name:** Functional Guards (Auth & Role)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-003 (AppState, but AuthService will be added later; we'll plan for it)
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Create functional route guards (`CanActivateFn`) for authentication (`authGuard`) and role‑based access (`roleGuard`). These guards will prevent unauthenticated users from accessing protected routes and restrict access based on user roles.

### Business Purpose

Ensure that only logged‑in users can access application features and that therapists cannot access admin areas, etc.

### Technical Purpose

Use Angular's functional guard API (`CanActivateFn`) with dependency injection (`inject`) to check authentication status and user roles. Redirect to login or forbidden page if conditions are not met.

### Prerequisites

*   Understanding of Angular functional guards.
*   Knowledge of `inject` and `Router`.
*   We assume `AuthService` will be created in the Authentication phase; we'll write guards that rely on that service. For now, we'll create the guards and import the service (will be available later).

### Dependencies

*   **Phase 5:** AuthService will provide `isAuthenticated()` and `hasRole()`.

### Inputs

*   Expected AuthService methods: `isAuthenticated()`, `hasRole(role)`.

### Outputs

*   `core/guards/auth.guard.ts`
*   `core/guards/role.guard.ts`
*   `core/guards/index.ts`

### Detailed Workflow

#### Step 1: Create `auth.guard.ts`

**What to do:** Implement a functional guard that checks if the user is authenticated.

```typescript
// core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (authService.isAuthenticated()) {
        return true;
    }

    // Redirect to login and store the attempted URL
    router.navigate(['/auth/login'], { queryParams: { returnUrl: router.url } });
    return false;
};
```
#### Step 2: Create `role.guard.ts`

**What to do:** Implement a higher‑order guard that checks if the user has at least one of the allowed roles.

```typescript
// core/guards/role.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
    return () => {
        const authService = inject(AuthService);
        const router = inject(Router);

        if (allowedRoles.some(role => authService.hasRole(role))) {
            return true;
        }

        // Redirect to a forbidden page or home
        router.navigate(['/forbidden']);
        return false;
    };
};
```
#### Step 3: Create Barrel Export

```typescript
// core/guards/index.ts
export * from './auth.guard';
export * from './role.guard';
```
### Files To Create

*   `core/guards/auth.guard.ts`
*   `core/guards/role.guard.ts`
*   `core/guards/index.ts`

### CLI Commands

```bash
# Generate functional guards
ng g guard core/guards/auth --functional
ng g guard core/guards/role --functional
```
Then replace the generated code with the implementation above.

### Concepts Required

*   Functional guards (`CanActivateFn`)
*   Dependency injection with `inject()`
*   Router navigation

### Tools/Libraries Required

*   Angular Router

### Angular Concepts Required

*   Route guards
*   ActivatedRoute and Router

### Testing Steps

1.  Write unit tests for guards using mock AuthService and Router.
2.  Test authenticated and unauthenticated scenarios.
3.  Test role guard with allowed and disallowed roles.

### Expected Deliverables

*   Two functional guards.

### Common Mistakes

**Mistake 1:** Using class‑based guards instead of functional.  
**Fix:** Ensure `--functional` flag is used during generation.

**Mistake 2:** Forgetting to inject dependencies with `inject()`.  
**Fix:** Use `inject(AuthService)` inside the guard function.

### Edge Cases

*   If AuthService is not yet implemented, the guards will fail. This is expected; they will be integrated later.

### Definition of Done

*   Guards are implemented and exported.
*   They can be used in route configurations.

### Frontend Architecture Notes

*   Guards are functional to align with Angular 17+ best practices.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Apply guards to routes to protect them.

## FE-CORE-005Interceptors (Auth, Error, Loading) P0 High 6h ▾

### Task Information

*   **Task ID:** FE-CORE-005
*   **Task Name:** Interceptors (Auth, Error, Loading)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001 (HttpClientService), FE-CORE-003 (LoadingService, NotificationService), FE-CORE-004 (AuthService – but will be integrated later)
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Create functional HTTP interceptors to handle authentication token injection, global error handling, and loading state management.

### Business Purpose

Interceptors centralise cross‑cutting concerns: adding JWT tokens to requests, displaying error messages to users, and showing a global loading indicator during network activity.

### Technical Purpose

Use `HttpInterceptorFn` (functional interceptors) to intercept all HTTP requests and responses, applying consistent logic without cluttering feature services.

### Prerequisites

*   Understanding of HTTP interceptors in Angular.
*   Knowledge of RxJS operators (`catchError`, `finalize`, `throwError`).

### Dependencies

*   **FE-CORE-003:** LoadingService and NotificationService.
*   **AuthService:** Will be created in Phase 5; we'll inject it and handle missing token gracefully.

### Inputs

*   AuthService (expected methods: `getToken()`, `logout()`).

### Outputs

*   `core/interceptors/auth.interceptor.ts`
*   `core/interceptors/error.interceptor.ts`
*   `core/interceptors/loading.interceptor.ts`
*   `core/interceptors/index.ts`

### Detailed Workflow

#### Step 1: Create `auth.interceptor.ts`

**What to do:** Add the JWT token to the Authorization header if available.

```typescript
// core/interceptors/auth.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const token = authService.getToken();

    if (token) {
        const clonedReq = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`,
            },
        });
        return next(clonedReq);
    }

    return next(req);
};
```

#### Step 2: Create `error.interceptor.ts`

**What to do:** Catch HTTP errors and display appropriate notifications, handle 401 (unauthorized) by logging out.

```typescript
// core/interceptors/error.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const notification = inject(NotificationService);
    const authService = inject(AuthService);
    const router = inject(Router);

    return next(req).pipe(
        catchError((error) => {
            // Log error for debugging
            console.error('HTTP Error:', error);

            if (error.status === 401) {
                // Unauthorized – log out and redirect to login
                authService.logout();
                router.navigate(['/auth/login']);
                notification.error('Your session has expired. Please log in again.');
            } else if (error.status === 403) {
                notification.error('You do not have permission to perform this action.');
            } else if (error.status === 400) {
                // Bad request – show the error message from the server
                const message = error.error?.message || 'Invalid request. Please check your input.';
                notification.error(message);
            } else if (error.status === 500) {
                notification.error('A server error occurred. Please try again later.');
            } else {
                notification.error('An unexpected error occurred. Please try again.');
            }

            return throwError(() => error);
        })
    );
};
```
#### Step 3: Create `loading.interceptor.ts`

**What to do:** Show/hide the global loading indicator for each request.

```typescript
// core/interceptors/loading.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
    const loadingService = inject(LoadingService);

    // Skip loading for certain requests (e.g., refresh token)
    if (req.url.includes('/refresh')) {
        return next(req);
    }

    loadingService.show();

    return next(req).pipe(
        finalize(() => loadingService.hide())
    );
};
```
#### Step 4: Create Barrel Export

```typescript
// core/interceptors/index.ts
export * from './auth.interceptor';
export * from './error.interceptor';
export * from './loading.interceptor';
```

#### Step 5: Register Interceptors in `app.config.ts`

**What to do:** Add the interceptors using `withInterceptors()` in `provideHttpClient`.

```typescript
// src/app/app.config.ts
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { authInterceptor, errorInterceptor, loadingInterceptor } from './core/interceptors';

export const appConfig: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideHttpClient(
            withInterceptors([authInterceptor, errorInterceptor, loadingInterceptor])
        ),
        provideAnimations(),
        // ... other providers
    ],
};
```
### Files To Create

*   `core/interceptors/auth.interceptor.ts`
*   `core/interceptors/error.interceptor.ts`
*   `core/interceptors/loading.interceptor.ts`
*   `core/interceptors/index.ts`

### CLI Commands

```bash
# Generate functional interceptors
ng g interceptor core/interceptors/auth --functional
ng g interceptor core/interceptors/error --functional
ng g interceptor core/interceptors/loading --functional
```

Then replace with the code above.

### Concepts Required

*   HttpInterceptorFn
*   RxJS operators
*   Dependency injection with `inject`

### Tools/Libraries Required

*   Angular HttpClient

### Angular Concepts Required

*   HTTP interceptors
*   Functional interceptors (Angular 15+)

### RxJS Concepts Required

*   `catchError`, `finalize`, `throwError`

### Testing Steps

1.  Unit test each interceptor with mock `HttpHandler`.
2.  Test auth interceptor adds token when present.
3.  Test error interceptor shows notification and handles 401.
4.  Test loading interceptor shows/hides loading.

### Expected Deliverables

*   Three interceptors registered in app.config.

### Common Mistakes

**Mistake 1:** Not registering interceptors in `app.config.ts`.  
**Fix:** Add `withInterceptors([...])` to `provideHttpClient`.

**Mistake 2:** Forgetting to clone the request before modifying headers.  
**Fix:** Always clone the request in auth interceptor.

### Edge Cases

*   Refresh token request should not trigger auth interceptor (to avoid infinite loops).
*   If token is expired, 401 triggers logout; ensure `logout()` does not cause infinite redirect.

### Definition of Done

*   Interceptors implemented and registered.
*   HTTP requests include token, show loading, and handle errors.

### Frontend Architecture Notes

*   Interceptors are the single point for cross‑cutting concerns.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Interceptors automatically handle token injection and errors; no need to duplicate logic in services.

## FE-CORE-006State Management Foundation P0 High 4h ▾

### Task Information

*   **Task ID:** FE-CORE-006
*   **Task Name:** State Management Foundation
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-003 (AppStateService already created)
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Establish the state management strategy by creating a framework for feature‑specific state services using Signals, and provide a base `StateService` base class or pattern for consistency.

### Business Purpose

Predictable state management reduces bugs and makes the application easier to debug and extend.

### Technical Purpose

Provide a set of utilities and patterns for managing state with Signals, including a base class for CRUD operations (e.g., loading, error, data).

### Prerequisites

*   Understanding of Angular Signals.
*   RxJS for asynchronous operations.

### Dependencies

*   **FE-CORE-003:** AppStateService is available.

### Inputs

*   None.

### Outputs

*   `core/state/base-state.service.ts` (optional base class).
*   `core/state/state-utils.ts` (helper functions).
*   `core/state/index.ts`.

### Detailed Workflow

#### Step 1: Create a Base State Service (Optional)

**What to do:** Provide a base class that wraps common patterns: loading, error, data signals.

```typescript
// core/state/base-state.service.ts
import { signal, WritableSignal } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';

export class BaseStateService<T> {
    protected dataSignal: WritableSignal<T | null> = signal(null);
    protected loadingSignal: WritableSignal<boolean> = signal(false);
    protected errorSignal: WritableSignal<string | null> = signal(null);

    readonly data = this.dataSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    protected setData(data: T) {
        this.dataSignal.set(data);
        this.errorSignal.set(null);
    }

    protected setError(error: string) {
        this.errorSignal.set(error);
    }

    protected startLoading() {
        this.loadingSignal.set(true);
        this.errorSignal.set(null);
    }

    protected stopLoading() {
        this.loadingSignal.set(false);
    }

    protected handleObservable<R>(obs: Observable<R>, onSuccess: (data: R) => void): Observable<R> {
        this.startLoading();
        return obs.pipe(
            catchError((err) => {
                this.setError(err.message || 'An error occurred');
                this.stopLoading();
                throw err;
            }),
            finalize(() => {
                this.stopLoading();
            })
        );
    }
}
```
**Note:** This base class is optional; teams can use it or implement their own state services.

#### Step 2: Create State Utilities

**What to do:** Provide helper functions for common state operations.

```typescript
// core/state/state-utils.ts
import { signal, WritableSignal } from '@angular/core';

export function createState<T>(initialValue: T) {
    const state = signal(initialValue);
    return {
        state: state.asReadonly(),
        set: (value: T) => state.set(value),
        update: (fn: (current: T) => T) => state.update(fn),
        reset: () => state.set(initialValue),
    };
}

export function createAsyncState<T>() {
    const data = signal<T | null>(null);
    const loading = signal(false);
    const error = signal<string | null>(null);

    return {
        data: data.asReadonly(),
        loading: loading.asReadonly(),
        error: error.asReadonly(),
        setData: (value: T) => {
            data.set(value);
            error.set(null);
        },
        setError: (err: string) => {
            error.set(err);
            data.set(null);
        },
        startLoading: () => loading.set(true),
        stopLoading: () => loading.set(false),
        reset: () => {
            data.set(null);
            loading.set(false);
            error.set(null);
        },
    };
}
```
#### Step 3: Create Barrel Export

```typescript
// core/state/index.ts
export * from './base-state.service';
export * from './state-utils';
```
### Files To Create

*   `core/state/base-state.service.ts` (optional)
*   `core/state/state-utils.ts`
*   `core/state/index.ts`

### CLI Commands

None; manual creation.

### Concepts Required

*   Angular Signals
*   RxJS Observables
*   Generics

### Tools/Libraries Required

*   None

### Testing Steps

1.  Test the `createState` and `createAsyncState` helpers with sample usage.

### Expected Deliverables

*   State utilities and base class.

### Common Mistakes

**Mistake 1:** Overcomplicating state with unnecessary abstractions.  
**Fix:** Keep it simple; use services with Signals directly if preferred.

### Definition of Done

*   State management foundation is available.

### Frontend Architecture Notes

*   This provides a consistent pattern but does not force a specific implementation.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `createAsyncState` for feature state or inherit from `BaseStateService`.

## FE-CORE-007Provide Core Services in App Config P0 Medium 2h ▾

### Task Information

*   **Task ID:** FE-CORE-007
*   **Task Name:** Provide Core Services in App Config
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous FE-CORE tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Critical

### Objective

Ensure all core services, interceptors, and guards are properly provided in the application configuration so they can be injected everywhere.

### Business Purpose

All core dependencies must be available for the application to function.

### Technical Purpose

Update `app.config.ts` to include necessary providers (some are already provided via `providedIn: 'root'`, but interceptors need explicit registration).

### Prerequisites

*   All core services and interceptors are created.

### Dependencies

*   All previous tasks.

### Inputs

*   List of providers to register.

### Outputs

*   Updated `src/app/app.config.ts`.

### Detailed Workflow

#### Step 1: Review Current Providers

Ensure `provideHttpClient` includes interceptors.

#### Step 2: Add Any Additional Providers

For example, if we want to provide a global error handler or a custom logger, we can add it here.

At this stage, most services are provided via `providedIn: 'root'`, so no extra registration is needed.

### Files To Modify

*   `src/app/app.config.ts` (already updated in interceptor task).

### CLI Commands

None.

### Testing Steps

1.  Run the application and verify that services can be injected without errors.

### Expected Deliverables

*   Application configuration is complete.

### Common Mistakes

**Mistake 1:** Forgetting to import `provideHttpClient` and interceptors.  
**Fix:** Ensure the imports are present.

### Definition of Done

*   All providers are correctly configured.

### Frontend Architecture Notes

*   This completes the core architecture setup.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** The core is now ready; next phases will build features on top.

## FE-CORE-008Validate and Commit Core Architecture P0 Medium 2h ▾

### Task Information

*   **Task ID:** FE-CORE-008
*   **Task Name:** Validate and Commit Core Architecture
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous FE-CORE tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Critical

### Objective

Perform a final validation of the core architecture: run lint, build, and tests, then commit all changes to the repository.

### Business Purpose

Ensure the core infrastructure is stable before proceeding to feature development.

### Technical Purpose

Verify that all core components work together and are ready to be used by feature teams.

### Prerequisites

*   All core tasks completed.

### Dependencies

*   All FE-CORE tasks.

### Inputs

*   All code changes.

### Outputs

*   Successful build and tests.
*   Commit to repository.

### Detailed Workflow

#### Step 1: Run Lint

npm run lint

#### Step 2: Run Tests

ng test --watch=false

#### Step 3: Build

ng build ng build --configuration=production

#### Step 4: Commit

```bash
git add .
git commit -m "feat: core architecture setup complete

- API endpoints and HTTP client
- Core models (DTOs)
- Core services (Notification, Loading, AppState)
- Functional guards (auth, role)
- Interceptors (auth, error, loading)
- State management foundation
- All provided in app.config"


```
### Files To Create

None; validation only.

### CLI Commands

```bash
npm run lint
ng test --watch=false
ng build
ng build --configuration=production
git add .
git commit -m "feat: core architecture setup complete"

```

### Testing Steps

1.  Run all checks.
2.  Verify no errors.

### Expected Deliverables

*   Stable core codebase.

### Common Mistakes

**Mistake 1:** Not committing the lock file.  
**Fix:** Ensure `package-lock.json` is committed.

### Definition of Done

*   All checks pass and code is committed.

### Frontend Architecture Notes

*   The core is now complete and ready for feature development.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Pull the latest code and start building features on top of this core.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 8 core tasks are complete (FE-CORE-001 to FE-CORE-008).
*   API endpoints are defined and HTTP client service works.
*   All DTO interfaces are defined.
*   Core services (Notification, Loading, AppState) are implemented.
*   Functional guards (auth, role) are implemented.
*   Interceptors (auth, error, loading) are implemented and registered.
*   State management foundation is available.
*   All code passes lint and tests.
*   All changes are committed to the repository.

📋 Code Review Checklist
------------------------

*   API endpoints are correctly defined and typed.
*   Models match the API contract.
*   Services are provided in root and use Signals where appropriate.
*   Guards are functional and use `inject`.
*   Interceptors are functional and registered correctly.
*   State utilities are clear and reusable.

🚀 Pull Request Checklist
-------------------------

*   Branch is up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   Commit message follows conventional commits.

🧪 Deployment Readiness Checklist
---------------------------------

*   Not applicable for this phase (core only).

* * *

Jalsa – Phase 2: Core Architecture Setup – Expanded Implementation Handbook • v1.0 • For development team

