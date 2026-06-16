# E-Commerce Microservices Backend

Backend microservices system for an e-commerce application. The project is built as a set of independent services connected through an API Gateway, with service-to-service communication using REST, gRPC, RabbitMQ, Redis, and multiple databases.

## Service Architecture Image

> Add the service architecture image here.

<div align="center">
  <img src="./docs/images/service-architecture.png" alt="Service architecture diagram" width="900" />
</div>

## Architecture

```mermaid
flowchart TD
    Gateway[API Gateway<br/>8020:8080]

    Gateway --> Catalog[Catalog Service<br/>8001:8080]
    Gateway --> Auth[Auth Service<br/>8010:8080]
    Gateway --> Basket[Basket Service<br/>8002:8080]

    Catalog --> Mongo[(MongoDB)]
    Auth --> AuthDb[(SQL Database)]

    Basket --> Redis[(Redis)]
    Basket --> RabbitMQ[RabbitMQ]
    Basket --> Grpc[gRPC]

    Grpc --> Discount[Discount Service<br/>8003:8080]
    Discount --> Postgres[(PostgreSQL)]

    RabbitMQ --> Ordering[Ordering Service<br/>8004:8080]
    Ordering --> OrderDb[(SQL Database)]
```

## Services

| Service | Port | Database / Dependency | Responsibility |
|---|---:|---|---|
| API Gateway | 8020 | - | Main entry point for backend APIs |
| Catalog Service | 8001 | MongoDB | Manages products, categories, and brands |
| Auth Service | 8010 | SQL Database | Handles users, authentication, and authorization |
| Basket Service | 8002 | Redis, RabbitMQ, gRPC | Manages shopping baskets and checkout process |
| Discount Service | 8003 | PostgreSQL | Manages coupons and discount calculations |
| Ordering Service | 8004 | SQL Database, RabbitMQ | Creates and manages customer orders |

## System Flow

1. The client sends requests to the API Gateway.
2. The API Gateway routes each request to the required backend service.
3. Catalog Service handles products, categories, and brands.
4. Auth Service handles user authentication and authorization.
5. Basket Service stores basket data in Redis.
6. Basket Service communicates with Discount Service using gRPC.
7. Basket Service publishes checkout events to RabbitMQ.
8. Ordering Service consumes checkout events from RabbitMQ and creates orders.

## Tech Stack

- Microservices Architecture
- API Gateway
- REST APIs
- gRPC
- RabbitMQ
- Redis
- MongoDB
- SQL Database
- PostgreSQL
- Docker
- Docker Compose
- JWT Authentication
- Refresh Tokens
- Permission-Based Authorization
- Swagger / OpenAPI
- Serilog
- Seq
- Correlation ID

## Backend Features

- API Gateway as the single entry point for backend services
- Correlation ID support for request tracing across services
- Structured logging using Serilog
- Centralized log monitoring using Seq
- Swagger / OpenAPI documentation for backend APIs
- Authentication using access tokens and refresh tokens
- Permission-based authorization in Auth Service
- User registration and login
- Basket checkout event publishing using RabbitMQ
- Discount lookup using gRPC
- Redis caching for basket data

## Ports

| Component | URL |
|---|---|
| API Gateway | `http://localhost:8020` |
| Catalog Service | `http://localhost:8001` |
| Auth Service | `http://localhost:8010` |
| Basket Service | `http://localhost:8002` |
| Discount Service | `http://localhost:8003` |
| Ordering Service | `http://localhost:8004` |
| Seq Dashboard | `http://localhost:5341` |

## Main Endpoints

### Catalog Service

| Method | Endpoint | Description |
|---|---|---|
| GET | `/products` | Get all products |
| GET | `/products/{id}` | Get product by id |
| GET | `/categories` | Get all categories |
| GET | `/brands` | Get all brands |

### Auth Service

| Method | Endpoint | Description |
|---|---|---|
| POST | `/auth/register` | Register a new user |
| POST | `/auth/login` | Login user and return access token with refresh token |
| POST | `/auth/refresh-token` | Generate a new access token using refresh token |
| POST | `/auth/revoke-token` | Revoke refresh token |
| GET | `/auth/profile` | Get authenticated user profile |
| GET | `/auth/permissions` | Get authenticated user permissions |

### Basket Service

| Method | Endpoint | Description |
|---|---|---|
| GET | `/basket/{userName}` | Get user basket |
| POST | `/basket` | Create or update basket |
| DELETE | `/basket/{userName}` | Delete user basket |
| POST | `/basket/checkout` | Checkout basket |

### Discount Service

| Method | Endpoint | Description |
|---|---|---|
| GET | `/discount/{productName}` | Get discount by product name |
| POST | `/discount` | Create discount coupon |
| PUT | `/discount` | Update discount coupon |
| DELETE | `/discount/{productName}` | Delete discount coupon |

### Ordering Service

| Method | Endpoint | Description |
|---|---|---|
| GET | `/orders/{userName}` | Get orders by username |
| GET | `/orders/order/{id}` | Get order by id |
| POST | `/orders` | Create order |

> Update endpoint paths if your actual controller routes are different.

## Authentication And Authorization

Auth Service is responsible for user registration, login, token generation, refresh token handling, and permission-based authorization.

### Auth Flow

```mermaid
sequenceDiagram
    participant User as User
    participant Auth as Auth Service
    participant DB as Auth DB

    User->>Auth: Register user
    Auth->>DB: Save user data
    Auth-->>User: Registration success

    User->>Auth: Login with credentials
    Auth->>DB: Validate user
    Auth-->>User: Access token + Refresh token

    User->>Auth: Refresh token request
    Auth->>DB: Validate refresh token
    Auth-->>User: New access token
```

### Access Token

The access token is used to authorize requests to protected endpoints.

```http
Authorization: Bearer <access-token>
```

### Refresh Token

The refresh token is used to generate a new access token without forcing the user to login again.

Recommended refresh token data:

- Token value
- User id
- Expiration date
- Created date
- Revoked date
- Replaced by token

### Permission-Based Authorization

Permissions are assigned to users or roles and used to protect backend actions.

Examples:

```txt
Catalog.Products.Read
Catalog.Products.Create
Catalog.Products.Update
Catalog.Products.Delete
Orders.Read
Orders.Create
Users.Manage
```

Example protected endpoint behavior:

| Permission | Allowed Action |
|---|---|
| `Catalog.Products.Read` | View products |
| `Catalog.Products.Create` | Create products |
| `Orders.Read` | View orders |
| `Users.Manage` | Manage users and permissions |

## API Documentation

Each service should expose Swagger / OpenAPI documentation in development mode.

| Service | Swagger URL |
|---|---|
| API Gateway | `http://localhost:8020/swagger` |
| Catalog Service | `http://localhost:8001/swagger` |
| Auth Service | `http://localhost:8010/swagger` |
| Basket Service | `http://localhost:8002/swagger` |
| Discount Service | `http://localhost:8003/swagger` |
| Ordering Service | `http://localhost:8004/swagger` |

Swagger helps document:

- Available endpoints
- Request payloads
- Response models
- Authentication requirements
- HTTP status codes

## Observability

### Correlation ID

Each request should include a correlation ID so logs can be traced across services.

Example header:

```http
X-Correlation-ID: 7f2f0a3f-6e3d-4e2f-9f45-0e1c55d2b111
```

If the client does not send a correlation ID, the API Gateway or service middleware should generate one and pass it to downstream services.

### Serilog With Seq

The backend uses Serilog for structured logging and Seq for centralized log search and monitoring.

Seq dashboard:

```txt
http://localhost:5341
```

Recommended log properties:

- CorrelationId
- ServiceName
- RequestPath
- RequestMethod
- StatusCode
- UserId
- UserName
- ElapsedMilliseconds
- Exception

Example Serilog output configuration:

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Seq"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": {
          "serverUrl": "http://seq:5341"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  }
}
```

## Prerequisites

Make sure you have installed:

- Docker
- Docker Compose
- .NET SDK
- Git

## Run With Docker Compose

Start all services:

```bash
docker compose up -d
```

Stop all services:

```bash
docker compose down
```

Rebuild and start all services:

```bash
docker compose up -d --build
```

Check running containers:

```bash
docker ps
```

View logs:

```bash
docker logs <container-name>
```

## Environment Variables

Example environment configuration:

```env
ASPNETCORE_ENVIRONMENT=Development

MongoDbSettings__ConnectionString=mongodb://catalogdb:27017
MongoDbSettings__DatabaseName=CatalogDb

ConnectionStrings__AuthDb=Server=authdb;Database=AuthDb;User Id=sa;Password=Your_password123;
ConnectionStrings__OrderingDb=Server=orderingdb;Database=OrderingDb;User Id=sa;Password=Your_password123;
ConnectionStrings__DiscountDb=Host=discountdb;Database=DiscountDb;Username=postgres;Password=postgres;

RedisSettings__ConnectionString=basketdb:6379

RabbitMQSettings__Host=rabbitmq
RabbitMQSettings__Username=guest
RabbitMQSettings__Password=guest

JwtSettings__Issuer=ECommerceAuth
JwtSettings__Audience=ECommerceClient
JwtSettings__Secret=Your_super_secret_key_should_be_long
JwtSettings__AccessTokenExpirationMinutes=15
JwtSettings__RefreshTokenExpirationDays=7

Serilog__WriteTo__1__Name=Seq
Serilog__WriteTo__1__Args__serverUrl=http://seq:5341

CorrelationIdSettings__HeaderName=X-Correlation-ID
```

## Databases

| Service | Database |
|---|---|
| Catalog Service | MongoDB |
| Auth Service | SQL Database |
| Basket Service | Redis |
| Discount Service | PostgreSQL |
| Ordering Service | SQL Database |

## Service Communication

### REST

The API Gateway communicates with backend services using HTTP/REST.

### gRPC

Basket Service communicates with Discount Service using gRPC to retrieve discount information during basket updates or checkout.

### RabbitMQ

Basket Service publishes checkout events to RabbitMQ. Ordering Service consumes these events and creates orders.

```mermaid
sequenceDiagram
    participant Basket as Basket Service
    participant MQ as RabbitMQ
    participant Ordering as Ordering Service
    participant DB as Ordering DB

    Basket->>MQ: Publish BasketCheckout event
    MQ->>Ordering: Consume BasketCheckout event
    Ordering->>DB: Create order
```

## Project Structure

```txt
src/
  ApiGateway/
  Services/
    Catalog/
    Auth/
    Basket/
    Discount/
    Ordering/
docker-compose.yml
README.md
```

## Development

Run a specific service locally:

```bash
dotnet run
```

Run tests:

```bash
dotnet test
```

## Troubleshooting

### Port Already In Use

Make sure no other service is running on the same port.

```bash
docker ps
docker stop <container-name>
```

### Database Connection Failed

Check that the database container is running and that the connection string matches the Docker service name.

### RabbitMQ Connection Failed

Make sure RabbitMQ is running and the service is using the correct host, username, and password.

### Redis Connection Failed

Make sure Redis is running and Basket Service is using the correct Redis connection string.

## Future Improvements

- Add distributed tracing with OpenTelemetry
- Add health checks for all services
- Add CI/CD pipeline
- Add unit and integration tests
- Add monitoring with Prometheus and Grafana
- Add role management dashboard
- Add refresh token rotation
- Add audit logs for sensitive actions

## License

This project is for educational and development purposes.
