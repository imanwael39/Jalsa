import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards';

export const NOTIFICATIONS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard],
        data: { breadcrumb: 'الإشعارات' },
        loadComponent: () =>
            import('./pages/notification-list/notification-list.component').then(m => m.NotificationListComponent),
    },
];
