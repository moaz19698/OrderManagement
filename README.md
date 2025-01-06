# Order Management Microservices

## Overview
This repository contains a microservice-based order management system for an e-commerce platform. The system allows users to:

- **Create**, **Update**, and **Track** orders.
- Process orders through a predefined workflow (e.g., `New -> Approved -> Dispatched`).
- Use **RabbitMQ** for messaging between services.
- Follow **CQRS** principles for separation of read and write operations.

The solution integrates with an **API Gateway (Ocelot)** to route requests and ensures proper security and scalability.

---

## Features
- **Microservices**:
  - **Order Service**: Manages order lifecycle.
  - **Notification Service**: Sends notifications when an order status changes.
- **Workflow Management**:
  - Predefined states: `New -> Approved -> Dispatched`.
- **CQRS**:
  - Commands: CreateOrder, UpdateOrder, ChangeOrderStatus.
  - Queries: Retrieve orders and their status.
- **Messaging**:
  - RabbitMQ for publishing and consuming order events.
- **API Gateway**:
  - Ocelot for routing requests.
  - JWT-based authentication for securing endpoints.
- **Database**:
  - Entity Framework Core with SQL Server.
- **Documentation and Testing**:
  - Swagger for API documentation.
  - Unit tests for key components.

---

## System Requirements
- .NET 7 SDK or later
- Docker and Docker Compose
- RabbitMQ (set up using Docker Compose)
- SQL Server

---

## Setup Instructions

### Clone the Repository
```bash
git clone https://github.com/moaz19698/OrderManagement.git
cd OrderManagement
```

### Run Services with Docker Compose
1. Ensure Docker is running.
2. Execute the following command:
   ```bash
   docker-compose up --build
   ```

### Apply Migrations
For each service, apply database migrations:

#### Order Service:
```bash
dotnet ef database update --project OrderService.Infrastructure
```

#### Notification Service:
```bash
dotnet ef database update --project NotificationService.Infrastructure
```

---

## API Endpoints
The API Gateway routes the following endpoints:

### Order Service
- **POST** `/orders`: Create a new order.
- **PUT** `/orders/{id}`: Update an existing order.
- **PATCH** `/orders/{id}/status`: Change the status of an order.
- **GET** `/orders/{id}`: Get order details.
- **GET** `/orders`: List all orders.

### Notification Service
- Listens to order status changes via RabbitMQ.
- Sends notifications/logs status changes.

---

## Architecture Overview
### Components
1. **Order Service**:
   - Handles commands and queries for orders.
   - Publishes events to RabbitMQ upon status change.
2. **Notification Service**:
   - Subscribes to RabbitMQ events to send notifications.
3. **RabbitMQ**:
   - Facilitates communication between services.
4. **Ocelot API Gateway**:
   - Central routing point for external requests.
5. **Databases**:
   - Separate read/write databases for the Order Service.

### Technologies Used
- .NET 7
- RabbitMQ
- Ocelot API Gateway
- SQL Server
- Entity Framework Core
- Docker and Docker Compose
- Swagger

---

## Design Choices
### CQRS
- **Commands**:
  - Handled in the write database to ensure consistency.
- **Queries**:
  - Handled in the read database for optimized performance.

### Event-Driven Architecture
- RabbitMQ decouples services and ensures asynchronous communication.

### Scalability
- Microservice architecture allows independent scaling of services.
- RabbitMQ provides reliable messaging.

---

## Future Enhancements
- **Retry Logic** for RabbitMQ consumers.
- **Caching** with Redis for frequently accessed queries.
- **Distributed Tracing** with tools like Jaeger or Zipkin.

---

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request for review.

---

## License
This project is licensed under the MIT License.

