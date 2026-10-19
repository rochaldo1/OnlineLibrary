using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Infrastructure.Data;

namespace OnlineLibrary.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация хранилища каталогов на EF Core.
    /// </summary>
    internal sealed class CatalogRepository : ICatalogRepository
    {
        private readonly AppDbContext _context;

        public CatalogRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Catalog>> GetListAsync(CancellationToken cancellationToken)
        {
            return await _context.Catalogs
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Catalog?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Catalogs
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateAsync(Catalog catalog, CancellationToken cancellationToken)
        {
            _context.Catalogs.Add(catalog);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var deletedCount = await _context.Catalogs
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deletedCount > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Catalogs.AnyAsync(c => c.Id == id, cancellationToken);
        }
    }
}
