import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { NavigationService } from './navigation.service';

describe('NavigationService', () => {
    let authService: AuthService;
    let service: NavigationService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [AuthService],
        });
        authService = TestBed.inject(AuthService);
        service = TestBed.inject(NavigationService);
    });

    function mockRoles(roles: string[]): void {
        vi.spyOn(authService, 'hasAnyRole').mockImplementation(allowed => allowed.some(r => roles.includes(r)));
    }

    it('shows only Admin-relevant items for an Admin user, never clinical items', () => {
        mockRoles(['Admin']);

        const routes = service.menuItems().map(item => item.route);

        expect(routes).toContain('/admin/dashboard');
        expect(routes).toContain('/admin');
        expect(routes).not.toContain('/dashboard');
        expect(routes).not.toContain('/sessions');
        expect(routes).not.toContain('/exercises');
        expect(routes).not.toContain('/reports');
        expect(routes).not.toContain('/therapist-chat');
        expect(routes).not.toContain('/support-chat');
        expect(routes).not.toContain('/patients');
        expect(routes).not.toContain('/crisis-alerts');
    });

    it('shows the clinical dashboard and clinical items for a Therapist, never the admin panel or patient support chat', () => {
        mockRoles(['Therapist']);

        const routes = service.menuItems().map(item => item.route);

        expect(routes).toContain('/dashboard');
        expect(routes).toContain('/sessions');
        expect(routes).toContain('/exercises');
        expect(routes).toContain('/reports');
        expect(routes).toContain('/patients');
        expect(routes).toContain('/therapist-chat');
        expect(routes).toContain('/crisis-alerts');
        expect(routes).not.toContain('/support-chat');
        expect(routes).not.toContain('/admin/dashboard');
        expect(routes).not.toContain('/admin');
    });

    it('shows only the support chat for a Patient, no dashboard, no admin/therapist items, and never the therapist chat', () => {
        mockRoles(['Patient']);

        const routes = service.menuItems().map(item => item.route);

        expect(routes).toContain('/support-chat');
        expect(routes).not.toContain('/therapist-chat');
        expect(routes).not.toContain('/dashboard');
        expect(routes).not.toContain('/admin/dashboard');
        expect(routes).not.toContain('/sessions');
        expect(routes).not.toContain('/patients');
    });

    it('never shows both Dashboard entries at once for a single role', () => {
        mockRoles(['Admin']);
        const dashboardEntries = service.menuItems().filter(item => item.label === 'لوحة التحكم');
        expect(dashboardEntries).toHaveLength(1);
        expect(dashboardEntries[0].route).toBe('/admin/dashboard');
    });
});
