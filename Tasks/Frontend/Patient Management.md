
👤 Jalsa – Phase 7: Patient Management Expanded Implementation Handbook · 7 Tasks · 40+ Subtasks
================================================================================================

👤 Phase 7 – Patient Management
-------------------------------

**Purpose:** Build the complete patient management feature: patient list, details, create/edit forms, digital intake forms with OCR image upload, and psychological assessment recording. This phase enables therapists to manage patient data efficiently.

📋 Tasks: 7 ⏱️ Total Effort: ~34 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 4 (Shared Components), Phase 5 (Authentication), Phase 6 (Layout System) 🎯 Deliverable: Complete patient management system with CRUD, intake forms, and assessments

## FE-PAT-001Patient Module Setup (Routes, Service, State) P0 High 5h ▾

### Task Information

*   **Task ID:** FE-PAT-001
*   **Task Name:** Patient Module Setup (Routes, Service, State)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001 (HttpClientService), FE-CORE-002 (Models), FE-AUTH-002 (AuthService), FE-LAYOUT-001 (Main layout)
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Create the patient feature module with lazy‑loaded routes, a PatientService for API communication, and a PatientStateService for managing patient state using Signals.

### Business Purpose

Patient management is the core of the application. A well‑structured setup ensures scalability and maintainability.

### Technical Purpose

Set up lazy‑loaded routes under the main layout, create a service that uses HttpClientService for CRUD operations, and create a state service with Signals for reactive data management.

### Prerequisites

*   Understanding of Angular lazy loading with Standalone components.
*   Knowledge of Signals and services.
*   API contract for patient endpoints.

### Dependencies

*   **FE-CORE-001:** HttpClientService for API calls.
*   **FE-CORE-002:** Patient model interfaces.
*   **FE-AUTH-002:** AuthService for guards.
*   **FE-LAYOUT-001:** Main layout as parent route.

### Inputs

*   API endpoints from `api-endpoints.ts`.

### Outputs

*   `features/patients/patients.routes.ts` – Lazy‑loaded routes.
*   `core/services/patient.service.ts` – Patient API service.
*   `core/state/patient-state.service.ts` – Patient state management with Signals.
*   Placeholder components (list, detail, form, intake, assessment).

### Detailed Workflow

#### Step 1: Create Routes File

```typescript
// features/patients/patients.routes.ts
import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const PATIENTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Admin'])],
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/patient-list/patient-list.component').then(m => m.PatientListComponent),
            },
            {
                path: 'new',
                loadComponent: () => import('./pages/patient-form/patient-form.component').then(m => m.PatientFormComponent),
            },
            {
                path: ':id',
                loadComponent: () => import('./pages/patient-detail/patient-detail.component').then(m => m.PatientDetailComponent),
            },
            {
                path: ':id/edit',
                loadComponent: () => import('./pages/patient-form/patient-form.component').then(m => m.PatientFormComponent),
            },
            {
                path: ':id/intake',
                loadComponent: () => import('./pages/intake-form/intake-form.component').then(m => m.IntakeFormComponent),
            },
            {
                path: ':id/assessments',
                loadComponent: () => import('./pages/assessment/assessment.component').then(m => m.AssessmentComponent),
            },
        ],
    },
];
```
#### Step 2: Register Routes in App Routes

Add lazy loading in `app.routes.ts` under the main layout.

```typescript
// app.routes.ts (inside the main layout children)
{
    path: 'patients',
    loadChildren: () => import('./features/patients/patients.routes').then(m => m.PATIENTS_ROUTES),
},
```
#### Step 3: Create Patient Service

```typescript
// core/services/patient.service.ts
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { Patient, CreatePatientRequest, UpdatePatientRequest, IntakeForm, Assessment, AssessmentRequest } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientService {
    private http = inject(HttpClientService);

    getPatients(params: { search?: string; page?: number; size?: number; includeArchived?: boolean }): Observable<{ items: Patient[]; totalCount: number }> {
        return this.http.get<{ items: Patient[]; totalCount: number }>(API.patients.base, { params });
    }

    getPatient(id: string): Observable<Patient> {
        return this.http.get<Patient>(API.patients.byId(id));
    }

    createPatient(data: CreatePatientRequest): Observable<Patient> {
        return this.http.post<Patient>(API.patients.base, data);
    }

    updatePatient(id: string, data: UpdatePatientRequest): Observable<Patient> {
        return this.http.put<Patient>(API.patients.byId(id), data);
    }

    archivePatient(id: string): Observable<void> {
        return this.http.delete<void>(API.patients.archive(id));
    }

    restorePatient(id: string): Observable<void> {
        return this.http.post<void>(API.patients.restore(id), null);
    }

    getIntakeForm(patientId: string): Observable<IntakeForm> {
        return this.http.get<IntakeForm>(API.patients.intake(patientId));
    }

    saveIntakeForm(patientId: string, data: any): Observable<IntakeForm> {
        return this.http.post<IntakeForm>(API.patients.intake(patientId), data);
    }

    uploadIntakeImage(patientId: string, file: File): Observable<{ imageUrl: string; extractedData: any }> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.upload<{ imageUrl: string; extractedData: any }>(API.patients.intakeImage(patientId), formData);
    }

    getAssessments(patientId: string): Observable<Assessment[]> {
        return this.http.get<Assessment[]>(API.patients.assessments(patientId));
    }

    addAssessment(patientId: string, data: AssessmentRequest): Observable<Assessment> {
        return this.http.post<Assessment>(API.patients.assessments(patientId), data);
    }
}
```
#### Step 4: Create Patient State Service

```typescript
// core/state/patient-state.service.ts
import { Injectable, signal, computed } from '@angular/core';
import { Patient } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientStateService {
    private patientsSignal = signal<Patient[]>([]);
    private selectedPatientSignal = signal<Patient | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly patients = this.patientsSignal.asReadonly();
    readonly selectedPatient = this.selectedPatientSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setPatients(patients: Patient[]) {
        this.patientsSignal.set(patients);
        this.errorSignal.set(null);
    }

    addPatient(patient: Patient) {
        this.patientsSignal.update(list => [...list, patient]);
    }

    updatePatient(updated: Patient) {
        this.patientsSignal.update(list => list.map(p => p.id === updated.id ? updated : p));
        if (this.selectedPatientSignal()?.id === updated.id) {
            this.selectedPatientSignal.set(updated);
        }
    }

    removePatient(id: string) {
        this.patientsSignal.update(list => list.filter(p => p.id !== id));
    }

    selectPatient(patient: Patient) {
        this.selectedPatientSignal.set(patient);
    }

    clearSelected() {
        this.selectedPatientSignal.set(null);
    }

    setLoading(loading: boolean) {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null) {
        this.errorSignal.set(error);
    }

    reset() {
        this.patientsSignal.set([]);
        this.selectedPatientSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
```
#### Step 5: Generate Placeholder Components

```bash
ng g c features/patients/pages/patient-list --standalone --skip-tests
ng g c features/patients/pages/patient-detail --standalone --skip-tests
ng g c features/patients/pages/patient-form --standalone --skip-tests
ng g c features/patients/pages/intake-form --standalone --skip-tests
ng g c features/patients/pages/assessment --standalone --skip-tests
```
### Files To Create

*   `features/patients/patients.routes.ts`
*   `core/services/patient.service.ts`
*   `core/state/patient-state.service.ts`
*   Placeholder components.

### CLI Commands

```bash
ng g c features/patients/pages/patient-list --standalone --skip-tests
ng g c features/patients/pages/patient-detail --standalone --skip-tests
ng g c features/patients/pages/patient-form --standalone --skip-tests
ng g c features/patients/pages/intake-form --standalone --skip-tests
ng g c features/patients/pages/assessment --standalone --skip-tests
```
### Code Flow

```
User navigates to /patients → PatientListComponent loads → Calls PatientService.getPatients() → Updates PatientStateService.patients → Renders list
```
### Concepts Required

*   Lazy loading with loadChildren
*   Standalone components
*   Angular Signals for state
*   Dependency injection

### Tools/Libraries Required

*   Angular Router

### Testing Steps

1.  Verify routes are lazy‑loaded (network tab).
2.  Test PatientService methods with mock data.
3.  Test PatientStateService signals update correctly.

### Expected Deliverables

*   Patient routes, service, and state service ready.

### Common Mistakes

**Mistake 1:** Not applying guards to patient routes.  
**Fix:** Add `canActivate: [authGuard, roleGuard(['Therapist'])]`.

**Mistake 2:** Not using the PatientStateService for reactivity.  
**Fix:** Use the state service in components.

### Edge Cases

*   User without Therapist role should not access patient routes.

### Definition of Done

*   Routes, service, and state service are implemented.

### Frontend Architecture Notes

*   PatientStateService is the single source of truth for patient data.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use PatientService for API calls and PatientStateService for state.

## FE-PAT-002Patient List Component P0 High 5h ▾

### Task Information

*   **Task ID:** FE-PAT-002
*   **Task Name:** Patient List Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PAT-001, FE-SHARED-005 (Table), FE-SHARED-006 (Pagination)
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Build the patient list page with search, filtering, pagination, archive toggle, and actions (view, edit, archive/restore).

### Business Purpose

Allow therapists to browse, search, and manage patients efficiently.

### Technical Purpose

Use shared Table and Pagination components, integrate with PatientService and PatientStateService, implement debounced search, and handle pagination.

### Prerequisites

*   Understanding of RxJS operators (`switchMap`, `debounceTime`).
*   Shared Table and Pagination components.

### Dependencies

*   **FE-PAT-001:** PatientService and StateService.
*   **FE-SHARED-005:** Table component.
*   **FE-SHARED-006:** Pagination component.

### Inputs

*   None.

### Outputs

*   `features/patients/pages/patient-list/patient-list.component.ts`
*   `patient-list.component.html`
*   `patient-list.component.scss`

### Detailed Workflow

#### Step 1: Generate Component (already done)

#### Step 2: Implement Component Logic

```typescript
// patient-list.component.ts
import { Component, inject, signal, OnInit, effect } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { TableComponent } from '../../../../shared/components/table/table.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-patient-list',
    standalone: true,
    imports: [TableComponent, PaginationComponent, ButtonComponent, InputComponent, RouterLink, FormsModule],
    templateUrl: './patient-list.component.html',
    styleUrls: ['./patient-list.component.scss'],
})
export class PatientListComponent implements OnInit {
    private patientService = inject(PatientService);
    private state = inject(PatientStateService);
    private router = inject(Router);

    patients = this.state.patients;
    loading = this.state.loading;
    error = this.state.error;

    searchTerm = '';
    currentPage = 1;
    pageSize = 10;
    includeArchived = false;
    totalItems = 0;

    private searchSubject = new Subject<string>();

    ngOnInit(): void {
        this.loadPatients();

        this.searchSubject.pipe(
            debounceTime(300),
            distinctUntilChanged(),
            switchMap(() => {
                this.currentPage = 1;
                return this.fetchPatients();
            })
        ).subscribe();
    }

    onSearch(term: string) {
        this.searchSubject.next(term);
    }

    onPageChange(page: number) {
        this.currentPage = page;
        this.fetchPatients().subscribe();
    }

    onToggleArchived() {
        this.currentPage = 1;
        this.fetchPatients().subscribe();
    }

    private fetchPatients() {
        this.state.setLoading(true);
        return this.patientService.getPatients({
            search: this.searchTerm || undefined,
            page: this.currentPage,
            size: this.pageSize,
            includeArchived: this.includeArchived,
        }).subscribe({
            next: (result) => {
                this.state.setPatients(result.items);
                this.totalItems = result.totalCount;
                this.state.setLoading(false);
            },
            error: (err) => {
                this.state.setError(err.message || 'Failed to load patients');
                this.state.setLoading(false);
            },
        });
    }

    loadPatients() {
        this.fetchPatients();
    }

    archivePatient(id: string) {
        if (confirm('Are you sure you want to archive this patient?')) {
            this.patientService.archivePatient(id).subscribe({
                next: () => {
                    this.loadPatients();
                },
                error: (err) => {
                    console.error('Archive failed', err);
                },
            });
        }
    }

    restorePatient(id: string) {
        this.patientService.restorePatient(id).subscribe({
            next: () => {
                this.loadPatients();
            },
            error: (err) => {
                console.error('Restore failed', err);
            },
        });
    }
}
```
#### Step 3: Create Template

````html
<!-- patient-list.component.html -->
<div class="patient-list">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2>Patients</h2>
        <app-button variant="primary" routerLink="/patients/new">Add Patient</app-button>
    </div>

    <div class="filters d-flex flex-wrap gap-2 mb-3">
        <app-input
            type="text"
            placeholder="Search patients..."
            [(ngModel)]="searchTerm"
            (ngModelChange)="onSearch($event)"
            class="flex-grow-1"
        ></app-input>
        <div class="form-check">
            <input class="form-check-input" type="checkbox" id="includeArchived" [(ngModel)]="includeArchived" (change)="onToggleArchived()">
            <label class="form-check-label" for="includeArchived">Show Archived</label>
        </div>
    </div>

    <div class="table-responsive">
        <app-table
            [data]="patients()"
            [columns]="[
                { key: 'firstName', label: 'First Name' },
                { key: 'lastName', label: 'Last Name' },
                { key: 'email', label: 'Email' },
                { key: 'dateOfBirth', label: 'DOB' },
                { key: 'gender', label: 'Gender' },
                { key: 'isArchived', label: 'Archived' }
            ]"
            [loading]="loading()"
            emptyMessage="No patients found"
        >
            <ng-template #cell let-row let-col="col">
                <ng-container *ngIf="col.key === 'isArchived'">
                    <span class="badge" [class.bg-secondary]="!row.isArchived" [class.bg-danger]="row.isArchived">
                        {{ row.isArchived ? 'Archived' : 'Active' }}
                    </span>
                </ng-container>
                <ng-container *ngIf="col.key === 'dateOfBirth'">
                    {{ row.dateOfBirth | date:'shortDate' }}
                </ng-container>
            </ng-template>
            <ng-template #actions let-row>
                <app-button variant="secondary" size="sm" [routerLink]="['/patients', row.id]">View</app-button>
                <app-button variant="primary" size="sm" [routerLink]="['/patients', row.id, 'edit']">Edit</app-button>
                <app-button *ngIf="!row.isArchived" variant="danger" size="sm" (click)="archivePatient(row.id)">Archive</app-button>
                <app-button *ngIf="row.isArchived" variant="success" size="sm" (click)="restorePatient(row.id)">Restore</app-button>
            </ng-template>
        </app-table>
    </div>

    <div class="mt-3">
        <app-pagination
            [totalItems]="totalItems"
            [pageSize]="pageSize"
            [currentPage]="currentPage"
            (pageChange)="onPageChange($event)"
        ></app-pagination>
    </div>
</div>
````
#### Step 4: Add Styles

```css
// patient-list.component.scss
.patient-list {
    padding: 1rem 0;
}
.filters {
    gap: 0.5rem;
}
.filters .form-check {
    display: flex;
    align-items: center;
}
.table-responsive {
    overflow-x: auto;
}
```
### Files To Create

*   `patient-list.component.ts`
*   `patient-list.component.html`
*   `patient-list.component.scss`

### CLI Commands

```bash
\# Already generated; now modify
```
### Concepts Required

*   RxJS: `Subject`, `debounceTime`, `switchMap`
*   Angular Signals
*   Shared components integration

### Testing Steps

1.  Test search debounces and fetches results.
2.  Test pagination changes page and fetches data.
3.  Test archive/restore updates the list.
4.  Test show archived toggle.

### Expected Deliverables

*   Patient list component fully functional.

### Common Mistakes

**Mistake 1:** Not unsubscribing from observables – but we use `Subject` with `switchMap` and subscribe in component; consider using `takeUntil`.  
**Fix:** Use `takeUntil` or `async` pipe.

**Mistake 2:** Forgetting to update `totalItems` from response.

### Edge Cases

*   Search returns no results → empty state.
*   Pagination with 0 items → hide pagination.

### Definition of Done

*   Patient list works with search, pagination, and actions.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Patient list is the entry point for patient management.

## FE-PAT-003Patient Detail Component P0 High 6h ▾

### Task Information

*   **Task ID:** FE-PAT-003
*   **Task Name:** Patient Detail Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PAT-001, FE-PAT-002, FE-SHARED-004 (Modal)
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** Critical

### Objective

Create the patient detail page with tabs for overview, sessions, exercises, reports, and assessments. Display patient information, intake form, and allow navigation to related modules.

### Business Purpose

Provide a comprehensive view of a patient's data, enabling therapists to access all relevant information in one place.

### Technical Purpose

Load patient data via PatientService, display in a tabbed interface, and integrate with child components for sessions, exercises, etc.

### Prerequisites

*   Understanding of Angular tabs (Bootstrap or custom).
*   Router and route parameters.

### Dependencies

*   **FE-PAT-001:** PatientService and StateService.
*   **FE-SHARED-004:** Modal (optional).

### Inputs

*   Patient ID from route.

### Outputs

*   `patient-detail.component.ts`
*   `patient-detail.component.html`
*   `patient-detail.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```typescript
// patient-detail.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'app-patient-detail',
    standalone: true,
    imports: [RouterLink, ButtonComponent, DatePipe],
    templateUrl: './patient-detail.component.html',
    styleUrls: ['./patient-detail.component.scss'],
})
export class PatientDetailComponent implements OnInit {
    private route = inject(ActivatedRoute);
    private patientService = inject(PatientService);
    private state = inject(PatientStateService);
    private router = inject(Router);

    patient = this.state.selectedPatient;
    loading = signal(true);
    error = signal<string | null>(null);
    activeTab = 'overview';

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.loadPatient(id);
        } else {
            this.error.set('Patient ID not found');
            this.loading.set(false);
        }
    }

    loadPatient(id: string) {
        this.loading.set(true);
        this.patientService.getPatient(id).subscribe({
            next: (patient) => {
                this.state.selectPatient(patient);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to load patient');
                this.loading.set(false);
            },
        });
    }

    setTab(tab: string) {
        this.activeTab = tab;
    }

    goBack() {
        this.router.navigate(['/patients']);
    }
}
```
#### Step 2: Create Template

```html
<!-- patient-detail.component.html -->
<div class="patient-detail">
    <div class="d-flex justify-content-between align-items-start mb-3">
        <div>
            <button class="btn btn-link p-0" (click)="goBack()">
                <i class="bi bi-arrow-left"></i> Back
            </button>
            <h2 class="mt-2" *ngIf="patient()">{{ patient()?.firstName }} {{ patient()?.lastName }}</h2>
        </div>
        <div class="d-flex gap-2" *ngIf="patient()">
            <app-button variant="primary" [routerLink]="['/patients', patient()?.id, 'edit']">Edit</app-button>
            <app-button variant="secondary" [routerLink]="['/patients', patient()?.id, 'intake']">Intake Form</app-button>
            <app-button variant="info" [routerLink]="['/patients', patient()?.id, 'assessments']">Assessments</app-button>
        </div>
    </div>

    <div *ngIf="loading()" class="text-center py-5">
        <app-spinner size="lg"></app-spinner>
    </div>

    <div *ngIf="error()" class="alert alert-danger">{{ error() }}</div>

    <div *ngIf="patient() && !loading()">
        <div class="card mb-3">
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <p><strong>Email:</strong> {{ patient()?.email }}</p>
                        <p><strong>Date of Birth:</strong> {{ patient()?.dateOfBirth | date:'shortDate' }}</p>
                        <p><strong>Gender:</strong> {{ patient()?.gender }}</p>
                    </div>
                    <div class="col-md-6">
                        <p><strong>Emergency Contact:</strong> {{ patient()?.emergencyContact || 'N/A' }}</p>
                        <p><strong>Status:</strong>
                            <span class="badge" [class.bg-success]="!patient()?.isArchived" [class.bg-danger]="patient()?.isArchived">
                                {{ patient()?.isArchived ? 'Archived' : 'Active' }}
                            </span>
                        </p>
                    </div>
                </div>
                <div class="mt-2" *ngIf="patient()?.notes">
                    <strong>Notes:</strong>
                    <p>{{ patient()?.notes }}</p>
                </div>
            </div>
        </div>

        <!-- Tabs -->
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <a class="nav-link" [class.active]="activeTab === 'overview'" (click)="setTab('overview')">Overview</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" [class.active]="activeTab === 'sessions'" (click)="setTab('sessions')">Sessions</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" [class.active]="activeTab === 'exercises'" (click)="setTab('exercises')">Exercises</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" [class.active]="activeTab === 'reports'" (click)="setTab('reports')">Reports</a>
            </li>
        </ul>

        <div class="tab-content mt-3">
            <div class="tab-pane" [class.active]="activeTab === 'overview'">
                <!-- Overview content: intake form, assessments summary -->
                <p>Intake form and assessments will be displayed here.</p>
            </div>
            <div class="tab-pane" [class.active]="activeTab === 'sessions'">
                <app-session-list [patientId]="patient()?.id" *ngIf="patient()?.id"></app-session-list>
            </div>
            <div class="tab-pane" [class.active]="activeTab === 'exercises'">
                <app-exercise-list [patientId]="patient()?.id" *ngIf="patient()?.id"></app-exercise-list>
            </div>
            <div class="tab-pane" [class.active]="activeTab === 'reports'">
                <app-report-list [patientId]="patient()?.id" *ngIf="patient()?.id"></app-report-list>
            </div>
        </div>
    </div>
</div>
```
#### Step 3: Add Styles

```css
// patient-detail.component.scss
.patient-detail {
    padding: 1rem 0;
}
.nav-tabs .nav-link {
    cursor: pointer;
}
.tab-content .tab-pane {
    display: none;
}
.tab-content .tab-pane.active {
    display: block;
}
```
### Files To Create

*   `patient-detail.component.ts`
*   `patient-detail.component.html`
*   `patient-detail.component.scss`

### CLI Commands

\# Already generated

### Concepts Required

*   ActivatedRoute for route params
*   Bootstrap tabs
*   Child components integration

### Testing Steps

1.  Navigate to a patient detail page with valid ID.
2.  Verify patient data displays.
3.  Test tab switching.
4.  Test navigation to related components.

### Expected Deliverables

*   Patient detail component with tabs.

### Common Mistakes

**Mistake 1:** Not handling invalid patient ID gracefully.  
**Fix:** Show error message and provide back navigation.

### Edge Cases

*   Patient has no sessions/exercises etc. → empty state.

### Definition of Done

*   Detail page works and integrates with other components.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Detail page uses tabs for related data.

## FE-PAT-004Patient Form (Create/Edit) P0 Medium 5h ▾

### Task Information

*   **Task ID:** FE-PAT-004
*   **Task Name:** Patient Form (Create/Edit)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PAT-001, FE-SHARED-003 (Input components)
*   **Complexity:** Medium
*   **Estimated Effort:** 5 hours
*   **Priority:** Critical

### Objective

Build a reactive form for creating and editing patients, with validation, loading state, and navigation.

### Business Purpose

Allow therapists to add new patients and update existing ones.

### Technical Purpose

Use Reactive Forms with validation, patch values for edit, and submit to PatientService.

### Prerequisites

*   Reactive Forms and custom validators.
*   Shared input components.

### Dependencies

*   **FE-PAT-001:** PatientService and StateService.
*   **FE-SHARED-003:** Input components.

### Inputs

*   Patient ID for edit (from route).

### Outputs

*   `patient-form.component.ts`
*   `patient-form.component.html`
*   `patient-form.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// patient-form.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../../../core/services/patient.service';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
    selector: 'app-patient-form',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, ButtonComponent],
    templateUrl: './patient-form.component.html',
    styleUrls: ['./patient-form.component.scss'],
})
export class PatientFormComponent implements OnInit {
    private fb = inject(FormBuilder);
    private patientService = inject(PatientService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    patientId = signal<string | null>(null);
    loading = signal(false);
    isEdit = signal(false);
    error = signal<string | null>(null);

    form = this.fb.group({
        firstName: ['', [Validators.required, Validators.minLength(2)]],
        lastName: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
        dateOfBirth: ['', [Validators.required]],
        gender: ['Male', [Validators.required]],
        emergencyContact: [''],
        notes: [''],
    });

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.patientId.set(id);
            this.isEdit.set(true);
            this.loadPatient(id);
        }
    }

    loadPatient(id: string) {
        this.loading.set(true);
        this.patientService.getPatient(id).subscribe({
            next: (patient) => {
                this.form.patchValue({
                    firstName: patient.firstName,
                    lastName: patient.lastName,
                    email: patient.email,
                    dateOfBirth: patient.dateOfBirth,
                    gender: patient.gender,
                    emergencyContact: patient.emergencyContact || '',
                    notes: patient.notes || '',
                });
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to load patient');
                this.loading.set(false);
            },
        });
    }

    onSubmit() {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        const data = this.form.value;

        if (this.isEdit()) {
            this.patientService.updatePatient(this.patientId()!, data).subscribe({
                next: () => {
                    this.loading.set(false);
                    this.router.navigate(['/patients', this.patientId()]);
                },
                error: (err) => {
                    this.loading.set(false);
                    this.error.set(err.message || 'Failed to update patient');
                },
            });
        } else {
            this.patientService.createPatient(data).subscribe({
                next: (patient) => {
                    this.loading.set(false);
                    this.router.navigate(['/patients', patient.id]);
                },
                error: (err) => {
                    this.loading.set(false);
                    this.error.set(err.message || 'Failed to create patient');
                },
            });
        }
    }
}
```
#### Step 2: Create Template

```html
<!-- patient-form.component.html -->
<div class="patient-form">
    <h2>{{ isEdit() ? 'Edit Patient' : 'Add New Patient' }}</h2>

    <div *ngIf="loading()" class="text-center py-5">
        <app-spinner size="lg"></app-spinner>
    </div>

    <div *ngIf="error()" class="alert alert-danger">{{ error() }}</div>

    <form [formGroup]="form" (ngSubmit)="onSubmit()" *ngIf="!loading()">
        <div class="row">
            <div class="col-md-6">
                <app-input
                    label="First Name"
                    type="text"
                    placeholder="Enter first name"
                    formControlName="firstName"
                    [error]="form.get('firstName')?.invalid && form.get('firstName')?.touched ? 'First name is required' : ''"
                ></app-input>
            </div>
            <div class="col-md-6">
                <app-input
                    label="Last Name"
                    type="text"
                    placeholder="Enter last name"
                    formControlName="lastName"
                    [error]="form.get('lastName')?.invalid && form.get('lastName')?.touched ? 'Last name is required' : ''"
                ></app-input>
            </div>
        </div>

        <app-input
            label="Email Address"
            type="email"
            placeholder="Enter email"
            formControlName="email"
            [error]="form.get('email')?.invalid && form.get('email')?.touched ? 'Valid email is required' : ''"
        ></app-input>

        <app-input
            label="Date of Birth"
            type="date"
            placeholder="Select date"
            formControlName="dateOfBirth"
            [error]="form.get('dateOfBirth')?.invalid && form.get('dateOfBirth')?.touched ? 'Date of birth is required' : ''"
        ></app-input>

        <div class="form-group">
            <label>Gender</label>
            <select class="form-select" formControlName="gender">
                <option value="Male">Male</option>
                <option value="Female">Female</option>
                <option value="Other">Other</option>
            </select>
        </div>

        <app-input
            label="Emergency Contact"
            type="text"
            placeholder="Emergency contact info"
            formControlName="emergencyContact"
        ></app-input>

        <div class="form-group">
            <label>Notes</label>
            <textarea class="form-control" formControlName="notes" rows="3"></textarea>
        </div>

        <div class="mt-4">
            <app-button type="submit" variant="primary" [loading]="loading()">
                {{ isEdit() ? 'Update' : 'Create' }} Patient
            </app-button>
            <app-button type="button" variant="secondary" class="ms-2" (click)="router.navigate(['/patients'])">Cancel</app-button>
        </div>
    </form>
</div>
```
### Files To Create

*   `patient-form.component.ts`, .html, .scss

### CLI Commands

\# Already generated

### Concepts Required

*   Reactive Forms
*   PatchValue for edit

### Testing Steps

1.  Test create new patient: validation, success redirect.
2.  Test edit existing patient: loads data, saves changes.

### Expected Deliverables

*   Patient form fully functional.

### Common Mistakes

**Mistake 1:** Not resetting errors on successful submission.

### Edge Cases

*   Email already exists → error from API.

### Definition of Done

*   Form works for create and edit.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use the form for patient CRUD.

## FE-PAT-005Intake Form Component with OCR Upload P1 High 6h ▾

### Task Information

*   **Task ID:** FE-PAT-005
*   **Task Name:** Intake Form Component with OCR Upload
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PAT-001, FE-SHARED-003 (Input)
*   **Complexity:** High
*   **Estimated Effort:** 6 hours
*   **Priority:** High

### Objective

Create a component for digital intake form entry, including image upload and OCR preview. Allow therapists to upload a scanned intake form and extract data automatically.

### Business Purpose

Enable efficient data entry for new patients using OCR.

### Technical Purpose

Combine a reactive form with a file upload, send image to backend for OCR, and pre‑fill the form with extracted data.

### Prerequisites

*   File upload with FormData.
*   Understanding of reactive forms.

### Dependencies

*   **FE-PAT-001:** PatientService methods for intake.

### Inputs

*   Patient ID from route.

### Outputs

*   `intake-form.component.ts`
*   `intake-form.component.html`
*   `intake-form.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// intake-form.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../../../core/services/patient.service';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
    selector: 'app-intake-form',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, ButtonComponent],
    templateUrl: './intake-form.component.html',
    styleUrls: ['./intake-form.component.scss'],
})
export class IntakeFormComponent implements OnInit {
    private fb = inject(FormBuilder);
    private patientService = inject(PatientService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    patientId = signal<string>('');
    loading = signal(false);
    error = signal<string | null>(null);
    ocrResult = signal<any | null>(null);

    form = this.fb.group({
        reasonForVisit: [''],
        medicalHistory: [''],
        medications: [''],
        allergies: [''],
        otherInfo: [''],
    });

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.patientId.set(id);
            this.loadIntakeForm();
        } else {
            this.error.set('Patient ID required');
        }
    }

    loadIntakeForm() {
        this.loading.set(true);
        this.patientService.getIntakeForm(this.patientId()).subscribe({
            next: (form) => {
                this.form.patchValue(form);
                this.loading.set(false);
            },
            error: () => {
                // If no form exists, just show empty form
                this.loading.set(false);
            },
        });
    }

    onFileSelected(event: Event) {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            const file = input.files[0];
            this.uploadImage(file);
        }
    }

    uploadImage(file: File) {
        this.loading.set(true);
        this.patientService.uploadIntakeImage(this.patientId(), file).subscribe({
            next: (result) => {
                this.ocrResult.set(result.extractedData);
                // Pre‑fill form with extracted data
                if (result.extractedData) {
                    this.form.patchValue({
                        reasonForVisit: result.extractedData.reasonForVisit || '',
                        medicalHistory: result.extractedData.medicalHistory || '',
                        medications: result.extractedData.medications || '',
                        allergies: result.extractedData.allergies || '',
                        otherInfo: result.extractedData.otherInfo || '',
                    });
                }
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to upload image');
                this.loading.set(false);
            },
        });
    }

    onSubmit() {
        this.loading.set(true);
        this.patientService.saveIntakeForm(this.patientId(), this.form.value).subscribe({
            next: () => {
                this.loading.set(false);
                this.router.navigate(['/patients', this.patientId()]);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to save intake form');
                this.loading.set(false);
            },
        });
    }
}
```
#### Step 2: Create Template

```html
<!-- intake-form.component.html -->
<div class="intake-form">
    <h2>Intake Form</h2>

    <div class="mb-3">
        <label class="form-label">Upload Intake Form Image</label>
        <input type="file" class="form-control" (change)="onFileSelected($event)" accept="image/*" />
        <div *ngIf="ocrResult()" class="mt-2 text-success">
            <i class="bi bi-check-circle"></i> OCR data extracted successfully.
        </div>
    </div>

    <div *ngIf="loading()" class="text-center py-3">
        <app-spinner size="md"></app-spinner>
    </div>

    <div *ngIf="error()" class="alert alert-danger">{{ error() }}</div>

    <form [formGroup]="form" (ngSubmit)="onSubmit()" *ngIf="!loading()">
        <app-input
            label="Reason for Visit"
            type="text"
            placeholder="Enter reason"
            formControlName="reasonForVisit"
        ></app-input>

        <div class="form-group">
            <label>Medical History</label>
            <textarea class="form-control" formControlName="medicalHistory" rows="3"></textarea>
        </div>

        <app-input
            label="Medications"
            type="text"
            placeholder="List medications"
            formControlName="medications"
        ></app-input>

        <app-input
            label="Allergies"
            type="text"
            placeholder="List allergies"
            formControlName="allergies"
        ></app-input>

        <div class="form-group">
            <label>Other Information</label>
            <textarea class="form-control" formControlName="otherInfo" rows="2"></textarea>
        </div>

        <div class="mt-4">
            <app-button type="submit" variant="primary" [loading]="loading()">Save Intake Form</app-button>
            <app-button type="button" variant="secondary" class="ms-2" (click)="router.navigate(['/patients', patientId()])">Cancel</app-button>
        </div>
    </form>
</div>
```
### Files To Create

*   `intake-form.component.ts`, .html, .scss

### CLI Commands

\# Already generated

### Concepts Required

*   File upload with FormData
*   OCR integration

### Testing Steps

1.  Upload an image and verify OCR data pre‑fills the form.
2.  Save form and navigate back.

### Expected Deliverables

*   Intake form with image upload and OCR.

### Common Mistakes

**Mistake 1:** Not handling large file uploads gracefully.

### Edge Cases

*   No image uploaded → form still works manually.
*   OCR fails → show error and allow manual entry.

### Definition of Done

*   Intake form works with OCR.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** OCR speeds up data entry.

## FE-PAT-006Assessment Component P1 Medium 4h ▾

### Task Information

*   **Task ID:** FE-PAT-006
*   **Task Name:** Assessment Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-PAT-001, FE-SHARED-003 (Input)
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Create a component to record and view psychological assessments for a patient.

### Business Purpose

Track patient scores over time.

### Technical Purpose

Form to select assessment type, enter score, date, and list previous assessments.

### Prerequisites

*   Reactive Forms.

### Dependencies

*   **FE-PAT-001:** PatientService.

### Inputs

*   Patient ID.

### Outputs

*   `assessment.component.ts`
*   `assessment.component.html`
*   `assessment.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// assessment.component.ts
import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientService } from '../../../../core/services/patient.service';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { DatePipe } from '@angular/common';

@Component({
    selector: 'app-assessment',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, ButtonComponent, DatePipe],
    templateUrl: './assessment.component.html',
    styleUrls: ['./assessment.component.scss'],
})
export class AssessmentComponent implements OnInit {
    private fb = inject(FormBuilder);
    private patientService = inject(PatientService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    patientId = signal<string>('');
    assessments = signal<any[]>([]);
    loading = signal(false);
    error = signal<string | null>(null);

    form = this.fb.group({
        type: ['', [Validators.required]],
        score: [null, [Validators.required, Validators.min(0)]],
        date: [new Date().toISOString().split('T')[0], [Validators.required]],
        notes: [''],
    });

    assessmentTypes = ['PHQ-9', 'GAD-7', 'DASS-21', 'Other'];

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.patientId.set(id);
            this.loadAssessments();
        } else {
            this.error.set('Patient ID required');
        }
    }

    loadAssessments() {
        this.loading.set(true);
        this.patientService.getAssessments(this.patientId()).subscribe({
            next: (data) => {
                this.assessments.set(data);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to load assessments');
                this.loading.set(false);
            },
        });
    }

    onSubmit() {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.patientService.addAssessment(this.patientId(), this.form.value).subscribe({
            next: () => {
                this.loading.set(false);
                this.form.reset({ date: new Date().toISOString().split('T')[0] });
                this.loadAssessments();
            },
            error: (err) => {
                this.error.set(err.message || 'Failed to add assessment');
                this.loading.set(false);
            },
        });
    }
}
```
#### Step 2: Create Template

```html
<!-- assessment.component.html -->
<div class="assessment">
    <h2>Psychological Assessments</h2>

    <div *ngIf="loading()" class="text-center py-3">
        <app-spinner size="md"></app-spinner>
    </div>

    <div *ngIf="error()" class="alert alert-danger">{{ error() }}</div>

    <div class="card mb-4">
        <div class="card-header">Add New Assessment</div>
        <div class="card-body">
            <form [formGroup]="form" (ngSubmit)="onSubmit()">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Type</label>
                            <select class="form-select" formControlName="type">
                                <option *ngFor="let type of assessmentTypes" [value]="type">{{ type }}</option>
                            </select>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <app-input
                            label="Score"
                            type="number"
                            placeholder="Enter score"
                            formControlName="score"
                        ></app-input>
                    </div>
                    <div class="col-md-4">
                        <app-input
                            label="Date"
                            type="date"
                            formControlName="date"
                        ></app-input>
                    </div>
                </div>
                <div class="form-group">
                    <label>Notes</label>
                    <textarea class="form-control" formControlName="notes" rows="2"></textarea>
                </div>
                <div class="mt-3">
                    <app-button type="submit" variant="primary" [loading]="loading()">Add Assessment</app-button>
                </div>
            </form>
        </div>
    </div>

    <h4>Assessment History</h4>
    <div *ngIf="assessments().length === 0">No assessments recorded.</div>
    <table class="table table-striped" *ngIf="assessments().length > 0">
        <thead>
            <tr>
                <th>Type</th>
                <th>Score</th>
                <th>Date</th>
                <th>Notes</th>
            </tr>
        </thead>
        <tbody>
            <tr *ngFor="let assessment of assessments()">
                <td>{{ assessment.type }}</td>
                <td>{{ assessment.score }}</td>
                <td>{{ assessment.date | date:'shortDate' }}</td>
                <td>{{ assessment.notes || '-' }}</td>
            </tr>
        </tbody>
    </table>
</div>
```
### Files To Create

*   `assessment.component.ts`, .html, .scss

### CLI Commands

\# Already generated

### Concepts Required

*   Reactive Forms
*   Date handling

### Testing Steps

1.  Add assessment and verify it appears in the list.
2.  Test validation.

### Expected Deliverables

*   Assessment component ready.

### Definition of Done

*   Assessment recording and listing works.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Assessments are tracked per patient.

## FE-PAT-007State Management and Integration P1 Medium 4h ▾

### Task Information

*   **Task ID:** FE-PAT-007
*   **Task Name:** State Management and Integration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous patient tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Ensure all patient components use the PatientStateService consistently, add caching and error handling, and integrate with other modules (sessions, exercises, reports).

### Business Purpose

A unified state management approach reduces bugs and ensures data consistency across the application.

### Technical Purpose

Update all components to use the state service, add methods for clearing state, and integrate with other feature services.

### Prerequisites

*   All patient components implemented.

### Dependencies

*   All FE-PAT tasks.

### Inputs

*   Existing components.

### Outputs

*   Updated components using state service consistently.
*   Integration with sessions, exercises, reports (placeholders).

### Detailed Workflow

#### Step 1: Refactor Components to Use State Service

Ensure PatientListComponent, PatientDetailComponent, etc., use `PatientStateService` for data and loading states.

#### Step 2: Add Clear State on Navigation

When navigating away from patient detail, clear selected patient.

// In patient-detail.component.ts ngOnDestroy() { this.state.clearSelected(); }

#### Step 3: Integrate with Other Modules

In PatientDetailComponent tabs, we have placeholders for `app-session-list`, `app-exercise-list`, etc. Ensure these child components receive the patient ID via input and fetch data accordingly.

#### Step 4: Add Error Handling to All API Calls

Ensure every API call in PatientService has error handling and updates the state error signal.

#### Step 5: Add Unit Tests for State Service

Test that state updates correctly.

### Files To Modify

*   All patient components.
*   PatientStateService (add clearSelected).

### CLI Commands

No new commands.

### Concepts Required

*   State management best practices
*   Lifecycle hooks (ngOnDestroy)

### Testing Steps

1.  Verify state persists correctly between components.
2.  Test that navigation clears selected patient.

### Expected Deliverables

*   Patient management fully integrated.

### Common Mistakes

**Mistake 1:** Not clearing state on component destroy, causing stale data.

### Definition of Done

*   All components use state service and integrate with other modules.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use PatientStateService for all patient data.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 7 patient tasks are complete (FE-PAT-001 to FE-PAT-007).
*   Patient routes are lazy‑loaded and guarded.
*   PatientService and PatientStateService are implemented.
*   Patient List works with search, pagination, archive/restore.
*   Patient Detail displays information and tabs.
*   Patient Form works for create and edit.
*   Intake Form supports image upload and OCR.
*   Assessment component records and lists assessments.
*   All components use the state service.
*   All tests pass.
*   All changes are committed.

📋 Code Review Checklist
------------------------

*   PatientService methods are typed and handle errors.
*   State service uses Signals correctly.
*   List component uses debounced search and pagination.
*   Detail component uses tabs and integrates child components.
*   Form uses reactive validation.
*   Intake form handles file upload and OCR.
*   Assessment component uses date picker correctly.

🚀 Pull Request Checklist
-------------------------

*   Branch up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   Documentation updated if needed.

🧪 Deployment Readiness Checklist
---------------------------------

*   Patient CRUD works end‑to‑end.
*   Intake form with OCR works.
*   Assessments are saved and displayed.
*   All pages accessible with correct permissions.

* * *

Jalsa – Phase 7: Patient Management – Expanded Implementation Handbook • v1.0 • For development team

