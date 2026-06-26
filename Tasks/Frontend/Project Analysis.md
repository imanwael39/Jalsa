
📐 Jalsa – Phase 0: Planning & Analysis Expanded Implementation Handbook · 7 Tasks · 42+ Subtasks
=================================================================================================

📐 Phase 0 – Planning & Analysis
--------------------------------

**Purpose:** Establish the technical foundation for the entire frontend development cycle. This phase transforms business requirements into a concrete technical plan. Every decision made here influences the next 19 phases. This handbook is written for junior developers — it assumes no prior knowledge and explains every step in implementation-level detail.

📋 Tasks: 7 ⏱️ Total Effort: ~32 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: None (phase 0 is the starting point) 🎯 Deliverable: Complete technical planning document set

## FE-PLAN-001Project Understanding & SRS Review P0 Medium 6h ▾

### Task Information

*   **Task ID:** FE-PLAN-001
*   **Task Name:** Project Understanding & SRS Review
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** None
*   **Complexity:** Medium
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Thoroughly understand the Jalsa project scope, business goals, user roles, feature requirements, and technical constraints from the Software Requirements Specification (SRS). Build a shared mental model across the frontend team.

### Business Purpose

Without a shared understanding of what we're building, the team will build the wrong thing. This task ensures every frontend developer knows:

*   Who the users are (therapists, patients, administrators)
*   What problems the platform solves
*   Which features are critical vs. nice-to-have
*   How the system should behave

### Technical Purpose

Translate business requirements into technical requirements. This involves:

*   Identifying all user journeys and flows
*   Mapping features to technical modules
*   Identifying integration points with backend
*   Understanding performance and security constraints

### Prerequisites

#### Concepts Required

*   **Software Requirements Specification (SRS):** A formal document that describes the software's intended features, behaviour, and constraints. It is the source of truth for what to build.
*   **User Personas:** Fictional characters representing different user types. For Jalsa: Therapist, Patient, Administrator.
*   **User Stories:** Short, simple descriptions of a feature from the perspective of the user. Format: "As a \[user role\], I want \[action\] so that \[benefit\]."
*   **Use Cases:** Detailed descriptions of how users interact with the system to achieve a goal.

#### Access Required

*   SRS document (latest version)
*   Product owner or business analyst for questions
*   Backend team lead for integration context
*   UX/UI mockups or wireframes

### Dependencies

*   **None:** This is the first task in the project. All other tasks depend on the outputs of this task.

### Inputs

*   SRS document (PDF/Word/Confluence)
*   UX/UI mockups (Figma/Sketch/Adobe XD)
*   Stakeholder meeting notes
*   Any existing documentation

### Outputs

*   Team understanding document (a summary for the frontend team)
*   Feature map (list of all features with priorities)
*   User role matrix (what each role can do)
*   User journey maps (visual diagrams of how users navigate the system)
*   Risk assessment document (technical risks identified during review)

### Detailed Workflow

#### Step 1: Read the SRS Document

**What to do:** Read the entire SRS document from beginning to end. Take detailed notes.

**Why it is required:** The SRS is the source of truth. You cannot build what you don't understand.

**Expected result:** Comprehensive notes covering all sections of the SRS.

**Common Mistake:** Skimming the SRS and missing critical details. The SRS often contains edge cases, error scenarios, and non-functional requirements that are easy to overlook.

#### Step 2: Identify and Document User Roles

**What to do:** Extract all user roles from the SRS. For each role, document:

*   Role name and description
*   What they can do (permissions)
*   What they cannot do (restrictions)
*   Their primary goals
*   Their pain points

**Why it is required:** The frontend architecture (guards, navigation, permissions) depends entirely on user roles.

**Expected result:** A user role matrix.

**Jalsa User Roles:**

*   **Therapist:** Primary user. Manages patients, creates sessions, assigns exercises, generates reports, monitors chat.
*   **Patient:** End‑user receiving therapy. Views exercises, participates in chat, tracks progress.
*   **Administrator:** System admin. Manages users, monitors system health, configures settings.

#### Step 3: Map Features to Modules

**What to do:** Create a list of all features and group them into logical modules.

**Why it is required:** This directly informs the folder structure and lazy‑loading strategy.

**Expected result:** Feature‑to‑module mapping.

**Jalsa Feature Modules (from SRS):**

*   **Auth:** Login, Register, Reset Password, Profile
*   **Patients:** List, Detail, Create/Edit, Intake Form, Assessments
*   **Sessions:** List, Create/Edit, Voice Memos, AI Summaries
*   **Exercises:** List, Assign, Patient View, Status Updates
*   **Reports:** Generate, Review, Edit, Approve, Export PDF
*   **Dashboard:** Stats, Charts, Progress Indicators
*   **Chatbot:** Patient Chat, Therapist Monitor, Crisis Alerts

#### Step 4: Analyse User Journeys

**What to do:** For each user role, map out the key journeys through the system.

**Why it is required:** User journeys reveal navigation flows, state requirements, and potential usability issues.

**Expected result:** User journey maps (flowcharts).

Therapist Journey: Login → Dashboard → Select Patient → View Patient Detail → Create Session → Record Voice Memo → View AI Summary

#### Step 5: Identify Technical Risks

**What to do:** List all technical risks that could impact the project.

**Why it is required:** Proactive risk management prevents surprises later.

**Expected result:** Risk register.

**Common Jalsa Risks:**

*   RTL support may cause layout issues with third-party libraries
*   SignalR connection stability in low-bandwidth environments
*   OCR accuracy for intake form images
*   AI summary generation latency
*   Large patient datasets causing performance issues

#### Step 6: Team Alignment Session

**What to do:** Present findings to the frontend team and get alignment.

**Why it is required:** Every team member needs to be on the same page before implementation begins.

**Expected result:** A team that understands the project and can start planning their work.

### Folder Structure

This task produces planning documents, not code. However, the planning will influence the folder structure. The target structure (to be implemented in Phase 1) is:

```
src/
├── app/
│   ├── core/
│   │   ├── api/
│   │   ├── guards/
│   │   ├── interceptors/
│   │   ├── models/
│   │   ├── services/
│   │   └── state/
│   ├── shared/
│   │   ├── components/
│   │   ├── directives/
│   │   ├── pipes/
│   │   └── layouts/
│   ├── features/
│   │   ├── auth/
│   │   ├── patients/
│   │   ├── sessions/
│   │   ├── exercises/
│   │   ├── reports/
│   │   ├── dashboard/
│   │   └── chatbot/
│   ├── app.routes.ts
│   └── app.config.ts
├── assets/
├── environments/
└── styles/
```
### Files To Create

This task produces documentation, not code. Create the following documents:

*   `/docs/frontend/team-understanding.md` – Team understanding summary
*   `/docs/frontend/feature-map.md` – Complete feature list with priorities
*   `/docs/frontend/user-role-matrix.md` – User roles and permissions
*   `/docs/frontend/user-journeys.md` – User journey flowcharts
*   `/docs/frontend/risk-register.md` – Technical risk assessment

### CLI Commands

No CLI commands are needed for this task (it is a planning task). However, you may want to set up the docs folder:

mkdir -p docs/frontend

### Concepts Required

*   **Software Requirements Specification (SRS):** Formal document describing software features and behaviour.
*   **User Personas:** Fictional users representing different user types.
*   **User Stories:** Short descriptions of features from the user's perspective.
*   **Use Cases:** Detailed descriptions of user‑system interactions.
*   **User Journeys:** Visual maps of how users navigate through the system.

### Tools/Libraries Required

*   **Markdown Editor:** For writing documentation (VS Code, Obsidian, Notion)
*   **Diagramming Tool:** For user journeys (Draw.io, Lucidchart, Miro, Excalidraw)
*   **Collaboration Tool:** For team alignment (Slack, Teams, Confluence)

### Angular Concepts Required

*   Understanding of Angular modules (even though we're using Standalone, understanding the conceptual grouping helps)
*   Lazy loading concept
*   Routing fundamentals

### RxJS Concepts Required

*   Basic understanding of Observables and Observers
*   Understanding of HTTP requests as observables

### Signals Concepts Required

*   Basic understanding of what Angular Signals are
*   Why Signals are used for state management (simpler than RxJS for many cases)

### UI/UX Requirements

*   Understand that all text will be in Arabic (RTL)
*   All layouts must mirror correctly for RTL
*   Responsive design required for mobile, tablet, desktop
*   Accessibility requirements (WCAG 2.1 AA)

### Error Handling Strategy

*   During this planning phase, identify which errors need special handling:
*   Authentication errors (401, 403) → redirect to login
*   Validation errors (400) → display field‑level messages
*   Server errors (500) → display user‑friendly error toasts
*   Network errors → retry mechanisms or offline UI

### Security Considerations

*   JWT tokens will be stored in localStorage (identify potential XSS risks)
*   All API calls need authentication headers
*   Role‑based access control (RBAC) must be enforced on the frontend
*   Sensitive data (patient records) must not be exposed in client‑side storage

### Performance Considerations

*   Large patient lists → need pagination and search
*   Chart rendering → need efficient data structures
*   Voice memo uploads → need progress indicators and chunking
*   AI summary generation → need loading states

### Testing Steps

This is a planning task, but we can test the understanding:

1.  **Walkthrough:** Present the understanding document to the product owner. Did they agree with the interpretation?
2.  **Team Quiz:** Ask the frontend team 5 questions about the SRS. Can they answer correctly?
3.  **User Journey Validation:** Walk through each user journey with a stakeholder. Are all steps correct?

### Expected Deliverables

*   `team-understanding.md` – 5–10 pages covering all SRS sections
*   `feature-map.md` – Complete list of features with priorities (P0, P1, P2)
*   `user-role-matrix.md` – Table of roles and permissions
*   `user-journeys.md` – Flowcharts for each user role
*   `risk-register.md` – List of technical risks with mitigation strategies

### Acceptance Criteria

*   SRS has been fully read and understood by the entire frontend team
*   All user roles are identified and documented
*   All features are mapped to modules
*   User journeys are documented for each role
*   Technical risks are identified and documented
*   Team alignment session has been completed

### Common Mistakes

**Mistake 1:** Skimming the SRS instead of reading it thoroughly.  
**Fix:** Set aside dedicated time, take notes, and highlight key sections.

**Mistake 2:** Assuming features instead of verifying with the SRS or product owner.  
**Fix:** Always verify assumptions with the source document or stakeholder.

**Mistake 3:** Not documenting technical risks early.  
**Fix:** Create a risk register and review it weekly.

**Mistake 4:** Not including the entire team in the alignment session.  
**Fix:** Ensure every team member attends or has access to the recording.

### Edge Cases

*   **What if the SRS is incomplete?** Schedule additional meetings with stakeholders to fill gaps.
*   **What if roles overlap?** A user could have multiple roles (e.g., a therapist who is also an admin). Document this.
*   **What if features conflict?** Identify dependencies between features and prioritise accordingly.

### Definition of Done

*   All documents are created and stored in `/docs/frontend/`
*   Team alignment session completed with all team members
*   Product owner has reviewed and approved the understanding document

### Frontend Architecture Notes

*   **Standalone Components:** The entire application will use Angular 17 Standalone components. No NgModules will be used except for the root configuration.
*   **Lazy Loading:** Every feature module will be lazy‑loaded to reduce initial bundle size.
*   **State Management:** Signals + RxJS services. No NgRx unless the project becomes significantly more complex.
*   **Styling:** SCSS with Bootstrap RTL. Custom variables will be used for theming.

### Team Handoff Notes

*   **To:** M3 (Frontend Lead) will hand off to the team during the alignment session.
*   **Key Takeaways:** The team should leave with a clear understanding of what they're building and why.
*   **Next Steps:** After this task, the team moves to FE-PLAN-002 (API Contract Planning).

## FE-PLAN-002API Contract Planning & Mocking P0 High 8h ▾

### Task Information

*   **Task ID:** FE-PLAN-002
*   **Task Name:** API Contract Planning & Mocking
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PLAN-001
*   **Complexity:** High
*   **Estimated Effort:** 8 hours
*   **Priority:** Critical

### Objective

Define the complete API contract between the frontend and backend, and set up mock API responses so frontend development can proceed independently of backend implementation.

### Business Purpose

Frontend and backend teams can work in parallel, reducing the overall project timeline by 30–40%. Without a contract, the frontend team would have to wait for the backend to be ready, causing delays.

### Technical Purpose

Create a central `api-endpoints.ts` file containing all endpoint constants with types. Set up mock interceptors or a mock server so the frontend can call endpoints and receive realistic responses without a real backend.

### Prerequisites

#### Backend

*   **OpenAPI/Swagger Specification:** If the backend team has already defined their API, this is ideal. If not, you'll need to work with them to define it.
*   **DTO Definitions:** The shape of request and response objects must be defined.

#### Frontend

*   Understanding of HTTP methods (GET, POST, PUT, DELETE, PATCH)
*   Understanding of RESTful API design patterns
*   Knowledge of `HttpClient` and interceptors
*   Understanding of TypeScript interfaces

### Dependencies

*   **FE-PLAN-001:** Need to know which features exist and what data they need.
*   **Backend Team:** Need to collaborate on endpoint definitions.

### Inputs

*   Feature list from FE-PLAN-001
*   OpenAPI/Swagger definition (if available)
*   Backend team's API design document (if available)
*   Existing API documentation from similar projects

### Outputs

*   `api-endpoints.ts` – Central file with all endpoint constants
*   `models/` – TypeScript interfaces for all DTOs
*   Mock data JSON files or in‑memory service
*   API contract documentation
*   Postman/Insomnia collection for testing

### Detailed Workflow

#### Step 1: Review Feature Data Requirements

**What to do:** For each feature identified in FE-PLAN-001, list the data it needs to display, create, update, or delete.

**Why it is required:** This tells us what endpoints we need.

**Expected result:** A list of data requirements per feature.

**Example: Patient Management Data Requirements**

*   Display patient list → need GET /patients
*   View patient details → need GET /patients/:id
*   Create patient → need POST /patients
*   Update patient → need PUT /patients/:id
*   Archive patient → need DELETE /patients/:id
*   Restore patient → need POST /patients/:id/restore
*   View intake form → need GET /patients/:id/intake
*   Save intake form → need POST /patients/:id/intake
*   Upload intake image → need POST /patients/:id/intake/image
*   View assessments → need GET /patients/:id/assessments
*   Add assessment → need POST /patients/:id/assessments

#### Step 2: Define All Endpoints

**What to do:** For each data requirement, define the endpoint path, HTTP method, request DTO, and response DTO.

**Why it is required:** This is the actual API contract. Both teams need to agree on these definitions.

**Expected result:** A complete endpoint specification table.

| Feature                | Endpoint                          | Method | Request DTO                                    | Response DTO                               |
|------------------------|-----------------------------------|--------|------------------------------------------------|--------------------------------------------|
| Auth Login             | /api/auth/login                   | POST   | { email, password }                            | { token }                                  |
| Auth Register          | /api/auth/register                | POST   | { firstName, lastName, email, password, role } | { userId }                                 |
| Auth Refresh           | /api/auth/refresh                 | POST   | -                                              | { token }                                  |
| Auth Profile           | /api/auth/profile                 | GET    | -                                              | { id, email, firstName, lastName, roles }  |
| Patients List          | /api/patients                     | GET    | Query: search, page, size, includeArchived     | { items: Patient[], totalCount }           |
| Patient Detail         | /api/patients/:id                 | GET    | -                                              | Patient                                    |
| Create Patient         | /api/patients                     | POST   | CreatePatientRequest                           | Patient                                    |
| Update Patient         | /api/patients/:id                 | PUT    | UpdatePatientRequest                           | Patient                                    |
| Archive Patient        | /api/patients/:id                 | DELETE | -                                              | void                                       |
| Restore Patient        | /api/patients/:id/restore         | POST   | -                                              | void                                       |
| Intake Form            | /api/patients/:id/intake          | GET    | -                                              | IntakeForm                                 |
| Save Intake            | /api/patients/:id/intake          | POST   | IntakeFormRequest                              | IntakeForm                                 |
| Upload Intake Image    | /api/patients/:id/intake/image    | POST   | Multipart file                                 | { imageUrl, extractedData }                |
| Assessments List       | /api/patients/:id/assessments     | GET    | -                                              | Assessment[]                               |
| Add Assessment         | /api/patients/:id/assessments     | POST   | AssessmentRequest                              | Assessment                                 |
| Sessions List          | /api/sessions/patient/:patientId  | GET    | -                                              | Session[]                                  |
| Create Session         | /api/sessions                     | POST   | CreateSessionRequest                           | Session                                    |
| Update Session         | /api/sessions/:id                 | PUT    | UpdateSessionRequest                           | Session                                    |
| Delete Session         | /api/sessions/:id                 | DELETE | -                                              | void                                       |
| Upload Voice Memo      | /api/sessions/:id/voice           | POST   | Multipart file                                 | { voiceUrl }                               |
| Session Summary        | /api/sessions/:id/summary         | GET    | -                                              | { summary }                                |
| Exercises List         | /api/exercises                    | GET    | -                                              | Exercise[]                                 |
| Assign Exercise        | /api/exercises/assign             | POST   | AssignExerciseRequest                          | ExerciseAssignment                         |
| Update Exercise Status | /api/exercises/:id/status         | PUT    | { status }                                     | void                                       |
| Patient Exercises      | /api/exercises/patient/:patientId | GET    | -                                              | ExerciseAssignment[]                       |
| Generate Report        | /api/reports/generate             | POST   | { patientId }                                  | Report                                     |
| Get Report             | /api/reports/:id                  | GET    | -                                              | Report                                     |
| Update Report          | /api/reports/:id                  | PUT    | UpdateReportRequest                            | Report                                     |
| Approve Report         | /api/reports/:id/approve          | POST   | -                                              | void                                       |
| Reject Report          | /api/reports/:id/reject           | POST   | -                                              | void                                       |
| Export Report PDF      | /api/reports/:id/export           | GET    | -                                              | Blob (PDF)                                 |
| Dashboard Stats        | /api/dashboard/stats              | GET    | -                                              | DashboardStats                             |
| Dashboard Trends       | /api/dashboard/trends             | GET    | -                                              | TrendData[]                                |
| Chat History           | /api/chat/:sessionId/history      | GET    | -                                              | ChatMessage[]                              |
| Send Chat Message      | /api/chat/send                    | POST   | { message }                                    | ChatMessage                                |


#### Step 3: Define TypeScript Interfaces (DTOs)

**What to do:** Convert the DTO definitions from the API contract into TypeScript interfaces.

**Why it is required:** TypeScript interfaces provide type safety and auto‑completion in the IDE.

**Expected result:** A set of TypeScript interfaces in `core/models/`.

```
// core/models/patient.model.ts
export interface Patient {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string; // ISO date
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

// core/models/auth.model.ts
export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
}

export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
}

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
```
#### Step 4: Create API Endpoints File

**What to do:** Create a central file with all endpoint constants.

**Why it is required:** Centralising endpoints makes it easy to update them if the API changes.

**Expected result:** `core/api/api-endpoints.ts`.

```
// core/api/api-endpoints.ts
export const API = {
    auth: {
        login: '/api/auth/login',
        register: '/api/auth/register',
        refresh: '/api/auth/refresh',
        profile: '/api/auth/profile',
    },
    patients: {
        base: '/api/patients',
        byId: (id: string) => `/api/patients/${id}`,
        archive: (id: string) => `/api/patients/${id}/archive`,
        restore: (id: string) => `/api/patients/${id}/restore`,
        intake: (id: string) => `/api/patients/${id}/intake`,
        intakeImage: (id: string) => `/api/patients/${id}/intake/image`,
        assessments: (id: string) => `/api/patients/${id}/assessments`,
    },
    sessions: {
        base: '/api/sessions',
        byPatient: (patientId: string) => `/api/sessions/patient/${patientId}`,
        byId: (id: string) => `/api/sessions/${id}`,
        voice: (id: string) => `/api/sessions/${id}/voice`,
        summary: (id: string) => `/api/sessions/${id}/summary`,
    },
    exercises: {
        base: '/api/exercises',
        assign: '/api/exercises/assign',
        byPatient: (patientId: string) => `/api/exercises/patient/${patientId}`,
        status: (id: string) => `/api/exercises/${id}/status`,
    },
    reports: {
        base: '/api/reports',
        generate: '/api/reports/generate',
        byId: (id: string) => `/api/reports/${id}`,
        approve: (id: string) => `/api/reports/${id}/approve`,
        reject: (id: string) => `/api/reports/${id}/reject`,
        export: (id: string) => `/api/reports/${id}/export`,
    },
    dashboard: {
        stats: '/api/dashboard/stats',
        trends: '/api/dashboard/trends',
    },
    chat: {
        history: (sessionId: string) => `/api/chat/${sessionId}/history`,
        send: '/api/chat/send',
    },
} as const;
```

#### Step 5: Set Up Mock Data

**What to do:** Create mock data for each endpoint so frontend development can continue without a real backend.

**Why it is required:** Parallel development. The frontend team can start building features immediately.

**Expected result:** JSON mock data files or an in‑memory data service.

**Mock Data Approaches:**

*   **Option A: JSON files in `/assets/mocks/`** – Simple, static, works with any setup.
*   **Option B: In‑memory data service** – Use `@angular/http` with an interceptor that returns mock data.
*   **Option C: json-server** – A full mock API server that can handle GET, POST, PUT, DELETE with realistic responses.

**Recommendation:** Use json-server for the most realistic experience.

#### Step 6: Create HTTP Client Service

**What to do:** Create a base HTTP client service that wraps Angular's HttpClient with error handling and authentication.

**Why it is required:** Centralised HTTP logic reduces code duplication and makes interceptors consistent.

**Expected result:** `core/api/http-client.service.ts`.

```
// core/api/http-client.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class HttpClientService {
    private baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) {}

    get<T>(endpoint: string, params?: HttpParams): Observable<T> {
        return this.http.get<T>(`${this.baseUrl}${endpoint}`, { params });
    }

    post<T>(endpoint: string, body: any): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}${endpoint}`, body);
    }

    put<T>(endpoint: string, body: any): Observable<T> {
        return this.http.put<T>(`${this.baseUrl}${endpoint}`, body);
    }

    delete<T>(endpoint: string): Observable<T> {
        return this.http.delete<T>(`${this.baseUrl}${endpoint}`);
    }

    patch<T>(endpoint: string, body: any): Observable<T> {
        return this.http.patch<T>(`${this.baseUrl}${endpoint}`, body);
    }

    upload<T>(endpoint: string, formData: FormData): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}${endpoint}`, formData);
    }
}
```

### Files To Create

*   `core/api/api-endpoints.ts` – All endpoint constants
*   `core/api/http-client.service.ts` – Base HTTP service
*   `core/models/patient.model.ts`
*   `core/models/auth.model.ts`
*   `core/models/session.model.ts`
*   `core/models/exercise.model.ts`
*   `core/models/report.model.ts`
*   `core/models/chat.model.ts`
*   `core/models/dashboard.model.ts`
*   `core/models/api-response.model.ts`
*   `assets/mocks/patients.json`
*   `assets/mocks/sessions.json`
*   `assets/mocks/exercises.json`
*   `assets/mocks/reports.json`
*   `assets/mocks/dashboard.json`

### CLI Commands

```
# Create core directories
mkdir -p src/app/core/api src/app/core/models

# Create mock data directory
mkdir -p src/assets/mocks

# Generate HTTP client service
ng g s core/api/http-client --skip-tests
```

### API Interaction Flow

Component → Feature Service → HttpClientService → API Endpoint → Interceptor → Backend ↕ Mock (dev)

### Tools/Libraries Required

*   **json-server:** For mock API server (optional but recommended)
*   **Postman/Insomnia:** For testing API endpoints
*   **VS Code:** With TypeScript extensions for auto‑completion

### Angular Concepts Required

*   HttpClientModule (provided in `app.config.ts`)
*   HttpClient service usage
*   Interceptors (planning for Phase 14)

### RxJS Concepts Required

*   Observable
*   Subscription
*   Error handling in observables

### Testing Steps

1.  Verify all endpoints are defined in `api-endpoints.ts`.
2.  Verify all DTOs are defined as TypeScript interfaces.
3.  Test mock API using json-server or interceptors.
4.  Verify the HTTP client service can make requests.

### Expected Deliverables

*   Complete `api-endpoints.ts` file
*   Complete model interfaces
*   Mock data for all endpoints
*   HTTP client service
*   API contract documentation

**Mistake 1:** Not using type safety for API responses.  
**Fix:** Always define TypeScript interfaces for responses.

**Mistake 2:** Hardcoding API URLs in components.  
**Fix:** Always use the central `API` constant.

**Mistake 3:** Not including all endpoints from the SRS.  
**Fix:** Cross‑reference the feature map from FE-PLAN-001.

### Edge Cases

*   **What if the backend changes an endpoint?** Update `api-endpoints.ts` and notify the team.
*   **What if an endpoint is missing?** Add it to the contract and coordinate with the backend team.
*   **What if we need a different response format?** Update the model interface and the mock data.

### Definition of Done

*   All endpoints are defined in `api-endpoints.ts`
*   All DTOs are defined as TypeScript interfaces
*   Mock data is available for all endpoints
*   HTTP client service is created and working
*   API contract is documented

### Frontend Architecture Notes

*   The HTTP client service wraps `HttpClient` to provide consistent error handling and environment configuration.
*   All feature services will use this HTTP client service, not `HttpClient` directly.
*   This centralisation makes it easier to add global behaviours (e.g., logging, metrics).

### Team Handoff Notes

*   **To:** M4 (Frontend Developer) and the rest of the team.
*   **Key Takeaways:** The API contract is the single source of truth for all API communication.
*   **Next Steps:** Use the mock data to start building features in parallel with the backend team.

## FE-PLAN-003Architecture Design P0 High 6h ▾

### Task Information

*   **Task ID:** FE-PLAN-003
*   **Task Name:** Architecture Design
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PLAN-001, FE-PLAN-002
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Define the complete frontend architecture for the Jalsa project, including folder structure, state management strategy, routing strategy, and coding standards.

### Business Purpose

A well‑defined architecture ensures the project remains maintainable and scalable over time. Without it, the codebase becomes difficult to navigate, test, and extend.

### Technical Purpose

Establish clear patterns and conventions that all developers will follow. This reduces decision fatigue and ensures consistency across the codebase.

### Prerequisites

#### Concepts Required

*   **Separation of Concerns:** Dividing code into distinct sections with specific responsibilities.
*   **Single Responsibility Principle:** Each component/service should have one reason to change.
*   **Dependency Injection:** Angular's built‑in DI system for providing services.
*   **Lazy Loading:** Loading feature modules only when needed.
*   **Standalone Components:** Angular 17's standalone API.

### Dependencies

*   **FE-PLAN-001:** Features and modules identified.
*   **FE-PLAN-002:** API endpoints and DTOs defined.

### Inputs

*   Feature map from FE-PLAN-001
*   API contract from FE-PLAN-002
*   Angular 17 best practices documentation

### Outputs

*   Folder structure diagram
*   State management strategy document
*   Routing strategy document
*   Coding standards document
*   Architecture decision record (ADR) document

### Detailed Workflow

#### Step 1: Define Folder Structure

**What to do:** Design the folder structure based on the feature map and Angular 17 Standalone best practices.

**Why it is required:** A consistent folder structure makes it easy to find code and onboard new developers.

**Expected result:** A documented folder structure with explanations.

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
│   ├── features/                   # Lazy‑loaded features
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
    ├── _variables.scss             # SCSS variables
    ├── _typography.scss            # Typography styles
    ├── _utilities.scss             # Utility classes
    └── styles.scss                 # Main stylesheet   
```

#### Step 2: Define State Management Strategy

**What to do:** Document the state management approach.

**Why it is required:** Clear guidelines prevent developers from using different patterns in different parts of the app.

**Expected result:** A state management strategy document.

**State Management Strategy:**

| State Type            | Approach                                       | Example                                          |
|-----------------------|------------------------------------------------|--------------------------------------------------|
| Local/Component State | Angular Signals (signal(), computed())         | Form values, UI visibility, local loading flags  |
| Shared/Feature State  | Services + Signals (signal() in root services) | Patient list, selected patient, session draft    |
| Application State     | AppStateService with Signals                   | Global loading indicator, notifications, theme   |
| Server/API State      | Services that call APIs and update Signals     | Patient data from API, session data              |
| URL/Route State       | Angular Router (params, query params, data)    | Patient ID, page, search filters                 |

**Rule:** Avoid NgRx unless the project complexity justifies it. Signals + services are sufficient for this project.

#### Step 3: Define Routing Strategy

**What to do:** Document the routing approach.

**Why it is required:** Consistent routing patterns make navigation predictable.

**Expected result:** A routing strategy document.

**Routing Strategy:**

*   **Lazy Loading:** Every feature uses lazy loading (loadChildren).
*   **Route Guards:** Use functional guards (`CanActivateFn`) for auth and role checks.
*   **Route Parameters:** Use `ActivatedRoute` to access params.
*   **Query Parameters:** Use for search filters, pagination.
*   **Route Data:** Use for static data like page titles.
*   **Route Resolvers:** Use for pre‑fetching data before component loads.

**Rule:** Always use lazy loading. Never import a feature module directly in `app.routes.ts`.

#### Step 4: Define Coding Standards

**What to do:** Document the coding standards.

**Why it is required:** Consistent code is easier to read, review, and maintain.

**Expected result:** A coding standards document.

**Coding Standards:**

*   **TypeScript:** Strict mode enabled (`strict: true` in `tsconfig.json`).
*   **Naming Conventions:** camelCase for variables/functions, PascalCase for classes/interfaces, kebab-case for file names.
*   **Component Selectors:** Use `app-` prefix (e.g., `app-button`).
*   **Standalone Components:** All components should be standalone.
*   **Change Detection:** Use `ChangeDetectionStrategy.OnPush` on all components.
*   **RxJS:** Use `takeUntil` or `async` pipe for subscription management. Never subscribe manually without cleanup.
*   **Signals:** Use `signal()` for local state, `computed()` for derived state.
*   **Styling:** Use SCSS with Bootstrap. Follow BEM naming conventions for custom CSS.
*   **Linting:** ESLint + Prettier with Airbnb style guide.

#### Step 5: Create Architecture Decision Records (ADRs)

**What to do:** Document key architectural decisions as ADRs.

**Why it is required:** ADRs capture the reasoning behind decisions for future reference.

**Expected result:** ADR documents for each major decision.

**Architecture Decisions to Document:**

1.  **ADR-001:** Use Angular 17 Standalone Components
2.  **ADR-002:** Use Signals + Services for State Management
3.  **ADR-003:** Lazy Load All Features
4.  **ADR-004:** Use Bootstrap RTL for Styling
5.  **ADR-005:** Use SignalR for Real‑time Communication
6.  **ADR-006:** Use Chart.js for Dashboard Charts
7.  **ADR-007:** Use JWT for Authentication

**ADR Format:**

*   **Context:** The situation that requires a decision.
*   **Decision:** What was decided.
*   **Alternatives:** What other options were considered.
*   **Consequences:** The impact of the decision.

### Files To Create

*   `/docs/architecture/folder-structure.md` – Folder structure diagram and explanation
*   `/docs/architecture/state-management.md` – State management strategy
*   `/docs/architecture/routing-strategy.md` – Routing strategy
*   `/docs/architecture/coding-standards.md` – Coding standards
*   `/docs/architecture/adr-001-standalone.md` – ADR for Standalone
*   `/docs/architecture/adr-002-state.md` – ADR for state management
*   `/docs/architecture/adr-003-lazy-loading.md` – ADR for lazy loading
*   `/docs/architecture/adr-004-styling.md` – ADR for styling
*   `/docs/architecture/adr-005-signalr.md` – ADR for SignalR
*   `/docs/architecture/adr-006-charts.md` – ADR for charts
*   `/docs/architecture/adr-007-jwt.md` – ADR for JWT

### Concepts Required

*   Separation of Concerns
*   Single Responsibility Principle
*   Dependency Injection
*   Lazy Loading
*   Standalone Components
*   Angular Signals
*   RxJS
*   RTL (Right-to-Left) layout

### Tools/Libraries Required

*   Markdown editor for documentation
*   Diagramming tool for architecture diagrams

### Angular Concepts Required

*   Standalone components
*   Lazy loading with `loadChildren`
*   Functional guards
*   Signals
*   RxJS

### Testing Steps

1.  Review the architecture documents with the team.
2.  Get sign‑off from the principal engineer or architect.
3.  Create a prototype (Phase 1) to validate the architecture.

### Expected Deliverables

*   Folder structure documentation
*   State management strategy
*   Routing strategy
*   Coding standards
*   Architecture Decision Records

**Mistake 1:** Over‑engineering the architecture with patterns that aren't needed.  
**Fix:** Keep it simple. Use the simplest solution that works.

**Mistake 2:** Not documenting decisions.  
**Fix:** Write ADRs for every major decision.

**Mistake 3:** Ignoring RTL support in the architecture.  
**Fix:** Plan for RTL from day one (Phase 3).

### Edge Cases

*   **What if the project grows larger than expected?** The architecture is designed to be scalable. Adding NgRx later is possible if needed.
*   **What if a new feature is added?** Follow the folder structure and add a new feature module.

### Definition of Done

*   All architecture documents are created
*   ADRs are documented
*   Team has reviewed and approved the architecture

### Frontend Architecture Notes

*   The architecture is designed to be modular and scalable.
*   All features are lazy‑loaded to reduce initial bundle size.
*   State management uses Signals for simplicity and performance.

### Team Handoff Notes

*   **To:** All frontend developers.
*   **Key Takeaways:** The architecture documents are the guide for all development work.
*   **Next Steps:** Start Phase 1 (Foundation Setup) with the architecture as the blueprint.

## FE-PLAN-004Design System Planning P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-PLAN-004
*   **Task Name:** Design System Planning
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PLAN-001
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Define the design system for the Jalsa platform: colours, typography, spacing, and component styles, with full RTL support from day one.

### Business Purpose

A consistent design system ensures brand consistency, improves user experience, and reduces development time by providing reusable design tokens.

### Technical Purpose

Set up SCSS variables, Bootstrap RTL configuration, and component styling standards that all developers will use.

### Prerequisites

#### Concepts Required

*   **Design Tokens:** Variables that store design decisions (colours, spacing, typography).
*   **Bootstrap RTL:** Bootstrap with right-to-left layout support.
*   **Typography:** Understanding of font sizes, weights, line heights.
*   **Accessibility:** WCAG 2.1 AA compliance (colour contrast, font sizes).

### Dependencies

*   **FE-PLAN-001:** Need to understand the brand and user expectations.
*   **UX/UI mockups:** Need to extract colours, fonts, and component styles from the design.

### Inputs

*   UX/UI mockups (Figma/Sketch/Adobe XD)
*   Brand guidelines (colours, fonts, logos)
*   Bootstrap RTL documentation

### Outputs

*   SCSS variables file (`_variables.scss`)
*   Typography styles (`_typography.scss`)
*   Utility classes (`_utilities.scss`)
*   Design system documentation

### Detailed Workflow

#### Step 1: Extract Design Tokens from Mockups

**What to do:** Go through the UI mockups and extract all colours, font styles, and spacing values.

**Why it is required:** Design tokens are the foundation of the design system.

**Expected result:** A list of all design tokens with their values.

**Jalsa Colour Palette (example):**

*   **Primary:** #0F172A (Dark navy)
*   **Primary Light:** #1E293B
*   **Primary Dark:** #020617
*   **Secondary:** #3B82F6 (Blue)
*   **Secondary Light:** #60A5FA
*   **Secondary Dark:** #2563EB
*   **Accent:** #22C55E (Green)
*   **Warning:** #F59E0B (Amber)
*   **Danger:** #EF4444 (Red)
*   **Background:** #F8FAFC
*   **Surface:** #FFFFFF
*   **Text Primary:** #0F172A
*   **Text Secondary:** #475569
*   **Text Muted:** #94A3B8
*   **Border:** #E2E8F0

**Jalsa Typography (example):**

*   **Font Family:** Cairo (Arabic), Inter (Latin) – fallback to system fonts
*   **Headings:** 900 weight, tight line height
*   **Body:** 400 weight, 1.6 line height
*   **Small:** 14px, 1.5 line height
*   **Caption:** 12px, 1.4 line height

#### Step 2: Create SCSS Variables

**What to do:** Create a SCSS variables file with all design tokens.

**Why it is required:** SCSS variables are used throughout the stylesheets.

**Expected result:** `styles/_variables.scss`.

```
// styles/_variables.scss

// ===== Colors =====
$primary: #0F172A;
$primary-light: #1E293B;
$primary-dark: #020617;

$secondary: #3B82F6;
$secondary-light: #60A5FA;
$secondary-dark: #2563EB;

$success: #22C55E;
$warning: #F59E0B;
$danger: #EF4444;
$info: #3B82F6;

$background: #F8FAFC;
$surface: #FFFFFF;

$text-primary: #0F172A;
$text-secondary: #475569;
$text-muted: #94A3B8;

$border-color: #E2E8F0;

// ===== Typography =====
$font-family-base: 'Cairo', 'Inter', -apple-system, sans-serif;
$font-size-base: 1rem;
$line-height-base: 1.6;

// Headings
$h1-size: 2.5rem;
$h2-size: 2rem;
$h3-size: 1.75rem;
$h4-size: 1.5rem;
$h5-size: 1.25rem;
$h6-size: 1rem;

// ===== Spacing =====
$spacer: 1rem;
$spacers: (
    0: 0,
    1: $spacer * 0.25,
    2: $spacer * 0.5,
    3: $spacer,
    4: $spacer * 1.5,
    5: $spacer * 3,
    6: $spacer * 4,
    7: $spacer * 5,
    8: $spacer * 6,
);

// ===== Breakpoints =====
$breakpoints: (
    xs: 0,
    sm: 576px,
    md: 768px,
    lg: 992px,
    xl: 1200px,
    xxl: 1400px,
);

// ===== Border Radius =====
$border-radius: 0.375rem;
$border-radius-lg: 0.5rem;
$border-radius-sm: 0.25rem;
$border-radius-pill: 50rem;

// ===== Shadows =====
$shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05);
$shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06);
$shadow-md: 0 4px 6px rgba(0, 0, 0, 0.07), 0 2px 4px rgba(0, 0, 0, 0.06);
$shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1), 0 4px 6px rgba(0, 0, 0, 0.05);

// ===== Z‑Index =====
$z-index-dropdown: 1000;
$z-index-sticky: 1020;
$z-index-modal: 1050;
$z-index-toast: 1060;
$z-index-loader: 2000;

// ===== RTL Support =====
$enable-rtl: true;
```
#### Step 3: Configure Bootstrap RTL

**What to do:** Install and configure Bootstrap with RTL support.

**Why it is required:** The application is in Arabic and must display correctly in RTL.

**Expected result:** Bootstrap RTL working in the application.

```
# Install Bootstrap and RTL
npm install bootstrap @popperjs/core
npm install bootstrap-rtl
```
```
// In styles.scss
@import 'bootstrap/scss/bootstrap';
@import 'bootstrap-rtl/scss/bootstrap-rtl';
@import './variables';
@import './typography';
@import './utilities';
```

#### Step 4: Create Typography Styles

**What to do:** Create typography styles using the design tokens.

**Why it is required:** Consistent typography across the app.

**Expected result:** `styles/_typography.scss`.

```
// styles/_typography.scss

// Base typography
body {
    font-family: $font-family-base;
    font-size: $font-size-base;
    line-height: $line-height-base;
    color: $text-primary;
}

// Headings
h1, .h1 { font-size: $h1-size; font-weight: 700; line-height: 1.2; }
h2, .h2 { font-size: $h2-size; font-weight: 700; line-height: 1.3; }
h3, .h3 { font-size: $h3-size; font-weight: 600; line-height: 1.3; }
h4, .h4 { font-size: $h4-size; font-weight: 600; line-height: 1.4; }
h5, .h5 { font-size: $h5-size; font-weight: 600; line-height: 1.4; }
h6, .h6 { font-size: $h6-size; font-weight: 600; line-height: 1.4; }

// Text utilities
.text-muted { color: $text-muted; }
.text-secondary { color: $text-secondary; }
.text-primary { color: $text-primary; }

.text-center { text-align: center; }
.text-right { text-align: right; }
.text-left { text-align: left; }

// RTL‑aware text alignment
.text-start { text-align: left; }
.text-end { text-align: right; }

[dir="rtl"] .text-start { text-align: right; }
[dir="rtl"] .text-end { text-align: left; }
```

#### Step 5: Create Utility Classes

**What to do:** Create utility classes for common use cases.

**Why it is required:** Utility classes speed up development.

**Expected result:** `styles/_utilities.scss`.

```
// styles/_utilities.scss

// Flex utilities
.d-flex { display: flex; }
.flex-column { flex-direction: column; }
.flex-row { flex-direction: row; }
.justify-content-center { justify-content: center; }
.justify-content-between { justify-content: space-between; }
.justify-content-end { justify-content: flex-end; }
.align-items-center { align-items: center; }
.align-items-start { align-items: flex-start; }
.align-items-end { align-items: flex-end; }
.flex-grow-1 { flex-grow: 1; }

// Spacing utilities (RTL‑aware)
.m-0 { margin: 0; }
.p-0 { padding: 0; }
.m-1 { margin: $spacer * 0.25; }
.p-1 { padding: $spacer * 0.25; }
// ... and so on for all spacers

// RTL‑aware margin utilities
.ms-1 { margin-left: $spacer * 0.25; }
.me-1 { margin-right: $spacer * 0.25; }
[dir="rtl"] .ms-1 { margin-left: 0; margin-right: $spacer * 0.25; }
[dir="rtl"] .me-1 { margin-right: 0; margin-left: $spacer * 0.25; }

// Display utilities
.d-none { display: none; }
.d-block { display: block; }
.d-inline-block { display: inline-block; }
.d-inline { display: inline; }

// Position utilities
.position-relative { position: relative; }
.position-absolute { position: absolute; }
.position-sticky { position: sticky; }

// Overflow utilities
.overflow-hidden { overflow: hidden; }
.overflow-auto { overflow: auto; }
.overflow-scroll { overflow: scroll; }
```

### Files To Create

*   `styles/_variables.scss`
*   `styles/_typography.scss`
*   `styles/_utilities.scss`
*   `styles/styles.scss`
*   `/docs/design-system/tokens.md`
*   `/docs/design-system/components.md`
*   `/docs/design-system/typography.md`

### CLI Commands

```
# Create styles directory
mkdir -p src/styles

# Install dependencies
npm install bootstrap @popperjs/core bootstrap-rtl
```

### Concepts Required

*   Design tokens
*   Bootstrap RTL
*   SCSS variables and nesting
*   RTL layout principles
*   Accessibility (WCAG 2.1 AA)

### Tools/Libraries Required

*   Bootstrap 5
*   bootstrap-rtl
*   SCSS preprocessor
*   Figma for design token extraction

### Angular Concepts Required

*   Styles in `angular.json`
*   Component styles (ViewEncapsulation)

### UI/UX Requirements

*   All text must be in Arabic (RTL)
*   All layouts must mirror for RTL
*   Colour contrast must meet WCAG 2.1 AA
*   Responsive design for all screen sizes

### Testing Steps

1.  Verify Bootstrap RTL works correctly in the browser.
2.  Test all typography styles on sample content.
3.  Verify colour contrast meets accessibility standards.
4.  Test RTL layout mirroring.

### Expected Deliverables

*   SCSS variables file
*   Typography styles
*   Utility classes
*   Design system documentation

### Common Mistakes

**Mistake 1:** Not testing RTL on actual Arabic text.  
**Fix:** Test with Arabic text (not just English with RTL direction).

**Mistake 2:** Using inline styles instead of design tokens.  
**Fix:** Always use SCSS variables.

### Definition of Done

*   All SCSS files are created
*   Bootstrap RTL is configured
*   Design system is documented
*   RTL layout is tested

### Frontend Architecture Notes

*   The design system is the foundation for all UI development.
*   All components will use the design system tokens.
*   RTL support is built into the design system from day one.

### Team Handoff Notes

*   **To:** All frontend developers.
*   **Key Takeaways:** Use the design system variables for all styling.
*   **Next Steps:** Phase 4 (Shared Component Library) will implement components using this design system.

## FE-PLAN-005Tooling & Development Environment P1 Medium 4h ▾

### Task Information

*   **Task ID:** FE-PLAN-005
*   **Task Name:** Tooling & Development Environment
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PLAN-003
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Set up all development tools, linters, formatters, and Git hooks for the frontend team.

### Business Purpose

Consistent tooling ensures code quality, reduces review time, and prevents style debates.

### Technical Purpose

Configure ESLint, Prettier, Husky, and commitlint to enforce coding standards automatically.

### Prerequisites

*   Node.js 18+
*   npm or yarn
*   Git
*   VS Code (recommended IDE)

### Dependencies

*   **FE-PLAN-003:** Coding standards define what tools to use.

### Inputs

*   Coding standards from FE-PLAN-003
*   Team preferences for tools

### Outputs

*   `.eslintrc.json` – ESLint configuration
*   `.prettierrc` – Prettier configuration
*   `.husky/` – Git hooks
*   `.commitlintrc.json` – Commit message linting
*   `.vscode/` – VS Code settings
*   `package.json` – Updated with dev dependencies
*   `README.md` – Setup instructions

### Detailed Workflow

#### Step 1: Install Dev Dependencies

**What to do:** Install all necessary dev dependencies.

**Why it is required:** Tools must be installed before they can be configured.

**Expected result:** All dev dependencies in `package.json`.

```
# Install ESLint and Prettier
npm install --save-dev eslint prettier @typescript-eslint/eslint-plugin @typescript-eslint/parser

# Install Angular ESLint
npm install --save-dev @angular-eslint/eslint-plugin @angular-eslint/eslint-plugin-template @angular-eslint/template-parser

# Install Husky and lint-staged
npm install --save-dev husky lint-staged

# Install commitlint
npm install --save-dev @commitlint/cli @commitlint/config-conventional
```

#### Step 2: Configure ESLint

**What to do:** Create `.eslintrc.json` with Angular rules.

**Why it is required:** ESLint enforces code quality rules.

**Expected result:** A working ESLint configuration.

```
// .eslintrc.json
{
    "root": true,
    "ignorePatterns": [
        "projects/**/*"
    ],
    "overrides": [
        {
            "files": ["*.ts"],
            "extends": [
                "eslint:recommended",
                "plugin:@typescript-eslint/recommended",
                "plugin:@angular-eslint/recommended"
            ],
            "rules": {
                "@angular-eslint/directive-selector": [
                    "error",
                    { "type": "attribute", "prefix": "app", "style": "camelCase" }
                ],
                "@angular-eslint/component-selector": [
                    "error",
                    { "type": "element", "prefix": "app", "style": "kebab-case" }
                ],
                "@typescript-eslint/no-unused-vars": ["error", { "argsIgnorePattern": "^_" }],
                "@typescript-eslint/explicit-function-return-type": "warn",
                "@typescript-eslint/no-explicit-any": "warn"
            }
        },
        {
            "files": ["*.html"],
            "extends": [
                "plugin:@angular-eslint/template/recommended"
            ],
            "rules": {}
        }
    ]
}
```
#### Step 3: Configure Prettier

**What to do:** Create `.prettierrc` with formatting rules.

**Why it is required:** Prettier automatically formats code, eliminating formatting debates.

**Expected result:** A working Prettier configuration.

```
// .prettierrc
{
    "singleQuote": true,
    "trailingComma": "es5",
    "printWidth": 120,
    "tabWidth": 4,
    "semi": true,
    "bracketSpacing": true,
    "arrowParens": "avoid",
    "endOfLine": "lf"
}
```

#### Step 4: Configure Husky and lint-staged

**What to do:** Set up Git hooks to run linting and formatting on commits.

**Why it is required:** This enforces code quality before code is committed.

**Expected result:** Husky hooks and lint-staged configuration.

```
# Set up Husky
npx husky install

# Add pre-commit hook
npx husky add .husky/pre-commit "npx lint-staged"

# Add commit-msg hook
npx husky add .husky/commit-msg "npx commitlint --edit $1"
```
```
# Set up Husky
npx husky install

# Add pre-commit hook
npx husky add .husky/pre-commit "npx lint-staged"

# Add commit-msg hook
npx husky add .husky/commit-msg "npx commitlint --edit $1"
```
```
// .lintstagedrc.json
{
    "*.{ts,js}": [
        "eslint --fix",
        "prettier --write"
    ],
    "*.html": [
        "eslint --fix",
        "prettier --write"
    ],
    "*.{scss,css}": [
        "prettier --write"
    ],
    "*.json": [
        "prettier --write"
    ]
}
```

#### Step 5: Configure Commitlint

**What to do:** Set up commit message linting.

**Why it is required:** Consistent commit messages make the git history easier to understand.

**Expected result:** Commitlint configuration.

```
// .commitlintrc.json
{
    "extends": ["@commitlint/config-conventional"],
    "rules": {
        "type-enum": [
            2,
            "always",
            [
                "feat",
                "fix",
                "docs",
                "style",
                "refactor",
                "perf",
                "test",
                "chore",
                "ci",
                "build"
            ]
        ],
        "subject-case": [0]
    }
}
```
#### Step 6: Configure VS Code

**What to do:** Create VS Code settings for the project.

**Why it is required:** Consistent IDE settings make collaboration smoother.

**Expected result:** `.vscode/` folder with settings.

```
// .vscode/settings.json
{
    "editor.formatOnSave": true,
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.codeActionsOnSave": {
        "source.fixAll.eslint": "explicit"
    },
    "typescript.tsdk": "node_modules/typescript/lib",
    "files.eol": "\n",
    "html.format.enable": false,
    "[html]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    },
    "[typescript]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    },
    "[json]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    }
}

// .vscode/extensions.json
{
    "recommendations": [
        "angular.ng-template",
        "dbaeumer.vscode-eslint",
        "esbenp.prettier-vscode",
        "bradlc.vscode-tailwindcss",
        "formulahendry.auto-rename-tag",
        "ms-vscode.vscode-typescript-next"
    ]
}
```

#### Step 7: Update package.json Scripts

**What to do:** Add useful npm scripts for the development workflow.

**Why it is required:** Standardised scripts make common tasks easy.

**Expected result:** Updated `package.json`.

```
// package.json scripts section
"scripts": {
    "ng": "ng",
    "start": "ng serve",
    "build": "ng build",
    "build:prod": "ng build --configuration production",
    "test": "ng test",
    "test:ci": "ng test --watch=false --browsers=ChromeHeadless",
    "lint": "eslint . --ext .ts,.html",
    "lint:fix": "eslint . --ext .ts,.html --fix",
    "format": "prettier --write \"**/*.{ts,html,scss,json}\"",
    "prepare": "husky install"
}
```

### Files To Create

*   `.eslintrc.json`
*   `.prettierrc`
*   `.prettierignore`
*   `.husky/pre-commit`
*   `.husky/commit-msg`
*   `.lintstagedrc.json`
*   `.commitlintrc.json`
*   `.vscode/settings.json`
*   `.vscode/extensions.json`
*   `README.md` (with setup instructions)

### CLI Commands

```
# Install all dev dependencies
npm install --save-dev eslint prettier @typescript-eslint/eslint-plugin @typescript-eslint/parser @angular-eslint/eslint-plugin @angular-eslint/eslint-plugin-template @angular-eslint/template-parser husky lint-staged @commitlint/cli @commitlint/config-conventional

# Set up Husky
npx husky install

# Add hooks
npx husky add .husky/pre-commit "npx lint-staged"
npx husky add .husky/commit-msg "npx commitlint --edit $1"
```

### Tools/Libraries Required

*   ESLint
*   Prettier
*   Husky
*   lint-staged
*   commitlint
*   VS Code (recommended)

### Testing Steps

1.  Run `npm run lint` – should report no errors.
2.  Run `npm run format` – should format all files.
3.  Make a commit with a valid conventional commit message – should pass.
4.  Make a commit with an invalid message – should be rejected.
5.  Make a commit with linting errors – should be blocked by pre-commit hook.

### Expected Deliverables

*   All configuration files created
*   Git hooks working
*   VS Code settings configured
*   README with setup instructions

### Common Mistakes

**Mistake 1:** Not testing the Git hooks after setup.  
**Fix:** Make a test commit to verify hooks work.

**Mistake 2:** Inconsistent ESLint and Prettier configurations (they conflict).  
**Fix:** Use `eslint-config-prettier` to disable conflicting rules.

### Definition of Done

*   All tools are installed and configured
*   Git hooks are working
*   Team members can set up their environment using the README

### Frontend Architecture Notes

*   Tooling is the foundation for code quality.
*   All developers must use the same tools and configurations.

### Team Handoff Notes

*   **To:** All frontend developers.
*   **Key Takeaways:** Run `npm install` and follow the README to set up the environment.
*   **Next Steps:** Start Phase 1 (Foundation Setup) with the tools configured.

## FE-PLAN-006Sprint Planning & Estimation P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-PLAN-006
*   **Task Name:** Sprint Planning & Estimation
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PLAN-001, FE-PLAN-003
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** High

### Objective

Break down all frontend work into sprints, estimate effort, and assign owners.

### Business Purpose

Without a plan, the team will drift. Sprint planning provides clear milestones, accountability, and a predictable delivery schedule.

### Technical Purpose

Ensure all work is decomposed into manageable tasks, dependencies are identified, and the team has a clear path forward.

### Prerequisites

*   Understanding of Agile Scrum methodology
*   Knowledge of story point estimation (Fibonacci, T‑shirt sizes, etc.)

### Dependencies

*   **FE-PLAN-001:** Feature list and priorities.
*   **FE-PLAN-003:** Architecture and structure.

### Inputs

*   Feature list from FE-PLAN-001
*   Architecture from FE-PLAN-003
*   Team capacity (hours per sprint)

### Outputs

*   Sprint plan (which tasks in which sprint)
*   Task estimates (story points or hours)
*   Owner assignments

### Detailed Workflow

#### Step 1: List All Frontend Tasks

**What to do:** Create a complete list of all frontend tasks.

**Why it is required:** You can't plan what you haven't listed.

**Expected result:** A complete task backlog.

**Jalsa Frontend Task Backlog (high‑level):**

*   Phase 0: Planning (7 tasks) – 32 hours
*   Phase 1: Foundation Setup (4 tasks) – 20 hours
*   Phase 2: Core Architecture (5 tasks) – 28 hours
*   Phase 3: RTL & Design System (3 tasks) – 16 hours
*   Phase 4: Shared Components (6 tasks) – 30 hours
*   Phase 5: Authentication (7 tasks) – 32 hours
*   Phase 6: Layout System (4 tasks) – 20 hours
*   Phase 7: Patient Management (7 tasks) – 34 hours
*   Phase 8: Session Management (6 tasks) – 30 hours
*   Phase 9: Exercise Management (5 tasks) – 24 hours
*   Phase 10: Dashboard (3 tasks) – 16 hours
*   Phase 11: AI Reports (5 tasks) – 28 hours
*   Phase 12: Chatbot (4 tasks) – 22 hours
*   Phase 13: State Management (3 tasks) – 18 hours
*   Phase 14: API Integration (4 tasks) – 22 hours
*   Phase 15: Error Handling (3 tasks) – 14 hours
*   Phase 16: Testing (4 tasks) – 24 hours
*   Phase 17: Performance (3 tasks) – 16 hours
*   Phase 18: Deployment (3 tasks) – 14 hours
*   Phase 19: Release (2 tasks) – 8 hours
*   Phase 20: Maintenance (2 tasks) – Ongoing

#### Step 2: Estimate Each Task

**What to do:** Estimate the effort for each task in hours or story points.

**Why it is required:** Estimates drive sprint planning and resource allocation.

**Expected result:** A task list with estimates.

**Estimation Guidelines:**

*   **1–2 hours:** Simple changes, documentation updates
*   **2–4 hours:** Small features, single component
*   **4–8 hours:** Medium features, multiple components
*   **8–16 hours:** Large features, multiple services and components
*   **16+ hours:** Epic‑sized features, break down further

#### Step 3: Identify Dependencies

**What to do:** For each task, identify what must be completed before it.

**Why it is required:** Dependencies determine the order of execution.

**Expected result:** A dependency graph.

Phase 0 → Phase 1 → Phase 2 → Phase 5 → Phase 6 → Phase 7 → Phase 10 → Phase 13 → Phase 14 → Phase 16 → Phase 17 → Phase 18 → Phase 19

Note: Phases 3, 4, 8, 9, 11, 12 can run in parallel where dependencies allow.

#### Step 4: Assign Owners

**What to do:** Assign each task to a specific developer.

**Why it is required:** Clear ownership ensures accountability.

**Expected result:** Task‑owner mapping.

**Ownership Assignments (example):**

*   **M3 (Frontend Lead):** Planning, Architecture, State Management, API Integration, Authentication
*   **M4 (Frontend Developer):** Patient Management, Session Management, Exercise Management, Reports
*   **M4 (Frontend Developer):** Dashboard, Chatbot, Shared Components, RTL & Design
*   **M3 + M4:** Testing, Performance, Deployment (collaboration)

#### Step 5: Create Sprint Plan

**What to do:** Allocate tasks to sprints (2‑week iterations).

**Why it is required:** Sprints provide a regular delivery cadence.

**Expected result:** A sprint plan for the entire project.

**Sprint Plan (example):**

Sprint

Phases

Key Deliverables

**Sprint 1**

Phase 0, Phase 1

Planning documents, Angular project scaffold

**Sprint 2**

Phase 2, Phase 3

Core architecture, RTL + Design System

**Sprint 3**

Phase 4, Phase 5

Shared components, Authentication

**Sprint 4**

Phase 6, Phase 7

Layout, Patient Management

**Sprint 5**

Phase 8, Phase 9

Session Management, Exercise Management

**Sprint 6**

Phase 10, Phase 11

Dashboard, AI Reports

**Sprint 7**

Phase 12

Chatbot

**Sprint 8**

Phase 13, Phase 14

State Management, API Integration

**Sprint 9**

Phase 15, Phase 16

Error Handling, Testing

**Sprint 10**

Phase 17, Phase 18

Performance, Deployment Preparation

**Sprint 11**

Phase 19, Phase 20

Production Release, Maintenance Setup

### Files To Create

*   `/docs/project/sprint-plan.md` – Sprint plan
*   `/docs/project/task-estimates.md` – Task estimates
*   `/docs/project/owner-assignments.md` – Owner assignments

### Tools/Libraries Required

*   Jira / Trello / Linear / GitHub Projects for tracking
*   Spreadsheet for estimation

### Testing Steps

1.  Review the sprint plan with the team.
2.  Get sign‑off from the product owner.
3.  Create tickets in the tracking tool.

### Expected Deliverables

*   Sprint plan document
*   Task estimates
*   Owner assignments

### Common Mistakes

**Mistake 1:** Underestimating tasks.  
**Fix:** Always add a 20% buffer to estimates.

**Mistake 2:** Not accounting for meeting time and context switching.  
**Fix:** Assume 6 hours of productive work per day, not 8.

### Definition of Done

*   Sprint plan is created and approved
*   All tasks have estimates and owners
*   Tickets are created in the tracking tool

### Frontend Architecture Notes

*   The sprint plan is a living document. It will change as the project progresses.
*   Review the plan at the end of each sprint and adjust as needed.

### Team Handoff Notes

*   **To:** All frontend developers.
*   **Key Takeaways:** Everyone knows what they're working on and when.
*   **Next Steps:** Start Sprint 1 with Phase 0 tasks.

FE-PLAN-007Team Onboarding & Handoff P1 Medium 2h ▾

### Task Information

*   **Task ID:** FE-PLAN-007
*   **Task Name:** Team Onboarding & Handoff
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All other FE-PLAN tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 2 hours
*   **Priority:** Critical

### Objective

Ensure all frontend team members are onboarded, have access to all tools, understand the project, and know how to start working.

### Business Purpose

A well‑onboarded team is productive from day one. Without onboarding, developers waste time figuring out the setup and context.

### Technical Purpose

Ensure everyone has the same environment, tools, and knowledge to contribute effectively.

### Prerequisites

*   All planning documents from FE-PLAN-001 to FE-PLAN-006
*   Access to the repository and tools

### Dependencies

*   **FE-PLAN-001 to FE-PLAN-006:** All planning work must be complete.

### Inputs

*   All documents created in FE-PLAN-001 to FE-PLAN-006
*   Repository access
*   Tool access (Jira, Slack, etc.)

### Outputs

*   Onboarding checklist
*   Team sync meeting
*   All developers ready to start

### Detailed Workflow

#### Step 1: Create Onboarding Checklist

**What to do:** Create a checklist of everything a developer needs to get started.

**Why it is required:** A checklist ensures nothing is missed.

**Expected result:** An onboarding checklist.

**Onboarding Checklist:**

*   ☐ Access to GitHub repository
*   ☐ Access to Jira/Linear (project management)
*   ☐ Access to Slack/Teams (communication)
*   ☐ Access to Figma (design files)
*   ☐ Access to Confluence/Notion (documentation)
*   ☐ Node.js 18+ installed
*   ☐ Git installed and configured
*   ☐ VS Code with recommended extensions installed
*   ☐ Repository cloned locally
*   ☐ `npm install` completed successfully
*   ☐ `ng serve` runs without errors
*   ☐ All planning documents read and understood

#### Step 2: Conduct Team Sync Meeting

**What to do:** Present the planning documents to the team.

**Why it is required:** Everyone needs to be on the same page.

**Expected result:** A team that understands the project.

**Meeting Agenda:**

*   **10 min:** Project overview and business goals
*   **15 min:** Architecture walkthrough
*   **10 min:** API contract and integration strategy
*   **10 min:** Design system and RTL
*   **10 min:** Sprint plan and task assignments
*   **15 min:** Q&A and discussion

#### Step 3: Verify Development Environment

**What to do:** Walk through the development setup with each team member.

**Why it is required:** Everyone must be able to run the application locally.

**Expected result:** All developers can run the app.

\# Verification steps git clone <repository-url> cd jalsa-frontend npm install ng serve \# Open http://localhost:4200

#### Step 4: Hand Over Planning Documents

**What to do:** Make sure all documents are stored in a shared location.

**Why it is required:** Documents must be accessible for reference.

**Expected result:** All documents in a central location.

**Document Storage:**

*   `/docs/` in the repository
*   Confluence/Notion page for reference
*   PDF exports for offline access

### Files To Create

*   `/docs/onboarding/checklist.md`
*   `/docs/onboarding/setup-guide.md`

### Tools/Libraries Required

*   Video conferencing tool (Zoom, Google Meet, Teams)
*   Shared document storage (Google Drive, Confluence, Notion, GitHub)

### Testing Steps

1.  Ask each developer to follow the setup guide.
2.  Verify the app runs on their machine.
3.  Verify they can access all tools.

### Expected Deliverables

*   All developers are onboarded and set up
*   Team sync meeting completed
*   All planning documents are accessible

### Common Mistakes

**Mistake 1:** Assuming everyone has read the documents.  
**Fix:** Walk through the documents in the sync meeting.

**Mistake 2:** Not verifying the environment setup.  
**Fix:** Have each developer run the app and report back.

### Definition of Done

*   All team members are onboarded
*   All developers can run the app locally
*   Team sync meeting is completed
*   All documents are accessible

### Frontend Architecture Notes

*   Onboarding is the final step before starting Phase 1 (Foundation Setup).
*   After this, the team moves into development mode.

### Team Handoff Notes

*   **To:** All frontend developers.
*   **Key Takeaways:** The planning phase is complete. Everyone knows what to build and how.
*   **Next Steps:** Start Phase 1 (Foundation Setup) – create the Angular project and install dependencies.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 7 planning tasks are complete (FE-PLAN-001 to FE-PLAN-007)
*   SRS is fully understood by the entire frontend team
*   API contract is defined and documented
*   Architecture is designed and approved
*   Design system is planned with RTL support
*   Development tools are configured
*   Sprint plan is created with estimates and owners
*   All team members are onboarded and can run the app
*   All planning documents are stored centrally

📋 Code Review Checklist
------------------------

*   All documents are well‑structured and easy to read
*   API contract is complete and matches the SRS
*   Architecture decisions are documented as ADRs
*   Design system tokens are complete
*   Tooling configuration is correct
*   Sprint plan is realistic and achievable

🚀 Pull Request Checklist
-------------------------

*   All planning documents are committed to the repository
*   Team sync meeting has been conducted
*   All team members have confirmed they can run the app
*   Product owner has approved the plan

🧪 Deployment Readiness Checklist
---------------------------------

*   Not applicable for Phase 0 (planning phase)
*   Phase 0 outputs are planning documents, not deployable code

* * *

Jalsa – Phase 0: Planning & Analysis – Expanded Implementation Handbook • v1.0 • For development team

function toggleTask(header) { const body = header.nextElementSibling; const isOpen = body.classList.contains('open'); body.classList.toggle('open'); header.classList.toggle('open'); } document.addEventListener('DOMContentLoaded', function() { const firstHeader = document.querySelector('.task-header'); if (firstHeader) firstHeader.click(); });