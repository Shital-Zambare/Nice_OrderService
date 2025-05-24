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



   





