import { Injectable, signal, computed } from '@angular/core';
import { Patient } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientStateService {
    private patientsSignal = signal<Patient[]>([]);
    private selectedPatientSignal = signal<Patient | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly patients = this.patientsSignal.asReadonly();
    readonly selectedPatient = this.selectedPatientSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    readonly patientCount = computed(() => this.patientsSignal().length);

    setPatients(patients: Patient[]): void {
        this.patientsSignal.set(patients);
        this.errorSignal.set(null);
    }

    addPatient(patient: Patient): void {
        this.patientsSignal.update(list => [...list, patient]);
    }

    updatePatient(updated: Patient): void {
        this.patientsSignal.update(list =>
            list.map(p => (p.id === updated.id ? updated : p)),
        );
        if (this.selectedPatientSignal()?.id === updated.id) {
            this.selectedPatientSignal.set(updated);
        }
    }

    removePatient(id: string): void {
        this.patientsSignal.update(list => list.filter(p => p.id !== id));
    }

    selectPatient(patient: Patient): void {
        this.selectedPatientSignal.set(patient);
    }

    clearSelected(): void {
        this.selectedPatientSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.patientsSignal.set([]);
        this.selectedPatientSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
