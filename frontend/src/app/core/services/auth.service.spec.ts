import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
    let service: AuthService;
    let httpMock: HttpTestingController;

    const profileResponse = {
        id: 'user-1',
        email: 't@t.com',
        firstName: 'Test',
        lastName: 'User',
        roles: ['Therapist'],
        isActive: true,
        lastLoginAt: null,
        createdAt: '2026-01-01T00:00:00Z',
        updatedAt: '2026-01-01T00:00:00Z',
    };

    beforeEach(() => {
        localStorage.clear();
        TestBed.configureTestingModule({
            providers: [provideHttpClient(), provideHttpClientTesting()],
        });
        service = TestBed.inject(AuthService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        httpMock.verify();
        localStorage.clear();
    });

    it('should resolve immediately when no token is stored', async () => {
        await service.initializeAuth();
        expect(service.initialized()).toBe(true);
        expect(service.currentUser()).toBeNull();
    });

    it('should hydrate the full profile (including roles) from the backend on init when a token exists', async () => {
        localStorage.setItem('jalsa_token', 'stored-token');

        const initPromise = service.initializeAuth();
        const req = httpMock.expectOne('http://localhost:5014/api/auth/profile');
        req.flush(profileResponse);
        await initPromise;

        expect(service.initialized()).toBe(true);
        expect(service.currentUser()).toEqual(profileResponse);
        expect(service.hasRole('Therapist')).toBe(true);
    });

    it('should log out when the stored token is rejected by the profile endpoint', async () => {
        localStorage.setItem('jalsa_token', 'stale-token');
        localStorage.setItem('jalsa_refresh_token', 'stale-refresh');

        const initPromise = service.initializeAuth();
        const req = httpMock.expectOne('http://localhost:5014/api/auth/profile');
        req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
        await initPromise;

        expect(service.currentUser()).toBeNull();
        expect(localStorage.getItem('jalsa_token')).toBeNull();
    });

    it('should hydrate full user details (roles/name) after login, not just the login response', () => {
        let result: unknown;
        service.login({ email: 't@t.com', password: 'secret' }).subscribe(res => (result = res));

        const loginReq = httpMock.expectOne('http://localhost:5014/api/auth/login');
        loginReq.flush({
            token: 'new-token',
            expiresAt: '2026-01-01T01:00:00Z',
            refreshToken: 'refresh-token',
            refreshTokenExpiresAt: '2026-01-08T00:00:00Z',
            email: 't@t.com',
            roles: ['Therapist'],
        });

        const profileReq = httpMock.expectOne('http://localhost:5014/api/auth/profile');
        profileReq.flush(profileResponse);

        expect(service.currentUser()).toEqual(profileResponse);
        expect(result).toBeTruthy();
    });
});
