export interface CrisisAlert {
    id: string;
    patientId: string;
    patientName: string;
    severity: string;
    status: string;
    triggeringMessage: string | null;
    createdAt: string;
    updatedAt: string;
}
