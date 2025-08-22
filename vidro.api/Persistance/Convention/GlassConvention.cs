using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vidro.api.Domain;

namespace vidro.api.Persistance.Convention
{
    public class GlassConvention : IEntityTypeConfiguration<Glass>
    {
        public void Configure(EntityTypeBuilder<Glass> builder)
        {
            builder.ToTable("glass");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("create_date");
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            builder.Property(e => e.ModifyDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("modify_date");
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            builder.Property(e => e.PriceTransparent)
                .HasPrecision(10, 2)
                .HasColumnName("price_in_transparent");
            builder.Property(e => e.PriceColor)
                .HasPrecision(10, 2)
                .HasColumnName("price_in_color");
        }
    }
}
