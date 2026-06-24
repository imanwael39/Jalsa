# Frontend

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 21.2.13.

## Prerequisites

- Node.js 18+
- npm 9+
- Git

## Setup

```bash
# Install dependencies
npm install

# The prepare script will automatically configure Git hooks
# If hooks are not working, run manually:
git config core.hooksPath frontend/.husky
```

## Development server

```bash
ng serve
```

Navigate to `http://localhost:4200/`. The application will automatically reload when you modify source files.

## Code scaffolding

```bash
ng generate component component-name
```

For a complete list of available schematics, run:

```bash
ng generate --help
```

## Building

```bash
# Development build
ng build

# Production build
ng build --configuration production
```

## Running tests

```bash
# Run unit tests
ng test

# CI mode (single run, no watch)
npm run test:ci
```

## Linting and Formatting

```bash
# Lint TypeScript and HTML files
npm run lint

# Auto-fix lint errors
npm run lint:fix

# Format code with Prettier
npm run format
```

## Git Hooks

This project uses [Husky](https://typicode.github.io/husky/) with [lint-staged](https://github.com/okonet/lint-staged) and [commitlint](https://commitlint.js.org/):

- **pre-commit**: Runs lint-staged (ESLint + Prettier) on staged files
- **commit-msg**: Validates commit messages against Conventional Commits format

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `chore`, `ci`, `build`

## Project Structure

```
src/
├── app/
│   ├── core/          # Singleton services, guards, interceptors, models
│   ├── shared/        # Reusable UI components, directives, pipes
│   └── features/      # Lazy-loaded feature modules
├── assets/            # Static assets (images, mock data)
├── environments/      # Environment configuration
└── styles/            # Global styles and design system
```

## Additional Resources

- [Angular CLI Overview](https://angular.dev/tools/cli)
- [Project Architecture Docs](../../docs/architecture/)
- [Design System Docs](../../docs/design-system/)
