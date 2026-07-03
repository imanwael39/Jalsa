import { TestBed } from '@angular/core/testing';
import { SessionStateService } from './session-state.service';
import { Session } from '../models';

describe('SessionStateService', () => {
    let service: SessionStateService;

    const mockSession: Session = {
        id: '1',
        patientId: 'patient-1',
        intakeFormId: null,
        sessionNumber: 1,
        sessionDate: '2024-01-15',
        durationMinutes: 50,
        sessionType: 'Individual',
        status: 'Completed',
        createdAt: '2024-01-15T10:00:00Z',
        updatedAt: '2024-01-15T10:00:00Z',
    };

    const mockSession2: Session = {
        ...mockSession,
        id: '2',
        sessionNumber: 2,
        sessionDate: '2024-01-22',
    };

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [SessionStateService],
        });
        service = TestBed.inject(SessionStateService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should have initial empty state', () => {
        expect(service.sessions()).toEqual([]);
        expect(service.selectedSession()).toBeNull();
        expect(service.loading()).toBe(false);
        expect(service.error()).toBeNull();
        expect(service.sessionCount()).toBe(0);
    });

    describe('setSessions', () => {
        it('should set sessions and clear error', () => {
            service.setError('Some error');
            service.setSessions([mockSession]);

            expect(service.sessions()).toEqual([mockSession]);
            expect(service.error()).toBeNull();
        });
    });

    describe('addSession', () => {
        it('should add session to the list', () => {
            service.setSessions([mockSession]);
            service.addSession(mockSession2);

            expect(service.sessions()).toEqual([mockSession, mockSession2]);
            expect(service.sessionCount()).toBe(2);
        });

        it('should add session to empty list', () => {
            service.addSession(mockSession);

            expect(service.sessions()).toEqual([mockSession]);
            expect(service.sessionCount()).toBe(1);
        });
    });

    describe('updateSession', () => {
        it('should update existing session in the list', () => {
            service.setSessions([mockSession]);
            const updatedSession = { ...mockSession, status: 'Archived' as const };

            service.updateSession(updatedSession);

            expect(service.sessions()).toEqual([updatedSession]);
        });

        it('should update selectedSession if it matches the updated session', () => {
            service.setSessions([mockSession]);
            service.selectSession(mockSession);
            const updatedSession = { ...mockSession, status: 'Archived' as const };

            service.updateSession(updatedSession);

            expect(service.selectedSession()).toEqual(updatedSession);
        });

        it('should not update selectedSession if it does not match', () => {
            service.setSessions([mockSession, mockSession2]);
            service.selectSession(mockSession);
            const updatedSession = { ...mockSession2, status: 'Archived' as const };

            service.updateSession(updatedSession);

            expect(service.selectedSession()).toEqual(mockSession);
        });
    });

    describe('removeSession', () => {
        it('should remove session from the list', () => {
            service.setSessions([mockSession, mockSession2]);
            service.removeSession('1');

            expect(service.sessions()).toEqual([mockSession2]);
            expect(service.sessionCount()).toBe(1);
        });

        it('should handle removing non-existent session', () => {
            service.setSessions([mockSession]);
            service.removeSession('999');

            expect(service.sessions()).toEqual([mockSession]);
        });
    });

    describe('selectSession', () => {
        it('should set selectedSession', () => {
            service.selectSession(mockSession);

            expect(service.selectedSession()).toEqual(mockSession);
        });
    });

    describe('clearSelected', () => {
        it('should clear selectedSession', () => {
            service.selectSession(mockSession);
            service.clearSelected();

            expect(service.selectedSession()).toBeNull();
        });
    });

    describe('setLoading', () => {
        it('should set loading state', () => {
            service.setLoading(true);
            expect(service.loading()).toBe(true);

            service.setLoading(false);
            expect(service.loading()).toBe(false);
        });
    });

    describe('setError', () => {
        it('should set error state', () => {
            service.setError('Test error');
            expect(service.error()).toBe('Test error');

            service.setError(null);
            expect(service.error()).toBeNull();
        });
    });

    describe('reset', () => {
        it('should reset all state to initial values', () => {
            service.setSessions([mockSession, mockSession2]);
            service.selectSession(mockSession);
            service.setLoading(true);
            service.setError('Test error');

            service.reset();

            expect(service.sessions()).toEqual([]);
            expect(service.selectedSession()).toBeNull();
            expect(service.loading()).toBe(false);
            expect(service.error()).toBeNull();
            expect(service.sessionCount()).toBe(0);
        });
    });

    describe('sessionCount computed', () => {
        it('should return correct count', () => {
            expect(service.sessionCount()).toBe(0);

            service.setSessions([mockSession]);
            expect(service.sessionCount()).toBe(1);

            service.addSession(mockSession2);
            expect(service.sessionCount()).toBe(2);

            service.removeSession('1');
            expect(service.sessionCount()).toBe(1);
        });
    });
});
