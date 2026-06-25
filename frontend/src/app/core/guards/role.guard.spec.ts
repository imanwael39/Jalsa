import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { roleGuard } from './role.guard';

describe('roleGuard', () => {
    let authService: AuthService;

    const mockRouter = {
        navigate: vi.fn(),
    };

    const mockRoute = {} as ActivatedRouteSnapshot;
    const mockState = {} as RouterStateSnapshot;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                AuthService,
                { provide: Router, useValue: mockRouter },
            ],
        });
        authService = TestBed.inject(AuthService);
        vi.clearAllMocks();
    });

    it('should return true when user has an allowed role', () => {
        vi.spyOn(authService, 'hasRole').mockImplementation(role => role === 'Therapist');
        const guard = roleGuard(['Therapist']);
        const result = TestBed.runInInjectionContext(() => guard(mockRoute, mockState));
        expect(result).toBe(true);
    });

    it('should return true when user has one of multiple allowed roles', () => {
        vi.spyOn(authService, 'hasRole').mockImplementation(role => role === 'Admin');
        const guard = roleGuard(['Therapist', 'Admin']);
        const result = TestBed.runInInjectionContext(() => guard(mockRoute, mockState));
        expect(result).toBe(true);
    });

    it('should return false and redirect when user has no allowed role', () => {
        vi.spyOn(authService, 'hasRole').mockReturnValue(false);
        const guard = roleGuard(['Therapist']);
        const result = TestBed.runInInjectionContext(() => guard(mockRoute, mockState));
        expect(result).toBe(false);
        expect(mockRouter.navigate).toHaveBeenCalledWith(['/forbidden']);
    });
});
