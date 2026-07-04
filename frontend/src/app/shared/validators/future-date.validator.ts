import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/** Mirrors the backend's "DueDate must be greater than today" FluentValidation rule. */
export const futureDateValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) return null;

    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const value = new Date(control.value);

    return value > today ? null : { notFutureDate: true };
};
