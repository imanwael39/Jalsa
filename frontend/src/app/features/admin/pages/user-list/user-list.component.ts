import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpClientService } from '../../../../core/api/http-client.service';
import { API } from '../../../../core/api/api-endpoints';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { AdminUser } from '../../../../core/models';

@Component({
    selector: 'app-user-list',
    standalone: true,
    imports: [SpinnerComponent, EmptyStateComponent],
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserListComponent implements OnInit {
    private http = inject(HttpClientService);
    private destroyRef = inject(DestroyRef);

    users = signal<AdminUser[]>([]);
    loading = signal<boolean>(false);
    error = signal<string | null>(null);
    busyId = signal<string | null>(null);

    ngOnInit(): void {
        this.loadUsers();
    }

    loadUsers(): void {
        this.loading.set(true);
        this.error.set(null);

        this.http
            .get<AdminUser[]>(API.admin.users)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.users.set(data);
                    this.loading.set(false);
                },
                error: () => {
                    this.error.set('فشل تحميل المستخدمين. يرجى المحاولة مرة أخرى.');
                    this.loading.set(false);
                },
            });
    }

    toggleActive(user: AdminUser): void {
        this.busyId.set(user.id);
        this.http
            .patch<AdminUser>(API.admin.status(user.id), !user.isActive)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.users.update(list => list.map(u => (u.id === updated.id ? updated : u)));
                    this.busyId.set(null);
                },
                error: () => {
                    this.error.set('فشل تحديث حالة المستخدم.');
                    this.busyId.set(null);
                },
            });
    }

    readonly availableRoles = ['Therapist', 'Patient', 'Admin'];

    changeRole(user: AdminUser, roleName: string): void {
        if (!roleName || user.roles[0] === roleName) return;

        this.busyId.set(user.id);
        this.http
            .patch<AdminUser>(API.admin.role(user.id), { roleName })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.users.update(list => list.map(u => (u.id === updated.id ? updated : u)));
                    this.busyId.set(null);
                },
                error: () => {
                    this.error.set('فشل تغيير دور المستخدم.');
                    this.busyId.set(null);
                },
            });
    }

    unlock(user: AdminUser): void {
        this.busyId.set(user.id);
        this.http
            .patch<AdminUser>(API.admin.unlock(user.id), {})
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.users.update(list => list.map(u => (u.id === updated.id ? updated : u)));
                    this.busyId.set(null);
                },
                error: () => {
                    this.error.set('فشل إلغاء قفل المستخدم.');
                    this.busyId.set(null);
                },
            });
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
}
