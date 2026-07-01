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

    it('should handle 401 on a non-login request as session expiry', () => {
        const logoutSpy = vi.spyOn(authService, 'logout');
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(logoutSpy).toHaveBeenCalled();
                expect(mockRouter.navigate).toHaveBeenCalledWith(['/auth/login']);
                expect(errorSpy).toHaveBeenCalledWith('انتهت صلاحية الجلسة، يرجى تسجيل الدخول مرة أخرى');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
    });

    it('should handle 401 on the login request as invalid credentials', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/auth/login').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('البريد الإلكتروني أو كلمة المرور غير صحيحة');
            },
        });

        const req = httpMock.expectOne('/api/auth/login');
        req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
    });

    it('should handle 429 with a rate limit message', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('عدد كبير جدًا من الطلبات، يرجى المحاولة لاحقًا');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Too Many Requests', { status: 429, statusText: 'Too Many Requests' });
    });

    it('should handle 403 with permission error', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('ليس لديك صلاحية للقيام بهذه العملية');
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
                expect(errorSpy).toHaveBeenCalledWith('حدث خطأ غير متوقع، يرجى المحاولة مرة أخرى');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Server Error', { status: 500, statusText: 'Server Error' });
    });

    it('should handle network error (status 0)', () => {
        const errorSpy = vi.spyOn(notificationService, 'error');

        http.get('/api/data').subscribe({
            error: () => {
                expect(errorSpy).toHaveBeenCalledWith('تعذر الاتصال بالخادم، يرجى التحقق من اتصالك بالإنترنت');
            },
        });

        const req = httpMock.expectOne('/api/data');
        req.flush('Network error', { status: 0, statusText: 'Unknown Error' });
    });
});
