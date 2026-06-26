import { Injectable, signal } from '@angular/core';
import { Exercise, ExerciseAssignment } from '../models';

@Injectable({
    providedIn: 'root',
})
export class ExerciseStateService {
    private exercisesSignal = signal<Exercise[]>([]);
    private assignmentsSignal = signal<ExerciseAssignment[]>([]);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly exercises = this.exercisesSignal.asReadonly();
    readonly assignments = this.assignmentsSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setExercises(exercises: Exercise[]): void {
        this.exercisesSignal.set(exercises);
        this.errorSignal.set(null);
    }

    setAssignments(assignments: ExerciseAssignment[]): void {
        this.assignmentsSignal.set(assignments);
        this.errorSignal.set(null);
    }

    addAssignment(assignment: ExerciseAssignment): void {
        this.assignmentsSignal.update(list => [...list, assignment]);
    }

    updateAssignmentStatus(id: string, status: string, reflection?: string): void {
        this.assignmentsSignal.update(list =>
            list.map(a =>
                a.id === id
                    ? { ...a, status, reflection: reflection ?? a.reflection }
                    : a,
            ),
        );
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.exercisesSignal.set([]);
        this.assignmentsSignal.set([]);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
