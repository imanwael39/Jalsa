import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { PatientProgressComponent } from './patient-progress.component';
import { PatientProgressService } from '../../../../core/services/patient-progress.service';
import { PatientProgressStateService } from '../../../../core/state/patient-progress-state.service';
import { PatientProgress } from '../../../../core/models';

@Component({ selector: 'app-line-chart', template: '', standalone: true })
class MockLineChart {}

@Component({ selector: 'app-bar-chart', template: '', standalone: true })
class MockBarChart {}

const mockProgress: PatientProgress = {
    assessmentScoreTrend: [
        { date: '2026-05', value: 20, label: 'مايو' },
        { date: '2026-06', value: 12, label: 'يونيو' },
    ],
    moodTrend: [
        { date: '2026-05', value: 5, label: 'مايو' },
        { date: '2026-06', value: 7, label: 'يونيو' },
    ],
    exerciseCompletionTrend: [
        { date: '2026-05', value: 2, label: 'مايو' },
        { date: '2026-06', value: 4, label: 'يونيو' },
    ],
    attendanceBreakdown: [
        { status: 'Completed', count: 5 },
        { status: 'Cancelled', count: 1 },
    ],
    statistics: {
        totalSessions: 6,
        completedSessions: 5,
        attendanceRate: 83.33,
        totalExercises: 10,
        completedExercises: 6,
        exerciseCompletionRate: 60,
        totalAssessments: 2,
        latestAssessmentScore: 12,
        firstAssessmentScore: 20,
        improvementPercentage: 40,
    },
};

interface SetupOptions {
    getProgressReturn?: Observable<unknown>;
}

describe('PatientProgressComponent', () => {
    let component: PatientProgressComponent;
    let fixture: ComponentFixture<PatientProgressComponent>;
    let patientProgressServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getProgressReturn } = opts;

        patientProgressServiceSpy = {
            getProgress: vi.fn().mockReturnValue(getProgressReturn ?? of(mockProgress)),
        };
        stateSpy = {
            progress: signal(mockProgress),
            loading: signal(false),
            error: signal(null),
            setProgress: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
        };

        TestBed.configureTestingModule({
            providers: [
                { provide: PatientProgressService, useValue: patientProgressServiceSpy },
                { provide: PatientProgressStateService, useValue: stateSpy },
            ],
        });

        TestBed.overrideComponent(PatientProgressComponent, {
            set: { imports: [MockLineChart, MockBarChart] },
        });

        fixture = TestBed.createComponent(PatientProgressComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load progress on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientProgressServiceSpy['getProgress']).toHaveBeenCalled();
        expect(stateSpy['setProgress']).toHaveBeenCalledWith(mockProgress);
    });

    it('should handle load error', (): void => {
        setup({ getProgressReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('should compute assessment score trend labels and data', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.assessmentScoreLabels()).toEqual(['مايو', 'يونيو']);
        const data = component.assessmentScoreData();
        expect(data).toHaveLength(1);
        expect(data[0].data).toEqual([20, 12]);
    });

    it('should compute mood trend labels and data', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.moodLabels()).toEqual(['مايو', 'يونيو']);
        const data = component.moodData();
        expect(data[0].data).toEqual([5, 7]);
    });

    it('should compute exercise completion trend data', (): void => {
        setup();
        fixture.detectChanges();
        const data = component.exerciseCompletionData();
        expect(data[0].data).toEqual([2, 4]);
    });

    it('should translate attendance status labels to Arabic', (): void => {
        setup();
        fixture.detectChanges();
        expect(component.attendanceLabels()).toEqual(['مكتمل', 'Cancelled']);
    });

    it('should reload progress using the from/to filter values on submit', (): void => {
        setup();
        fixture.detectChanges();
        component.filterForm.setValue({ from: '2026-01-01', to: '2026-06-30' });

        component.loadProgress();

        expect(patientProgressServiceSpy['getProgress']).toHaveBeenLastCalledWith({
            from: '2026-01-01',
            to: '2026-06-30',
        });
    });

    it('should clear the filter and reload on reset', (): void => {
        setup();
        fixture.detectChanges();
        component.filterForm.setValue({ from: '2026-01-01', to: '2026-06-30' });

        component.resetFilter();

        expect(component.filterForm.getRawValue()).toEqual({ from: '', to: '' });
        expect(patientProgressServiceSpy['getProgress']).toHaveBeenLastCalledWith({
            from: undefined,
            to: undefined,
        });
    });
});
