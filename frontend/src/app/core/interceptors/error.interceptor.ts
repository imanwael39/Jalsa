import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const notification = inject(NotificationService);
    const authService = inject(AuthService);
    const router = inject(Router);

    return next(req).pipe(
        catchError((error) => {
            console.error('HTTP Error:', error);

            const serverMessage = error.error?.message || error.error?.error;

            if (error.status === 401) {
                authService.logout();
                router.navigate(['/auth/login']);
                notification.error('Your session has expired. Please log in again.');
            } else if (error.status === 403) {
                notification.error('You do not have permission to perform this action.');
            } else if (error.status === 409) {
                notification.error(serverMessage || 'This resource already exists.');
            } else if (error.status >= 400 && error.status < 500) {
                notification.error(serverMessage || 'Invalid request. Please check your input.');
            } else if (error.status >= 500) {
                notification.error('A server error occurred. Please try again later.');
            } else if (error.status === 0) {
                notification.error('Network error. Please check your connection.');
            } else {
                notification.error('An unexpected error occurred. Please try again.');
            }

            return throwError(() => error);
        })
    );
};
