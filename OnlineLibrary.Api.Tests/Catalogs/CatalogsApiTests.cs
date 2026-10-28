using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Api.Tests.Common;
using OnlineLibrary.Api.Tests.Infrastructure;
using OnlineLibrary.Application.Dtos.Catalogs;
using Shouldly;

namespace OnlineLibrary.Api.Tests.Catalogs
{
    [Collection(ApiCollection.Name)]
    public sealed class CatalogsApiTests
    {
        private readonly HttpClient _client;
        private readonly Fixture _fixture = new();

        public CatalogsApiTests(OnlineLibraryApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetList_ContainsCreatedCatalogs()
        {
            var first = await _client.CreateCatalogAsync();
            var second = await _client.CreateCatalogAsync();

            var response = await _client.GetAsync("/api/catalogs");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var catalogs = await response.Content.ReadFromJsonAsync<List<CatalogResponse>>();
            catalogs.ShouldNotBeNull();
            catalogs.ShouldContain(first);
            catalogs.ShouldContain(second);
        }

        [Fact]
        public async Task GetById_Existing_ReturnsCatalog()
        {
            var catalog = await _client.CreateCatalogAsync();

            var response = await _client.GetAsync($"/api/catalogs/{catalog.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var fetched = await response.Content.ReadFromJsonAsync<CatalogResponse>();
            fetched.ShouldBe(catalog);
        }

        [Fact]
        public async Task GetById_NotExisting_ReturnsNotFound()
        {
            var response = await _client.GetAsync($"/api/catalogs/{_fixture.Create<Guid>()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Title.ShouldBe("Не найдено");
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedWithLocation()
        {
            var name = _fixture.Create<string>();

            var response = await _client.PostAsJsonAsync("/api/catalogs", new CreateCatalogRequest($"  {name}  "));

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var created = await response.Content.ReadFromJsonAsync<CatalogResponse>();
            created.ShouldNotBeNull();
            created.Name.ShouldBe(name);
            response.Headers.Location.ShouldNotBeNull();

            var getResponse = await _client.GetAsync(response.Headers.Location);
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var fetched = await getResponse.Content.ReadFromJsonAsync<CatalogResponse>();
            fetched.ShouldBe(created);
        }

        [Fact]
        public async Task Create_NameExceedsMaxLength_ReturnsBadRequestWithNameError()
        {
            var request = new CreateCatalogRequest(TestData.CreateString(201));

            var response = await _client.PostAsJsonAsync("/api/catalogs", request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Errors.ShouldContainKey("name");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public async Task Create_EmptyName_ReturnsBadRequestWithNameError(string name)
        {
            var response = await _client.PostAsJsonAsync("/api/catalogs", new CreateCatalogRequest(name));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Errors.ShouldContainKey("name");
        }

        [Fact]
        public async Task Create_MissingName_ReturnsBadRequestWithNameError()
        {
            var response = await _client.PostAsJsonAsync("/api/catalogs", new { });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Errors.ShouldContainKey("name");
        }

        [Fact]
        public async Task Update_Existing_ReturnsUpdatedCatalog()
        {
            var catalog = await _client.CreateCatalogAsync();
            var request = TestData.ValidUpdateCatalogRequest();

            var response = await _client.PutAsJsonAsync($"/api/catalogs/{catalog.Id}", request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var updated = await response.Content.ReadFromJsonAsync<CatalogResponse>();
            updated.ShouldBe(new CatalogResponse(catalog.Id, request.Name));

            var fetched = await _client.GetFromJsonAsync<CatalogResponse>($"/api/catalogs/{catalog.Id}");
            fetched.ShouldBe(updated);
        }

        [Fact]
        public async Task Update_NotExisting_ReturnsNotFound()
        {
            var response = await _client.PutAsJsonAsync(
                $"/api/catalogs/{_fixture.Create<Guid>()}",
                TestData.ValidUpdateCatalogRequest());

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Existing_ReturnsNoContent()
        {
            var catalog = await _client.CreateCatalogAsync();

            var response = await _client.DeleteAsync($"/api/catalogs/{catalog.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var getResponse = await _client.GetAsync($"/api/catalogs/{catalog.Id}");
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotExisting_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync($"/api/catalogs/{_fixture.Create<Guid>()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
