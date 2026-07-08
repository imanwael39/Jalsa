export interface PatientSessionSummary {
    id: string;
    sessionNumber: number;
    sessionDate: string;
    durationMinutes: number | null;
    sessionType: string | null;
    status: string;
    patientRequestType: string | null;
    patientRequestStatus: string | null;
}

export interface PatientSessionDetail {
    id: string;
    sessionNumber: number;
    sessionDate: string;
    durationMinutes: number | null;
    sessionType: string | null;
    status: string;
    therapistName: string;
    patientRequestType: string | null;
    patientRequestNote: string | null;
    patientRequestStatus: string | null;
    patientRequestedAt: string | null;
    canRequestChange: boolean;
}

export interface SessionChangeRequest {
    note: string;
}
