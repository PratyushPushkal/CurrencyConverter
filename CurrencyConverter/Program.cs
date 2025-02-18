using CurrencyConverter.Application;
using CurrencyConverter.Domain.Middlewares;
using CurrencyConverter.Infrastructure;
using CurrencyConverter.Infrastructure.Interface;
using CurrencyConverter.Infrastructure.Service;
using CurrencyConverter.WebAPI.ServiceExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using RateLimit;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder.Extensions;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
//builder.Services.AddJwtToken(builder.Configuration);
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdministratorRole",
//    policy => policy.RequireRole("Administrator"));
//});

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "policyname", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(500);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    }));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
else 
{
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
app.UseRateLimiter();
app.UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseCors()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

app.Run();
