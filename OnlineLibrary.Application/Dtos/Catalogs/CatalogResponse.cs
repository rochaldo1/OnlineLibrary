namespace OnlineLibrary.Application.Dtos.Catalogs
{
    /// <summary>
    /// Каталог в ответе API.
    /// </summary>
    /// <param name="Id">Идентификатор каталога.</param>
    /// <param name="Name">Название каталога.</param>
    public record CatalogResponse(Guid Id, string Name);
}
