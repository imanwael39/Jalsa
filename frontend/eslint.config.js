const tseslint = require('typescript-eslint');
const angularEslint = require('@angular-eslint/eslint-plugin');
const angularTemplateEslint = require('@angular-eslint/eslint-plugin-template');
const angularTemplateParser = require('@angular-eslint/template-parser');
const eslintConfigPrettier = require('eslint-config-prettier');

module.exports = tseslint.config(
    {
        ignores: ['projects/**/*', 'dist/**', '.angular/**', 'node_modules/**'],
    },
    ...tseslint.configs.recommended.map(c => ({
        ...c,
        files: ['**/*.ts'],
    })),
    {
        files: ['**/*.ts'],
        plugins: {
            '@angular-eslint': angularEslint,
        },
        rules: {
            '@angular-eslint/directive-selector': [
                'error',
                { type: 'attribute', prefix: 'app', style: 'camelCase' },
            ],
            '@angular-eslint/component-selector': [
                'error',
                { type: 'element', prefix: 'app', style: 'kebab-case' },
            ],
            '@typescript-eslint/no-unused-vars': [
                'error',
                { argsIgnorePattern: '^_' },
            ],
            '@typescript-eslint/explicit-function-return-type': 'warn',
            '@typescript-eslint/no-explicit-any': 'error',
        },
    },
    {
        files: ['**/*.html'],
        plugins: {
            '@angular-eslint/template': angularTemplateEslint,
        },
        languageOptions: {
            parser: {
                parse: angularTemplateParser.parse,
                parseForESLint: angularTemplateParser.parseForESLint,
            },
        },
        rules: {
            '@angular-eslint/template/banana-in-box': 'error',
            '@angular-eslint/template/no-negated-async': 'error',
        },
    },
    eslintConfigPrettier,
);
