namespace OnlineLibrary.Application.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при отсуствии сущности с заданным идентификатором.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, Guid id)
            : base($"Сущность '{entityName}' с идентификатором '{id}' не найдена.")
        {
        }
    }
}
