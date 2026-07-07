import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { AdminService } from '../../../../core/services/admin.service';
import { AdminUsersStateService } from '../../../../core/state/admin-users-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { AdminUser } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';

@Component({
    selector: 'app-user-list',
    standalone: true,
    imports: [FormsModule, TableComponent, ColumnCellDirective, ButtonComponent, PaginationComponent],
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserListComponent implements OnInit {
    private adminService = inject(AdminService);
    private state = inject(AdminUsersStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    users = this.state.users;
    totalCount = this.state.totalCount;
    loading = this.state.loading;
    error = this.state.error;
    busyId = signal<string | null>(null);

    searchTerm = '';
    roleFilter = '';
    includeDeleted = false;
    currentPage = 1;
    pageSize = 10;

    readonly availableRoles = ['Therapist', 'Patient', 'Admin'];

    private searchSubject = new Subject<string>();

    columns: TableColumn[] = [
        { key: 'email', label: 'البريد الإلكتروني', sortable: true },
        { key: 'fullName', label: 'الاسم' },
        { key: 'roles', label: 'الأدوار' },
        { key: 'status', label: 'الحالة' },
        { key: 'lastLoginAt', label: 'آخر دخول' },
        { key: 'actions', label: 'إجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        this.loadUsers();

        this.searchSubject
            .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed(this.destroyRef))
            .subscribe(() => {
                this.currentPage = 1;
                this.loadUsers();
            });
    }

    onSearch(term: string): void {
        this.searchTerm = term;
        this.searchSubject.next(term);
    }

    onFilterChange(): void {
        this.currentPage = 1;
        this.loadUsers();
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.loadUsers();
    }

    loadUsers(): void {
        this.state.setLoading(true);
        this.adminService
            .getUsers({
                searchTerm: this.searchTerm || undefined,
                role: this.roleFilter || undefined,
                includeDeleted: this.includeDeleted,
                page: this.currentPage,
                pageSize: this.pageSize,
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: result => {
                    this.state.setUsers(result.items, result.totalCount);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || 'فشل تحميل المستخدمين. يرجى المحاولة مرة أخرى.');
                    this.state.setLoading(false);
                },
            });
    }

    toggleActive(user: AdminUser): void {
        this.busyId.set(user.id);
        this.adminService
            .setUserActive(user.id, !user.isActive)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.updateUser(updated);
                    this.busyId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل تحديث حالة المستخدم.');
                    this.busyId.set(null);
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
                next: updated => {
                    this.state.updateUser(updated);
                    this.busyId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل تغيير دور المستخدم.');
                    this.busyId.set(null);
                },
            });
    }

    unlock(user: AdminUser): void {
        this.busyId.set(user.id);
        this.adminService
            .unlockUser(user.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.updateUser(updated);
                    this.busyId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل إلغاء قفل المستخدم.');
                    this.busyId.set(null);
                },
            });
    }

    softDelete(user: AdminUser): void {
        if (!confirm(`هل تريد حذف حساب "${user.email}"؟`)) return;

        this.busyId.set(user.id);
        this.adminService
            .softDeleteUser(user.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.notification.success('تم حذف الحساب بنجاح');
                    this.busyId.set(null);
                    this.loadUsers();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل حذف الحساب.');
                    this.busyId.set(null);
                },
            });
    }

    restore(user: AdminUser): void {
        this.busyId.set(user.id);
        this.adminService
            .restoreUser(user.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: updated => {
                    this.state.updateUser(updated);
                    this.notification.success('تم استعادة الحساب بنجاح');
                    this.busyId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || 'فشل استعادة الحساب.');
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

    trackByUserId(index: number, user: AdminUser): string {
        return user.id;
    }
}
