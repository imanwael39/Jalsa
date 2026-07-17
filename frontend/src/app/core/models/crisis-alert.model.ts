export interface CrisisAlert {
    id: string;
    patientId: string;
    patientName: string;
    conversationId: string | null;
    severity: string;
    reason: string | null;
    confidence: number | null;
    status: string;
    triggeringMessage: string | null;
    createdAt: string;
    updatedAt: string;
}
