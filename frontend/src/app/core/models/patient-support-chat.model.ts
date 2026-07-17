export interface PatientSupportConversation {
    id: string;
    status: string;
    lastActivityAt: string | null;
    createdAt: string;
    messageCount: number;
}

export interface PatientSupportChatMessage {
    id: string;
    conversationId: string;
    senderType: 'Patient' | 'AI';
    content: string | null;
    createdAt: string;
}

export interface SendPatientSupportMessageRequest {
    conversationId: string;
    content: string;
}
