import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { PatientAssessmentSummary, PatientAssessmentDetail, SaveAnswerRequest } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientAssessmentService {
    private http = inject(HttpClientService);

    getList(): Observable<PatientAssessmentSummary[]> {
        return this.http.get<PatientAssessmentSummary[]>(API.patientAssessments.base);
    }

    getDetail(id: string): Observable<PatientAssessmentDetail> {
        return this.http.get<PatientAssessmentDetail>(API.patientAssessments.byId(id));
    }

    saveAnswer(id: string, questionId: string, dto: SaveAnswerRequest): Observable<void> {
        return this.http.put<void>(API.patientAssessments.saveAnswer(id, questionId), dto);
    }

    submit(id: string): Observable<PatientAssessmentDetail> {
        return this.http.post<PatientAssessmentDetail>(API.patientAssessments.submit(id), null);
    }
}
