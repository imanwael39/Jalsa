import { Injectable, signal, computed } from '@angular/core';
import { Session } from '../models';

@Injectable({
    providedIn: 'root',
})
export class SessionStateService {
    private sessionsSignal = signal<Session[]>([]);
    private selectedSessionSignal = signal<Session | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly sessions = this.sessionsSignal.asReadonly();
    readonly selectedSession = this.selectedSessionSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    readonly sessionCount = computed(() => this.sessionsSignal().length);

    setSessions(sessions: Session[]): void {
        this.sessionsSignal.set(sessions);
        this.errorSignal.set(null);
    }

    addSession(session: Session): void {
        this.sessionsSignal.update(list => [...list, session]);
    }

    updateSession(updated: Session): void {
        this.sessionsSignal.update(list =>
            list.map(s => (s.id === updated.id ? updated : s)),
        );
        if (this.selectedSessionSignal()?.id === updated.id) {
            this.selectedSessionSignal.set(updated);
        }
    }

    removeSession(id: string): void {
        this.sessionsSignal.update(list => list.filter(s => s.id !== id));
    }

    selectSession(session: Session): void {
        this.selectedSessionSignal.set(session);
    }

    clearSelected(): void {
        this.selectedSessionSignal.set(null);
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
