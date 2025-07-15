using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vidro.api.Domain;

namespace vidro.api.Persistance.Convention
{
    public class VisitConvention : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("visit");

            builder.HasIndex(e => e.Date, "ix_visit_date");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("create_date");
            builder.Property(e => e.Date).HasColumnName("date");
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            builder.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            builder.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        }
    }
}
