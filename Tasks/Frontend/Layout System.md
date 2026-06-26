

📐 Jalsa – Phase 6: Layout System Expanded Implementation Handbook · 6 Tasks · 30+ Subtasks
===========================================================================================

📐 Phase 6 – Layout System
--------------------------

**Purpose:** Build the main application layout that wraps all authenticated pages. This includes the sidebar, header, and footer components, along with role‑based navigation, responsive design, and RTL support. The layout system provides a consistent shell for the entire application.

📋 Tasks: 6 ⏱️ Total Effort: ~20 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 4 (Shared Components), Phase 5 (Authentication) 🎯 Deliverable: Complete main layout with sidebar, header, footer, and navigation service

## FE-LAYOUT-001Main Layout Component Setup P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-LAYOUT-001
*   **Task Name:** Main Layout Component Setup
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SHARED-001 (Shared setup), FE-AUTH-001 (Auth routes)
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Create the main layout component that serves as the shell for all authenticated (protected) pages. It will contain the sidebar, header, footer, and a main content area where routed components are rendered using `<router-outlet>`.

### Business Purpose

Every authenticated page shares the same layout (sidebar, header, footer). A single layout component ensures consistency and reduces code duplication.

### Technical Purpose

Build a Standalone component that uses `<router-outlet>` to display feature components. The layout will be used as the parent route for all protected routes.

### Prerequisites

*   Understanding of Angular routing with nested routes.
*   Knowledge of Bootstrap grid and layout utilities.
*   Basic understanding of RTL and responsive design.

### Dependencies

*   **FE-SHARED-001:** Shared library is available.
*   **FE-AUTH-001:** Auth routes are defined; we'll protect them with guards.

### Inputs

*   Design mockups for the layout.

### Outputs

*   `shared/layouts/main-layout/main-layout.component.ts`
*   `main-layout.component.html`
*   `main-layout.component.scss`

### Detailed Workflow

#### Step 1: Generate Main Layout Component

```bash
ng g c shared/layouts/main-layout --standalone --skip-tests
```
#### Step 2: Define the Layout Structure

Create a template with a sidebar, header, and content area.

```html
<!-- main-layout.component.html -->
<div class="app-layout" [class.sidebar-collapsed]="sidebarCollapsed()">
    <app-sidebar [collapsed]="sidebarCollapsed()" (toggle)="toggleSidebar()"></app-sidebar>
    <div class="app-main">
        <app-header (toggleSidebar)="toggleSidebar()"></app-header>
        <main class="app-content">
            <router-outlet></router-outlet>
        </main>
        <app-footer></app-footer>
    </div>
</div>
```
#### Step 3: Implement Logic in Component

```typescript
// main-layout.component.ts
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppStateService } from '../../../core/services/app-state.service';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';

@Component({
    selector: 'app-main-layout',
    standalone: true,
    imports: [RouterOutlet, SidebarComponent, HeaderComponent, FooterComponent],
    templateUrl: './main-layout.component.html',
    styleUrls: ['./main-layout.component.scss'],
})
export class MainLayoutComponent {
    private appState = inject(AppStateService);

    sidebarCollapsed = this.appState.sidebarCollapsed;

    toggleSidebar() {
        this.appState.toggleSidebar();
    }
}
```
#### Step 4: Add Styles

```css
// main-layout.component.scss
.app-layout {
    display: flex;
    min-height: 100vh;
    width: 100%;
}

.app-main {
    flex: 1;
    display: flex;
    flex-direction: column;
    min-height: 100vh;
}

.app-content {
    flex: 1;
    padding: 1.5rem;
    background-color: $body-bg; // from variables
}

// Sidebar collapsed state
.sidebar-collapsed .app-content {
    margin-left: 0; // will be adjusted in RTL
}
[dir="rtl"] .sidebar-collapsed .app-content {
    margin-right: 0;
}
```
#### Step 5: Update Route Configuration

Use the main layout as a parent route for all protected features.

```typescript
// app.routes.ts
{
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard], // from Phase 2
    children: [
        {
            path: 'dashboard',
            loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
        },
        {
            path: 'patients',
            loadChildren: () => import('./features/patients/patients.routes').then(m => m.PATIENTS_ROUTES),
        },
        // ... other features
        {
            path: '',
            redirectTo: 'dashboard',
            pathMatch: 'full',
        },
    ],
},
// Auth routes are outside the main layout
{
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
},
{
    path: '**',
    redirectTo: 'dashboard',
},
```
#### Step 6: Create Placeholder Sidebar, Header, Footer

Generate placeholder components (they will be implemented in later tasks).

```bash
ng g c shared/layouts/sidebar --standalone --skip-tests
ng g c shared/layouts/header --standalone --skip-tests
ng g c shared/layouts/footer --standalone --skip-tests
```

### Files To Create

*   `shared/layouts/main-layout/main-layout.component.ts`
*   `main-layout.component.html`
*   `main-layout.component.scss`
*   Placeholder sidebar, header, footer components.

### CLI Commands

```bash
ng g c shared/layouts/sidebar --standalone --skip-tests
ng g c shared/layouts/header --standalone --skip-tests
ng g c shared/layouts/footer --standalone --skip-tests
```
### Code Flow

```
App Routes → MainLayoutComponent (with authGuard) → Child routes (dashboard, patients, etc.) → Sidebar, Header, Footer visible
```
### Concepts Required

*   Nested routes with `<router-outlet>`
*   Component communication via services (AppStateService)
*   Flexbox layout
*   RTL support

### Tools/Libraries Required

*   Angular Router
*   Bootstrap (for utilities)

### Angular Concepts Required

*   Standalone components
*   `@Input` and `@Output`
*   Dependency injection

### Testing Steps

1.  Navigate to a protected route and verify the layout renders with sidebar, header, footer.
2.  Verify the sidebar toggle works (via AppStateService).
3.  Test RTL direction: set `dir="rtl"` and check layout.

### Expected Deliverables

*   Main layout component with sidebar, header, footer placeholders.
*   Updated route configuration.

### Common Mistakes

**Mistake 1:** Not using `<router-outlet>` in the layout template.  
**Fix:** Add `<router-outlet>` in the content area.

**Mistake 2:** Forgetting to import the layout components into the main layout.  
**Fix:** Add SidebarComponent, HeaderComponent, FooterComponent to the imports array.

### Edge Cases

*   When the sidebar is collapsed, the content should expand to fill the width.
*   In RTL, the sidebar should be on the right side.

### Definition of Done

*   Main layout component exists and renders with placeholders.
*   Protected routes are wrapped by the layout.

### Frontend Architecture Notes

*   The main layout is a Standalone component.
*   Sidebar state is managed globally via AppStateService.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** All authenticated pages will use this layout.

## FE-LAYOUT-002Sidebar Component P0 High 5h ▾

### Task Information

*   **Task ID:** FE-LAYOUT-002
*   **Task Name:** Sidebar Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-LAYOUT-001, FE-AUTH-002 (AuthService), FE-CORE-003 (AppStateService)
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Build the sidebar navigation component with collapsible functionality, role‑based menu items, active route highlighting, and support for RTL.

### Business Purpose

The sidebar is the primary navigation tool for users. It must be intuitive, responsive, and reflect the user's permissions.

### Technical Purpose

Create a reusable sidebar component that receives menu items from a navigation service (or defines them internally), uses `routerLinkActive` for highlighting, and toggles via AppStateService.

### Prerequisites

*   Understanding of Angular Router and `routerLink`.
*   Knowledge of structural directives (`*ngIf`, `*ngFor`).
*   Bootstrap classes for styling.

### Dependencies

*   **FE-LAYOUT-001:** Main layout uses sidebar.
*   **FE-AUTH-002:** AuthService for role checking.
*   **FE-CORE-003:** AppStateService for sidebar collapsed state.

### Inputs

*   `collapsed` – boolean from parent (main layout).

### Outputs

*   `toggle` – event emitted when toggle button is clicked.

### Outputs (Files)

*   `shared/layouts/sidebar/sidebar.component.ts`
*   `sidebar.component.html`
*   `sidebar.component.scss`

### Detailed Workflow

#### Step 1: Define Navigation Menu Items

Create a data structure for menu items, including label, icon, route, and required roles.

```typescript
// sidebar.component.ts (part)
import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { AppStateService } from '../../../core/services/app-state.service';
import { NgClass } from '@angular/common';

export interface NavItem {
    label: string;
    icon: string; // Bootstrap icon class
    route: string;
    roles?: string[]; // if not provided, visible to all authenticated
    children?: NavItem[];
}

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [RouterLink, RouterLinkActive, NgClass],
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent {
    private authService = inject(AuthService);
    private appState = inject(AppStateService);

    @Input() collapsed = false;
    @Output() toggle = new EventEmitter<void>();

    menuItems: NavItem[] = [
        { label: 'Dashboard', icon: 'bi-grid', route: '/dashboard' },
        { label: 'Patients', icon: 'bi-people', route: '/patients', roles: ['Therapist', 'Admin'] },
        { label: 'Sessions', icon: 'bi-calendar', route: '/sessions', roles: ['Therapist', 'Admin'] },
        { label: 'Exercises', icon: 'bi-clipboard', route: '/exercises', roles: ['Therapist', 'Admin'] },
        { label: 'Reports', icon: 'bi-file-text', route: '/reports', roles: ['Therapist', 'Admin'] },
        { label: 'Chat', icon: 'bi-chat', route: '/chatbot', roles: ['Patient'] },
    ];

    get filteredMenu(): NavItem[] {
        return this.menuItems.filter(item => {
            if (!item.roles) return true;
            return this.authService.hasAnyRole(item.roles);
        });
    }

    toggleSidebar() {
        this.toggle.emit();
    }
}
```
#### Step 2: Create Template

```html
<!-- sidebar.component.html -->
<aside class="sidebar" [class.collapsed]="collapsed">
    <div class="sidebar-header">
        <div class="sidebar-brand">
            <span *if="!collapsed">Jalsa</span>
            <span *if="collapsed">J</span>
        </div>
        <button class="btn btn-ghost toggle-btn" (click)="toggleSidebar()">
            <i class="bi bi-chevron-{{ collapsed ? 'right' : 'left' }}"></i>
        </button>
    </div>
    <nav class="sidebar-nav">
        <ul class="nav flex-column">
            <li *for="let item of filteredMenu" class="nav-item">
                <a class="nav-link"
                   [routerLink]="item.route"
                   routerLinkActive="active"
                   [routerLinkActiveOptions]="{ exact: false }">
                    <i class="bi {{ item.icon }}"></i>
                    <span *if="!collapsed">{{ item.label }}</span>
                </a>
            </li>
        </ul>
    </nav>
</aside>
```
#### Step 3: Add Styles

```css
// sidebar.component.scss
.sidebar {
    width: 260px;
    min-height: 100vh;
    background-color: $primary;
    color: #fff;
    transition: width 0.3s ease;
    display: flex;
    flex-direction: column;
    flex-shrink: 0;
    overflow: hidden;
}

.sidebar.collapsed {
    width: 64px;
}

.sidebar-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 1rem;
    border-bottom: 1px solid rgba(255,255,255,0.1);
}

.sidebar-brand {
    font-size: 1.2rem;
    font-weight: 700;
    white-space: nowrap;
}

.toggle-btn {
    color: #fff;
    background: transparent;
    border: none;
    cursor: pointer;
    padding: 0.25rem;
}

.sidebar-nav {
    flex: 1;
    padding: 0.5rem 0;
}

.nav-link {
    color: rgba(255,255,255,0.7);
    padding: 0.75rem 1rem;
    display: flex;
    align-items: center;
    gap: 0.75rem;
    text-decoration: none;
    transition: background 0.15s;
    white-space: nowrap;
}

.nav-link:hover {
    background: rgba(255,255,255,0.1);
    color: #fff;
}

.nav-link.active {
    background: rgba(255,255,255,0.2);
    color: #fff;
    font-weight: 500;
}

.nav-link i {
    font-size: 1.2rem;
    min-width: 1.5rem;
    text-align: center;
}

.sidebar.collapsed .nav-link span {
    display: none;
}
.sidebar.collapsed .sidebar-brand span {
    display: none;
}
```
#### Step 4: RTL Adjustments

Ensure the sidebar is on the right in RTL mode.

```css
// In main-layout.component.scss or global
[dir="rtl"] .sidebar {
    order: 2;
}
[dir="rtl"] .app-main {
    order: 1;
}
```
### Files To Create

*   `shared/layouts/sidebar/sidebar.component.ts`
*   `sidebar.component.html`
*   `sidebar.component.scss`

### CLI Commands

```bash
ng g c shared/layouts/sidebar --standalone --skip-tests
```
### Code Flow

````
SidebarComponent → AuthService.hasAnyRole() → Filtered menu → Render nav items with routerLink
````
### Concepts Required

*   RouterLink and RouterLinkActive
*   Structural directives (`*ngIf`, `*ngFor`)
*   Component inputs and outputs
*   Bootstrap icons

### Tools/Libraries Required

*   Bootstrap Icons
*   Angular Router

### Testing Steps

1.  Test that menu items are filtered based on user roles.
2.  Test that clicking a menu item navigates to the correct route.
3.  Test that the active route is highlighted.
4.  Test collapsing/expanding the sidebar.
5.  Test RTL layout.

### Expected Deliverables

*   Sidebar component with dynamic, role‑based navigation.

### Common Mistakes

**Mistake 1:** Not using `routerLinkActive` correctly – missing `routerLinkActiveOptions`.  
**Fix:** Set `[routerLinkActiveOptions]="{ exact: false }"` for nested routes.

**Mistake 2:** Not handling RTL properly – sidebar should be on the right.  
**Fix:** Use CSS flex order or margin adjustments.

### Edge Cases

*   User has no roles → only items without roles are shown.
*   Sidebar collapsed state should be persisted (via AppStateService).

### Definition of Done

*   Sidebar renders with correct menu items.
*   Navigation works and active route is highlighted.
*   Toggle collapses/expands the sidebar.
*   RTL works.

### Frontend Architecture Notes

*   Menu items are defined in the component; could be moved to a service later.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** The sidebar is the primary navigation; ensure new routes are added to the menu.

## FE-LAYOUT-003Header Component P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-LAYOUT-003
*   **Task Name:** Header Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-LAYOUT-001, FE-AUTH-002 (AuthService), FE-SHARED-002 (Button)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Build the header component that displays the application title, user avatar/name, logout button, and a hamburger menu toggle for mobile.

### Business Purpose

The header provides essential navigation controls and user context.

### Technical Purpose

Create a Standalone component that uses AuthService for user info and logout, emits toggleSidebar event to parent, and adapts for mobile.

### Prerequisites

*   Understanding of AuthService and Signals.
*   Bootstrap grid and dropdown components.

### Dependencies

*   **FE-LAYOUT-001:** Main layout uses header.
*   **FE-AUTH-002:** AuthService for currentUser and logout.
*   **FE-SHARED-002:** Button component (optional).

### Inputs

*   None (uses AuthService).

### Outputs

*   `toggleSidebar` – emitted when hamburger menu is clicked.

### Outputs (Files)

*   `shared/layouts/header/header.component.ts`
*   `header.component.html`
*   `header.component.scss`

### Detailed Workflow

#### Step 1: Generate Header Component

```bash
ng g c shared/layouts/header --standalone --skip-tests
```
#### Step 2: Implement Header Component

```typescript
// header.component.ts
import { Component, Output, EventEmitter, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { NgClass } from '@angular/common';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [NgClass],
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
    private authService = inject(AuthService);
    private router = inject(Router);

    @Output() toggleSidebar = new EventEmitter<void>();

    user = this.authService.currentUser;
    isDropdownOpen = false;

    toggleDropdown() {
        this.isDropdownOpen = !this.isDropdownOpen;
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }

    goToProfile() {
        this.router.navigate(['/auth/profile']);
        this.isDropdownOpen = false;
    }
}
```
#### Step 3: Create Template

```html
<!-- header.component.html -->
<header class="app-header">
    <div class="header-left">
        <button class="btn btn-ghost mobile-toggle" (click)="toggleSidebar.emit()">
            <i class="bi bi-list"></i>
        </button>
        <span class="header-title">Jalsa</span>
    </div>
    <div class="header-right">
        <div class="user-menu" (click)="toggleDropdown()">
            <span class="user-name">{{ user()?.firstName }} {{ user()?.lastName }}</span>
            <i class="bi bi-person-circle"></i>
            <i class="bi bi-chevron-down" [class.rotated]="isDropdownOpen"></i>
        </div>
        <div class="dropdown-menu" *if="isDropdownOpen">
            <a (click)="goToProfile()">Profile</a>
            <hr>
            <a (click)="logout()" class="text-danger">Logout</a>
        </div>
    </div>
</header>
```
#### Step 4: Add Styles

```css
// header.component.scss
.app-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.75rem 1.5rem;
    background: #fff;
    border-bottom: 1px solid $border-color;
    height: 64px;
    position: sticky;
    top: 0;
    z-index: 1000;
}

.header-left {
    display: flex;
    align-items: center;
    gap: 1rem;
}

.mobile-toggle {
    display: none;
    background: transparent;
    border: none;
    font-size: 1.5rem;
}

.header-title {
    font-size: 1.2rem;
    font-weight: 600;
}

.header-right {
    display: flex;
    align-items: center;
    position: relative;
}

.user-menu {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    cursor: pointer;
    padding: 0.25rem 0.5rem;
    border-radius: 4px;
    transition: background 0.15s;
}

.user-menu:hover {
    background: #f1f5f9;
}

.user-name {
    font-weight: 500;
}

.dropdown-menu {
    position: absolute;
    top: calc(100% + 8px);
    right: 0;
    background: #fff;
    border: 1px solid $border-color;
    border-radius: 8px;
    box-shadow: $box-shadow-lg;
    min-width: 160px;
    padding: 0.5rem 0;
    z-index: 1001;
}

.dropdown-menu a {
    display: block;
    padding: 0.5rem 1rem;
    cursor: pointer;
    text-decoration: none;
    color: $text-primary;
}

.dropdown-menu a:hover {
    background: #f1f5f9;
}

.dropdown-menu hr {
    margin: 0.25rem 0;
    border-color: $border-color;
}

.rotated {
    transform: rotate(180deg);
}

@media (max-width: 768px) {
    .mobile-toggle {
        display: block;
    }
    .header-title {
        font-size: 1rem;
    }
    .user-name {
        display: none;
    }
}
```
### Files To Create

*   `shared/layouts/header/header.component.ts`
*   `header.component.html`
*   `header.component.scss`

### CLI Commands

```bash
ng g c shared/layouts/header --standalone --skip-tests
```
### Concepts Required

*   EventEmitter for parent communication
*   Dropdown handling
*   Responsive design with media queries

### Testing Steps

1.  Verify user name and avatar display correctly.
2.  Test dropdown menu opens/closes.
3.  Test logout redirects to login.
4.  Test profile navigation.
5.  Test mobile toggle button.

### Expected Deliverables

*   Header component with user info and actions.

### Common Mistakes

**Mistake 1:** Not closing the dropdown when clicking outside.  
**Fix:** Use ClickOutsideDirective (from shared) to close dropdown.

### Edge Cases

*   User is null – show default or placeholder.
*   Mobile view – hide user name and show toggle.

### Definition of Done

*   Header displays user info and logout works.
*   Responsive behavior is correct.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Header is consistent across all pages.

## FE-LAYOUT-004Footer Component P1 Low 1h ▾

### Task Information

*   **Task ID:** FE-LAYOUT-004
*   **Task Name:** Footer Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-LAYOUT-001
*   **Complexity:** Low
*   **Estimated Effort:** 1 hour
*   **Priority:** Low

### Objective

Create a simple footer component that displays copyright information and any links (e.g., privacy policy).

### Business Purpose

Provide legal and copyright information in a consistent location.

### Technical Purpose

A Standalone component with static content, can be extended later.

### Prerequisites

*   Basic Angular component knowledge.

### Dependencies

*   **FE-LAYOUT-001:** Main layout uses footer.

### Inputs

*   None.

### Outputs

*   `shared/layouts/footer/footer.component.ts`
*   `footer.component.html`
*   `footer.component.scss`

### Detailed Workflow

#### Step 1: Generate Footer Component

```bash
ng g c shared/layouts/footer --standalone --skip-tests
```
#### Step 2: Implement Template

```html
<!-- footer.component.html -->
<footer class="app-footer">
    <div class="footer-content">
        <span>&copy; {{ currentYear }} Jalsa. All rights reserved.</span>
        <div class="footer-links">
            <a href="#">Privacy Policy</a>
            <a href="#">Terms of Service</a>
        </div>
    </div>
</footer>
```
#### Step 3: Add Logic

```typescript
// footer.component.ts
import { Component } from '@angular/core';

@Component({
    selector: 'app-footer',
    standalone: true,
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
})
export class FooterComponent {
    currentYear = new Date().getFullYear();
}
```
#### Step 4: Add Styles

```css
// footer.component.scss
.app-footer {
    background: #fff;
    border-top: 1px solid $border-color;
    padding: 0.75rem 1.5rem;
    font-size: 0.875rem;
    color: $text-muted;
}

.footer-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    gap: 0.5rem;
}

.footer-links {
    display: flex;
    gap: 1.5rem;
}

.footer-links a {
    color: $text-muted;
    text-decoration: none;
}

.footer-links a:hover {
    color: $primary;
    text-decoration: underline;
}
```
### Files To Create

*   `shared/layouts/footer/footer.component.ts`
*   `footer.component.html`
*   `footer.component.scss`

### CLI Commands

```bash
ng g c shared/layouts/footer --standalone --skip-tests
```
### Testing Steps

1.  Verify footer renders at the bottom of the page.

### Expected Deliverables

*   Footer component.

### Definition of Done

*   Footer is visible and contains copyright.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Footer is static; can be updated later.

## FE-LAYOUT-005Navigation Service & Role‑Based Menu P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-LAYOUT-005
*   **Task Name:** Navigation Service & Role‑Based Menu
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-AUTH-002 (AuthService), FE-LAYOUT-002 (Sidebar)
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Create a navigation service that provides menu items based on user roles, so the sidebar can consume a dynamic menu from a central source.

### Business Purpose

Centralises menu definition, making it easy to update navigation across the app. Also ensures consistency between sidebar and any other navigation elements.

### Technical Purpose

Define a `NavigationService` that returns an array of `NavItem` objects, filtered by roles. The sidebar will inject this service.

### Prerequisites

*   Understanding of Angular services and dependency injection.
*   Knowledge of RxJS or Signals (we'll use Signals for reactivity).

### Dependencies

*   **FE-AUTH-002:** AuthService for role checking.

### Inputs

*   None.

### Outputs

*   `core/services/navigation.service.ts`
*   Updated SidebarComponent to use the service.

### Detailed Workflow

#### Step 1: Generate Navigation Service

```bash
ng g s core/services/navigation --skip-tests
```
#### Step 2: Implement Navigation Service

```typescript
// core/services/navigation.service.ts
import { Injectable, inject, computed, signal, effect } from '@angular/core';
import { AuthService } from './auth.service';

export interface NavItem {
    label: string;
    icon: string;
    route: string;
    roles?: string[];
    children?: NavItem[];
}

@Injectable({
    providedIn: 'root',
})
export class NavigationService {
    private authService = inject(AuthService);

    // Define all menu items
    private allItems: NavItem[] = [
        { label: 'Dashboard', icon: 'bi-grid', route: '/dashboard' },
        { label: 'Patients', icon: 'bi-people', route: '/patients', roles: ['Therapist', 'Admin'] },
        { label: 'Sessions', icon: 'bi-calendar', route: '/sessions', roles: ['Therapist', 'Admin'] },
        { label: 'Exercises', icon: 'bi-clipboard', route: '/exercises', roles: ['Therapist', 'Admin'] },
        { label: 'Reports', icon: 'bi-file-text', route: '/reports', roles: ['Therapist', 'Admin'] },
        { label: 'Chat', icon: 'bi-chat', route: '/chatbot', roles: ['Patient'] },
    ];

    // Computed signal for filtered menu
    readonly menuItems = computed(() => {
        return this.allItems.filter(item => {
            if (!item.roles) return true;
            return this.authService.hasAnyRole(item.roles);
        });
    });

    // Optionally, an effect to log changes
    constructor() {
        effect(() => {
            console.log('Menu updated:', this.menuItems());
        });
    }
}
```
#### Step 3: Update Sidebar to Use NavigationService

```typescript
// sidebar.component.ts (updated)
import { NavigationService } from '../../../core/services/navigation.service';

export class SidebarComponent {
    private navService = inject(NavigationService);
    // ...
    menuItems = this.navService.menuItems;
    // remove local menuItems definition
}
```
#### Step 4: Update Template to Use the Signal

In sidebar template, change `filteredMenu` to `menuItems()`.

```html
<!-- sidebar.component.html -->
<li *for="let item of menuItems()" ...>
```
### Files To Create

*   `core/services/navigation.service.ts`

### CLI Commands

```bash
ng g s core/services/navigation --skip-tests
```
### Concepts Required

*   Angular Signals (`computed`, `effect`)
*   Dependency injection

### Testing Steps

1.  Test that menu items update when user roles change.
2.  Test that the sidebar reflects the filtered menu.

### Expected Deliverables

*   Navigation service with role‑based filtering.
*   Sidebar updated to use the service.

### Common Mistakes

**Mistake 1:** Not using `computed` to react to AuthService changes.  
**Fix:** Use `computed` to ensure reactivity.

### Definition of Done

*   Navigation service provides filtered menu.
*   Sidebar uses the service.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Add new nav items to NavigationService only.

## FE-LAYOUT-006RTL and Responsive Layout Integration P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-LAYOUT-006
*   **Task Name:** RTL and Responsive Layout Integration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-LAYOUT-001, FE-LAYOUT-002, FE-LAYOUT-003, FE-RTL-004
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Ensure the entire layout system works correctly in RTL mode and is fully responsive across all device sizes (mobile, tablet, desktop).

### Business Purpose

Jalsa serves Arabic‑speaking users; RTL support is non‑negotiable. Responsive design ensures a good experience on all devices.

### Technical Purpose

Apply CSS logical properties, use Bootstrap's RTL utilities, add media queries for responsiveness, and test thoroughly.

### Prerequisites

*   Understanding of CSS logical properties (e.g., `margin-inline-start`).
*   Knowledge of Bootstrap's RTL classes.
*   Media queries and responsive design.

### Dependencies

*   **FE-LAYOUT-001:** Main layout.
*   **FE-LAYOUT-002:** Sidebar.
*   **FE-RTL-004:** RTL validation (already done in Phase 3).

### Inputs

*   Current layout components.

### Outputs

*   Updated styles for all layout components to support RTL and responsiveness.
*   Media query adjustments.

### Detailed Workflow

#### Step 1: Apply Logical Properties

Replace `margin-left`, `margin-right`, `padding-left`, `padding-right` with `margin-inline-start`, `margin-inline-end`, etc. Or use Bootstrap's RTL‑aware classes.

For example, in sidebar:

```css
// Instead of margin-left: 0; use margin-inline-start: 0;
.sidebar {
    margin-inline-start: 0;
}
```
In main layout:

```css
.app-content {
    padding-inline: 1.5rem;
}
```
#### Step 2: Use Bootstrap RTL Utilities

Bootstrap provides classes like `.text-start`, `.text-end`, `.me-`, `.ms-` that automatically flip in RTL. Use these in templates.

#### Step 3: Adjust Sidebar Position in RTL

In RTL, the sidebar should be on the right. Use CSS order or flex direction.

```css
[dir="rtl"] .app-layout {
    flex-direction: row-reverse; // or use order
}
[dir="rtl"] .sidebar {
    order: 2;
}
[dir="rtl"] .app-main {
    order: 1;
}
```
#### Step 4: Responsive Design

Add media queries for mobile: sidebar collapses, header changes.

```css
@media (max-width: 768px) {
    .sidebar {
        position: fixed;
        top: 0;
        left: 0;
        bottom: 0;
        z-index: 1050;
        transform: translateX(-100%);
        transition: transform 0.3s ease;
        width: 260px;
    }
    .sidebar.open {
        transform: translateX(0);
    }
    .app-main {
        margin-left: 0;
        width: 100%;
    }
    [dir="rtl"] .sidebar {
        left: auto;
        right: 0;
        transform: translateX(100%);
    }
    [dir="rtl"] .sidebar.open {
        transform: translateX(0);
    }
    .mobile-toggle {
        display: block;
    }
}
```
#### Step 5: Update Sidebar Toggle for Mobile

On mobile, the sidebar should overlay the content, not push it. Use a backdrop overlay.

Add an overlay element in main layout when sidebar is open on mobile.

#### Step 6: Test Thoroughly

Test on Chrome, Firefox, Safari, with RTL and LTR, and on various screen sizes.

### Files To Modify

*   `shared/layouts/main-layout/main-layout.component.scss`
*   `shared/layouts/sidebar/sidebar.component.scss`
*   `shared/layouts/header/header.component.scss`
*   `shared/layouts/footer/footer.component.scss`

### CLI Commands

No CLI commands; manual editing.

### Concepts Required

*   CSS logical properties
*   RTL layout
*   Responsive design
*   Flexbox

### Tools/Libraries Required

*   Bootstrap RTL
*   Browser dev tools for testing

### Testing Steps

1.  Set `dir="rtl"` on `html` element and verify layout mirrors.
2.  Test on mobile (320px width) – sidebar should be hidden and toggleable.
3.  Test on tablet (768px) and desktop (1024px+).
4.  Test dropdowns, navigation, and content rendering.

### Expected Deliverables

*   Layout fully RTL‑compatible.
*   Fully responsive layout.

### Common Mistakes

**Mistake 1:** Using `left` and `right` instead of `inset-inline-start` etc.  
**Fix:** Use logical properties or Bootstrap utilities.

**Mistake 2:** Not testing on actual Arabic text (which might have different lengths).  
**Fix:** Test with Arabic content.

### Edge Cases

*   Sidebar overlay on mobile should close when clicking outside.
*   Mixed content (English numbers) in RTL should still look correct.

### Definition of Done

*   All layout components work flawlessly in RTL.
*   Responsive breakpoints are correct.
*   Mobile sidebar toggle works.

### Frontend Architecture Notes

*   We use Bootstrap's built‑in RTL support and logical CSS for maximum compatibility.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** When adding new components, test in RTL and on mobile.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 6 layout tasks are complete (FE-LAYOUT-001 to FE-LAYOUT-006).
*   Main layout component wraps all authenticated routes.
*   Sidebar displays role‑based navigation items and collapses.
*   Header shows user info, logout, and mobile toggle.
*   Footer is present.
*   Navigation service centralizes menu items.
*   Layout is fully responsive and works on mobile, tablet, desktop.
*   RTL support is complete and tested.
*   All components are Standalone.
*   All changes are committed.

📋 Code Review Checklist
------------------------

*   Main layout uses `<router-outlet>` correctly.
*   Sidebar menu items are filtered by roles.
*   Header logout clears session and redirects.
*   Navigation service uses Signals and reactivity.
*   RTL styles are applied via logical properties or Bootstrap classes.
*   Responsive breakpoints work as expected.

🚀 Pull Request Checklist
-------------------------

*   Branch is up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   RTL and responsive tested manually.

🧪 Deployment Readiness Checklist
---------------------------------

*   Layout works for all user roles.
*   Navigation is correct based on permissions.
*   RTL and responsive are verified.

* * *

Jalsa – Phase 6: Layout System – Expanded Implementation Handbook • v1.0 • For development team

