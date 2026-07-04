import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef, input } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Session } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-session-list',
    standalone: true,
    imports: [TableComponent, ColumnCellDirective, ButtonComponent, ModalComponent, StatusArPipe],
    templateUrl: './session-list.html',
    styleUrl: './session-list.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SessionList implements OnInit {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private sessionService = inject(SessionService);
    private state = inject(SessionStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    patientIdInput = input<string | null>(null);

    patientId = '';
    sessions = this.state.sessions;
    loading = this.state.loading;
    error = this.state.error;

    showDeleteModal = signal(false);
    deletingSessionId = signal<string | null>(null);

    columns: TableColumn[] = [
        { key: 'sessionDate', label: 'التاريخ', sortable: true },
        { key: 'sessionNumber', label: 'رقم الجلسة', sortable: true },
        { key: 'sessionType', label: 'النوع' },
        { key: 'durationMinutes', label: 'المدة' },
        { key: 'status', label: 'الحالة' },
        { key: 'actions', label: 'الإجراءات', align: 'center' },
    ];

    ngOnInit(): void {
        const inputPatientId = this.patientIdInput();
        if (inputPatientId) {
            this.patientId = inputPatientId;
            this.loadSessions();
        } else {
            this.patientId = this.route.snapshot.paramMap.get('patientId') || '';
            if (this.patientId) {
                this.loadSessions();
            }
        }
    }

    loadSessions(): void {
        this.state.setLoading(true);
        this.sessionService
            .getSessions(this.patientId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setSessions(data);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || err.error?.error || 'فشل تحميل الجلسات');
                    this.state.setLoading(false);
                },
            });
    }

    onRowClick(session: Session): void {
        this.router.navigate(['/sessions', session.id]);
    }

    navigateToNew(): void {
        const pid = this.patientIdInput() || this.patientId;
        if (pid) {
            this.router.navigate(['/sessions/new', pid]);
        }
    }

    navigateToView(id: string): void {
        this.router.navigate(['/sessions', id]);
    }

    navigateToEdit(id: string): void {
        this.router.navigate(['/sessions', id, 'edit']);
    }

    openDeleteModal(id: string): void {
        this.deletingSessionId.set(id);
        this.showDeleteModal.set(true);
    }

    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
        this.deletingSessionId.set(null);
    }

    deleteSession(): void {
        const id = this.deletingSessionId();
        if (!id) return;

        this.sessionService
            .deleteSession(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.removeSession(id);
                    this.notification.success('تم حذف الجلسة بنجاح');
                    this.closeDeleteModal();
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل حذف الجلسة');
                },
            });
    }

    formatDate(date: string): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString();
    }

    formatDuration(minutes: number | null): string {
        if (!minutes) return '-';
        return `${minutes} دقيقة`;
    }

    trackBySessionId(index: number, session: Session): string {
        return session.id;
    }
}
