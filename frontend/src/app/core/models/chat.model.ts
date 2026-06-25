export interface ChatConversation {
    id: string;
    patientId: string;
    status: string;
    lastActivityAt: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface ChatMessage {
    id: string;
    conversationId: string;
    senderType: string;
    content: string | null;
    tokensUsed: number | null;
    latencyMs: number | null;
    createdAt: string;
}

export interface SendMessageRequest {
    conversationId: string;
    message: string;
}
