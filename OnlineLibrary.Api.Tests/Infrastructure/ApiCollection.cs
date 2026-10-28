namespace OnlineLibrary.Api.Tests.Infrastructure
{
    /// <summary>
    /// Коллекция тестов, использующих общий экземпляр приложения и базы данных.
    /// </summary>
    [CollectionDefinition(Name)]
    public sealed class ApiCollection : ICollectionFixture<OnlineLibraryApiFactory>
    {
        public const string Name = "Api";
    }
}
