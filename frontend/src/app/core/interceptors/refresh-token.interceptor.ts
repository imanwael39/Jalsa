import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const SKIP_REFRESH = new HttpContextToken<boolean>(() => false);

const AUTH_EXEMPT_PATHS = [
    '/api/auth/login',
    '/api/auth/register',
    '/api/auth/refresh',
    '/api/auth/revoke',
    '/api/auth/forgot-password',
    '/api/auth/reset-password',
];

let isRefreshing = false;
const refreshedToken$ = new BehaviorSubject<string | null>(null);

function cloneWithToken(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
    return req.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
        context: req.context.set(SKIP_REFRESH, true),
    });
}

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);

    const isExempt = AUTH_EXEMPT_PATHS.some(path => req.url.includes(path)) || req.context.get(SKIP_REFRESH);
    if (isExempt) {
        return next(req);
    }

    return next(req).pipe(
        catchError((error: unknown) => {
            if (!(error instanceof HttpErrorResponse) || error.status !== 401) {
                return throwError(() => error);
            }

            if (!isRefreshing) {
                isRefreshing = true;
                refreshedToken$.next(null);

                return authService.refreshToken().pipe(
                    switchMap(response => {
                        isRefreshing = false;
                        refreshedToken$.next(response.token);
                        return next(cloneWithToken(req, response.token));
                    }),
                    catchError((refreshError: unknown) => {
                        isRefreshing = false;
                        authService.logout();
                        return throwError(() => refreshError);
                    })
                );
            }

            return refreshedToken$.pipe(
                filter((token): token is string => token !== null),
                take(1),
                switchMap(token => next(cloneWithToken(req, token)))
            );
        })
    );
};
