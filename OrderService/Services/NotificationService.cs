using OrderService.Models;
using Polly;

namespace OrderService.Services
{

    public interface INotificationService
    {
        Task NotifyAsync(Order order);
    }

    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncPolicy _retryPolicy;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
        }

        public async Task NotifyAsync(Order order)
        {
            var summary = new
            {
                order.OrderId,
                order.CustomerId,
                order.Timestamp,
                order.Status
            };

            await _retryPolicy.ExecuteAsync(() =>
            _httpClient.PostAsJsonAsync("http://localhost:5000/notifications", summary));
        }
    }

}
