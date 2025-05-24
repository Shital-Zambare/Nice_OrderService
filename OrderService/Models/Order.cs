namespace OrderService.Models
{

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped
    }


    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public string? CustomerId { get; set; }
        public List<ProductItem>? Items { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

}
