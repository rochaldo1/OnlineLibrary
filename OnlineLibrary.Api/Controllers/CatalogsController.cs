using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Application.Dtos.Catalogs;
using OnlineLibrary.Application.Interfaces.Services;

namespace OnlineLibrary.Api.Controllers
{
    /// <summary>
    /// Управление каталогами.
    /// </summary>
    [ApiController]
    [Route("api/catalogs")]
    public sealed class CatalogsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Возвращает все каталоги, отсортированные по названию.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Список каталогов.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CatalogResponse>>> GetList(CancellationToken cancellationToken)
        {
            var catalogs = await _catalogService.GetListAsync(cancellationToken);

            return Ok(catalogs);
        }

        /// <summary>
        /// Возвращает каталог по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Найденный каталог.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CatalogResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var catalog = await _catalogService.GetByIdAsync(id, cancellationToken);

            return Ok(catalog);
        }

        /// <summary>
        /// Создаёт каталог.
        /// </summary>
        /// <param name="request">Данные нового каталога.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Созданный каталог и ссылка на него в заголовке Location.</returns>
        [HttpPost]
        public async Task<ActionResult<CatalogResponse>> Create(CreateCatalogRequest request, CancellationToken cancellationToken)
        {
            var catalog = await _catalogService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = catalog.Id },
                catalog);
        }

        /// <summary>
        /// Обновляет название каталога.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="request">Новые данные каталога.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Обновлённый каталог.</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CatalogResponse>> Update(Guid id, UpdateCatalogRequest request, CancellationToken cancellationToken)
        {
            var catalog = await _catalogService.UpdateAsync(id, request, cancellationToken);

            return Ok(catalog);
        }

        /// <summary>
        /// Удаляет каталог вместе с его книгами.
        /// </summary>
        /// <param name="id">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _catalogService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
