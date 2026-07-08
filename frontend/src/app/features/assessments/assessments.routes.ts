import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const ASSESSMENTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Patient'])],
        data: { breadcrumb: 'تقييماتي' },
        loadComponent: () =>
            import('./pages/assessment-list/assessment-list.component').then(m => m.AssessmentListComponent),
    },
    {
        path: ':id',
        canActivate: [authGuard, roleGuard(['Patient'])],
        data: { breadcrumb: 'التقييم' },
        loadComponent: () =>
            import('./pages/assessment-detail/assessment-detail.component').then(m => m.AssessmentDetailComponent),
    },
];
