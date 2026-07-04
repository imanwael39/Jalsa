import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { SessionList } from './session-list';
import { SessionService } from '../../../../core/services/session.service';
import { SessionStateService } from '../../../../core/state/session-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Session } from '../../../../core/models';

const mockSessions: Session[] = [
    {
        id: 'sess-1',
        patientId: 'pat-1',
        intakeFormId: null,
        sessionNumber: 1,
        sessionDate: '2024-01-15T10:00:00Z',
        content: 'First session',
        durationMinutes: 50,
        sessionType: 'Individual',
        status: 'Completed',
        createdAt: '2024-01-15T10:00:00Z',
        updatedAt: '2024-01-15T11:00:00Z',
    },
    {
        id: 'sess-2',
        patientId: 'pat-1',
        intakeFormId: null,
        sessionNumber: 2,
        sessionDate: '2024-01-22T10:00:00Z',
        content: 'Second session',
        durationMinutes: 45,
        sessionType: 'Individual',
        status: 'Scheduled',
        createdAt: '2024-01-22T10:00:00Z',
        updatedAt: '2024-01-22T11:00:00Z',
    },
];

interface SetupOptions {
    patientId?: string | null;
    routePatientId?: string | null;
    getSessionsReturn?: Observable<unknown>;
    deleteSessionReturn?: Observable<unknown>;
}

describe('SessionList', () => {
    let component: SessionList;
    let fixture: ComponentFixture<SessionList>;
    let sessionServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { patientId = null, routePatientId = 'pat-1', getSessionsReturn, deleteSessionReturn } = opts;

        sessionServiceSpy = {
            getSessions: vi.fn().mockReturnValue(getSessionsReturn ?? of(mockSessions)),
            deleteSession: vi.fn().mockReturnValue(deleteSessionReturn ?? of(undefined)),
        };
        stateSpy = {
            sessions: signal(mockSessions),
            loading: signal(false),
            error: signal(null),
            setSessions: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
            removeSession: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
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
                                get: (key: string): string | null => (key === 'patientId' ? routePatientId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(SessionList);
        component = fixture.componentInstance;
        if (patientId !== null) {
            fixture.componentRef.setInput('patientIdInput', patientId);
        }
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load sessions from route param on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(sessionServiceSpy['getSessions']).toHaveBeenCalledWith('pat-1');
    });

    it('should load sessions from input when route param absent', (): void => {
        setup({ routePatientId: null, patientId: 'pat-2' });
        fixture.detectChanges();
        expect(sessionServiceSpy['getSessions']).toHaveBeenCalledWith('pat-2');
    });

    it('should not load when no patient ID available', (): void => {
        setup({ routePatientId: null, patientId: null });
        fixture.detectChanges();
        expect(sessionServiceSpy['getSessions']).not.toHaveBeenCalled();
    });

    it('should handle load error', (): void => {
        setup({ getSessionsReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('should navigate to session detail on row click', (): void => {
        setup();
        fixture.detectChanges();
        component.onRowClick(mockSessions[0]);
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions', 'sess-1']);
    });

    it('should navigate to new session form', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToNew();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions/new', 'pat-1']);
    });

    it('should navigate to edit session', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToEdit('sess-1');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions', 'sess-1', 'edit']);
    });

    it('should navigate to view session', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToView('sess-1');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/sessions', 'sess-1']);
    });

    it('should delete session with confirmation', (): void => {
        const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
        setup();
        fixture.detectChanges();
        component.deleteSession('sess-1');
        expect(sessionServiceSpy['deleteSession']).toHaveBeenCalledWith('sess-1');
        expect(stateSpy['removeSession']).toHaveBeenCalledWith('sess-1');
        expect(notificationSpy['success']).toHaveBeenCalled();
        confirmSpy.mockRestore();
    });

    it('should not delete when confirmation is cancelled', (): void => {
        const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
        setup();
        fixture.detectChanges();
        component.deleteSession('sess-1');
        expect(sessionServiceSpy['deleteSession']).not.toHaveBeenCalled();
        confirmSpy.mockRestore();
    });

    it('should format duration', (): void => {
        setup();
        expect(component.formatDuration(50)).toBe('50 دقيقة');
        expect(component.formatDuration(null)).toBe('-');
    });

    it('should format date', (): void => {
        setup();
        expect(component.formatDate('2024-01-15T10:00:00Z')).toBeTruthy();
        expect(component.formatDate(null)).toBe('-');
    });

    it('should track by session id', (): void => {
        setup();
        expect(component.trackBySessionId(0, mockSessions[0])).toBe('sess-1');
    });
});
