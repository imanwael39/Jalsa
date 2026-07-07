import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, signal } from '@angular/core';
import { RouterLink, provideRouter } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { PatientDashboardComponent } from './patient-dashboard.component';
import { PatientDashboardService } from '../../../../core/services/patient-dashboard.service';
import { PatientDashboardStateService } from '../../../../core/state/patient-dashboard-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { StatsCardComponent } from '../../../../shared/components/stats-card/stats-card.component';
import { PatientDashboard } from '../../../../core/models';

@Component({ selector: 'app-line-chart', template: '', standalone: true })
class MockLineChart {}

const mockDashboard: PatientDashboard = {
    patientFirstName: 'سارة',
    upcomingSessions: [
        { id: 's1', sessionDate: '2026-07-10', sessionType: 'فردية', durationMinutes: 50, status: 'Scheduled' },
    ],
    todayReminders: [{ type: 'Exercise', title: 'تمرين التنفس', referenceId: 'e1' }],
    assignedExercises: [
        {
            id: 'e1',
            description: 'تمرين التنفس',
            frequency: 'يومي',
            dueDate: '2026-07-09',
            status: 'Active',
            isOverdue: false,
        },
    ],
    pendingAssessments: [],
    recentConversations: [{ conversationId: 'c1', lastActivityAt: '2026-07-08T10:00:00Z', status: 'Open' }],
    progressOverview: {
        exerciseCompletionRate: 60,
        latestAssessmentScore: 12,
        previousAssessmentScore: 20,
        assessmentTrend: [
            { date: '2026-05', value: 20, label: 'مايو' },
            { date: '2026-06', value: 12, label: 'يونيو' },
        ],
        completedSessionsCount: 4,
    },
    therapist: {
        id: 't1',
        fullName: 'د. أحمد سالم',
        specialization: 'علاج معرفي سلوكي',
        phone: '0100000000',
        profileImageUrl: null,
    },
    crisisSupport: { hotlineNumber: '08008880700', hotlineLabel: 'الخط الساخن للصحة النفسية' },
};

interface SetupOptions {
    getDashboardReturn?: Observable<unknown>;
}

describe('PatientDashboardComponent', () => {
    let component: PatientDashboardComponent;
    let fixture: ComponentFixture<PatientDashboardComponent>;
    let patientDashboardServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getDashboardReturn } = opts;

        patientDashboardServiceSpy = {
            getDashboard: vi.fn().mockReturnValue(getDashboardReturn ?? of(mockDashboard)),
        };
        stateSpy = {
            dashboard: signal(mockDashboard),
            loading: signal(false),
            error: signal(null),
            isStale: signal(false),
            setDashboard: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
        };
        authServiceSpy = {
            currentUser: signal({
                id: 'user-1',
                email: 'patient@test.com',
                firstName: 'سارة',
                lastName: 'أحمد',
                profileImageUrl: null,
                roles: ['Patient'],
            }),
        };

        TestBed.configureTestingModule({
            providers: [
                provideRouter([]),
                { provide: PatientDashboardService, useValue: patientDashboardServiceSpy },
                { provide: PatientDashboardStateService, useValue: stateSpy },
                { provide: AuthService, useValue: authServiceSpy },
            ],
        });

        TestBed.overrideComponent(PatientDashboardComponent, {
            set: { imports: [RouterLink, MockLineChart, EmptyStateComponent, StatsCardComponent] },
        });

        fixture = TestBed.createComponent(PatientDashboardComponent);
        component = fixture.componentInstance;
    };

    it('should create', (): void => {
        setup();
        expect(component).toBeTruthy();
    });

    it('should load dashboard on init', (): void => {
        setup();
        fixture.detectChanges();
        expect(patientDashboardServiceSpy['getDashboard']).toHaveBeenCalled();
        expect(stateSpy['setDashboard']).toHaveBeenCalledWith(mockDashboard);
    });

    it('should handle load error', (): void => {
        setup({ getDashboardReturn: throwError((): Error => new Error('Load failed')) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalled();
    });

    it('should compute assessment trend labels from dashboard', (): void => {
        setup();
        fixture.detectChanges();
        const labels = component.assessmentTrendLabels();
        expect(labels).toEqual(['مايو', 'يونيو']);
    });

    it('should compute assessment trend data as line chart dataset', (): void => {
        setup();
        fixture.detectChanges();
        const data = component.assessmentTrendData();
        expect(data).toHaveLength(1);
        expect(data[0].label).toBe('درجة التقييم');
        expect(data[0].data).toEqual([20, 12]);
    });

    it('should render empty state for pending assessments section', (): void => {
        setup();
        fixture.detectChanges();
        const compiled = fixture.nativeElement as HTMLElement;
        expect(compiled.textContent).toContain('لا توجد تقييمات معلقة');
    });

    it('should render the crisis support hotline number', (): void => {
        setup();
        fixture.detectChanges();
        const compiled = fixture.nativeElement as HTMLElement;
        expect(compiled.textContent).toContain('08008880700');
    });
});
