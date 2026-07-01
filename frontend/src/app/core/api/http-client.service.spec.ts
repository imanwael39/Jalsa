import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClientService } from './http-client.service';
import { environment } from '../../../environments/environment';

describe('HttpClientService', () => {
    let service: HttpClientService;
    let httpMock: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [HttpClientService],
        });
        service = TestBed.inject(HttpClientService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('GET', () => {
        it('should perform a GET request with the correct URL', () => {
            const endpoint = '/patients';
            const mockData = [{ id: '1', name: 'Test' }];

            service.get<{ id: string; name: string }[]>(endpoint).subscribe(data => {
                expect(data).toEqual(mockData);
            });

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('GET');
            req.flush(mockData);
        });

        it('should append query params when provided', () => {
            const endpoint = '/patients';
            const params = { page: '1', limit: '10' };

            service.get(endpoint, params).subscribe();

            const req = httpMock.expectOne(
                r =>
                    r.url === `${environment.apiUrl}${endpoint}` &&
                    r.params.get('page') === '1' &&
                    r.params.get('limit') === '10'
            );
            expect(req.request.method).toBe('GET');
            req.flush([]);
        });
    });

    describe('POST', () => {
        it('should perform a POST request with the correct URL and body', () => {
            const endpoint = '/auth/login';
            const body = { email: 'test@example.com', password: 'password' };
            const mockResponse = { token: 'abc123' };

            service.post<{ token: string }>(endpoint, body).subscribe(data => {
                expect(data).toEqual(mockResponse);
            });

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('POST');
            expect(req.request.body).toEqual(body);
            req.flush(mockResponse);
        });

        it('should accept null body', () => {
            const endpoint = '/auth/logout';

            service.post(endpoint, null).subscribe();

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('POST');
            expect(req.request.body).toBeNull();
            req.flush({});
        });
    });

    describe('PUT', () => {
        it('should perform a PUT request with the correct URL and body', () => {
            const endpoint = `/patients/1`;
            const body = { name: 'Updated' };

            service.put(endpoint, body).subscribe();

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('PUT');
            expect(req.request.body).toEqual(body);
            req.flush({});
        });
    });

    describe('PATCH', () => {
        it('should perform a PATCH request with the correct URL and body', () => {
            const endpoint = `/patients/1`;
            const body = { name: 'Patched' };

            service.patch(endpoint, body).subscribe();

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('PATCH');
            expect(req.request.body).toEqual(body);
            req.flush({});
        });
    });

    describe('DELETE', () => {
        it('should perform a DELETE request with the correct URL', () => {
            const endpoint = `/patients/1`;

            service.delete(endpoint).subscribe();

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('DELETE');
            req.flush({});
        });
    });

    describe('UPLOAD', () => {
        it('should perform a POST request with FormData', () => {
            const endpoint = '/patients/1/intake/image';
            const formData = new FormData();
            formData.append('file', new Blob(['test']), 'test.png');

            service.upload(endpoint, formData).subscribe();

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            expect(req.request.method).toBe('POST');
            expect(req.request.body).toBe(formData);
            req.flush({});
        });
    });

    describe('Error Handling', () => {
        it('should throw an error on failed request', () => {
            const endpoint = '/patients';
            const mockError = { status: 500, statusText: 'Server Error' };

            service.get(endpoint).subscribe({
                error: error => {
                    expect(error.status).toBe(500);
                },
            });

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            req.flush('Server Error', mockError);
        });

        it('should propagate error without logging to console', () => {
            const consoleSpy = vi.spyOn(console, 'error');
            const endpoint = '/patients';

            service.get(endpoint).subscribe({
                error: error => {
                    expect(error.status).toBe(404);
                    expect(consoleSpy).not.toHaveBeenCalled();
                },
            });

            const req = httpMock.expectOne(`${environment.apiUrl}${endpoint}`);
            req.flush('Not Found', { status: 404, statusText: 'Not Found' });
        });
    });
});
