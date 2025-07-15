using FluentValidation;
using Microsoft.EntityFrameworkCore;
using vidro.api.Extension;
using vidro.api.Persistance;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add health checks
        builder.Services.AddHealthChecks();

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
                policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
                      .WithMethods("GET", "POST", "PUT", "DELETE")
                      .WithHeaders("Content-Type", "Authorization");
            });
        });

        // Register PostgresSQL context.
        string postgresConnectionString = PostgresConnectionFactory.GetConnectionString(builder.Configuration);
        builder.Services.AddDbContext<VidroContext>(option => option.UseNpgsql(postgresConnectionString));

        // Add FluentValidation validators
        builder.Services.AddScoped<IValidator<vidro.api.Feature.Visit.Create.Model.CreateVisitWriteModel>, vidro.api.Feature.Visit.Create.ValidationCollection.CreateVisitRequestValidator>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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

        app.MapControllers();

        app.Run();
    }
}