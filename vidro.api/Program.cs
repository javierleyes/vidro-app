using FluentValidation;
using Microsoft.EntityFrameworkCore;
using vidro.api.Extension;
using vidro.api.Persistance;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck<vidro.api.Extension.DatabaseHealthCheck>("database");
        
        // Register the health check service
        builder.Services.AddScoped<vidro.api.Extension.DatabaseHealthCheck>();

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });

            options.AddPolicy("Production", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:8081",
                        "http://localhost:3000",
                        "http://127.0.0.1:8081",
                        "http://127.0.0.1:3000"
                      )
                      .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                      .WithHeaders("Content-Type", "Authorization")
                      .AllowCredentials();
            });
        });

        // Register PostgresSQL context.
        try
        {
            string postgresConnectionString = PostgresConnectionFactory.GetConnectionString(builder.Configuration);
            
            // Log connection attempt (without exposing sensitive data)
            Console.WriteLine($"Attempting to connect to database. Environment: {builder.Environment.EnvironmentName}");
            
            builder.Services.AddDbContext<VidroContext>(option => option.UseNpgsql(postgresConnectionString));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to configure database connection: {ex.Message}");
            throw;
        }

        // Add FluentValidation validators
        builder.Services.AddScoped<IValidator<vidro.api.Feature.Visit.Create.Model.CreateVisitWriteModel>, vidro.api.Feature.Visit.Create.ValidationCollection.CreateVisitRequestValidator>();
        builder.Services.AddScoped<IValidator<vidro.api.Feature.Visit.Patch.Model.UpdateVisitWriteModel>, vidro.api.Feature.Visit.Patch.ValidationCollection.UpdateVisitRequestValidator>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Test database connection on startup
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<VidroContext>();
                Console.WriteLine("Testing database connection...");
                await context.Database.CanConnectAsync();
                Console.WriteLine("Database connection successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                // Don't throw here in production, let the app start and handle it gracefully
                if (app.Environment.IsDevelopment())
                {
                    throw;
                }
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Use permissive CORS policy for development
            app.UseCors("AllowAll");
        }
        else
        {
            // Use restrictive CORS policy for production
            app.UseCors("Production");
        }

        // Don't redirect to HTTPS on Render as it handles SSL termination
        if (!app.Environment.IsProduction())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        // Add health check endpoint
        app.MapHealthChecks("/health");

        // Add a simple root endpoint for health checks
        app.MapGet("/", () => "Vidro API is running!");

        // Debug endpoint for connection string (only in development)
        if (app.Environment.IsDevelopment())
        {
            app.MapGet("/debug/env", () => 
            {
                var connString = app.Configuration.GetConnectionString("VidroConnection");
                var envVar = Environment.GetEnvironmentVariable("ConnectionStrings__VidroConnection");
                
                return new { 
                    ConfigConnectionString = !string.IsNullOrEmpty(connString) ? "SET" : "NOT SET",
                    EnvConnectionString = !string.IsNullOrEmpty(envVar) ? "SET" : "NOT SET",
                    Environment = app.Environment.EnvironmentName
                };
            });
        }

        app.MapControllers();

        await app.RunAsync();
    }
}