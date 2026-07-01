export interface Session {
    id: string;
    patientId: string;
    intakeFormId: string | null;
    sessionNumber: number;
    sessionDate: string;
    durationMinutes: number | null;
    sessionType: string | null;
    status: string;
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

export interface VoiceMemo {
    id: string;
    sessionId: string;
    transcript: string | null;
    createdAt: string;
}

export interface CreateSessionRequest {
    patientId: string;
    intakeFormId?: string;
    sessionDate: string;
    durationMinutes?: number;
    sessionType?: string;
}

export interface UpdateSessionRequest {
    sessionDate?: string;
    durationMinutes?: number;
    sessionType?: string;
    status?: string;
}
