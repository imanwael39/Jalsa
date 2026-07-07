import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { AdminService } from '../../../../core/services/admin.service';
import { AdminPatientsStateService } from '../../../../core/state/admin-patients-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { PatientAccountAdmin } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';

/**
 * Account-only view — this page never links into clinical patient pages (sessions,
 * exercises, notes, reports). Admin manages accounts, not treatment data.
 */
@Component({
    selector: 'app-patient-account-list',
    standalone: true,
    imports: [FormsModule, TableComponent, ColumnCellDirective, ButtonComponent, PaginationComponent],
    templateUrl: './patient-account-list.component.html',
    styleUrl: './patient-account-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientAccountListComponent implements OnInit {
    private adminService = inject(AdminService);
    private state = inject(AdminPatientsStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    patients = this.state.patients;
    totalCount = this.state.totalCount;
    loading = this.state.loading;
    error = this.state.error;
    busyId = signal<string | null>(null);

    searchTerm = '';
    statusFilter = '';
    currentPage = 1;
    pageSize = 10;

    private searchSubject = new Subject<string>();

    columns: TableColumn[] = [
        { key: 'fullName', label: 'الاسم' },
        { key: 'email', label: 'البريد الإلكتروني' },
        { key: 'therapistName', label: 'المعالج' },
        { key: 'status', label: 'حالة السجل' },
        { key: 'accountStatus', label: 'حالة الحساب' },
        { key: 'actions', label: 'إجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        this.loadPatients();

        this.searchSubject
            .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed(this.destroyRef))
            .subscribe(() => {
                this.currentPage = 1;
                this.loadPatients();
            });
    }

    onSearch(term: string): void {
        this.searchTerm = term;
        this.searchSubject.next(term);
    }

    onFilterChange(): void {
        this.currentPage = 1;
        this.loadPatients();
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.loadPatients();
    }

    loadPatients(): void {
        this.state.setLoading(true);
        this.adminService
            .getPatientAccounts({
                searchTerm: this.searchTerm || undefined,
                status: this.statusFilter || undefined,
                page: this.currentPage,
                pageSize: this.pageSize,
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.state.setPatients(result.items, result.totalCount);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || 'فشل تحميل حسابات المرضى.');
                    this.state.setLoading(false);
                },
            });
    }

    disable(patient: PatientAccountAdmin): void {
        this.runAction(patient, () => this.adminService.disablePatientAccount(patient.id), 'تم تعطيل الحساب بنجاح');
    }

    restore(patient: PatientAccountAdmin): void {
        this.runAction(patient, () => this.adminService.restorePatientAccount(patient.id), 'تم استعادة الحساب بنجاح');
    }

    delete(patient: PatientAccountAdmin): void {
        if (!confirm(`هل تريد حذف حساب "${patient.fullName}"؟`)) return;
        this.runAction(patient, () => this.adminService.deletePatientAccount(patient.id), 'تم حذف الحساب بنجاح');
    }

    private runAction(
        patient: PatientAccountAdmin,
        action: () => Observable<PatientAccountAdmin>,
        successMessage: string
    ): void {
        this.busyId.set(patient.id);
        action()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.updatePatient(updated);
                    this.notification.success(successMessage);
                    this.busyId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل تنفيذ العملية.');
                    this.busyId.set(null);
                },
            });
    }

    trackByPatientId(index: number, patient: PatientAccountAdmin): string {
        return patient.id;
    }
}
