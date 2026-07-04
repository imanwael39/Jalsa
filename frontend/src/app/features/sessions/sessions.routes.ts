import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const SESSIONS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Admin'])],
        data: { breadcrumb: 'الجلسات' },
        children: [
            {
                path: '',
                pathMatch: 'full',
                loadComponent: () => import('./pages/session-landing/session-landing').then(m => m.SessionLanding),
            },
            {
                path: 'patient/:patientId',
                data: { breadcrumb: 'جلسات المريض' },
                loadComponent: () => import('./pages/session-list/session-list').then(m => m.SessionList),
            },
            {
                path: 'new/:patientId',
                data: { breadcrumb: 'جلسة جديدة' },
                loadComponent: () => import('./pages/session-form/session-form').then(m => m.SessionForm),
            },
            {
                path: ':id',
                data: { breadcrumb: 'تفاصيل الجلسة' },
                loadComponent: () => import('./pages/session-detail/session-detail').then(m => m.SessionDetail),
            },
            {
                path: ':id/edit',
                data: { breadcrumb: 'تعديل الجلسة' },
                loadComponent: () => import('./pages/session-form/session-form').then(m => m.SessionForm),
            },
        ],
    },
];
