using System.Net;
using OnlineLibrary.Api.Tests.Infrastructure;
using Shouldly;

namespace OnlineLibrary.Api.Tests.Catalogs
{
    [Collection(ApiCollection.Name)]
    public sealed class CatalogsApiTests
    {
        private readonly HttpClient _client;

        public CatalogsApiTests(OnlineLibraryApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetList_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/catalogs");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
