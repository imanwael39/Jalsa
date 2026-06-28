import { Injectable, inject } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { Patient, CreatePatientRequest, UpdatePatientRequest, PatientFilter, IntakeForm, Assessment } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientService {
    private http = inject(HttpClientService);

    getPatients(filter: PatientFilter): Observable<Patient[]> {
        let params = new HttpParams().set('page', filter.page.toString()).set('pageSize', filter.pageSize.toString());
        if (filter.searchTerm) {
            params = params.set('searchTerm', filter.searchTerm);
        }
        if (filter.status) {
            params = params.set('status', filter.status);
        }
        return this.http.get<Patient[]>(API.patients.base, params);
    }

    getPatient(id: string): Observable<Patient> {
        return this.http.get<Patient>(API.patients.byId(id));
    }

    createPatient(data: CreatePatientRequest): Observable<Patient> {
        return this.http.post<Patient>(API.patients.base, data);
    }

    updatePatient(id: string, data: UpdatePatientRequest): Observable<Patient> {
        return this.http.put<Patient>(API.patients.byId(id), data);
    }

    archivePatient(id: string): Observable<void> {
        return this.http.patch<void>(API.patients.archive(id), {});
    }

    restorePatient(id: string): Observable<void> {
        return this.http.patch<void>(API.patients.restore(id), {});
    }

    deletePatient(id: string): Observable<void> {
        return this.http.delete<void>(API.patients.byId(id));
    }

    getIntakeForm(patientId: string): Observable<IntakeForm> {
        return this.http.get<IntakeForm>(API.patients.intake(patientId));
    }

    saveIntakeForm(patientId: string, data: Partial<IntakeForm>): Observable<IntakeForm> {
        return this.http.post<IntakeForm>(API.patients.intake(patientId), data);
    }

    uploadIntakeImage(
        patientId: string,
        intakeFormId: string,
        file: File
    ): Observable<{ imageUrl: string; extractedData: Record<string, string> }> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.upload<{ imageUrl: string; extractedData: Record<string, string> }>(
            API.patients.intakeOcr(patientId, intakeFormId),
            formData
        );
    }

    getAssessments(patientId: string): Observable<Assessment[]> {
        return this.http.get<Assessment[]>(API.patients.assessments(patientId));
    }

    addAssessment(patientId: string, data: Partial<Assessment>): Observable<Assessment> {
        return this.http.post<Assessment>(API.patients.assessments(patientId), data);
    }
}
