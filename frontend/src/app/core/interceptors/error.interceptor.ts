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
            if (error.status === 401) {
                authService.logout();
                router.navigate(['/auth/login']);
                notification.error('Your session has expired. Please log in again.');
            } else if (error.status === 403) {
                notification.error('You do not have permission to perform this action.');
            } else if (error.status === 400) {
                const message = error.error?.message || 'Invalid request. Please check your input.';
                notification.error(message);
            } else if (error.status === 500) {
                notification.error('A server error occurred. Please try again later.');
            } else {
                notification.error('An unexpected error occurred. Please try again.');
            }

            return throwError(() => error);
        })
    );
};
