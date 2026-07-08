import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { SessionDetail } from './session-detail';
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
    durationMinutes: 50,
    sessionType: 'Individual',
    status: 'Completed',
    patientRequestType: null,
    patientRequestNote: null,
    patientRequestStatus: null,
    patientRequestedAt: null,
    createdAt: '2024-01-15T10:00:00Z',
    updatedAt: '2024-01-15T11:00:00Z',
};

const mockSessionWithPendingReschedule: Session = {
    ...mockSession,
    status: 'Scheduled',
    patientRequestType: 'Reschedule',
    patientRequestNote: 'أحتاج موعداً آخر',
    patientRequestStatus: 'Pending',
    patientRequestedAt: '2026-07-01T10:00:00Z',
};

interface SetupOptions {
    routeId?: string | null;
    getSessionReturn?: Observable<unknown>;
    getNoteReturn?: Observable<unknown>;
    deleteSessionReturn?: Observable<unknown>;
    selectedSession?: Session;
    approvePatientRequestReturn?: Observable<unknown>;
    rejectPatientRequestReturn?: Observable<unknown>;
}

describe('SessionDetail', () => {
    let component: SessionDetail;
    let fixture: ComponentFixture<SessionDetail>;
    let sessionServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const {
            routeId = 'sess-1',
            getSessionReturn,
            getNoteReturn,
            deleteSessionReturn,
            selectedSession = mockSession,
            approvePatientRequestReturn,
            rejectPatientRequestReturn,
        } = opts;

        sessionServiceSpy = {
            getSession: vi.fn().mockReturnValue(getSessionReturn ?? of(mockSession)),
            getSessionNote: vi.fn().mockReturnValue(getNoteReturn ?? of({ observations: 'Note content' })),
            deleteSession: vi.fn().mockReturnValue(deleteSessionReturn ?? of(undefined)),
            getSummary: vi.fn().mockReturnValue(of({ summary: 'AI summary' })),
            approvePatientRequest: vi.fn().mockReturnValue(approvePatientRequestReturn ?? of(mockSession)),
            rejectPatientRequest: vi.fn().mockReturnValue(rejectPatientRequestReturn ?? of(mockSession)),
        };
        stateSpy = {
            selectedSession: signal(selectedSession),
            selectSession: vi.fn(),
            removeSession: vi.fn(),
            clearSelected: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
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
                                get: (key: string): string | null => (key === 'id' ? routeId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(SessionDetail);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load session on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(sessionServiceSpy['getSession']).toHaveBeenCalledWith('sess-1');
        expect(stateSpy['selectSession']).toHaveBeenCalledWith(expect.objectContaining({ id: 'sess-1' }));
    });

    it('should set error when session ID not found', (): void => {
        setup({ routeId: null });
        fixture.detectChanges();
        expect(component.error()).toBe('معرف الجلسة غير موجود');
    });

    it('should set error when API call fails', (): void => {
        setup({ getSessionReturn: throwError(() => ({ error: { message: 'API Error' } })) });
        fixture.detectChanges();
        expect(component.error()).toBe('API Error');
    });

    it('should navigate to edit', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToEdit();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions', 'sess-1', 'edit']);
    });

    it('should navigate back', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions/patient', 'pat-1']);
    });

    it('should delete session', (): void => {
        setup();
        fixture.detectChanges();
        component.deleteSession();
        expect(sessionServiceSpy['deleteSession']).toHaveBeenCalledWith('sess-1');
        expect(stateSpy['removeSession']).toHaveBeenCalled();
        expect(stateSpy['clearSelected']).toHaveBeenCalled();
        expect(notificationSpy['success']).toHaveBeenCalled();
    });

    it('should require the delete modal to be opened before showing the confirmation', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.showDeleteModal()).toBe(false);
        component.openDeleteModal();
        expect(component.showDeleteModal()).toBe(true);
        component.closeDeleteModal();
        expect(component.showDeleteModal()).toBe(false);
    });

    it('should format date', (): void => {
        setup();
        expect(component.formatDate('2024-01-15T10:00:00Z')).toBeTruthy();
        expect(component.formatDate(null)).toBe('غير متوفر');
    });

    it('should clear selected on destroy', (): void => {
        setup();
        fixture.detectChanges();
        fixture.destroy();
        expect(stateSpy['clearSelected']).toHaveBeenCalled();
    });

    it('should open and close the patient request action modal', (): void => {
        setup({ selectedSession: mockSessionWithPendingReschedule });
        fixture.detectChanges();

        component.openRequestActionModal('approve');
        expect(component.requestAction()).toBe('approve');

        component.closeRequestActionModal();
        expect(component.requestAction()).toBeNull();
    });

    it('should require a new session date before approving a reschedule request', (): void => {
        setup({ selectedSession: mockSessionWithPendingReschedule });
        fixture.detectChanges();
        component.openRequestActionModal('approve');

        component.confirmRequestAction();

        expect(sessionServiceSpy['approvePatientRequest']).not.toHaveBeenCalled();
    });

    it('should approve a reschedule request with the chosen date', (): void => {
        setup({ selectedSession: mockSessionWithPendingReschedule });
        fixture.detectChanges();
        component.openRequestActionModal('approve');
        component.newSessionDate.set('2026-08-01');

        component.confirmRequestAction();

        expect(sessionServiceSpy['approvePatientRequest']).toHaveBeenCalledWith('sess-1', '2026-08-01');
        expect(component.requestAction()).toBeNull();
        expect(notificationSpy['success']).toHaveBeenCalled();
    });

    it('should approve a cancel request without requiring a date', (): void => {
        const cancelSession: Session = {
            ...mockSessionWithPendingReschedule,
            patientRequestType: 'Cancel',
        };
        setup({ selectedSession: cancelSession });
        fixture.detectChanges();
        component.openRequestActionModal('approve');

        component.confirmRequestAction();

        expect(sessionServiceSpy['approvePatientRequest']).toHaveBeenCalledWith('sess-1', undefined);
    });

    it('should reject a pending patient request', (): void => {
        setup({ selectedSession: mockSessionWithPendingReschedule });
        fixture.detectChanges();
        component.openRequestActionModal('reject');

        component.confirmRequestAction();

        expect(sessionServiceSpy['rejectPatientRequest']).toHaveBeenCalledWith('sess-1');
        expect(notificationSpy['success']).toHaveBeenCalled();
    });

    it('should surface an error when the request action fails', (): void => {
        setup({
            selectedSession: mockSessionWithPendingReschedule,
            rejectPatientRequestReturn: throwError(() => ({ error: { error: 'فشل تنفيذ الإجراء' } })),
        });
        fixture.detectChanges();
        component.openRequestActionModal('reject');

        component.confirmRequestAction();

        expect(notificationSpy['error']).toHaveBeenCalledWith('فشل تنفيذ الإجراء');
        expect(component.requestActionSubmitting()).toBe(false);
    });
});
