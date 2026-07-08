import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { PatientSessionSummary, PatientSessionDetail, SessionChangeRequest } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientSessionService {
    private http = inject(HttpClientService);

    getSessions(): Observable<PatientSessionSummary[]> {
        return this.http.get<PatientSessionSummary[]>(API.patientSessions.base);
    }

    getSession(id: string): Observable<PatientSessionDetail> {
        return this.http.get<PatientSessionDetail>(API.patientSessions.byId(id));
    }

    requestReschedule(id: string, data: SessionChangeRequest): Observable<void> {
        return this.http.post<void>(API.patientSessions.requestReschedule(id), data);
    }

    requestCancel(id: string, data: SessionChangeRequest): Observable<void> {
        return this.http.post<void>(API.patientSessions.requestCancel(id), data);
    }
}
