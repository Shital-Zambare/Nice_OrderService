using Moq;
using OrderService.Models;
using OrderService.Services;
using StackExchange.Redis;

namespace OrderService.Tests
{
    [TestFixture]
    public class OrderServiceTest
    {
        private OrdersService _orderService;
        private Mock<IKafkaProducerService> _kafkaProducerMock;
        private Mock<IConnectionMultiplexer> _redisMock;
        private Mock<IDatabase> _databaseMock;
        private Mock<INotificationService> _notificationServiceMock;

        [SetUp]
        public void SetUp()
        {
            // Create a mock for IKafkaProducerService
            _kafkaProducerMock = new Mock<IKafkaProducerService>();

            // Set up the behavior for PublishOrderCreatedEvent method
            _kafkaProducerMock.Setup(p => p.PublishOrderCreatedEvent(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                              .Returns(Task.CompletedTask);

            _redisMock = new Mock<IConnectionMultiplexer>();
            _databaseMock = new Mock<IDatabase>();

            // Set up the behavior for GetDatabase method
            _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_databaseMock.Object);
            _notificationServiceMock = new Mock<INotificationService>();
            _orderService = new OrdersService(
                _notificationServiceMock.Object,
                _kafkaProducerMock.Object,_redisMock.Object);
        }

        [Test]
        public async Task CreateOrderAsync_ShouldReturnCreatedOrder()
        {
            // Arrange
            var order = new Models.Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // Act
            var result = await _orderService.CreateOrderAsync(order);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OrderId, Is.EqualTo(order.OrderId));
        }

        [Test]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Models.Order
            {
                OrderId = orderId,
                CustomerId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // Act
            await _orderService.CreateOrderAsync(order);
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.OrderId, Is.EqualTo(orderId));
        }

        [Test]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
