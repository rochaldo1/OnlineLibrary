namespace OnlineLibrary.Application.Dtos.Books
{
    /// <summary>
    /// Книга в ответе API.
    /// </summary>
    /// <param name="Id">Идентификатор книги.</param>
    /// <param name="Title">Название книги.</param>
    /// <param name="Description">Описание книги (может отсутствовать).</param>
    /// <param name="CatalogId">Идентификатор каталога, к которому принадлежит книга.</param>
    public record BookResponse(Guid Id, string Title, string? Description, Guid CatalogId);
}
