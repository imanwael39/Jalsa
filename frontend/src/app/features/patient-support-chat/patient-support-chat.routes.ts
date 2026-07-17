import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const PATIENT_SUPPORT_CHAT_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Patient'])],
        data: { breadcrumb: 'الدعم النفسي' },
        loadComponent: () =>
            import('./pages/patient-support-chat-room/patient-support-chat-room.component').then(
                m => m.PatientSupportChatRoomComponent
            ),
    },
];
