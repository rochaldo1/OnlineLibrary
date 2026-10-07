using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Конфигурация сущности "Книга" для Entity Framework Core
    /// </summary>
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(2000);

            builder.HasIndex(b => b.CatalogId);
        }
    }
}
