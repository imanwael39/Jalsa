
🏗️ Jalsa – Phase 1: Foundation Setup Expanded Implementation Handbook · 7 Tasks · 35+ Subtasks
===============================================================================================

🏗️ Phase 1 – Foundation Setup
------------------------------

**Purpose:** Create the initial Angular 21 project, install all necessary dependencies, configure environment files, set up the folder structure, and prepare the development environment so that the team can start building features immediately. This phase transforms the planning documents into a runnable application skeleton.

📋 Tasks: 7 ⏱️ Total Effort: ~20 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 0 (Planning & Analysis) must be complete 🎯 Deliverable: Fully configured Angular project with all tooling and base structure

## FE-SETUP-001Angular Project Initialization P0 Medium 3h 

### Task Information

*   **Task ID:** FE-SETUP-001
*   **Task Name:** Angular Project Initialization
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** Phase 0 complete (planning documents, tooling)
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Create a new Angular 21 project using the Standalone architecture, with SCSS as the stylesheet format, routing enabled, and no server‑side rendering (SSR) for simplicity.

### Business Purpose

This is the starting point for all frontend development. A properly initialized project saves hours of configuration later and ensures we follow best practices from the beginning.

### Technical Purpose

Generate the base Angular project with the correct configuration: Standalone components, SCSS, routing, and minimal dependencies. This provides a clean foundation for the team to build upon.

### Prerequisites

#### Concepts Required

*   **Node.js and npm:** JavaScript runtime and package manager. Ensure Node.js 18+ is installed.
*   **Angular CLI:** Command‑line interface for Angular. Install globally with `npm install -g @angular/cli`.
*   **Standalone Architecture:** Angular 21+ default; no NgModules needed.

#### Access Required

*   Git repository (create one if not exists)
*   Terminal/command prompt access

### Dependencies

*   **Phase 0:** Planning documents provide the project name and structure.

### Inputs

*   Project name: `jalsa-frontend` (or as decided)
*   Folder location for the new project

### Outputs

*   A new Angular 21 project directory with all base files.
*   `package.json` with initial dependencies.
*   `angular.json` with build configuration.
*   `tsconfig.json` with strict TypeScript settings.
*   `src/` folder with default app component (Standalone).

### Detailed Workflow

#### Step 1: Verify Node.js and npm

**What to do:** Run `node -v` and `npm -v` to ensure versions are correct.

**Why it is required:** The CLI requires Node.js 18+ and npm 6+.

**Expected result:** Node v18.x.x and npm v9.x.x or higher.

node -v \# Should be v18 or higher npm -v \# Should be v9 or higher

#### Step 2: Install Angular CLI Globally (if not already)

**What to do:** Install the Angular CLI globally using npm.

**Why it is required:** The `ng` command is used to create and manage the project.

**Expected result:** `ng version` shows Angular CLI 17+.

npm install -g @angular/cli@17

#### Step 3: Create the Angular Project

**What to do:** Run the `ng new` command with appropriate flags.

**Why it is required:** This generates the entire project skeleton.

**Expected result:** A new folder named `jalsa-frontend` with all initial files.

ng new jalsa-frontend --standalone --style=scss --routing --ssr=false

**Explanation of flags:**

*   `--standalone` – Use Standalone components (no NgModules).
*   `--style=scss` – Use SCSS for styles.
*   `--routing` – Generate a routing module (app.routes.ts).
*   `--ssr=false` – Disable server‑side rendering (we use SPA).

#### Step 4: Navigate into the Project Directory

**What to do:** Change directory to the new project.

**Why it is required:** Subsequent commands must run inside the project.

**Expected result:** Inside `jalsa-frontend` folder.

cd jalsa-frontend

#### Step 5: Install Initial Dependencies (optional)

Angular CLI automatically installs base packages. We will add more in later tasks.

#### Step 6: Test the Application

**What to do:** Run `ng serve` and open `http://localhost:4200` in a browser.

**Why it is required:** Verify the project builds correctly.

**Expected result:** The default Angular welcome page appears.

ng serve \# Open http://localhost:4200

#### Step 7: Initialize Git Repository (if not already)

**What to do:** If not created automatically, initialize Git and make the first commit.

**Why it is required:** Version control is essential.

**Expected result:** Git repository with initial commit.

git init git add . git commit -m "feat: initialize Angular project"

### Folder Structure

After creation, the project has the following key files:
```
jalsa-frontend/
├── .angular/                     # Cache
├── .vscode/                      # (if configured)
├── node_modules/                 # Dependencies
├── src/
│   ├── app/
│   │   ├── app.component.html
│   │   ├── app.component.scss
│   │   ├── app.component.spec.ts
│   │   ├── app.component.ts      # Standalone root component
│   │   ├── app.config.ts          # Application configuration
│   │   └── app.routes.ts          # Root routing
│   ├── assets/
│   ├── environments/             # (will create)
│   ├── favicon.ico
│   ├── index.html
│   ├── main.ts                    # Bootstrap
│   └── styles.scss                # Global styles
├── .editorconfig
├── .gitignore
├── angular.json                   # Build configuration
├── package.json
├── tsconfig.app.json
├── tsconfig.json
└── tsconfig.spec.json
```
### Files To Create

This task generates all files automatically via CLI. No manual creation needed.

### CLI Commands
```
# Install CLI (if not global)
npm install -g @angular/cli@17

# Create project
ng new jalsa-frontend --standalone --style=scss --routing --ssr=false

# Navigate
cd jalsa-frontend

# Serve
ng serve
```
### Code Flow

main.ts → bootstrapApplication(AppComponent, appConfig) → AppComponent → AppComponent template

### Concepts Required

*   Angular CLI
*   Standalone components
*   Bootstrapping an Angular application

### Tools/Libraries Required

*   Node.js 18+
*   Angular CLI 17
*   Git

### Angular Concepts Required

*   `main.ts` bootstrapping with `bootstrapApplication`
*   `app.config.ts` for providers
*   `app.routes.ts` for routing

### Testing Steps

1.  Run `ng serve` and verify the app loads at localhost:4200.
2.  Run `ng build` and verify it builds without errors.
3.  Run `ng test` to ensure default tests pass.

### Expected Deliverables

*   A fully functional Angular project.

### Acceptance Criteria

*   The project builds with `ng build`.
*   The app serves with `ng serve` and displays the default page.
*   Standalone flag is active (no `AppModule`).
*   SCSS is configured correctly.
*   Routing is enabled.

### Common Mistakes

**Mistake 1:** Using an older Angular CLI version.  
**Fix:** Check `ng version` and update with `npm install -g @angular/cli@latest`.

**Mistake 2:** Forgetting the `--standalone` flag, resulting in a module‑based project.  
**Fix:** Delete and recreate with the flag.

**Mistake 3:** Not using `--ssr=false`, which adds unnecessary complexity.  
**Fix:** Regenerate without SSR.

### Edge Cases

*   **Port 4200 already in use:** Use `ng serve --port 4201`.
*   **npm install fails:** Clear cache with `npm cache clean --force` and retry.

### Definition of Done

*   Project created and builds successfully.
*   App runs at localhost:4200.
*   Initial commit pushed to repository.

### Frontend Architecture Notes

*   We use Standalone to reduce boilerplate and improve tree‑shaking.
*   All future components will be Standalone.

### Team Handoff Notes

*   **To:** All frontend developers.
*   **Key Takeaways:** The project skeleton is ready. Next task: Environment Configuration.

## FE-SETUP-002Environment Configuration P0 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SETUP-002
*   **Task Name:** Environment Configuration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-001
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Critical

### Objective

Set up environment files for development, staging, and production. Centralize all environment‑specific variables (API URLs, SignalR hub URLs, feature flags, etc.) so that the application can be deployed to different environments with zero code changes.

### Business Purpose

Enable seamless deployment across different environments (local, staging, production) without modifying code. This reduces human error and accelerates the release process.

### Technical Purpose

Use Angular's built‑in environment replacement feature to swap configurations at build time. All environment‑specific values are defined in `environment.ts` files.

### Prerequisites

*   Understanding of Angular environment files.
*   Knowledge of the API endpoints (from Phase 0).

### Dependencies

*   **FE-SETUP-001:** Project must exist.
*   **Phase 0:** API contracts define the base URLs.

### Inputs

*   API base URL for each environment (e.g., local: `http://localhost:5014`, staging: `https://staging-api.jalsa.com`, production: `https://api.jalsa.com`).
*   SignalR hub URL for each environment.
*   AI service URL if separate.

### Outputs

*   `src/environments/environment.ts` – Development defaults.
*   `src/environments/environment.prod.ts` – Production values.
*   `src/environments/environment.staging.ts` – Staging values (optional).
*   Updated `angular.json` to recognize staging and production configurations.

### Detailed Workflow

#### Step 1: Create the Environments Folder

**What to do:** If not present, create `src/environments/`.

**Why it is required:** Angular CLI uses this folder by convention.

**Expected result:** A folder with environment files.

mkdir -p src/environments

#### Step 2: Define Environment Interfaces

**What to do:** Create a type or interface to enforce consistency across environment files.

**Why it is required:** Ensures all environment files have the same properties.

**Expected result:** `src/environments/environment.model.ts`.
```
// src/environments/environment.model.ts
export interface Environment {
    production: boolean;
    apiUrl: string;
    signalRHubUrl: string;
    aiServiceUrl: string;
    appName: string;
    enableMockApi: boolean;
    logLevel: 'debug' | 'info' | 'warn' | 'error';
}
```
#### Step 3: Create Environment Files

**What to do:** Create three files: development, staging, production.

**Why it is required:** Each environment has different values.

**Expected result:** Three environment files.
```
// src/environments/environment.ts (development)
import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'http://localhost:5014',
    signalRHubUrl: 'http://localhost:5014/chatHub',
    aiServiceUrl: 'http://localhost:5001',
    appName: 'Jalsa (Dev)',
    enableMockApi: true,
    logLevel: 'debug',
};
// src/environments/environment.staging.ts
import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'https://staging-api.jalsa.com/api',
    signalRHubUrl: 'https://staging-api.jalsa.com/chatHub',
    aiServiceUrl: 'https://staging-ai.jalsa.com',
    appName: 'Jalsa (Staging)',
    enableMockApi: false,
    logLevel: 'info',
};
// src/environments/environment.prod.ts
import { Environment } from './environment.model';

export const environment: Environment = {
    production: true,
    apiUrl: 'https://api.jalsa.com/api',
    signalRHubUrl: 'https://api.jalsa.com/chatHub',
    aiServiceUrl: 'https://ai.jalsa.com',
    appName: 'Jalsa',
    enableMockApi: false,
    logLevel: 'error',
};
```
#### Step 4: Update angular.json to Include Staging and Production Configurations

**What to do:** Modify `angular.json` to add `staging` and `production` configurations for the `build` and `serve` targets.

**Why it is required:** So that `ng build --configuration=production` uses the correct environment file.

**Expected result:** Angular builds with environment‑specific values.
```
// In angular.json, under projects > jalsa-frontend > architect > build > configurations
"staging": {
    "fileReplacements": [
        {
            "src": "src/environments/environment.ts",
            "replace": "src/environments/environment.staging.ts"
        }
    ],
    "optimization": true,
    "outputHashing": "all",
    "sourceMap": false,
    "namedChunks": false,
    "extractLicenses": true,
    "vendorChunk": false,
    "buildOptimizer": true,
    "budgets": [ /* ... */ ]
},
"production": {
    "fileReplacements": [
        {
            "src": "src/environments/environment.ts",
            "replace": "src/environments/environment.prod.ts"
        }
    ],
    // ... same as staging
}
```
Also update the `serve` target to allow `--configuration=staging` and `--configuration=production`.

#### Step 5: Test Environment Switching

**What to do:** Run `ng serve --configuration=staging` and verify the environment values are used.

**Why it is required:** Confirm the configuration works.

**Expected result:** The application runs with staging values.

ng serve --configuration=staging

### Files To Create

*   `src/environments/environment.model.ts`
*   `src/environments/environment.ts`
*   `src/environments/environment.staging.ts`
*   `src/environments/environment.prod.ts`

### CLI Commands
```
# Build for production
ng build --configuration=production

# Serve with staging
ng serve --configuration=staging
```
### Concepts Required

*   Angular environment files
*   Build configurations
*   File replacement

### Tools/Libraries Required

*   Angular CLI

### Testing Steps

1.  Add a temporary console log in `app.component.ts` to print `environment.apiUrl`.
2.  Run with different configurations and verify the correct URL is printed.
3.  Remove the log after testing.

### Expected Deliverables

*   All environment files created and configured.
*   `angular.json` updated with staging and production configurations.

### Common Mistakes

**Mistake 1:** Forgetting to add file replacement configurations in angular.json.  
**Fix:** Verify the fileReplacements array exists for each environment.

**Mistake 2:** Using environment variables in components without importing the correct interface.  
**Fix:** Always import `environment` from the correct file.

### Edge Cases

*   **Missing API URL:** Provide a fallback or error if environment is not defined.

### Definition of Done

*   All environment files exist.
*   Builds for each environment succeed.
*   Environment values are correctly used in the app.

### Frontend Architecture Notes

*   Environment variables are injected at build time, not runtime.
*   This approach is simple and secure; no secrets are exposed on the client.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `environment.apiUrl` for all API calls; never hardcode URLs.

## FE-SETUP-003Install and Configure Dependencies P0 High 4h ▾

### Task Information

*   **Task ID:** FE-SETUP-003
*   **Task Name:** Install and Configure Dependencies
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-001
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Install all required third‑party libraries: Bootstrap with RTL, SCSS tooling, SignalR client, Chart.js, and any other dependencies identified in the planning phase. Configure them correctly so they are ready to use.

### Business Purpose

These libraries are the building blocks of the user interface and real‑time communication. Without them, the application cannot be built as designed.

### Technical Purpose

Add and configure dependencies to support:

*   Styling (Bootstrap RTL, SCSS)
*   Real‑time communication (SignalR)
*   Charts (Chart.js)
*   Additional utilities (jwt‑decode, etc.)

### Prerequisites

*   Angular project exists.
*   Knowledge of npm install and package.json.

### Dependencies

*   **FE-SETUP-001:** Project must exist.

### Inputs

*   List of required packages and their versions.

### Outputs

*   `package.json` updated with all dependencies.
*   `node_modules/` installed.
*   Configuration files for each library (if needed).
*   Global styles importing Bootstrap and custom SCSS.

### Detailed Workflow

#### Step 1: Install Core Dependencies

**What to do:** Install the following packages:

*   `bootstrap`
*   `@popperjs/core` (required by Bootstrap)
*   `bootstrap-rtl` (for RTL support)
*   `chart.js`
*   `ng2-charts` (Angular wrapper for Chart.js)
*   `@microsoft/signalr` (SignalR client)
*   `jwt-decode` (for JWT decoding)
*   `@angular/material` and `@angular/cdk` (optional, but may be used for advanced UI)

**Why it is required:** These libraries provide essential functionality.

**Expected result:** All packages are installed.
```
npm install bootstrap @popperjs/core bootstrap-rtl npm install chart.js ng2-charts npm install @microsoft/signalr npm install jwt-decodenpm install bootstrap @popperjs/core bootstrap-rtl
npm install chart.js ng2-charts
npm install @microsoft/signalr
npm install jwt-decode
```
#### Step 2: Configure Bootstrap RTL in Global Styles

**What to do:** Import Bootstrap and Bootstrap RTL in `src/styles.scss`.

**Why it is required:** Bootstrap styles must be available globally.

**Expected result:** Styles are applied.
```
// src/styles.scss
// Order matters: Bootstrap first, then RTL override
@import 'bootstrap/scss/bootstrap';
@import 'bootstrap-rtl/scss/bootstrap-rtl';

// Custom variables and overrides
@import './variables';
@import './typography';
@import './utilities';
```
#### Step 3: Configure Chart.js

**What to do:** No specific configuration needed; just import the module when using charts.

**Why it is required:** Charts are used in the dashboard.

#### Step 4: Configure SignalR

**What to do:** Install and ensure the SignalR client is available. We'll create a service later.

#### Step 5: Configure Angular JSON for Bootstrap Styles

**What to do:** Ensure the styles are correctly referenced in `angular.json`.

**Why it is required:** The build process includes the styles.

**Expected result:** `styles` array includes `src/styles.scss`.

This is already set by default.

#### Step 6: Verify Installation

**What to do:** Try using a Bootstrap component in the app template to ensure styles load.

**Why it is required:** Confirm everything works.

// app.component.html <button class="btn btn-primary">Test Bootstrap</button>

### Files To Create

*   None new; only modifications to `styles.scss`.

### CLI Commands
```
\# Install all 
npm install bootstrap @popperjs/core bootstrap-rtl chart.js ng2-charts @microsoft/signalr jwt-decode
```
### Concepts Required

*   npm package management
*   SCSS imports
*   RTL styling

### Tools/Libraries Required

*   Bootstrap 5
*   Chart.js
*   SignalR

### Angular Concepts Required

*   Global styles
*   Importing third‑party libraries

### Testing Steps

1.  After installation, add a Bootstrap button and verify it appears with correct styles.
2.  Check that RTL works (e.g., text direction).

### Expected Deliverables

*   All dependencies installed and configured.
*   Global styles import Bootstrap with RTL support.

### Common Mistakes

**Mistake 1:** Forgetting to install `@popperjs/core` (required for Bootstrap dropdowns/modals).  
**Fix:** Always install the peer dependencies.

**Mistake 2:** Importing Bootstrap RTL before Bootstrap (order matters).  
**Fix:** Import Bootstrap first, then Bootstrap RTL.

### Edge Cases

*   **Version conflicts:** Use `--save-exact` or pin versions to avoid breaking changes.

### Definition of Done

*   All dependencies installed.
*   Bootstrap RTL works in the app.
*   SignalR and Chart.js packages are available.

### Frontend Architecture Notes

*   We use Bootstrap for layout and components; we can use custom SCSS for branding.
*   SignalR will be used for real‑time chat and notifications.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Libraries are ready to use in components.

## FE-SETUP-004Folder Structure and Scaffolding P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-SETUP-004
*   **Task Name:** Folder Structure and Scaffolding
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-001
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Create the complete folder structure as defined in the architecture design. Create placeholder files for core services, guards, interceptors, models, and shared components so that the team can start implementing features immediately.

### Business Purpose

An organised folder structure improves maintainability, reduces confusion, and accelerates onboarding. It provides a home for every piece of code.

### Technical Purpose

Establish the base folders and files (e.g., `core/`, `shared/`, `features/`) with barrel exports and placeholder services. This reduces friction when developers start adding features.

### Prerequisites

*   Understanding of the architecture from Phase 0.
*   Knowledge of Angular project structure.

### Dependencies

*   **FE-SETUP-001:** Project exists.

### Inputs

*   Architecture document (folder structure).

### Outputs

*   Complete folder structure under `src/app/`.
*   Placeholder files (e.g., `core/services/.gitkeep`, `shared/components/.gitkeep`).
*   Barrel index files (optional).
*   Base model files (e.g., `core/models/user.model.ts`).

### Detailed Workflow

#### Step 1: Create the Main Folders

**What to do:** Using the terminal, create the following folders inside `src/app/`:

*   `core/`
*   `shared/`
*   `features/`

**Why it is required:** These are the top‑level separation of concerns.

**Expected result:** Folders exist.

```
cd src/app
mkdir core shared features
# Inside core:
cd core && mkdir api guards interceptors models services state
cd ../shared && mkdir components directives pipes layouts
cd ../features && mkdir auth patients sessions exercises reports dashboard chatbot
```

#### Step 2: Create Placeholder Files

**What to do:** Add a `.gitkeep` file in each folder to ensure they are tracked by Git (if empty).

**Why it is required:** Git ignores empty directories.
```
touch core/api/.gitkeep core/guards/.gitkeep core/interceptors/.gitkeep core/models/.gitkeep core/services/.gitkeep core/state/.gitkeep
touch shared/components/.gitkeep shared/directives/.gitkeep shared/pipes/.gitkeep shared/layouts/.gitkeep
# For features, we can add a .gitkeep in each
touch features/auth/.gitkeep features/patients/.gitkeep ...
```
#### Step 3: Create Initial Model Files

**What to do:** Create some base model files as per the API contract.

**Why it is required:** Early scaffolding speeds up development.

**Expected result:** `core/models/user.model.ts`, `core/models/api-response.model.ts`.
```
// core/models/user.model.ts
export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
}
// core/models/api-response.model.ts
export interface ApiResponse<T> {
    data: T;
    message: string;
    success: boolean;
    errors?: string[];
}
```
#### Step 4: Create Barrel Exports (optional)

**What to do:** Create `index.ts` files to export all files in a folder.

**Why it is required:** Simplifies imports (e.g., `import { User } from '@core/models';`).
```
// core/models/index.ts
 export \* from './user.model';
export \* from './api-response.model';
```
Repeat for other folders as needed.

#### Step 5: Create Root Feature Route Placeholders

**What to do:** In each feature folder, create a routes file placeholder (e.g., `auth.routes.ts`).

**Why it is required:** Later tasks will fill these with actual routes.
```
// features/auth/auth.routes.ts 
import { Routes } from '@angular/router';
export const AUTH\_ROUTES: Routes = \[\];
```
Repeat for other features.

### Folder Structure (Final)

The final folder structure should match the design from Phase 0.

### Files To Create

*   All folders and placeholder files.
*   `core/models/user.model.ts`
*   `core/models/api-response.model.ts`
*   `features/auth/auth.routes.ts`
*   `features/patients/patients.routes.ts`
*   `features/sessions/sessions.routes.ts`
*   `features/exercises/exercises.routes.ts`
*   `features/reports/reports.routes.ts`
*   `features/dashboard/dashboard.routes.ts`
*   `features/chatbot/chatbot.routes.ts`
*   Barrel `index.ts` files where beneficial.

### CLI Commands
```
# Create folder structure (run from src/app)
mkdir -p core/api core/guards core/interceptors core/models core/services core/state
mkdir -p shared/components shared/directives shared/pipes shared/layouts
mkdir -p features/auth features/patients features/sessions features/exercises features/reports features/dashboard features/chatbot

# Create placeholder files
touch core/api/.gitkeep core/guards/.gitkeep core/interceptors/.gitkeep core/models/.gitkeep core/services/.gitkeep core/state/.gitkeep
touch shared/components/.gitkeep shared/directives/.gitkeep shared/pipes/.gitkeep shared/layouts/.gitkeep
touch features/auth/auth.routes.ts features/patients/patients.routes.ts features/sessions/sessions.routes.ts features/exercises/exercises.routes.ts features/reports/reports.routes.ts features/dashboard/dashboard.routes.ts features/chatbot/chatbot.routes.ts
```
### Concepts Required

*   Angular project structure
*   Barrel exports
*   Lazy loading (routes)

### Tools/Libraries Required

*   Terminal / command prompt

### Testing Steps

1.  Verify all folders exist and contain placeholder files.
2.  Run `ng build` to ensure no errors from empty files.

### Expected Deliverables

*   A well‑organised folder structure.

### Common Mistakes

**Mistake 1:** Forgetting to create `.gitkeep` files, causing empty folders to not be tracked.  
**Fix:** Add `.gitkeep` in each empty folder.

**Mistake 2:** Naming inconsistencies (e.g., using singular vs plural).  
**Fix:** Follow the agreed naming conventions from architecture.

### Edge Cases

*   None significant.

### Definition of Done

*   All folders and placeholder files are created.
*   Project builds without errors.

### Frontend Architecture Notes

*   The folder structure is designed to scale with the project.
*   Each feature is lazy‑loaded.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Place your feature files in the appropriate folders.

## FE-SETUP-005Routing and Application Configuration P0 Medium 3h ▾

### Task Information

*   **Task ID:** FE-SETUP-005
*   **Task Name:** Routing and Application Configuration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-004 (folder structure)
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** Critical

### Objective

Set up the root routing (`app.routes.ts`) with lazy loading for all feature modules. Configure `app.config.ts` with necessary providers (HttpClient, etc.).

### Business Purpose

Routing enables navigation between features. Lazy loading ensures fast initial load times by only loading the code needed for the current view.

### Technical Purpose

Define the application's route structure and register global providers (e.g., `provideHttpClient`, `provideAnimations`).

### Prerequisites

*   Understanding of Angular routing.
*   Knowledge of standalone application configuration.

### Dependencies

*   **FE-SETUP-004:** Feature routes files exist.

### Inputs

*   Feature route definitions (will be added later).

### Outputs

*   `src/app/app.routes.ts` with lazy loading.
*   `src/app/app.config.ts` with providers.

### Detailed Workflow

#### Step 1: Update `app.routes.ts`

**What to do:** Define routes for each feature using `loadChildren`.

**Why it is required:** This enables lazy loading.

**Expected result:** A route configuration that loads features on demand.
```
// src/app/app.routes.ts
import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
    },
    {
        path: 'patients',
        loadChildren: () => import('./features/patients/patients.routes').then(m => m.PATIENTS_ROUTES),
    },
    {
        path: 'sessions',
        loadChildren: () => import('./features/sessions/sessions.routes').then(m => m.SESSIONS_ROUTES),
    },
    {
        path: 'exercises',
        loadChildren: () => import('./features/exercises/exercises.routes').then(m => m.EXERCISES_ROUTES),
    },
    {
        path: 'reports',
        loadChildren: () => import('./features/reports/reports.routes').then(m => m.REPORTS_ROUTES),
    },
    {
        path: 'dashboard',
        loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
    },
    {
        path: 'chatbot',
        loadChildren: () => import('./features/chatbot/chatbot.routes').then(m => m.CHATBOT_ROUTES),
    },
    {
        path: '',
        redirectTo: 'auth/login',
        pathMatch: 'full',
    },
    {
        path: '**',
        redirectTo: 'auth/login',
    },
];
```
#### Step 2: Update `app.config.ts`

**What to do:** Provide necessary services and configurations.

**Why it is required:** This is where we configure HttpClient, animations, etc.

**Expected result:** A configuration object.
```
// src/app/app.config.ts
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideHttpClient(),
        provideAnimations(),
        // Add other providers like interceptors later
    ],
};
```
#### Step 3: Update `main.ts` to use `appConfig`

**What to do:** Ensure `main.ts` bootstraps with the configuration.

**Why it is required:** The app must use the providers defined.

**Expected result:** `main.ts` uses `bootstrapApplication` with `appConfig`.
```
// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

bootstrapApplication(AppComponent, appConfig)
    .catch((err) => console.error(err));
```
#### Step 4: Update Feature Route Placeholders (Optional)

We already created placeholder route files. For now, they can be empty arrays. Later tasks will add actual routes.

### Files To Create / Modify

*   `src/app/app.routes.ts` – created.
*   `src/app/app.config.ts` – created.
*   `src/main.ts` – modified to use `appConfig`.

### CLI Commands

None; manual file editing.

### Concepts Required

*   Lazy loading with `loadChildren`
*   `ApplicationConfig` and `provideRouter`
*   `bootstrapApplication`

### Testing Steps

1.  Run `ng serve` and navigate to `/auth/login` (should redirect if no route).
2.  Check that lazy loading works (network tab should show chunk loading).

### Expected Deliverables

*   Working routing with lazy loading.
*   Application configured with HttpClient and animations.

### Common Mistakes

**Mistake 1:** Incorrect import path in `loadChildren` (must be a function that returns a Promise).  
**Fix:** Use `() => import('./path').then(m => m.MODULE_ROUTES)`.

**Mistake 2:** Forgetting to add `provideHttpClient()` in providers.  
**Fix:** Always include `provideHttpClient()` to use HttpClient.

### Edge Cases

*   If a feature route is not defined, the app will redirect to auth/login.

### Definition of Done

*   Root routing is set up with lazy loading.
*   Application providers are configured.
*   App runs without errors.

### Frontend Architecture Notes

*   Lazy loading reduces initial bundle size.
*   All features are prefixed with their module name.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Add new routes to `app.routes.ts`; feature routes are lazy‑loaded.

## FE-SETUP-006Development Proxy Configuration P1 Medium 1h ▾

### Task Information

*   **Task ID:** FE-SETUP-006
*   **Task Name:** Development Proxy Configuration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SETUP-002 (environment)
*   **Complexity:** Medium
*   **Estimated Effort:** 1 hour
*   **Priority:** High

### Objective

Set up a proxy configuration for the Angular development server to forward API requests to the backend, avoiding CORS issues during development.

### Business Purpose

During development, the frontend runs on `localhost:4200` and the backend on a different port. A proxy forwards requests so that the frontend can call the backend as if they were on the same origin.

### Technical Purpose

Create a `proxy.conf.json` file and configure the Angular CLI to use it. This simplifies API calls and avoids CORS preflight issues.

### Prerequisites

*   Backend API is running on a known URL (e.g., `http://localhost:5014`).
*   Knowledge of proxy configuration.

### Dependencies

*   **FE-SETUP-002:** Environment files define the API URL.

### Inputs

*   Backend API base URL (e.g., `http://localhost:5014`).

### Outputs

*   `proxy.conf.json` in the project root.
*   Updated `angular.json` to use the proxy when serving.

### Detailed Workflow

#### Step 1: Create `proxy.conf.json`

**What to do:** Create a JSON file that maps API routes to the backend.

**Why it is required:** The proxy will rewrite requests.

**Expected result:** A file with proxy rules.
```
{
    "/api": {
        "target": "http://localhost:5014",
        "secure": false,
        "changeOrigin": true,
        "logLevel": "debug"
    },
    "/chatHub": {
        "target": "http://localhost:5014",
        "secure": false,
        "changeOrigin": true,
        "ws": true
    }
}
```
#### Step 2: Update `angular.json`

**What to do:** Add the proxy configuration to the `serve` target.

**Why it is required:** The CLI needs to know about the proxy.
```
// In angular.json, under projects > jalsa-frontend > architect > serve > options
"proxyConfig": "proxy.conf.json"
```
#### Step 3: Test the Proxy

**What to do:** Start the dev server and make an API call to `/api/...`.

**Why it is required:** Verify that requests are forwarded.

**Expected result:** API calls succeed without CORS errors.

ng serve

### Files To Create

*   `proxy.conf.json`

### CLI Commands

```
# Create proxy.conf.json manually
# Then serve 
ng serve
```

### Concepts Required

*   CORS
*   HTTP proxy
*   Angular CLI proxy configuration

### Tools/Libraries Required

*   None

### Testing Steps

1.  Ensure backend is running.
2.  Make a fetch to `/api/patients` from the frontend.
3.  Check network tab to see the request is proxied to the backend.

### Expected Deliverables

*   Proxy configuration file.
*   Angular CLI serves with proxy.

### Common Mistakes

**Mistake 1:** Forgetting to add the proxyConfig path correctly.  
**Fix:** Ensure the path is relative to the project root.

**Mistake 2:** Not including `changeOrigin: true` for virtual hosts.  
**Fix:** Always set `changeOrigin` to true.

### Edge Cases

*   If backend uses HTTPS, set `secure: true` and provide certificate.

### Definition of Done

*   Proxy works and API calls are forwarded.

### Frontend Architecture Notes

*   Proxy is only used in development; in production, API calls go directly to the backend URL.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use the proxy to avoid CORS issues.

## FE-SETUP-007Validate and Commit Setup P0 Medium 2h ▾

### Task Information

*   **Task ID:** FE-SETUP-007
*   **Task Name:** Validate and Commit Setup
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous FE-SETUP tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Critical

### Objective

Perform a final validation of the entire foundation setup: run all tests, build the project, check linting, and commit all changes to the repository with a meaningful message.

### Business Purpose

Ensure the foundation is stable before starting feature development. A broken foundation will cause delays later.

### Technical Purpose

Verify that all configuration works together: environment switching, routing, dependency imports, linting, and building. Also, commit the baseline so that the team can branch from a known good state.

### Prerequisites

*   All previous tasks completed.
*   Git repository set up.

### Dependencies

*   All FE-SETUP-001 to FE-SETUP-006.

### Inputs

*   All code and configuration files.

### Outputs

*   Successful builds and tests.
*   Commit pushed to repository.

### Detailed Workflow

#### Step 1: Run Lint Check

**What to do:** Run `npm run lint` (or `ng lint`) to ensure no linting errors.

**Why it is required:** Code quality must be maintained.

**Expected result:** No errors.
```
npm run lint
```
#### Step 2: Run Tests

**What to do:** Run unit tests to ensure they pass.

**Why it is required:** Ensure default tests are not broken.
```
ng test --watch=false
```
#### Step 3: Build for Development

**What to do:** Build the app to check for compile errors.
```
ng build
```
#### Step 4: Build for Production

**What to do:** Build with production configuration.
```
ng build --configuration=production
```
#### Step 5: Serve and Test Manually

**What to do:** Run `ng serve` and navigate through the app (though there are no features yet, ensure the default route works).

#### Step 6: Commit All Changes

**What to do:** Stage all changes and commit with a descriptive message.
```
ggit add .
git commit -m "feat: foundation setup complete

- Angular 21 project with Standalone
- Environment files for dev, staging, prod
- Bootstrap RTL, Chart.js, SignalR installed
- Folder structure scaffolded
- Lazy routing configured
- Proxy configured for development
- Linting and tests passing"


```
### Files To Create

None; validation only.

### CLI Commands

```
npm run lint
ng test --watch=false
ng build
ng build --configuration=production
git add .
git commit -m "feat: foundation setup complete"

```

### Concepts Required

*   Git workflow
*   Continuous integration basics

### Testing Steps

1.  Run all checks.
2.  Verify no errors.


### Expected Deliverables

*   Stable foundation codebase.

### Common Mistakes

**Mistake 1:** Forgetting to commit the `package-lock.json`.  
**Fix:** Always commit both `package.json` and `package-lock.json`.

**Mistake 2:** Not running tests before committing.  
**Fix:** Run tests locally before push.

### Edge Cases

*   If tests fail, fix issues and recommit.

### Definition of Done

*   All checks pass.
*   Code committed to the main branch.

### Frontend Architecture Notes

*   This commit serves as the baseline for all future feature branches.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** The foundation is stable. Pull the latest code and start working on features.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 7 tasks are complete (FE-SETUP-001 to FE-SETUP-007).
*   Angular project builds and runs without errors.
*   Environment files are configured for dev, staging, and production.
*   All required dependencies (Bootstrap RTL, Chart.js, SignalR, etc.) are installed and configured.
*   Folder structure matches the architecture design.
*   Lazy routing is set up with placeholders for all features.
*   Proxy is configured and working.
*   Lint and tests pass.
*   All changes are committed to the repository.

📋 Code Review Checklist
------------------------

*   `angular.json` is correctly configured with environment file replacements.
*   All dependencies are listed with appropriate versions.
*   Folder structure is logical and matches the plan.
*   Routing configuration uses lazy loading correctly.
*   Proxy configuration does not expose sensitive information.

🚀 Pull Request Checklist
-------------------------

*   All changes are committed with a clear commit message.
*   Branch is up‑to‑date with main.
*   CI pipeline (if any) passes.
*   Code review has been performed.

🧪 Deployment Readiness Checklist
---------------------------------

*   Not applicable for this phase (foundation only).
*   However, the foundation should be deployable to a dev environment.

* * *

Jalsa – Phase 1: Foundation Setup – Expanded Implementation Handbook • v1.0 • For development team
```
