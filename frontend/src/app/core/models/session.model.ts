export interface Session {
    id: string;
    patientId: string;
    intakeFormId: string | null;
    sessionNumber: number;
    sessionDate: string;
    content: string | null;
    durationMinutes: number | null;
    sessionType: string | null;
    status: string;
    patientRequestType: string | null;
    patientRequestNote: string | null;
    patientRequestStatus: string | null;
    patientRequestedAt: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface SessionNote {
    id: string;
    sessionId: string;
    observations: string | null;
    interventions: string | null;
    patientResponse: string | null;
    homeworkAssigned: string | null;
    nextGoals: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface CreateSessionRequest {
    patientId: string;
    intakeFormId?: string;
    sessionDate: string;
    content?: string;
    durationMinutes?: number;
    sessionType?: string;
}

export interface UpdateSessionRequest {
    sessionDate?: string;
    content?: string;
    durationMinutes?: number;
    sessionType?: string;
    status?: string;
}
