import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { PatientDetail } from './patient-detail';
import { PatientService } from '../../../../core/services/patient.service';
import { PatientStateService } from '../../../../core/state/patient-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Patient } from '../../../../core/models';

const mockPatient: Patient = {
    id: '123e4567-e89b-12d3-a456-426614174000',
    therapistId: '123e4567-e89b-12d3-a456-426614174001',
    clinicId: null,
    userId: null,
    fullName: 'John Doe',
    dateOfBirth: '1990-01-15',
    gender: 'Male',
    phone: '+1234567890',
    email: 'john.doe@example.com',
    address: '123 Main St',
    referralSource: 'Self-referral',
    chiefComplaint: 'Anxiety',
    status: 'Active',
    createdAt: '2024-01-01T00:00:00Z',
    updatedAt: '2024-01-02T00:00:00Z',
};

interface SetupOptions {
    patientId?: string | null;
    patientStatus?: string;
    getPatientReturn?: Observable<unknown>;
    archivePatientReturn?: Observable<unknown>;
    deletePatientReturn?: Observable<unknown>;
}

describe('PatientDetail', () => {
    let component: PatientDetail;
    let fixture: ComponentFixture<PatientDetail>;
    let patientServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateServiceSpy: Record<string, unknown>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const {
            patientId = '123e4567-e89b-12d3-a456-426614174000',
            patientStatus = 'Active',
            getPatientReturn,
            archivePatientReturn,
            deletePatientReturn,
        } = opts;

        const patient = { ...mockPatient, status: patientStatus };

        patientServiceSpy = {
            getPatient: vi.fn().mockReturnValue(getPatientReturn ?? of(patient)),
            archivePatient: vi.fn().mockReturnValue(archivePatientReturn ?? of(undefined)),
            restorePatient: vi.fn().mockReturnValue(of(undefined)),
            deletePatient: vi.fn().mockReturnValue(deletePatientReturn ?? of(undefined)),
        };
        stateServiceSpy = {
            selectedPatient: signal(patient),
            selectPatient: vi.fn(),
            updatePatient: vi.fn(),
            removePatient: vi.fn(),
            clearSelected: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            providers: [
                provideRouter([]),
                { provide: PatientService, useValue: patientServiceSpy },
                { provide: PatientStateService, useValue: stateServiceSpy },
                { provide: NotificationService, useValue: notificationSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            paramMap: {
                                get: (key: string): string | null => (key === 'id' ? patientId : null),
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(PatientDetail);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load patient on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientServiceSpy['getPatient']).toHaveBeenCalledWith('123e4567-e89b-12d3-a456-426614174000');
        expect(stateServiceSpy['selectPatient']).toHaveBeenCalledWith(expect.objectContaining({ id: mockPatient.id }));
    });

    it('should set error when patient ID is not found', (): void => {
        setup({ patientId: null });
        fixture.detectChanges();
        expect(component.error()).toBe('معرف المريض غير موجود');
        expect(component.loading()).toBe(false);
    });

    it('should set error when API call fails', (): void => {
        setup({ getPatientReturn: throwError((): Error => new Error('API Error')) });
        fixture.detectChanges();
        expect(component.error()).toBe('API Error');
        expect(component.loading()).toBe(false);
    });

    it('should navigate back to patients list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients']);
    });

    it('should navigate to edit page', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToEdit();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', mockPatient.id, 'edit']);
    });

    it('should navigate to intake form', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToIntake();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', mockPatient.id, 'intake']);
    });

    it('should navigate to assessments', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToAssessments();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', mockPatient.id, 'assessments']);
    });

    it('should set active tab', (): void => {
        setup();
        fixture.detectChanges();
        component.setTab('sessions');
        expect(component.activeTab()).toBe('sessions');
    });

    it('should open and close archive modal', (): void => {
        setup();
        fixture.detectChanges();
        component.openArchiveModal();
        expect(component.showArchiveModal()).toBe(true);
        component.closeArchiveModal();
        expect(component.showArchiveModal()).toBe(false);
    });

    it('should open and close delete modal', (): void => {
        setup();
        fixture.detectChanges();
        component.openDeleteModal();
        expect(component.showDeleteModal()).toBe(true);
        component.closeDeleteModal();
        expect(component.showDeleteModal()).toBe(false);
    });

    it('should archive patient successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.openArchiveModal();
        component.archivePatient();
        expect(patientServiceSpy['archivePatient']).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy['updatePatient']).toHaveBeenCalledWith(expect.objectContaining({ status: 'Archived' }));
        expect(notificationSpy['success']).toHaveBeenCalledWith('تم أرشفة المريض بنجاح');
        expect(component.showArchiveModal()).toBe(false);
    });

    it('should handle archive error', (): void => {
        setup({ archivePatientReturn: throwError((): Error => new Error('Archive failed')) });
        fixture.detectChanges();
        component.archivePatient();
        expect(notificationSpy['error']).toHaveBeenCalledWith('Archive failed');
    });

    it('should delete patient successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.openDeleteModal();
        component.deletePatient();
        expect(patientServiceSpy['deletePatient']).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy['removePatient']).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy['clearSelected']).toHaveBeenCalled();
        expect(notificationSpy['success']).toHaveBeenCalledWith('تم حذف المريض بنجاح');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients']);
    });

    it('should handle delete error', (): void => {
        setup({ deletePatientReturn: throwError((): Error => new Error('Delete failed')) });
        fixture.detectChanges();
        component.deletePatient();
        expect(notificationSpy['error']).toHaveBeenCalledWith('Delete failed');
    });

    it('should return false for isArchived when status is Active', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isArchived()).toBe(false);
    });

    it('should format date correctly', (): void => {
        setup();
        expect(component.formatDate('2024-01-15')).toBeTruthy();
        expect(component.formatDate(null)).toBe('غير متوفر');
    });
});
