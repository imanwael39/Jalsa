export interface Patient {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    gender: 'Male' | 'Female' | 'Other';
    phoneNumber: string | null;
    emergencyContact: string | null;
    notes: string | null;
    isArchived: boolean;
    createdAt: string;
    updatedAt: string;
}

export interface CreatePatientRequest {
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    gender: 'Male' | 'Female' | 'Other';
    phoneNumber?: string;
    emergencyContact?: string;
    notes?: string;
}

export interface UpdatePatientRequest {
    firstName?: string;
    lastName?: string;
    email?: string;
    dateOfBirth?: string;
    gender?: 'Male' | 'Female' | 'Other';
    phoneNumber?: string;
    emergencyContact?: string;
    notes?: string;
}

export interface IntakeForm {
    id: string;
    patientId: string;
    content: string;
    extractedData: Record<string, unknown>;
    status: 'Pending' | 'Completed' | 'Reviewed';
    imageUrl: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface IntakeFormRequest {
    content: string;
}

export interface UploadIntakeImageResponse {
    imageUrl: string;
    extractedData: Record<string, unknown>;
}

export interface Assessment {
    id: string;
    patientId: string;
    type: string;
    score: number;
    notes: string | null;
    date: string;
    createdAt: string;
}

export interface AssessmentRequest {
    type: string;
    score: number;
    notes?: string;
    date: string;
}
