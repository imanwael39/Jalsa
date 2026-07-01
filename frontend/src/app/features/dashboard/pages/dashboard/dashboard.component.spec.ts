import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, signal } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { DashboardComponent } from './dashboard.component';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { DashboardStateService } from '../../../../core/state/dashboard-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { DashboardSummary } from '../../../../core/models';

@Component({ selector: 'app-line-chart', template: '', standalone: true })
class MockLineChart {}

@Component({ selector: 'app-bar-chart', template: '', standalone: true })
class MockBarChart {}

const mockSummary: DashboardSummary = {
    totalPatients: 25,
    activePatients: 18,
    archivedPatients: 7,
    totalSessions: 120,
    sessionsThisMonth: 15,
    exerciseCompletionRate: 72,
    averageAssessmentScore: 65,
    recentAlerts: ['Alert 1'],
    analytics: {
        assessmentTrend: [
            { date: '2024-01', value: 70, label: 'يناير' },
            { date: '2024-02', value: 75, label: 'فبراير' },
        ],
        exerciseCompletion: { complete: 45, partial: 20, skipped: 10 },
        sessionFrequency: [
            { date: '2024-01', value: 10, label: 'يناير' },
            { date: '2024-02', value: 12, label: 'فبراير' },
        ],
    },
};

interface SetupOptions {
    getSummaryReturn?: Observable<unknown>;
}

describe('DashboardComponent', () => {
    let component: DashboardComponent;
    let fixture: ComponentFixture<DashboardComponent>;
    let dashboardServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getSummaryReturn } = opts;

        dashboardServiceSpy = {
            getDashboardSummary: vi.fn().mockReturnValue(getSummaryReturn ?? of(mockSummary)),
        };
        stateSpy = {
            summary: signal(mockSummary),
            loading: signal(false),
            error: signal(null),
            isStale: signal(false),
            setSummary: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
        };
        authServiceSpy = {
            currentUser: signal({
                id: 'user-1',
                email: 'test@test.com',
                firstName: 'Test',
                lastName: 'User',
                roles: ['Therapist'],
            }),
        };

        TestBed.configureTestingModule({
            providers: [
                { provide: DashboardService, useValue: dashboardServiceSpy },
                { provide: DashboardStateService, useValue: stateSpy },
                { provide: AuthService, useValue: authServiceSpy },
            ],
        });

        TestBed.overrideComponent(DashboardComponent, {
            set: { imports: [SpinnerComponent, MockLineChart, MockBarChart] },
        });

        fixture = TestBed.createComponent(DashboardComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load dashboard on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(dashboardServiceSpy['getDashboardSummary']).toHaveBeenCalled();
        expect(stateSpy['setSummary']).toHaveBeenCalledWith(mockSummary);
    });

    it('should handle load error', (): void => {
        setup({ getSummaryReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('should compute assessment trend labels from summary', (): void => {
        setup();
        fixture.detectChanges();
        const labels = component.assessmentTrendLabels();
        expect(labels).toEqual(['2024-01', '2024-02']);
    });

    it('should compute session frequency labels from summary', (): void => {
        setup();
        fixture.detectChanges();
        const labels = component.sessionFrequencyLabels();
        expect(labels).toEqual(['2024-01', '2024-02']);
    });

    it('should compute exercise completion labels', (): void => {
        setup();
        fixture.detectChanges();
        const labels = component.exerciseCompletionLabels();
        expect(labels).toEqual(['مكتمل', 'جزئي', 'تم التخطي']);
    });

    it('should compute exercise completion data', (): void => {
        setup();
        fixture.detectChanges();
        const data = component.exerciseCompletionData();
        expect(data).toHaveLength(1);
        expect(data[0].data).toEqual([45, 20, 10]);
    });

    it('should compute assessment trend data as line chart dataset', (): void => {
        setup();
        fixture.detectChanges();
        const data = component.assessmentTrendData();
        expect(data).toHaveLength(1);
        expect(data[0].label).toBe('درجة التقييم');
        expect(data[0].data).toEqual([70, 75]);
    });

    it('should compute session frequency data as line chart dataset', (): void => {
        setup();
        fixture.detectChanges();
        const data = component.sessionFrequencyData();
        expect(data).toHaveLength(1);
        expect(data[0].label).toBe('الجلسات');
        expect(data[0].data).toEqual([10, 12]);
    });
});
