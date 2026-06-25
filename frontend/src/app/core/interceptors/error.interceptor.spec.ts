import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { errorInterceptor } from './error.interceptor';

describe('errorInterceptor', () => {
    let http: HttpClient;
    let httpMock: HttpTestingController;
    let authService: AuthService;
    let notificationService: NotificationService;

    const mockRouter = { navigate: vi.fn() };

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provideHttpClient(withInterceptors([errorInterceptor])),
                provideHttpClientTesting(),
                AuthService,
                NotificationService,
                { provide: Router, useValue: mockRouter },
            ],
        });
        http = TestBed.inject(HttpClient);
        httpMock = TestBed.inject(HttpTestingController);
        authService = TestBed.inject(AuthService);
        notificationService = TestBed.inject(NotificationService);
        localStorage.clear();
        vi.clearAllMocks();
    });

    afterEach(() => {
        httpMock.verify();
        localStorage.clear();
    });

    it('should handle 401 by logging out and redirecting', () => {
        const logoutSpy = vi.spyOn(authService, 'logout');
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(logoutSpy).toHaveBeenCalled();
                expect(mockRouter.navigate).toHaveBeenCalledWith(['/auth/login']);
                expect(errorSpy).toHaveBeenCalledWith('Your session has expired. Please log in again.');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
    });

    it('should handle 403 with permission error', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('You do not have permission to perform this action.');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Forbidden', { status: 403, statusText: 'Forbidden' });
    });

    it('should handle 400 with server message when available', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('Validation failed');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush({ message: 'Validation failed' }, { status: 400, statusText: 'Bad Request' });
    });

    it('should handle 500 with generic server error', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('A server error occurred. Please try again later.');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Server Error', { status: 500, statusText: 'Server Error' });
    });

    it('should handle network error (status 0)', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('Network error. Please check your connection.');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Network error', { status: 0, statusText: 'Unknown Error' });
    });
});
