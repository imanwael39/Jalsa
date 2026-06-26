import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './shared/layouts/main-layout/main-layout.component';

export const routes: Routes = [
    // Auth routes (outside main layout)
    {
        path: 'auth',
        loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
    },
    // Protected routes (inside main layout with auth guard)
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: 'dashboard',
                loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
            },
            {
                path: 'patients',
                loadChildren: () => import('./features/patients/patients.routes').then(m => m.PATIENTS_ROUTES),
            },
            {
                path: 'sessions',
                loadChildren: () => import('./features/sessions/sessions.routes').then(m => m.SESSIONS_ROUTES),
            },
            {
                path: 'exercises',
                loadChildren: () => import('./features/exercises/exercises.routes').then(m => m.EXERCISES_ROUTES),
            },
            {
                path: 'reports',
                loadChildren: () => import('./features/reports/reports.routes').then(m => m.REPORTS_ROUTES),
            },
            {
                path: 'chatbot',
                loadChildren: () => import('./features/chatbot/chatbot.routes').then(m => m.CHATBOT_ROUTES),
            },
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full',
            },
        ],
    },
    {
        path: '**',
        redirectTo: 'auth/login',
    },
];
