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
            const serverMessage = error.error?.message || error.error?.error;

            if (error.status === 401) {
                authService.logout();
                router.navigate(['/auth/login']);
                notification.error('البريد الإلكتروني أو كلمة المرور غير صحيحة');
            } else if (error.status === 403) {
                notification.error('ليس لديك صلاحية للقيام بهذه العملية');
            } else if (error.status === 404) {
                notification.error('المورد المطلوب غير موجود');
            } else if (error.status === 409) {
                notification.error(serverMessage || 'يوجد حساب مسجل بالفعل بهذا البريد الإلكتروني');
            } else if (error.status >= 400 && error.status < 500) {
                notification.error(serverMessage || 'البيانات المدخلة غير صحيحة');
            } else if (error.status >= 500) {
                notification.error('حدث خطأ غير متوقع، يرجى المحاولة مرة أخرى');
            } else if (error.status === 0) {
                notification.error('تعذر الاتصال بالخادم، يرجى التحقق من اتصالك بالإنترنت');
            } else {
                notification.error('حدث خطأ غير متوقع، يرجى المحاولة مرة أخرى');
            }

            return throwError(() => error);
        })
    );
};
