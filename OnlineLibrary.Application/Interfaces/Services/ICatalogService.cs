using OnlineLibrary.Application.Dtos.Catalogs;
using OnlineLibrary.Application.Exceptions;

namespace OnlineLibrary.Application.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с каталогами.
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Возвращает список каталогов. Если каталогов нет, возвращает пустой список.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список каталогов.</returns>
        Task<IReadOnlyList<CatalogResponse>> GetListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает каталог по идентификатору.
        /// </summary>
        /// <param name="id">Индентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденный каталог.</returns>
        /// <exception cref="NotFoundException">Каталог с указанным идентификатором не найден.</exception>
        Task<CatalogResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Создаёт новый каталог.
        /// </summary>
        /// <param name="request">Данные для создания каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Созданный каталог.</returns>
        Task<CatalogResponse> CreateAsync(CreateCatalogRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет название каталога.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="request">Новые данные каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Обновлённый каталог.</returns>
        /// <exception cref="NotFoundException">Каталог с указанным идентификатором не найден.</exception>
        Task<CatalogResponse> UpdateAsync(Guid id, UpdateCatalogRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет каталог вместе с его книгами.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <exception cref="NotFoundException">Каталог с указанным идентификатором не найден.</exception>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
