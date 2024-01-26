using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using auto_checkin.Persistances.Entities;

namespace auto_checkin.Persistances.Configuration
{
    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().IsUnicode();
            builder.Property(u => u.Slug).IsRequired().IsUnicode();

            builder.ToTable("Series");

        }
    }
}
