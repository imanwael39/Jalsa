export interface Exercise {
    id: string;
    patientId: string;
    description: string | null;
    frequency: string | null;
    startDate: string | null;
    dueDate: string | null;
    status: string;
    createdAt: string;
    updatedAt: string;
}

export interface ExerciseLog {
    id: string;
    exerciseId: string;
    patientId: string;
    completionStatus: string;
    reflectionNote: string | null;
    loggedAt: string | null;
    createdAt: string;
}

export interface CreateExerciseRequest {
    patientId: string;
    description: string;
    frequency?: string;
    startDate?: string;
    dueDate?: string;
}

export interface UpdateExerciseRequest {
    description?: string;
    frequency?: string;
    dueDate?: string;
    status?: string;
}

export interface LogExerciseRequest {
    exerciseId: string;
    patientId: string;
    completionStatus: string;
    reflectionNote?: string;
}

export interface ExtendDueDateRequest {
    newDueDate: string;
}
