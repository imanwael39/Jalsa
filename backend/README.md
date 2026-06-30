# Jalsa Backend

ASP.NET Core 8 Web API for the Jalsa mental health clinic management system.

## Architecture

Clean Architecture with four layers:

```
Jalsa.Domain          → Entity models (33 entities)
Jalsa.Application     → Services, DTOs, interfaces, validators
Jalsa.Infrastructure  → EF Core DbContext, repositories, migrations
Jalsa.API             → Controllers, auth, AI services, hubs, middleware
Jalsa.Tests           → xUnit + Moq + FluentAssertions (154 tests)
```

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or Express)
- Node.js 22+ (for frontend, optional)

## Setup

1. **Clone and navigate:**
   ```bash
   cd backend
   ```

2. **Configure secrets** — create a `.env` file in `Jalsa.API/`:
   ```env
   Jwt__Key=your-256-bit-secret-key-here
   ConnectionStrings__DefaultConnection=Server=localhost\\SQLEXPRESS;Database=Jalsa_DB;Trusted_Connection=True;TrustServerCertificate=True;
   OpenAI__ApiKey=your-openai-api-key
   ```

3. **Restore and build:**
   ```bash
   dotnet restore backend.sln
   dotnet build backend.sln
   ```

4. **Run migrations** (auto-applied on startup via `db.Database.Migrate()`):
   ```bash
   dotnet run --project Jalsa.API
   ```

5. **Access:**
   - API: `http://localhost:5014`
   - Swagger: `http://localhost:5014/swagger`
   - Hangfire: `http://localhost:5014/hangfire` (Admin role required)

## Environment Configuration

| Environment | File | CORS Origins |
|------------|------|-------------|
| Development | `appsettings.Development.json` | `http://localhost:4200` |
| Staging | `appsettings.Staging.json` | `https://staging.jalsa.com` |
| Production | `appsettings.Production.json` | `https://jalsa.com` |

Set environment via `ASPNETCORE_ENVIRONMENT` variable.

## API Endpoints

### Auth (`/api/auth`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/register` | — | Register therapist/patient |
| POST | `/login` | — | Login (returns JWT + refresh token) |
| POST | `/refresh` | — | Refresh access token |
| POST | `/revoke` | — | Revoke refresh token |
| POST | `/forgot-password` | — | Send password reset email |
| POST | `/reset-password` | — | Reset password with token |
| GET | `/profile` | Bearer | Get current user profile |
| PUT | `/profile` | Bearer | Update profile |

### Patients (`/api/patient`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/` | Therapist | List therapist's patients |
| GET | `/{id}` | Therapist | Get patient detail |
| POST | `/` | Therapist | Create patient |
| PUT | `/{id}` | Therapist | Update patient |
| PATCH | `/{id}/archive` | Therapist | Archive patient |
| PATCH | `/{id}/restore` | Therapist | Restore patient |
| DELETE | `/{id}` | Therapist | Delete patient |

### Sessions (`/api/sessions`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/` | Therapist | Create session |
| GET | `/{id}` | Therapist | Get session detail |
| GET | `/patient/{patientId}` | Therapist | List patient sessions |
| PUT | `/{id}` | Therapist | Update session |
| DELETE | `/{id}` | Therapist | Delete session |
| POST | `/{id}/note` | Therapist | Create/update session note |
| GET | `/{id}/note` | Therapist | Get session note |
| POST | `/{id}/voice` | Therapist | Upload voice memo (Whisper STT) |
| GET | `/{id}/summary` | Therapist | AI session summary (GPT-4o) |

### Exercises (`/api/exercises`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/` | Therapist | List exercises |
| GET | `/{id}` | Therapist | Get exercise detail |
| GET | `/patient/{patientId}` | Therapist | List patient exercises |
| POST | `/` | Therapist | Create exercise |
| PUT | `/{id}` | Therapist | Update exercise |
| DELETE | `/{id}` | Therapist | Delete exercise |
| PUT | `/{id}/extend` | Therapist | Extend due date |
| GET | `/my` | Patient | My assigned exercises |
| POST | `/log` | Patient | Log exercise completion |
| GET | `/my/logs` | Patient | My exercise logs |

### Reports (`/api/reports`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/generate` | Therapist | Generate AI report draft |
| GET | `/{id}` | Therapist | Get report |
| GET | `/patient/{patientId}` | Therapist | List patient reports |
| PUT | `/{id}` | Therapist | Update report |
| POST | `/{id}/approve` | Therapist | Approve report |
| POST | `/{id}/reject` | Therapist | Reject report |
| GET | `/{id}/export` | Therapist | Export as HTML |
| DELETE | `/{id}` | Therapist | Delete report |

### Assessments (`/api/patient/{id}/assessments`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/` | Therapist | List patient assessments |
| POST | `/` | Therapist | Create assessment |

### Intake (`/api/patient/{id}/intake`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/` | Therapist | Get intake form |
| POST | `/` | Therapist | Save intake form |
| POST | `/submit` | Therapist | Submit intake form |
| POST | `/{intakeFormId}/ocr` | Therapist | OCR extraction |

### AI (`/api/ai`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/summarize/{patientId}` | Therapist | Summarize patient data |
| POST | `/report-draft/{patientId}` | Therapist | Generate report draft |

### Dashboard (`/api/progress`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/dashboard` | Therapist | Dashboard analytics |

### Chat (`/api/chat`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/conversations` | Bearer | List conversations |
| GET | `/{conversationId}/history` | Bearer | Chat history |
| POST | `/conversations` | Bearer | Create conversation |
| PATCH | `/conversations/{conversationId}/close` | Bearer | Close conversation |
| POST | `/send` | Bearer | Send message |

### Notifications (`/api/notifications`)
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/` | Bearer | Get notifications (last 50) |
| PATCH | `/{id}/read` | Bearer | Mark as read |
| PATCH | `/read-all` | Bearer | Mark all as read |

### SignalR Hub
- **Route:** `/chatHub`
- **Methods:** `SendMessage(conversationId, patientId, message)`, `JoinConversation(conversationId)`

## Authentication

JWT Bearer tokens (HS256). Access tokens expire in 60 minutes with 7-day refresh token rotation. Account lockout after 5 failed login attempts (15-minute window).

## Rate Limiting

- **General:** 100 requests/minute per user (all controllers)
- **AI:** 10 requests/minute per user (AiController)

## Background Jobs

Hangfire runs `ExerciseReminderJob` daily at 9:00 AM to send exercise reminders.

## Testing

```bash
dotnet test backend.sln --verbosity normal
```

154 tests across 19 test classes covering all controllers and services.

## Key Dependencies

| Package | Purpose |
|---------|---------|
| Entity Framework Core | ORM + migrations |
| FluentValidation | Request validation (14 validators) |
| Hangfire | Background job scheduling |
| Microsoft.AspNetCore.SignalR | Real-time chat |
| Azure.AI.OpenAI | GPT-4o, Whisper STT, embeddings |
| BCrypt.Net | Password hashing |
| DotNetEnv | .env file loading |
