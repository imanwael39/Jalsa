import { Environment } from './environment.model';

export const environment: Environment = {
    production: true,
    apiUrl: 'https://api.jalsa.com/api',
    signalRHubUrl: 'https://api.jalsa.com/chatHub',
    notificationHubUrl: 'https://api.jalsa.com/notificationHub',
    aiServiceUrl: 'https://ai.jalsa.com',
    appName: 'Jalsa',
    enableMockApi: false,
    logLevel: 'error',
    sentryDsn: '',
};
