using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Interfaces.Repositories
{
    /// <summary>
    /// Хранилище каталогов. Создание и удаление сохраняются сразу.
    /// Изменения загруженного каталога сохраняются вызовом <see cref="SaveChangesAsync"/>.
    /// </summary>
    public interface ICatalogRepository
    {
        /// <summary>
        /// Возвращает все каталоги. Если каталогов нет, возвращает пустой список.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список каталогов.</returns>
        Task<IReadOnlyList<Catalog>> GetListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает каталог по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Каталог или <c>null</c>, если он не найден.</returns>
        Task<Catalog?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет каталог и сохраняет изменения. После вызова у каталога заполнен идентификатор.
        /// </summary>
        /// <param name="catalog">Добавляемый каталог.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task CreateAsync(Catalog catalog, CancellationToken cancellationToken);

        /// <summary>
        /// Сохраняет изменения каталогов, загруженных через этот репозиторий.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет каталог вместе с его книгами.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns><c>true</c>, если каталог был удалён; <c>false</c>, если его не существует.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Проверяет, существует ли каталог с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns><c>true</c>, если каталог существует, иначе <c>false</c>.</returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}
