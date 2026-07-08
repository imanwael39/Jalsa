import { Component, ChangeDetectionStrategy, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientSessionService } from '../../../../core/services/patient-session.service';
import { PatientSessionDetail } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { TextareaComponent } from '../../../../shared/components/textarea/textarea.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

type RequestKind = 'Reschedule' | 'Cancel';

@Component({
    selector: 'app-appointment-detail',
    standalone: true,
    imports: [ReactiveFormsModule, ButtonComponent, ModalComponent, TextareaComponent, StatusArPipe],
    templateUrl: './appointment-detail.component.html',
    styleUrl: './appointment-detail.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppointmentDetailComponent implements OnInit {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private patientSessionService = inject(PatientSessionService);
    private fb = inject(NonNullableFormBuilder);
    private destroyRef = inject(DestroyRef);

    session = signal<PatientSessionDetail | null>(null);
    loading = signal(false);
    error = signal<string | null>(null);
    submitting = signal(false);

    showModal = signal(false);
    activeRequestKind = signal<RequestKind | null>(null);

    requestForm = this.fb.group({
        note: ['', [Validators.required, Validators.minLength(5)]],
    });

    private sessionId = '';

    ngOnInit(): void {
        this.sessionId = this.route.snapshot.paramMap.get('id') ?? '';
        this.loadSession();
    }

    loadSession(): void {
        this.loading.set(true);
        this.error.set(null);
        this.patientSessionService
            .getSession(this.sessionId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: session => {
                    this.session.set(session);
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.error || err.error?.message || 'فشل تحميل تفاصيل الموعد');
                    this.loading.set(false);
                },
            });
    }

    openRequestModal(kind: RequestKind): void {
        this.activeRequestKind.set(kind);
        this.requestForm.reset({ note: '' });
        this.showModal.set(true);
    }

    closeModal(): void {
        this.showModal.set(false);
        this.activeRequestKind.set(null);
    }

    submitRequest(): void {
        if (this.requestForm.invalid) {
            this.requestForm.markAllAsTouched();
            return;
        }

        const kind = this.activeRequestKind();
        if (!kind) return;

        const note = this.requestForm.getRawValue().note;
        this.submitting.set(true);

        const request$ =
            kind === 'Reschedule'
                ? this.patientSessionService.requestReschedule(this.sessionId, { note })
                : this.patientSessionService.requestCancel(this.sessionId, { note });

        request$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
            next: () => {
                this.submitting.set(false);
                this.closeModal();
                this.loadSession();
            },
            error: (err: HttpErrorResponse) => {
                this.submitting.set(false);
                this.error.set(err.error?.error || err.error?.message || 'فشل إرسال الطلب');
            },
        });
    }

    goBack(): void {
        this.router.navigate(['/appointments']);
    }
}
