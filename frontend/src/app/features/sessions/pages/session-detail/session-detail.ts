import { Component, ChangeDetectionStrategy, inject, signal, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { SessionNote } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { Summary } from '../../components/summary/summary';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

type PatientRequestAction = 'approve' | 'reject';

@Component({
    selector: 'app-session-detail',
    standalone: true,
    imports: [FormsModule, ButtonComponent, SpinnerComponent, ModalComponent, Summary, StatusArPipe],
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
    note = signal<SessionNote | null>(null);
    noteLoading = signal(false);
    loading = signal(true);
    error = signal<string | null>(null);
    showDeleteModal = signal(false);

    requestAction = signal<PatientRequestAction | null>(null);
    requestActionSubmitting = signal(false);
    newSessionDate = signal('');

    ngOnDestroy(): void {
        this.state.clearSelected();
    }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.loadSession(id);
        } else {
            this.error.set('معرف الجلسة غير موجود');
            this.loading.set(false);
        }
    }

    loadSession(id: string): void {
        this.loading.set(true);
        this.error.set(null);
        this.sessionService
            .getSession(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: session => {
                    this.state.selectSession(session);
                    this.loading.set(false);
                    this.loadNote(id);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل في تحميل الجلسة');
                    this.loading.set(false);
                },
            });
    }

    private loadNote(sessionId: string): void {
        this.noteLoading.set(true);
        this.sessionService
            .getSessionNote(sessionId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: note => {
                    this.note.set(note);
                    this.noteLoading.set(false);
                },
                error: () => {
                    this.noteLoading.set(false);
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

    openDeleteModal(): void {
        this.showDeleteModal.set(true);
    }

    closeDeleteModal(): void {
        this.showDeleteModal.set(false);
    }

    deleteSession(): void {
        const s = this.session();
        if (!s) return;

        this.sessionService
            .deleteSession(s.id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: () => {
                    this.state.removeSession(s.id);
                    this.state.clearSelected();
                    this.notification.success('تم حذف الجلسة بنجاح');
                    this.router.navigate(['/sessions/patient', s.patientId]);
                },
                error: (err: HttpErrorResponse) => {
                    this.notification.error(err.error?.message || err.error?.error || 'فشل حذف الجلسة');
                    this.closeDeleteModal();
                },
            });
    }

    formatDate(date: string | null): string {
        if (!date) return 'غير متوفر';
        return new Date(date).toLocaleDateString();
    }

    openRequestActionModal(action: PatientRequestAction): void {
        this.requestAction.set(action);
        this.newSessionDate.set('');
    }

    closeRequestActionModal(): void {
        this.requestAction.set(null);
        this.newSessionDate.set('');
    }

    confirmRequestAction(): void {
        const s = this.session();
        const action = this.requestAction();
        if (!s || !action) return;

        if (action === 'approve' && s.patientRequestType === 'Reschedule' && !this.newSessionDate()) {
            this.notification.error('يرجى تحديد الموعد الجديد للجلسة');
            return;
        }

        this.requestActionSubmitting.set(true);
        const request$ =
            action === 'approve'
                ? this.sessionService.approvePatientRequest(
                      s.id,
                      s.patientRequestType === 'Reschedule' ? this.newSessionDate() : undefined
                  )
                : this.sessionService.rejectPatientRequest(s.id);

        request$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
            next: () => {
                this.requestActionSubmitting.set(false);
                this.closeRequestActionModal();
                this.notification.success(action === 'approve' ? 'تم قبول طلب المريض' : 'تم رفض طلب المريض');
                this.loadSession(s.id);
            },
            error: (err: HttpErrorResponse) => {
                this.requestActionSubmitting.set(false);
                this.notification.error(err.error?.message || err.error?.error || 'فشل تنفيذ الإجراء');
            },
        });
    }
}
