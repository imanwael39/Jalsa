import { Signal, signal } from '@angular/core';

export interface StateHandle<T> {
    state: Signal<T>;
    set: (value: T) => void;
    update: (fn: (current: T) => T) => void;
    reset: () => void;
}

export interface AsyncStateHandle<T> {
    data: Signal<T | null>;
    loading: Signal<boolean>;
    error: Signal<string | null>;
    setData: (value: T) => void;
    setError: (err: string) => void;
    startLoading: () => void;
    stopLoading: () => void;
    reset: () => void;
}

export function createState<T>(initialValue: T): StateHandle<T> {
    const state = signal(initialValue);
    return {
        state: state.asReadonly(),
        set: (value: T): void => state.set(value),
        update: (fn: (current: T) => T): void => state.update(fn),
        reset: (): void => state.set(initialValue),
    };
}

export function createAsyncState<T>(): AsyncStateHandle<T> {
    const data = signal<T | null>(null);
    const loading = signal(false);
    const error = signal<string | null>(null);

    return {
        data: data.asReadonly(),
        loading: loading.asReadonly(),
        error: error.asReadonly(),
        setData: (value: T): void => {
            data.set(value);
            error.set(null);
        },
        setError: (err: string): void => {
            error.set(err);
            data.set(null);
        },
        startLoading: (): void => loading.set(true),
        stopLoading: (): void => loading.set(false),
        reset: (): void => {
            data.set(null);
            loading.set(false);
            error.set(null);
        },
    };
}
