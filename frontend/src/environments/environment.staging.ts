import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'https://staging-api.jalsa.com/api',
    therapistAiChatHubUrl: 'https://staging-api.jalsa.com/therapistAiChatHub',
    patientSupportChatHubUrl: 'https://staging-api.jalsa.com/patientSupportChatHub',
    notificationHubUrl: 'https://staging-api.jalsa.com/notificationHub',
    aiServiceUrl: 'https://staging-ai.jalsa.com',
    appName: 'Jalsa (Staging)',
    enableMockApi: false,
    logLevel: 'info',
    sentryDsn: '',
};
