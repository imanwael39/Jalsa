import { Injectable, signal } from '@angular/core';
import { PatientProgress } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientProgressStateService {
    private progressSignal = signal<PatientProgress | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly progress = this.progressSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setProgress(progress: PatientProgress): void {
        this.progressSignal.set(progress);
        this.errorSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.progressSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
