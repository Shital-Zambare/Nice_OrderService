using OrderService.Services;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddSingleton<IOrderService, OrderService.Services.OrdersService>();

// Add Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
 ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

builder.Services.AddHttpClient("NotificationService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000"); // Adjust if needed
})
.AddPolicyHandler(HttpPolicyExtensions
 .HandleTransientHttpError()
 .WaitAndRetryAsync(3, retryAttempt =>
 TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
