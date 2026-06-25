import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { authGuard } from './auth.guard';

describe('authGuard', () => {
    let authService: AuthService;

    const mockRouter = {
        navigate: vi.fn(),
        url: '/patients',
    };

    const mockRoute = {} as ActivatedRouteSnapshot;
    const mockState = { url: '/patients' } as RouterStateSnapshot;

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

    it('should return true when user is authenticated', () => {
        vi.spyOn(authService, 'isAuthenticated').mockReturnValue(true);
        const result = TestBed.runInInjectionContext(() => authGuard(mockRoute, mockState));
        expect(result).toBe(true);
    });

    it('should return false and redirect to login when not authenticated', () => {
        vi.spyOn(authService, 'isAuthenticated').mockReturnValue(false);
        const result = TestBed.runInInjectionContext(() => authGuard(mockRoute, mockState));
        expect(result).toBe(false);
        expect(mockRouter.navigate).toHaveBeenCalledWith(['/auth/login'], {
            queryParams: { returnUrl: '/patients' },
        });
    });
});
