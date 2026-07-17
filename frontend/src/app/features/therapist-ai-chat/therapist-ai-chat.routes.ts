import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const THERAPIST_AI_CHAT_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist'])],
        data: { breadcrumb: 'المساعد السريري الذكي' },
        children: [
            {
                path: '',
                loadComponent: () =>
                    import('./pages/therapist-chat-list/therapist-chat-list.component').then(
                        m => m.TherapistChatListComponent
                    ),
            },
            {
                path: ':id',
                data: { breadcrumb: 'المحادثة' },
                loadComponent: () =>
                    import('./pages/therapist-chat-room/therapist-chat-room.component').then(
                        m => m.TherapistChatRoomComponent
                    ),
            },
        ],
    },
];
