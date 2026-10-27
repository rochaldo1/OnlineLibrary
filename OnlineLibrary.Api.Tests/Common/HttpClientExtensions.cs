using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Application.Dtos.Catalogs;
using System.Net.Http.Json;

namespace OnlineLibrary.Api.Tests.Common
{
    /// <summary>
    /// Методы подготовки данных через API для интеграционных тестов.
    /// </summary>
    internal static class HttpClientExtensions
    {
        /// <summary>
        /// Создаёт каталог через API и возвращает его. Падает, если создание не удалось.
        /// </summary>
        /// <param name="client">Клиент API.</param>
        /// <returns>Созданный каталог.</returns>
        public static async Task<CatalogResponse> CreateCatalogAsync(this HttpClient client)
        {
            var response = await client.PostAsJsonAsync("/api/catalogs", TestData.ValidCreateCatalogRequest());
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<CatalogResponse>())!;
        }

        /// <summary>
        /// Создаёт книгу в указанном каталоге через API и возвращает её. Падает, если создание не удалось.
        /// </summary>
        /// <param name="client">Клиент API.</param>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public static async Task<BookResponse> CreateBookAsync(this HttpClient client, Guid catalogId)
        {
            var response = await client.PostAsJsonAsync("/api/books", TestData.ValidCreateBookRequest(catalogId));
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<BookResponse>())!;
        }
    }
}
