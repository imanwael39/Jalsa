import { ComponentFixture, TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { of } from 'rxjs';
import { PatientExerciseComponent } from './patient-exercise.component';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Exercise } from '../../../../core/models';

const exercise: Exercise = {
    id: 'ex-1',
    patientId: 'patient-abc',
    description: 'Breathing exercise',
    frequency: 'Daily',
    startDate: null,
    dueDate: null,
    status: 'Active',
    createdAt: '',
    updatedAt: '',
};

describe('PatientExerciseComponent', () => {
    let component: PatientExerciseComponent;
    let fixture: ComponentFixture<PatientExerciseComponent>;
    let exerciseServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let authServiceSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (): void => {
        exerciseServiceSpy = {
            getExercisesByPatient: vi.fn().mockReturnValue(of([exercise])),
            getMyExercises: vi.fn().mockReturnValue(of([exercise])),
            getMyLogs: vi.fn().mockReturnValue(of([])),
            logCompletion: vi.fn().mockReturnValue(
                of({
                    id: 'log-1',
                    exerciseId: 'ex-1',
                    patientId: 'patient-abc',
                    completionStatus: 'Completed',
                    reflectionNote: null,
                    loggedAt: null,
                    createdAt: '',
                })
            ),
        };
        stateSpy = {
            exercises: signal([exercise]),
            loading: signal(false),
            error: signal(null),
            logsByExercise: signal(new Map()),
            setExercises: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
            setLogs: vi.fn(),
            addLog: vi.fn(),
            reset: vi.fn(),
        };
        authServiceSpy = {
            currentUser: vi.fn().mockReturnValue({ id: 'user-patient-1', roles: ['Patient'] }),
        };

        TestBed.configureTestingModule({
            providers: [
                { provide: ExerciseService, useValue: exerciseServiceSpy },
                { provide: ExerciseStateService, useValue: stateSpy },
                { provide: AuthService, useValue: authServiceSpy },
            ],
        });

        fixture = TestBed.createComponent(PatientExerciseComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        fixture.detectChanges();
        expect(component).toBeTruthy();
    });

    it('should call getMyExercises (not the Therapist-only endpoint) when a Patient views their own exercises', () => {
        setup();
        fixture.detectChanges();

        expect(exerciseServiceSpy['getMyExercises']).toHaveBeenCalled();
        expect(exerciseServiceSpy['getExercisesByPatient']).not.toHaveBeenCalled();
    });

    it('should call getExercisesByPatient when a Therapist views a specific patient via patientIdOverride', () => {
        setup();
        fixture.componentRef.setInput('patientIdOverride', 'patient-abc');
        fixture.detectChanges();

        expect(exerciseServiceSpy['getExercisesByPatient']).toHaveBeenCalledWith('patient-abc');
        expect(exerciseServiceSpy['getMyExercises']).not.toHaveBeenCalled();
    });

    it('should log completion using the exercise patientId, not the logged-in user id', () => {
        setup();
        fixture.detectChanges();
        component.logCompletion(exercise);

        expect(exerciseServiceSpy['logCompletion']).toHaveBeenCalledWith(
            expect.objectContaining({ patientId: 'patient-abc' })
        );
    });
});
