export interface Exercise {
    id: string;
    name: string;
    description: string;
    category: string;
    instructions: string;
    duration: number;
    isActive: boolean;
    createdAt: string;
}

export interface ExerciseAssignment {
    id: string;
    patientId: string;
    exerciseId: string;
    exercise: Exercise;
    dueDate: string;
    status: 'Pending' | 'InProgress' | 'Completed';
    reflection: string | null;
    completedAt: string | null;
    assignedAt: string;
}

export interface AssignExerciseRequest {
    patientId: string;
    exerciseId: string;
    dueDate: string;
}

export interface UpdateExerciseStatusRequest {
    status: 'Pending' | 'InProgress' | 'Completed';
}
