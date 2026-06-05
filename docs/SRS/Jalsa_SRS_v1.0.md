# (Jalsa) - Software Requirements Specification

Arabic Mental Health Clinic Management System

---

| Field            | Detail                           |
|------------------|----------------------------------|
| Document Version | 1.0                              |
| Date             | June 2026                        |
| Status           | Draft - Internal Review          |
| Project          | Jalsa - ITI Capstone 2026        |
| Team             | Group 6 - .NET Track (5 members) |
| Classification   | Confidential                     |

---

## Table of Contents

1. [Introduction](#1-introduction)
2. [Overall System Description](#2-overall-system-description)
3. [Functional Requirements](#3-functional-requirements)
   - 3.1 [Authentication & Authorization](#31-module-1-authentication--authorization)
   - 3.2 [Patient Management](#32-module-2-patient-management)
   - 3.3 [Session Notes](#33-module-3-session-notes)
   - 3.4 [Exercise Tracking](#34-module-4-exercise-tracking)
   - 3.5 [Progress Dashboard](#35-module-5-progress-dashboard)
   - 3.6 [AI Referral Report Generator](#36-module-6-ai-referral-report-generator)
   - 3.7 [Patient Support Chatbot](#37-module-7-patient-support-chatbot)
4. [Non-Functional Requirements](#4-non-functional-requirements)
5. [AI Architecture Requirements](#5-ai-architecture-requirements)
6. [Data Model Overview](#6-data-model-overview)
7. [Key Use Cases](#7-key-use-cases)
8. [Risks and Mitigations](#8-risks-and-mitigations)
9. [Appendix](#9-appendix)

---

## 1. Introduction

### 1.1 Purpose

This Software Requirements Specification (SRS) defines the functional and non-functional requirements for **جلسة (Jalsa)**, a fully Arabic-language, web-based clinic management system designed for licensed psychological therapists in Egypt and the Gulf region.

This document serves as the authoritative reference for all development, testing, and stakeholder communication activities throughout the ITI Capstone 2026 project lifecycle.

### 1.2 Project Scope

جلسة digitizes the full daily workflow of a private-practice psychological therapist. The system covers:

- Patient profile management and digital intake forms
- Structured per-session note-taking with voice input
- Therapeutic exercise assignment and patient completion tracking
- Visual progress monitoring via charts and dashboards
- AI-powered Arabic referral report generation (core AI feature)
- Between-session patient emotional support chatbot with strict clinical guardrails

> **Out of Scope for v1.0**
>
> Billing / insurance management · Telehealth video sessions · Multi-clinic enterprise management · Native mobile applications (iOS / Android) · Integration with Egyptian Ministry of Health national databases

### 1.3 Document Conventions

| Convention        | Definition                                                             |
|-------------------|------------------------------------------------------------------------|
| **REQ-ID Prefix** | `FR` = Functional · `NFR` = Non-Functional · `AI` = AI-specific        |
| **Priority**      | `High` = Must-have MVP · `Medium` = Should-have · `Low` = Nice-to-have |
| **User Roles**    | Therapist (primary user) · Patient (secondary user)                    |
| **Language**      | All UI text in Arabic (RTL); API keys and code identifiers in English  |

### 1.4 Intended Audience

- Development team (5 full-stack .NET engineers)
- ITI project supervisors and evaluation committee
- Prospective therapist users involved in user testing
- Future contributors or investors

### 1.5 References

- JMIR Mental Health (2020) — Digital health readiness among Egyptian psychiatrists
- OWASP LLM Top 10 (2025 edition) — AI security guidelines
- ISO/IEC 29148:2018 — Systems and software requirements engineering
- IEEE Std 830-1998 — Recommended practice for SRS
- MindTrack Project Documentation (internal) — جلسة predecessor research

---

## 2. Overall System Description

### 2.1 Product Perspective

جلسة is a greenfield web application with no direct predecessor system. It replaces paper-based workflows currently used by the majority of Egyptian private-practice therapists. The system interfaces with:

- **Azure OpenAI (GPT-4o)** — language generation and vision analysis
- **Azure Whisper** — Arabic speech-to-text transcription
- **PostgreSQL + pgvector** — relational storage with semantic vector retrieval

### 2.2 User Classes and Characteristics

#### Therapist (Primary User)

| Attribute          | Detail                                                            |
|--------------------|-------------------------------------------------------------------|
| Role               | Licensed psychological therapist / specialist                     |
| Technical Literacy | Basic to intermediate — comfortable with web apps, not developers |
| Access Device      | Desktop / laptop computer in clinic setting                       |
| Language           | Arabic (primary), may understand some English terms               |
| Session Volume     | 5–12 patients per day in a busy private clinic                    |
| Key Pain Points    | Paper records, manual report writing, no progress tracking        |

#### Patient (Secondary User)

| Attribute       | Detail                                                                        |
|-----------------|-------------------------------------------------------------------------------|
| Role            | Current patient of a registered therapist                                     |
| Access Device   | Mobile browser or desktop                                                     |
| Interaction     | Between-session emotional support chat; exercise log submission               |
| Hard Constraint | Cannot access therapist notes or other patients' data under any circumstances |

### 2.3 Operating Environment

- **Browser Support:** Chrome 120+, Edge 120+, Firefox 120+, Safari 17+
- **Frontend:** Angular 17 SPA served via Azure App Service
- **Backend:** ASP.NET Core 8 Web API on Azure App Service (Linux container)
- **Database:** PostgreSQL 16 + pgvector extension on Azure Database for PostgreSQL
- **Cache:** Redis 7 on Azure Cache for Redis
- **Real-time:** SignalR over WebSockets (patient support chat)
- **AI:** Azure OpenAI — GPT-4o, text-embedding-ada-002, Whisper STT
- **CI/CD:** GitHub Actions → Docker → Azure App Service

### 2.4 Design and Implementation Constraints

- All user-facing text **MUST** be in Arabic with full RTL layout (Bootstrap 5 RTL)
- Patient data must never be accessible across therapist accounts (strict row-level isolation)
- AI chatbot must **NEVER** provide clinical diagnosis, prescriptions, or treatment plans
- System prompt injection attacks must be mitigated per OWASP LLM Top 10 #1
- Development timeline: 4 weeks with a 5-person intermediate team
- Tech stack is fixed: Angular 17, ASP.NET Core 8, EF Core, PostgreSQL, Semantic Kernel

---

## 3. Functional Requirements

### 3.1 Module 1: Authentication & Authorization

Secure, role-based access control for Therapist and Patient portals.

| REQ-ID     | Requirement Description                                                                                   | Priority | Source       |
|------------|-----------------------------------------------------------------------------------------------------------|----------|--------------|
| FR-AUTH-01 | Therapist shall register with full name, email, password, and license number                              | High     | Interview    |
| FR-AUTH-02 | System shall authenticate users with email/password and issue a JWT (HS256, 1-hour expiry)                | High     | Architecture |
| FR-AUTH-03 | System shall implement Refresh Token rotation with 7-day sliding expiry                                   | High     | Security     |
| FR-AUTH-04 | System shall enforce role-based access: Therapist role cannot access Patient portal routes and vice versa | High     | Security     |
| FR-AUTH-05 | Therapist may reset password via email magic link (6-hour expiry)                                         | Medium   | UX           |
| FR-AUTH-06 | System shall lock account for 15 minutes after 5 consecutive failed login attempts                        | Medium   | Security     |
| FR-AUTH-07 | Therapist profile page shall display license number, specialization, and total session count              | Low      | UX           |

---

### 3.2 Module 2: Patient Management

Full lifecycle management of patient profiles from intake to discharge.

| REQ-ID    | Requirement Description                                                                                                                                      | Priority | Source       |
|-----------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|--------------|
| FR-PAT-01 | Therapist shall create a new patient profile with: full name, date of birth, gender, contact info, referral source, and chief complaint                      | High     | Interview    |
| FR-PAT-02 | System shall generate a unique Patient-ID (UUID) for each patient upon creation                                                                              | High     | Architecture |
| FR-PAT-03 | Therapist shall complete a structured digital intake form including presenting problem, psychiatric history, family history, medications, and social history | High     | Interview    |
| FR-PAT-04 | Therapist shall record psychological assessment results (PHQ-9, GAD-7, Beck BDI, custom scales) with date stamps                                             | High     | Interview    |
| FR-PAT-05 | Therapist shall upload scanned paper forms; system triggers GPT-4o Vision to extract and auto-populate structured fields                                     | Medium   | AI Spec      |
| FR-PAT-06 | Therapist shall search patients by name, ID, or chief complaint with real-time filtering                                                                     | High     | UX           |
| FR-PAT-07 | Therapist shall archive (soft-delete) a patient; archived patients are excluded from default search but remain accessible                                    | Medium   | Interview    |
| FR-PAT-08 | System shall enforce strict data isolation: a therapist can only view and modify their own patients                                                          | High     | Security     |
| FR-PAT-09 | Patient list page shall show last session date, total sessions, and active exercise count as summary chips                                                   | Medium   | UX           |

---

### 3.3 Module 3: Session Notes

Structured, searchable session documentation entered after each therapy session.

| REQ-ID    | Requirement Description                                                                                                                     | Priority | Source    |
|-----------|---------------------------------------------------------------------------------------------------------------------------------------------|----------|-----------|
| FR-SES-01 | Therapist shall create a session note linked to a patient with: date, session number (auto-incremented), duration, and session type         | High     | Interview |
| FR-SES-02 | Session note shall include structured fields: observations, interventions used, patient response, homework assigned, and next session goals | High     | Interview |
| FR-SES-03 | Therapist shall record voice memos via Azure Whisper STT; transcribed Arabic text is inserted automatically into the note field             | High     | AI Spec   |
| FR-SES-04 | System shall auto-save session note drafts every 60 seconds to prevent data loss                                                            | Medium   | UX        |
| FR-SES-05 | Upon saving, system shall generate a 1536-dimension text embedding of the note and store it in pgvector for semantic retrieval              | High     | RAG Spec  |
| FR-SES-06 | Therapist shall view the full chronological history of all session notes for a given patient                                                | High     | Interview |
| FR-SES-07 | Therapist shall search session notes semantically using natural-language Arabic queries powered by pgvector hybrid search                   | Medium   | AI Spec   |
| FR-SES-08 | System shall display a warning if a new session is being created for a patient absent for over 90 days                                      | Low      | UX        |

---

### 3.4 Module 4: Exercise Tracking

Assignment and completion monitoring of between-session therapeutic tasks.

| REQ-ID   | Requirement Description                                                                                                    | Priority | Source    |
|----------|----------------------------------------------------------------------------------------------------------------------------|----------|-----------|
| FR-EX-01 | Therapist shall assign exercises to a patient with: title, description, frequency (daily/weekly), start date, and due date | High     | Interview |
| FR-EX-02 | Patient shall view assigned exercises in the patient portal and mark each as Completed, Partially Done, or Skipped         | High     | Interview |
| FR-EX-03 | Patient may submit a written reflection note (up to 500 characters) when logging an exercise                               | Medium   | Interview |
| FR-EX-04 | Therapist dashboard shall display per-exercise completion rate as a progress bar                                           | High     | UX        |
| FR-EX-05 | System shall send an in-app notification to the patient if an exercise is due in 24 hours and not yet completed            | Medium   | UX        |
| FR-EX-06 | Therapist shall deactivate or extend an exercise due date at any time                                                      | Medium   | Interview |
| FR-EX-07 | Exercise completion history shall be preserved and included in the AI Report Generator context                             | High     | AI Spec   |

---

### 3.5 Module 5: Progress Dashboard

Visual, therapist-facing overview of individual patient progress over time.

| REQ-ID     | Requirement Description                                                                                            | Priority | Source       |
|------------|--------------------------------------------------------------------------------------------------------------------|----------|--------------|
| FR-DASH-01 | Dashboard shall display a line chart of assessment scale scores (PHQ-9, GAD-7, etc.) plotted over time per patient | High     | Interview    |
| FR-DASH-02 | Dashboard shall show a session frequency bar chart (sessions per month) for the trailing 12 months                 | Medium   | UX           |
| FR-DASH-03 | Dashboard shall display exercise completion as a donut chart segmented by: completed / partially done / skipped    | Medium   | UX           |
| FR-DASH-04 | Therapist home page shall show today's appointments, total active patient count, and overdue exercise count        | High     | UX           |
| FR-DASH-05 | All charts shall be rendered using ngx-charts and support RTL axis labels in Arabic                                | High     | Architecture |

---

### 3.6 Module 6: AI Referral Report Generator

The core AI feature — one-click generation of a structured medical referral report in Arabic.

| REQ-ID   | Requirement Description                                                                                                                                                                      | Priority | Source      |
|----------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|-------------|
| FR-AI-01 | Therapist shall trigger report generation from a patient's profile page with a single button click                                                                                           | High     | Interview   |
| FR-AI-02 | Patient Context Agent shall retrieve and summarize all session notes, assessments, and exercise history for the patient                                                                      | High     | AI Spec     |
| FR-AI-03 | Report Generator Agent shall produce a structured Arabic referral report containing: presenting complaint, diagnostic impressions, interventions used, progress summary, and recommendations | High     | Interview   |
| FR-AI-04 | Generated report shall be editable in a rich-text editor before finalization; therapist may accept, modify, or discard the AI draft                                                          | High     | UX          |
| FR-AI-05 | Finalized report shall be exportable as a formatted PDF with the clinic header                                                                                                               | High     | Interview   |
| FR-AI-06 | System shall store all generated report versions with timestamp and therapist confirmation status                                                                                            | Medium   | Compliance  |
| FR-AI-07 | Generation time shall not exceed 30 seconds for a patient with up to 50 sessions (P95 target)                                                                                                | Medium   | Performance |
| FR-AI-08 | All AI-generated content shall include a visible Arabic disclaimer: *"هذا التقرير أُنشئ بمساعدة ذكاء اصطناعي ويجب مراجعته من الأخصائي"*                                                            | High     | Ethics      |

---

### 3.7 Module 7: Patient Support Chatbot

Between-session emotional support channel with strict, non-negotiable clinical guardrails.

| REQ-ID    | Requirement Description                                                                                                                              | Priority | Source         |
|-----------|-------------------------|----------|--------|
| FR-BOT-01 | Patient shall access an Arabic-language support chat in their portal, powered by GPT-4o with a hardened system prompt                                | High     | AI Spec        |
| FR-BOT-02 | Chatbot shall **NEVER** provide clinical diagnosis, medication advice, or treatment plans under any input framing                                    | High     | Ethics / Legal |
| FR-BOT-03 | Chatbot shall redirect all clinical questions explicitly: *"هذا السؤال يحتاج إجابة من أخصائيك — تواصل معه مباشرة"*                                        | High     | Ethics / Legal |
| FR-BOT-04 | Support Chat Agent shall maintain per-patient long-term memory (last 20 messages + patient profile summary) for context-aware responses              | High     | AI Spec        |
| FR-BOT-05 | System shall detect crisis keywords (e.g., إيذاء نفسي, انتحار) and immediately display crisis hotline info and send a silent alert to the therapist | High     | Safety         |
| FR-BOT-06 | Chat shall use SignalR for real-time message delivery with streamed token output                                                                     | Medium   | Architecture   |
| FR-BOT-07 | System shall log all chatbot exchanges with: patient ID, timestamp, token usage, and response latency for audit and adversarial review               | High     | Compliance     |
| FR-BOT-08 | Therapist shall view a per-patient chat engagement summary (not full message logs) in their dashboard                                                | Medium   | Interview      |

---

## 4. Non-Functional Requirements

### 4.1 Security

| REQ-ID     | Requirement                                                                                                                                   | Priority | Standard  |
|------------|-----------------------------------------------------------------------------------------------------------------------------------------------|----------|-----------|
| NFR-SEC-01 | All data in transit shall be encrypted with TLS 1.3                                                                                           | High     | HIPAA     |
| NFR-SEC-02 | All data at rest shall be encrypted using AES-256 (Azure managed keys)                                                                        | High     | HIPAA     |
| NFR-SEC-03 | Password storage shall use BCrypt with cost factor ≥ 12                                                                                       | High     | OWASP     |
| NFR-SEC-04 | All AI endpoints shall implement prompt injection defenses (OWASP LLM #1): input sanitization, output validation, and system-prompt isolation | High     | OWASP LLM |
| NFR-SEC-05 | API shall enforce rate limiting: 100 requests/minute per authenticated user; 10 AI generation calls/minute                                    | High     | Security  |
| NFR-SEC-06 | Row-level security shall ensure a therapist's JWT cannot query another therapist's patient rows                                               | High     | Privacy   |
| NFR-SEC-07 | All database queries shall use parameterized statements via EF Core to prevent SQL injection                                                  | High     | OWASP     |

### 4.2 Performance

| REQ-ID      | Requirement                                                                               | Target       | Priority |
|-------------|-------------------------------------------------------------------------------------------|--------------|----------|
| NFR-PERF-01 | Standard CRUD API endpoints shall respond at P95 under 50 concurrent users                | ≤ 300 ms     | High     |
| NFR-PERF-02 | Patient search shall return results for a therapist with up to 500 patients               | ≤ 500 ms     | High     |
| NFR-PERF-03 | AI referral report generation for patients with ≤ 50 sessions                             | ≤ 30 s (P95) | Medium   |
| NFR-PERF-04 | pgvector semantic search — top-5 chunks with HNSW index                                   | ≤ 200 ms     | Medium   |
| NFR-PERF-05 | Redis shall cache patient summary context to reduce repeated DB load on report generation | 5-minute TTL | Medium   |

### 4.3 Usability

| REQ-ID     | Requirement                                                                                                | Priority |
|------------|------------------------------------------------------------------------------------------------------------|----------|
| NFR-USE-01 | All UI text, labels, error messages, and notifications shall be in Arabic                                  | High     |
| NFR-USE-02 | Layout shall render correctly in RTL direction using Bootstrap 5 RTL stylesheet                            | High     |
| NFR-USE-03 | A therapist with no prior digital system experience shall complete patient registration in under 5 minutes | Medium   |
| NFR-USE-04 | System shall display inline Arabic validation messages for all form fields                                 | Medium   |
| NFR-USE-05 | Loading states and skeleton screens shall be shown for all async operations exceeding 300 ms               | Low      |

### 4.4 Reliability & Availability

| REQ-ID     | Requirement                                                                                                        | Priority |
|------------|--------------------------------------------------------------------------------------------------------------------|----------|
| NFR-REL-01 | System target uptime: 99% monthly (excluding planned maintenance windows)                                          | High     |
| NFR-REL-02 | Session note auto-save shall prevent loss of more than 60 seconds of therapist input                               | High     |
| NFR-REL-03 | If Azure OpenAI is unavailable, system shall display Arabic fallback message: *"خدمة الذكاء الاصطناعي غير متاحة حالياً"* | Medium   |
| NFR-REL-04 | Database backups shall be automated daily with 7-day retention via Azure managed backup                            | High     |

### 4.5 Maintainability

| REQ-ID      | Requirement                                                                                                                        | Priority |
|-------------|------------------------------------------------------------------------------------------------------------------------------------|----------|
| NFR-MAIN-01 | Backend shall follow Clean Architecture with clear layer separation: Core → Infrastructure → API                                   | High     |
| NFR-MAIN-02 | All AI system prompts shall be stored as external string constants — not hardcoded inline — to allow updates without recompilation | High     |
| NFR-MAIN-03 | Langfuse shall log all LLM calls (model, token count, latency, estimated cost) for observability and prompt debugging              | Medium   |
| NFR-MAIN-04 | Sentry shall capture all unhandled exceptions in both frontend and backend with Arabic-locale context                              | Medium   |
| NFR-MAIN-05 | GitHub Actions CI pipeline shall run on every PR: build + unit tests; no merge permitted without green CI                          | High     |

---

## 5. AI Architecture Requirements

### 5.1 RAG Pipeline Specification

| Parameter              | Specification                                                                  |
|------------------------|--------------------------------------------------------------------------------|
| **Data Sources**       | Session notes, intake forms, assessment results, exercise completion logs      |
| **Chunking Strategy**  | Per-session chunks with 128-token sliding window overlap                       |
| **Chunk Size**         | 512–1024 tokens (adaptive based on session note length)                        |
| **Embedding Model**    | `text-embedding-ada-002` — 1536 dimensions via Azure OpenAI                    |
| **Vector Store**       | PostgreSQL + pgvector with HNSW index (`ef_construction=64`, `m=16`)           |
| **Retrieval Strategy** | Hybrid: cosine semantic similarity + BM25 keyword search, re-ranked by recency |
| **Top-K**              | 5 most relevant chunks per query                                               |
| **Context Window**     | Max 8,000 tokens passed to GPT-4o (retrieved chunks + patient metadata)        |

### 5.2 Multi-Agent Sequential Pipeline

The AI Referral Report system uses a **3-agent Sequential Pipeline** implemented via Semantic Kernel Agents for .NET. Each agent's output becomes the next agent's input.

```graph

[Therapist clicks "Generate Report"]
         │
         ▼
┌─────────────────────────┐
│  Agent 1                │
│  Patient Context Agent  │
│                         │
│  • Pulls all sessions,  │
│    assessments, exercises│
│  • Runs pgvector RAG    │
│  • Produces structured  │
│    context JSON         │
└───────────┬─────────────┘
            │ context JSON
            ▼
┌─────────────────────────┐
│  Agent 2                │
│  Report Generator Agent │
│                         │
│  • Calls GPT-4o with    │
│    medical report prompt│
│  • Generates structured │
│    Arabic referral text │
│  • Saves as draft       │
└───────────┬─────────────┘
            │ draft report
            ▼
┌─────────────────────────┐
│  Therapist Review       │
│  Rich-text editor       │
│  Accept / Edit / Discard│
└─────────────────────────┘

[Separate — Patient Portal]
┌─────────────────────────┐
│  Agent 3                │
│  Support Chat Agent     │
│                         │
│  • Per-patient memory   │
│    (last 20 messages)   │
│  • Streams via SignalR  │
│  • Hard guardrails      │
│  • Crisis detection     │
└─────────────────────────┘

```

#### Agent 1 — Patient Context Agent

- **Input:** Patient UUID from therapist request
- **Actions:** Retrieves all session notes, assessments, and exercise logs from DB; executes pgvector hybrid search for most clinically relevant chunks; produces structured JSON context object
- **Output:** Structured patient context JSON → passed to Agent 2

#### Agent 2 — Report Generator Agent

- **Input:** Patient context JSON from Agent 1
- **Actions:** Calls GPT-4o with a medical referral report system prompt in Arabic; generates structured output with defined clinical sections
- **Output:** Arabic markdown report text stored in `ReferralReports` table with `status = draft`

#### Agent 3 — Support Chat Agent

- **Input:** Patient message + last 20 messages + patient profile summary from memory store
- **Actions:** Responds empathetically in Arabic; enforces guardrails via hardened system prompt; detects crisis keywords and triggers escalation function call
- **Output:** Streamed token response delivered via SignalR

### 5.3 Prompt Injection Defense — OWASP LLM #1

> **⚠️ Security Requirement — All AI Endpoints**
>
> All user input passed to LLM calls must be:
>
> 1. Sanitized to remove control characters and instruction-like tokens
> 2. Inserted into a dedicated user-content slot, **separated** from the system prompt using XML-like delimiters
> 3. Validated that the output does not contain instruction-following artifacts
>
> Patient names and session content must **never** appear in system prompt construction. Adversarial testing (red-teaming) must be performed before final submission.

### 5.4 Multimodal Capabilities

| Capability               | Implementation | Use Case                                                                                                                                                                                      |
|--------------------------|----------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Speech-to-Text (STT)** | Azure Whisper  | Therapist records post-session voice memo in Arabic; transcription is inserted into the session note text field automatically                                                                 |
| **Vision / OCR**         | GPT-4o Vision  | Therapist uploads image of legacy paper intake form; model extracts structured fields (name, DOB, chief complaint) and populates the digital intake form; human review required before saving |

---

## 6. Data Model Overview

### 6.1 Core Entities

```graph

Therapists
├── id              UUID PK
├── email           TEXT UNIQUE NOT NULL
├── password_hash   TEXT NOT NULL
├── full_name       TEXT NOT NULL
├── license_number  TEXT NOT NULL
├── specialization  TEXT
└── created_at      TIMESTAMPTZ

Patients
├── id              UUID PK
├── therapist_id    UUID FK → Therapists
├── full_name       TEXT NOT NULL
├── dob             DATE
├── gender          TEXT
├── contact_info    JSONB
├── chief_complaint TEXT
├── status          TEXT ('active' | 'archived')
├── intake_data     JSONB          -- flexible schema for intake form
└── created_at      TIMESTAMPTZ

Sessions
├── id              UUID PK
├── patient_id      UUID FK → Patients
├── session_number  INTEGER        -- auto-incremented per patient
├── date            DATE NOT NULL
├── duration_min    INTEGER
├── session_type    TEXT
├── notes           TEXT
├── interventions   TEXT
├── patient_response TEXT
└── next_goals      TEXT

Assessments
├── id              UUID PK
├── patient_id      UUID FK → Patients
├── scale_type      TEXT           -- 'PHQ-9' | 'GAD-7' | 'BDI' | custom
├── result_data     JSONB          -- flexible scoring schema
└── administered_at TIMESTAMPTZ

Exercises
├── id              UUID PK
├── session_id      UUID FK → Sessions
├── patient_id      UUID FK → Patients
├── title           TEXT NOT NULL
├── description     TEXT
├── frequency       TEXT           -- 'daily' | 'weekly'
├── due_date        DATE
└── is_active       BOOLEAN DEFAULT TRUE

ExerciseLogs
├── id              UUID PK
├── exercise_id     UUID FK → Exercises
├── patient_id      UUID FK → Patients
├── status          TEXT           -- 'completed' | 'partial' | 'skipped'
├── note            TEXT           -- patient reflection (max 500 chars)
└── logged_at       TIMESTAMPTZ

ReferralReports
├── id              UUID PK
├── patient_id      UUID FK → Patients
├── content         TEXT           -- Arabic markdown report
├── status          TEXT           -- 'draft' | 'finalized'
├── generated_at    TIMESTAMPTZ
└── confirmed_at    TIMESTAMPTZ

SessionEmbeddings
├── id              UUID PK
├── session_id      UUID FK → Sessions
├── chunk_text      TEXT
├── embedding       vector(1536)   -- pgvector column
└── chunk_index     INTEGER

ChatMessages
├── id              UUID PK
├── patient_id      UUID FK → Patients
├── role            TEXT           -- 'user' | 'assistant'
├── content         TEXT
├── tokens_used     INTEGER
└── created_at      TIMESTAMPTZ

```

### 6.2 Key Design Decisions

| Decision                                      | Rationale                                                                |
|-----------------------------------------------|--------------------------------------------------------------------------|
| `intake_data` as JSONB                        | Intake form schema evolves without requiring DB migrations               |
| `result_data` as JSONB in Assessments         | Supports arbitrary scale types and scoring structures                    |
| Soft-delete on Patients (`status = archived`) | Preserves full data history for audit and compliance                     |
| UUID primary keys throughout                  | Prevents sequential ID enumeration attacks on API endpoints              |
| Row-level security at application layer       | `therapist_id` FK checked on every query — no cross-account data leakage |
| pgvector HNSW index                           | Approximate nearest-neighbor search with sub-200ms latency at scale      |

---

## 7. Key Use Cases

### UC-01: Returning Patient After Long Absence

| Field            | Detail                                                  |
|------------------|---------------------------------------------------------|
| **Actor**        | Therapist                                               |
| **Trigger**      | Patient who was absent for 8 months books a new session |
| **Precondition** | Patient has an existing profile with ≥ 1 prior session  |

**Main Flow:**

1. Therapist opens patient profile
2. System displays timeline of all past sessions with session numbers and dates
3. Therapist uses semantic search (e.g., *"مشاكل النوم"*) to retrieve relevant past notes
4. Therapist reviews AI-generated session history summary from the last 3 sessions
5. Therapist creates a new session note based on retrieved context

**Outcome:** Therapist is fully briefed without asking the patient to repeat their history — eliminating the professionally uncomfortable scenario documented in user interviews.

---

### UC-02: Generating a Referral Report

| Field            | Detail                                                                    |
|------------------|---------------------------------------------------------------------------|
| **Actor**        | Therapist                                                                 |
| **Trigger**      | Patient requires psychiatric medication — referral to psychiatrist needed |
| **Precondition** | Patient has ≥ 3 session notes and at least one recorded assessment result |

**Main Flow:**

1. Therapist clicks **"إنشاء تقرير إحالة"** on the patient profile page
2. System triggers the 3-agent pipeline (estimated 15–30 seconds)
3. AI draft report appears in an editable rich-text editor with the mandatory Arabic disclaimer
4. Therapist reviews the draft, makes edits if necessary, and clicks **"تأكيد"**
5. System saves the finalized report and generates a formatted PDF

**Outcome:** A professional Arabic referral report is ready in under 2 minutes — compared to 60+ minutes of manual work from scattered paper notes.

---

### UC-03: Patient Crisis Detection

| Field            | Detail                                                                         |
|------------------|--------------------------------------------------------------------------------|
| **Actor**        | Patient                                                                        |
| **Trigger**      | Patient sends a message containing crisis-related keywords in the support chat |
| **Precondition** | Patient is authenticated and the chat is active                                |

**Main Flow:**

1. Patient types a message containing *"أريد إيذاء نفسي"*
2. System detects the crisis keyword pattern **before** sending to the LLM
3. System immediately displays crisis resources (خط نجدة الصحة النفسية)
4. System sends a silent priority notification to the therapist's dashboard
5. Chatbot responds with empathy and explicitly directs the patient to contact their therapist or emergency services

**Outcome:** Patient receives immediate structured support; therapist is alerted; no clinical advice is generated by the AI; the system escalates without replacing human care.

---

## 8. Risks and Mitigations

| Risk                                         | Level   | Impact                                                      | Mitigation                                                                                                    |
|----------------------------------------------|---------|-------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------|
| Patient data breach                          |  High   | Legal liability, reputational damage, loss of patient trust | AES-256 at rest, TLS 1.3, UUID PKs, row-level security, parameterized queries only                            |
| Chatbot boundary violation (clinical advice) |  High   | Direct clinical harm to vulnerable patient                  | Hardened system prompt, crisis keyword filter, adversarial testing sprint, Langfuse monitoring of all outputs |
| RAG quality degradation on dialectal Arabic  |  Medium | Inaccurate or hallucinated referral report content          | Test embeddings on Egyptian dialect text; include session date in metadata for recency-weighted filtering     |
| Therapist adoption resistance                |  Medium | Low product usage — the core business failure mode          | Keep UX minimal; voice input (STT) reduces data-entry friction; onboarding wizard guides first session        |
| Azure OpenAI quota limits / rate limits      |  Medium | Report generation failures at scale                         | Implement queue with exponential-backoff retry; cache patient context summaries in Redis                      |
| 4-week timeline overrun                      |  Medium | Incomplete feature set at ITI submission                    | Core CRUD modules in Weeks 1–2; AI features in Weeks 3–4; chatbot is last priority; scope cut if needed       |

---

## 9. Appendix

### 9.1 Requirements Traceability Matrix

| Source                        | Requirement IDs                                                                                                                      |
|-------------------------------|--------------------------------------------------------------------------------------------------------------------------------------|
| **User Interviews**           | FR-AUTH-01, FR-PAT-01–04, FR-PAT-06–07, FR-SES-01–02, FR-SES-06, FR-EX-01–03, FR-EX-06, FR-DASH-04, FR-AI-01, FR-AI-04–05, FR-BOT-08 |
| **AI / RAG Specification**    | FR-PAT-05, FR-SES-03, FR-SES-05, FR-SES-07, FR-EX-07, FR-AI-02–03, FR-BOT-01, FR-BOT-04                                              |
| **Security / Ethics / Legal** | FR-AUTH-02–04, FR-AUTH-06, FR-PAT-08, FR-BOT-02–03, FR-BOT-05, FR-BOT-07, FR-AI-08                                                   |
| **UX Research**               | FR-AUTH-05, FR-PAT-09, FR-SES-04, FR-SES-08, FR-EX-04–05, FR-DASH-01–03, FR-DASH-05, FR-AI-06–07                                     |

### 9.2 Glossary

| Term                | Definition                                                                                                                       |
|---------------------|----------------------------------------------------------------------------------------------------------------------------------|
| **RAG**             | Retrieval-Augmented Generation — AI technique that retrieves relevant context from a knowledge base before generating a response |
| **pgvector**        | PostgreSQL extension for storing and querying high-dimensional vector embeddings natively                                        |
| **Semantic Kernel** | Microsoft's open-source AI orchestration SDK for .NET applications                                                               |
| **HNSW**            | Hierarchical Navigable Small World — approximate nearest-neighbor index algorithm used by pgvector for fast vector search        |
| **OWASP LLM #1**    | Prompt Injection — the top-ranked AI security risk per the OWASP LLM Top 10 guidelines                                           |
| **RTL**             | Right-to-Left — text direction used for Arabic, Hebrew, and Persian language rendering                                           |
| **SignalR**         | ASP.NET Core library for real-time bidirectional communication over WebSockets                                                   |
| **Guardrail**       | A hard constraint placed on an AI model to prevent specific categories of output regardless of input framing                     |
| **JWT**             | JSON Web Token — a compact, digitally-signed token used for stateless authentication                                             |
| **EF Core**         | Entity Framework Core — Microsoft's ORM for .NET database access using LINQ                                                      |
| **Soft-delete**     | A deletion pattern where records are marked as inactive rather than physically removed, preserving data for audit trails         |

### 9.3 Document Revision History

| Version | Date      | Changes                                                                | Author                 |
|---------|-----------|------------------------------------------------------------------------|------------------------|
| v1.0    | June 2026 | Full requirements, AI architecture, use cases, data model, risk matrix | Group 6 — ITI Capstone |

---

> **Document Status:** This SRS is version 1.0 (Draft — Internal Review).
> All requirements marked **High** priority are committed for the 4-week MVP development sprint.
> Medium and Low priority items are subject to scope review at the end of Week 2.
>
> *جلسة — ITI Capstone 2026 · Group 6 · Confidential*
