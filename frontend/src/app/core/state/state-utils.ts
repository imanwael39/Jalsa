import { signal } from '@angular/core';

export function createState<T>(initialValue: T) {
    const state = signal(initialValue);
    return {
        state: state.asReadonly(),
        set: (value: T) => state.set(value),
        update: (fn: (current: T) => T) => state.update(fn),
        reset: () => state.set(initialValue),
    };
}

export function createAsyncState<T>() {
    const data = signal<T | null>(null);
    const loading = signal(false);
    const error = signal<string | null>(null);

    return {
        data: data.asReadonly(),
        loading: loading.asReadonly(),
        error: error.asReadonly(),
        setData: (value: T) => {
            data.set(value);
            error.set(null);
        },
        setError: (err: string) => {
            error.set(err);
            data.set(null);
        },
        startLoading: () => loading.set(true),
        stopLoading: () => loading.set(false),
        reset: () => {
            data.set(null);
            loading.set(false);
            error.set(null);
        },
    };
}
