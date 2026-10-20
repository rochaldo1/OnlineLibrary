using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Application.Exceptions;

namespace OnlineLibrary.Application.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с книгами.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Возвращает книги каталога. Если книг нет, возвращает пустой список.
        /// </summary>
        /// <param name="catalogId">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список книг из каталога.</returns>
        /// <exception cref="NotFoundException">Каталог с указанным идентификатором не найден.</exception>
        Task<IReadOnlyList<BookResponse>> GetListByCatalogIdAsync(Guid catalogId, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает книгу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденная книга.</returns>
        /// <exception cref="NotFoundException">Книга с указанным идентификатором не найдена.</exception>
        Task<BookResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создаёт новую книгу в указанном каталоге.
        /// </summary>
        /// <param name="request">Данные для создания книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Созданная книга.</returns>
        /// <exception cref="NotFoundException">Каталог, указанный в запросе, не найден.</exception>
        Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет название и описание книги. Каталог книги не меняется.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="request">Новые данные книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Обновлённая книга.</returns>
        /// <exception cref="NotFoundException">Книга с указанным идентификатором не найдена.</exception>
        Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет книгу.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <exception cref="NotFoundException">Книга с указанным идентификатором не найдена.</exception>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
