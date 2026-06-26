import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import {
    Exercise,
    ExerciseAssignment,
    AssignExerciseRequest,
    UpdateExerciseStatusRequest,
} from '../models';

@Injectable({
    providedIn: 'root',
})
export class ExerciseService {
    private http = inject(HttpClientService);

    getExercises(): Observable<Exercise[]> {
        return this.http.get<Exercise[]>(API.exercises.base);
    }

    getPatientExercises(patientId: string): Observable<ExerciseAssignment[]> {
        return this.http.get<ExerciseAssignment[]>(API.exercises.byPatient(patientId));
    }

    assignExercise(data: AssignExerciseRequest): Observable<ExerciseAssignment> {
        return this.http.post<ExerciseAssignment>(API.exercises.assign, data);
    }

    updateStatus(id: string, data: UpdateExerciseStatusRequest): Observable<void> {
        return this.http.put<void>(API.exercises.status(id), data);
    }
}
