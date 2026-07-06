import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const PATIENTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist'])],
        data: { breadcrumb: 'المرضى' },
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/patient-list/patient-list').then(m => m.PatientList),
            },
            {
                path: 'new',
                data: { breadcrumb: 'مريض جديد' },
                loadComponent: () => import('./pages/patient-form/patient-form').then(m => m.PatientForm),
            },
            {
                path: ':id',
                data: { breadcrumb: 'ملف المريض' },
                loadComponent: () => import('./pages/patient-detail/patient-detail').then(m => m.PatientDetail),
            },
            {
                path: ':id/edit',
                data: { breadcrumb: 'تعديل المريض' },
                loadComponent: () => import('./pages/patient-form/patient-form').then(m => m.PatientForm),
            },
            {
                path: ':id/intake',
                data: { breadcrumb: 'استمارة الاستقبال' },
                loadComponent: () => import('./pages/intake-form/intake-form').then(m => m.IntakeForm),
            },
            {
                path: ':id/assessments',
                data: { breadcrumb: 'التقييمات' },
                loadComponent: () => import('./pages/assessment/assessment').then(m => m.Assessment),
            },
        ],
    },
];
