import { Component, ChangeDetectionStrategy, computed, DestroyRef, inject, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { interval } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';
import { ChartDataset } from 'chart.js';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { DashboardStateService } from '../../../../core/state/dashboard-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { AppStateService } from '../../../../core/services/app-state.service';
import { TrendDto } from '../../../../core/models/dashboard.model';
import { LineChartComponent } from '../../../../shared/components/line-chart/line-chart.component';
import { BarChartComponent } from '../../../../shared/components/bar-chart/bar-chart.component';

const AUTO_REFRESH_INTERVAL_MS = 5 * 60 * 1000;

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [LineChartComponent, BarChartComponent],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
    private dashboardService = inject(DashboardService);
    private state = inject(DashboardStateService);
    private authService = inject(AuthService);
    private appState = inject(AppStateService);
    private destroyRef = inject(DestroyRef);

    summary = this.state.summary;
    loading = this.state.loading;
    error = this.state.error;
    isStale = this.state.isStale;
    user = this.authService.currentUser;

    assessmentTrendLabels = computed(() => this.extractTrendLabels(this.summary()?.analytics?.assessmentTrend));
    assessmentTrendData = computed(() => {
        this.appState.theme();
        return this.buildLineDataset(
            'درجة التقييم',
            this.summary()?.analytics?.assessmentTrend,
            this.resolveToken('--primary'),
            this.resolveToken('--primary-light')
        );
    });

    sessionFrequencyLabels = computed(() => this.extractTrendLabels(this.summary()?.analytics?.sessionFrequency));
    sessionFrequencyData = computed(() => {
        this.appState.theme();
        return this.buildLineDataset(
            'الجلسات',
            this.summary()?.analytics?.sessionFrequency,
            this.resolveToken('--success'),
            this.resolveToken('--success-light')
        );
    });

    exerciseCompletionLabels = computed(() => ['مكتمل', 'جزئي', 'تم التخطي']);
    exerciseCompletionData = computed<ChartDataset<'bar'>[]>(() => {
        this.appState.theme();
        const breakdown = this.summary()?.analytics?.exerciseCompletion;
        return [
            {
                label: 'التمارين',
                data: breakdown ? [breakdown.complete, breakdown.partial, breakdown.skipped] : [0, 0, 0],
                backgroundColor: [
                    this.resolveToken('--success'),
                    this.resolveToken('--warning'),
                    this.resolveToken('--danger'),
                ],
            },
        ];
    });

    ngOnInit(): void {
        this.loadDashboard();
        this.startAutoRefresh();
    }

    loadDashboard(): void {
        this.state.setLoading(true);
        this.dashboardService
            .getDashboardSummary()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setSummary(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || err.error?.error || 'فشل تحميل بيانات لوحة التحكم');
                    this.state.setLoading(false);
                },
            });
    }

    private startAutoRefresh(): void {
        interval(AUTO_REFRESH_INTERVAL_MS)
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                filter(() => !this.loading()),
                switchMap(() => this.dashboardService.getDashboardSummary())
            )
            .subscribe({
                next: data => {
                    this.state.setSummary(data);
                    this.state.setLoading(false);
                },
                error: () => {
                    this.state.setLoading(false);
                },
            });
    }

    private extractTrendLabels(trends: TrendDto[] | undefined): string[] {
        return trends?.map(t => t.date) ?? [];
    }

    private buildLineDataset(
        label: string,
        trends: TrendDto[] | undefined,
        borderColor: string,
        fillColor: string
    ): ChartDataset<'line'>[] {
        return [
            {
                label,
                data: trends?.map(t => t.value) ?? [],
                borderColor,
                backgroundColor: fillColor,
                tension: 0.3,
                fill: true,
            },
        ];
    }

    private resolveToken(name: string): string {
        return getComputedStyle(document.documentElement).getPropertyValue(name).trim();
    }
}
