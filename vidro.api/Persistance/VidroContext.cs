using Microsoft.EntityFrameworkCore;
using vidro.api.Domain;

namespace vidro.api.Persistance
{
    public class VidroContext : DbContext
    {
        // Generate database models using values from .env file:
        // For localhost: dotnet ef dbcontext scaffold "Host=localhost;Database=vidrodb;Username=admin;Password=admin123" Npgsql.EntityFrameworkCore.PostgreSQL -o DatabaseModels
        // For Docker container: dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=vidrodb;Username=admin;Password=admin123" Npgsql.EntityFrameworkCore.PostgreSQL -o DatabaseModels

        public VidroContext(DbContextOptions<VidroContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VidroContext).Assembly);
        }

        public virtual DbSet<Visit> Visits { get; set; }

        public virtual DbSet<Glass> Glasses { get; set; }
    }
}
