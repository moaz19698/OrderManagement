using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
//ocelot configuration
builder.Host.ConfigureAppConfiguration((env, config) =>
{
    config.AddJsonFile($"ocelot.json", true, true);
});

var authConfig = builder.Configuration.GetSection("Authentication");

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authConfig["Authority"];
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false
        };
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();

builder.Services.AddOpenTelemetry()
    .WithTracing(traceBuilder =>
    {
        traceBuilder
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("OcelotGateway")) // Replace with your service name
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
app.UseRouting();

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello Ocelot"); });
});
app.UseAuthentication();
await app.UseOcelot();
await app.RunAsync();