namespace OnlineLibrary.Application.Dtos.Books
{
    /// <summary>
    /// Данные для обновления книги.
    /// </summary>
    /// <param name="Title">Название книги.</param>
    /// <param name="Description">Описание книги (необязательно).</param>
    public record UpdateBookRequest(string Title, string? Description);
}
