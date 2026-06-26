import { Injectable, signal, computed } from '@angular/core';
import { Exercise, ExerciseLog } from '../models';

@Injectable({
    providedIn: 'root',
})
export class ExerciseStateService {
    private exercisesSignal = signal<Exercise[]>([]);
    private logsSignal = signal<ExerciseLog[]>([]);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly exercises = this.exercisesSignal.asReadonly();
    readonly logs = this.logsSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    readonly logsByExercise = computed(() => {
        const allLogs = this.logsSignal();
        const map = new Map<string, ExerciseLog[]>();
        for (const log of allLogs) {
            const existing = map.get(log.exerciseId) || [];
            existing.push(log);
            map.set(log.exerciseId, existing);
        }
        return map;
    });

    setExercises(exercises: Exercise[]): void {
        this.exercisesSignal.set(exercises);
        this.errorSignal.set(null);
    }

    addExercise(exercise: Exercise): void {
        this.exercisesSignal.update(list => [...list, exercise]);
    }

    setLogs(logs: ExerciseLog[]): void {
        this.logsSignal.set(logs);
    }

    addLog(log: ExerciseLog): void {
        this.logsSignal.update(list => [...list, log]);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.exercisesSignal.set([]);
        this.logsSignal.set([]);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
