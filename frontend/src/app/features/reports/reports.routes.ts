import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const REPORTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist'])],
        data: { breadcrumb: 'التقارير' },
        children: [
            {
                path: '',
                pathMatch: 'full',
                loadComponent: () => import('./pages/report-landing/report-landing').then(m => m.ReportLanding),
            },
            {
                path: 'patient/:patientId',
                data: { breadcrumb: 'تقارير المريض' },
                loadComponent: () => import('./pages/report-list/report-list').then(m => m.ReportList),
            },
            {
                path: 'generate/:patientId',
                data: { breadcrumb: 'إنشاء تقرير' },
                loadComponent: () => import('./pages/report-generate/report-generate').then(m => m.ReportGenerate),
            },
            {
                path: ':id',
                data: { breadcrumb: 'تفاصيل التقرير' },
                loadComponent: () => import('./pages/report-detail/report-detail').then(m => m.ReportDetail),
            },
        ],
    },
];
