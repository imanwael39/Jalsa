import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { DashboardSummary } from '../models';

@Injectable({
    providedIn: 'root',
})
export class DashboardService {
    private http = inject(HttpClientService);

    getDashboardSummary(): Observable<DashboardSummary> {
        return this.http.get<DashboardSummary>(API.dashboard.summary);
    }
}
