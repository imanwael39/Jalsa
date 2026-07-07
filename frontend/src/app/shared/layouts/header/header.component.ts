import {
    Component,
    Output,
    EventEmitter,
    inject,
    ChangeDetectionStrategy,
    signal,
    computed,
    OnInit,
    OnDestroy,
} from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { InAppNotificationService, InAppNotification } from '../../../core/services/in-app-notification.service';
import { AppStateService } from '../../../core/services/app-state.service';
import { ClickOutsideDirective } from '../../directives/click-outside/click-outside.directive';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [ClickOutsideDirective],
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent implements OnInit, OnDestroy {
    @Output() toggleSidebar = new EventEmitter<void>();

    private authService = inject(AuthService);
    private router = inject(Router);
    readonly notifService = inject(InAppNotificationService);
    private readonly appState = inject(AppStateService);

    user = this.authService.currentUser;
    avatarUrl = computed(() => this.authService.resolveAvatarUrl(this.user()?.profileImageUrl));
    theme = this.appState.theme;
    isDropdownOpen = signal(false);
    isNotifOpen = signal(false);
    searchQuery = signal('');

    ngOnInit(): void {
        if (this.authService.isAuthenticated()) {
            this.notifService.startPolling();
        }
    }

    ngOnDestroy(): void {
        this.notifService.stopPolling();
    }

    toggleDropdown(): void {
        this.isDropdownOpen.update(v => !v);
        if (this.isDropdownOpen()) this.isNotifOpen.set(false);
    }

    closeDropdown(): void {
        this.isDropdownOpen.set(false);
    }

    toggleNotif(): void {
        this.isNotifOpen.update(v => !v);
        if (this.isNotifOpen()) this.isDropdownOpen.set(false);
    }

    closeNotif(): void {
        this.isNotifOpen.set(false);
    }

    onNotificationClick(notification: InAppNotification): void {
        this.notifService.markRead(notification.id);
        if (notification.type === 'CrisisAlert') {
            this.isNotifOpen.set(false);
            this.router.navigate(['/crisis-alerts']);
        }
    }

    logout(): void {
        this.notifService.stopPolling();
        this.authService.logout();
        this.router.navigate(['/auth/login']);
        this.isDropdownOpen.set(false);
    }

    goToProfile(): void {
        this.router.navigate(['/auth/profile']);
        this.isDropdownOpen.set(false);
    }

    onSearch(value: string): void {
        this.searchQuery.set(value);
    }

    onSearchSubmit(): void {
        const query = this.searchQuery().trim();
        if (!query) return;

        this.router.navigate(['/patients'], { queryParams: { search: query } });
    }

    canSearchPatients(): boolean {
        return this.authService.hasRole('Therapist');
    }

    toggleTheme(): void {
        this.appState.toggleTheme();
    }

    getRoleLabel(): string {
        const u = this.user();
        if (!u?.roles || u.roles.length === 0) return '';
        const role = u.roles[0];
        if (role === 'Admin') return 'مدير النظام';
        if (role === 'Therapist') return 'معالج';
        if (role === 'Patient') return 'مريض';
        return role;
    }
}
