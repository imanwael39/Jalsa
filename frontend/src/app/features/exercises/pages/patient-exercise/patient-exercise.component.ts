import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Exercise, ExerciseLog } from '../../../../core/models';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-patient-exercise',
    standalone: true,
    imports: [FormsModule, ButtonComponent, SpinnerComponent],
    templateUrl: './patient-exercise.component.html',
    styleUrl: './patient-exercise.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PatientExerciseComponent implements OnInit {
    private exerciseService = inject(ExerciseService);
    private state = inject(ExerciseStateService);
    private authService = inject(AuthService);
    private destroyRef = inject(DestroyRef);

    exercises = this.state.exercises;
    loading = this.state.loading;
    error = this.state.error;

    logs = signal<ExerciseLog[]>([]);
    loggingId = signal<string | null>(null);
    selectedStatus = signal<{ [exerciseId: string]: string }>({});
    reflectionNotes = signal<{ [exerciseId: string]: string }>({});

    ngOnInit(): void {
        const user = this.authService.currentUser();
        if (user) {
            this.loadExercises();
            this.loadLogs();
        } else {
            this.state.setError('Patient not logged in');
        }
    }

    loadExercises(): void {
        this.state.setLoading(true);
        this.state.setError(null);
        this.exerciseService.getMyExercises()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (data) => {
                    this.state.setExercises(data);
                    this.state.setLoading(false);
                },
                error: (err) => {
                    this.state.setError(err.message || 'Failed to load exercises');
                    this.state.setLoading(false);
                },
            });
    }

    loadLogs(): void {
        this.exerciseService.getMyLogs()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (data) => {
                    this.logs.set(data);
                },
                error: () => {
                    // Logs are optional, silently fail
                },
            });
    }

    logCompletion(exercise: Exercise): void {
        const user = this.authService.currentUser();
        if (!user) return;

        const status = this.selectedStatus()[exercise.id] || 'Completed';
        const reflection = this.reflectionNotes()[exercise.id] || undefined;

        this.loggingId.set(exercise.id);

        this.exerciseService.logCompletion({
            exerciseId: exercise.id,
            patientId: user.id,
            completionStatus: status,
            reflectionNote: reflection,
        }).pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (log) => {
                    this.logs.update(current => [...current, log]);
                    this.loggingId.set(null);
                },
                error: (err) => {
                    this.state.setError(err.message || 'Failed to log completion');
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

    getLogsForExercise(exerciseId: string): ExerciseLog[] {
        return this.logs().filter(log => log.exerciseId === exerciseId);
    }

    formatDate(date: string | null): string {
        if (!date) return '-';
        return new Date(date).toLocaleDateString();
    }
}
