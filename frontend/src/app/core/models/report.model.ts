export interface Report {
    id: string;
    patientId: string;
    therapistId: string;
    title: string;
    content: string;
    status: 'Draft' | 'PendingReview' | 'Approved' | 'Rejected';
    rejectionReason: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface GenerateReportRequest {
    patientId: string;
}

export interface UpdateReportRequest {
    title?: string;
    content?: string;
}
