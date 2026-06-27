import { Component, ChangeDetectionStrategy, inject, signal, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReportService } from '../../../../core/services/report.service';
import { ReportStateService } from '../../../../core/state/report-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-report-detail',
    standalone: true,
    imports: [ButtonComponent, SpinnerComponent],
    templateUrl: './report-detail.html',
    styleUrl: './report-detail.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReportDetail implements OnInit, OnDestroy {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private reportService = inject(ReportService);
    private state = inject(ReportStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    report = this.state.selectedReport;
    loading = signal(true);
    error = signal<string | null>(null);
    approving = signal(false);

    ngOnDestroy(): void {
        this.state.clearSelected();
    }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.loadReport(id);
        } else {
            this.error.set('معرّف التقرير غير موجود');
            this.loading.set(false);
        }
    }

    loadReport(id: string): void {
        this.loading.set(true);
        this.error.set(null);
        this.reportService
            .getReport(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: report => {
                    this.state.selectReport(report);
                    this.loading.set(false);
                },
                error: err => {
                    this.error.set(err.message || 'فشل في تحميل التقرير');
                    this.loading.set(false);
                },
            });
    }

    approveReport(): void {
        const r = this.report();
        if (!r) return;

        this.approving.set(true);
        this.reportService
            .approveReport(r.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.selectReport(updated);
                    this.state.updateReport(updated);
                    this.notification.success('تم اعتماد التقرير بنجاح');
                    this.approving.set(false);
                },
                error: err => {
                    this.notification.error(err.message || 'فشل في اعتماد التقرير');
                    this.approving.set(false);
                },
            });
    }

    deleteReport(): void {
        const r = this.report();
        if (!r) return;

        if (confirm('هل أنت متأكد من حذف هذا التقرير؟')) {
            this.reportService
                .deleteReport(r.id)
                .pipe(takeUntilDestroyed(this.destroyRef))
                .subscribe({
                    next: () => {
                        this.state.removeReport(r.id);
                        this.state.clearSelected();
                        this.notification.success('تم حذف التقرير بنجاح');
                        this.router.navigate(['/reports/patient', r.patientId]);
                    },
                    error: err => {
                        this.notification.error(err.message || 'فشل في حذف التقرير');
                    },
                });
        }
    }

    navigateBack(): void {
        const r = this.report();
        if (r) {
            this.router.navigate(['/reports/patient', r.patientId]);
        }
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString('ar-EG');
    }

    formatDateTime(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleString('ar-EG');
    }
}
