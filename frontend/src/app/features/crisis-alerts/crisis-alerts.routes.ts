import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const CRISIS_ALERTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist'])],
        data: { breadcrumb: 'تنبيهات الأزمات' },
        children: [
            {
                path: '',
                pathMatch: 'full',
                loadComponent: () =>
                    import('./pages/crisis-alerts-list/crisis-alerts-list.component').then(
                        m => m.CrisisAlertsListComponent
                    ),
            },
        ],
    },
];
