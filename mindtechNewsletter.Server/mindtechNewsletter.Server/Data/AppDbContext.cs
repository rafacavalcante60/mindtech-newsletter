using Microsoft.EntityFrameworkCore;
using mindtechNewsletter.Server.Models;

namespace mindtechNewsletter.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //unique email
            modelBuilder.Entity<Subscriber>()
                .HasIndex(s => s.Email)
                .IsUnique();

            //default soft delete
            modelBuilder.Entity<Subscriber>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);
        }
    }
}
