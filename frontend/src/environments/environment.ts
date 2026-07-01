import { Environment } from './environment.model';

export const environment: Environment = {
    production: false,
    apiUrl: 'https://localhost:7051',
    signalRHubUrl: 'https://localhost:7051/chatHub',
    aiServiceUrl: 'https://localhost:5001',
    appName: 'Jalsa (Dev)',
    enableMockApi: false,
    logLevel: 'debug',
    sentryDsn: '',
};
