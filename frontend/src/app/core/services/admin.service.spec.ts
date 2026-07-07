import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AdminService } from './admin.service';
import { API } from '../api/api-endpoints';
import { environment } from '../../../environments/environment';

describe('AdminService', () => {
    let service: AdminService;
    let httpMock: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
        });
        service = TestBed.inject(AdminService);
        httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        httpMock.verify();
    });

    it('getUsers sends filter fields as query params', () => {
        service.getUsers({ searchTerm: 'ali', role: 'Therapist', page: 1, pageSize: 20 }).subscribe();

        const req = httpMock.expectOne(
            r =>
                r.url === `${environment.apiUrl}${API.admin.users}` &&
                r.params.get('searchTerm') === 'ali' &&
                r.params.get('role') === 'Therapist' &&
                r.params.get('page') === '1'
        );
        expect(req.request.method).toBe('GET');
        req.flush({ items: [], totalCount: 0, page: 1, pageSize: 20 });
    });

    it('getUsers omits undefined/empty filter fields', () => {
        service.getUsers({ page: 1, pageSize: 20 }).subscribe();

        const req = httpMock.expectOne(`${environment.apiUrl}${API.admin.users}?page=1&pageSize=20`);
        expect(req.request.params.has('searchTerm')).toBe(false);
        req.flush({ items: [], totalCount: 0, page: 1, pageSize: 20 });
    });

    it('changeUserRole sends the role name in the request body', () => {
        service.changeUserRole('user-1', 'Admin').subscribe();

        const req = httpMock.expectOne(`${environment.apiUrl}${API.admin.role('user-1')}`);
        expect(req.request.method).toBe('PATCH');
        expect(req.request.body).toEqual({ roleName: 'Admin' });
        req.flush({});
    });

    it('softDeleteUser issues a DELETE request', () => {
        service.softDeleteUser('user-1').subscribe();

        const req = httpMock.expectOne(`${environment.apiUrl}${API.admin.softDelete('user-1')}`);
        expect(req.request.method).toBe('DELETE');
        req.flush({});
    });

    it('updateDoctorStatus sends newStatus in the request body', () => {
        service.updateDoctorStatus('doctor-1', 'Suspended').subscribe();

        const req = httpMock.expectOne(`${environment.apiUrl}${API.admin.doctorStatus('doctor-1')}`);
        expect(req.request.method).toBe('PATCH');
        expect(req.request.body).toEqual({ newStatus: 'Suspended' });
        req.flush({});
    });

    it('getDashboard performs a GET to the dashboard endpoint', () => {
        service.getDashboard().subscribe();

        const req = httpMock.expectOne(`${environment.apiUrl}${API.admin.dashboard}`);
        expect(req.request.method).toBe('GET');
        req.flush({});
    });

    it('updateSettings performs a PUT with the settings body', () => {
        const settings = {
            siteName: 'Jalsa',
            defaultLanguage: 'ar',
            passwordMinLength: 8,
            sessionTimeoutMinutes: 60,
            maintenanceMode: false,
        };

        service.updateSettings(settings).subscribe();

        const req = httpMock.expectOne(`${environment.apiUrl}${API.admin.settings}`);
        expect(req.request.method).toBe('PUT');
        expect(req.request.body).toEqual(settings);
        req.flush(settings);
    });
});
