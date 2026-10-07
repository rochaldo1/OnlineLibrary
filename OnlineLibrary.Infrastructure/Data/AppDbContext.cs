using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Infrastructure.Data
{
    /// <summary>
    /// Контекст базы данных приложения.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
