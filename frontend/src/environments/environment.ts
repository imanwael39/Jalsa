import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'http://localhost:5014',
    signalRHubUrl: 'http://localhost:5014/chatHub',
    aiServiceUrl: 'http://localhost:5001',
    appName: 'Jalsa (Dev)',
    enableMockApi: true,
    logLevel: 'debug',
};
