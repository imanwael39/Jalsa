import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { Observable, of, throwError } from 'rxjs';
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

interface SetupOptions {
    patientId?: string | null;
    getIntakeFormReturn?: Observable<unknown>;
    saveIntakeFormReturn?: Observable<unknown>;
    uploadIntakeImageReturn?: Observable<unknown>;
}

describe('IntakeForm', () => {
    let component: IntakeForm;
    let fixture: ComponentFixture<IntakeForm>;
    let patientServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { patientId = 'patient-123', getIntakeFormReturn, saveIntakeFormReturn, uploadIntakeImageReturn } = opts;

        patientServiceSpy = {
            getIntakeForm: vi.fn().mockReturnValue(getIntakeFormReturn ?? of(mockIntakeForm)),
            saveIntakeForm: vi.fn().mockReturnValue(saveIntakeFormReturn ?? of(mockIntakeForm)),
            uploadIntakeImage: vi.fn().mockReturnValue(
                uploadIntakeImageReturn ??
                    of({
                        imageUrl: 'http://example.com/image.jpg',
                        extractedData: {
                            presentingProblem: 'Extracted problem',
                            psychiatricHistory: 'Extracted history',
                        },
                    })
            ),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };
        routerSpy = { navigate: vi.fn() };

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
        setup({ patientId: null });
        fixture.detectChanges();
        expect(component.error()).toBe('معرف المريض مطلوب');
    });

    it('should load intake form on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientServiceSpy['getIntakeForm']).toHaveBeenCalledWith('patient-123');
        expect(component.form.get('presentingProblem')?.value).toBe('Anxiety and panic attacks');
    });

    it('should handle load error gracefully', (): void => {
        setup({ getIntakeFormReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(component.loading()).toBe(false);
    });

    it('should upload image and pre-fill form', (): void => {
        setup();
        fixture.detectChanges();
        const mockFile = new File(['test'], 'test.jpg', { type: 'image/jpeg' });
        const mockEvent = {
            target: { files: [mockFile] },
        } as unknown as Event;
        component.onFileSelected(mockEvent);
        expect(patientServiceSpy['uploadIntakeImage']).toHaveBeenCalled();
        expect(component.ocrSuccess()).toBe(true);
    });

    it('should handle upload error', (): void => {
        setup({ uploadIntakeImageReturn: throwError(() => ({ error: { message: 'Upload failed' } })) });
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
        expect(patientServiceSpy['saveIntakeForm']).toHaveBeenCalled();
        expect(notificationSpy['success']).toHaveBeenCalledWith('تم حفظ استمارة الاستقبال بنجاح');
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', 'patient-123']);
    });

    it('should handle submit error', (): void => {
        setup({ saveIntakeFormReturn: throwError(() => ({ error: { message: 'Save failed' } })) });
        fixture.detectChanges();
        component.onSubmit();
        expect(component.error()).toBe('Save failed');
    });

    it('should navigate back to patient detail', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/patients', 'patient-123']);
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
        expect(component.ocrSuccess()).toBe(false);
    });
});
