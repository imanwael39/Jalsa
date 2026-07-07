import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const DASHBOARD_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist'])],
        data: { breadcrumb: 'لوحة التحكم' },
        loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
    },
    {
        path: 'patient',
        canActivate: [authGuard, roleGuard(['Patient'])],
        data: { breadcrumb: 'لوحة التحكم' },
        loadComponent: () =>
            import('./pages/patient-dashboard/patient-dashboard.component').then(m => m.PatientDashboardComponent),
    },
];
