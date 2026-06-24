import { Environment } from './environment.model';

export const environment: Environment = {
    production: true,
    apiUrl: 'https://api.jalsa.com/api',
    signalRHubUrl: 'https://api.jalsa.com/chatHub',
    aiServiceUrl: 'https://ai.jalsa.com',
    appName: 'Jalsa',
    enableMockApi: false,
    logLevel: 'error',
};
