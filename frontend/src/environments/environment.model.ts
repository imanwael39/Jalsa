export interface Environment {
    production: boolean;
    apiUrl: string;
    therapistAiChatHubUrl: string;
    patientSupportChatHubUrl: string;
    notificationHubUrl: string;
    aiServiceUrl: string;
    appName: string;
    enableMockApi: boolean;
    logLevel: 'debug' | 'info' | 'warn' | 'error';
    sentryDsn: string;
}
