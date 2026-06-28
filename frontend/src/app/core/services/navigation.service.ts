import { Injectable, inject, computed } from '@angular/core';
import { AuthService } from './auth.service';

export interface NavItem {
    label: string;
    icon: string;
    route: string;
    roles?: string[];
    children?: NavItem[];
}

@Injectable({
    providedIn: 'root',
})
export class NavigationService {
    private readonly authService = inject(AuthService);

    private readonly allItems: NavItem[] = [
        { label: 'لوحة التحكم', icon: 'bi-grid', route: '/dashboard' },
        { label: 'المرضى', icon: 'bi-people', route: '/patients', roles: ['Therapist', 'Admin'] },
        { label: 'الجلسات', icon: 'bi-calendar', route: '/sessions', roles: ['Therapist', 'Admin'] },
        { label: 'التمارين', icon: 'bi-clipboard', route: '/exercises', roles: ['Therapist', 'Admin'] },
        { label: 'التقارير', icon: 'bi-file-text', route: '/reports', roles: ['Therapist', 'Admin'] },
        { label: 'المحادثات', icon: 'bi-chat-dots', route: '/chatbot', roles: ['Therapist', 'Admin', 'Patient'] },
    ];

    readonly menuItems = computed(() => {
        return this.allItems.filter(item => {
            if (!item.roles) return true;
            return this.authService.hasAnyRole(item.roles);
        });
    });
}
