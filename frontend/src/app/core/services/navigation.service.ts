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
        { label: 'لوحة التحكم', icon: 'bi-grid', route: '/dashboard', roles: ['Therapist'] },
        { label: 'لوحة التحكم', icon: 'bi-grid', route: '/admin/dashboard', roles: ['Admin'] },
        { label: 'المرضى', icon: 'bi-people', route: '/patients', roles: ['Therapist'] },
        { label: 'الجلسات', icon: 'bi-calendar', route: '/sessions', roles: ['Therapist'] },
        { label: 'التمارين', icon: 'bi-clipboard', route: '/exercises', roles: ['Therapist'] },
        { label: 'التقارير', icon: 'bi-file-text', route: '/reports', roles: ['Therapist'] },
        { label: 'المحادثات', icon: 'bi-chat-dots', route: '/chatbot', roles: ['Therapist', 'Patient'] },
        { label: 'تقدمي', icon: 'bi-graph-up-arrow', route: '/progress', roles: ['Patient'] },
        {
            label: 'تنبيهات الأزمات',
            icon: 'bi-exclamation-triangle-fill',
            route: '/crisis-alerts',
            roles: ['Therapist'],
        },
        { label: 'إدارة النظام', icon: 'bi-gear-fill', route: '/admin', roles: ['Admin'] },
    ];

    readonly menuItems = computed(() => {
        return this.allItems.filter(item => {
            if (!item.roles) return true;
            return this.authService.hasAnyRole(item.roles);
        });
    });
}
