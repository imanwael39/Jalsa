import { Injectable, computed, signal } from '@angular/core';
import { DashboardSummary } from '../models';

const CACHE_TTL_MS = 5 * 60 * 1000; // 5 minutes

@Injectable({
    providedIn: 'root',
})
export class DashboardStateService {
    private summarySignal = signal<DashboardSummary | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);
    private lastLoadedAtSignal = signal<number | null>(null);

    readonly summary = this.summarySignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();
    readonly lastLoadedAt = this.lastLoadedAtSignal.asReadonly();

    readonly isStale = computed(() => {
        const ts = this.lastLoadedAtSignal();
        return ts === null || Date.now() - ts > CACHE_TTL_MS;
    });

    setSummary(summary: DashboardSummary): void {
        this.summarySignal.set(summary);
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
        this.summarySignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
        this.lastLoadedAtSignal.set(null);
    }
}
