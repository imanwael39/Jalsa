import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { AdminService } from '../../../../core/services/admin.service';
import { AdminDoctorsStateService } from '../../../../core/state/admin-doctors-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { TherapistAdmin } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';

const STATUS_LABELS: Record<string, string> = {
    Pending: 'قيد المراجعة',
    Approved: 'معتمد',
    Rejected: 'مرفوض',
    Suspended: 'موقوف',
};

@Component({
    selector: 'app-doctor-list',
    standalone: true,
    imports: [FormsModule, TableComponent, ColumnCellDirective, ButtonComponent, PaginationComponent],
    templateUrl: './doctor-list.component.html',
    styleUrl: './doctor-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DoctorListComponent implements OnInit {
    private adminService = inject(AdminService);
    private state = inject(AdminDoctorsStateService);
    private notification = inject(NotificationService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    doctors = this.state.doctors;
    totalCount = this.state.totalCount;
    loading = this.state.loading;
    error = this.state.error;
    busyId = signal<string | null>(null);

    searchTerm = '';
    statusFilter = '';
    currentPage = 1;
    pageSize = 10;

    readonly statusOptions = Object.keys(STATUS_LABELS);

    private searchSubject = new Subject<string>();

    columns: TableColumn[] = [
        { key: 'fullName', label: 'الاسم' },
        { key: 'email', label: 'البريد الإلكتروني' },
        { key: 'licenseNumber', label: 'رقم الترخيص' },
        { key: 'patientCount', label: 'عدد المرضى', align: 'center' },
        { key: 'approvalStatus', label: 'حالة الاعتماد' },
        { key: 'actions', label: 'إجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        this.loadDoctors();

        this.searchSubject
            .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed(this.destroyRef))
            .subscribe(() => {
                this.currentPage = 1;
                this.loadDoctors();
            });
    }

    onSearch(term: string): void {
        this.searchTerm = term;
        this.searchSubject.next(term);
    }

    onFilterChange(): void {
        this.currentPage = 1;
        this.loadDoctors();
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.loadDoctors();
    }

    loadDoctors(): void {
        this.state.setLoading(true);
        this.adminService
            .getDoctors({
                searchTerm: this.searchTerm || undefined,
                approvalStatus: this.statusFilter || undefined,
                page: this.currentPage,
                pageSize: this.pageSize,
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.state.setDoctors(result.items, result.totalCount);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || 'فشل تحميل قائمة المعالجين.');
                    this.state.setLoading(false);
                },
            });
    }

    updateStatus(doctor: TherapistAdmin, newStatus: string): void {
        this.busyId.set(doctor.id);
        this.adminService
            .updateDoctorStatus(doctor.id, newStatus)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.updateDoctor(updated);
                    this.notification.success('تم تحديث حالة المعالج بنجاح');
                    this.busyId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل تحديث حالة المعالج.');
                    this.busyId.set(null);
                },
            });
    }

    statusLabel(status: string): string {
        return STATUS_LABELS[status] ?? status;
    }

    viewDetail(doctor: TherapistAdmin): void {
        this.router.navigate(['/admin/doctors', doctor.id]);
    }

    trackByDoctorId(index: number, doctor: TherapistAdmin): string {
        return doctor.id;
    }
}
