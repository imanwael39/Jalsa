import { signal, WritableSignal } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';

export class BaseStateService<T> {
    protected dataSignal: WritableSignal<T | null> = signal(null);
    protected loadingSignal: WritableSignal<boolean> = signal(false);
    protected errorSignal: WritableSignal<string | null> = signal(null);

    readonly data = this.dataSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    protected setData(data: T): void {
        this.dataSignal.set(data);
        this.errorSignal.set(null);
    }

    protected setError(error: string): void {
        this.errorSignal.set(error);
    }

    protected startLoading(): void {
        this.loadingSignal.set(true);
        this.errorSignal.set(null);
    }

    protected stopLoading(): void {
        this.loadingSignal.set(false);
    }

    protected handleObservable<R>(obs: Observable<R>, _onSuccess: (data: R) => void): Observable<R> {
        this.startLoading();
        return obs.pipe(
            catchError(err => {
                this.setError(err.message || 'حدث خطأ غير متوقع');
                this.stopLoading();
                throw err;
            }),
            finalize(() => {
                this.stopLoading();
            })
        );
    }
}
