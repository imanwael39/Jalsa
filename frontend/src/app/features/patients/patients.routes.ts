import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const PATIENTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Admin'])],
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/patient-list/patient-list').then(m => m.PatientList),
            },
            {
                path: 'new',
                loadComponent: () => import('./pages/patient-form/patient-form').then(m => m.PatientForm),
            },
            {
                path: ':id',
                loadComponent: () => import('./pages/patient-detail/patient-detail').then(m => m.PatientDetail),
            },
            {
                path: ':id/edit',
                loadComponent: () => import('./pages/patient-form/patient-form').then(m => m.PatientForm),
            },
            {
                path: ':id/intake',
                loadComponent: () => import('./pages/intake-form/intake-form').then(m => m.IntakeForm),
            },
            {
                path: ':id/assessments',
                loadComponent: () => import('./pages/assessment/assessment').then(m => m.Assessment),
            },
        ],
    },
];
