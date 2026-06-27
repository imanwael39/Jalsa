import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { signal } from '@angular/core';
import { of, throwError } from 'rxjs';
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

describe('PatientDetail', () => {
    let component: PatientDetail;
    let fixture: ComponentFixture<PatientDetail>;
    let patientServiceSpy: jasmine.SpyObj<PatientService>;
    let stateServiceSpy: jasmine.SpyObj<PatientStateService>;
    let notificationSpy: jasmine.SpyObj<NotificationService>;
    let routerSpy: jasmine.SpyObj<Router>;

    const setup = (patientId: string | null = '123e4567-e89b-12d3-a456-426614174000'): void => {
        patientServiceSpy = jasmine.createSpyObj('PatientService', [
            'getPatient',
            'archivePatient',
            'restorePatient',
            'deletePatient',
        ]);
        stateServiceSpy = jasmine.createSpyObj(
            'PatientStateService',
            ['selectPatient', 'updatePatient', 'removePatient', 'clearSelected'],
            {
                selectedPatient: signal(mockPatient),
            }
        );
        notificationSpy = jasmine.createSpyObj('NotificationService', ['success', 'error']);
        routerSpy = jasmine.createSpyObj('Router', ['navigate']);

        patientServiceSpy.getPatient.and.returnValue(of(mockPatient));
        patientServiceSpy.archivePatient.and.returnValue(of(undefined as void));
        patientServiceSpy.restorePatient.and.returnValue(of(undefined as void));
        patientServiceSpy.deletePatient.and.returnValue(of(undefined as void));

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
        expect(patientServiceSpy.getPatient).toHaveBeenCalledWith('123e4567-e89b-12d3-a456-426614174000');
        expect(stateServiceSpy.selectPatient).toHaveBeenCalledWith(mockPatient);
    });

    it('should set error when patient ID is not found', (): void => {
        setup(null);
        fixture.detectChanges();
        expect(component.error()).toBe('معرف المريض غير موجود');
        expect(component.loading()).toBeFalse();
    });

    it('should set error when API call fails', (): void => {
        setup();
        patientServiceSpy.getPatient.and.returnValue(throwError((): Error => new Error('API Error')));
        fixture.detectChanges();
        expect(component.error()).toBe('API Error');
        expect(component.loading()).toBeFalse();
    });

    it('should navigate back to patients list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients']);
    });

    it('should navigate to edit page', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToEdit();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', mockPatient.id, 'edit']);
    });

    it('should navigate to intake form', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToIntake();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', mockPatient.id, 'intake']);
    });

    it('should navigate to assessments', (): void => {
        setup();
        fixture.detectChanges();
        component.navigateToAssessments();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', mockPatient.id, 'assessments']);
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
        expect(component.showArchiveModal()).toBeTrue();
        component.closeArchiveModal();
        expect(component.showArchiveModal()).toBeFalse();
    });

    it('should open and close delete modal', (): void => {
        setup();
        fixture.detectChanges();
        component.openDeleteModal();
        expect(component.showDeleteModal()).toBeTrue();
        component.closeDeleteModal();
        expect(component.showDeleteModal()).toBeFalse();
    });

    it('should archive patient successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.openArchiveModal();
        component.archivePatient();
        expect(patientServiceSpy.archivePatient).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy.updatePatient).toHaveBeenCalledWith({ ...mockPatient, status: 'Archived' });
        expect(notificationSpy.success).toHaveBeenCalledWith('تم أرشفة المريض بنجاح');
        expect(component.showArchiveModal()).toBeFalse();
    });

    it('should handle archive error', (): void => {
        setup();
        patientServiceSpy.archivePatient.and.returnValue(throwError((): Error => new Error('Archive failed')));
        fixture.detectChanges();
        component.archivePatient();
        expect(notificationSpy.error).toHaveBeenCalledWith('Archive failed');
    });

    it('should restore patient successfully', (): void => {
        setup();
        stateServiceSpy.selectedPatient = signal({ ...mockPatient, status: 'Archived' });
        TestBed.overrideProvider(PatientStateService, {
            useValue: stateServiceSpy,
        });
        fixture = TestBed.createComponent(PatientDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();

        component.restorePatient();
        expect(patientServiceSpy.restorePatient).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy.updatePatient).toHaveBeenCalledWith({ ...mockPatient, status: 'Active' });
        expect(notificationSpy.success).toHaveBeenCalledWith('تم استعادة المريض بنجاح');
    });

    it('should handle restore error', (): void => {
        setup();
        stateServiceSpy.selectedPatient = signal({ ...mockPatient, status: 'Archived' });
        TestBed.overrideProvider(PatientStateService, {
            useValue: stateServiceSpy,
        });
        fixture = TestBed.createComponent(PatientDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();

        patientServiceSpy.restorePatient.and.returnValue(throwError((): Error => new Error('Restore failed')));
        component.restorePatient();
        expect(notificationSpy.error).toHaveBeenCalledWith('Restore failed');
    });

    it('should delete patient successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.openDeleteModal();
        component.deletePatient();
        expect(patientServiceSpy.deletePatient).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy.removePatient).toHaveBeenCalledWith(mockPatient.id);
        expect(stateServiceSpy.clearSelected).toHaveBeenCalled();
        expect(notificationSpy.success).toHaveBeenCalledWith('تم حذف المريض بنجاح');
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients']);
    });

    it('should handle delete error', (): void => {
        setup();
        patientServiceSpy.deletePatient.and.returnValue(throwError((): Error => new Error('Delete failed')));
        fixture.detectChanges();
        component.deletePatient();
        expect(notificationSpy.error).toHaveBeenCalledWith('Delete failed');
    });

    it('should return true for isArchived when status is Archived', (): void => {
        setup();
        stateServiceSpy.selectedPatient = signal({ ...mockPatient, status: 'Archived' });
        TestBed.overrideProvider(PatientStateService, {
            useValue: stateServiceSpy,
        });
        fixture = TestBed.createComponent(PatientDetail);
        component = fixture.componentInstance;
        expect(component.isArchived()).toBeTrue();
    });

    it('should return false for isArchived when status is Active', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isArchived()).toBeFalse();
    });

    it('should format date correctly', (): void => {
        setup();
        expect(component.formatDate('2024-01-15')).toBeTruthy();
        expect(component.formatDate(null)).toBe('غير متوفر');
    });
});
