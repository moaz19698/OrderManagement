using NotificationService.Application.Consumers;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Messaging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithTracing(traceBuilder =>
    {
        traceBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("NotificationService")) // Replace with your service name
            .AddAspNetCoreInstrumentation() // Instrument HTTP requests
            .AddHttpClientInstrumentation() // Instrument HTTP client calls
            .AddJaegerExporter(options =>
            {
                options.AgentHost = builder.Configuration["Jaeger:Host"] ?? "jaeger";
                options.AgentPort = int.Parse(builder.Configuration["Jaeger:Port"] ?? "6831");
            });
    });



// Add services
builder.Services.AddSingleton<INotificationService, NotificationService.Application.Services.NotificationService>();
builder.Services.AddSingleton(sp =>
{
    return new RabbitMQConsumer(
        hostname: "localhost",
        queueName: "order_status_changed",
        messageProcessor: sp.GetRequiredService<OrderStatusChangedConsumer>().ConsumeAsync
    );
});
builder.Services.AddSingleton<OrderStatusChangedConsumer>();
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
// Start RabbitMQ Consumer
var rabbitMQConsumer = app.Services.GetRequiredService<RabbitMQConsumer>();
rabbitMQConsumer.Start();
app.Run();
