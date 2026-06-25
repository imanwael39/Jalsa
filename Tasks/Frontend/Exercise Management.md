# 🏋️ Jalsa – Phase 9: Exercise Management Expanded Implementation Handbook · 5 Tasks · 30+ Subtasks

## 🏋️ Phase 9 – Exercise Management

**Purpose:** Build the complete exercise management feature: therapist view of all exercises, assignment of exercises to patients, and patient view for tracking progress and providing reflections. This feature enables therapists to prescribe exercises and patients to update their progress.

📋 Tasks: 5 ⏱️ Total Effort: ~24 hours 👤 Owners: M4 (Frontend Developer), M3 (Frontend Lead) 🔗 Dependencies: Phase 7 (Patient Management), Phase 4 (Shared Components), Phase 6 (Layout System) 🎯 Deliverable: Complete exercise management with assignment, status tracking, and patient reflection

## FE-EXE-001Exercise Module Setup (Routes, Service, State) P1 High 5h ▾

### Task Information

- **Task ID:** FE-EXE-001
- **Task Name:** Exercise Module Setup (Routes, Service, State)
- **Owner:** M4 (Frontend Developer)
- **Dependencies:** FE-CORE-001 (HttpClientService), FE-CORE-002 (Models), FE-AUTH-002 (AuthService), FE-LAYOUT-001 (Main layout)
- **Complexity:** High
- **Estimated Effort:** 5 hours
- **Priority:** High

### Objective

Create the exercise feature with lazy‑loaded routes, an ExerciseService for API communication, and an ExerciseStateService for managing exercise and assignment state using Signals.

### Business Purpose

Therapists need to assign exercises to patients; patients need to view and update their progress. A well‑structured setup ensures scalability.

### Technical Purpose

Set up lazy‑loaded routes under the main layout, create a service that uses HttpClientService for exercise and assignment endpoints, and create a state service with Signals for reactive data management.

### Prerequisites

- Understanding of Angular lazy loading with Standalone components.
- Knowledge of Signals and services.
- API contract for exercise endpoints.

### Dependencies

- **FE-CORE-001:** HttpClientService for API calls.
- **FE-CORE-002:** Exercise model interfaces.
- **FE-AUTH-002:** AuthService for guards.
- **FE-LAYOUT-001:** Main layout as parent route.

### Inputs

- API endpoints from `api-endpoints.ts`.

### Outputs

- `features/exercises/exercises.routes.ts` – Lazy‑loaded routes.
- `core/services/exercise.service.ts` – Exercise API service.
- `core/state/exercise-state.service.ts` – Exercise state management with Signals.
- Placeholder components (list, assign, patient-view).

### Detailed Workflow

#### Step 1: Create Routes File

```typescript
// features/exercises/exercises.routes.ts
import { Routes } from "@angular/router";
import { authGuard, roleGuard } from "../../core/guards";

export const EXERCISES_ROUTES: Routes = [
  {
    path: "",
    canActivate: [authGuard],
    children: [
      {
        path: "",
        canActivate: [roleGuard(["Therapist", "Admin"])],
        loadComponent: () =>
          import("./pages/exercise-list/exercise-list.component").then(
            (m) => m.ExerciseListComponent,
          ),
      },
      {
        path: "assign",
        canActivate: [roleGuard(["Therapist", "Admin"])],
        loadComponent: () =>
          import("./pages/assign-exercise/assign-exercise.component").then(
            (m) => m.AssignExerciseComponent,
          ),
      },
      {
        path: "my-exercises",
        canActivate: [roleGuard(["Patient"])],
        loadComponent: () =>
          import("./pages/patient-exercise/patient-exercise.component").then(
            (m) => m.PatientExerciseComponent,
          ),
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
    path: 'exercises',
    loadChildren: () => import('./features/exercises/exercises.routes').then(m => m.EXERCISES_ROUTES),
},
```

#### Step 3: Create Exercise Service

```ts
// core/services/exercise.service.ts
import { Injectable, inject } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClientService } from "../api/http-client.service";
import { API } from "../api/api-endpoints";
import {
  Exercise,
  ExerciseAssignment,
  AssignExerciseRequest,
  UpdateExerciseStatusRequest,
} from "../models";

@Injectable({
  providedIn: "root",
})
export class ExerciseService {
  private http = inject(HttpClientService);

  getExercises(): Observable<Exercise[]> {
    return this.http.get<Exercise[]>(API.exercises.base);
  }

  getPatientExercises(patientId: string): Observable<ExerciseAssignment[]> {
    return this.http.get<ExerciseAssignment[]>(
      API.exercises.byPatient(patientId),
    );
  }

  assignExercise(data: AssignExerciseRequest): Observable<ExerciseAssignment> {
    return this.http.post<ExerciseAssignment>(API.exercises.assign, data);
  }

  updateStatus(
    id: string,
    data: UpdateExerciseStatusRequest,
  ): Observable<void> {
    return this.http.put<void>(API.exercises.status(id), data);
  }
}
```

#### Step 4: Create Exercise State Service

```ts
// core/state/exercise-state.service.ts
import { Injectable, signal } from "@angular/core";
import { Exercise, ExerciseAssignment } from "../models";

@Injectable({
  providedIn: "root",
})
export class ExerciseStateService {
  private exercisesSignal = signal<Exercise[]>([]);
  private assignmentsSignal = signal<ExerciseAssignment[]>([]);
  private loadingSignal = signal(false);
  private errorSignal = signal<string | null>(null);

  readonly exercises = this.exercisesSignal.asReadonly();
  readonly assignments = this.assignmentsSignal.asReadonly();
  readonly loading = this.loadingSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();

  setExercises(exercises: Exercise[]) {
    this.exercisesSignal.set(exercises);
    this.errorSignal.set(null);
  }

  setAssignments(assignments: ExerciseAssignment[]) {
    this.assignmentsSignal.set(assignments);
    this.errorSignal.set(null);
  }

  addAssignment(assignment: ExerciseAssignment) {
    this.assignmentsSignal.update((list) => [...list, assignment]);
  }

  updateAssignmentStatus(id: string, status: string, reflection?: string) {
    this.assignmentsSignal.update((list) =>
      list.map((a) =>
        a.id === id
          ? {
              ...a,
              status: status as any,
              reflection: reflection || a.reflection,
            }
          : a,
      ),
    );
  }

  setLoading(loading: boolean) {
    this.loadingSignal.set(loading);
  }

  setError(error: string | null) {
    this.errorSignal.set(error);
  }

  reset() {
    this.exercisesSignal.set([]);
    this.assignmentsSignal.set([]);
    this.loadingSignal.set(false);
    this.errorSignal.set(null);
  }
}
```

#### Step 5: Generate Placeholder Components

```bash
ng g c features/exercises/pages/exercise-list --standalone --skip-tests
ng g c features/exercises/pages/assign-exercise --standalone --skip-tests
ng g c features/exercises/pages/patient-exercise --standalone --skip-tests
```

### Files To Create

- `features/exercises/exercises.routes.ts`
- `core/services/exercise.service.ts`
- `core/state/exercise-state.service.ts`
- Placeholder components.

### CLI Commands

```bash
ng g c features/exercises/pages/exercise-list --standalone --skip-tests
ng g c features/exercises/pages/assign-exercise --standalone --skip-tests
ng g c features/exercises/pages/patient-exercise --standalone --skip-tests
```

### Code Flow

```
User navigates to /exercises → ExerciseListComponent loads → Calls ExerciseService.getExercises() → Updates ExerciseStateService.exercises → Renders list
```

### Concepts Required

- Lazy loading with loadChildren
- Standalone components
- Angular Signals for state

### Tools/Libraries Required

- Angular Router

### Testing Steps

1.  Verify routes are lazy‑loaded (network tab).
2.  Test ExerciseService methods with mock data.
3.  Test ExerciseStateService signals update correctly.

### Expected Deliverables

- Exercise routes, service, and state service ready.

### Common Mistakes

**Mistake 1:** Not applying appropriate guards for therapist vs patient routes.  
**Fix:** Use `roleGuard` with the correct roles.

### Edge Cases

- Patient without therapist role should not see assignment routes.

### Definition of Done

- Routes, service, and state service are implemented.

### Frontend Architecture Notes

- ExerciseStateService is the single source of truth for exercise data.

### Team Handoff Notes

- **To:** All developers.
- **Key Takeaways:** Use ExerciseService for API and ExerciseStateService for state.

## FE-EXE-002Exercise List (Therapist) P1 Medium 4h ▾

### Task Information

- **Task ID:** FE-EXE-002
- **Task Name:** Exercise List (Therapist)
- **Owner:** M4 (Frontend Developer)
- **Dependencies:** FE-EXE-001, FE-SHARED-005 (Table), FE-SHARED-002 (Button)
- **Complexity:** Medium
- **Estimated Effort:** 4 hours
- **Priority:** High

### Objective

Build the exercise list page for therapists, displaying all exercises with the ability to assign exercises to patients.

### Business Purpose

Therapists need to see available exercises and assign them to patients.

### Technical Purpose

Use shared Table component, integrate with ExerciseService and ExerciseStateService, and provide navigation to the assign form.

### Prerequisites

- Shared Table and Button components.

### Dependencies

- **FE-EXE-001:** ExerciseService and StateService.
- **FE-SHARED-005:** Table component.

### Inputs

- None.

### Outputs

- `exercise-list.component.ts`
- `exercise-list.component.html`
- `exercise-list.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// exercise-list.component.ts
import { Component, inject, OnInit } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { ExerciseService } from "../../../../core/services/exercise.service";
import { ExerciseStateService } from "../../../../core/state/exercise-state.service";
import { TableComponent } from "../../../../shared/components/table/table.component";
import { ButtonComponent } from "../../../../shared/components/button/button.component";

@Component({
  selector: "app-exercise-list",
  standalone: true,
  imports: [TableComponent, ButtonComponent, RouterLink],
  templateUrl: "./exercise-list.component.html",
  styleUrls: ["./exercise-list.component.scss"],
})
export class ExerciseListComponent implements OnInit {
  private exerciseService = inject(ExerciseService);
  private state = inject(ExerciseStateService);

  exercises = this.state.exercises;
  loading = this.state.loading;
  error = this.state.error;

  ngOnInit(): void {
    this.loadExercises();
  }

  loadExercises() {
    this.state.setLoading(true);
    this.exerciseService.getExercises().subscribe({
      next: (data) => {
        this.state.setExercises(data);
        this.state.setLoading(false);
      },
      error: (err) => {
        this.state.setError(err.message || "Failed to load exercises");
        this.state.setLoading(false);
      },
    });
  }
}
```

#### Step 2: Create Template

```html
<!-- exercise-list.component.html -->
<div class="exercise-list">
  <div class="d-flex justify-content-between align-items-center mb-3">
    <h2>Exercises</h2>
    <app-button variant="primary" routerLink="/exercises/assign"
      >Assign Exercise</app-button
    >
  </div>

  <app-table
    [data]="exercises()"
    [columns]="[
            { key: 'name', label: 'Exercise Name' },
            { key: 'category', label: 'Category' },
            { key: 'description', label: 'Description' }
        ]"
    [loading]="loading()"
    emptyMessage="No exercises found"
  >
    <ng-template #cell let-row let-col="col">
      <ng-container *if="col.key === 'description'">
        {{ row.description | truncate:60 }}
      </ng-container>
    </ng-template>
    <ng-template #actions let-row>
      <app-button
        variant="primary"
        size="sm"
        [routerLink]="['/exercises/assign', { exerciseId: row.id }]"
        >Assign</app-button
      >
    </ng-template>
  </app-table>
</div>
```

#### Step 3: Add Styles

```css
// exercise-list.component.scss
 .exercise-list {
  padding: 1rem 0;
}
```

### Files To Create

- `exercise-list.component.ts`
- `exercise-list.component.html`
- `exercise-list.component.scss`

### CLI Commands

\# Already generated

### Concepts Required

- Shared Table component
- Async data fetching

### Testing Steps

1.  Navigate to /exercises.
2.  Verify exercises load.
3.  Test assign button navigation.

### Expected Deliverables

- Exercise list ready.

### Common Mistakes

**Mistake 1:** Not using truncate pipe for description, causing layout issues.

### Edge Cases

- No exercises → empty state.

### Definition of Done

- Exercise list displays and allows navigation to assign.

### Team Handoff Notes

- **To:** All developers.
- **Key Takeaways:** This is the starting point for therapists.

## FE-EXE-003Assign Exercise Form P1 Medium 4h ▾

### Task Information

- **Task ID:** FE-EXE-003
- **Task Name:** Assign Exercise Form
- **Owner:** M4 (Frontend Developer)
- **Dependencies:** FE-EXE-001, FE-PAT-001 (PatientService for patient list), FE-SHARED-003 (Input), FE-SHARED-002 (Button)
- **Complexity:** Medium
- **Estimated Effort:** 4 hours
- **Priority:** High

### Objective

Build a form for therapists to assign an exercise to a patient, with selection of patient, exercise, due date, and optional notes.

### Business Purpose

Enable therapists to create exercise assignments for patients.

### Technical Purpose

Use reactive forms with dropdowns for patients and exercises, submit to ExerciseService, and update state.

### Prerequisites

- Reactive Forms.
- PatientService to fetch patient list.
- ExerciseService to fetch exercise list.

### Dependencies

- **FE-EXE-001:** ExerciseService and StateService.
- **FE-PAT-001:** PatientService for patients.

### Inputs

- Optional pre‑selected exercise ID via query param.

### Outputs

- `assign-exercise.component.ts`
- `assign-exercise.component.html`
- `assign-exercise.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// assign-exercise.component.ts
import { Component, inject, signal, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { ExerciseService } from "../../../../core/services/exercise.service";
import { PatientService } from "../../../../core/services/patient.service";
import { ExerciseStateService } from "../../../../core/state/exercise-state.service";
import { InputComponent } from "../../../../shared/components/input/input.component";
import { ButtonComponent } from "../../../../shared/components/button/button.component";

@Component({
  selector: "app-assign-exercise",
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent, ButtonComponent],
  templateUrl: "./assign-exercise.component.html",
  styleUrls: ["./assign-exercise.component.scss"],
})
export class AssignExerciseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private exerciseService = inject(ExerciseService);
  private patientService = inject(PatientService);
  private state = inject(ExerciseStateService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  patients = signal<any[]>([]);
  exercises = signal<any[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    patientId: ["", [Validators.required]],
    exerciseId: ["", [Validators.required]],
    dueDate: ["", [Validators.required]],
    notes: [""],
  });

  ngOnInit(): void {
    this.loadData();
    const exerciseId = this.route.snapshot.queryParams["exerciseId"];
    if (exerciseId) {
      this.form.patchValue({ exerciseId });
    }
  }

  loadData() {
    this.loading.set(true);
    // Load patients
    this.patientService.getPatients({ page: 1, size: 100 }).subscribe({
      next: (result) => {
        this.patients.set(result.items);
      },
      error: (err) => {
        this.error.set("Failed to load patients");
      },
    });
    // Load exercises
    this.exerciseService.getExercises().subscribe({
      next: (data) => {
        this.exercises.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set("Failed to load exercises");
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
    this.exerciseService.assignExercise(this.form.value).subscribe({
      next: (assignment) => {
        this.state.addAssignment(assignment);
        this.loading.set(false);
        this.router.navigate(["/exercises"]);
      },
      error: (err) => {
        this.error.set(err.message || "Failed to assign exercise");
        this.loading.set(false);
      },
    });
  }
}
```

#### Step 2: Create Template

```html
// assign-exercise.component.html
<div class="assign-exercise">
  <h2>Assign Exercise to Patient</h2>
  <div \*if\="loading()" class="text-center py-5">
    <app-spinner size="lg"></app-spinner>
  </div>
  <div \*if\="error()" class="alert alert-danger">{{ error() }}</div>
  <form \[formGroup\]="form" (ngSubmit)="onSubmit()" \*if\="!loading()">
    <div class="form-group">
      <label>Patient</label>
      <select class="form-select" formControlName="patientId">
        <option value="">Select Patient</option>
        <option \*for\="let patient of patients()" \[value\]="patient.id">
          {{ patient.firstName }} {{ patient.lastName }}
        </option>
      </select>
    </div>
    <div class="form-group">
      <label>Exercise</label>
      <select class="form-select" formControlName="exerciseId">
        <option value="">Select Exercise</option>
        <option \*for\="let exercise of exercises()" \[value\]="exercise.id">
          {{ exercise.name }}
        </option>
      </select>
    </div>
    <app-input
      label="Due Date"
      type="date"
      formControlName="dueDate"
      \[error\]="form.get('dueDate')?.invalid && form.get('dueDate')?.touched ? 'Due date is required' : ''"
    ></app-input>
    <div class="form-group">
      <label>Notes (optional)</label>
      <textarea
        class="form-control"
        formControlName="notes"
        rows="2"
      ></textarea>
    </div>
    <div class="mt-4">
      <app-button type="submit" variant="primary" \[loading\]="loading()"
        >Assign Exercise</app-button
      >
      <app-button
        type="button"
        variant="secondary"
        class="ms-2"
        (click)="router.navigate(\['/exercises'\])"
        >Cancel</app-button
      >
    </div>
  </form>
</div>
```

#### Step 3: Add Styles

```css
// assign-exercise.component.scss
.assign-exercise {
  padding: 1rem 0;
  max-width: 600px;
}
```

### Files To Create

- `assign-exercise.component.ts`
- `assign-exercise.component.html`
- `assign-exercise.component.scss`

### CLI Commands

\# Already generated

### Concepts Required

- Reactive Forms with dropdowns
- Combined async data loading

### Testing Steps

1.  Load assign form, verify patients and exercises appear.
2.  Submit with valid data, verify assignment created.
3.  Test pre‑selected exercise via query param.

### Expected Deliverables

- Assignment form ready.

### Common Mistakes

**Mistake 1:** Not handling empty patient/exercise lists gracefully.

### Edge Cases

- Patient list empty → show message.
- Exercise list empty → show message.

### Definition of Done

- Form works and assigns exercises.

### Team Handoff Notes

- **To:** All developers.
- **Key Takeaways:** Assignment links patient and exercise.

## FE-EXE-004Patient Exercise View P1 Medium 5h ▾

### Task Information

- **Task ID:** FE-EXE-004
- **Task Name:** Patient Exercise View
- **Owner:** M4 (Frontend Developer)
- **Dependencies:** FE-EXE-001, FE-AUTH-002 (AuthService for patient ID), FE-SHARED-003 (Input)
- **Complexity:** Medium
- **Estimated Effort:** 5 hours
- **Priority:** High

### Objective

Build the patient view for their assigned exercises, allowing them to update the status (Pending, InProgress, Completed) and add reflection notes.

### Business Purpose

Patients can track their exercises and provide feedback to therapists.

### Technical Purpose

Fetch exercises for the current patient, display them in a list, and allow status updates and reflection editing.

### Prerequisites

- AuthService to get current user and patient ID.
- ExerciseService for patient exercises and updates.

### Dependencies

- **FE-EXE-001:** ExerciseService and StateService.
- **FE-AUTH-002:** AuthService for user ID.

### Inputs

- Patient ID from AuthService (or from route).

### Outputs

- `patient-exercise.component.ts`
- `patient-exercise.component.html`
- `patient-exercise.component.scss`

### Detailed Workflow

#### Step 1: Implement Component

```ts
// patient-exercise.component.ts
import { Component, inject, signal, OnInit } from "@angular/core";
import { ExerciseService } from "../../../../core/services/exercise.service";
import { ExerciseStateService } from "../../../../core/state/exercise-state.service";
import { AuthService } from "../../../../core/services/auth.service";
import { ButtonComponent } from "../../../../shared/components/button/button.component";
import { InputComponent } from "../../../../shared/components/input/input.component";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-patient-exercise",
  standalone: true,
  imports: [ButtonComponent, InputComponent, FormsModule],
  templateUrl: "./patient-exercise.component.html",
  styleUrls: ["./patient-exercise.component.scss"],
})
export class PatientExerciseComponent implements OnInit {
  private exerciseService = inject(ExerciseService);
  private state = inject(ExerciseStateService);
  private authService = inject(AuthService);

  assignments = this.state.assignments;
  loading = this.state.loading;
  error = this.state.error;
  patientId = signal<string>("");

  updating = signal<{ [key: string]: boolean }>({});

  ngOnInit(): void {
    const user = this.authService.currentUser();
    if (user) {
      this.patientId.set(user.id);
      this.loadAssignments();
    } else {
      this.error.set("Patient not logged in");
    }
  }

  loadAssignments() {
    this.state.setLoading(true);
    this.exerciseService.getPatientExercises(this.patientId()).subscribe({
      next: (data) => {
        this.state.setAssignments(data);
        this.state.setLoading(false);
      },
      error: (err) => {
        this.state.setError(err.message || "Failed to load exercises");
        this.state.setLoading(false);
      },
    });
  }

  updateStatus(assignmentId: string, status: string, reflection?: string) {
    this.updating.set({ ...this.updating(), [assignmentId]: true });
    this.exerciseService
      .updateStatus(assignmentId, { status, reflection })
      .subscribe({
        next: () => {
          this.state.updateAssignmentStatus(assignmentId, status, reflection);
          this.updating.set({ ...this.updating(), [assignmentId]: false });
        },
        error: (err) => {
          console.error("Update failed", err);
          this.updating.set({ ...this.updating(), [assignmentId]: false });
        },
      });
  }
}
```

#### Step 2: Create Template

```html
// patient-exercise.component.html
<div class="patient-exercise">
  <h2>My Exercises</h2>
  <div \*if\="loading()" class="text-center py-5">
    <app-spinner size="lg"></app-spinner>
  </div>
  <div \*if\="error()" class="alert alert-danger">{{ error() }}</div>
  <div \*if\="assignments().length === 0 && !loading()">
    <div class="alert alert-info">No exercises assigned yet.</div>
  </div>
  <div \*for\="let assignment of assignments()" class="card mb-3">
    <div class="card-body">
      <h5 class="card-title">{{ assignment.exercise.name }}</h5>
      <p class="card-text">{{ assignment.exercise.description }}</p>
      <p><strong>Due:</strong> {{ assignment.dueDate | date:'shortDate' }}</p>
      <p>
        <strong>Status:</strong>
        <span
          class="badge"
          \[class.bg-secondary\]="assignment.status === 'Pending'"
          \[class.bg-info\]="assignment.status === 'InProgress'"
          \[class.bg-success\]="assignment.status === 'Completed'"
        >
          {{ assignment.status }}
        </span>
      </p>
      <div class="mt-2">
        <label>Update Status</label>
        <select
          class="form-select form-select-sm"
          \[ngModel\]="assignment.status"
          (ngModelChange)="updateStatus(assignment.id, $event, assignment.reflection)"
        >
          <option value="Pending">Pending</option>
          <option value="InProgress">In Progress</option>
          <option value="Completed">Completed</option>
        </select>
      </div>
      <div class="mt-2">
        <label>Reflection</label>
        <textarea
          class="form-control form-control-sm"
          rows="2"
          \[ngModel\]="assignment.reflection"
          (ngModelChange)="updateStatus(assignment.id, assignment.status, $event)"
          placeholder="Add your reflection..."
        ></textarea>
      </div>
      <div \*if\="updating()\[assignment.id\]" class="mt-2 text-muted">
        <small>Saving...</small>
      </div>
    </div>
  </div>
</div>
```

#### Step 3: Add Styles

// patient-exercise.component.scss .patient-exercise { padding: 1rem 0; }

### Files To Create

- `patient-exercise.component.ts`
- `patient-exercise.component.html`
- `patient-exercise.component.scss`

### CLI Commands

\# Already generated

### Concepts Required

- Two‑way binding with `ngModel`
- Dynamic state updates

### Testing Steps

1.  Login as a patient, navigate to /exercises/my-exercises.
2.  Verify assignments load.
3.  Change status and reflection, verify update.

### Expected Deliverables

- Patient exercise view ready.

### Common Mistakes

**Mistake 1:** Not handling debouncing of reflection updates (saving on every keystroke). Consider using a save button or debounce.

### Edge Cases

- No assignments → empty state.
- Large reflection text → handle appropriately.

### Definition of Done

- Patient can view and update exercises.

### Team Handoff Notes

- **To:** All developers.
- **Key Takeaways:** Patient view is the primary patient interface for exercises.

## FE-EXE-005State Management and Integration P1 Medium 4h ▾

### Task Information

- **Task ID:** FE-EXE-005
- **Task Name:** State Management and Integration
- **Owner:** M4 (Frontend Developer)
- **Dependencies:** All previous exercise tasks
- **Complexity:** Medium
- **Estimated Effort:** 4 hours
- **Priority:** High

### Objective

Ensure all exercise components use the ExerciseStateService consistently, add caching and error handling, and integrate with the patient module (display exercises in patient detail).

### Business Purpose

Unified state management reduces bugs and ensures data consistency across the exercise feature.

### Technical Purpose

Refactor components to use the state service, add methods for clearing state, and integrate with PatientDetailComponent.

### Prerequisites

- All exercise components implemented.

### Dependencies

- All FE-EXE tasks.

### Inputs

- Existing components.

### Outputs

- Updated components using state service consistently.
- Integration with PatientDetailComponent.

### Detailed Workflow

#### Step 1: Refactor Components to Use State Service

Ensure ExerciseListComponent, AssignExerciseComponent, and PatientExerciseComponent use `ExerciseStateService` for data and loading states.

#### Step 2: Add Clear State on Navigation

When navigating away, clear assignments if needed.

```ts
// In patient-exercise.component.ts
ngOnDestroy() {
    // Optionally reset assignments if you want to avoid stale data
    // this.state.setAssignments([]);
}
```
#### Step 3: Integrate with Patient Detail

In PatientDetailComponent, add a tab for exercises using `<app-exercise-list [patientId]="patient()?.id"></app-exercise-list>` but note that the exercise list for a specific patient should show assignments, not all exercises. We'll create a separate component or reuse PatientExerciseComponent with input patientId. We'll adjust: the PatientExerciseComponent currently uses the logged‑in patient; we can add an `@Input() patientId` to make it reusable. Modify it:

```ts
// patient-exercise.component.ts
@Input() patientIdOverride?: string;
// In ngOnInit, use patientIdOverride if provided, else from auth.
ngOnInit(): void {
    this.patientId.set(this.patientIdOverride || this.authService.currentUser()?.id || '');
    if (this.patientId()) this.loadAssignments();
}
```
Then in PatientDetailComponent, add:
'''html
<app-patient-exercise [patientIdOverride]="patient()?.id"></app-patient-exercise>
'''

#### Step 4: Add Error Handling

Ensure every API call in ExerciseService has error handling and updates the state error signal.

#### Step 5: Add Unit Tests for State Service

Test that state updates correctly.

### Files To Modify

- All exercise components.
- PatientDetailComponent (add exercise tab).
- PatientExerciseComponent (add @Input).

### CLI Commands

No new commands.

### Concepts Required

- State management best practices
- Component reusability with inputs

### Testing Steps

1.  Verify state persists correctly between components.
2.  Test that PatientDetail shows exercise assignments.

### Expected Deliverables

- Exercise management fully integrated.

### Common Mistakes

**Mistake 1:** Not handling different patient IDs in the patient exercise view.

### Definition of Done

- All components use state service and integrate with patient module.

### Team Handoff Notes

- **To:** All developers.
- **Key Takeaways:** ExerciseStateService is the source of truth for exercise data.

---

## ✅ Phase Completion Criteria

- All 5 exercise tasks are complete (FE-EXE-001 to FE-EXE-005).
- Exercise routes are lazy‑loaded and guarded.
- ExerciseService and ExerciseStateService are implemented.
- Exercise List shows all exercises and allows navigation to assign.
- Assign Exercise Form works and creates assignments.
- Patient Exercise View displays assignments and allows status/reflection updates.
- All components use the state service.
- Integration with PatientDetailComponent works.
- All tests pass.
- All changes are committed.

## 📋 Code Review Checklist

- ExerciseService methods are typed and handle errors.
- State service uses Signals correctly.
- Assign form validates required fields.
- Patient view updates status and reflection correctly.
- Integration with patient detail uses Input binding.

## 🚀 Pull Request Checklist

- Branch up‑to‑date with main.
- All tests pass.
- Code review completed.
- Documentation updated.

## 🧪 Deployment Readiness Checklist

- Exercise assignment works for therapists.
- Patient can view and update exercises.
- Integration with patient detail works.

---

Jalsa – Phase 9: Exercise Management – Expanded Implementation Handbook • v1.0 • For development team
