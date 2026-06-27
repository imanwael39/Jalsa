# Jalsa – Project Overview

## Purpose
Jalsa is a large‑scale healthcare/mental‑health platform that connects therapists and patients. It provides tools for managing patient records, session notes, exercise prescriptions, AI‑generated referral reports, real‑time chat support, and progress analytics.

## User Roles
- **Therapist**: Primary user. Manages patients, creates sessions, assigns exercises, reviews AI‑generated reports, monitors patient chat.
- **Patient**: End‑user receiving therapy. Views exercises, completes assignments, participates in AI‑powered chat support.
- **Administrator**: System admin. Manages users, monitors system health, configures settings.

## Key Features (Modules)
1. **Authentication** – login, registration, password reset, profile management.
2. **Patient Management** – CRUD, intake forms (with OCR), assessments.
3. **Session Management** – notes, voice memos, AI‑generated summaries.
4. **Exercise Management** – assign exercises to patients, track status, patient reflection.
5. **AI Reports** – generate referral reports using AI, review, approve, export PDF.
6. **Dashboard & Analytics** – charts and stats for therapist overview.
7. **Chatbot** – real‑time AI‑powered chat for patients with crisis alerts; therapist monitoring.

## Technology Stack
- **Framework**: Angular 17
- **Architecture**: Standalone components (no NgModules)
- **Language**: TypeScript (strict mode)
- **Styling**: **Plain CSS** with Bootstrap 5 and RTL support (no preprocessors)
- **State Management**: Angular Signals + RxJS services (no NgRx)
- **API Communication**: HttpClient with functional interceptors (auth, error, loading)
- **Real‑time**: SignalR for chat
- **Charts**: Chart.js via ng2-charts
- **Authentication**: JWT (stored in localStorage)
- **Internationalisation**: Arabic (RTL) with full RTL support

## Key Non‑Functional Requirements
- **RTL‑first**: All UI must support RTL layout and mirror correctly.
- **Accessibility**: WCAG 2.1 AA level compliance.
- **Performance**: Lighthouse scores ≥ 90 for Performance, Accessibility, Best Practices.
- **Responsive**: Mobile‑first, works on all screen sizes.
- **Security**: JWT token management, role‑based access control, XSS protection.

## Development Principles
- **Standalone First**: Every component, directive, pipe, and guard must be standalone.
- **Lazy Loading**: All feature modules must be lazy‑loaded.
- **Type Safety**: Use strict TypeScript, avoid `any`.
- **Reactive**: Prefer Signals for state, RxJS for async operations.
- **Consistency**: Follow a single design system and coding style.