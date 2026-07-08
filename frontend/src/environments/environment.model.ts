export interface Environment {
    production: boolean;
    apiUrl: string;
    signalRHubUrl: string;
    notificationHubUrl: string;
    aiServiceUrl: string;
    appName: string;
    enableMockApi: boolean;
    logLevel: 'debug' | 'info' | 'warn' | 'error';
    sentryDsn: string;
}
