export interface ReportVersion {
    id: string;
    reportId: string;
    versionNumber: number;
    content: string | null;
    createdByTherapistId: string;
    approvedAt: string | null;
    changeNote: string | null;
    createdAt: string;
}

export interface ReferralReport {
    id: string;
    patientId: string;
    therapistId: string;
    generatedByTherapistId: string;
    status: string;
    currentVersionId: string | null;
    createdAt: string;
    updatedAt: string;
    currentVersion: ReportVersion | null;
    versions: ReportVersion[];
}
