import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';

export const CHATBOT_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/chat-list/chat-list.component').then(m => m.ChatListComponent),
    },
    {
        path: ':id',
        canActivate: [authGuard],
        loadComponent: () => import('./pages/chat-room/chat-room.component').then(m => m.ChatRoomComponent),
    },
];
