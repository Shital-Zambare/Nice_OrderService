using Microsoft.AspNetCore.Mvc;
using OrderService.Controllers;

namespace NotificationServiceTest
{

    public class NotificationControllerTests
    {
        [Test]
        public void Notify_ReturnsOkResult_WhenCalledWithValidObject()
        {
            // Arrange
            var controller = new NotificationController();
            var testOrderSummary = new
            {
                OrderId = 123,
                CustomerName = "John Doe",
                TotalAmount = 99.99
            };

            // Act
            var result = controller.notify(testOrderSummary);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }

}