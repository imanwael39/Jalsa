import { Component, ChangeDetectionStrategy, computed, inject, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { interval, Subscription } from 'rxjs';
import { switchMap, filter } from 'rxjs/operators';
import { ChartDataset } from 'chart.js';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { DashboardStateService } from '../../../../core/state/dashboard-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { TrendDto } from '../../../../core/models/dashboard.model';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { LineChartComponent } from '../../../../shared/components/line-chart/line-chart.component';
import { BarChartComponent } from '../../../../shared/components/bar-chart/bar-chart.component';

const AUTO_REFRESH_INTERVAL_MS = 5 * 60 * 1000; // 5 minutes

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [SpinnerComponent, LineChartComponent, BarChartComponent],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit, OnDestroy {
    private dashboardService = inject(DashboardService);
    private state = inject(DashboardStateService);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);
    private autoRefreshSub: Subscription | null = null;

    summary = this.state.summary;
    loading = this.state.loading;
    error = this.state.error;
    isStale = this.state.isStale;
    user = this.authService.currentUser;

    assessmentTrendLabels = computed(() => this.extractTrendLabels(this.summary()?.analytics?.assessmentTrend));
    assessmentTrendData = computed(() =>
        this.buildLineDataset('درجة التقييم', this.summary()?.analytics?.assessmentTrend, '#0d6efd')
    );

    sessionFrequencyLabels = computed(() => this.extractTrendLabels(this.summary()?.analytics?.sessionFrequency));
    sessionFrequencyData = computed(() =>
        this.buildLineDataset('الجلسات', this.summary()?.analytics?.sessionFrequency, '#198754')
    );

    exerciseCompletionLabels = computed(() => ['مكتمل', 'جزئي', 'تم التخطي']);
    exerciseCompletionData = computed<ChartDataset<'bar'>[]>(() => {
        const breakdown = this.summary()?.analytics?.exerciseCompletion;
        return [
            {
                label: 'التمارين',
                data: breakdown ? [breakdown.complete, breakdown.partial, breakdown.skipped] : [0, 0, 0],
                backgroundColor: ['#198754', '#ffc107', '#dc3545'],
            },
        ];
    });

    ngOnInit(): void {
        this.loadDashboard();
        this.startAutoRefresh();
    }

    ngOnDestroy(): void {
        this.stopAutoRefresh();
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
                error: err => {
                    this.state.setError(err.message || 'فشل تحميل بيانات لوحة التحكم');
                    this.state.setLoading(false);
                },
            });
    }

    private startAutoRefresh(): void {
        this.autoRefreshSub = interval(AUTO_REFRESH_INTERVAL_MS)
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

    private stopAutoRefresh(): void {
        if (this.autoRefreshSub) {
            this.autoRefreshSub.unsubscribe();
            this.autoRefreshSub = null;
        }
    }

    private extractTrendLabels(trends: TrendDto[] | undefined): string[] {
        return trends?.map(t => t.date) ?? [];
    }

    private buildLineDataset(label: string, trends: TrendDto[] | undefined, color: string): ChartDataset<'line'>[] {
        return [
            {
                label,
                data: trends?.map(t => t.value) ?? [],
                borderColor: color,
                backgroundColor: color + '20',
                tension: 0.3,
                fill: true,
            },
        ];
    }
}
