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
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}