import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'https://staging-api.jalsa.com/api',
    signalRHubUrl: 'https://staging-api.jalsa.com/chatHub',
    aiServiceUrl: 'https://staging-ai.jalsa.com',
    appName: 'Jalsa (Staging)',
    enableMockApi: false,
    logLevel: 'info',
};
