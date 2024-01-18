using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using auto_checkin.Persistances.Entities;

namespace auto_checkin.Persistances.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired().IsUnicode();
            builder.Property(u => u.Message).IsRequired().IsUnicode();

            builder.HasOne(c => c.Blog)
              .WithMany(c => c.Comments)
              .HasForeignKey(c => c.BlogId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Comments");

        }
    }
}
