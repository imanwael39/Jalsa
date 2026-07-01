import { Component, ChangeDetectionStrategy, inject, signal, OnInit, OnDestroy, DestroyRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { PatientService } from '../../../../core/services/patient.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { Patient } from '../../../../core/models';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-assign-exercise',
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent, ButtonComponent, SpinnerComponent],
    templateUrl: './assign-exercise.component.html',
    styleUrl: './assign-exercise.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AssignExerciseComponent implements OnInit, OnDestroy {
    private fb = inject(FormBuilder);
    private exerciseService = inject(ExerciseService);
    private patientService = inject(PatientService);
    private state = inject(ExerciseStateService);
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    patients = signal<Patient[]>([]);
    loading = signal(false);
    error = signal<string | null>(null);

    form = this.fb.group({
        patientId: ['', [Validators.required]],
        description: ['', [Validators.required, Validators.maxLength(1000)]],
        frequency: [''],
        startDate: [''],
        dueDate: [''],
    });

    ngOnInit(): void {
        this.loadPatients();
    }

    ngOnDestroy(): void {
        this.state.reset();
    }

    loadPatients(): void {
        this.loading.set(true);
        this.patientService
            .getPatients({ page: 1, pageSize: 100 })
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: data => {
                    this.patients.set(data);
                    this.loading.set(false);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل تحميل قائمة المرضى');
                    this.loading.set(false);
                },
            });
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.loading.set(true);
        this.error.set(null);

        const formValue = this.form.value;
        const request = {
            patientId: formValue.patientId!,
            description: formValue.description!,
            frequency: formValue.frequency || undefined,
            startDate: formValue.startDate || undefined,
            dueDate: formValue.dueDate || undefined,
        };

        this.exerciseService
            .createExercise(request)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: exercise => {
                    this.state.setExercises([...this.state.exercises(), exercise]);
                    this.loading.set(false);
                    this.router.navigate(['/exercises']);
                },
                error: (err: HttpErrorResponse) => {
                    this.error.set(err.error?.message || err.error?.error || 'فشل إنشاء التمرين');
                    this.loading.set(false);
                },
            });
    }

    navigateBack(): void {
        this.router.navigate(['/exercises']);
    }

    get f(): FormGroup['controls'] {
        return this.form.controls;
    }
}
