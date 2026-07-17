export interface TherapistChatConversation {
    id: string;
    patientId: string;
    patientName: string;
    status: string;
    lastActivityAt: string | null;
    createdAt: string;
    messageCount: number;
}

export interface TherapistChatMessage {
    id: string;
    conversationId: string;
    senderType: 'Therapist' | 'AI';
    content: string | null;
    createdAt: string;
}

export interface SendTherapistChatMessageRequest {
    conversationId: string;
    content: string;
}
