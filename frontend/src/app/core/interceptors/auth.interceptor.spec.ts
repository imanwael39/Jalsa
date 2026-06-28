import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService } from '../services/auth.service';
import { authInterceptor } from './auth.interceptor';

describe('authInterceptor', () => {
    let http: HttpClient;
    let httpMock: HttpTestingController;
    let authService: AuthService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideHttpClient(withInterceptors([authInterceptor])), provideHttpClientTesting()],
        });
        http = TestBed.inject(HttpClient);
        httpMock = TestBed.inject(HttpTestingController);
        authService = TestBed.inject(AuthService);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('should add Authorization header when token exists', () => {
        vi.spyOn(authService, 'getToken').mockReturnValue('test-token-123');
        http.get('/api/data').subscribe();

        const req = httpMock.expectOne('/api/data');
        expect(req.request.headers.has('Authorization')).toBe(true);
        expect(req.request.headers.get('Authorization')).toBe('Bearer test-token-123');
        req.flush({});
    });

    it('should not add Authorization header when no token exists', () => {
        vi.spyOn(authService, 'getToken').mockReturnValue(null);
        http.get('/api/data').subscribe();

        const req = httpMock.expectOne('/api/data');
        expect(req.request.headers.has('Authorization')).toBe(false);
        req.flush({});
    });

    it('should skip auth for refresh token endpoint', () => {
        vi.spyOn(authService, 'getToken').mockReturnValue('test-token-123');
        http.get('/api/auth/refresh').subscribe();

        const req = httpMock.expectOne('/api/auth/refresh');
        expect(req.request.headers.has('Authorization')).toBe(false);
        req.flush({});
    });
});
