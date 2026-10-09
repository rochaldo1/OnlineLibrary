using OnlineLibrary.Application.Dtos.Catalogs;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Mapping
{
    /// <summary>
    /// Методы преобразования между каталогом и его DTO.
    /// </summary>
    public static class CatalogMappings
    {
        /// <summary>
        /// Преобразует каталог в ответ API.
        /// </summary>
        /// <param name="entity">Каталог.</param>
        /// <returns>Данные каталога для ответа.</returns>
        public static CatalogResponse ToResponse(this Catalog entity)
        {
            return new CatalogResponse(entity.Id, entity.Name);
        }
        
        /// <summary>
        /// Создаёт каталог из запроса на создание. Идентификатор задаётся автоматически при добавлении.
        /// </summary>
        /// <param name="request">Данные для создания каталога.</param>
        /// <returns>Новый каталог.</returns>
        public static Catalog ToEntity(this CreateCatalogRequest request)
        {
            return new Catalog { Name = request.Name };
        }
    }
}
