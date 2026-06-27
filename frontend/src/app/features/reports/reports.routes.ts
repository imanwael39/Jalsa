import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const REPORTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Admin'])],
        children: [
            {
                path: 'patient/:patientId',
                loadComponent: () => import('./pages/report-list/report-list').then(m => m.ReportList),
            },
            {
                path: 'generate/:patientId',
                loadComponent: () => import('./pages/report-generate/report-generate').then(m => m.ReportGenerate),
            },
            {
                path: ':id',
                loadComponent: () => import('./pages/report-detail/report-detail').then(m => m.ReportDetail),
            },
        ],
    },
];
