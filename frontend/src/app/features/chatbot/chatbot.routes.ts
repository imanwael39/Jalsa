import { Routes } from '@angular/router';

export const CHATBOT_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/chat-list/chat-list.component').then(m => m.ChatListComponent),
    },
    {
        path: ':id',
        loadComponent: () => import('./pages/chat-room/chat-room.component').then(m => m.ChatRoomComponent),
    },
];
