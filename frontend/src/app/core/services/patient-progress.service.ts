import { Injectable, inject } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { PatientProgress, PatientProgressDateFilter } from '../models';

function toHttpParams(filter: PatientProgressDateFilter): HttpParams {
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
export class PatientProgressService {
    private http = inject(HttpClientService);

    getProgress(filter: PatientProgressDateFilter = {}): Observable<PatientProgress> {
        return this.http.get<PatientProgress>(API.patientProgress.summary, toHttpParams(filter));
    }
}
