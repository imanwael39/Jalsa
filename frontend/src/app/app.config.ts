import { ApplicationConfig, provideZoneChangeDetection, ErrorHandler } from '@angular/core';
import { provideRouter, Router } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import * as Sentry from '@sentry/angular';

import { routes } from './app.routes';
import { authInterceptor, errorInterceptor, loadingInterceptor } from './core/interceptors';

export const appConfig: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, loadingInterceptor])),
        provideAnimations(),
        { provide: ErrorHandler, useValue: Sentry.createErrorHandler({ showDialog: false }) },
        { provide: Sentry.TraceService, deps: [Router] },
    ],
};
