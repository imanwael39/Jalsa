import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const APPOINTMENTS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Patient'])],
        data: { breadcrumb: 'مواعيدي' },
        children: [
            {
                path: '',
                pathMatch: 'full',
                loadComponent: () =>
                    import('./pages/appointment-list/appointment-list.component').then(m => m.AppointmentListComponent),
            },
            {
                path: ':id',
                data: { breadcrumb: 'تفاصيل الموعد' },
                loadComponent: () =>
                    import('./pages/appointment-detail/appointment-detail.component').then(
                        m => m.AppointmentDetailComponent
                    ),
            },
        ],
    },
];
