import { Component, Output, EventEmitter, inject, ChangeDetectionStrategy, input, computed } from '@angular/core';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { NavigationService } from '../../../core/services/navigation.service';
import { AppStateService } from '../../../core/services/app-state.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [RouterLink, RouterLinkActive],
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
    private readonly navService = inject(NavigationService);
    private readonly appState = inject(AppStateService);
    private readonly authService = inject(AuthService);
    private readonly router = inject(Router);

    readonly collapsed = input(false);
    @Output() toggle = new EventEmitter<void>();

    readonly menuItems = this.navService.menuItems;
    readonly user = this.authService.currentUser;
    readonly avatarUrl = computed(() => this.authService.resolveAvatarUrl(this.user()?.profileImageUrl));

    toggleSidebar(): void {
        this.toggle.emit();
    }

    logout(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
    }

    goToProfile(): void {
        this.router.navigate(['/auth/profile']);
    }

    getRoleLabel(): string {
        const user = this.user();
        if (!user?.roles || user.roles.length === 0) return '';
        const role = user.roles[0];
        if (role === 'Admin') return 'مدير النظام';
        if (role === 'Therapist') return 'معالج';
        if (role === 'Patient') return 'مريض';
        return role;
    }
}
