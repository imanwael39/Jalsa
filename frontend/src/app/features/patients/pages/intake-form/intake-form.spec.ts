import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { IntakeForm } from './intake-form';
import { PatientService } from '../../../../core/services/patient.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { IntakeForm as IntakeFormModel } from '../../../../core/models';

const mockIntakeForm: IntakeFormModel = {
    id: 'intake-123',
    patientId: 'patient-123',
    presentingProblem: 'Anxiety and panic attacks',
    psychiatricHistory: 'Previous depression',
    familyHistory: 'Father had depression',
    medications: 'Sertraline 50mg',
    socialHistory: 'Works in IT, lives alone',
    status: 'Draft',
    createdAt: '2024-01-01T00:00:00Z',
    submittedAt: null,
};

describe('IntakeForm', () => {
    let component: IntakeForm;
    let fixture: ComponentFixture<IntakeForm>;
    let patientServiceSpy: jasmine.SpyObj<PatientService>;
    let notificationSpy: jasmine.SpyObj<NotificationService>;
    let routerSpy: jasmine.SpyObj<Router>;

    const setup = (patientId: string | null = 'patient-123'): void => {
        patientServiceSpy = jasmine.createSpyObj('PatientService', [
            'getIntakeForm',
            'saveIntakeForm',
            'uploadIntakeImage',
        ]);
        notificationSpy = jasmine.createSpyObj('NotificationService', ['success', 'error']);
        routerSpy = jasmine.createSpyObj('Router', ['navigate']);

        patientServiceSpy.getIntakeForm.and.returnValue(of(mockIntakeForm));
        patientServiceSpy.saveIntakeForm.and.returnValue(of(mockIntakeForm));
        patientServiceSpy.uploadIntakeImage.and.returnValue(
            of({
                imageUrl: 'http://example.com/image.jpg',
                extractedData: {
                    presentingProblem: 'Extracted problem',
                    psychiatricHistory: 'Extracted history',
                },
            })
        );

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule],
            providers: [
                provideRouter([]),
                { provide: PatientService, useValue: patientServiceSpy },
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

        fixture = TestBed.createComponent(IntakeForm);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should set patient ID from route', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.patientId()).toBe('patient-123');
    });

    it('should set error when patient ID is missing', (): void => {
        setup(null);
        fixture.detectChanges();
        expect(component.error()).toBe('معرف المريض مطلوب');
    });

    it('should load intake form on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientServiceSpy.getIntakeForm).toHaveBeenCalledWith('patient-123');
        expect(component.form.get('presentingProblem')?.value).toBe('Anxiety and panic attacks');
    });

    it('should handle load error gracefully', (): void => {
        setup();
        patientServiceSpy.getIntakeForm.and.returnValue(throwError((): Error => new Error('Load failed')));
        fixture.detectChanges();
        expect(component.loading()).toBeFalse();
    });

    it('should upload image and pre-fill form', (): void => {
        setup();
        fixture.detectChanges();
        const mockFile = new File(['test'], 'test.jpg', { type: 'image/jpeg' });
        const mockEvent = {
            target: { files: [mockFile] },
        } as unknown as Event;
        component.onFileSelected(mockEvent);
        expect(patientServiceSpy.uploadIntakeImage).toHaveBeenCalled();
        expect(component.ocrSuccess()).toBeTrue();
    });

    it('should handle upload error', (): void => {
        setup();
        patientServiceSpy.uploadIntakeImage.and.returnValue(throwError((): Error => new Error('Upload failed')));
        fixture.detectChanges();
        const mockFile = new File(['test'], 'test.jpg', { type: 'image/jpeg' });
        const mockEvent = {
            target: { files: [mockFile] },
        } as unknown as Event;
        component.onFileSelected(mockEvent);
        expect(component.error()).toBe('Upload failed');
    });

    it('should submit form successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            presentingProblem: 'Updated problem',
        });
        component.onSubmit();
        expect(patientServiceSpy.saveIntakeForm).toHaveBeenCalled();
        expect(notificationSpy.success).toHaveBeenCalledWith('Intake form saved successfully');
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', 'patient-123']);
    });

    it('should handle submit error', (): void => {
        setup();
        patientServiceSpy.saveIntakeForm.and.returnValue(throwError((): Error => new Error('Save failed')));
        fixture.detectChanges();
        component.onSubmit();
        expect(component.error()).toBe('Save failed');
    });

    it('should navigate back to patient detail', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', 'patient-123']);
    });

    it('should clear file selection', (): void => {
        setup();
        fixture.detectChanges();
        component.onFileSelected({
            target: { files: [new File(['test'], 'test.jpg', { type: 'image/jpeg' })] },
        } as unknown as Event);
        expect(component.selectedFileName()).toBeTruthy();
        component.clearFileSelection();
        expect(component.selectedFileName()).toBeNull();
        expect(component.ocrSuccess()).toBeFalse();
    });
});
