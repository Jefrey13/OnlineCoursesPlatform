# OnlineCoursesPlatform
An advanced API for a web platform offering online courses built with .NET 8. It incorporates RESTful architecture, JWT authentication with refresh tokens, role-based access control, SQL Server integration, OTP-based password recovery, robust logging, middleware for error handling, and Blazor for UI services.

## Features

### 1. **JWT Authentication, Refresh Tokens, and Dynamic Role & Permission Management**
   - **JWT Authentication**: Secure authentication using JWT with role and permission validation.
   - **Refresh Tokens**: Implemented refresh tokens stored in the database for secure session handling.
   - **AuthController**: Supports login and JWT token refresh functionality.
   - **Role & Permission Management**: Dynamically retrieves user roles and permissions from the database and includes them in the JWT token.
   - **Password Hashing**: Utilizes BCrypt for secure password storage.
   - **Configuration**: JWT authentication and authorization setup in `Program.cs`.

### 2. **Database Integration**
   - Implemented repositories for managing users, roles, permissions, and refresh tokens.
   - Added migrations to create database tables for:
     - `Users`
     - `Roles`
     - `Permissions`
     - `RefreshTokens`

### 3. **Email Service for OTP (One-Time Password)**
   - **EmailService**: Sends OTP codes for password recovery.
   - **MailKit Integration**: Secure email handling with MailKit, using configuration from `.env`.
   - Removed hardcoded credentials, improving security by leveraging environment variables for email configuration.

### 4. **Robust Middleware & Error Handling**
   - **Response Compression**: Optimized API response size with compression.
   - **ProblemDetails Middleware**: Standardized error responses across the API for better client-side error handling.
   - **Logging**: Integrated logging using Serilog for API request tracing and error reporting.

### 5. **Blazor Integration for UI**
   - **Blazor WebAssembly**: Utilized Blazor for the front-end interface of the online courses platform.
   - **LocalStorage Service**: Handles token management (authToken, refreshToken) using `Blazored.LocalStorage` with conflict resolution for custom local storage.

## Commit History Highlights

### Implementation of JWT Authentication, Refresh Tokens, and Dynamic Role & Permission Management
   - **JWT authentication** added with role and permission validation.
   - **Refresh token** implementation with database storage and validation.
   - **AuthController** supports login and token refresh.
   - **JWT token generation** includes dynamic roles and permissions retrieved from the database.
   - Added repositories for users, roles, permissions, and refresh tokens.
   - **Password hashing** using BCrypt for secure authentication.
   - Configured JWT authentication and authorization in `Program.cs`.
   - Migrations added for creating the necessary tables.

### Added Email Service for Sending OTP with MailKit
   - Implemented `EmailService` to send OTP codes for password reset.
   - Moved sensitive SMTP configuration (host, port, email, password) to `.env`.
   - Improved security by removing hardcoded email credentials and using `.env` configuration.
   - Integrated **MailKit** for secure email handling based on environment variables.

### Fixed HTTPS Certificate Issue
   - Regenerated developer certificate for local development.
   - Updated `Program.cs` to include improved response compression and middleware for problem details.
   - Added dependency injection for `IEmailService` and `IPasswordResetService`.

### LocalStorage Service Implementation
   - Implemented a custom `LocalStorageService` for handling token storage.
   - Resolved conflicts between the project's `ILocalStorageService` and `Blazored.LocalStorage`.
   - Added operations to save, retrieve, and remove tokens in local storage.

### Fixed Authentication Redirection and Layout Handling
   - Added authentication check and redirection to `/login` in `App.razor`.
   - Updated `MainLayout.razor` to conditionally show the `NavMenu` based on authentication status.
   - Ensured unauthenticated users are redirected when accessing protected routes.
   - Simplified layout handling for public pages like `/login`, `/register`, and `/forgot-password`.

### Adjusted Authentication Logic and Protected Routes Handling
   - Added a loading screen while checking authentication status with the token in LocalStorage.
   - Fixed redirection flow to prevent incorrect login redirection.
   - Adjusted logic for handling protected and public routes.
   - Corrected `MainLayout` so the `NavBar` only appears for authenticated users.
   - Consolidated authentication checks to avoid redundancy.
   - Verified that public pages load correctly and protected routes require valid authentication.

## Setup Instructions

1. Clone the repository:

   ```bash
   git clone https://github.com/your-repo/online-courses-platform.git
