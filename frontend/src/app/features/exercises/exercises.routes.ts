import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

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
