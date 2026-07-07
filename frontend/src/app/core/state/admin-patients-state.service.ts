import { Injectable, signal } from '@angular/core';
import { PatientAccountAdmin } from '../models';

@Injectable({
    providedIn: 'root',
})
export class AdminPatientsStateService {
    private patientsSignal = signal<PatientAccountAdmin[]>([]);
    private totalCountSignal = signal(0);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly patients = this.patientsSignal.asReadonly();
    readonly totalCount = this.totalCountSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setPatients(patients: PatientAccountAdmin[], totalCount: number): void {
        this.patientsSignal.set(patients);
        this.totalCountSignal.set(totalCount);
        this.errorSignal.set(null);
    }

    updatePatient(updated: PatientAccountAdmin): void {
        this.patientsSignal.update(list => list.map(p => (p.id === updated.id ? updated : p)));
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.patientsSignal.set([]);
        this.totalCountSignal.set(0);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
