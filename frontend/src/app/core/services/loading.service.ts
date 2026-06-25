import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
    private loadingSignal = signal<boolean>(false);
    readonly isLoading = this.loadingSignal.asReadonly();

    private loadingCount = 0;

    show() {
        this.loadingCount++;
        if (this.loadingCount === 1) {
            this.loadingSignal.set(true);
        }
    }

    hide() {
        this.loadingCount--;
        if (this.loadingCount <= 0) {
            this.loadingCount = 0;
            this.loadingSignal.set(false);
        }
    }

    reset() {
        this.loadingCount = 0;
        this.loadingSignal.set(false);
    }
}
