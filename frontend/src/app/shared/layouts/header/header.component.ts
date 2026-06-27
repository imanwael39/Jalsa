import { Component, Output, EventEmitter, inject, ChangeDetectionStrategy, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
    selector: 'app-header',
    standalone: true,
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
    @Output() toggleSidebar = new EventEmitter<void>();

    private authService = inject(AuthService);
    private router = inject(Router);

    user = this.authService.currentUser;
    isDropdownOpen = false;
    searchQuery = signal('');

    toggleDropdown(): void {
        this.isDropdownOpen = !this.isDropdownOpen;
    }

    closeDropdown(): void {
        this.isDropdownOpen = false;
    }

    logout(): void {
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
