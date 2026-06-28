import { TestBed } from '@angular/core/testing';
import { PatientStateService } from './patient-state.service';
import { Patient } from '../models';

function createPatient(overrides: Partial<Patient> = {}): Patient {
    return {
        id: '1',
        therapistId: 't1',
        clinicId: null,
        userId: null,
        fullName: 'John Doe',
        dateOfBirth: null,
        gender: null,
        phone: null,
        email: null,
        address: null,
        referralSource: null,
        chiefComplaint: null,
        status: 'Active',
        createdAt: '2026-01-01T00:00:00Z',
        updatedAt: '2026-01-01T00:00:00Z',
        ...overrides,
    };
}

describe('PatientStateService', () => {
    let service: PatientStateService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(PatientStateService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should have empty initial state', () => {
        expect(service.patients()).toEqual([]);
        expect(service.selectedPatient()).toBeNull();
        expect(service.loading()).toBe(false);
        expect(service.error()).toBeNull();
        expect(service.patientCount()).toBe(0);
    });

    describe('setPatients', () => {
        it('should set the patients list', () => {
            const patients = [createPatient({ id: '1' }), createPatient({ id: '2' })];
            service.setPatients(patients);
            expect(service.patients()).toEqual(patients);
            expect(service.patientCount()).toBe(2);
        });

        it('should clear error when setting patients', () => {
            service.setError('Some error');
            service.setPatients([]);
            expect(service.error()).toBeNull();
        });
    });

    describe('addPatient', () => {
        it('should add a patient to the list', () => {
            const patient = createPatient({ id: '1' });
            service.addPatient(patient);
            expect(service.patients()).toEqual([patient]);
            expect(service.patientCount()).toBe(1);
        });

        it('should append to existing list', () => {
            const p1 = createPatient({ id: '1' });
            const p2 = createPatient({ id: '2' });
            service.setPatients([p1]);
            service.addPatient(p2);
            expect(service.patientCount()).toBe(2);
            expect(service.patients()[1]).toEqual(p2);
        });
    });

    describe('updatePatient', () => {
        it('should update a patient by id', () => {
            const original = createPatient({ id: '1', fullName: 'John' });
            service.setPatients([original]);
            const updated = createPatient({ id: '1', fullName: 'Jane' });
            service.updatePatient(updated);
            expect(service.patients()[0].fullName).toBe('Jane');
        });

        it('should update selectedPatient if it matches', () => {
            const patient = createPatient({ id: '1', fullName: 'John' });
            service.selectPatient(patient);
            const updated = createPatient({ id: '1', fullName: 'Jane' });
            service.updatePatient(updated);
            expect(service.selectedPatient()?.fullName).toBe('Jane');
        });

        it('should not update selectedPatient if ids do not match', () => {
            const selected = createPatient({ id: '1', fullName: 'John' });
            service.selectPatient(selected);
            const updated = createPatient({ id: '2', fullName: 'Other' });
            service.updatePatient(updated);
            expect(service.selectedPatient()?.fullName).toBe('John');
        });
    });

    describe('removePatient', () => {
        it('should remove a patient by id', () => {
            const p1 = createPatient({ id: '1' });
            const p2 = createPatient({ id: '2' });
            service.setPatients([p1, p2]);
            service.removePatient('1');
            expect(service.patientCount()).toBe(1);
            expect(service.patients()[0].id).toBe('2');
        });

        it('should not throw if id not found', () => {
            service.setPatients([createPatient({ id: '1' })]);
            expect(() => service.removePatient('nonexistent')).not.toThrow();
            expect(service.patientCount()).toBe(1);
        });
    });

    describe('selectPatient / clearSelected', () => {
        it('should set selected patient', () => {
            const patient = createPatient({ id: '1' });
            service.selectPatient(patient);
            expect(service.selectedPatient()).toEqual(patient);
        });

        it('should clear selected patient', () => {
            service.selectPatient(createPatient());
            service.clearSelected();
            expect(service.selectedPatient()).toBeNull();
        });
    });

    describe('setLoading', () => {
        it('should toggle loading state', () => {
            service.setLoading(true);
            expect(service.loading()).toBe(true);
            service.setLoading(false);
            expect(service.loading()).toBe(false);
        });
    });

    describe('setError', () => {
        it('should set error message', () => {
            service.setError('Network error');
            expect(service.error()).toBe('Network error');
        });

        it('should clear error', () => {
            service.setError('error');
            service.setError(null);
            expect(service.error()).toBeNull();
        });
    });

    describe('reset', () => {
        it('should reset all state to initial values', () => {
            service.setPatients([createPatient({ id: '1' })]);
            service.selectPatient(createPatient({ id: '2' }));
            service.setLoading(true);
            service.setError('error');

            service.reset();

            expect(service.patients()).toEqual([]);
            expect(service.selectedPatient()).toBeNull();
            expect(service.loading()).toBe(false);
            expect(service.error()).toBeNull();
            expect(service.patientCount()).toBe(0);
        });
    });

    describe('computed patientCount', () => {
        it('should reflect patients list length', () => {
            expect(service.patientCount()).toBe(0);
            service.setPatients([createPatient({ id: '1' }), createPatient({ id: '2' })]);
            expect(service.patientCount()).toBe(2);
            service.removePatient('1');
            expect(service.patientCount()).toBe(1);
        });
    });
});
