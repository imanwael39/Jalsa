import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { forkJoin } from 'rxjs';
import { AdminService } from '../../../../core/services/admin.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { AdminUser } from '../../../../core/models';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

const ROLE_NAMES = ['Admin', 'Therapist', 'Patient'] as const;
const ROLE_LABELS: Record<string, string> = {
    Admin: 'مدراء النظام',
    Therapist: 'المعالجون',
    Patient: 'المرضى',
};

interface RoleGroup {
    role: string;
    label: string;
    users: AdminUser[];
    totalCount: number;
}

@Component({
    selector: 'app-roles',
    standalone: true,
    imports: [SpinnerComponent],
    templateUrl: './roles.component.html',
    styleUrl: './roles.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RolesComponent implements OnInit {
    private adminService = inject(AdminService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    readonly availableRoles = [...ROLE_NAMES];

    groups = signal<RoleGroup[]>([]);
    loading = signal(false);
    error = signal<string | null>(null);
    busyId = signal<string | null>(null);

    ngOnInit(): void {
        this.load();
    }

    load(): void {
        this.loading.set(true);
        this.error.set(null);

        forkJoin(ROLE_NAMES.map(role => this.adminService.getUsers({ role, page: 1, pageSize: 50 })))
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: results => {
                    this.groups.set(
                        ROLE_NAMES.map((role, i) => ({
                            role,
                            label: ROLE_LABELS[role],
                            users: results[i].items,
                            totalCount: results[i].totalCount,
                        }))
                    );
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || 'فشل تحميل الأدوار.');
                    this.loading.set(false);
                },
            });
    }

    changeRole(user: AdminUser, roleName: string): void {
        if (!roleName || user.roles[0] === roleName) return;

        this.busyId.set(user.id);
        this.adminService
            .changeUserRole(user.id, roleName)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.notification.success('تم تغيير الدور بنجاح');
                    this.busyId.set(null);
                    this.load();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل تغيير الدور.');
                    this.busyId.set(null);
                },
            });
    }

    trackByUserId(index: number, user: AdminUser): string {
        return user.id;
    }

    trackByRole(index: number, group: RoleGroup): string {
        return group.role;
    }
}
