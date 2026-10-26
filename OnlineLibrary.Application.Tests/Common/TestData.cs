using AutoFixture;
using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Domain.Entities;

namespace OnlineLibrary.Application.Tests.Common
{
    /// <summary>
    /// Вспомогательные методы для создания тестовых данных.
    /// </summary>
    internal static class TestData
    {
        /// <summary>
        /// Создаёт строку фиксированной длины.
        /// </summary>
        /// <param name="length">Длина строки.</param>
        /// <returns>Строка с фиксированной длиной.</returns>
        public static string CreateString(int length) =>
            new string(new Fixture().CreateMany<char>(length).ToArray());

        /// <summary>
        /// Создаёт корректный запрос на создание книги со сгенерированными значениями.
        /// </summary>
        /// <returns>Запрос, проходящий валидацию.</returns>
        public static CreateBookRequest ValidCreateBookRequest()
        {
            var fixture = new Fixture();

            return new CreateBookRequest(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<Guid>());
        }

        /// <summary>
        /// Создаёт корректный запрос на обновление книги со сгенерированными значениями.
        /// </summary>
        /// <returns>Запрос, проходящий валидацию.</returns>
        public static UpdateBookRequest ValidUpdateBookRequest()
        {
            var fixture = new Fixture();

            return new UpdateBookRequest(fixture.Create<string>(), fixture.Create<string>());
        }

        /// <summary>
        /// Создаёт каталог со сгенерированным идентификатором и названием.
        /// </summary>
        /// <returns>Каталог без книг.</returns>
        public static Catalog CreateCatalog()
        {
            var fixture = new Fixture();

            return new Catalog
            {
                Id = fixture.Create<Guid>(),
                Name = fixture.Create<string>()
            };
        }
    }
}
