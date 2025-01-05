using Duende.IdentityServer.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var identityConfig = builder.Configuration.GetSection("IdentityServer");
var clients = identityConfig.GetSection("Clients").Get<List<Client>>();
var apiScopes = identityConfig.GetSection("ApiScopes").Get<List<ApiScope>>();

// Configure IdentityServer
builder.Services.AddIdentityServer()
    .AddInMemoryClients(clients)
    .AddInMemoryApiScopes(apiScopes);

builder.Services.AddOpenTelemetry()
    .WithTracing(traceBuilder =>
    {
        traceBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("IdentityServer")) // Replace with your service name
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
app.UseIdentityServer();

app.Run();
