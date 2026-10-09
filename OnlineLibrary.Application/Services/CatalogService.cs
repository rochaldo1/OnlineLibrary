using OnlineLibrary.Application.Dtos.Catalogs;
using OnlineLibrary.Application.Exceptions;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Application.Mapping;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Services
{
    /// <summary>
    /// Сервис для работы с каталогами.
    /// </summary>
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<CatalogResponse>> GetListAsync(CancellationToken cancellationToken)
        {
            var catalogs = await _catalogRepository.GetListAsync(cancellationToken);

            return catalogs
                .Select(c => c.ToResponse())
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<CatalogResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var catalog = await GetCatalogOrThrowAsync(id, cancellationToken);

            return catalog.ToResponse();
        }

        /// <inheritdoc/>
        public async Task<CatalogResponse> CreateAsync(CreateCatalogRequest request, CancellationToken cancellationToken)
        {
            var catalog = request.ToEntity();

            catalog.Name = NormalizeName(catalog.Name);
            await _catalogRepository.CreateAsync(catalog, cancellationToken);

            return catalog.ToResponse();
        }

        /// <inheritdoc/>
        public async Task<CatalogResponse> UpdateAsync(Guid id, UpdateCatalogRequest request, CancellationToken cancellationToken)
        {
            var catalog = await GetCatalogOrThrowAsync(id, cancellationToken);

            catalog.Name = NormalizeName(request.Name);
            await _catalogRepository.UpdateAsync(catalog, cancellationToken);

            return catalog.ToResponse();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _catalogRepository.DeleteAsync(id, cancellationToken);

            if (!deleted)
                throw new NotFoundException("Каталог", id);
        }

        /// <summary>
        /// Загружает каталог по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Найденный каталог.</returns>
        /// <exception cref="NotFoundException">Каталог с указанным идентификатором не найден.</exception>
        private async Task<Catalog> GetCatalogOrThrowAsync(Guid id, CancellationToken cancellationToken)
        {
            var catalog = await _catalogRepository.GetByIdAsync(id, cancellationToken);

            if (catalog is null)
                throw new NotFoundException("Каталог", id);

            return catalog;
        }

        /// <summary>
        /// Обрезает пробелы по краям названия каталога.
        /// </summary>
        /// <param name="name">Название каталога.</param>
        /// <returns>Название каталога без пробелов по краям.</returns>
        private static string NormalizeName(string name)
        {
            return name.Trim();
        }
    }
}
