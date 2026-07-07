import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const CHATBOT_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Patient'])],
        data: { breadcrumb: 'المحادثات' },
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/chat-list/chat-list.component').then(m => m.ChatListComponent),
            },
            {
                path: ':id',
                data: { breadcrumb: 'المحادثة' },
                loadComponent: () => import('./pages/chat-room/chat-room.component').then(m => m.ChatRoomComponent),
            },
        ],
    },
];
