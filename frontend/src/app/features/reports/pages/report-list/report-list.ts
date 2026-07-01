import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef, input } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReportService } from '../../../../core/services/report.service';
import { ReportStateService } from '../../../../core/state/report-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { ReferralReport } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-report-list',
    standalone: true,
    imports: [TableComponent, ColumnCellDirective, ButtonComponent, ModalComponent, StatusArPipe],
    templateUrl: './report-list.html',
    styleUrl: './report-list.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReportList implements OnInit {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private reportService = inject(ReportService);
    private state = inject(ReportStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    patientIdInput = input<string | null>(null);

    patientId = '';
    reports = this.state.reports;
    loading = this.state.loading;
    error = this.state.error;

    showDeleteModal = signal(false);
    deletingReportId = signal<string | null>(null);

    columns: TableColumn[] = [
        { key: 'createdAt', label: 'التاريخ', sortable: true },
        { key: 'status', label: 'الحالة' },
        { key: 'versionCount', label: 'الإصدارات' },
        { key: 'actions', label: 'الإجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        const inputId = this.patientIdInput();
        if (inputId) {
            this.patientId = inputId;
        } else {
            this.patientId = this.route.snapshot.paramMap.get('patientId') || '';
        }
        if (this.patientId) {
            this.loadReports();
        }
    }

    loadReports(): void {
        this.state.setLoading(true);
        this.reportService
            .getPatientReports(this.patientId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setReports(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || err.error?.error || 'فشل في تحميل التقارير');
                    this.state.setLoading(false);
                },
            });
    }

    onRowClick(report: ReferralReport): void {
        this.router.navigate(['/reports', report.id]);
    }

    navigateToGenerate(): void {
        this.router.navigate(['/reports/generate', this.patientId]);
    }

    navigateToView(id: string): void {
        this.router.navigate(['/reports', id]);
    }

    openDeleteModal(id: string): void {
        this.deletingReportId.set(id);
        this.showDeleteModal.set(true);
    }

    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
        this.deletingReportId.set(null);
    }

    deleteReport(): void {
        const id = this.deletingReportId();
        if (!id) return;

        this.reportService
            .deleteReport(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.removeReport(id);
                    this.notification.success('تم حذف التقرير بنجاح');
                    this.closeDeleteModal();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل في حذف التقرير');
                },
            });
    }

    formatDate(date: string): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString('ar-EG');
    }

    getVersionCount(report: ReferralReport): number {
        return report.versions?.length ?? 0;
    }
}
