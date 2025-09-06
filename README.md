# SmiTo URL Shortener API

SmiTo is a URL shortening service built with ASP.NET Core. It allows users to generate short URLs, track visits, and view statistics. The project follows clean architecture principles and includes services for tracking visits and resolving short URLs.

---

## Features

- **User Authentication**: JWT-based authentication system
- **URL Shortening**: Generate unique short codes for long URLs
- **Visit Tracking**: Automatic visit logging with device/browser detection
- **Analytics**: Comprehensive statistics including daily visit counts and unique visitors
- **Public Analytics**: Visit statistics are publicly accessible without authentication
- **Expiration Support**: URLs can have custom expiration dates
- **Device Detection**: Track visitor device types and browsers
- **IP Geolocation**: Country detection for visitors (when available)
- **RESTful API endpoints**
- **Built with .NET 9 and ASP.NET Core Web API**
- **Error handling through middleware**
- **Supports `GeneralResult<T>` for consistent API responses**

---

## Technologies Used

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Dependency Injection & Repository Pattern
- Logging via built-in ASP.NET Core logging

---

## Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio 2022
- SQL Server

### Installation

1. Clone the repository:

```bash
git clone <repository-url>
cd SmiTo
```
2. Restore dependencies:
dotnet restore

3. Update database connection string in appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=SmiToDB;Trusted_Connection=True;"
}
```
4. Apply migrations:
```bash
dotnet ef database update
```
5. Run the project:
```bash
dotnet run
```
# SmiTo URL Shortener API

A URL shortening service API that allows users to create shortened URLs, track visits, and view analytics.

## Base URL
```
https://localhost:7121
```

## Authentication

The API uses JWT (JSON Web Token) authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your_jwt_token>
```

## API Endpoints

### Authentication

#### Register User
Create a new user account.

**Endpoint:** `POST /api/user/auth/register`

**Request Body:**
```json
{
    "firstname": "John",
    "lastname": "Doe",
    "email": "user@example.com",
    "password": "Password@123"
}
```

**Response:**
```json
{
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "expiration": "2025-09-06T22:35:20Z"
    },
    "success": true,
    "message": "User registered successfully",
    "errors": []
}
```

#### Login User
Authenticate an existing user.

**Endpoint:** `POST /api/user/auth/login`

**Request Body:**
```json
{
    "email": "user@example.com",
    "password": "Password@123"
}
```

**Response:**
```json
{
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "expiration": "2025-09-06T22:35:59Z"
    },
    "success": true,
    "message": "Login successful",
    "errors": []
}
```

### URL Management

#### Create Short URL
Create a shortened URL from a long URL.

**Endpoint:** `POST /api/url`

**Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
    "originalUrl": "https://github.com/username/repository",
    "userid": d04e93bf-c71c-426f-89c5-08dded7f126
    "expiresAt": "2025-10-06T19:53:39.2868136Z" // Optional
}
```

**Response:**
```json
{
    "data": {
        "id": "d04e93bf-c71c-426f-89c5-08dded7f1313",
        "originalUrl": "https://github.com/username/repository",
        "shortCode": "ho32Id",
        "shortenedUrl": "https://smi.to/ho32Id",
        "createdAt": "2025-09-06T19:53:39.2865729Z",
        "expiresAt": "2025-10-06T19:53:39.2868136Z",
        "clickCount": 0
    },
    "success": true,
    "message": "Shortened URL created successfully.",
    "errors": []
}
```

### URL Redirection

#### Redirect to Original URL
Redirects to the original URL and tracks the visit.

**Endpoint:** `GET /api/redirect/{shortCode}`

**Example:** `GET /api/redirect/ho32Id`

**Response:** HTTP 302 Redirect to original URL

#### Resolve Short URL (API Response)
Returns the original URL without redirecting (useful for API clients).

**Endpoint:** `GET /api/redirect/resolve/{shortCode}`

**Example:** `GET /api/redirect/resolve/ho32Id`

**Response:**
```json
{
    "data": "https://github.com/username/repository",
    "success": true,
    "message": "Visit tracked successfully",
    "errors": []
}
```

### Analytics & Statistics

#### Get Visit Statistics
Retrieve detailed statistics for a shortened URL.

**Endpoint:** `GET /api/visit/{urlId}/stats`

**Query Parameters:**
- `from` (optional): Start date in format `MM-dd-yyyy`
- `to` (optional): End date in format `MM-dd-yyyy`

**Example:** `GET /api/visit/d04e93bf-c71c-426f-89c5-08dded7f1313/stats?from=01-01-2025&to=12-12-2025`

**Response:**
```json
{
    "data": {
        "urlId": "d04e93bf-c71c-426f-89c5-08dded7f1313",
        "totalClicks": 2,
        "uniqueVisitors": 1,
        "dailyStats": [
            {
                "date": "2025-09-06T00:00:00",
                "visitCount": 2,
                "uniqueVisitors": 1
            }
        ],
        "dateRange": {
            "from": "2025-01-01T00:00:00",
            "to": "2025-12-12T00:00:00"
        }
    },
    "success": true,
    "message": "Visit statistics retrieved successfully",
    "errors": []
}
```

#### Get Visit Details
Retrieve individual visit records for a shortened URL.

**Endpoint:** `GET /api/visit/{urlId}/visits`

**Query Parameters:**
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)

**Example:** `GET /api/visit/d04e93bf-c71c-426f-89c5-08dded7f1313/visits?page=1&pageSize=10`

**Response:**
```json
{
    "data": [
        {
            "id": "visit-guid-here",
            "visitedAt": "2025-09-06T19:54:15Z",
            "visitorIp": "192.168.1.1",
            "userAgent": "Mozilla/5.0...",
            "deviceType": "Desktop",
            "browser": "Chrome",
            "referrer": "https://google.com",
            "country": "US"
        }
    ],
    "success": true,
    "message": "Visits retrieved successfully",
    "errors": []
}
```

## Response Format

All API responses follow this consistent format:

```json
{
    "data": {}, // Response data (varies by endpoint)
    "success": boolean, // Whether the request was successful
    "message": "string", // Human-readable message
    "errors": [] // Array of error messages (empty if successful)
}
```

## Error Responses

### 400 Bad Request
```json
{
    "data": null,
    "success": false,
    "message": "Request validation failed",
    "errors": ["Email is required", "Password must be at least 8 characters"]
}
```

### 401 Unauthorized
```json
{
    "data": null,
    "success": false,
    "message": "Authentication required",
    "errors": ["Invalid or expired token"]
}
```

### 404 Not Found
```json
{
    "data": null,
    "success": false,
    "message": "Resource not found",
    "errors": ["Short URL not found or expired"]
}
```

## Usage Examples

### Complete Workflow

1. **Register a new user:**
```bash
curl -X POST https://localhost:7121/api/user/auth/register \
  -H "Content-Type: application/json" \
  -d '{"firstname":"John","lastname":"Doe","email":"john@example.com","password":"Password@123"}'
```

2. **Create a short URL:**
```bash
curl -X POST https://localhost:7121/api/url \
  -H "Content-Type: application/json" \
  -d '{"originalUrl":"https://github.com/username/repository"}'
```

3. **Access the short URL:**
```bash
curl https://localhost:7121/api/redirect/ho32Id
```

4. **View statistics:**
```bash
curl https://localhost:7121/api/visit/d04e93bf-c71c-426f-89c5-08dded7f1313/stats
```

## Notes

- Short URLs expire after 30 days by default if no expiration date is specified
- All timestamps are in UTC format
- Analytics are publicly accessible and don't require authentication
- Visit tracking includes IP address, user agent, device type, browser, and referrer information

