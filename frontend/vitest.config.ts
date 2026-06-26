/// <reference types="vitest" />
import { defineConfig } from 'vitest/config';
import { getViteConfig } from 'astro/config';

export default defineConfig({
    test: {
        globals: true,
        environment: 'jsdom',
        include: ['src/**/*.spec.ts'],
        setupFiles: ['src/test-setup.ts'],
    },
});