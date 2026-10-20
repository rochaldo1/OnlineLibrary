using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Interfaces.Repositories
{
    /// <summary>
    /// Хранилище книг. Создание и удаление выполняются сразу.
    /// Изменения загруженной книги сохраняются вызовом <see cref="SaveChangesAsync"/>.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Возвращает книги каталога. Если книг нет, возвращает пустой список.
        /// </summary>
        /// <param name="catalogId">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список книг из каталога.</returns>
        Task<IReadOnlyList<Book>> GetListByCatalogIdAsync(Guid catalogId, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает книгу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Книга или <c>null</c>, если она не найдена.</returns>
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет книгу и сохраняет изменения. После вызова у книги заполнен идентификатор.
        /// </summary>
        /// <param name="book">Добавляемая книга.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task CreateAsync(Book book, CancellationToken cancellationToken);

        /// <summary>
        /// Сохраняет изменения книг, загруженных через этот репозиторий.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет книгу по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns><c>true</c>, если книга была удалена; <c>false</c>, если её не существует.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
