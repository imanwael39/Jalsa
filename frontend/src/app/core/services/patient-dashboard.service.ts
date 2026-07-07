import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { PatientDashboard } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientDashboardService {
    private http = inject(HttpClientService);

    getDashboard(): Observable<PatientDashboard> {
        return this.http.get<PatientDashboard>(API.patientDashboard.summary);
    }
}
