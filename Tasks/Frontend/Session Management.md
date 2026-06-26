📝 Jalsa – Phase 8: Session Management Expanded Implementation Handbook · 6 Tasks · 35+ Subtasks
================================================================================================

📝 Phase 8 – Session Management
-------------------------------

**Purpose:** Build the complete session management feature: session list, create/edit session notes with a rich text editor, voice memo recording and upload, and AI‑generated summary display. This phase enables therapists to document therapy sessions and leverage AI for summarisation.

📋 Tasks: 6 ⏱️ Total Effort: ~30 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 7 (Patient Management), Phase 4 (Shared Components) 🎯 Deliverable: Complete session management system with notes, voice, and AI summaries

## FE-SES-001Session Module Setup (Routes, Service, State) P0 High 5h ▾

### Task Information

*   **Task ID:** FE-SES-001
*   **Task Name:** Session Module Setup (Routes, Service, State)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001 (HttpClientService), FE-CORE-002 (Models), FE-PAT-001 (Patient module), FE-LAYOUT-001 (Main layout)
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Create the session feature with lazy‑loaded routes, a SessionService for API communication, and a SessionStateService for managing session state using Signals.

### Business Purpose

Session management is a core clinical workflow. A structured setup ensures therapists can efficiently document sessions.

### Technical Purpose

Set up lazy‑loaded routes under the main layout, create a service that uses HttpClientService for CRUD and voice upload, and create a state service with Signals for reactive data management.

### Prerequisites

*   Understanding of Angular lazy loading with Standalone components.
*   Knowledge of Signals and services.
*   API contract for session endpoints.

### Dependencies

*   **FE-CORE-001:** HttpClientService for API calls.
*   **FE-CORE-002:** Session model interfaces.
*   **FE-AUTH-002:** AuthService for guards.
*   **FE-LAYOUT-001:** Main layout as parent route.

### Inputs

*   API endpoints from `api-endpoints.ts`.

### Outputs

*   `features/sessions/sessions.routes.ts` – Lazy‑loaded routes.
*   `core/services/session.service.ts` – Session API service.
*   `core/state/session-state.service.ts` – Session state management with Signals.
*   Placeholder components (list, form, detail).

### Detailed Workflow

#### Step 1: Create Routes File

```typescript
// features/sessions/sessions.routes.ts
import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const SESSIONS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Admin'])],
        children: [
            {
                path: 'patient/:patientId',
                loadComponent: () => import('./pages/session-list/session-list.component').then(m => m.SessionListComponent),
            },
            {
                path: 'new/:patientId',
                loadComponent: () => import('./pages/session-form/session-form.component').then(m => m.SessionFormComponent),
            },
            {
                path: ':id',
                loadComponent: () => import('./pages/session-detail/session-detail.component').then(m => m.SessionDetailComponent),
            },
            {
                path: ':id/edit',
                loadComponent: () => import('./pages/session-form/session-form.component').then(m => m.SessionFormComponent),
            },
        ],
    },
];
```
#### Step 2: Register Routes in App Routes

Add lazy loading in `app.routes.ts` under the main layout.

```ts
// app.routes.ts (inside the main layout children)
{
    path: 'sessions',
    loadChildren: () => import('./features/sessions/sessions.routes').then(m => m.SESSIONS_ROUTES),
},
```
#### Step 3: Create Session Service

```ts
// core/services/session.service.ts
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { Session, CreateSessionRequest, UpdateSessionRequest } from '../models';

@Injectable({
    providedIn: 'root',
})
export class SessionService {
    private http = inject(HttpClientService);

    getSessions(patientId: string): Observable<Session[]> {
        return this.http.get<Session[]>(API.sessions.byPatient(patientId));
    }

    getSession(id: string): Observable<Session> {
        return this.http.get<Session>(API.sessions.byId(id));
    }

    createSession(data: CreateSessionRequest): Observable<Session> {
        return this.http.post<Session>(API.sessions.base, data);
    }

    updateSession(id: string, data: UpdateSessionRequest): Observable<Session> {
        return this.http.put<Session>(API.sessions.byId(id), data);
    }

    deleteSession(id: string): Observable<void> {
        return this.http.delete<void>(API.sessions.byId(id));
    }

    uploadVoiceMemo(sessionId: string, file: File): Observable<{ voiceUrl: string }> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.upload<{ voiceUrl: string }>(API.sessions.voice(sessionId), formData);
    }

    getSummary(sessionId: string): Observable<{ summary: string }> {
        return this.http.get<{ summary: string }>(API.sessions.summary(sessionId));
    }
}
```
#### Step 4: Create Session State Service

```ts
// core/state/session-state.service.ts
import { Injectable, signal } from '@angular/core';
import { Session } from '../models';

@Injectable({
    providedIn: 'root',
})
export class SessionStateService {
    private sessionsSignal = signal<Session[]>([]);
    private selectedSessionSignal = signal<Session | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly sessions = this.sessionsSignal.asReadonly();
    readonly selectedSession = this.selectedSessionSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setSessions(sessions: Session[]) {
        this.sessionsSignal.set(sessions);
        this.errorSignal.set(null);
    }

    addSession(session: Session) {
        this.sessionsSignal.update(list => [...list, session]);
    }

    updateSession(updated: Session) {
        this.sessionsSignal.update(list => list.map(s => s.id === updated.id ? updated : s));
        if (this.selectedSessionSignal()?.id === updated.id) {
            this.selectedSessionSignal.set(updated);
        }
    }

    removeSession(id: string) {
        this.sessionsSignal.update(list => list.filter(s => s.id !== id));
    }

    selectSession(session: Session) {
        this.selectedSessionSignal.set(session);
    }

    clearSelected() {
        this.selectedSessionSignal.set(null);
    }

    setLoading(loading: boolean) {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null) {
        this.errorSignal.set(error);
    }

    reset() {
        this.sessionsSignal.set([]);
        this.selectedSessionSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
```
#### Step 5: Generate Placeholder Components

```bash
ng g c features/sessions/pages/session-list --standalone --skip-tests
ng g c features/sessions/pages/session-form --standalone --skip-tests
ng g c features/sessions/pages/session-detail --standalone --skip-tests
ng g c features/sessions/components/voice-recorder --standalone --skip-tests
ng g c features/sessions/components/summary --standalone --skip-tests
```
### Files To Create

*   `features/sessions/sessions.routes.ts`
*   `core/services/session.service.ts`
*   `core/state/session-state.service.ts`
*   Placeholder components.

### CLI Commands

```bash
ng g c features/sessions/pages/session-list --standalone --skip-tests
ng g c features/sessions/pages/session-form --standalone --skip-tests
ng g c features/sessions/pages/session-detail --standalone --skip-tests
ng g c features/sessions/components/voice-recorder --standalone --skip-tests
ng g c features/sessions/components/summary --standalone --skip-tests
```
### Code Flow

```
User navigates to /sessions/patient/:patientId → SessionListComponent loads → Calls SessionService.getSessions() → Updates SessionStateService.sessions → Renders list
```
### Concepts Required

*   Lazy loading with loadChildren
*   Standalone components
*   Angular Signals for state

### Tools/Libraries Required

*   Angular Router

### Testing Steps

1.  Verify routes are lazy‑loaded (network tab).
2.  Test SessionService methods with mock data.
3.  Test SessionStateService signals update correctly.

### Expected Deliverables

*   Session routes, service, and state service ready.

### Common Mistakes

**Mistake 1:** Not applying guards to session routes.  
**Fix:** Add `canActivate: [authGuard, roleGuard(['Therapist'])]`.

### Edge Cases

*   Patient ID missing → redirect to patients list.

### Definition of Done

*   Routes, service, and state service are implemented.

### Frontend Architecture Notes

*   SessionStateService is the single source of truth for session data.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use SessionService for API and SessionStateService for state.

## FE-SES-002Session List Component P0 Medium 4h ▾

### Task Information

*   **Task ID:** FE-SES-002
*   **Task Name:** Session List Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SES-001, FE-SHARED-005 (Table), FE-SHARED-002 (Button)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** Critical

### Objective

Build the session list page for a patient, displaying session date, content preview, status, and actions (view, edit, delete).

### Business Purpose

Allow therapists to view and manage session history for a patient.

### Technical Purpose

Use shared Table component, integrate with SessionService and SessionStateService, and handle navigation.

### Prerequisites

*   Shared Table and Button components.
*   ActivatedRoute for patient ID.

### Dependencies

*   **FE-SES-001:** SessionService and StateService.
*   **FE-SHARED-005:** Table component.

### Inputs

*   Patient ID from route.

### Outputs

*   `session-list.component.ts`
*   `session-list.component.html`
*   `session-list.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// session-list.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'app-session-list',
    standalone: true,
    imports: [TableComponent, ButtonComponent, RouterLink, DatePipe],
    templateUrl: './session-list.component.html',
    styleUrls: ['./session-list.component.scss'],
})
export class SessionListComponent implements OnInit {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private sessionService = inject(SessionService);
    private state = inject(SessionStateService);

    patientId = '';
    sessions = this.state.sessions;
    loading = this.state.loading;
    error = this.state.error;

    ngOnInit(): void {
        this.patientId = this.route.snapshot.paramMap.get('patientId') || '';
        if (this.patientId) {
            this.loadSessions();
        }
    }

    loadSessions() {
        this.state.setLoading(true);
        this.sessionService.getSessions(this.patientId).subscribe({
            next: (data) => {
                this.state.setSessions(data);
                this.state.setLoading(false);
            },
            error: (err) => {
                this.state.setError(err.message || 'Failed to load sessions');
                this.state.setLoading(false);
            },
        });
    }

    deleteSession(id: string) {
        if (confirm('Are you sure you want to delete this session?')) {
            this.sessionService.deleteSession(id).subscribe({
                next: () => {
                    this.state.removeSession(id);
                },
                error: (err) => {
                    console.error('Delete failed', err);
                },
            });
        }
    }
}
```
#### Step 2: Create Template

```html
<!-- session-list.component.html -->
<div class="session-list">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2>Sessions</h2>
        <app-button variant="primary" [routerLink]="['/sessions/new', patientId]">New Session</app-button>
    </div>

    <app-table
        [data]="sessions()"
        [columns]="[
            { key: 'date', label: 'Date' },
            { key: 'content', label: 'Content Preview' },
            { key: 'status', label: 'Status' }
        ]"
        [loading]="loading()"
        emptyMessage="No sessions found"
    >
        <ng-template #cell let-row let-col="col">
            <ng-container *if="col.key === 'date'">
                {{ row.date | date:'short' }}
            </ng-container>
            <ng-container *if="col.key === 'content'">
                {{ (row.content | truncate:50) || '(No content)' }}
            </ng-container>
            <ng-container *if="col.key === 'status'">
                <span class="badge" [class.bg-secondary]="row.status === 'Draft'" [class.bg-success]="row.status === 'Completed'" [class.bg-warning]="row.status === 'Archived'">
                    {{ row.status }}
                </span>
            </ng-container>
        </ng-template>
        <ng-template #actions let-row>
            <app-button variant="secondary" size="sm" [routerLink]="['/sessions', row.id]">View</app-button>
            <app-button variant="primary" size="sm" [routerLink]="['/sessions', row.id, 'edit']">Edit</app-button>
            <app-button variant="danger" size="sm" (click)="deleteSession(row.id)">Delete</app-button>
        </ng-template>
    </app-table>
</div>
```
#### Step 3: Add Styles

```css
// session-list.component.scss
.session-list {
    padding: 1rem 0;
}
```
### Files To Create

*   `session-list.component.ts`
*   `session-list.component.html`
*   `session-list.component.scss`

### CLI Commands

\# Already generated

### Concepts Required

*   ActivatedRoute
*   Shared components

### Testing Steps

1.  Navigate to session list for a patient.
2.  Verify sessions display.
3.  Test delete action.

### Expected Deliverables

*   Session list component ready.

### Common Mistakes

**Mistake 1:** Forgetting to pass patient ID to API.

### Edge Cases

*   No sessions → empty state.

### Definition of Done

*   List works with CRUD actions.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Session list is accessed from patient detail.

## FE-SES-003Session Form with Rich Text Editor P0 High 6h ▾

### Task Information

*   **Task ID:** FE-SES-003
*   **Task Name:** Session Form with Rich Text Editor
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SES-001, FE-SHARED-003 (Input), FE-SHARED-002 (Button), ngx-quill (or similar)
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Build a form for creating and editing session notes with a rich text editor (Quill), date picker, and optional voice memo upload.

### Business Purpose

Enable therapists to create detailed, formatted session notes.

### Technical Purpose

Use Quill editor for rich text, integrate with SessionService, and handle voice memo upload.

### Prerequisites

*   Install `ngx-quill` and `quill`.
*   Reactive Forms.

### Dependencies

*   **FE-SES-001:** SessionService and StateService.
*   **FE-SES-004:** VoiceRecorderComponent (will be available).

### Inputs

*   Patient ID (from route).
*   Session ID for edit (from route).

### Outputs

*   `session-form.component.ts`
*   `session-form.component.html`
*   `session-form.component.scss`

### Detailed Workflow

#### Step 1: Install Quill

```bash
npm install quill ngx-quill
```
#### Step 2: Import Quill in `angular.json` (or `styles.scss`)

```json
// angular.json
"styles": [
    "src/styles.scss",
    "node_modules/quill/dist/quill.snow.css"
]
```
#### Step 3: Implement Component

```ts
// session-form.component.ts
import { Component, inject, signal, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { VoiceRecorderComponent } from '../voice-recorder/voice-recorder.component';
import { QuillModule } from 'ngx-quill';

@Component({
    selector: 'app-session-form',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, ButtonComponent, VoiceRecorderComponent, QuillModule],
    templateUrl: './session-form.component.html',
    styleUrls: ['./session-form.component.scss'],
})
export class SessionFormComponent implements OnInit {
    private fb = inject(FormBuilder);
    private sessionService = inject(SessionService);
    private state = inject(SessionStateService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    patientId = signal('');
    sessionId = signal<string | null>(null);
    loading = signal(false);
    isEdit = signal(false);
    error = signal<string | null>(null);
    voiceMemoUrl = signal<string | null>(null);

    form = this.fb.group({
        date: [new Date().toISOString().split('T')[0], [Validators.required]],
        content: ['', [Validators.required]],
        status: ['Draft'],
    });

    quillConfig = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            ['blockquote', 'code-block'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ size: ['small', false, 'large', 'huge'] }],
            [{ color: [] }, { background: [] }],
            ['link', 'image'],
            ['clean'],
        ],
    };

    ngOnInit(): void {
        const patientId = this.route.snapshot.paramMap.get('patientId');
        if (patientId) {
            this.patientId.set(patientId);
        }
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.sessionId.set(id);
            this.isEdit.set(true);
            this.loadSession(id);
        }
    }

    loadSession(id: string) {
        this.loading.set(true);
        this.sessionService.getSession(id).subscribe({
            next: (session) => {
                this.form.patchValue({
                    date: session.date,
                    content: session.content,
                    status: session.status,
                });
                this.voiceMemoUrl.set(session.voiceMemoUrl);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to load session');
                this.loading.set(false);
            },
        });
    }

    onVoiceUploaded(url: string) {
        this.voiceMemoUrl.set(url);
    }

    onSubmit() {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        const data = this.form.value;

        if (this.isEdit()) {
            this.sessionService.updateSession(this.sessionId()!, data).subscribe({
                next: (session) => {
                    this.state.updateSession(session);
                    this.loading.set(false);
                    this.router.navigate(['/sessions', session.id]);
                },
                error: (err) => {
                    this.error.set(err.message || 'Failed to update session');
                    this.loading.set(false);
                },
            });
        } else {
            this.sessionService.createSession({ ...data, patientId: this.patientId() }).subscribe({
                next: (session) => {
                    this.state.addSession(session);
                    this.loading.set(false);
                    this.router.navigate(['/sessions', session.id]);
                },
                error: (err) => {
                    this.error.set(err.message || 'Failed to create session');
                    this.loading.set(false);
                },
            });
        }
    }
}
```
#### Step 4: Create Template

```html
<!-- session-form.component.html -->
<div class="session-form">
    <h2>{{ isEdit() ? 'Edit Session' : 'New Session' }}</h2>

    <div *if="loading()" class="text-center py-5">
        <app-spinner size="lg"></app-spinner>
    </div>

    <div *if="error()" class="alert alert-danger">{{ error() }}</div>

    <form [formGroup]="form" (ngSubmit)="onSubmit()" *if="!loading()">
        <app-input
            label="Date"
            type="date"
            formControlName="date"
            [error]="form.get('date')?.invalid && form.get('date')?.touched ? 'Date is required' : ''"
        ></app-input>

        <div class="form-group">
            <label>Session Notes (Rich Text)</label>
            <quill-editor
                formControlName="content"
                [modules]="quillConfig"
                placeholder="Enter session notes..."
            ></quill-editor>
        </div>

        <div class="form-group">
            <label>Status</label>
            <select class="form-select" formControlName="status">
                <option value="Draft">Draft</option>
                <option value="Completed">Completed</option>
                <option value="Archived">Archived</option>
            </select>
        </div>

        <div class="mt-3">
            <app-voice-recorder
                (uploaded)="onVoiceUploaded($event)"
                [sessionId]="sessionId()"
            ></app-voice-recorder>
            <div *if="voiceMemoUrl()" class="mt-2 text-success">
                <i class="bi bi-check-circle"></i> Voice memo uploaded.
            </div>
        </div>

        <div class="mt-4">
            <app-button type="submit" variant="primary" [loading]="loading()">
                {{ isEdit() ? 'Update' : 'Create' }} Session
            </app-button>
            <app-button type="button" variant="secondary" class="ms-2" (click)="router.navigate(['/sessions/patient', patientId()])">Cancel</app-button>
        </div>
    </form>
</div>
```
#### Step 5: Add Styles

```css
// session-form.component.scss
.session-form {
    padding: 1rem 0;
}
quill-editor {
    min-height: 200px;
}
.ql-container {
    min-height: 150px;
}
```
### Files To Create

*   `session-form.component.ts`
*   `session-form.component.html`
*   `session-form.component.scss`

### CLI Commands

\# Already generated npm install quill ngx-quill

### Concepts Required

*   Quill rich text editor
*   Reactive Forms

### Testing Steps

1.  Test create new session with rich text.
2.  Test edit existing session.
3.  Test voice memo upload.

### Expected Deliverables

*   Session form with rich text and voice.

### Common Mistakes

**Mistake 1:** Not including Quill CSS in angular.json.

### Edge Cases

*   Large content → Quill handles it.

### Definition of Done

*   Form works with rich text and voice.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use Quill for notes.




## FE-SES-005AI Summary Display Component P1 Medium 3h ▾

### Task Information

*   **Task ID:** FE-SES-005
*   **Task Name:** AI Summary Display Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-SES-001 (SessionService)
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** High

### Objective

Create a component that displays the AI‑generated summary for a session with loading states and error handling.

### Business Purpose

Therapists need to see the AI summarisation of session notes.

### Technical Purpose

Fetch the summary from the backend using SessionService, show a loading spinner, and display the summary.

### Prerequisites

*   Understanding of async data fetching.

### Dependencies

*   **FE-SES-001:** SessionService.

### Inputs

*   `sessionId` – to fetch the summary.

### Outputs

*   None (display only).

### Outputs (Files)

*   `summary.component.ts`
*   `summary.component.html`
*   `summary.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// summary.component.ts
import { Component, Input, signal, inject, OnInit } from '@angular/core';
import { SessionService } from '../../../../core/services/session.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-summary',
    standalone: true,
    imports: [SpinnerComponent],
    templateUrl: './summary.component.html',
    styleUrls: ['./summary.component.scss'],
})
export class SummaryComponent implements OnInit {
    @Input() sessionId = '';
    private sessionService = inject(SessionService);

    loading = signal(true);
    summary = signal<string | null>(null);
    error = signal<string | null>(null);

    ngOnInit(): void {
        if (this.sessionId) {
            this.loadSummary();
        }
    }

    loadSummary() {
        this.loading.set(true);
        this.sessionService.getSummary(this.sessionId).subscribe({
            next: (result) => {
                this.summary.set(result.summary);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to load summary');
                this.loading.set(false);
            },
        });
    }
}
```
#### Step 2: Create Template

```html
<!-- summary.component.html -->
<div class="summary">
    <h5>AI Summary</h5>
    <div *if="loading()" class="text-center py-3">
        <app-spinner size="md"></app-spinner>
    </div>
    <div *if="error()" class="alert alert-warning">{{ error() }}</div>
    <div *if="summary()" class="summary-content">
        {{ summary() }}
    </div>
    <div *if="!loading() && !summary() && !error()" class="text-muted">
        No summary available.
    </div>
</div>
```
#### Step 3: Add Styles

```css
// summary.component.scss
.summary-content {
    background: #f8f9fa;
    padding: 1rem;
    border-radius: 4px;
    border-left: 4px solid #3b82f6;
}
```
### Files To Create

*   `summary.component.ts`
*   `summary.component.html`
*   `summary.component.scss`

### CLI Commands

\# Already generated

### Concepts Required

*   Input binding
*   Async data loading

### Testing Steps

1.  Test with valid session ID.
2.  Test with missing session ID.

### Expected Deliverables

*   Summary component ready.

### Common Mistakes

**Mistake 1:** Not handling empty summary gracefully.

### Definition of Done

*   Summary displays correctly.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Place this in session detail view.

FE-SES-006State Management and Integration P1 Medium 3h ▾

### Task Information

*   **Task ID:** FE-SES-006
*   **Task Name:** State Management and Integration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous session tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** High

### Objective

Ensure all session components use the SessionStateService consistently, add caching and error handling, and integrate with the patient module.

### Business Purpose

Unified state management reduces bugs and ensures data consistency.

### Technical Purpose

Update all components to use the state service, add methods for clearing state, and integrate with PatientDetailComponent tabs.

### Prerequisites

*   All session components implemented.

### Dependencies

*   All FE-SES tasks.

### Inputs

*   Existing components.

### Outputs

*   Updated components using state service consistently.
*   Integration with PatientDetailComponent.

### Detailed Workflow

#### Step 1: Refactor Components to Use State Service

Ensure SessionListComponent, SessionFormComponent, etc., use `SessionStateService` for data and loading states.

#### Step 2: Add Clear State on Navigation

When navigating away from session detail, clear selected session.

// In session-detail.component.ts (if created) ngOnDestroy() { this.state.clearSelected(); }

#### Step 3: Integrate with Patient Detail

In PatientDetailComponent, add a tab for sessions using `<app-session-list [patientId]="patient()?.id"></app-session-list>`.

#### Step 4: Add Error Handling

Ensure every API call in SessionService has error handling and updates the state error signal.

#### Step 5: Add Unit Tests for State Service

Test that state updates correctly.

### Files To Modify

*   All session components.
*   PatientDetailComponent (add session tab).
*   SessionStateService.

### CLI Commands

No new commands.

### Concepts Required

*   State management best practices
*   Lifecycle hooks

### Testing Steps

1.  Verify state persists correctly between components.
2.  Test that navigation clears state.

### Expected Deliverables

*   Session management fully integrated.

### Common Mistakes

**Mistake 1:** Not clearing state on destroy, causing stale data.

### Definition of Done

*   All components use state service and integrate with patient module.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use SessionStateService for all session data.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 6 session tasks are complete (FE-SES-001 to FE-SES-006).
*   Session routes are lazy‑loaded and guarded.
*   SessionService and SessionStateService are implemented.
*   Session List displays sessions for a patient.
*   Session Form supports rich text and voice memo upload.
*   AI Summary component displays summaries.
*   All components use the state service.
*   Integration with PatientDetailComponent works.
*   All tests pass.
*   All changes are committed.

📋 Code Review Checklist
------------------------

*   SessionService methods are typed and handle errors.
*   State service uses Signals correctly.
*   Rich text editor is properly configured.
*   AI summary has loading and error states.

🚀 Pull Request Checklist
-------------------------

*   Branch up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   Documentation updated.

🧪 Deployment Readiness Checklist
---------------------------------

*   Session CRUD works end‑to‑end.
*   AI summary is displayed.

* * *

Jalsa – Phase 8: Session Management – Expanded Implementation Handbook • v1.0 • For development team

