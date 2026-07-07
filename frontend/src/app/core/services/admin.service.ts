import { Injectable, inject } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import {
    AdminUser,
    UserFilter,
    PagedResult,
    TherapistAdmin,
    TherapistAdminDetail,
    TherapistFilter,
    PatientAccountAdmin,
    PatientAccountFilter,
    AuditLogEntry,
    AuditLogFilter,
    SystemSettings,
    AdminDashboardSummary,
    SystemHealth,
} from '../models';

function toHttpParams(filter: object): HttpParams {
    let params = new HttpParams();
    for (const [key, value] of Object.entries(filter)) {
        if (value === undefined || value === null || value === '') continue;
        params = params.set(key, String(value));
    }
    return params;
}

@Injectable({
    providedIn: 'root',
})
export class AdminService {
    private http = inject(HttpClientService);

    // Users
    getUsers(filter: UserFilter): Observable<PagedResult<AdminUser>> {
        return this.http.get<PagedResult<AdminUser>>(API.admin.users, toHttpParams(filter));
    }

    setUserActive(id: string, isActive: boolean): Observable<AdminUser> {
        return this.http.patch<AdminUser>(API.admin.status(id), isActive);
    }

    unlockUser(id: string): Observable<AdminUser> {
        return this.http.patch<AdminUser>(API.admin.unlock(id), {});
    }

    changeUserRole(id: string, roleName: string): Observable<AdminUser> {
        return this.http.patch<AdminUser>(API.admin.role(id), { roleName });
    }

    softDeleteUser(id: string): Observable<AdminUser> {
        return this.http.delete<AdminUser>(API.admin.softDelete(id));
    }

    restoreUser(id: string): Observable<AdminUser> {
        return this.http.patch<AdminUser>(API.admin.restore(id), {});
    }

    getUserAuditLogs(id: string, page: number, pageSize: number): Observable<PagedResult<AuditLogEntry>> {
        return this.http.get<PagedResult<AuditLogEntry>>(API.admin.userAuditLogs(id), toHttpParams({ page, pageSize }));
    }

    // Doctors
    getDoctors(filter: TherapistFilter): Observable<PagedResult<TherapistAdmin>> {
        return this.http.get<PagedResult<TherapistAdmin>>(API.admin.doctors, toHttpParams(filter));
    }

    getDoctorDetail(id: string): Observable<TherapistAdminDetail> {
        return this.http.get<TherapistAdminDetail>(API.admin.doctorById(id));
    }

    updateDoctorStatus(id: string, newStatus: string): Observable<TherapistAdmin> {
        return this.http.patch<TherapistAdmin>(API.admin.doctorStatus(id), { newStatus });
    }

    // Patient accounts
    getPatientAccounts(filter: PatientAccountFilter): Observable<PagedResult<PatientAccountAdmin>> {
        return this.http.get<PagedResult<PatientAccountAdmin>>(API.admin.patientAccounts, toHttpParams(filter));
    }

    disablePatientAccount(id: string): Observable<PatientAccountAdmin> {
        return this.http.patch<PatientAccountAdmin>(API.admin.patientAccountDisable(id), {});
    }

    restorePatientAccount(id: string): Observable<PatientAccountAdmin> {
        return this.http.patch<PatientAccountAdmin>(API.admin.patientAccountRestore(id), {});
    }

    deletePatientAccount(id: string): Observable<PatientAccountAdmin> {
        return this.http.delete<PatientAccountAdmin>(API.admin.patientAccountDelete(id));
    }

    // Audit logs
    getAuditLogs(filter: AuditLogFilter): Observable<PagedResult<AuditLogEntry>> {
        return this.http.get<PagedResult<AuditLogEntry>>(API.admin.auditLogs, toHttpParams(filter));
    }

    // Settings
    getSettings(): Observable<SystemSettings> {
        return this.http.get<SystemSettings>(API.admin.settings);
    }

    updateSettings(settings: SystemSettings): Observable<SystemSettings> {
        return this.http.put<SystemSettings>(API.admin.settings, settings);
    }

    // Dashboard + system health
    getDashboard(): Observable<AdminDashboardSummary> {
        return this.http.get<AdminDashboardSummary>(API.admin.dashboard);
    }

    getSystemHealth(): Observable<SystemHealth> {
        return this.http.get<SystemHealth>(API.admin.systemHealth);
    }
}
