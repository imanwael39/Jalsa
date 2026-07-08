import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const PROGRESS_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Patient'])],
        data: { breadcrumb: 'تقدمي' },
        loadComponent: () =>
            import('./pages/patient-progress/patient-progress.component').then(m => m.PatientProgressComponent),
    },
];
