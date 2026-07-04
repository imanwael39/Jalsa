import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';

@Injectable({
    providedIn: 'root',
})
export class AiService {
    private http = inject(HttpClientService);

    summarizePatient(patientId: string, language = 'ar'): Observable<{ summary: string }> {
        return this.http.post<{ summary: string }>(API.ai.summarize(patientId), { language });
    }

    generateReportDraft(
        patientId: string,
        therapistInstructions?: string,
        language = 'ar'
    ): Observable<{ draft: string }> {
        return this.http.post<{ draft: string }>(API.ai.reportDraft(patientId), { therapistInstructions, language });
    }
}
