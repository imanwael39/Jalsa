import { Injectable, signal } from '@angular/core';
import { DashboardSummary } from '../models';

@Injectable({
    providedIn: 'root',
})
export class DashboardStateService {
    private summarySignal = signal<DashboardSummary | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly summary = this.summarySignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setSummary(summary: DashboardSummary): void {
        this.summarySignal.set(summary);
        this.errorSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.summarySignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
