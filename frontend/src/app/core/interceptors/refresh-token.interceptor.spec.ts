import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from '../services/auth.service';
import { refreshTokenInterceptor } from './refresh-token.interceptor';
import { of, throwError } from 'rxjs';

describe('refreshTokenInterceptor', () => {
    let http: HttpClient;
    let httpMock: HttpTestingController;
    let authService: AuthService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(withInterceptors([refreshTokenInterceptor])), provideHttpClientTesting()],
        });
        http = TestBed.inject(HttpClient);
        httpMock = TestBed.inject(HttpTestingController);
        authService = TestBed.inject(AuthService);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('should refresh the token and retry the original request on 401', () => {
        vi.spyOn(authService, 'refreshToken').mockReturnValue(
            of({
                token: 'new-token',
                expiresAt: '',
                refreshToken: 'new-refresh',
                refreshTokenExpiresAt: '',
                email: 'a@a.com',
                roles: ['Therapist'],
            })
        );

        let result: unknown;
        http.get('/api/data').subscribe(res => (result = res));

        const firstReq = httpMock.expectOne('/api/data');
        firstReq.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });

        const retriedReq = httpMock.expectOne('/api/data');
        expect(retriedReq.request.headers.get('Authorization')).toBe('Bearer new-token');
        retriedReq.flush({ ok: true });

        expect(result).toEqual({ ok: true });
    });

    it('should logout and propagate the error when refresh fails', () => {
        vi.spyOn(authService, 'refreshToken').mockReturnValue(throwError(() => new Error('refresh failed')));
        const logoutSpy = vi.spyOn(authService, 'logout').mockImplementation(() => {});

        let caught: unknown;
        http.get('/api/data').subscribe({ error: err => (caught = err) });

        const req = httpMock.expectOne('/api/data');
        req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });

        expect(logoutSpy).toHaveBeenCalled();
        expect(caught).toBeInstanceOf(Error);
    });

    it('should not attempt refresh for the login endpoint', () => {
        const refreshSpy = vi.spyOn(authService, 'refreshToken');

        http.get('/api/auth/login').subscribe({ error: () => {} });

        const req = httpMock.expectOne('/api/auth/login');
        req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });

        expect(refreshSpy).not.toHaveBeenCalled();
    });

    it('should pass through non-401 errors untouched', () => {
        let caught: unknown;
        http.get('/api/data').subscribe({ error: err => (caught = err) });

        const req = httpMock.expectOne('/api/data');
        req.flush('Server Error', { status: 500, statusText: 'Server Error' });

        expect((caught as { status: number }).status).toBe(500);
    });
});
