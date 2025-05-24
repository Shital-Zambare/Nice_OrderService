using Confluent.Kafka;
using OrderService.Models;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;

namespace OrderService.Services
{

    public interface IOrderService
    {
        Task<Models.Order> CreateOrderAsync(Models.Order order);
        Task<Models.Order?> GetOrderByIdAsync(Guid orderId);
    }

    public class OrdersService : IOrderService
    {
        private readonly INotificationService _notificationService;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly IDatabase _redisDb;
        private readonly ConcurrentDictionary<Guid, Models.Order> _orderStore = new(); // In-memory store

        public OrdersService(INotificationService notificationService, IKafkaProducerService kafkaProducer, IConnectionMultiplexer redis)
        {
            _notificationService = notificationService;
            _kafkaProducer = kafkaProducer;
            _redisDb = redis.GetDatabase();
        }

        public async Task<Models.Order> CreateOrderAsync(Models.Order order)
        {
            order.CustomerId = order.CustomerId;
            order.OrderId = order.OrderId;
            order.Timestamp = DateTime.UtcNow;
            order.Status = order.Status;

            _orderStore[order.OrderId] = order;

            // Notify external service with retry
            await _notificationService.NotifyAsync(order);

            // Publish to Kafka
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(new { order.OrderId, order.Timestamp })
            };
            await _kafkaProducer.PublishOrderCreatedEvent(order.OrderId, order.Timestamp);

            return order;
        }

        public async Task<Models.Order?> GetOrderByIdAsync(Guid orderId)
        {
            string cacheKey = $"order:{orderId}";
            var cached = await _redisDb.StringGetAsync(cacheKey);

            if (cached.HasValue)
            {
                return JsonSerializer.Deserialize<Models.Order>(cached!);
            }

            if (_orderStore.TryGetValue(orderId, out var order))
            {
                await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(order), TimeSpan.FromMinutes(5));
                return order;
            }

            return null;
        }
    }

}
