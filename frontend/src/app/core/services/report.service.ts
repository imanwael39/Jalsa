import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { ReferralReport } from '../models';

export interface GenerateReportRequest {
    patientId: string;
    therapistInstructions?: string;
    language?: string;
}

export interface UpdateReportRequest {
    content?: string;
    changeNote?: string;
}

@Injectable({
    providedIn: 'root',
})
export class ReportService {
    private http = inject(HttpClientService);

    generateReport(data: GenerateReportRequest): Observable<ReferralReport> {
        return this.http.post<ReferralReport>(API.reports.generate, data);
    }

    getReport(id: string): Observable<ReferralReport> {
        return this.http.get<ReferralReport>(API.reports.byId(id));
    }

    getPatientReports(patientId: string): Observable<ReferralReport[]> {
        return this.http.get<ReferralReport[]>(`${API.reports.base}/patient/${patientId}`);
    }

    updateReport(id: string, data: UpdateReportRequest): Observable<ReferralReport> {
        return this.http.put<ReferralReport>(API.reports.byId(id), data);
    }

    approveReport(id: string): Observable<ReferralReport> {
        return this.http.post<ReferralReport>(API.reports.approve(id), null);
    }

    deleteReport(id: string): Observable<void> {
        return this.http.delete<void>(API.reports.byId(id));
    }

    exportReport(id: string): Observable<Blob> {
        return this.http.blob(API.reports.export(id));
    }
}
