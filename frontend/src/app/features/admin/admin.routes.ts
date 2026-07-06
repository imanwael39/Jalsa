import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const ADMIN_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Admin'])],
        data: { breadcrumb: 'إدارة النظام' },
        children: [
            {
                path: '',
                pathMatch: 'full',
                loadComponent: () => import('./pages/user-list/user-list.component').then(m => m.UserListComponent),
            },
        ],
    },
];
