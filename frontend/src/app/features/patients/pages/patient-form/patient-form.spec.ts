import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { PatientForm } from './patient-form';
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

describe('PatientForm', () => {
    let component: PatientForm;
    let fixture: ComponentFixture<PatientForm>;
    let patientServiceSpy: jasmine.SpyObj<PatientService>;
    let stateServiceSpy: jasmine.SpyObj<PatientStateService>;
    let notificationSpy: jasmine.SpyObj<NotificationService>;
    let routerSpy: jasmine.SpyObj<Router>;

    const setup = (patientId: string | null = null): void => {
        patientServiceSpy = jasmine.createSpyObj('PatientService', ['getPatient', 'createPatient', 'updatePatient']);
        stateServiceSpy = jasmine.createSpyObj('PatientStateService', ['selectPatient', 'addPatient', 'updatePatient']);
        notificationSpy = jasmine.createSpyObj('NotificationService', ['success', 'error']);
        routerSpy = jasmine.createSpyObj('Router', ['navigate']);

        patientServiceSpy.getPatient.and.returnValue(of(mockPatient));
        patientServiceSpy.createPatient.and.returnValue(of(mockPatient));
        patientServiceSpy.updatePatient.and.returnValue(of(mockPatient));

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule],
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

        fixture = TestBed.createComponent(PatientForm);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should initialize in create mode when no patient ID', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isEdit()).toBeFalse();
        expect(component.patientId()).toBeNull();
    });

    it('should initialize in edit mode when patient ID provided', (): void => {
        setup('123e4567-e89b-12d3-a456-426614174000');
        fixture.detectChanges();
        expect(component.isEdit()).toBeTrue();
        expect(component.patientId()).toBe('123e4567-e89b-12d3-a456-426614174000');
    });

    it('should load patient data in edit mode', (): void => {
        setup('123e4567-e89b-12d3-a456-426614174000');
        fixture.detectChanges();
        expect(patientServiceSpy.getPatient).toHaveBeenCalledWith('123e4567-e89b-12d3-a456-426614174000');
        expect(component.form.get('fullName')?.value).toBe('John Doe');
        expect(component.form.get('email')?.value).toBe('john.doe@example.com');
    });

    it('should handle load error', (): void => {
        setup('123e4567-e89b-12d3-a456-426614174000');
        patientServiceSpy.getPatient.and.returnValue(throwError((): Error => new Error('Load failed')));
        fixture.detectChanges();
        expect(component.error()).toBe('Load failed');
    });

    it('should require fullName field', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('');
        fullNameControl?.markAsTouched();
        expect(fullNameControl?.valid).toBeFalse();
        expect(fullNameControl?.errors?.['required']).toBeTruthy();
    });

    it('should validate minLength for fullName', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('A');
        fullNameControl?.markAsTouched();
        expect(fullNameControl?.valid).toBeFalse();
        expect(fullNameControl?.errors?.['minlength']).toBeTruthy();
    });

    it('should validate email format', (): void => {
        setup();
        fixture.detectChanges();
        const emailControl = component.form.get('email');
        emailControl?.setValue('invalid-email');
        emailControl?.markAsTouched();
        expect(emailControl?.valid).toBeFalse();
        expect(emailControl?.errors?.['email']).toBeTruthy();
    });

    it('should submit create form successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'New Patient',
            email: 'new@example.com',
        });
        component.onSubmit();
        expect(patientServiceSpy.createPatient).toHaveBeenCalled();
        expect(stateServiceSpy.addPatient).toHaveBeenCalledWith(mockPatient);
        expect(notificationSpy.success).toHaveBeenCalledWith('تم إنشاء المريض بنجاح');
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', mockPatient.id]);
    });

    it('should submit edit form successfully', (): void => {
        setup('123e4567-e89b-12d3-a456-426614174000');
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'Updated Name',
        });
        component.onSubmit();
        expect(patientServiceSpy.updatePatient).toHaveBeenCalled();
        expect(stateServiceSpy.updatePatient).toHaveBeenCalledWith(mockPatient);
        expect(notificationSpy.success).toHaveBeenCalledWith('تم تحديث بيانات المريض بنجاح');
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', '123e4567-e89b-12d3-a456-426614174000']);
    });

    it('should handle create error', (): void => {
        setup();
        patientServiceSpy.createPatient.and.returnValue(throwError((): Error => new Error('Create failed')));
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'New Patient',
        });
        component.onSubmit();
        expect(component.error()).toBe('Create failed');
    });

    it('should handle update error', (): void => {
        setup('123e4567-e89b-12d3-a456-426614174000');
        patientServiceSpy.updatePatient.and.returnValue(throwError((): Error => new Error('Update failed')));
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'Updated Name',
        });
        component.onSubmit();
        expect(component.error()).toBe('Update failed');
    });

    it('should mark all fields as touched on invalid submit', (): void => {
        setup();
        fixture.detectChanges();
        component.onSubmit();
        expect(component.form.get('fullName')?.touched).toBeTrue();
    });

    it('should navigate back to patients list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients']);
    });

    it('should return field invalid status', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('');
        fullNameControl?.markAsTouched();
        expect(component.isFieldInvalid('fullName')).toBeTrue();
    });

    it('should return field error message', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('');
        fullNameControl?.markAsTouched();
        expect(component.getFieldError('fullName')).toBe('Full name is required');
    });

    it('should return empty error for valid field', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('Valid Name');
        fullNameControl?.markAsTouched();
        expect(component.getFieldError('fullName')).toBe('');
    });

    it('should not submit when form is invalid', (): void => {
        setup();
        fixture.detectChanges();
        component.form.get('fullName')?.setValue('');
        component.onSubmit();
        expect(patientServiceSpy.createPatient).not.toHaveBeenCalled();
        expect(patientServiceSpy.updatePatient).not.toHaveBeenCalled();
    });
});
