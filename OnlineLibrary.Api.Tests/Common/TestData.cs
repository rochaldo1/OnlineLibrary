using AutoFixture;
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

        public static UpdateCatalogRequest ValidUpdateCatalogRequest() =>
            new(new Fixture().Create<string>());
    }
}
