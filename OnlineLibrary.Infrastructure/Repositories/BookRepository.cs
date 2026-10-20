using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Domain.Entities;
using OnlineLibrary.Infrastructure.Data;

namespace OnlineLibrary.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация хранилища книг на EF Core.
    /// </summary>
    internal sealed class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Book>> GetListByCatalogIdAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.CatalogId == catalogId)
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Id)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateAsync(Book book, CancellationToken cancellationToken)
        {
            _context.Books.Add(book);
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
            var deletedCount = await _context.Books
                .Where(b => b.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return deletedCount > 0;
        }
    }
}
