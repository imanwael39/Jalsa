import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'http://localhost:5014',
    therapistAiChatHubUrl: 'http://localhost:5014/therapistAiChatHub',
    patientSupportChatHubUrl: 'http://localhost:5014/patientSupportChatHub',
    notificationHubUrl: 'http://localhost:5014/notificationHub',
    aiServiceUrl: 'https://localhost:5001',
    appName: 'Jalsa (Dev)',
    enableMockApi: false,
    logLevel: 'debug',
    sentryDsn: '',
};
