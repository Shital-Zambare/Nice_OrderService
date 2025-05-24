using System;
using System.Net;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Tests
{
    [TestFixture]
    public class NotificationServiceTest
    {
        [Test]
        public async Task NotifyAsync_ShouldPostOrderSummary()
        {
            // Arrange
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = "customer-1",
                Timestamp = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == new Uri("http://localhost:5000/notifications")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new NotificationService(httpClient);

            // Act
            await service.NotifyAsync(order);

            // Assert
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("http://localhost:5000/notifications")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task NotifyAsync_ShouldRetryOnFailure()
        {
            // Arrange
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = "customer-1",
                Timestamp = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            int callCount = 0;

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    callCount++;
                    // Fail the first 3 times, succeed on the 4th
                    if (callCount < 4)
                        throw new HttpRequestException("Simulated network failure");
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var service = new NotificationService(httpClient);

            // Act
            await service.NotifyAsync(order);

            // Assert
            Assert.That(callCount, Is.EqualTo(4), "Should attempt 4 times (1 initial + 3 retries)");
        }

    }
}
