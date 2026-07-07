import { Injectable, computed, signal } from '@angular/core';
import { PatientDashboard } from '../models';

const CACHE_TTL_MS = 5 * 60 * 1000; // 5 minutes

@Injectable({
    providedIn: 'root',
})
export class PatientDashboardStateService {
    private dashboardSignal = signal<PatientDashboard | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);
    private lastLoadedAtSignal = signal<number | null>(null);

    readonly dashboard = this.dashboardSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();
    readonly lastLoadedAt = this.lastLoadedAtSignal.asReadonly();

    readonly isStale = computed(() => {
        const ts = this.lastLoadedAtSignal();
        return ts === null || Date.now() - ts > CACHE_TTL_MS;
    });

    setDashboard(dashboard: PatientDashboard): void {
        this.dashboardSignal.set(dashboard);
        this.lastLoadedAtSignal.set(Date.now());
        this.errorSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.dashboardSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
        this.lastLoadedAtSignal.set(null);
    }
}
