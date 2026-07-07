import { Component, ChangeDetectionStrategy, inject, signal, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import html2pdf from 'html2pdf.js';
import { ReportService } from '../../../../core/services/report.service';
import { ReportStateService } from '../../../../core/state/report-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-report-detail',
    standalone: true,
    imports: [ButtonComponent, SpinnerComponent, ModalComponent, StatusArPipe],
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
    rejecting = signal(false);
    exporting = signal(false);
    showDeleteModal = signal(false);

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
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل في تحميل التقرير');
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
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل في اعتماد التقرير');
                    this.approving.set(false);
                },
            });
    }

    rejectReport(): void {
        const r = this.report();
        if (!r) return;

        this.rejecting.set(true);
        this.reportService
            .rejectReport(r.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.selectReport(updated);
                    this.state.updateReport(updated);
                    this.notification.success('تم رفض التقرير');
                    this.rejecting.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل في رفض التقرير');
                    this.rejecting.set(false);
                },
            });
    }

    openDeleteModal(): void {
        this.showDeleteModal.set(true);
    }

    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
    }

    deleteReport(): void {
        const r = this.report();
        if (!r) return;

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
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل في حذف التقرير');
                    this.closeDeleteModal();
                },
            });
    }

    exportReport(): void {
        const r = this.report();
        if (!r || !r.currentVersion?.content) return;

        this.exporting.set(true);
        this.reportService
            .exportReport(r.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: async blob => {
                    const html = await blob.text();
                    const container = document.createElement('div');
                    container.innerHTML = html;
                    container.style.position = 'fixed';
                    container.style.left = '0';
                    container.style.top = '0';
                    container.style.width = '210mm';
                    container.style.zIndex = '-1';
                    container.style.pointerEvents = 'none';
                    document.body.appendChild(container);

                    try {
                        await new Promise(resolve => requestAnimationFrame(resolve));
                        await html2pdf()
                            .set({
                                margin: 10,
                                filename: `report-${r.id}.pdf`,
                                html2canvas: { scale: 2 },
                                jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' },
                            })
                            .from(container)
                            .save();
                    } catch {
                        this.notification.error('فشل في تصدير التقرير كملف PDF');
                    } finally {
                        document.body.removeChild(container);
                        this.exporting.set(false);
                    }
                },
                error: () => {
                    this.notification.error('فشل في تصدير التقرير');
                    this.exporting.set(false);
                },
            });
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
