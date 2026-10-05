namespace OnlineLibrary.Domain.Entities
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
