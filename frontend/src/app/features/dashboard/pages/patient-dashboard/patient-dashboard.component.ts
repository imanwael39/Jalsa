import { Component, ChangeDetectionStrategy, computed, DestroyRef, inject, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { interval } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';
import { ChartDataset } from 'chart.js';
import { PatientDashboardService } from '../../../../core/services/patient-dashboard.service';
import { PatientDashboardStateService } from '../../../../core/state/patient-dashboard-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { AppStateService } from '../../../../core/services/app-state.service';
import { LineChartComponent } from '../../../../shared/components/line-chart/line-chart.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { StatsCardComponent } from '../../../../shared/components/stats-card/stats-card.component';

const AUTO_REFRESH_INTERVAL_MS = 5 * 60 * 1000;

@Component({
    selector: 'app-patient-dashboard',
    standalone: true,
    imports: [RouterLink, LineChartComponent, EmptyStateComponent, StatsCardComponent],
    templateUrl: './patient-dashboard.component.html',
    styleUrl: './patient-dashboard.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientDashboardComponent implements OnInit {
    private patientDashboardService = inject(PatientDashboardService);
    private state = inject(PatientDashboardStateService);
    private authService = inject(AuthService);
    private appState = inject(AppStateService);
    private destroyRef = inject(DestroyRef);

    dashboard = this.state.dashboard;
    loading = this.state.loading;
    error = this.state.error;
    isStale = this.state.isStale;
    user = this.authService.currentUser;

    assessmentTrendLabels = computed(
        () => this.dashboard()?.progressOverview?.assessmentTrend?.map(t => t.label) ?? []
    );
    assessmentTrendData = computed<ChartDataset<'line'>[]>(() => {
        this.appState.theme();
        const trend = this.dashboard()?.progressOverview?.assessmentTrend;
        return [
            {
                label: 'درجة التقييم',
                data: trend?.map(t => t.value) ?? [],
                borderColor: this.resolveToken('--primary'),
                backgroundColor: this.resolveToken('--primary-light'),
                tension: 0.3,
                fill: true,
            },
        ];
    });

    ngOnInit(): void {
        this.loadDashboard();
        this.startAutoRefresh();
    }

    loadDashboard(): void {
        this.state.setLoading(true);
        this.patientDashboardService
            .getDashboard()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setDashboard(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.error || err.error?.message || 'فشل تحميل لوحة التحكم');
                    this.state.setLoading(false);
                },
            });
    }

    private startAutoRefresh(): void {
        interval(AUTO_REFRESH_INTERVAL_MS)
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                filter(() => !this.loading()),
                switchMap(() => this.patientDashboardService.getDashboard())
            )
            .subscribe({
                next: data => {
                    this.state.setDashboard(data);
                    this.state.setLoading(false);
                },
                error: () => {
                    this.state.setLoading(false);
                },
            });
    }

    private resolveToken(name: string): string {
        return getComputedStyle(document.documentElement).getPropertyValue(name).trim();
    }
}
