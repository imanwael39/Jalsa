import { ComponentFixture, TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { AssessmentDetailComponent } from './assessment-detail.component';
import { PatientAssessmentService } from '../../../../core/services/patient-assessment.service';
import { PatientAssessmentStateService } from '../../../../core/state/patient-assessment-state.service';
import { PatientAssessmentDetail } from '../../../../core/models';

const mockAssignedDetail: PatientAssessmentDetail = {
    id: 'a1',
    templateName: 'phq-9',
    title: null,
    status: 'Assigned',
    assessmentDate: null,
    completedAt: null,
    totalScore: null,
    severity: null,
    questions: [
        { id: 'q1', questionText: 'Question 1', questionType: 'Likert0to3', sortOrder: 1, answerNumber: null },
        { id: 'q2', questionText: 'Question 2', questionType: 'Likert0to3', sortOrder: 2, answerNumber: 1 },
    ],
};

const mockCompletedDetail: PatientAssessmentDetail = {
    ...mockAssignedDetail,
    status: 'Completed',
    totalScore: 5,
    severity: 'Mild',
    completedAt: '2026-06-01T10:00:00Z',
    questions: [
        { id: 'q1', questionText: 'Question 1', questionType: 'Likert0to3', sortOrder: 1, answerNumber: 2 },
        { id: 'q2', questionText: 'Question 2', questionType: 'Likert0to3', sortOrder: 2, answerNumber: 3 },
    ],
};

interface SetupOptions {
    detail?: PatientAssessmentDetail;
    getDetailReturn?: Observable<unknown>;
    saveAnswerReturn?: Observable<unknown>;
    submitReturn?: Observable<unknown>;
    routeId?: string | null;
}

describe('AssessmentDetailComponent', () => {
    let component: AssessmentDetailComponent;
    let fixture: ComponentFixture<AssessmentDetailComponent>;
    let serviceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let routerSpy: Record<string, ReturnType<typeof vi.fn>>;
    let currentDetail: PatientAssessmentDetail | null;

    const setup = (opts: SetupOptions = {}): void => {
        const { detail = mockAssignedDetail, getDetailReturn, saveAnswerReturn, submitReturn, routeId = 'a1' } = opts;

        currentDetail = detail;

        serviceSpy = {
            getDetail: vi.fn().mockReturnValue(getDetailReturn ?? of(detail)),
            saveAnswer: vi.fn().mockReturnValue(saveAnswerReturn ?? of(undefined)),
            submit: vi.fn().mockReturnValue(submitReturn ?? of(mockCompletedDetail)),
        };
        stateSpy = {
            detail: signal<PatientAssessmentDetail | null>(null),
            loading: signal(false),
            saving: signal(false),
            error: signal(null),
            setDetail: vi.fn((d: PatientAssessmentDetail) => {
                currentDetail = d;
                (stateSpy['detail'] as ReturnType<typeof signal>).set(d);
            }),
            setLoading: vi.fn(),
            setSaving: vi.fn(),
            setError: vi.fn(),
        };
        routerSpy = { navigate: vi.fn() };

        TestBed.configureTestingModule({
            providers: [
                { provide: PatientAssessmentService, useValue: serviceSpy },
                { provide: PatientAssessmentStateService, useValue: stateSpy },
                { provide: Router, useValue: routerSpy },
                {
                    provide: ActivatedRoute,
                    useValue: {
                        snapshot: {
                            paramMap: { get: (key: string): string | null => (key === 'id' ? routeId : null) },
                        },
                    },
                },
            ],
        });

        fixture = TestBed.createComponent(AssessmentDetailComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load detail on init using the route id', (): void => {
        setup();
        fixture.detectChanges();
        expect(serviceSpy['getDetail']).toHaveBeenCalledWith('a1');
        expect(stateSpy['setDetail']).toHaveBeenCalledWith(mockAssignedDetail);
    });

    it('should set an error when the route has no id', (): void => {
        setup({ routeId: null });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalledWith('معرف التقييم مطلوب');
        expect(serviceSpy['getDetail']).not.toHaveBeenCalled();
    });

    it('should handle load error', (): void => {
        setup({ getDetailReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('isAssigned should be true for an Assigned assessment', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.isAssigned()).toBe(true);
    });

    it('isAssigned should be false for a Completed assessment', (): void => {
        setup({ detail: mockCompletedDetail });
        fixture.detectChanges();
        expect(component.isAssigned()).toBe(false);
    });

    it('allAnswered should be false when some questions are unanswered', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.allAnswered()).toBe(false);
    });

    it('allAnswered should be true once every question has an answer', (): void => {
        setup({ detail: mockCompletedDetail });
        fixture.detectChanges();
        expect(component.allAnswered()).toBe(true);
    });

    it('selectAnswer should optimistically update local state and call saveAnswer', (): void => {
        setup();
        fixture.detectChanges();

        component.selectAnswer('q1', 2);

        expect(serviceSpy['saveAnswer']).toHaveBeenCalledWith('a1', 'q1', { answerNumber: 2 });
        const updatedQuestion = currentDetail?.questions.find(q => q.id === 'q1');
        expect(updatedQuestion?.answerNumber).toBe(2);
    });

    it('submit should not call the service when not all questions are answered', (): void => {
        setup();
        fixture.detectChanges();
        component.submit();
        expect(serviceSpy['submit']).not.toHaveBeenCalled();
    });

    it('submit should call the service and update detail when all questions are answered', (): void => {
        setup({ detail: mockCompletedDetail });
        fixture.detectChanges();

        component.submit();

        expect(serviceSpy['submit']).toHaveBeenCalledWith('a1');
        expect(stateSpy['setDetail']).toHaveBeenCalledWith(mockCompletedDetail);
    });

    it('goBack should navigate to the assessments list', (): void => {
        setup();
        fixture.detectChanges();
        component.goBack();
        expect(routerSpy['navigate']).toHaveBeenCalledWith(['/assessments']);
    });
});
