using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Api.Tests.Common;
using OnlineLibrary.Api.Tests.Infrastructure;
using OnlineLibrary.Application.Dtos.Books;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace OnlineLibrary.Api.Tests.Books
{
    [Collection(ApiCollection.Name)]
    public sealed class BooksApiTests
    {
        private readonly HttpClient _client;
        private readonly Fixture _fixture = new();

        public BooksApiTests(OnlineLibraryApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetListByCatalog_Existing_ReturnsCatalogBooks()
        {
            var catalog = await _client.CreateCatalogAsync();
            var first = await _client.CreateBookAsync(catalog.Id);
            var second = await _client.CreateBookAsync(catalog.Id);

            var response = await _client.GetAsync($"/api/catalogs/{catalog.Id}/books");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var books = await response.Content.ReadFromJsonAsync<List<BookResponse>>();
            books.ShouldNotBeNull();
            books.ShouldBe([first, second], ignoreOrder: true);
        }

        [Fact]
        public async Task GetListByCatalog_EmptyCatalog_ReturnsEmptyList()
        {
            var catalog = await _client.CreateCatalogAsync();

            var response = await _client.GetAsync($"/api/catalogs/{catalog.Id}/books");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var books = await response.Content.ReadFromJsonAsync<List<BookResponse>>();
            books.ShouldNotBeNull();
            books.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetListByCatalog_NotExistingCatalog_ReturnsNotFound()
        {
            var response = await _client.GetAsync($"/api/catalogs/{_fixture.Create<Guid>()}/books");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_Existing_ReturnsBook()
        {
            var catalog = await _client.CreateCatalogAsync();
            var book = await _client.CreateBookAsync(catalog.Id);

            var response = await _client.GetAsync($"/api/books/{book.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var fetched = await response.Content.ReadFromJsonAsync<BookResponse>();
            fetched.ShouldBe(book);
        }

        [Fact]
        public async Task GetById_NotExisting_ReturnsNotFound()
        {
            var response = await _client.GetAsync($"/api/books/{_fixture.Create<Guid>()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Title.ShouldBe("Не найдено");
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedWithLocation()
        {
            var catalog = await _client.CreateCatalogAsync();
            var title = _fixture.Create<string>();
            var request = TestData.ValidCreateBookRequest(catalog.Id) with { Title = $"  {title}  " };

            var response = await _client.PostAsJsonAsync("/api/books", request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var created = await response.Content.ReadFromJsonAsync<BookResponse>();
            created.ShouldNotBeNull();
            created.Title.ShouldBe(title);
            created.Description.ShouldBe(request.Description);
            created.CatalogId.ShouldBe(catalog.Id);
            response.Headers.Location.ShouldNotBeNull();

            var getResponse = await _client.GetAsync(response.Headers.Location);
            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var fetched = await getResponse.Content.ReadFromJsonAsync<BookResponse>();
            fetched.ShouldBe(created);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public async Task Create_EmptyTitle_ReturnsBadRequestWithTitleError(string title)
        {
            var request = TestData.ValidCreateBookRequest(_fixture.Create<Guid>()) with { Title = title };

            var response = await _client.PostAsJsonAsync("/api/books", request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Errors.ShouldContainKey("title");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public async Task Create_NullOrWhiteSpaceDescription_ReturnsNullDescription(string? description)
        {
            var catalog = await _client.CreateCatalogAsync();
            var request = TestData.ValidCreateBookRequest(catalog.Id) with { Description = description };

            var response = await _client.PostAsJsonAsync("/api/books", request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var created = await response.Content.ReadFromJsonAsync<BookResponse>();
            created.ShouldNotBeNull();
            created.Description.ShouldBeNull();
        }

        [Fact]
        public async Task Create_EmptyCatalogId_ReturnsBadRequestWithCatalogIdError()
        {
            var request = TestData.ValidCreateBookRequest(Guid.Empty);

            var response = await _client.PostAsJsonAsync("/api/books", request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem.ShouldNotBeNull();
            problem.Errors.ShouldContainKey("catalogId");
        }

        [Fact]
        public async Task Create_NotExistingCatalog_ReturnsNotFound()
        {
            var request = TestData.ValidCreateBookRequest(_fixture.Create<Guid>());

            var response = await _client.PostAsJsonAsync("/api/books", request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_Existing_ReturnsUpdatedBook()
        {
            var catalog = await _client.CreateCatalogAsync();
            var book = await _client.CreateBookAsync(catalog.Id);
            var request = TestData.ValidUpdateBookRequest();

            var response = await _client.PutAsJsonAsync($"/api/books/{book.Id}", request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var updated = await response.Content.ReadFromJsonAsync<BookResponse>();
            updated.ShouldBe(new BookResponse(book.Id, request.Title, request.Description, catalog.Id));

            var fetched = await _client.GetFromJsonAsync<BookResponse>($"/api/books/{book.Id}");
            fetched.ShouldBe(updated);
        }

        [Fact]
        public async Task Update_NotExisting_ReturnsNotFound()
        {
            var response = await _client.PutAsJsonAsync(
                $"/api/books/{_fixture.Create<Guid>()}",
                TestData.ValidUpdateBookRequest());

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Existing_ReturnsNoContent()
        {
            var catalog = await _client.CreateCatalogAsync();
            var book = await _client.CreateBookAsync(catalog.Id);

            var response = await _client.DeleteAsync($"/api/books/{book.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var getResponse = await _client.GetAsync($"/api/books/{book.Id}");
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_NotExisting_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync($"/api/books/{_fixture.Create<Guid>()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteCatalog_WithBooks_DeletesBooks()
        {
            var catalog = await _client.CreateCatalogAsync();
            var book = await _client.CreateBookAsync(catalog.Id);

            var deleteResponse = await _client.DeleteAsync($"/api/catalogs/{catalog.Id}");

            deleteResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var getResponse = await _client.GetAsync($"/api/books/{book.Id}");
            getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
