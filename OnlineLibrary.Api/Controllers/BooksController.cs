using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Application.Interfaces.Services;

namespace OnlineLibrary.Api.Controllers
{
    /// <summary>
    /// Управление книгами.
    /// </summary>
    [ApiController]
    [Route("api/books")]
    public sealed class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Возвращает книги каталога, отсортированные по названию. Если в каталоге нет книг, возвращает пустой список.
        /// </summary>
        /// <param name="catalogId">Идентификатор каталога.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Список книг каталога.</returns>
        [HttpGet("/api/catalogs/{catalogId:guid}/books")]
        [ProducesResponseType(typeof(IReadOnlyList<BookResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<BookResponse>>> GetListByCatalogId(
            Guid catalogId,
            CancellationToken cancellationToken)
        {
            var books = await _bookService.GetListByCatalogIdAsync(catalogId, cancellationToken);

            return Ok(books);
        }

        /// <summary>
        /// Возвращает книгу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Найденная книга.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(id, cancellationToken);

            return Ok(book);
        }

        /// <summary>
        /// Создаёт книгу.
        /// </summary>
        /// <param name="request">Данные новой книги.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Созданная книга и ссылка на неё в заголовке Location.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _bookService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book);
        }

        /// <summary>
        /// Обновляет название и описание книги. Каталог книги не меняется.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="request">Новые данные книги.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Обновлённая книга.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookResponse>> Update(Guid id, UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _bookService.UpdateAsync(id, request, cancellationToken);

            return Ok(book);
        }

        /// <summary>
        /// Удаляет книгу.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
