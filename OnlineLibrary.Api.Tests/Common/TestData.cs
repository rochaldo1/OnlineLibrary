using AutoFixture;
using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Application.Dtos.Catalogs;

namespace OnlineLibrary.Api.Tests.Common
{
    /// <summary>
    /// Вспомогательные методы для создания тестовых данных.
    /// </summary>
    internal static class TestData
    {
        /// <summary>
        /// Создаёт строку заданной длины из сгенерированных символов.
        /// </summary>
        /// <param name="length">Длина строки.</param>
        /// <returns>Строка заданной длины.</returns>
        public static string CreateString(int length) =>
            new string(new Fixture().CreateMany<char>(length).ToArray());

        /// <summary>
        /// Создаёт корректный запрос на создание каталога со сгенерированным названием.
        /// </summary>
        /// <returns>Запрос, проходящий валидацию.</returns>
        public static CreateCatalogRequest ValidCreateCatalogRequest() =>
            new(new Fixture().Create<string>());

        /// <summary>
        /// Создаёт корректный запрос на обновление каталога со сгенерированным названием.
        /// </summary>
        /// <returns>Запрос, проходящий валидацию.</returns>
        public static UpdateCatalogRequest ValidUpdateCatalogRequest() =>
            new(new Fixture().Create<string>());

        /// <summary>
        /// Создаёт корректный запрос на создание книги в указанном каталоге
        /// со сгенерированными названием и описанием.
        /// </summary>
        /// <param name="catalogId">Идентификатор существующего каталога.</param>
        /// <returns>Запрос, проходящий валидацию.</returns>
        public static CreateBookRequest ValidCreateBookRequest(Guid catalogId)
        {
            var fixture = new Fixture();

            return new CreateBookRequest(fixture.Create<string>(), fixture.Create<string>(), catalogId);
        }

        /// <summary>
        /// Создаёт корректный запрос на обновление книги со сгенерированными названием и описанием.
        /// </summary>
        /// <returns>Запрос, проходящий валидацию.</returns>
        public static UpdateBookRequest ValidUpdateBookRequest()
        {
            var fixture = new Fixture();

            return new UpdateBookRequest(fixture.Create<string>(), fixture.Create<string>());
        }
    }
}
