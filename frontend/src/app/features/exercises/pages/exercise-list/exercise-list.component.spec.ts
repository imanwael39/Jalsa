import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { signal } from '@angular/core';
import { of, throwError } from 'rxjs';
import { ExerciseListComponent } from './exercise-list.component';
import { ExerciseService } from '../../../../core/services/exercise.service';
import { ExerciseStateService } from '../../../../core/state/exercise-state.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { Exercise } from '../../../../core/models';

const mockExercise: Exercise = {
    id: 'ex-1',
    patientId: 'pat-1',
    description: 'Breathing exercise',
    frequency: 'Daily',
    startDate: null,
    dueDate: null,
    status: 'Pending',
    createdAt: '',
    updatedAt: '',
};

interface SetupOptions {
    getExercisesReturn?: unknown;
    deleteExerciseReturn?: unknown;
}

describe('ExerciseListComponent', () => {
    let component: ExerciseListComponent;
    let fixture: ComponentFixture<ExerciseListComponent>;
    let exerciseServiceSpy: Record<string, ReturnType<typeof vi.fn>>;
    let stateSpy: Record<string, ReturnType<typeof vi.fn> | ReturnType<typeof signal>>;
    let notificationSpy: Record<string, ReturnType<typeof vi.fn>>;

    const setup = (opts: SetupOptions = {}): void => {
        const { getExercisesReturn, deleteExerciseReturn } = opts;

        exerciseServiceSpy = {
            getExercises: vi.fn().mockReturnValue(getExercisesReturn ?? of([mockExercise])),
            deleteExercise: vi.fn().mockReturnValue(deleteExerciseReturn ?? of(undefined)),
        };
        stateSpy = {
            exercises: signal([mockExercise]),
            loading: signal(false),
            error: signal(null),
            setExercises: vi.fn(),
            setLoading: vi.fn(),
            setError: vi.fn(),
            removeExercise: vi.fn(),
            reset: vi.fn(),
        };
        notificationSpy = { success: vi.fn(), error: vi.fn() };

        TestBed.configureTestingModule({
            providers: [
                provideRouter([]),
                { provide: ExerciseService, useValue: exerciseServiceSpy },
                { provide: ExerciseStateService, useValue: stateSpy },
                { provide: NotificationService, useValue: notificationSpy },
            ],
        });

        fixture = TestBed.createComponent(ExerciseListComponent);
        component = fixture.componentInstance;
    };

    it('should create', () => {
        setup();
        fixture.detectChanges();
        expect(component).toBeTruthy();
    });

    it('should load exercises on init', () => {
        setup();
        fixture.detectChanges();
        expect(exerciseServiceSpy['getExercises']).toHaveBeenCalled();
        expect(stateSpy['setExercises']).toHaveBeenCalledWith([mockExercise]);
    });

    it('should surface the backend error message on load failure', () => {
        setup({ getExercisesReturn: throwError(() => ({ error: { message: 'فشل داخلي' } })) });
        fixture.detectChanges();
        expect(stateSpy['setError']).toHaveBeenCalledWith('فشل داخلي');
    });

    it('should navigate to the assign page', () => {
        setup();
        fixture.detectChanges();
        const router = TestBed.inject(Router);
        const navSpy = vi.spyOn(router, 'navigate');
        component.navigateToAssign();
        expect(navSpy).toHaveBeenCalledWith(['/exercises/assign']);
    });

    it('should open and close the delete modal', () => {
        setup();
        fixture.detectChanges();
        component.openDeleteModal('ex-1');
        expect(component.showDeleteModal()).toBe(true);
        component.closeDeleteModal();
        expect(component.showDeleteModal()).toBe(false);
    });

    it('should delete the exercise and update state on confirm', () => {
        setup();
        fixture.detectChanges();
        component.openDeleteModal('ex-1');
        component.deleteExercise();
        expect(exerciseServiceSpy['deleteExercise']).toHaveBeenCalledWith('ex-1');
        expect(stateSpy['removeExercise']).toHaveBeenCalledWith('ex-1');
        expect(notificationSpy['success']).toHaveBeenCalled();
        expect(component.showDeleteModal()).toBe(false);
    });

    it('should not attempt delete when no exercise is selected', () => {
        setup();
        fixture.detectChanges();
        component.deleteExercise();
        expect(exerciseServiceSpy['deleteExercise']).not.toHaveBeenCalled();
    });
});
