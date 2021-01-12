using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace MicrosoftEntitiyFrameworkCoreSqlite
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Sharable;Mode=Memory;Cache=Shared", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<User>().ToTable(nameof(Users));
            modelBuilder.Entity<Address>().ToTable(nameof(Addresses));
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
