import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { AuthLayoutComponent } from '../../shared/layouts/auth-layout/auth-layout.component';

export const AUTH_ROUTES: Routes = [
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
            {
                path: 'login',
                loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent),
            },
            {
                path: 'register',
                loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent),
            },
            {
                path: 'forgot-password',
                loadComponent: () =>
                    import('./pages/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent),
            },
            {
                path: 'reset-password',
                loadComponent: () =>
                    import('./pages/reset-password/reset-password.component').then(m => m.ResetPasswordComponent),
            },
            {
                path: '',
                redirectTo: 'login',
                pathMatch: 'full',
            },
        ],
    },
    {
        path: 'profile',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/profile/profile.component').then(m => m.ProfileComponent),
    },
];
