namespace OnlineLibrary.Application.Common
{
    /// <summary>
    /// Правила нормализации текстовых полей перед проверкой и сохранением.
    /// </summary>
    public static class TextNormalizer
    {
        /// <summary>
        /// Обрезает пробельные символы по краям обязательного поля.
        /// </summary>
        /// <param name="value">Исходное значение.</param>
        /// <returns>Значение без пробельных символов по краям.</returns>
        public static string NormalizeRequired(string value)
        {
            return value.Trim();
        }

        /// <summary>
        /// Обрезает пробельные символы по краям необязательного поля.
        /// </summary>
        /// <param name="value">Исходное значение.</param>
        /// <returns>Значение без пробельных символов по краям или <c>null</c> если
        /// значение отсутствует или состоит из одних пробельных символов.</returns>
        public static string? NormalizeOptional(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.Trim();
        }
    }
}
