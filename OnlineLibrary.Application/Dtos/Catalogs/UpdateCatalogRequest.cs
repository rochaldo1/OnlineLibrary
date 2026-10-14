namespace OnlineLibrary.Application.Dtos.Catalogs
{
    /// <summary>
    /// Данные для обновления каталога.
    /// </summary>
    /// <param name="Name">Название каталога.</param>
    public record UpdateCatalogRequest(string Name);
}
