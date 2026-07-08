import { Component, ChangeDetectionStrategy, DestroyRef, computed, inject, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ChartDataset } from 'chart.js';
import { PatientProgressService } from '../../../../core/services/patient-progress.service';
import { PatientProgressStateService } from '../../../../core/state/patient-progress-state.service';
import { AppStateService } from '../../../../core/services/app-state.service';
import { LineChartComponent } from '../../../../shared/components/line-chart/line-chart.component';
import { BarChartComponent } from '../../../../shared/components/bar-chart/bar-chart.component';
import { StatsCardComponent } from '../../../../shared/components/stats-card/stats-card.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-patient-progress',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        LineChartComponent,
        BarChartComponent,
        StatsCardComponent,
        InputComponent,
        ButtonComponent,
    ],
    templateUrl: './patient-progress.component.html',
    styleUrl: './patient-progress.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientProgressComponent implements OnInit {
    private patientProgressService = inject(PatientProgressService);
    private state = inject(PatientProgressStateService);
    private appState = inject(AppStateService);
    private destroyRef = inject(DestroyRef);
    private fb = inject(NonNullableFormBuilder);

    progress = this.state.progress;
    loading = this.state.loading;
    error = this.state.error;

    filterForm = this.fb.group({
        from: [''],
        to: [''],
    });

    assessmentScoreLabels = computed(() => this.progress()?.assessmentScoreTrend?.map(t => t.label) ?? []);
    assessmentScoreData = computed<ChartDataset<'line'>[]>(() => {
        this.appState.theme();
        return [
            this.buildLineDataset(
                'درجة التقييم',
                this.progress()?.assessmentScoreTrend,
                '--primary',
                '--primary-light'
            ),
        ];
    });

    moodLabels = computed(() => this.progress()?.moodTrend?.map(t => t.label) ?? []);
    moodData = computed<ChartDataset<'line'>[]>(() => {
        this.appState.theme();
        return [this.buildLineDataset('المزاج', this.progress()?.moodTrend, '--success', '--success-light')];
    });

    exerciseCompletionLabels = computed(() => this.progress()?.exerciseCompletionTrend?.map(t => t.label) ?? []);
    exerciseCompletionData = computed<ChartDataset<'bar'>[]>(() => {
        this.appState.theme();
        const trend = this.progress()?.exerciseCompletionTrend;
        return [
            {
                label: 'تمارين مكتملة',
                data: trend?.map(t => t.value) ?? [],
                backgroundColor: this.resolveToken('--info'),
            },
        ];
    });

    private statusArPipe = new StatusArPipe();

    attendanceLabels = computed(
        () => this.progress()?.attendanceBreakdown?.map(b => this.statusArPipe.transform(b.status)) ?? []
    );
    attendanceData = computed<ChartDataset<'bar'>[]>(() => {
        this.appState.theme();
        const breakdown = this.progress()?.attendanceBreakdown ?? [];
        return [
            {
                label: 'الجلسات',
                data: breakdown.map(b => b.count),
                backgroundColor: breakdown.map(b => this.resolveAttendanceColor(b.status)),
            },
        ];
    });

    ngOnInit(): void {
        this.loadProgress();
    }

    loadProgress(): void {
        const { from, to } = this.filterForm.getRawValue();
        this.state.setLoading(true);
        this.patientProgressService
            .getProgress({ from: from || undefined, to: to || undefined })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setProgress(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.error || err.error?.message || 'فشل تحميل بيانات التقدم');
                    this.state.setLoading(false);
                },
            });
    }

    resetFilter(): void {
        this.filterForm.reset({ from: '', to: '' });
        this.loadProgress();
    }

    private buildLineDataset(
        label: string,
        trend: { value: number }[] | undefined,
        borderToken: string,
        fillToken: string
    ): ChartDataset<'line'> {
        return {
            label,
            data: trend?.map(t => t.value) ?? [],
            borderColor: this.resolveToken(borderToken),
            backgroundColor: this.resolveToken(fillToken),
            tension: 0.3,
            fill: true,
        };
    }

    private resolveAttendanceColor(status: string): string {
        if (status === 'Completed') return this.resolveToken('--success');
        if (status === 'Cancelled') return this.resolveToken('--danger');
        return this.resolveToken('--gray-300');
    }

    private resolveToken(name: string): string {
        return getComputedStyle(document.documentElement).getPropertyValue(name).trim();
    }
}
