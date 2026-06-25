import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { LoadingService } from '../services/loading.service';
import { loadingInterceptor } from './loading.interceptor';

describe('loadingInterceptor', () => {
    let http: HttpClient;
    let httpMock: HttpTestingController;
    let loadingService: LoadingService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provideHttpClient(withInterceptors([loadingInterceptor])),
                provideHttpClientTesting(),
                LoadingService,
            ],
        });
        http = TestBed.inject(HttpClient);
        httpMock = TestBed.inject(HttpTestingController);
        loadingService = TestBed.inject(LoadingService);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('should show loading during request and hide after', () => {
        const showSpy = vi.spyOn(loadingService, 'show');
        const hideSpy = vi.spyOn(loadingService, 'hide');

        http.get('/api/data').subscribe();

        expect(showSpy).toHaveBeenCalled();

        const req = httpMock.expectOne('/api/data');
        req.flush({});

        expect(hideSpy).toHaveBeenCalled();
    });

    it('should skip loading for refresh token requests', () => {
        const showSpy = vi.spyOn(loadingService, 'show');

        http.get('/auth/refresh').subscribe();

        expect(showSpy).not.toHaveBeenCalled();

        const req = httpMock.expectOne('/auth/refresh');
        req.flush({});
    });
});
