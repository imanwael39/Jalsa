import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AdminService } from '../../../../core/services/admin.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { TherapistAdminDetail } from '../../../../core/models';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

const STATUS_LABELS: Record<string, string> = {
    Pending: 'قيد المراجعة',
    Approved: 'معتمد',
    Rejected: 'مرفوض',
    Suspended: 'موقوف',
};

@Component({
    selector: 'app-doctor-detail',
    standalone: true,
    imports: [SpinnerComponent, ButtonComponent],
    templateUrl: './doctor-detail.component.html',
    styleUrl: './doctor-detail.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DoctorDetailComponent implements OnInit {
    private adminService = inject(AdminService);
    private notification = inject(NotificationService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    doctor = signal<TherapistAdminDetail | null>(null);
    loading = signal(false);
    error = signal<string | null>(null);
    busy = signal(false);

    private doctorId = '';

    ngOnInit(): void {
        this.doctorId = this.route.snapshot.paramMap.get('id') ?? '';
        this.loadDoctor();
    }

    loadDoctor(): void {
        this.loading.set(true);
        this.adminService
            .getDoctorDetail(this.doctorId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: doctor => {
                    this.doctor.set(doctor);
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || 'فشل تحميل بيانات المعالج.');
                    this.loading.set(false);
                },
            });
    }

    updateStatus(newStatus: string): void {
        this.busy.set(true);
        this.adminService
            .updateDoctorStatus(this.doctorId, newStatus)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.notification.success('تم تحديث حالة المعالج بنجاح');
                    this.busy.set(false);
                    this.loadDoctor();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل تحديث حالة المعالج.');
                    this.busy.set(false);
                },
            });
    }

    statusLabel(status: string): string {
        return STATUS_LABELS[status] ?? status;
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString('ar-EG', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        });
    }

    goBack(): void {
        this.router.navigate(['/admin/doctors']);
    }
}
