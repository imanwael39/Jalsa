import { Component, ChangeDetectionStrategy, inject, OnInit, DestroyRef, input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Session } from '../../../../core/models';
import { TableComponent, TableColumn } from '../../../../shared/components/table/table.component';
import { ColumnCellDirective } from '../../../../shared/components/table/column-cell.directive';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
    selector: 'app-session-list',
    standalone: true,
    imports: [TableComponent, ColumnCellDirective, ButtonComponent],
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

    columns: TableColumn[] = [
        { key: 'sessionDate', label: 'Date', sortable: true },
        { key: 'sessionNumber', label: 'Session #', sortable: true },
        { key: 'sessionType', label: 'Type' },
        { key: 'durationMinutes', label: 'Duration' },
        { key: 'status', label: 'Status' },
        { key: 'actions', label: 'Actions', align: 'center' },
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
        this.sessionService.getSessions(this.patientId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (data) => {
                    this.state.setSessions(data);
                    this.state.setLoading(false);
                },
                error: (err) => {
                    this.state.setError(err.message || 'Failed to load sessions');
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

    deleteSession(id: string): void {
        if (confirm('Are you sure you want to delete this session?')) {
            this.sessionService.deleteSession(id)
                .pipe(takeUntilDestroyed(this.destroyRef))
                .subscribe({
                    next: () => {
                        this.state.removeSession(id);
                        this.notification.success('Session deleted successfully');
                    },
                    error: (err) => {
                        this.notification.error(err.message || 'Failed to delete session');
                    },
                });
        }
    }

    formatDate(date: string): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString();
    }

    formatDuration(minutes: number | null): string {
        if (!minutes) return '-';
        return `${minutes} min`;
    }

    trackBySessionId(index: number, session: Session): string {
        return session.id;
    }
}
