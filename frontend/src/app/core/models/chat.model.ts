export interface ChatMessage {
    id: string;
    sessionId: string;
    senderId: string;
    senderName: string;
    senderRole: 'Therapist' | 'Patient' | 'AI';
    content: string;
    timestamp: string;
    isCrisisAlert: boolean;
}

export interface SendMessageRequest {
    message: string;
}
