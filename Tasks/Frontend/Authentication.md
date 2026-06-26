

🔐 Jalsa – Phase 5: Authentication Expanded Implementation Handbook · 7 Tasks · 35+ Subtasks
============================================================================================

🔐 Phase 5 – Authentication
---------------------------

**Purpose:** Build the complete authentication system for the Jalsa platform. This includes the AuthService with JWT handling and Signals, Login and Register components, Reset Password flow, Profile management, HTTP interceptors for token injection and error handling, and functional route guards. This phase secures the application and provides users with a seamless login and registration experience.

📋 Tasks: 7 ⏱️ Total Effort: ~32 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 2 (Core Architecture), Phase 3 (RTL & Design System), Phase 4 (Shared Components) 🎯 Deliverable: Complete authentication system with login, register, reset password, profile, guards, and interceptors

## FE-AUTH-001Auth Routes Setup (Standalone) P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-AUTH-001
*   **Task Name:** Auth Routes Setup (Standalone)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-004 (Folder structure), FE-SETUP-005 (App routing)
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Create the authentication feature routes with lazy loading, define the route structure for login, register, reset password, and profile, and set up the auth layout component that wraps all auth pages.

### Business Purpose

Authentication routes are the entry point to the application. They must be properly structured to handle all authentication flows and redirect unauthenticated users to login.

### Technical Purpose

Set up `features/auth/auth.routes.ts` with lazy loading, create a dedicated auth layout for pages that don't show the main sidebar, and register the routes in `app.routes.ts`.

### Prerequisites

*   Understanding of Angular lazy loading with Standalone components.
*   Knowledge of route definitions and child routes.

### Dependencies

*   **FE-SETUP-004:** `features/auth/` folder exists.
*   **FE-SETUP-005:** `app.routes.ts` is configured.

### Inputs

*   None.

### Outputs

*   `features/auth/auth.routes.ts` with full route configuration.
*   `shared/layouts/auth-layout/auth-layout.component.ts` (Standalone).
*   Updated `app.routes.ts` with lazy loading for auth.

### Detailed Workflow

#### Step 1: Create Auth Layout Component

**What to do:** Generate a layout component for authentication pages (login, register, etc.) that doesn't include the main sidebar.

**Why it is required:** Auth pages should be simple, centered, and not show the application sidebar.

```bash
ng g c shared/layouts/auth-layout --standalone --skip-tests
```
Template (`auth-layout.component.html`):

```html
<div class="auth-layout">
    <div class="auth-container">
        <div class="auth-brand">
            <h1>جلسة</h1>
            <p>Jalsa Platform</p>
        </div>
        <div class="auth-card">
            <ng-content></ng-content>
        </div>
        <div class="auth-footer">
            © {{ currentYear }} Jalsa. All rights reserved.
        </div>
    </div>
</div>
```
Styles (`auth-layout.component.scss`): Use flexbox to center content.

#### Step 2: Create Auth Routes File

**What to do:** Define routes for all auth pages.
File: `features/auth/auth.routes.ts`

```typescript
// features/auth/auth.routes.ts
import { Routes } from '@angular/router';
import { AuthLayoutComponent } from '../../shared/layouts/auth-layout/auth-layout.component';

// Lazy-loaded components (will be created in later tasks)
export const AUTH_ROUTES: Routes = [
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
            {
                path: 'login',
                loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent),
            },
            {
                path: 'register',
                loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent),
            },
            {
                path: 'forgot-password',
                loadComponent: () => import('./pages/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent),
            },
            {
                path: 'reset-password',
                loadComponent: () => import('./pages/reset-password/reset-password.component').then(m => m.ResetPasswordComponent),
            },
            {
                path: 'profile',
                loadComponent: () => import('./pages/profile/profile.component').then(m => m.ProfileComponent),
            },
            {
                path: '',
                redirectTo: 'login',
                pathMatch: 'full',
            },
        ],
    },
];
```
#### Step 3: Register Auth Routes in App Routes

Add lazy loading in `app.routes.ts`.
File: `app.routes.ts`


```typescript
// app.routes.ts
{
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
},
// Redirect root to auth
{
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full',
},
// Catch-all redirect to auth/login
{
    path: '**',
    redirectTo: 'auth/login',
},
```
#### Step 4: Create Placeholder Components

Generate placeholder components for all auth pages (they will be implemented in later tasks).

```bash
ng g c features/auth/pages/login --standalone --skip-tests
ng g c features/auth/pages/register --standalone --skip-tests
ng g c features/auth/pages/forgot-password --standalone --skip-tests
ng g c features/auth/pages/reset-password --standalone --skip-tests
ng g c features/auth/pages/profile --standalone --skip-tests
```
### Files To Create

*   `features/auth/auth.routes.ts`
*   `shared/layouts/auth-layout/auth-layout.component.ts`
*   `shared/layouts/auth-layout/auth-layout.component.html`
*   `shared/layouts/auth-layout/auth-layout.component.scss`
*   Placeholder components in `features/auth/pages/`.

### CLI Commands


```bash
ng g c features/auth/pages/login --standalone --skip-tests
ng g c features/auth/pages/register --standalone --skip-tests
ng g c features/auth/pages/forgot-password --standalone --skip-tests
ng g c features/auth/pages/reset-password --standalone --skip-tests
ng g c features/auth/pages/profile --standalone --skip-tests
```
### Concepts Required

*   Lazy loading with `loadChildren`
*   Standalone components
*   Child routes
*   Componentless routes

### Tools/Libraries Required

*   Angular Router

### Angular Concepts Required

*   `loadChildren` with `import()`
*   `loadComponent` for lazy loading Standalone components

### Testing Steps

1.  Run `ng serve` and navigate to `/auth/login`.
2.  Verify the auth layout renders.
3.  Check that lazy loading works (network tab).

### Expected Deliverables

*   Auth routes configured with lazy loading.
*   Auth layout component created.
*   Placeholder components generated.

### Common Mistakes

**Mistake 1:** Using `loadChildren` incorrectly for Standalone routes.  
**Fix:** Use `loadChildren: () => import('./path').then(m => m.MODULE_ROUTES)`.

**Mistake 2:** Forgetting to add the auth layout component to the route.  
**Fix:** Wrap all auth pages with `component: AuthLayoutComponent`.

### Edge Cases

*   If user is already authenticated, they should be redirected to dashboard instead of login.

### Definition of Done

*   Auth routes are configured and lazy‑loaded.
*   Auth layout renders correctly.
*   All placeholder components are created.

### Frontend Architecture Notes

*   Auth pages use a separate layout without the sidebar for a clean login experience.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Auth routes are at `/auth`. Add new auth pages as children.

## FE-AUTH-002Auth Service with Signals P0 High 6h ▾

### Task Information

*   **Task ID:** FE-AUTH-002
*   **Task Name:** Auth Service with Signals
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001 (HttpClientService), FE-CORE-002 (Models), FE-CORE-003 (NotificationService)
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Create the authentication service that handles login, register, logout, token storage, session restoration, and user state using Angular Signals. The service should be the single source of truth for authentication state.

### Business Purpose

Centralises all authentication logic, manages user session state, and provides reactive user data to components. This ensures consistent authentication behaviour across the application.

### Technical Purpose

Use `HttpClientService` for API calls, `jwt-decode` for token parsing, `localStorage` for token storage, and `signal` for reactive user state.

### Prerequisites

*   Understanding of Angular Signals.
*   Knowledge of JWT tokens and `jwt-decode` library.
*   Understanding of HttpClient and observables.

### Dependencies

*   **FE-CORE-001:** HttpClientService for API calls.
*   **FE-CORE-002:** User model interfaces.
*   **FE-CORE-003:** NotificationService for user feedback.

### Inputs

*   API endpoints from `api-endpoints.ts`.
*   User model interface.

### Outputs

*   `core/services/auth.service.ts` with full implementation.
*   Methods: `login()`, `register()`, `logout()`, `refreshToken()`, `getProfile()`, `updateProfile()`, `changePassword()`, `forgotPassword()`, `resetPassword()`.
*   Signal: `currentUser` (readonly) for reactive user state.
*   Methods: `isAuthenticated()`, `hasRole()`, `hasAnyRole()`, `getToken()`.

### Detailed Workflow

#### Step 1: Generate the Service

```bash
ng g s core/services/auth --skip-tests
```
#### Step 2: Install jwt-decode

```bash
npm install jwt-decode
```
#### Step 3: Implement AuthService
File: `core/services/auth.service.ts`


```typescript
// core/services/auth.service.ts
import { Injectable, signal, computed, effect } from '@angular/core';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { User, LoginRequest, LoginResponse, RegisterRequest, UpdateProfileRequest, ChangePasswordRequest, ForgotPasswordRequest, ResetPasswordRequest } from '../models';
import { jwtDecode } from 'jwt-decode';
import { NotificationService } from './notification.service';

interface DecodedToken {
    sub: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
    exp: number;
    iat: number;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private readonly TOKEN_KEY = 'jalsa_token';

    // Private signals
    private userSignal = signal<User | null>(null);
    private loadingSignal = signal<boolean>(false);

    // Public readonly signals
    readonly currentUser = this.userSignal.asReadonly();
    readonly isLoading = this.loadingSignal.asReadonly();

    // Computed: is authenticated
    readonly isAuthenticated = computed(() => this.userSignal() !== null && this.getToken() !== null);

    constructor(
        private http: HttpClientService,
        private notification: NotificationService,
    ) {
        // Restore session on app initialization
        this.restoreSession();

        // Effect to log user changes (optional)
        effect(() => {
            const user = this.userSignal();
            if (user) {
                console.log('User authenticated:', user.email);
            } else {
                console.log('User not authenticated');
            }
        });
    }

    // ============================================================
    // Public API Methods
    // ============================================================

    login(credentials: LoginRequest): Observable<LoginResponse> {
        this.loadingSignal.set(true);
        return this.http.post<LoginResponse>(API.auth.login, credentials).pipe(
            tap(response => {
                this.handleAuthentication(response.token);
                this.notification.success('Welcome back!');
            }),
            catchError((error) => {
                this.loadingSignal.set(false);
                return throwError(() => error);
            }),
        );
    }

    register(userData: RegisterRequest): Observable<any> {
        this.loadingSignal.set(true);
        return this.http.post(API.auth.register, userData).pipe(
            tap(() => {
                this.loadingSignal.set(false);
                this.notification.success('Account created successfully! Please log in.');
            }),
            catchError((error) => {
                this.loadingSignal.set(false);
                return throwError(() => error);
            }),
        );
    }

    logout(): void {
        localStorage.removeItem(this.TOKEN_KEY);
        this.userSignal.set(null);
        this.notification.info('You have been logged out.');
    }

    refreshToken(): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(API.auth.refresh, {}).pipe(
            tap(response => {
                this.handleAuthentication(response.token);
            }),
        );
    }

    getProfile(): Observable<User> {
        return this.http.get<User>(API.auth.profile);
    }

    updateProfile(data: UpdateProfileRequest): Observable<User> {
        return this.http.put<User>(API.auth.profile, data).pipe(
            tap(user => {
                this.userSignal.set(user);
                this.notification.success('Profile updated successfully!');
            }),
        );
    }

    changePassword(data: ChangePasswordRequest): Observable<any> {
        return this.http.post(API.auth.changePassword, data).pipe(
            tap(() => {
                this.notification.success('Password changed successfully!');
            }),
        );
    }

    forgotPassword(email: string): Observable<any> {
        return this.http.post(API.auth.forgotPassword, { email }).pipe(
            tap(() => {
                this.notification.success('Password reset link sent to your email.');
            }),
        );
    }

    resetPassword(data: ResetPasswordRequest): Observable<any> {
        return this.http.post(API.auth.resetPassword, data).pipe(
            tap(() => {
                this.notification.success('Password reset successfully! Please log in.');
            }),
        );
    }

    // ============================================================
    // Token & Session Management
    // ============================================================

    getToken(): string | null {
        return localStorage.getItem(this.TOKEN_KEY);
    }

    private setToken(token: string): void {
        localStorage.setItem(this.TOKEN_KEY, token);
    }

    private handleAuthentication(token: string): void {
        this.setToken(token);
        const user = this.decodeToken(token);
        if (user) {
            this.userSignal.set(user);
        }
        this.loadingSignal.set(false);
    }

    private decodeToken(token: string): User | null {
        try {
            const decoded = jwtDecode<DecodedToken>(token);
            return {
                id: decoded.sub,
                email: decoded.email,
                firstName: decoded.firstName,
                lastName: decoded.lastName,
                roles: decoded.roles || [],
            };
        } catch (error) {
            console.error('Failed to decode token:', error);
            return null;
        }
    }

    private restoreSession(): void {
        const token = this.getToken();
        if (token) {
            const user = this.decodeToken(token);
            if (user) {
                this.userSignal.set(user);
                // Optionally check token expiration here
            } else {
                this.logout();
            }
        }
    }

    // ============================================================
    // Role & Permission Checks
    // ============================================================

    hasRole(role: string): boolean {
        const user = this.userSignal();
        return user?.roles?.includes(role) ?? false;
    }

    hasAnyRole(roles: string[]): boolean {
        const user = this.userSignal();
        if (!user) return false;
        return roles.some(role => user.roles.includes(role));
    }

    // ============================================================
    // Utility
    // ============================================================

    getCurrentUser(): User | null {
        return this.userSignal();
    }
}
```
#### Step 4: Add to Models

Ensure `user.model.ts` includes all necessary request/response types.
File: `core/models/user.model.ts`


```typescript
// core/models/user.model.ts
export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
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
    role?: string;
}

export interface UpdateProfileRequest {
    firstName?: string;
    lastName?: string;
    email?: string;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
}

export interface ForgotPasswordRequest {
    email: string;
}

export interface ResetPasswordRequest {
    token: string;
    newPassword: string;
}
```
### Files To Create

*   `core/services/auth.service.ts`
*   Update `core/models/user.model.ts`.
*   Update `core/models/index.ts`.

### CLI Commands

```bash
ng g s core/services/auth --skip-tests npm install jwt-decode
```
### Code Flow

Component → AuthService.login() → HttpClientService.post() → API ↕ Token stored, user signal updated

### State Flow

Login successful → setToken() → decodeToken() → userSignal.set() → Components react via computed signals

### Concepts Required

*   Angular Signals (`signal`, `computed`, `effect`)
*   JWT tokens and decoding
*   RxJS operators (`tap`, `catchError`)
*   localStorage for client-side storage

### Tools/Libraries Required

*   jwt-decode
*   RxJS

### Security Considerations

*   Token stored in localStorage (XSS risk – mitigated by CSP).
*   Token expiration should be handled (refresh token logic).
*   Never log tokens in console.

### Testing Steps

1.  Unit test login: mock HttpClientService, verify token is stored.
2.  Unit test logout: verify token is removed and user signal is null.
3.  Test role checking methods.
4.  Test session restoration on app load.

### Expected Deliverables

*   AuthService fully implemented.

### Common Mistakes

**Mistake 1:** Not handling token expiration or invalid tokens.  
**Fix:** Check `exp` claim and refresh or logout.

**Mistake 2:** Using `localStorage` directly without a constant key.  
**Fix:** Use a constant for the token key.

**Mistake 3:** Not setting `loadingSignal` to false on error.  
**Fix:** Always set loading state in `catchError`.

### Edge Cases

*   Token is invalid or expired → logout and redirect to login.
*   User closes and reopens the app → session should be restored.
*   Multiple tabs → token is shared, but state is per‑tab (consider using storage events).

### Definition of Done

*   AuthService is implemented and works with all methods.
*   Token storage and session restoration work.
*   Signals provide reactive user state.

### Frontend Architecture Notes

*   AuthService is provided in root and is a singleton.
*   All authentication logic is encapsulated in this service.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `authService.currentUser()` to access user state.

## FE-AUTH-003Login Component P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-AUTH-003
*   **Task Name:** Login Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-AUTH-002 (AuthService), FE-SHARED-002 (Button), FE-SHARED-003 (Input)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Build the login page with a reactive form, validation, loading state, error handling, and redirect to the dashboard after successful authentication.

### Business Purpose

Allow therapists and patients to log in to the application. This is the primary entry point for users.

### Technical Purpose

Use `FormBuilder` with validation (email, required), call `AuthService.login()`, handle success (navigate to dashboard) and error (display messages), and manage loading state.

### Prerequisites

*   Understanding of Reactive Forms.
*   Knowledge of Router navigation.
*   Shared components (Button, Input).

### Dependencies

*   **FE-AUTH-002:** AuthService with login method.
*   **FE-SHARED-002:** Button component.
*   **FE-SHARED-003:** Input component.

### Inputs

*   Query param `returnUrl` for redirect after login.

### Outputs

*   `features/auth/pages/login/login.component.ts`
*   `login.component.html`
*   `login.component.scss`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c features/auth/pages/login --standalone --skip-tests
```
#### Step 2: Implement Login Component
File: `login.component.ts`


```typescript
// login.component.ts
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);
    private route = inject(ActivatedRoute);

    loading = signal(false);
    loginError = signal<string | null>(null);

    loginForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
    });

    get returnUrl(): string {
        return this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    }

    onSubmit(): void {
        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.loginError.set(null);

        const { email, password } = this.loginForm.value;

        this.authService.login({ email: email!, password: password! }).subscribe({
            next: () => {
                this.loading.set(false);
                this.router.navigateByUrl(this.returnUrl);
            },
            error: (err) => {
                this.loading.set(false);
                this.loginError.set(err.error?.message || 'Invalid email or password. Please try again.');
            },
        });
    }
}
```
#### Step 3: Create Template
File: `login.component.html`


```html
<!-- login.component.html -->
<div class="login-form">
    <h2 class="text-center mb-4">Welcome Back</h2>

    <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
        <app-input
            label="Email Address"
            type="email"
            placeholder="Enter your email"
            formControlName="email"
            [error]="loginForm.get('email')?.invalid && loginForm.get('email')?.touched ? getEmailError() : ''"
        ></app-input>

        <app-input
            label="Password"
            type="password"
            placeholder="Enter your password"
            formControlName="password"
            [error]="loginForm.get('password')?.invalid && loginForm.get('password')?.touched ? 'Password is required and must be at least 6 characters' : ''"
        ></app-input>

        <div class="d-flex justify-content-between align-items-center mt-2">
            <a routerLink="/auth/forgot-password" class="forgot-link">Forgot Password?</a>
        </div>

        <div class="mt-4">
            <app-button
                type="submit"
                variant="primary"
                [loading]="loading()"
                [disabled]="loginForm.invalid"
                class="w-100"
            >Sign In</app-button>
        </div>

        <div class="mt-3 text-center">
            <span class="text-muted">Don't have an account?</span>
            <a routerLink="/auth/register" class="ms-2">Sign Up</a>
        </div>

        <div *if="loginError()" class="alert alert-danger mt-3" role="alert">
            {{ loginError() }}
        </div>
    </form>
</div>
```
#### Step 4: Add Styles
File: `login.component.css`


```css
// login.component.scss
:host {
    display: block;
    width: 100%;
    max-width: 400px;
    margin: 0 auto;
}

.login-form {
    padding: 1rem;
}

.forgot-link {
    font-size: 0.9rem;
    color: var(--bs-secondary);
    text-decoration: none;

    &:hover {
        text-decoration: underline;
    }
}
```
### Files To Create

*   `features/auth/pages/login/login.component.ts`
*   `login.component.html`
*   `login.component.scss`

### CLI Commands

```bash
ng g c features/auth/pages/login --standalone --skip-tests
```
### Code Flow

User submits form → LoginComponent.onSubmit() → AuthService.login() → API ↕ Success → Navigate to dashboard Error → Display error message

### Concepts Required

*   Reactive Forms
*   Form validation
*   Router navigation
*   Signals for loading state

### Tools/Libraries Required

*   Angular Reactive Forms
*   Shared components

### UI/UX Requirements

*   Form should be centered on the page.
*   Loading state should disable the button.
*   Error messages should be displayed clearly.
*   RTL support for Arabic.

### Testing Steps

1.  Test form validation: empty fields, invalid email.
2.  Test successful login: redirect to dashboard.
3.  Test failed login: error message displayed.
4.  Test loading state: button disabled and spinner shown.

### Expected Deliverables

*   Login component fully functional.

### Common Mistakes

**Mistake 1:** Not handling subscription cleanup.  
**Fix:** Use `takeUntil` or `async` pipe.

**Mistake 2:** Not calling `markAllAsTouched()` on invalid form.  
**Fix:** Always mark all touched before showing errors.

### Edge Cases

*   User is already authenticated → redirect to dashboard.
*   Return URL from query params should be used after login.

### Definition of Done

*   Login component works with form validation.
*   Login flow succeeds and redirects.
*   Error handling displays messages.

### Frontend Architecture Notes

*   Uses shared Input and Button components for consistency.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Login is the entry point; test thoroughly.

## FE-AUTH-004Register Component P1 Medium 4h ▾

### Task Information

*   **Task ID:** FE-AUTH-004
*   **Task Name:** Register Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-AUTH-002 (AuthService), FE-SHARED-002 (Button), FE-SHARED-003 (Input)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Build the registration page for therapists to sign up, with form validation, password confirmation, and success redirection.

### Business Purpose

Allow new therapists to create accounts and join the platform.

### Technical Purpose

Reactive form with fields: firstName, lastName, email, password, confirmPassword (with custom validator for match), and role selection.

### Prerequisites

*   Understanding of Reactive Forms and custom validators.
*   Shared components (Button, Input).

### Dependencies

*   **FE-AUTH-002:** AuthService.register method.
*   **FE-SHARED-002:** Button component.
*   **FE-SHARED-003:** Input component.

### Inputs

*   None.

### Outputs

*   `features/auth/pages/register/register.component.ts`
*   `register.component.html`
*   `register.component.scss`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c features/auth/pages/register --standalone --skip-tests
```
#### Step 2: Create Password Match Validator
Create a custom validator function.
File: `validators/password-match.validator.ts`

```typescript
// validators/password-match.validator.ts
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
        return { passwordMismatch: true };
    }
    return null;
};
```
#### Step 3: Implement Register Component
File: `register.component.ts`


```typescript
// register.component.ts
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);

    loading = signal(false);
    registerError = signal<string | null>(null);
    success = signal(false);

    registerForm = this.fb.group({
        firstName: ['', [Validators.required, Validators.minLength(2)]],
        lastName: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
        role: ['Therapist', [Validators.required]],
    }, { validators: passwordMatchValidator });

    onSubmit(): void {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.registerError.set(null);

        const { firstName, lastName, email, password, role } = this.registerForm.value;

        this.authService.register({ firstName: firstName!, lastName: lastName!, email: email!, password: password!, role: role! }).subscribe({
            next: () => {
                this.loading.set(false);
                this.success.set(true);
                setTimeout(() => {
                    this.router.navigate(['/auth/login']);
                }, 2000);
            },
            error: (err) => {
                this.loading.set(false);
                this.registerError.set(err.error?.message || 'Registration failed. Please try again.');
            },
        });
    }
}
```
#### Step 4: Create Template
File: `register.component.html`


```html
<!-- register.component.html -->
<div class="register-form">
    <h2 class="text-center mb-4">Create Account</h2>

    <div *if="success()" class="alert alert-success" role="alert">
        Account created successfully! Redirecting to login...
    </div>

    <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" *if="!success()">
        <div class="row">
            <div class="col-md-6">
                <app-input
                    label="First Name"
                    type="text"
                    placeholder="Enter first name"
                    formControlName="firstName"
                    [error]="registerForm.get('firstName')?.invalid && registerForm.get('firstName')?.touched ? 'First name is required' : ''"
                ></app-input>
            </div>
            <div class="col-md-6">
                <app-input
                    label="Last Name"
                    type="text"
                    placeholder="Enter last name"
                    formControlName="lastName"
                    [error]="registerForm.get('lastName')?.invalid && registerForm.get('lastName')?.touched ? 'Last name is required' : ''"
                ></app-input>
            </div>
        </div>

        <app-input
            label="Email Address"
            type="email"
            placeholder="Enter your email"
            formControlName="email"
            [error]="registerForm.get('email')?.invalid && registerForm.get('email')?.touched ? 'Valid email is required' : ''"
        ></app-input>

        <app-input
            label="Password"
            type="password"
            placeholder="Create a password"
            formControlName="password"
            [error]="registerForm.get('password')?.invalid && registerForm.get('password')?.touched ? 'Password must be at least 6 characters' : ''"
        ></app-input>

        <app-input
            label="Confirm Password"
            type="password"
            placeholder="Confirm your password"
            formControlName="confirmPassword"
            [error]="registerForm.errors?.['passwordMismatch'] && registerForm.get('confirmPassword')?.touched ? 'Passwords do not match' : ''"
        ></app-input>

        <div class="form-group">
            <label>Role</label>
            <select class="form-select" formControlName="role">
                <option value="Therapist">Therapist</option>
                <option value="Admin">Admin</option>
            </select>
        </div>

        <div class="mt-4">
            <app-button
                type="submit"
                variant="primary"
                [loading]="loading()"
                [disabled]="registerForm.invalid"
                class="w-100"
            >Create Account</app-button>
        </div>

        <div class="mt-3 text-center">
            <span class="text-muted">Already have an account?</span>
            <a routerLink="/auth/login" class="ms-2">Sign In</a>
        </div>

        <div *if="registerError()" class="alert alert-danger mt-3" role="alert">
            {{ registerError() }}
        </div>
    </form>
</div>
```
### Files To Create

*   `features/auth/pages/register/register.component.ts`
*   `register.component.html`
*   `register.component.scss`
*   `shared/validators/password-match.validator.ts`

### CLI Commands

```bash
ng g c features/auth/pages/register --standalone --skip-tests
```
### Concepts Required

*   Reactive Forms with custom validators
*   Cross‑field validation

### Testing Steps

1.  Test form validation (required fields, email, password match).
2.  Test successful registration (redirect to login).
3.  Test error handling.

### Expected Deliverables

*   Register component fully functional.

### Common Mistakes

**Mistake 1:** Forgetting to add the custom validator to the form group.  
**Fix:** Include `{ validators: passwordMatchValidator }`.

**Mistake 2:** Not handling the success state before redirect.  
**Fix:** Show success message and redirect after a delay.

### Edge Cases

*   Email already registered → error message from API.
*   Weak password → validation prevents submission.

### Definition of Done

*   Register component works with validation.
*   Registration flow succeeds.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Registration creates new therapist accounts.

## FE-AUTH-005Reset Password Components P1 Medium 4h ▾

### Task Information

*   **Task ID:** FE-AUTH-005
*   **Task Name:** Reset Password Components
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-AUTH-002 (AuthService), FE-SHARED-002 (Button), FE-SHARED-003 (Input)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Create two components: `ForgotPasswordComponent` (request reset link) and `ResetPasswordComponent` (set new password using token from query param).

### Business Purpose

Allow users to reset their password if they forget it, improving user experience and reducing support tickets.

### Technical Purpose

Implement two-step flow: request reset (email) and reset password (token + new password) using the AuthService.

### Prerequisites

*   Reactive Forms.
*   ActivatedRoute for token parameter.
*   Shared components.

### Dependencies

*   **FE-AUTH-002:** AuthService methods: `forgotPassword()`, `resetPassword()`.

### Inputs

*   Token from query parameter for reset.

### Outputs

*   `features/auth/pages/forgot-password/forgot-password.component.ts`
*   `features/auth/pages/reset-password/reset-password.component.ts`

### Detailed Workflow

#### Step 1: Generate Components

```bash
ng g c features/auth/pages/forgot-password --standalone --skip-tests 
ng g c features/auth/pages/reset-password --standalone --skip-tests
```
#### Step 2: Implement ForgotPasswordComponent
File: `forgot-password.component.ts`


```typescript
// forgot-password.component.ts
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';

@Component({
    selector: 'app-forgot-password',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);

    loading = signal(false);
    submitted = signal(false);
    error = signal<string | null>(null);

    forgotForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
    });

    onSubmit(): void {
        if (this.forgotForm.invalid) {
            this.forgotForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const email = this.forgotForm.value.email!;

        this.authService.forgotPassword(email).subscribe({
            next: () => {
                this.loading.set(false);
                this.submitted.set(true);
            },
            error: (err) => {
                this.loading.set(false);
                this.error.set(err.error?.message || 'Failed to send reset link. Please try again.');
            },
        });
    }
}
```
#### Step 3: Implement ResetPasswordComponent
File: `reset-password.component.ts`


```typescript
// reset-password.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';

@Component({
    selector: 'app-reset-password',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent, RouterLink],
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    loading = signal(false);
    success = signal(false);
    error = signal<string | null>(null);
    token = signal<string | null>(null);

    resetForm = this.fb.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
    }, { validators: passwordMatchValidator });

    ngOnInit(): void {
        this.token.set(this.route.snapshot.queryParams['token'] || null);
        if (!this.token()) {
            this.error.set('Invalid or missing reset token. Please request a new password reset link.');
        }
    }

    onSubmit(): void {
        if (this.resetForm.invalid || !this.token()) {
            this.resetForm.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const password = this.resetForm.value.password!;

        this.authService.resetPassword({ token: this.token()!, newPassword: password }).subscribe({
            next: () => {
                this.loading.set(false);
                this.success.set(true);
                setTimeout(() => {
                    this.router.navigate(['/auth/login']);
                }, 3000);
            },
            error: (err) => {
                this.loading.set(false);
                this.error.set(err.error?.message || 'Failed to reset password. Please try again.');
            },
        });
    }
}
```
### Files To Create

*   `forgot-password.component.ts`, `.html`, `.scss`
*   `reset-password.component.ts`, `.html`, `.scss`

### CLI Commands

```bash
ng g c features/auth/pages/forgot-password --standalone --skip-tests 
ng g c features/auth/pages/reset-password --standalone --skip-tests
```
### Testing Steps

1.  Test forgot password: valid email → success message.
2.  Test forgot password: invalid email → error.
3.  Test reset password: valid token → success and redirect.
4.  Test reset password: invalid/expired token → error.

### Expected Deliverables

*   Two components for reset password flow.

### Common Mistakes

**Mistake 1:** Not checking for token presence before allowing reset.  
**Fix:** Display error if token is missing.

### Definition of Done

*   Both components work and the flow is complete.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Reset password flow uses email and token.

## FE-AUTH-006Profile Component P1 Medium 4h ▾

### Task Information

*   **Task ID:** FE-AUTH-006
*   **Task Name:** Profile Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-AUTH-002 (AuthService), FE-SHARED-002 (Button), FE-SHARED-003 (Input)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Create a profile page where users can view and update their information, and change their password.

### Business Purpose

Allow users to manage their own profile information.

### Technical Purpose

Fetch current user data via AuthService, display in a form, allow updates (name, email) and password change (current, new, confirm).

### Prerequisites

*   Reactive Forms with nested groups.
*   AuthService methods: `getProfile()`, `updateProfile()`, `changePassword()`.

### Dependencies

*   **FE-AUTH-002:** AuthService.

### Inputs

*   User ID from auth state.

### Outputs

*   `features/auth/pages/profile/profile.component.ts`
*   `profile.component.html`
*   `profile.component.scss`

### Detailed Workflow

#### Step 1: Generate Component

```bash
ng g c features/auth/pages/profile --standalone --skip-tests
```
#### Step 2: Implement Profile Component
File: `profile.component.ts`


```typescript
// profile.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { passwordMatchValidator } from '../../../../shared/validators/password-match.validator';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, InputComponent],
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);

    loading = signal(true);
    updating = signal(false);
    changingPassword = signal(false);
    error = signal<string | null>(null);
    passwordError = signal<string | null>(null);
    success = signal(false);

    profileForm = this.fb.group({
        firstName: ['', [Validators.required, Validators.minLength(2)]],
        lastName: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
    });

    passwordForm = this.fb.group({
        currentPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
    }, { validators: passwordMatchValidator });

    ngOnInit(): void {
        this.loadProfile();
    }

    loadProfile(): void {
        this.loading.set(true);
        this.authService.getProfile().subscribe({
            next: (user) => {
                this.profileForm.patchValue({
                    firstName: user.firstName,
                    lastName: user.lastName,
                    email: user.email,
                });
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
                this.error.set('Failed to load profile. Please try again.');
            },
        });
    }

    updateProfile(): void {
        if (this.profileForm.invalid) {
            this.profileForm.markAllAsTouched();
            return;
        }

        this.updating.set(true);
        this.error.set(null);

        const { firstName, lastName, email } = this.profileForm.value;

        this.authService.updateProfile({ firstName: firstName!, lastName: lastName!, email: email! }).subscribe({
            next: () => {
                this.updating.set(false);
                this.success.set(true);
                setTimeout(() => this.success.set(false), 3000);
            },
            error: (err) => {
                this.updating.set(false);
                this.error.set(err.error?.message || 'Failed to update profile.');
            },
        });
    }

    changePassword(): void {
        if (this.passwordForm.invalid) {
            this.passwordForm.markAllAsTouched();
            return;
        }

        this.changingPassword.set(true);
        this.passwordError.set(null);

        const { currentPassword, newPassword } = this.passwordForm.value;

        this.authService.changePassword({ currentPassword: currentPassword!, newPassword: newPassword! }).subscribe({
            next: () => {
                this.changingPassword.set(false);
                this.passwordForm.reset();
                this.success.set(true);
                setTimeout(() => this.success.set(false), 3000);
            },
            error: (err) => {
                this.changingPassword.set(false);
                this.passwordError.set(err.error?.message || 'Failed to change password.');
            },
        });
    }
}
```
#### Step 3: Create Template
File: `profile.component.html`


```html
<!-- profile.component.html -->
<div class="profile-container">
    <h2 class="mb-4">Profile</h2>

    <div *if="loading()" class="text-center py-5">
        <app-spinner size="lg"></app-spinner>
    </div>

    <div *if="!loading()">
        <div *if="success()" class="alert alert-success" role="alert">
            Changes saved successfully!
        </div>

        <div class="card mb-4">
            <div class="card-header">
                <h5 class="mb-0">Personal Information</h5>
            </div>
            <div class="card-body">
                <form [formGroup]="profileForm">
                    <div class="row">
                        <div class="col-md-6">
                            <app-input
                                label="First Name"
                                type="text"
                                placeholder="Enter first name"
                                formControlName="firstName"
                                [error]="profileForm.get('firstName')?.invalid && profileForm.get('firstName')?.touched ? 'First name is required' : ''"
                            ></app-input>
                        </div>
                        <div class="col-md-6">
                            <app-input
                                label="Last Name"
                                type="text"
                                placeholder="Enter last name"
                                formControlName="lastName"
                                [error]="profileForm.get('lastName')?.invalid && profileForm.get('lastName')?.touched ? 'Last name is required' : ''"
                            ></app-input>
                        </div>
                    </div>

                    <app-input
                        label="Email Address"
                        type="email"
                        placeholder="Enter your email"
                        formControlName="email"
                        [error]="profileForm.get('email')?.invalid && profileForm.get('email')?.touched ? 'Valid email is required' : ''"
                    ></app-input>

                    <div *if="error()" class="alert alert-danger mt-3">
                        {{ error() }}
                    </div>

                    <div class="mt-3">
                        <app-button
                            type="button"
                            variant="primary"
                            [loading]="updating()"
                            (click)="updateProfile()"
                        >Update Profile</app-button>
                    </div>
                </form>
            </div>
        </div>

        <div class="card">
            <div class="card-header">
                <h5 class="mb-0">Change Password</h5>
            </div>
            <div class="card-body">
                <form [formGroup]="passwordForm">
                    <app-input
                        label="Current Password"
                        type="password"
                        placeholder="Enter current password"
                        formControlName="currentPassword"
                        [error]="passwordForm.get('currentPassword')?.invalid && passwordForm.get('currentPassword')?.touched ? 'Current password is required' : ''"
                    ></app-input>

                    <app-input
                        label="New Password"
                        type="password"
                        placeholder="Enter new password"
                        formControlName="newPassword"
                        [error]="passwordForm.get('newPassword')?.invalid && passwordForm.get('newPassword')?.touched ? 'Password must be at least 6 characters' : ''"
                    ></app-input>

                    <app-input
                        label="Confirm New Password"
                        type="password"
                        placeholder="Confirm new password"
                        formControlName="confirmPassword"
                        [error]="passwordForm.errors?.['passwordMismatch'] && passwordForm.get('confirmPassword')?.touched ? 'Passwords do not match' : ''"
                    ></app-input>

                    <div *if="passwordError()" class="alert alert-danger mt-3">
                        {{ passwordError() }}
                    </div>

                    <div class="mt-3">
                        <app-button
                            type="button"
                            variant="danger"
                            [loading]="changingPassword()"
                            (click)="changePassword()"
                        >Change Password</app-button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>
```
### Files To Create

*   `profile.component.ts`, `.html`, `.scss`

### CLI Commands

```bash
ng g c features/auth/pages/profile --standalone --skip-tests
```
### Testing Steps

1.  Test profile loads correctly.
2.  Test profile update.
3.  Test password change with current password validation.

### Expected Deliverables

*   Profile component fully functional.

### Common Mistakes

**Mistake 1:** Not updating the AuthService user signal after profile update.  
**Fix:** AuthService should update the user signal in `updateProfile()`.

### Definition of Done

*   Profile component works with all features.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Profile page is accessible from the sidebar.

## FE-AUTH-007HTTP Interceptors (Auth, Error, Loading) P0 High 5h ▾

### Task Information

*   **Task ID:** FE-AUTH-007
*   **Task Name:** HTTP Interceptors (Auth, Error, Loading)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-AUTH-002 (AuthService), FE-CORE-003 (NotificationService, LoadingService)
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Create three functional HTTP interceptors: `authInterceptor` (adds JWT token), `errorInterceptor` (handles 401, 403, 500 errors), and `loadingInterceptor` (tracks global loading state).

### Business Purpose

Ensure all API requests are authenticated, handle token expiration gracefully, provide user‑friendly error messages, and show a global loading indicator.

### Technical Purpose

Use `HttpInterceptorFn` (Angular 15+) for cleaner, tree‑shakable interceptors. Integrate with `LoadingService` and `NotificationService`.

### Prerequisites

*   Understanding of Angular HTTP interceptors.
*   Knowledge of RxJS operators (`catchError`, `throwError`, `finalize`).
*   Functional interceptors in Angular 15+.

### Dependencies

*   **FE-AUTH-002:** AuthService for token and logout.
*   **FE-CORE-003:** LoadingService and NotificationService.

### Inputs

*   AuthService with `getToken()`, `logout()`.

### Outputs

*   `core/interceptors/auth.interceptor.ts`
*   `core/interceptors/error.interceptor.ts`
*   `core/interceptors/loading.interceptor.ts`
*   `core/interceptors/index.ts`
*   Updated `app.config.ts` with interceptors registered.

### Detailed Workflow

#### Step 1: Generate Interceptors

```bash
ng g interceptor core/interceptors/auth --functional --skip-tests
ng g interceptor core/interceptors/error --functional --skip-tests
ng g interceptor core/interceptors/loading --functional --skip-tests
```
#### Step 2: Implement `auth.interceptor.ts`
File: `core/interceptors/auth.interceptor.ts`


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
#### Step 3: Implement `error.interceptor.ts`
File: `core/interceptors/error.interceptor.ts`


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
            console.error('HTTP Error:', error);

            if (error.status === 401) {
                // Unauthorized – log out and redirect to login
                authService.logout();
                router.navigate(['/auth/login']);
                notification.error('Your session has expired. Please log in again.');
            } else if (error.status === 403) {
                notification.error('You do not have permission to perform this action.');
            } else if (error.status === 400) {
                const message = error.error?.message || 'Invalid request. Please check your input.';
                notification.error(message);
            } else if (error.status === 500) {
                notification.error('A server error occurred. Please try again later.');
            } else if (error.status === 0) {
                notification.error('Network error. Please check your connection.');
            } else {
                notification.error('An unexpected error occurred. Please try again.');
            }

            return throwError(() => error);
        })
    );
};
```
#### Step 4: Implement `loading.interceptor.ts`
File: `core/interceptors/loading.interceptor.ts`


```typescript
// core/interceptors/loading.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
    const loadingService = inject(LoadingService);

    // Skip loading for certain requests
    if (req.url.includes('/refresh')) {
        return next(req);
    }

    loadingService.show();

    return next(req).pipe(
        finalize(() => loadingService.hide())
    );
};
```
#### Step 5: Create Barrel Export
File: `core/interceptors/index.ts`


```typescript
// core/interceptors/index.ts
export * from './auth.interceptor';
export * from './error.interceptor';
export * from './loading.interceptor';
```
#### Step 6: Register Interceptors in `app.config.ts`
File: a`pp.config.ts`


```typescript
// app.config.ts
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor, errorInterceptor, loadingInterceptor } from './core/interceptors';

export const appConfig: ApplicationConfig = {
    providers: [
        provideHttpClient(
            withInterceptors([authInterceptor, errorInterceptor, loadingInterceptor])
        ),
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
ng g interceptor core/interceptors/auth --functional --skip-tests
ng g interceptor core/interceptors/error --functional --skip-tests
ng g interceptor core/interceptors/loading --functional --skip-tests
```

### Code Flow

```
HttpClient.get() → authInterceptor (add token) → loadingInterceptor (show) → API ↕ errorInterceptor (catch) → loadingInterceptor (hide)
```
### Concepts Required

*   HttpInterceptorFn (functional interceptors)
*   RxJS operators: `catchError`, `throwError`, `finalize`
*   Dependency injection with `inject()`

### Tools/Libraries Required

*   Angular HttpClient

### Angular Concepts Required

*   HTTP interceptors
*   Functional interceptors (Angular 15+)

### RxJS Concepts Required

*   `catchError`, `finalize`, `throwError`

### Security Considerations

*   Tokens are sent with every request via the auth interceptor.
*   401 responses trigger logout and redirect, preventing stale tokens.
*   Refresh token endpoint should be excluded from auth interceptor if needed.

### Testing Steps

1.  Unit test each interceptor with mock `HttpHandler`.
2.  Test auth interceptor adds token when present.
3.  Test error interceptor shows notification and handles 401.
4.  Test loading interceptor shows/hides loading.

### Expected Deliverables

*   Three interceptors implemented and registered.

### Common Mistakes

**Mistake 1:** Forgetting to register interceptors in `app.config.ts`.  
**Fix:** Add `withInterceptors([...])` to `provideHttpClient`.

**Mistake 2:** Not cloning the request before modifying headers.  
**Fix:** Always clone the request in auth interceptor.

**Mistake 3:** Infinite loop when refresh token also returns 401.  
**Fix:** Skip auth interceptor for refresh token endpoint.

### Edge Cases

*   Refresh token request should not trigger auth interceptor.
*   If token is expired, 401 triggers logout; ensure `logout()` doesn't cause infinite redirect.
*   Network errors (status 0) should show a network error message.

### Definition of Done

*   All interceptors are implemented and registered.
*   HTTP requests include token, show loading, and handle errors.

### Frontend Architecture Notes

*   Interceptors are the single point for cross‑cutting concerns.
*   All HTTP requests go through this pipeline.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Interceptors automatically handle token injection and errors; no need to duplicate logic in services.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 7 authentication tasks are complete (FE-AUTH-001 to FE-AUTH-007).
*   Auth routes are configured with lazy loading.
*   AuthService is fully implemented with Signals and JWT handling.
*   Login component works with validation and redirect.
*   Register component works with validation and success redirect.
*   Forgot Password and Reset Password components work.
*   Profile component allows viewing and updating user information and changing password.
*   Interceptors (auth, error, loading) are implemented and registered.
*   All components follow the design system and are RTL‑aware.
*   Unit tests pass for each component and service.
*   All changes are committed to the repository.

📋 Code Review Checklist
------------------------

*   AuthService uses Signals correctly (`signal`, `computed`, `effect`).
*   Token storage uses a constant key.
*   Login and Register forms have proper validation.
*   Password match validator works correctly.
*   Interceptors are functional and handle all error cases.
*   Guards (from Core phase) are applied to protected routes.
*   All components are Standalone and use shared components.
*   RTL support is verified.

🚀 Pull Request Checklist
-------------------------

*   Branch is up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   Commit message follows conventional commits.

🧪 Deployment Readiness Checklist
---------------------------------

*   Authentication flow works end‑to‑end (login → dashboard).
*   Registration works and creates new users.
*   Reset password flow works with valid tokens.
*   Interceptors handle 401 and redirect to login.

* * *

Jalsa – Phase 5: Authentication – Expanded Implementation Handbook • v1.0 • For development team

