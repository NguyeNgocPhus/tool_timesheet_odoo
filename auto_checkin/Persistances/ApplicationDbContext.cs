using auto_checkin.Persistances.Configuration;
using auto_checkin.Persistances.Entities;
using Microsoft.EntityFrameworkCore;

namespace auto_checkin.Persistances
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Commets { get; set; }
        public DbSet<Series> Series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new SeriesConfiguration());
        }

    }
}
