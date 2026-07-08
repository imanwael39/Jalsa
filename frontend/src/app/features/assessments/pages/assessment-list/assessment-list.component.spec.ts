import { ComponentFixture, TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { provideRouter } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { AssessmentListComponent } from './assessment-list.component';
import { PatientAssessmentService } from '../../../../core/services/patient-assessment.service';
import { PatientAssessmentStateService } from '../../../../core/state/patient-assessment-state.service';
import { PatientAssessmentSummary } from '../../../../core/models';

const mockList: PatientAssessmentSummary[] = [
    {
        id: 'a1',
        templateName: 'phq-9',
        title: null,
        status: 'Assigned',
        assessmentDate: null,
        completedAt: null,
        totalScore: null,
        severity: null,
        questionCount: 9,
        answeredCount: 3,
    },
    {
        id: 'a2',
        templateName: 'phq-9',
        title: null,
        status: 'Completed',
        assessmentDate: '2026-06-01',
        completedAt: '2026-06-01T10:00:00Z',
        totalScore: 8,
        severity: 'Mild',
        questionCount: 9,
        answeredCount: 9,
    },
];

interface SetupOptions {
    getListReturn?: Observable<unknown>;
}

describe('AssessmentListComponent', () => {
    let component: AssessmentListComponent;
    let fixture: ComponentFixture<AssessmentListComponent>;
    let serviceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getListReturn } = opts;

        serviceSpy = {
            getList: vi.fn().mockReturnValue(getListReturn ?? of(mockList)),
        };
        stateSpy = {
            list: signal(mockList),
            loading: signal(false),
            error: signal(null),
            setList: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
        };

        TestBed.configureTestingModule({
            providers: [
                provideRouter([]),
                { provide: PatientAssessmentService, useValue: serviceSpy },
                { provide: PatientAssessmentStateService, useValue: stateSpy },
            ],
        });

        fixture = TestBed.createComponent(AssessmentListComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load list on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(serviceSpy['getList']).toHaveBeenCalled();
        expect(stateSpy['setList']).toHaveBeenCalledWith(mockList);
    });

    it('should handle load error', (): void => {
        setup({ getListReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('should split list into pending and completed', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.pending()).toHaveLength(1);
        expect(component.pending()[0].id).toBe('a1');
        expect(component.completed()).toHaveLength(1);
        expect(component.completed()[0].id).toBe('a2');
    });
});
