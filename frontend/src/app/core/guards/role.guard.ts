import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
    return () => {
        const authService = inject(AuthService);
        const router = inject(Router);

        if (allowedRoles.some(role => authService.hasRole(role))) {
            return true;
        }

        router.navigate(['/forbidden']);
        return false;
    };
};
