export interface Session {
    id: string;
    patientId: string;
    therapistId: string;
    date: string;
    content: string;
    status: 'Draft' | 'Completed' | 'Archived';
    voiceMemoUrl: string | null;
    aiSummary: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface CreateSessionRequest {
    patientId: string;
    date: string;
    content?: string;
}

export interface UpdateSessionRequest {
    date?: string;
    content?: string;
    status?: 'Draft' | 'Completed' | 'Archived';
}

export interface VoiceMemoResponse {
    voiceUrl: string;
}

export interface SessionSummaryResponse {
    summary: string;
}
