using AutoFixture;

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
    }
}
