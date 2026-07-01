import {
    Component,
    ChangeDetectionStrategy,
    inject,
    signal,
    input,
    OnInit,
    OnDestroy,
    DestroyRef,
} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Exercise } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';
import { StatusArPipe } from '../../../../shared/pipes/status-ar.pipe';

@Component({
    selector: 'app-patient-exercise',
    standalone: true,
    imports: [FormsModule, ButtonComponent, SpinnerComponent, StatusArPipe],
    templateUrl: './patient-exercise.component.html',
    styleUrl: './patient-exercise.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientExerciseComponent implements OnInit, OnDestroy {
    private exerciseService = inject(ExerciseService);
    private state = inject(ExerciseStateService);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);

    patientIdOverride = input<string | undefined>(undefined);

    exercises = this.state.exercises;
    loading = this.state.loading;
    error = this.state.error;
    logsByExercise = this.state.logsByExercise;

    loggingId = signal<string | null>(null);
    selectedStatus = signal<Record<string, string>>({});
    reflectionNotes = signal<Record<string, string>>({});

    private patientId = '';

    ngOnInit(): void {
        this.patientId = this.patientIdOverride() || this.authService.currentUser()?.id || '';
        if (this.patientId) {
            this.loadExercises();
            this.loadLogs();
        } else {
            this.state.setError('المريض غير مسجل الدخول');
        }
    }

    ngOnDestroy(): void {
        this.state.reset();
    }

    loadExercises(): void {
        this.state.setLoading(true);
        this.state.setError(null);

        // A Therapist viewing a specific patient's tab passes patientIdOverride and is
        // authorized to fetch that patient's exercises. A Patient viewing their own
        // exercises has no override and must use the "my" endpoint — the Therapist-only
        // /api/exercises/patient/{id} endpoint would 403 for a Patient-role user.
        const request$ = this.patientIdOverride()
            ? this.exerciseService.getExercisesByPatient(this.patientId)
            : this.exerciseService.getMyExercises();

        request$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
            next: data => {
                this.state.setExercises(data);
                this.state.setLoading(false);
            },
            error: (err: HttpErrorResponse) => {
                this.state.setError(err.error?.message || err.error?.error || 'فشل تحميل التمارين');
                this.state.setLoading(false);
            },
        });
    }

    loadLogs(): void {
        this.exerciseService
            .getMyLogs()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.state.setLogs(data);
                },
                error: () => {
                    // Logs are optional, silently fail
                },
            });
    }

    logCompletion(exercise: Exercise): void {
        const status = this.selectedStatus()[exercise.id] || 'Completed';
        const reflection = this.reflectionNotes()[exercise.id] || undefined;

        this.loggingId.set(exercise.id);

        this.exerciseService
            .logCompletion({
                exerciseId: exercise.id,
                patientId: exercise.patientId,
                completionStatus: status,
                reflectionNote: reflection,
            })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: log => {
                    this.state.addLog(log);
                    this.loggingId.set(null);
                },
                error: (err: HttpErrorResponse) => {
                    this.state.setError(err.error?.message || err.error?.error || 'فشل تسجيل الإنجاز');
                    this.loggingId.set(null);
                },
            });
    }

    onStatusChange(exerciseId: string, status: string): void {
        this.selectedStatus.update(current => ({ ...current, [exerciseId]: status }));
    }

    onReflectionChange(exerciseId: string, note: string): void {
        this.reflectionNotes.update(current => ({ ...current, [exerciseId]: note }));
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString();
    }
}
