# OrderService
OrderService is a .NET 8 microservice for managing orders, publishing order events to Kafka, caching with Redis, and sending notifications to external systems.
Features
- Order Management: Create, retrieve, and manage orders via RESTful API.
- Kafka Integration: Publishes order creation events to a Kafka topic.
- Redis Caching: Caches order data for fast retrieval.
- Notification Service: Sends order notifications to external services with retry logic (using Polly).
- Unit Tested: Includes comprehensive unit tests using NUnit and Moq.

## Project Structure
    OrderService/
    │
    ├── Controllers/
    │   └── OrdersController.cs
    ├── Models/
    │   └── Order.cs, ProductItem.cs, etc.
    ├── Services/
    │   ├── OrdersService.cs
    │   ├── KafkaProducerService.cs
    │   ├── RedisCacheService.cs
    │   └── NotificationService.cs
    ├── appsettings.json
    ├── Program.cs
    │
    └── OrderServiceTest/
        ├── OrdersControllerTests.cs
        ├── OrderServiceTest.cs
        └── NotificationServiceTest.cs

## Architecture Decisions
- ASP.NET Core Web API:
Chosen for its performance, scalability, and first-class support for RESTful APIs.
- Redis:
Used for caching order data to improve read performance and reduce database load. Redis is a fast, in-memory data store suitable for caching scenarios.
- Kafka:
Used for publishing order creation events to enable event-driven architecture and integration with other services (e.g., analytics, fulfillment).
- Polly:
Used for implementing retry logic in the notification service to handle transient HTTP failures gracefully. 

## Integration Points
- Kafka:
Publishes messages to the orders.created topic when a new order is created.
- Redis:
Caches order data for quick retrieval and to reduce load on the primary data store.
- Notification Service:
Sends HTTP POST requests to external notification endpoints when order events occur.

# Getting Started
## Prerequisites
•	.NET 8 SDK
•	Kafka (for event publishing)
•	Redis (for caching)
## Configuration
Edit appsettings.json to set up Kafka and Redis connection strings:
```json
{
  "Kafka": {
    "BootstrapServers": "localhost:29092"
  },
  "Redis": {
    "ConnectionString": "localhost:26379,abortConnect=false"
  }
}
```
## Build and Run
    dotnet build
    dotnet run --project OrderService

## Running Tests
    dotnet test OrderServiceTest

## API Endpoints
- POST /orders - Create a new order
- GET /orders/{id} - Get order by ID
## Technologies Used
- ASP.NET Core 8
- Confluent.Kafka
- StackExchange.Redis
- Polly
- NUnit, Moq, FluentAssertions

  ## Please find attached Images, which captured while running the microservices in local machine
1. Run Notifiction Service
![image](https://github.com/user-attachments/assets/c7403bd9-4001-4152-8916-229983533dd8)

2. Run Order Service
![image](https://github.com/user-attachments/assets/cc632f73-9bfa-41bd-b1bf-0279023459c5)

3. Create New Order
   ![image](https://github.com/user-attachments/assets/4a5ddea3-40cd-4bd5-9e2b-0175a1531660)

4. Check Order details in Kafka
![image](https://github.com/user-attachments/assets/f85bcd94-6488-4d16-92c5-e9cf6d89277e)

5. Get Order details
![image](https://github.com/user-attachments/assets/a2072498-6fd5-4163-9e8d-71ac5094fe7e)

6. Get Order details from Redis
![image](https://github.com/user-attachments/assets/12016af9-9182-4c2e-8d4c-90234301196c)

## Assumptions Made
- All external dependencies (Kafka, Redis, Notification endpoints) are available and reachable from the service.
- Order data is uniquely identified by a GUID.
- The static Orders list in the controller is for demonstration/testing only and should be replaced with persistent storage in production.
- Security (authentication/authorization) is not implemented in this sample but should be added for production.
- The notification endpoint is available at http://localhost:5000/notifications by default.

## Production Readiness
###### - Configuration
Ensure all connection strings (Kafka, Redis, Notification endpoints) are set via environment variables or secure configuration providers.
###### - Health Checks
Implement health endpoints for liveness and readiness probes (e.g., /health) to integrate with orchestration platforms like Kubernetes.
###### - Scalability
The service is stateless (except for the static Orders list, which should be removed or replaced with persistent storage for production).
###### - Observability
Integrate structured logging and distributed tracing for monitoring and diagnostics.
###### - Resilience 
Uses Polly for retry logic in notification delivery. Consider circuit breakers for external dependencies.

## NFR List
###### - Retention:
Order data should be persisted in a database or durable cache. Kafka topics should have appropriate retention policies.
###### -	Performance:
Designed for low-latency order processing. Caching with Redis improves read performance.
###### -	Security:
Secure all endpoints with authentication and authorization (e.g., JWT, OAuth2). Protect sensitive configuration with secrets management.
###### -	Reliability:
Retry policies are in place for notifications. Use redundant Kafka brokers and Redis clusters for high availability.
###### -	Scalability:
Can be horizontally scaled. Ensure statelessness and externalize state.
###### -	Monitoring:
Integrate with monitoring tools (e.g., Prometheus, Grafana, Azure Monitor).

## Testing Approaches
###### -	Unit Testing:
All core services and controllers are covered by unit tests using NUnit and Moq.
###### -	Integration Testing:
Recommended to add integration tests for Kafka, Redis, and HTTP endpoints.
###### -	Continuous Integration:
Tests should be run in CI pipelines to ensure code quality and prevent regressions.

## Troubleshooting (Logging, Alarms, etc.)
###### -	Logging:
All exceptions are logged to the console. For production, use a structured logging framework (e.g., Serilog, NLog) and forward logs to a centralized system.
###### -	Alarms:
Set up alerts for failed order processing, notification delivery failures, and external service outages.
###### -	Tracing:
Add correlation IDs to requests and propagate them through all service calls for easier traceability.
###### -	Metrics:
Expose metrics for request rates, error rates, and latency. Integrate with monitoring dashboards.
###### -	Common Issues:
 Connection failures to Kafka/Redis: Check configuration and network connectivity.
 Notification failures: Review logs for retry attempts and root causes.


   





