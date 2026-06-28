import {
    Component,
    Output,
    EventEmitter,
    inject,
    ChangeDetectionStrategy,
    signal,
    OnInit,
    OnDestroy,
} from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { InAppNotificationService } from '../../../core/services/in-app-notification.service';
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

    user = this.authService.currentUser;
    isDropdownOpen = false;
    isNotifOpen = false;
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
        this.isDropdownOpen = !this.isDropdownOpen;
        if (this.isDropdownOpen) this.isNotifOpen = false;
    }

    toggleNotif(): void {
        this.isNotifOpen = !this.isNotifOpen;
        if (this.isNotifOpen) this.isDropdownOpen = false;
    }

    closeDropdown(): void {
        this.isDropdownOpen = false;
    }

    logout(): void {
        this.notifService.stopPolling();
        this.authService.logout();
        this.router.navigate(['/auth/login']);
        this.isDropdownOpen = false;
    }

    goToProfile(): void {
        this.router.navigate(['/auth/profile']);
        this.isDropdownOpen = false;
    }

    onSearch(value: string): void {
        this.searchQuery.set(value);
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
