import { Component, ChangeDetectionStrategy, inject, signal, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Session } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { Summary } from '../../components/summary/summary';

@Component({
    selector: 'app-session-detail',
    standalone: true,
    imports: [ButtonComponent, SpinnerComponent, Summary],
    templateUrl: './session-detail.html',
    styleUrl: './session-detail.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SessionDetail implements OnInit, OnDestroy {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private sessionService = inject(SessionService);
    private state = inject(SessionStateService);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    session = this.state.selectedSession;
    loading = signal(true);
    error = signal<string | null>(null);

    ngOnDestroy(): void {
        this.state.clearSelected();
    }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.loadSession(id);
        } else {
            this.error.set('Session ID not found');
            this.loading.set(false);
        }
    }

    loadSession(id: string): void {
        this.loading.set(true);
        this.error.set(null);
        this.sessionService.getSession(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (session) => {
                    this.state.selectSession(session);
                    this.loading.set(false);
                },
                error: (err) => {
                    this.error.set(err.message || 'Failed to load session');
                    this.loading.set(false);
                },
            });
    }

    navigateToEdit(): void {
        const s = this.session();
        if (s) {
            this.router.navigate(['/sessions', s.id, 'edit']);
        }
    }

    navigateBack(): void {
        const s = this.session();
        if (s) {
            this.router.navigate(['/sessions/patient', s.patientId]);
        }
    }

    deleteSession(): void {
        const s = this.session();
        if (!s) return;

        this.sessionService.deleteSession(s.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.removeSession(s.id);
                    this.state.clearSelected();
                    this.notification.success('Session deleted successfully');
                    this.router.navigate(['/sessions/patient', s.patientId]);
                },
                error: (err) => {
                    this.notification.error(err.message || 'Failed to delete session');
                },
            });
    }

    formatDate(date: string | null): string {
        if (!date) return 'N/A';
        return new Date(date).toLocaleDateString();
    }
}
