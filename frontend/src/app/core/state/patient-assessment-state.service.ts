import { Injectable, signal } from '@angular/core';
import { PatientAssessmentSummary, PatientAssessmentDetail } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientAssessmentStateService {
    private listSignal = signal<PatientAssessmentSummary[]>([]);
    private detailSignal = signal<PatientAssessmentDetail | null>(null);
    private loadingSignal = signal(false);
    private savingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly list = this.listSignal.asReadonly();
    readonly detail = this.detailSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly saving = this.savingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setList(list: PatientAssessmentSummary[]): void {
        this.listSignal.set(list);
        this.errorSignal.set(null);
    }

    setDetail(detail: PatientAssessmentDetail): void {
        this.detailSignal.set(detail);
        this.errorSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setSaving(saving: boolean): void {
        this.savingSignal.set(saving);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.listSignal.set([]);
        this.detailSignal.set(null);
        this.loadingSignal.set(false);
        this.savingSignal.set(false);
        this.errorSignal.set(null);
    }
}
