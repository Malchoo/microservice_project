# Microservices Project

## Overview

This project demonstrates a microservices architecture implementation with a focus on Domain-Driven Design (DDD) and Clean Architecture principles. It's important to note that this is not a fully functional system, but rather a presentation-oriented project designed to showcase architectural concepts and patterns.

## Key Features

- Event-driven architecture with eventual consistency
- Implementation of the Outbox pattern
- CQRS (Command Query Responsibility Segregation) pattern
- Microservices communication via RabbitMQ

## Project Structure

```
src/
├── Users/
├── Friendships/
│   ├── Friendships.Write/
│   └── Friendships.Read/
└── SharedKernel/
```

### Microservices

1. [Users](#users-microservice): Implements user management following DDD and Clean Architecture principles.
2. [Friendships.Write](#friendshipswrite-microservice): Manages friendship relationships, adhering to DDD and Clean Architecture.
3. [Friendships.Read](#friendshipsread-microservice): Optimized for read operations, using Dapper and raw SQL for high performance.
4. [SharedKernel](#sharedkernel): Common code shared across all microservices.

## Technologies Used

- .NET 8
- C# 12
- Entity Framework Core
- Dapper
- SQL Server
- RabbitMQ
- Serilog
- Swagger/OpenAPI
- MediatR (for CQRS implementation)
- FluentValidation

## Detailed Microservices Overview

### Users Microservice

[Link to Users Microservice](https://github.com/Malchoo/microservice_project/tree/main/src/Users)

#### Structure

```
Users/
│
├── Users.Api
│   ├── Controllers
│   ├── Errors
│   └── Program.cs
│
├── Users.Application
│   ├── Common
│   ├── Profiles
│   ├── Users
│   └── DependencyInjection.cs
│
├── Users.Contracts
│   ├── Profiles
│   └── Users
│
├── Users.Domain
│   ├── Contracts
│   ├── DomainEvents
│   ├── Entities
│   ├── Enums
│   ├── Errors
│   ├── EventualConsistency
│   ├── Factories
│   ├── Ids
│   ├── Primitives
│   ├── Time
│   ├── Validators
│   └── ValueObjects
│
└── Users.Infrastructure
    ├── IntegrationEvents
    ├── Middleware
    ├── Migrations
    ├── Persistence
    ├── Services
    └── DependencyInjection.cs
```

#### Key Components

| Namespace | Description |
|-----------|-------------|
| Users.Api | API layer, handles HTTP requests and defines endpoints |
| Users.Application | Application layer, contains business logic and use cases |
| Users.Contracts | Defines contracts (DTOs) for communication |
| Users.Domain | Core domain layer, contains business entities and logic |
| Users.Infrastructure | Infrastructure layer, handles data access and external services |

#### Integration

When a new user is created, an IntegrationEvent is generated and sent to the Friendships.Write microservice to create an empty friendship ledger.

### Friendships.Write Microservice

[Link to Friendships.Write Microservice](https://github.com/Malchoo/microservice_project/tree/main/src/Friendships/Friendships.Write)

#### Structure

```
Friendships.Write/
├── Friendships.Write.Api/
│   ├── Controllers/
│   ├── Errors/
│   ├── Middleware/
│   └── Program.cs
│
├── Friendships.Write.Application/
│   ├── ApplicationErrors/
│   ├── Dto/
│   ├── IntegrationEvents/
│   ├── Queries/
│   ├── Users/
│   └── DependencyInjection.cs
│
├── Friendships.Write.Contracts/
│   └── Friendships/
│
├── Friendships.Write.Domain/
│   ├── Contracts/
│   ├── DomainErrors/
│   ├── DomainEvents/
│   ├── Entities/
│   ├── Enums/
│   ├── EventualConsistency/
│   ├── Ids/
│   ├── Primitives/
│   ├── Time/
│   ├── Validators/
│   └── ValueObjects/
│
└── Friendships.Write.Infrastructure/
    ├── IntegrationEvents/
    ├── Middleware/
    ├── Persistence/
    ├── Services/
    └── DependencyInjection.cs
```

#### Key Components

| Namespace | Description |
|-----------|-------------|
| Friendships.Write.Api | API layer, handles HTTP requests and defines endpoints |
| Friendships.Write.Application | Application layer, contains business logic, DTOs, and use cases |
| Friendships.Write.Contracts | Defines contracts (DTOs) for communication |
| Friendships.Write.Domain | Core domain layer, contains business entities and logic |
| Friendships.Write.Infrastructure | Infrastructure layer, handles data access and external services |

### Friendships.Read Microservice

[Link to Friendships.Read Microservice](https://github.com/Malchoo/microservice_project/tree/main/src/Friendships/Friendships.Read)

#### Structure

```
Friendships.Read
│
├── Friendships.Read.Api
│   ├── Endpoints
│   ├── Middleware
│   └── Program.cs
│
├── Friendships.Read.Application
│   ├── Abstractions
│   ├── Commands
│   ├── Contracts
│   ├── Dto
│   ├── Enums
│   ├── Logging
│   └── DependencyInjection.cs
│
└── Friendships.Read.Infrastructure
    ├── Contracts
    ├── Dapper
    ├── DatabaseScripts
    ├── IntegrationEvents
    ├── MessageBrocker
    └── DependencyInjection.cs
```

#### Key Components

| Namespace | Description |
|-----------|-------------|
| Friendships.Read.Api | API layer, handles HTTP requests |
| Friendships.Read.Application | Business logic layer |
| Friendships.Read.Infrastructure | Data access and external services layer |

This microservice is optimized for read operations, using Dapper and raw SQL for high performance. It receives updates from Friendships.Write via IntegrationEvents and provides fast read access to friendship data.

### SharedKernel

[Link to SharedKernel](https://github.com/Malchoo/microservice_project/tree/main/src/SharedKernel)

The SharedKernel contains common code and abstractions used across all microservices. This promotes code reuse and ensures consistency in the implementation of cross-cutting concerns.

**Note**: This project is for presentation purposes and demonstrates architectural concepts. It is not intended to be a fully functional production system.