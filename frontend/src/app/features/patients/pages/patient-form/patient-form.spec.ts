import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { Observable, of, throwError } from 'rxjs';
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

interface SetupOptions {
    patientId?: string | null;
    getPatientReturn?: Observable<unknown>;
    createPatientReturn?: Observable<unknown>;
    updatePatientReturn?: Observable<unknown>;
}

describe('PatientForm', () => {
    let component: PatientForm;
    let fixture: ComponentFixture<PatientForm>;
    let patientServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { patientId = null, getPatientReturn, createPatientReturn, updatePatientReturn } = opts;

        patientServiceSpy = {
            getPatient: vi.fn().mockReturnValue(getPatientReturn ?? of(mockPatient)),
            createPatient: vi.fn().mockReturnValue(createPatientReturn ?? of(mockPatient)),
            updatePatient: vi.fn().mockReturnValue(updatePatientReturn ?? of(mockPatient)),
        };
        stateServiceSpy = {
            selectPatient: vi.fn(),
            addPatient: vi.fn(),
            updatePatient: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

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
        expect(component.isEdit()).toBe(false);
        expect(component.patientId()).toBeNull();
    });

    it('should initialize in edit mode when patient ID provided', (): void => {
        setup({ patientId: '123e4567-e89b-12d3-a456-426614174000' });
        fixture.detectChanges();
        expect(component.isEdit()).toBe(true);
        expect(component.patientId()).toBe('123e4567-e89b-12d3-a456-426614174000');
    });

    it('should load patient data in edit mode', (): void => {
        setup({ patientId: '123e4567-e89b-12d3-a456-426614174000' });
        fixture.detectChanges();
        expect(patientServiceSpy['getPatient']).toHaveBeenCalledWith('123e4567-e89b-12d3-a456-426614174000');
        expect(component.form.get('fullName')?.value).toBe('John Doe');
        expect(component.form.get('email')?.value).toBe('john.doe@example.com');
    });

    it('should handle load error', (): void => {
        setup({
            patientId: '123e4567-e89b-12d3-a456-426614174000',
            getPatientReturn: throwError(() => ({ error: { message: 'Load failed' } })),
        });
        fixture.detectChanges();
        expect(component.error()).toBe('Load failed');
    });

    it('should require fullName field', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('');
        fullNameControl?.markAsTouched();
        expect(fullNameControl?.valid).toBe(false);
        expect(fullNameControl?.errors?.['required']).toBeTruthy();
    });

    it('should validate minLength for fullName', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('A');
        fullNameControl?.markAsTouched();
        expect(fullNameControl?.valid).toBe(false);
        expect(fullNameControl?.errors?.['minlength']).toBeTruthy();
    });

    it('should validate email format', (): void => {
        setup();
        fixture.detectChanges();
        const emailControl = component.form.get('email');
        emailControl?.setValue('invalid-email');
        emailControl?.markAsTouched();
        expect(emailControl?.valid).toBe(false);
        expect(emailControl?.errors?.['email']).toBeTruthy();
    });

    it('should submit create form successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'New Patient',
            email: 'new@example.com',
            password: 'Passw0rd123',
        });
        component.onSubmit();
        expect(patientServiceSpy['createPatient']).toHaveBeenCalled();
        expect(stateServiceSpy['addPatient']).toHaveBeenCalledWith(mockPatient);
        expect(notificationSpy['success']).toHaveBeenCalledWith('تم إنشاء المريض بنجاح');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', mockPatient.id]);
    });
    it('should require password when email is set on create', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'New Patient',
            email: 'new@example.com',
        });
        component.form.get('password')?.markAsTouched();
        expect(component.form.valid).toBe(false);
        expect(component.getPasswordError()).toBe('كلمة المرور مطلوبة عند إدخال بريد إلكتروني للمريض');
    });
    it('should not require password when editing a patient that already has an email', (): void => {
        setup({ patientId: '123e4567-e89b-12d3-a456-426614174000' });
        fixture.detectChanges();
        component.form.patchValue({ fullName: 'Updated Name' });
        component.onSubmit();
        expect(patientServiceSpy['updatePatient']).toHaveBeenCalled();
    });

    it('should submit edit form successfully', (): void => {
        setup({ patientId: '123e4567-e89b-12d3-a456-426614174000' });
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'Updated Name',
        });
        component.onSubmit();
        expect(patientServiceSpy['updatePatient']).toHaveBeenCalled();
        expect(stateServiceSpy['updatePatient']).toHaveBeenCalledWith(mockPatient);
        expect(notificationSpy['success']).toHaveBeenCalledWith('تم تحديث بيانات المريض بنجاح');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', '123e4567-e89b-12d3-a456-426614174000']);
    });

    it('should handle create error', (): void => {
        setup({ createPatientReturn: throwError(() => ({ error: { message: 'Create failed' } })) });
        fixture.detectChanges();
        component.form.patchValue({
            fullName: 'New Patient',
        });
        component.onSubmit();
        expect(component.error()).toBe('Create failed');
    });

    it('should handle update error', (): void => {
        setup({
            patientId: '123e4567-e89b-12d3-a456-426614174000',
            updatePatientReturn: throwError(() => ({ error: { message: 'Update failed' } })),
        });
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
        expect(component.form.get('fullName')?.touched).toBe(true);
    });

    it('should navigate back to patients list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients']);
    });

    it('should return field invalid status', (): void => {
        setup();
        fixture.detectChanges();
        const fullNameControl = component.form.get('fullName');
        fullNameControl?.setValue('');
        fullNameControl?.markAsTouched();
        expect(component.isFieldInvalid('fullName')).toBe(true);
    });

    it('should not submit when form is invalid', (): void => {
        setup();
        fixture.detectChanges();
        component.form.get('fullName')?.setValue('');
        component.onSubmit();
        expect(patientServiceSpy['createPatient']).not.toHaveBeenCalled();
        expect(patientServiceSpy['updatePatient']).not.toHaveBeenCalled();
    });
});
