import { Injectable, signal } from '@angular/core';
import { PatientSessionSummary, PatientSessionDetail } from '../models';

@Injectable({
    providedIn: 'root',
})
export class PatientSessionStateService {
    private sessionsSignal = signal<PatientSessionSummary[]>([]);
    private selectedSessionSignal = signal<PatientSessionDetail | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly sessions = this.sessionsSignal.asReadonly();
    readonly selectedSession = this.selectedSessionSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setSessions(sessions: PatientSessionSummary[]): void {
        this.sessionsSignal.set(sessions);
        this.errorSignal.set(null);
    }

    setSelectedSession(session: PatientSessionDetail | null): void {
        this.selectedSessionSignal.set(session);
        this.errorSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.sessionsSignal.set([]);
        this.selectedSessionSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
