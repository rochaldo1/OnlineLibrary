using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace OnlineLibrary.Api.Tests.Infrastructure
{
    /// <summary>
    /// Запускает приложение для интеграционных тестов с одноразовой базой PostgreSQL в Docker.
    /// </summary>
    public sealed class OnlineLibraryApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:17").Build();

        public Task InitializeAsync() => _database.StartAsync();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ConnectionStrings:DefaultConnection", _database.GetConnectionString());
        }

        Task IAsyncLifetime.DisposeAsync() => _database.DisposeAsync().AsTask();
    }
}
