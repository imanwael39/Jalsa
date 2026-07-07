import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const ADMIN_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Admin'])],
        data: { breadcrumb: 'إدارة النظام' },
        children: [
            { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
            {
                path: 'dashboard',
                data: { breadcrumb: 'لوحة تحكم النظام' },
                loadComponent: () =>
                    import('./pages/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent),
            },
            {
                path: 'users',
                data: { breadcrumb: 'إدارة المستخدمين' },
                loadComponent: () => import('./pages/user-list/user-list.component').then(m => m.UserListComponent),
            },
            {
                path: 'doctors',
                data: { breadcrumb: 'الأطباء المعالجون' },
                loadComponent: () =>
                    import('./pages/doctor-list/doctor-list.component').then(m => m.DoctorListComponent),
            },
            {
                path: 'doctors/:id',
                data: { breadcrumb: 'تفاصيل المعالج' },
                loadComponent: () =>
                    import('./pages/doctor-detail/doctor-detail.component').then(m => m.DoctorDetailComponent),
            },
            {
                path: 'patients',
                data: { breadcrumb: 'حسابات المرضى' },
                loadComponent: () =>
                    import('./pages/patient-account-list/patient-account-list.component').then(
                        m => m.PatientAccountListComponent
                    ),
            },
            {
                path: 'roles',
                data: { breadcrumb: 'الأدوار' },
                loadComponent: () => import('./pages/roles/roles.component').then(m => m.RolesComponent),
            },
            {
                path: 'settings',
                data: { breadcrumb: 'إعدادات النظام' },
                loadComponent: () => import('./pages/settings/settings.component').then(m => m.SettingsComponent),
            },
            {
                path: 'activity-logs',
                data: { breadcrumb: 'سجل النشاط' },
                loadComponent: () =>
                    import('./pages/activity-logs/activity-logs.component').then(m => m.ActivityLogsComponent),
            },
        ],
    },
];
