using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Application.Queries;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;

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
