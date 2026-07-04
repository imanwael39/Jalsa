import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Session } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { QuillEditorComponent } from 'ngx-quill';

@Component({
    selector: 'app-session-form',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        ButtonComponent,
        InputComponent,
        SpinnerComponent,
        QuillEditorComponent,
    ],
    templateUrl: './session-form.html',
    styleUrl: './session-form.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SessionForm implements OnInit {
    private fb = inject(FormBuilder);
    private sessionService = inject(SessionService);
    private state = inject(SessionStateService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private notification = inject(NotificationService);
    private destroyRef = inject(DestroyRef);

    patientId = signal('');
    sessionId = signal<string | null>(null);
    loading = signal(false);
    isEdit = signal(false);
    error = signal<string | null>(null);

    form = this.fb.group({
        sessionDate: [new Date().toISOString().split('T')[0], [Validators.required]],
        content: ['', [Validators.required]],
        sessionType: [''],
        durationMinutes: [null as number | null],
        status: ['Draft'],
    });

    quillConfig = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            ['blockquote', 'code-block'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ size: ['small', false, 'large', 'huge'] }],
            [{ color: [] }, { background: [] }],
            ['link', 'image'],
            ['clean'],
        ],
    };

    ngOnInit(): void {
        const patientId = this.route.snapshot.paramMap.get('patientId');
        if (patientId) {
            this.patientId.set(patientId);
        }
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.sessionId.set(id);
            this.isEdit.set(true);
            this.loadSession(id);
        }
    }

    loadSession(id: string): void {
        this.loading.set(true);
        this.sessionService
            .getSession(id)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: session => {
                    this.form.patchValue({
                        sessionDate: session.sessionDate,
                        sessionType: session.sessionType,
                        durationMinutes: session.durationMinutes,
                        status: session.status,
                    });
                    this.patientId.set(session.patientId);
                    this.loadNote(id);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل في تحميل الجلسة');
                    this.loading.set(false);
                },
            });
    }

    private loadNote(sessionId: string): void {
        this.sessionService
            .getSessionNote(sessionId)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: note => {
                    this.form.patchValue({ content: note.observations });
                    this.loading.set(false);
                },
                error: () => {
                    this.loading.set(false);
                },
            });
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        const formValue = this.form.value;
        const noteContent = formValue.content;

        if (this.isEdit() && this.sessionId()) {
            this.sessionService
                .updateSession(this.sessionId()!, {
                    sessionDate: formValue.sessionDate ?? undefined,
                    sessionType: formValue.sessionType ?? undefined,
                    durationMinutes: formValue.durationMinutes ?? undefined,
                    status: formValue.status ?? undefined,
                })
                .pipe(
                    switchMap(session => this.saveNoteIfNeeded(session, noteContent)),
                    takeUntilDestroyed(this.destroyRef)
                )
                .subscribe({
                    next: session => {
                        this.state.updateSession(session);
                        this.notification.success('تم تحديث الجلسة بنجاح');
                        this.loading.set(false);
                        this.router.navigate(['/sessions', session.id]);
                    },
                    error: (err: HttpErrorResponse) => {
                        this.error.set(err.error?.message || err.error?.error || 'فشل في تحديث الجلسة');
                        this.loading.set(false);
                    },
                });
        } else {
            this.sessionService
                .createSession({
                    patientId: this.patientId(),
                    sessionDate: formValue.sessionDate ?? '',
                    sessionType: formValue.sessionType ?? undefined,
                    durationMinutes: formValue.durationMinutes ?? undefined,
                })
                .pipe(
                    switchMap(session => this.saveNoteIfNeeded(session, noteContent)),
                    takeUntilDestroyed(this.destroyRef)
                )
                .subscribe({
                    next: session => {
                        this.state.addSession(session);
                        this.notification.success('تم إنشاء الجلسة بنجاح');
                        this.loading.set(false);
                        this.router.navigate(['/sessions', session.id]);
                    },
                    error: (err: HttpErrorResponse) => {
                        this.error.set(err.error?.message || err.error?.error || 'فشل في إنشاء الجلسة');
                        this.loading.set(false);
                    },
                });
        }
    }

    private saveNoteIfNeeded(session: Session, content: string | null | undefined) {
        if (!content) return of(session);
        return this.sessionService
            .saveSessionNote(session.id, {
                observations: content,
            })
            .pipe(switchMap(() => of(session)));
    }

    cancel(): void {
        this.router.navigate(['/sessions/patient', this.patientId()]);
    }
}
