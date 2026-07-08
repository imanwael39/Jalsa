import { Component, ChangeDetectionStrategy, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PatientSessionService } from '../../../../core/services/patient-session.service';
import { PatientSessionStateService } from '../../../../core/state/patient-session-state.service';
import { PatientSessionSummary } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

type ViewMode = 'list' | 'calendar';

interface CalendarDay {
    dateKey: string | null;
    dayNumber: number | null;
    sessions: PatientSessionSummary[];
}

const WEEKDAY_LABELS = ['الأحد', 'الاثنين', 'الثلاثاء', 'الأربعاء', 'الخميس', 'الجمعة', 'السبت'];

@Component({
    selector: 'app-appointment-list',
    standalone: true,
    imports: [ButtonComponent, EmptyStateComponent, StatusArPipe],
    templateUrl: './appointment-list.component.html',
    styleUrl: './appointment-list.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppointmentListComponent implements OnInit {
    private patientSessionService = inject(PatientSessionService);
    private state = inject(PatientSessionStateService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    sessions = this.state.sessions;
    loading = this.state.loading;
    error = this.state.error;

    viewMode = signal<ViewMode>('list');
    currentMonth = signal<Date>(this.startOfMonth(new Date()));

    readonly weekdayLabels = WEEKDAY_LABELS;

    private todayKey = this.toDateKey(new Date());

    upcomingSessions = computed(() =>
        [...this.sessions()]
            .filter(s => s.sessionDate >= this.todayKey)
            .sort((a, b) => a.sessionDate.localeCompare(b.sessionDate))
    );

    pastSessions = computed(() =>
        [...this.sessions()]
            .filter(s => s.sessionDate < this.todayKey)
            .sort((a, b) => b.sessionDate.localeCompare(a.sessionDate))
    );

    monthLabel = computed(() => this.currentMonth().toLocaleDateString('ar-EG', { month: 'long', year: 'numeric' }));

    calendarDays = computed<CalendarDay[]>(() => {
        const monthDate = this.currentMonth();
        const sessionsByDate = this.groupSessionsByDate(this.sessions());

        const year = monthDate.getFullYear();
        const month = monthDate.getMonth();
        const firstOfMonth = new Date(year, month, 1);
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        const leadingBlanks = firstOfMonth.getDay();

        const days: CalendarDay[] = [];
        for (let i = 0; i < leadingBlanks; i++) {
            days.push({ dateKey: null, dayNumber: null, sessions: [] });
        }
        for (let d = 1; d <= daysInMonth; d++) {
            const dateKey = `${year}-${String(month + 1).padStart(2, '0')}-${String(d).padStart(2, '0')}`;
            days.push({ dateKey, dayNumber: d, sessions: sessionsByDate.get(dateKey) ?? [] });
        }
        return days;
    });

    ngOnInit(): void {
        this.loadSessions();
    }

    loadSessions(): void {
        this.state.setLoading(true);
        this.patientSessionService
            .getSessions()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: sessions => {
                    this.state.setSessions(sessions);
                    this.state.setLoading(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.error || err.error?.message || 'فشل تحميل المواعيد');
                    this.state.setLoading(false);
                },
            });
    }

    setViewMode(mode: ViewMode): void {
        this.viewMode.set(mode);
    }

    goToPreviousMonth(): void {
        const current = this.currentMonth();
        this.currentMonth.set(new Date(current.getFullYear(), current.getMonth() - 1, 1));
    }

    goToNextMonth(): void {
        const current = this.currentMonth();
        this.currentMonth.set(new Date(current.getFullYear(), current.getMonth() + 1, 1));
    }

    goToToday(): void {
        this.currentMonth.set(this.startOfMonth(new Date()));
    }

    openSession(id: string): void {
        this.router.navigate(['/appointments', id]);
    }

    isToday(dateKey: string | null): boolean {
        return dateKey === this.todayKey;
    }

    private groupSessionsByDate(sessions: PatientSessionSummary[]): Map<string, PatientSessionSummary[]> {
        const map = new Map<string, PatientSessionSummary[]>();
        for (const session of sessions) {
            const key = session.sessionDate.slice(0, 10);
            const existing = map.get(key);
            if (existing) {
                existing.push(session);
            } else {
                map.set(key, [session]);
            }
        }
        return map;
    }

    private startOfMonth(date: Date): Date {
        return new Date(date.getFullYear(), date.getMonth(), 1);
    }

    private toDateKey(date: Date): string {
        return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
    }
}
