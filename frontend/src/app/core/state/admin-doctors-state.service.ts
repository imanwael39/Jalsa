import { Injectable, signal } from '@angular/core';
import { TherapistAdmin } from '../models';

@Injectable({
    providedIn: 'root',
})
export class AdminDoctorsStateService {
    private doctorsSignal = signal<TherapistAdmin[]>([]);
    private totalCountSignal = signal(0);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly doctors = this.doctorsSignal.asReadonly();
    readonly totalCount = this.totalCountSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setDoctors(doctors: TherapistAdmin[], totalCount: number): void {
        this.doctorsSignal.set(doctors);
        this.totalCountSignal.set(totalCount);
        this.errorSignal.set(null);
    }

    updateDoctor(updated: TherapistAdmin): void {
        this.doctorsSignal.update(list => list.map(d => (d.id === updated.id ? updated : d)));
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.doctorsSignal.set([]);
        this.totalCountSignal.set(0);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
