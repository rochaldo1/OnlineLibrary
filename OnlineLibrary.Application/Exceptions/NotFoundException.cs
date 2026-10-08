namespace OnlineLibrary.Application.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при отсутствии сущности с заданным идентификатором.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, Guid id)
            : base($"Сущность '{entityName}' с идентификатором '{id}' не найдена.")
        {
        }
    }
}
