export interface ChatConversation {
    id: string;
    patientId: string;
    patientName: string;
    status: string;
    lastActivityAt: string | null;
    createdAt: string;
    messageCount: number;
}

export interface ChatMessage {
    id: string;
    conversationId: string;
    senderType: 'Patient' | 'AI' | 'Therapist';
    content: string | null;
    createdAt: string;
}

export interface SendMessageRequest {
    conversationId: string;
    content: string;
}
