import {
    ApplicationConfig,
    provideZoneChangeDetection,
    provideAppInitializer,
    inject,
    ErrorHandler,
} from '@angular/core';
import { provideRouter, Router } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import * as Sentry from '@sentry/angular';

import { routes } from './app.routes';
import { authInterceptor, refreshTokenInterceptor, errorInterceptor, loadingInterceptor } from './core/interceptors';
import { AuthService } from './core/services/auth.service';
import { AppStateService } from './core/services/app-state.service';

export const appConfig: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideHttpClient(
            withInterceptors([authInterceptor, errorInterceptor, refreshTokenInterceptor, loadingInterceptor])
        ),
        provideAnimations(),
        provideAppInitializer(() => {
            inject(AppStateService);
            return inject(AuthService).initializeAuth();
        }),
        { provide: ErrorHandler, useValue: Sentry.createErrorHandler({ showDialog: false }) },
        { provide: Sentry.TraceService, deps: [Router] },
    ],
};
