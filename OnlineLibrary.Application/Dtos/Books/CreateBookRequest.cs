namespace OnlineLibrary.Application.Dtos.Books
{
    /// <summary>
    /// Данные для создания книги.
    /// </summary>
    /// <param name="Title">Название книги.</param>
    /// <param name="Description">Описание книги (необязательно).</param>
    /// <param name="CatalogId">Идентификатор каталога, в который добавляется книга.</param>
    public record CreateBookRequest(string Title, string? Description, Guid CatalogId);
}
