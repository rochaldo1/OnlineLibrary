namespace OnlineLibrary.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public Guid CatalogId { get; set; }
        public Catalog Catalog { get; set; } = null!;
    }
}
