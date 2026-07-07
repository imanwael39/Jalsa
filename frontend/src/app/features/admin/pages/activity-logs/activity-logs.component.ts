import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AdminService } from '../../../../core/services/admin.service';
import { AuditLogEntry } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';

@Component({
    selector: 'app-activity-logs',
    standalone: true,
    imports: [FormsModule, TableComponent, ColumnCellDirective, PaginationComponent],
    templateUrl: './activity-logs.component.html',
    styleUrl: './activity-logs.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ActivityLogsComponent implements OnInit {
    private adminService = inject(AdminService);
    private destroyRef = inject(DestroyRef);

    logs = signal<AuditLogEntry[]>([]);
    totalCount = signal(0);
    loading = signal(false);
    error = signal<string | null>(null);

    entityNameFilter = '';
    actionFilter = '';
    currentPage = 1;
    pageSize = 20;

    columns: TableColumn[] = [
        { key: 'occurredAt', label: 'التاريخ' },
        { key: 'userEmail', label: 'المستخدم' },
        { key: 'action', label: 'الإجراء' },
        { key: 'entityName', label: 'الكيان' },
        { key: 'ipAddress', label: 'عنوان IP' },
    ];

    ngOnInit(): void {
        this.load();
    }

    onFilterChange(): void {
        this.currentPage = 1;
        this.load();
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.load();
    }

    load(): void {
        this.loading.set(true);
        this.error.set(null);
        this.adminService
            .getAuditLogs({
                entityName: this.entityNameFilter || undefined,
                action: this.actionFilter || undefined,
                page: this.currentPage,
                pageSize: this.pageSize,
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.logs.set(result.items);
                    this.totalCount.set(result.totalCount);
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || 'فشل تحميل سجل النشاط.');
                    this.loading.set(false);
                },
            });
    }

    formatDate(date: string): string {
        return new Date(date).toLocaleDateString('ar-EG', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        });
    }

    trackByLogId(index: number, log: AuditLogEntry): string {
        return log.id;
    }
}
