import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import {
    Exercise,
    CreateExerciseRequest,
    UpdateExerciseRequest,
    ExerciseLog,
    LogExerciseRequest,
} from '../models';

@Injectable({
    providedIn: 'root',
})
export class ExerciseService {
    private http = inject(HttpClientService);

    getExercises(): Observable<Exercise[]> {
        return this.http.get<Exercise[]>(API.exercises.base);
    }

    getExercise(id: string): Observable<Exercise> {
        return this.http.get<Exercise>(`${API.exercises.base}/${id}`);
    }

    getExercisesByPatient(patientId: string): Observable<Exercise[]> {
        return this.http.get<Exercise[]>(API.exercises.byPatient(patientId));
    }

    createExercise(data: CreateExerciseRequest): Observable<Exercise> {
        return this.http.post<Exercise>(API.exercises.base, data);
    }

    updateExercise(id: string, data: UpdateExerciseRequest): Observable<Exercise> {
        return this.http.put<Exercise>(`${API.exercises.base}/${id}`, data);
    }

    deleteExercise(id: string): Observable<void> {
        return this.http.delete<void>(`${API.exercises.base}/${id}`);
    }

    extendDueDate(id: string, newDueDate: string): Observable<void> {
        return this.http.put<void>(`${API.exercises.base}/${id}/extend`, { newDueDate });
    }

    getMyExercises(): Observable<Exercise[]> {
        return this.http.get<Exercise[]>(API.exercises.myExercises);
    }

    logCompletion(data: LogExerciseRequest): Observable<ExerciseLog> {
        return this.http.post<ExerciseLog>(API.exercises.log, data);
    }

    getMyLogs(): Observable<ExerciseLog[]> {
        return this.http.get<ExerciseLog[]>(API.exercises.myLogs);
    }
}
