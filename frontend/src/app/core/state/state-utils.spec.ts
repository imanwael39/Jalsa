import { createState, createAsyncState } from './state-utils';

describe('createState', () => {
    it('should create state with initial value', () => {
        const counter = createState(0);
        expect(counter.state()).toBe(0);
    });

    it('should set new value', () => {
        const counter = createState(0);
        counter.set(5);
        expect(counter.state()).toBe(5);
    });

    it('should update value with function', () => {
        const counter = createState(0);
        counter.update(current => current + 1);
        expect(counter.state()).toBe(1);
    });

    it('should reset to initial value', () => {
        const counter = createState(10);
        counter.set(20);
        counter.reset();
        expect(counter.state()).toBe(10);
    });
});

describe('createAsyncState', () => {
    it('should create async state with defaults', () => {
        const state = createAsyncState<string>();
        expect(state.data()).toBeNull();
        expect(state.loading()).toBe(false);
        expect(state.error()).toBeNull();
    });

    it('should set data and clear error', () => {
        const state = createAsyncState<string>();
        state.setData('loaded');
        expect(state.data()).toBe('loaded');
        expect(state.error()).toBeNull();
    });

    it('should set error and clear data', () => {
        const state = createAsyncState<string>();
        state.setData('loaded');
        state.setError('Failed');
        expect(state.data()).toBeNull();
        expect(state.error()).toBe('Failed');
    });

    it('should manage loading state', () => {
        const state = createAsyncState<string>();
        state.startLoading();
        expect(state.loading()).toBe(true);
        state.stopLoading();
        expect(state.loading()).toBe(false);
    });

    it('should reset all state', () => {
        const state = createAsyncState<string>();
        state.setData('loaded');
        state.startLoading();
        state.reset();
        expect(state.data()).toBeNull();
        expect(state.loading()).toBe(false);
        expect(state.error()).toBeNull();
    });
});
