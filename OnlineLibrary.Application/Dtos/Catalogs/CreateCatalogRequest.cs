namespace OnlineLibrary.Application.Dtos.Catalogs
{
    /// <summary>
    /// Данные для создания каталога.
    /// </summary>
    /// <param name="Name">Название каталога.</param>
    public record CreateCatalogRequest(string Name);
}
