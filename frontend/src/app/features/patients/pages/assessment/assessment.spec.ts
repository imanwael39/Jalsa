import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { Assessment } from './assessment';
import { PatientService } from '../../../../core/services/patient.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Assessment as AssessmentModel } from '../../../../core/models';

const mockAssessments: AssessmentModel[] = [
    {
        id: 'assess-1',
        patientId: 'patient-123',
        sessionId: null,
        templateId: 'phq-9',
        title: 'PHQ-9 (Depression)',
        assessmentDate: '2024-01-15',
        totalScore: 12,
        status: 'Completed',
        createdAt: '2024-01-15T00:00:00Z',
        updatedAt: '2024-01-15T00:00:00Z',
    },
    {
        id: 'assess-2',
        patientId: 'patient-123',
        sessionId: null,
        templateId: 'gad-7',
        title: 'GAD-7 (Anxiety)',
        assessmentDate: '2024-01-20',
        totalScore: 8,
        status: 'Completed',
        createdAt: '2024-01-20T00:00:00Z',
        updatedAt: '2024-01-20T00:00:00Z',
    },
];

describe('Assessment', () => {
    let component: Assessment;
    let fixture: ComponentFixture<Assessment>;
    let patientServiceSpy: jasmine.SpyObj<PatientService>;
    let notificationSpy: jasmine.SpyObj<NotificationService>;
    let routerSpy: jasmine.SpyObj<Router>;

    const setup = (patientId: string | null = 'patient-123'): void => {
        patientServiceSpy = jasmine.createSpyObj('PatientService', [
            'getAssessments',
            'addAssessment',
        ]);
        notificationSpy = jasmine.createSpyObj('NotificationService', ['success', 'error']);
        routerSpy = jasmine.createSpyObj('Router', ['navigate']);

        patientServiceSpy.getAssessments.and.returnValue(of(mockAssessments));
        patientServiceSpy.addAssessment.and.returnValue(of(mockAssessments[0]));

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
                                get: (key: string): string | null => key === 'id' ? patientId : null,
                            },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(Assessment);
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
        expect(component.error()).toBe('Patient ID required');
    });

    it('should load assessments on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientServiceSpy.getAssessments).toHaveBeenCalledWith('patient-123');
        expect(component.assessments().length).toBe(2);
    });

    it('should handle load error', (): void => {
        setup();
        patientServiceSpy.getAssessments.and.returnValue(throwError((): Error => new Error('Load failed')));
        fixture.detectChanges();
        expect(component.error()).toBe('Load failed');
    });

    it('should require templateId field', (): void => {
        setup();
        fixture.detectChanges();
        const templateIdControl = component.form.get('templateId');
        templateIdControl?.setValue('');
        templateIdControl?.markAsTouched();
        expect(templateIdControl?.valid).toBeFalse();
        expect(templateIdControl?.errors?.['required']).toBeTruthy();
    });

    it('should require totalScore field', (): void => {
        setup();
        fixture.detectChanges();
        const totalScoreControl = component.form.get('totalScore');
        totalScoreControl?.setValue(null);
        totalScoreControl?.markAsTouched();
        expect(totalScoreControl?.valid).toBeFalse();
        expect(totalScoreControl?.errors?.['required']).toBeTruthy();
    });

    it('should validate min score', (): void => {
        setup();
        fixture.detectChanges();
        const totalScoreControl = component.form.get('totalScore');
        totalScoreControl?.setValue(-1);
        totalScoreControl?.markAsTouched();
        expect(totalScoreControl?.valid).toBeFalse();
        expect(totalScoreControl?.errors?.['min']).toBeTruthy();
    });

    it('should submit form successfully', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            templateId: 'phq-9',
            totalScore: 15,
            assessmentDate: '2024-01-25',
        });
        component.onSubmit();
        expect(patientServiceSpy.addAssessment).toHaveBeenCalled();
        expect(notificationSpy.success).toHaveBeenCalledWith('Assessment added successfully');
    });

    it('should reload assessments after successful submit', (): void => {
        setup();
        fixture.detectChanges();
        component.form.patchValue({
            templateId: 'phq-9',
            totalScore: 15,
            assessmentDate: '2024-01-25',
        });
        component.onSubmit();
        expect(patientServiceSpy.getAssessments).toHaveBeenCalledTimes(2);
    });

    it('should handle submit error', (): void => {
        setup();
        patientServiceSpy.addAssessment.and.returnValue(throwError((): Error => new Error('Submit failed')));
        fixture.detectChanges();
        component.form.patchValue({
            templateId: 'phq-9',
            totalScore: 15,
            assessmentDate: '2024-01-25',
        });
        component.onSubmit();
        expect(component.error()).toBe('Submit failed');
    });

    it('should mark all fields as touched on invalid submit', (): void => {
        setup();
        fixture.detectChanges();
        component.onSubmit();
        expect(component.form.get('templateId')?.touched).toBeTrue();
        expect(component.form.get('totalScore')?.touched).toBeTrue();
    });

    it('should navigate back to patient detail', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy.navigate).toHaveBeenCalledWith(['/patients', 'patient-123']);
    });

    it('should return field invalid status', (): void => {
        setup();
        fixture.detectChanges();
        const templateIdControl = component.form.get('templateId');
        templateIdControl?.setValue('');
        templateIdControl?.markAsTouched();
        expect(component.isFieldInvalid('templateId')).toBeTrue();
    });

    it('should return field error message', (): void => {
        setup();
        fixture.detectChanges();
        const templateIdControl = component.form.get('templateId');
        templateIdControl?.setValue('');
        templateIdControl?.markAsTouched();
        expect(component.getFieldError('templateId')).toBe('Assessment type is required');
    });

    it('should return empty error for valid field', (): void => {
        setup();
        fixture.detectChanges();
        const templateIdControl = component.form.get('templateId');
        templateIdControl?.setValue('phq-9');
        templateIdControl?.markAsTouched();
        expect(component.getFieldError('templateId')).toBe('');
    });

    it('should not submit when form is invalid', (): void => {
        setup();
        fixture.detectChanges();
        component.form.get('templateId')?.setValue('');
        component.form.get('totalScore')?.setValue(null);
        component.onSubmit();
        expect(patientServiceSpy.addAssessment).not.toHaveBeenCalled();
    });

    it('should track assessments by id', (): void => {
        setup();
        fixture.detectChanges();
        const assessment = mockAssessments[0];
        expect(component.trackByAssessmentId(0, assessment)).toBe('assess-1');
    });
});
