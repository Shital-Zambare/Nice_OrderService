using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderService.Controllers;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private OrdersController _controller;
        private Mock<IOrderService> _orderService;

        [SetUp]
        public void SetUp()
        {
            _orderService = new Mock<IOrderService>();
        }

        [Test]
        public async Task CreateOrder_ShouldAddOrder()
        {
            // Arrange
            var order = new Models.Order { OrderId = Guid.NewGuid(), CustomerId = Guid.NewGuid().ToString(), Timestamp = DateTime.UtcNow, Status = OrderStatus.Pending };
            _orderService.Setup(x => x.GetOrderByIdAsync(order.OrderId)).ReturnsAsync(order);
            _controller = new OrdersController(_orderService.Object);
            // Act
            var result = await _controller.CreateOrder(order);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.Value, Is.InstanceOf<Models.Order>());
            var createdOrder = createdResult.Value as Models.Order;
            Assert.AreEqual(order.OrderId, createdOrder.OrderId);

        }

        [Test]
        public async Task GetOrder_ShouldReturnOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Models.Order { OrderId = orderId, CustomerId = Guid.NewGuid().ToString(), Timestamp = DateTime.UtcNow, Status = OrderStatus.Pending };
            
            _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
            _controller = new OrdersController(_orderService.Object);
            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(order));
        }

        [Test]
        public async Task GetOrder_ShouldReturnNotFoundIfOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _orderService.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync((Models.Order)null);
            _controller = new OrdersController(_orderService.Object);
            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
