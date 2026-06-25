import { Component, Input, Output, EventEmitter, inject, ChangeDetectionStrategy } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { AppStateService } from '../../../core/services/app-state.service';

export interface NavItem {
    label: string;
    icon: string;
    route: string;
    roles?: string[];
}

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [RouterLink, RouterLinkActive],
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
    private authService = inject(AuthService);
    private appState = inject(AppStateService);

    @Input() collapsed = false;
    @Output() toggle = new EventEmitter<void>();

    private readonly allMenuItems: NavItem[] = [
        { label: 'لوحة التحكم', icon: 'bi-grid', route: '/dashboard' },
        { label: 'المرضى', icon: 'bi-people', route: '/patients', roles: ['Therapist', 'Admin'] },
        { label: 'الجلسات', icon: 'bi-calendar', route: '/sessions', roles: ['Therapist', 'Admin'] },
        { label: 'التمارين', icon: 'bi-clipboard', route: '/exercises', roles: ['Therapist', 'Admin'] },
        { label: 'التقارير', icon: 'bi-file-text', route: '/reports', roles: ['Therapist', 'Admin'] },
        { label: 'المحادثة', icon: 'bi-chat', route: '/chatbot', roles: ['Patient'] },
    ];

    get filteredMenu(): NavItem[] {
        return this.allMenuItems.filter(item => {
            if (!item.roles) return true;
            return this.authService.hasAnyRole(item.roles);
        });
    }

    toggleSidebar(): void {
        this.toggle.emit();
    }
}
