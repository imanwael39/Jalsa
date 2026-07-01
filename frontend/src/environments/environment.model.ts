export interface Environment {
    production: boolean;
    apiUrl: string;
    signalRHubUrl: string;
    aiServiceUrl: string;
    appName: string;
    enableMockApi: boolean;
    logLevel: 'debug' | 'info' | 'warn' | 'error';
    sentryDsn: string;
}
