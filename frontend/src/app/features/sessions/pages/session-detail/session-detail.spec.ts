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
    content: 'Session content',
    durationMinutes: 50,
    sessionType: 'Individual',
    status: 'Completed',
    createdAt: '2024-01-15T10:00:00Z',
    updatedAt: '2024-01-15T11:00:00Z',
};

interface SetupOptions {
    routeId?: string | null;
    getSessionReturn?: Observable<unknown>;
    getNoteReturn?: Observable<unknown>;
    deleteSessionReturn?: Observable<unknown>;
}

describe('SessionDetail', () => {
    let component: SessionDetail;
    let fixture: ComponentFixture<SessionDetail>;
    let sessionServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { routeId = 'sess-1', getSessionReturn, getNoteReturn, deleteSessionReturn } = opts;

        sessionServiceSpy = {
            getSession: vi.fn().mockReturnValue(getSessionReturn ?? of(mockSession)),
            getSessionNote: vi.fn().mockReturnValue(getNoteReturn ?? of({ observations: 'Note content' })),
            deleteSession: vi.fn().mockReturnValue(deleteSessionReturn ?? of(undefined)),
            getSummary: vi.fn().mockReturnValue(of({ summary: 'AI summary' })),
        };
        stateSpy = {
            selectedSession: signal(mockSession),
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
});
