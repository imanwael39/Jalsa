import { TestBed } from '@angular/core/testing';
import { ExerciseStateService } from './exercise-state.service';

describe('ExerciseStateService', () => {
    let service: ExerciseStateService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(ExerciseStateService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should start with empty state', () => {
        expect(service.exercises()).toEqual([]);
        expect(service.logs()).toEqual([]);
        expect(service.loading()).toBe(false);
        expect(service.error()).toBeNull();
    });

    it('should set exercises and clear error', () => {
        const exercises = [
            {
                id: '1',
                patientId: 'p1',
                description: 'Test',
                frequency: null,
                startDate: null,
                dueDate: null,
                status: 'Pending',
                createdAt: '',
                updatedAt: '',
            },
        ];
        service.setExercises(exercises);
        expect(service.exercises()).toEqual(exercises);
        expect(service.error()).toBeNull();
    });

    it('should update an existing exercise in the list', () => {
        const original = {
            id: '1',
            patientId: 'p1',
            description: 'Test',
            frequency: null,
            startDate: null,
            dueDate: null,
            status: 'Pending',
            createdAt: '',
            updatedAt: '',
        };
        service.setExercises([original]);
        const updated = { ...original, status: 'Completed' };
        service.updateExercise(updated);
        expect(service.exercises()).toEqual([updated]);
    });

    it('should remove an exercise from the list', () => {
        const exercise = {
            id: '1',
            patientId: 'p1',
            description: 'Test',
            frequency: null,
            startDate: null,
            dueDate: null,
            status: 'Pending',
            createdAt: '',
            updatedAt: '',
        };
        service.setExercises([exercise]);
        service.removeExercise('1');
        expect(service.exercises()).toEqual([]);
    });

    it('should set loading state', () => {
        service.setLoading(true);
        expect(service.loading()).toBe(true);
        service.setLoading(false);
        expect(service.loading()).toBe(false);
    });

    it('should reset to initial state', () => {
        service.setLoading(true);
        service.setError('error');
        service.reset();
        expect(service.exercises()).toEqual([]);
        expect(service.logs()).toEqual([]);
        expect(service.loading()).toBe(false);
        expect(service.error()).toBeNull();
    });
});
