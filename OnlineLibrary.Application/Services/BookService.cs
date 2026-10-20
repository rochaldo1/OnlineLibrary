using OnlineLibrary.Application.Common;
using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Application.Exceptions;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Application.Mapping;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Services
{
    /// <summary>
    /// Сервис для работы с книгами.
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICatalogRepository _catalogRepository;

        public BookService(IBookRepository bookRepository, ICatalogRepository catalogRepository)
        {
            _bookRepository = bookRepository;
            _catalogRepository = catalogRepository;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<BookResponse>> GetListByCatalogIdAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            await EnsureCatalogExistsAsync(catalogId, cancellationToken);

            var books = await _bookRepository.GetListByCatalogIdAsync(catalogId, cancellationToken);

            return books
                .Select(b => b.ToResponse())
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<BookResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var book = await GetBookOrThrowAsync(id, cancellationToken);

            return book.ToResponse();
        }

        /// <inheritdoc/>
        public async Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken)
        {
            await EnsureCatalogExistsAsync(request.CatalogId, cancellationToken);

            var book = request.ToEntity();

            book.Title = TextNormalizer.NormalizeRequired(request.Title);
            book.Description = TextNormalizer.NormalizeOptional(request.Description);

            await _bookRepository.CreateAsync(book, cancellationToken);

            return book.ToResponse();
        }

        /// <inheritdoc/>
        public async Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await GetBookOrThrowAsync(id, cancellationToken);

            book.Title = TextNormalizer.NormalizeRequired(request.Title);
            book.Description = TextNormalizer.NormalizeOptional(request.Description);

            await _bookRepository.SaveChangesAsync(cancellationToken);

            return book.ToResponse();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _bookRepository.DeleteAsync(id, cancellationToken);

            if (!deleted)
                throw new NotFoundException("Книга", id);
        }

        /// <summary>
        /// Проверяет, что каталог существует.
        /// </summary>
        /// <param name="catalogId">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <exception cref="NotFoundException">Каталог с указанным идентификатором не найден.</exception>
        private async Task EnsureCatalogExistsAsync(Guid catalogId, CancellationToken cancellationToken)
        {
            var exists = await _catalogRepository.ExistsAsync(catalogId, cancellationToken);

            if (!exists)
                throw new NotFoundException("Каталог", catalogId);
        }

        /// <summary>
        /// Загружает книгу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденная книга.</returns>
        /// <exception cref="NotFoundException">Книга с указанным идентификатором не найдена.</exception>
        private async Task<Book> GetBookOrThrowAsync(Guid id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (book is null)
                throw new NotFoundException("Книга", id);

            return book;
        }
    }
}
