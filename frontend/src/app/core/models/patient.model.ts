export interface Patient {
    id: string;
    therapistId: string;
    clinicId: string | null;
    userId: string | null;
    fullName: string;
    dateOfBirth: string | null;
    gender: string | null;
    phone: string | null;
    email: string | null;
    address: string | null;
    referralSource: string | null;
    chiefComplaint: string | null;
    emergencyContactName: string | null;
    emergencyContactRelationship: string | null;
    emergencyContactPhone: string | null;
    treatmentStartDate: string | null;
    status: string;
    createdAt: string;
    updatedAt: string;
}

export interface CreatePatientRequest {
    fullName: string;
    dateOfBirth?: string | null;
    gender?: string | null;
    phone?: string | null;
    email?: string | null;
    /** Required when email is set — creates the patient's login account with this password. */
    password?: string | null;
    address?: string | null;
    referralSource?: string | null;
    chiefComplaint?: string | null;
    emergencyContactName?: string | null;
    emergencyContactRelationship?: string | null;
    emergencyContactPhone?: string | null;
    treatmentStartDate?: string | null;
}

export interface UpdatePatientRequest {
    fullName?: string;
    dateOfBirth?: string | null;
    gender?: string | null;
    phone?: string | null;
    email?: string | null;
    address?: string | null;
    referralSource?: string | null;
    chiefComplaint?: string | null;
    emergencyContactName?: string | null;
    emergencyContactRelationship?: string | null;
    emergencyContactPhone?: string | null;
    treatmentStartDate?: string | null;
}

export interface PatientFilter {
    searchTerm?: string;
    status?: string;
    page: number;
    pageSize: number;
}

export interface IntakeForm {
    id: string;
    patientId: string;
    presentingProblem: string | null;
    psychiatricHistory: string | null;
    familyHistory: string | null;
    medications: string | null;
    socialHistory: string | null;
    status: string;
    createdAt: string;
    submittedAt: string | null;
}

export interface Assessment {
    id: string;
    patientId: string;
    sessionId: string | null;
    templateId: string;
    title: string | null;
    assessmentDate: string | null;
    totalScore: number | null;
    severity: string | null;
    status: string;
    completedAt: string | null;
    createdAt: string;
    updatedAt: string;
}
