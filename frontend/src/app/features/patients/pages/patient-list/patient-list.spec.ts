import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, signal } from '@angular/core';
import { of, throwError } from 'rxjs';
import { PatientList } from './patient-list';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Patient } from '../../../../core/models';

@Component({ selector: 'app-table', template: '', standalone: true })
class MockTable {}

@Component({ selector: 'app-button', template: '<ng-content></ng-content>', standalone: true })
class MockButton {}

function makePatients(count: number): Patient[] {
    return Array.from({ length: count }, (_, i) => ({
        id: `patient-${i}`,
        therapistId: 't-1',
        clinicId: null,
        userId: null,
        fullName: `Patient ${i}`,
        dateOfBirth: null,
        gender: null,
        phone: null,
        email: null,
        address: null,
        referralSource: null,
        chiefComplaint: null,
        status: 'Active',
        createdAt: '',
        updatedAt: '',
    }));
}

describe('PatientList', () => {
    let component: PatientList;
    let fixture: ComponentFixture<PatientList>;
    let patientServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (getPatientsReturn?: unknown): void => {
        patientServiceSpy = {
            getPatients: vi.fn().mockReturnValue(getPatientsReturn ?? of(makePatients(10))),
            archivePatient: vi.fn().mockReturnValue(of(undefined)),
            restorePatient: vi.fn().mockReturnValue(of(undefined)),
        };
        stateSpy = {
            patients: signal(makePatients(10)),
            loading: signal(false),
            error: signal(null),
            setPatients: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            providers: [
                { provide: PatientService, useValue: patientServiceSpy },
                { provide: PatientStateService, useValue: stateSpy },
                { provide: NotificationService, useValue: notificationSpy },
            ],
        });

        TestBed.overrideComponent(PatientList, {
            set: { imports: [MockTable, MockButton] },
        });

        fixture = TestBed.createComponent(PatientList);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should request pageSize + 1 rows to detect a next page without a backend total count', () => {
        setup(of(makePatients(10)));
        component.pageSize = 10;
        fixture.detectChanges();

        expect(patientServiceSpy['getPatients']).toHaveBeenCalledWith(expect.objectContaining({ pageSize: 11 }));
    });

    it('should detect a next page exists when more than pageSize rows are returned', () => {
        setup(of(makePatients(11)));
        component.pageSize = 10;
        fixture.detectChanges();

        expect(component.hasNextPage).toBe(true);
        expect(stateSpy['setPatients']).toHaveBeenCalledWith(makePatients(10));
    });

    it('should report no next page when fewer than pageSize + 1 rows are returned', () => {
        setup(of(makePatients(5)));
        component.pageSize = 10;
        fixture.detectChanges();

        expect(component.hasNextPage).toBe(false);
        expect(stateSpy['setPatients']).toHaveBeenCalledWith(makePatients(5));
    });

    it('should show the backend error message on load failure, not a generic HTTP string', () => {
        setup(
            throwError(() => ({
                message: 'Http failure response for /api/patient: 500 Internal Server Error',
                error: { message: 'فشل داخلي في الخادم' },
            }))
        );
        fixture.detectChanges();

        expect(stateSpy['setError']).toHaveBeenCalledWith('فشل داخلي في الخادم');
    });

    it('should archive a patient and reload the list', () => {
        setup();
        fixture.detectChanges();
        component.archivePatient('patient-0');

        expect(patientServiceSpy['archivePatient']).toHaveBeenCalledWith('patient-0');
        expect(notificationSpy['success']).toHaveBeenCalled();
    });

    it('should move to the next page when onPageChange is called', () => {
        setup(of(makePatients(11)));
        component.pageSize = 10;
        fixture.detectChanges();
        component.onPageChange(2);

        expect(component.currentPage).toBe(2);
        expect(patientServiceSpy['getPatients']).toHaveBeenCalledWith(expect.objectContaining({ page: 2 }));
    });
});
