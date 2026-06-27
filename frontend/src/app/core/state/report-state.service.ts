import { Injectable, signal, computed } from '@angular/core';
import { ReferralReport } from '../models';

@Injectable({
    providedIn: 'root',
})
export class ReportStateService {
    private reportsSignal = signal<ReferralReport[]>([]);
    private selectedReportSignal = signal<ReferralReport | null>(null);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly reports = this.reportsSignal.asReadonly();
    readonly selectedReport = this.selectedReportSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    readonly reportCount = computed(() => this.reportsSignal().length);

    setReports(reports: ReferralReport[]): void {
        this.reportsSignal.set(reports);
        this.errorSignal.set(null);
    }

    addReport(report: ReferralReport): void {
        this.reportsSignal.update(list => [report, ...list]);
    }

    updateReport(updated: ReferralReport): void {
        this.reportsSignal.update(list => list.map(r => (r.id === updated.id ? updated : r)));
        if (this.selectedReportSignal()?.id === updated.id) {
            this.selectedReportSignal.set(updated);
        }
    }

    removeReport(id: string): void {
        this.reportsSignal.update(list => list.filter(r => r.id !== id));
    }

    selectReport(report: ReferralReport): void {
        this.selectedReportSignal.set(report);
    }

    clearSelected(): void {
        this.selectedReportSignal.set(null);
    }

    setLoading(loading: boolean): void {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null): void {
        this.errorSignal.set(error);
    }

    reset(): void {
        this.reportsSignal.set([]);
        this.selectedReportSignal.set(null);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
