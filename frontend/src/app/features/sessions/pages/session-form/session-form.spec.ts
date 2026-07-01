import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { Observable, of, throwError } from 'rxjs';
import { SessionForm } from './session-form';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Session } from '../../../../core/models';

const mockSession: Session = {
    id: 'sess-1',
    patientId: 'pat-1',
    intakeFormId: null,
    sessionNumber: 1,
    sessionDate: '2024-01-15T10:00:00Z',
    content: 'Session content',
    durationMinutes: 50,
    sessionType: 'Individual',
    status: 'Draft',
    voiceMemoUrl: null,
    createdAt: '2024-01-15T10:00:00Z',
    updatedAt: '2024-01-15T11:00:00Z',
};

interface SetupOptions {
    routePatientId?: string | null;
    routeId?: string | null;
    createSessionReturn?: Observable<unknown>;
    updateSessionReturn?: Observable<unknown>;
    getSessionReturn?: Observable<unknown>;
    saveNoteReturn?: Observable<unknown>;
}

describe('SessionForm', () => {
    let component: SessionForm;
    let fixture: ComponentFixture<SessionForm>;
    let sessionServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const {
            routePatientId = 'pat-1',
            routeId = null,
            createSessionReturn,
            updateSessionReturn,
            getSessionReturn,
            saveNoteReturn,
        } = opts;

        sessionServiceSpy = {
            getSession: vi.fn().mockReturnValue(getSessionReturn ?? of(mockSession)),
            createSession: vi.fn().mockReturnValue(createSessionReturn ?? of(mockSession)),
            updateSession: vi.fn().mockReturnValue(updateSessionReturn ?? of(mockSession)),
            getSessionNote: vi.fn().mockReturnValue(of({ observations: null })),
            saveSessionNote: vi.fn().mockReturnValue(saveNoteReturn ?? of({ observations: 'note' })),
        };
        stateSpy = {
            addSession: vi.fn(),
            updateSession: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule],
            providers: [
                provideRouter([]),
                { provide: SessionService, useValue: sessionServiceSpy },
                { provide: SessionStateService, useValue: stateSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            paramMap: {
                                get: (key: string): string | null => {
                                    if (key === 'patientId') return routePatientId;
                                    if (key === 'id') return routeId;
                                    return null;
                                },
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(SessionForm);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should initialize in create mode', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isEdit()).toBe(false);
        expect(component.sessionId()).toBeNull();
    });

    it('should initialize in edit mode with session ID', (): void => {
        setup({ routeId: 'sess-1' });
        fixture.detectChanges();
        expect(component.isEdit()).toBe(true);
        expect(component.sessionId()).toBe('sess-1');
    });

    it('should load session data in edit mode', (): void => {
        setup({ routeId: 'sess-1' });
        fixture.detectChanges();
        expect(sessionServiceSpy['getSession']).toHaveBeenCalledWith('sess-1');
    });

    it('should submit create form successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            sessionDate: '2024-02-01',
            content: 'New session content',
        });
        component.onSubmit();
        expect(sessionServiceSpy['createSession']).toHaveBeenCalled();
        expect(stateSpy['addSession']).toHaveBeenCalledWith(mockSession);
        expect(notificationSpy['success']).toHaveBeenCalled();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions', mockSession.id]);
    });

    it('should submit edit form successfully', (): void => {
        setup({ routeId: 'sess-1' });
        fixture.detectChanges();
        component.form.patchValue({ sessionDate: '2024-02-01', content: 'Updated' });
        component.onSubmit();
        expect(sessionServiceSpy['updateSession']).toHaveBeenCalled();
        expect(stateSpy['updateSession']).toHaveBeenCalledWith(mockSession);
        expect(notificationSpy['success']).toHaveBeenCalled();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions', mockSession.id]);
    });

    it('should handle create error', (): void => {
        setup({ createSessionReturn: throwError(() => ({ error: { message: 'Create failed' } })) });
        fixture.detectChanges();
        component.form.patchValue({ sessionDate: '2024-02-01', content: 'test' });
        component.onSubmit();
        expect(component.error()).toBe('Create failed');
    });

    it('should handle update error', (): void => {
        setup({ routeId: 'sess-1', updateSessionReturn: throwError(() => ({ error: { message: 'Update failed' } })) });
        fixture.detectChanges();
        component.form.patchValue({ sessionDate: '2024-02-01', content: 'test' });
        component.onSubmit();
        expect(component.error()).toBe('Update failed');
    });

    it('should not submit when form is invalid', (): void => {
        setup();
        fixture.detectChanges();
        component.form.get('sessionDate')?.setValue('');
        component.onSubmit();
        expect(sessionServiceSpy['createSession']).not.toHaveBeenCalled();
        expect(sessionServiceSpy['updateSession']).not.toHaveBeenCalled();
    });

    it('should navigate to cancel', (): void => {
        setup();
        fixture.detectChanges();
        component.cancel();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions/patient', 'pat-1']);
    });

    it('should handle voice upload', (): void => {
        setup();
        fixture.detectChanges();
        component.onVoiceUploaded('http://example.com/voice.mp3');
        expect(component.voiceMemoUrl()).toBe('http://example.com/voice.mp3');
    });
});
