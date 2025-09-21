using Microsoft.EntityFrameworkCore;
using ZimmetApp.Models;

namespace ZimmetApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> Persons => Set<Person>();
        public DbSet<Assignment> Assignments => Set<Assignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                        .Property(a => a.ItemType)
                        .HasConversion<int>();

            modelBuilder.Entity<Assignment>()
                        .HasOne(a => a.Person)
                        .WithMany()
                        .HasForeignKey(a => a.PersonId)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
