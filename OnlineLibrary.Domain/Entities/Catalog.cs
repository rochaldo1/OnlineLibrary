namespace OnlineLibrary.Domain.Entities
{
    /// <summary>
    /// Сущность "Каталог".
    /// </summary>
    public class Catalog
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
