# GymLogsApp

A full-stack personal training tracker built with **ASP.NET Core** and **Angular**. Log your workouts, track progress over time, and view monthly reports — all behind secure JWT authentication.

---

## Tech Stack

### Backend
- **ASP.NET Core 9** — Web API
- **Clean Architecture** — Domain / Application / Infrastructure / API layers
- **CQRS + MediatR** — command and query separation
- **ASP.NET Identity** — user management
- **JWT Authentication** — short-lived access tokens (15 min) via HttpOnly cookies
- **Refresh Token Rotation** — long-lived refresh tokens (7 days) stored in DB + HttpOnly cookie
- **Entity Framework Core** — SQL Server
- **FluentValidation** — request validation pipeline

### Frontend
- **Angular 17+** — standalone components, signals
- **Reactive Forms** — form validation
- **HTTP Interceptors** — auto-attach credentials, silent token refresh, global error handling
- **Auth Guard** — route protection

---

## Architecture

### Backend — Clean Architecture

```
Domain              → Entities (TrainingSession, RefreshToken) — no dependencies
Application         → CQRS handlers, interfaces, DTOs, validators, exceptions
Infrastructure      → EF Core, ASP.NET Identity, TokenService, AuthService
Web.API             → Controllers, Program.cs, GlobalExceptionHandler
```

The dependency rule is strictly enforced — outer layers depend on inner layers, never the reverse. Identity and EF Core are confined to Infrastructure. Application defines interfaces (`IAuthService`, `ITokenService`, `ICurrentUserService`) that Infrastructure implements.

### Frontend — Feature-Based Architecture

```
src/
  app/
    core/
      guards/         → authGuard
      interceptors/   → authInterceptor, errorInterceptor
      models/         → ProblemDetails
    features/
      auth/           → login, register, auth service, models
      training/       → training form, sessions list, monthly report, training service
    shared/
      navbar/
      models/         → TrainingSession, TrainingType
```

---

## Authentication Flow

```
Register / Login
  → Backend issues JWT (15 min) + refresh token (7 days)
  → Both set as HttpOnly cookies (never accessible to JS)
  → Angular signal stores user info in memory

Every request
  → authInterceptor attaches withCredentials: true
  → Browser sends cookies automatically

JWT expires
  → Backend returns 401
  → errorInterceptor catches 401
  → Silently POSTs to /api/auth/refresh
  → On success: retries original request transparently
  → On failure: redirects to /login?reason=session-expired

Refresh token expires
  → /refresh returns 401
  → User redirected to login with "session expired" message

Page refresh
  → Signal resets to null
  → App calls GET /api/auth/me on startup
  → If cookie valid: rehydrates signal
  → authGuard uses fetchCurrentUser() as fallback
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Angular CLI](https://angular.io/cli)

---

### Backend Setup

**1. Clone the repository**
```bash
git clone https://github.com/Paja123/GymLogsApp.git
cd GymLogsApp/Backend
```

**2. Configure `appsettings.json`**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=GymLogsApp;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "your-super-secret-key-minimum-32-characters!!",
    "Issuer": "https://localhost:5294",
    "Audience": "http://localhost:4200"
  }
}

```


**3. Run migrations**
```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Web.API
dotnet ef database update --project Infrastructure --startup-project Web.API
```

**4. Run the API**
```bash
cd API
dotnet run
```

API will be available at `https://localhost:7247`

---

### Frontend Setup

**1. Install dependencies**
```bash
cd frontend
npm install
```

**2. Configure the proxy** (`proxy.conf.json`)
```json
{
  "/api": {
    "target": "https://localhost:7247",
    "secure": false,
    "changeOrigin": true
  }
}
```

**3. Run the app**
```bash
ng serve
```

App will be available at `http://localhost:4200`

---

## API Endpoints

### Auth
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/register` | Public | Register new user |
| POST | `/api/auth/login` | Public | Login |
| POST | `/api/auth/refresh` | Public | Refresh JWT using refresh token cookie |
| POST | `/api/auth/logout` | Required | Revoke refresh token and clear cookies |
| GET | `/api/auth/me` | Optional | Get current user info |

### Training Sessions
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/TrainingSession` | Required | Get all sessions for current user |
| POST | `/api/TrainingSession` | Required | Create new training session |
| GET | `/api/TrainingSession/monthly-report` | Required | Get weekly breakdown for current month |

---

## Password Requirements

- Minimum 6 characters
- At least one uppercase letter
- At least one number
- At least one special character

---

## Key Design Decisions

**HttpOnly cookies over localStorage** — JWT and refresh tokens are stored in HttpOnly cookies so they are never accessible to JavaScript, protecting against XSS attacks. CSRF is mitigated with `SameSite: Strict`.

**Refresh token rotation** — every time the refresh token is used, it is revoked and a new one is issued. This limits the damage of a stolen refresh token.

**`ICurrentUserService`** — user identity is resolved from JWT claims via an interface defined in Application and implemented in API. CQRS handlers extend `AuthorizedHandler<>` base class to avoid repeating this in every handler.

**`GlobalExceptionHandler`** — all exceptions are caught in one place and mapped to RFC 9110 compliant `ProblemDetails` responses with consistent status codes.

**Silent token refresh** — the Angular error interceptor automatically retries failed requests after refreshing the JWT. Users are never interrupted unless their refresh token has also expired.
