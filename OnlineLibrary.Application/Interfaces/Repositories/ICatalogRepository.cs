using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Interfaces.Repositories
{
    /// <summary>
    /// Хранилище каталогов. Операции изменения сразу сохраняют изменения в хранилище.
    /// </summary>
    public interface ICatalogRepository
    {
        /// <summary>
        /// Возвращает все каталоги. Если катаогов нет, возвращает пустой список.
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
        /// <param name="catalog">Добавляет каталог.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task CreateAsync(Catalog catalog, CancellationToken cancellationToken);

        /// <summary>
        /// Сохраняет изменения каталога, полученного из этого репозитория.
        /// </summary>
        /// <param name="catalog">Изменённый каталог.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task UpdateAsync(Catalog catalog, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет каталог вместе с его книгами.
        /// </summary>
        /// <param name="id">Идентификатор каталог.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns><c>true</c>, если каталог был удалён; <c>false</c>, если его не существует.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
