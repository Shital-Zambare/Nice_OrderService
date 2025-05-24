using Confluent.Kafka;
using System.Text.Json;

namespace OrderService.Services
{

    public interface IKafkaProducerService
    {
        Task PublishOrderCreatedEvent(Guid orderId, DateTime timestamp);
    }

    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic = "orders.created";

        public KafkaProducerService(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishOrderCreatedEvent(Guid orderId, DateTime timestamp)
        {
            var message = new
            {
                OrderId = orderId,
                Timestamp = timestamp
            };

            var json = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
        }

    }
}
