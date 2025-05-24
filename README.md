# OrderService
OrderService is a .NET 8 microservice for managing orders, publishing order events to Kafka, caching with Redis, and sending notifications via HTTP.
It is designed for extensibility and integration with distributed systems.
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
    "BootstrapServers": "localhost:9092"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
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
