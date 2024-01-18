using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using auto_checkin.Persistances.Entities;

namespace auto_checkin.Persistances.Configuration
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Title).IsRequired().IsUnicode();
            builder.Property(u => u.ShortTitle).IsRequired().IsUnicode();
            builder.Property(u => u.Slug).IsRequired().IsUnicode();


            builder.HasOne(c => c.Series)
              .WithMany(c => c.Blogs)
              .HasForeignKey(c => c.SerieId)
              .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Blogs");

        }
    }
}
