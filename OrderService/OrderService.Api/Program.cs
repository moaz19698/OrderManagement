using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Application.Queries;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext

// Add DbContexts for write and read
builder.Services.AddDbContext<OrderWriteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WriteConnection")));

builder.Services.AddDbContext<OrderReadDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReadConnection")));
// Add MediatR and Handlers
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));


// Add Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
var authConfig = builder.Configuration.GetSection("Authentication");
// Configure authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authConfig["Authority"];
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OrderServiceRead", policy =>
        policy.RequireClaim("scope", "order.read"));
    options.AddPolicy("OrderServiceWrite", policy =>
        policy.RequireClaim("scope", "order.write"));
});

builder.Services.AddOpenTelemetry()
    .WithTracing(traceBuilder =>
    {
        traceBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("OderService")) // Replace with your service name
            .AddAspNetCoreInstrumentation() // Instrument HTTP requests
            .AddHttpClientInstrumentation() // Instrument HTTP client calls
            .AddJaegerExporter(options =>
            {
                options.AgentHost = builder.Configuration["Jaeger:Host"] ?? "jaeger";
                options.AgentPort = int.Parse(builder.Configuration["Jaeger:Port"] ?? "6831");
            });
    });
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
