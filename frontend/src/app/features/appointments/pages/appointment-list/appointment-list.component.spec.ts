import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA, signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { AppointmentListComponent } from './appointment-list.component';
import { PatientSessionService } from '../../../../core/services/patient-session.service';
import { PatientSessionStateService } from '../../../../core/state/patient-session-state.service';
import { PatientSessionSummary } from '../../../../core/models';

const mockSessions: PatientSessionSummary[] = [
    {
        id: 'sess-past',
        sessionNumber: 1,
        sessionDate: '2026-07-01',
        durationMinutes: 50,
        sessionType: 'فردية',
        status: 'Completed',
        patientRequestType: null,
        patientRequestStatus: null,
    },
    {
        id: 'sess-future',
        sessionNumber: 2,
        sessionDate: '2026-07-15',
        durationMinutes: 50,
        sessionType: 'فردية',
        status: 'Scheduled',
        patientRequestType: null,
        patientRequestStatus: null,
    },
    {
        id: 'sess-future-pending',
        sessionNumber: 3,
        sessionDate: '2026-07-20',
        durationMinutes: 45,
        sessionType: 'متابعة',
        status: 'Scheduled',
        patientRequestType: 'Reschedule',
        patientRequestStatus: 'Pending',
    },
];

interface SetupOptions {
    getSessionsReturn?: Observable<unknown>;
}

describe('AppointmentListComponent', () => {
    let component: AppointmentListComponent;
    let fixture: ComponentFixture<AppointmentListComponent>;
    let patientSessionServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    beforeEach(() => {
        vi.useFakeTimers();
        vi.setSystemTime(new Date('2026-07-08T09:00:00Z'));
    });

    afterEach(() => {
        vi.useRealTimers();
    });

    const setup = (opts: SetupOptions = {}): void => {
        const { getSessionsReturn } = opts;

        patientSessionServiceSpy = {
            getSessions: vi.fn().mockReturnValue(getSessionsReturn ?? of(mockSessions)),
        };
        stateSpy = {
            sessions: signal(mockSessions),
            selectedSession: signal(null),
            loading: signal(false),
            error: signal(null),
            setSessions: vi.fn(),
            setSelectedSession: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [
                provideRouter([]),
                { provide: PatientSessionService, useValue: patientSessionServiceSpy },
                { provide: PatientSessionStateService, useValue: stateSpy },
                { provide: Router, useValue: routerSpy },
            ],
        });

        fixture = TestBed.createComponent(AppointmentListComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load sessions on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientSessionServiceSpy['getSessions']).toHaveBeenCalled();
        expect(stateSpy['setSessions']).toHaveBeenCalledWith(mockSessions);
    });

    it('should handle load error', (): void => {
        setup({ getSessionsReturn: throwError(() => ({ error: { error: 'فشل' } })) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalledWith('فشل');
    });

    it('should split sessions into upcoming and past relative to today', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.upcomingSessions().map(s => s.id)).toEqual(['sess-future', 'sess-future-pending']);
        expect(component.pastSessions().map(s => s.id)).toEqual(['sess-past']);
    });

    it('should default to list view and toggle to calendar view', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.viewMode()).toBe('list');
        component.setViewMode('calendar');
        expect(component.viewMode()).toBe('calendar');
    });

    it('should navigate to session detail when a session is opened', (): void => {
        setup();
        fixture.detectChanges();
        component.openSession('sess-future');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/appointments', 'sess-future']);
    });

    it('should group sessions by date in the calendar grid', (): void => {
        setup();
        fixture.detectChanges();
        const dayWithSession = component.calendarDays().find(d => d.dateKey === '2026-07-15');
        expect(dayWithSession?.sessions.map(s => s.id)).toEqual(['sess-future']);
    });

    it('should mark today in the calendar grid', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isToday('2026-07-08')).toBe(true);
        expect(component.isToday('2026-07-09')).toBe(false);
    });

    it('should navigate to the previous and next month', (): void => {
        setup();
        fixture.detectChanges();
        const initialMonth = component.currentMonth().getMonth();
        component.goToNextMonth();
        expect(component.currentMonth().getMonth()).toBe((initialMonth + 1) % 12);
        component.goToPreviousMonth();
        expect(component.currentMonth().getMonth()).toBe(initialMonth);
    });

    it('should reset to the current month on goToToday', (): void => {
        setup();
        fixture.detectChanges();
        component.goToNextMonth();
        component.goToNextMonth();
        component.goToToday();
        expect(component.currentMonth().getMonth()).toBe(new Date('2026-07-08T09:00:00Z').getMonth());
    });
});
