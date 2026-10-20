using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Mapping
{
    /// <summary>
    /// Методы преобразования между книгой и её DTO.
    /// </summary>
    public static class BookMappings
    {
        /// <summary>
        /// Преобразует книгу в ответ API.
        /// </summary>
        /// <param name="entity">Книга.</param>
        /// <returns>Данные книги для ответа.</returns>
        public static BookResponse ToResponse(this Book entity)
        {
            return new BookResponse(entity.Id, entity.Title, entity.Description, entity.CatalogId);
        }

        /// <summary>
        /// Создаёт книгу из запроса на создание. Идентификатор задаётся автоматически при добавлении.
        /// </summary>
        /// <param name="request">Данные для создания книги.</param>
        /// <returns>Новая книга.</returns>
        public static Book ToEntity(this CreateBookRequest request)
        {
            return new Book
            {
                Title = request.Title,
                Description = request.Description,
                CatalogId = request.CatalogId
            };
        }
    }
}
