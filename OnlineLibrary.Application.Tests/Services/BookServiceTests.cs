using AutoFixture;
using NSubstitute;
using OnlineLibrary.Application.Dtos.Books;
using OnlineLibrary.Application.Exceptions;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Services;
using OnlineLibrary.Application.Tests.Common;
using OnlineLibrary.Domain.Entities;
using Shouldly;

namespace OnlineLibrary.Application.Tests.Services
{
    public sealed class BookServiceTests
    {
        private readonly ICatalogRepository _catalogRepository = Substitute.For<ICatalogRepository>();
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
        private readonly BookService _service;
        private readonly Fixture _fixture = new();

        public BookServiceTests()
        {
            _service = new BookService(_bookRepository, _catalogRepository);
        }

        [Fact]
        public async Task GetListByCatalogIdAsync_CatalogExists_ReturnsMappedBooks()
        {
            var catalogId = _fixture.Create<Guid>();
            var books = new List<Book> { TestData.CreateBook(catalogId), TestData.CreateBook(catalogId) };
            _catalogRepository.ExistsAsync(catalogId, Arg.Any<CancellationToken>()).Returns(true);
            _bookRepository.GetListByCatalogIdAsync(catalogId, Arg.Any<CancellationToken>()).Returns(books);

            var result = await _service.GetListByCatalogIdAsync(catalogId, CancellationToken.None);

            result.ShouldBe(books.Select(b => new BookResponse(b.Id, b.Title, b.Description, b.CatalogId)));
        }

        [Fact]
        public async Task GetListByCatalogIdAsync_CatalogNotFound_ThrowsAndDoesNotQueryBooks()
        {
            var catalogId = _fixture.Create<Guid>();
            _catalogRepository.ExistsAsync(catalogId, Arg.Any<CancellationToken>()).Returns(false);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.GetListByCatalogIdAsync(catalogId, CancellationToken.None));

            await _bookRepository.DidNotReceive().GetListByCatalogIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetByIdAsync_BookExists_ReturnsBook()
        {
            var book = TestData.CreateBook(_fixture.Create<Guid>());
            _bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);

            var result = await _service.GetByIdAsync(book.Id, CancellationToken.None);

            result.ShouldBe(new BookResponse(book.Id, book.Title, book.Description, book.CatalogId));
        }

        [Fact]
        public async Task GetByIdAsync_BookNotFound_ThrowsNotFoundException()
        {
            var id = _fixture.Create<Guid>();
            _bookRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Book?)null);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.GetByIdAsync(id, CancellationToken.None));
        }

        [Theory]
        [InlineData("", "")] // Без пробельных символов
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public async Task CreateAsync_ValidRequest_CreatesBookWithNormalizedValues(string prefix, string suffix)
        {
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var catalogId = _fixture.Create<Guid>();
            var request = new CreateBookRequest(
                prefix + title + suffix, 
                prefix + description + suffix, 
                catalogId);
            _catalogRepository.ExistsAsync(catalogId, Arg.Any<CancellationToken>()).Returns(true);

            var result = await _service.CreateAsync(request, CancellationToken.None);

            await _bookRepository.Received(1).CreateAsync(
                Arg.Is<Book>(b => b.Title == title && b.Description == description && b.CatalogId == catalogId),
                Arg.Any<CancellationToken>());
            result.Title.ShouldBe(title);
            result.Description.ShouldBe(description);
            result.CatalogId.ShouldBe(catalogId);
        }

        [Fact]
        public async Task CreateAsync_CatalogNotFound_ThrowsAndDoesNotCreate()
        {
            var request = TestData.ValidCreateBookRequest();
            _catalogRepository.ExistsAsync(request.CatalogId, Arg.Any<CancellationToken>()).Returns(false);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.CreateAsync(request, CancellationToken.None));

            await _bookRepository.DidNotReceive().CreateAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsIdAssignedOnCreate()
        {
            var id = _fixture.Create<Guid>();
            var request = TestData.ValidCreateBookRequest();
            _catalogRepository.ExistsAsync(request.CatalogId, Arg.Any<CancellationToken>()).Returns(true);
            _bookRepository
                .When(r => r.CreateAsync(Arg.Any<Book>(), Arg.Any<CancellationToken>()))
                .Do(call => call.Arg<Book>().Id = id);

            var result = await _service.CreateAsync(request, CancellationToken.None);

            result.Id.ShouldBe(id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public async Task CreateAsync_NullOrWhiteSpaceDescription_CreatesBookWithNullDescription(string? description)
        {
            var request = TestData.ValidCreateBookRequest() with { Description = description };
            _catalogRepository.ExistsAsync(request.CatalogId, Arg.Any<CancellationToken>()).Returns(true);

            await _service.CreateAsync(request, CancellationToken.None);

            await _bookRepository.Received(1).CreateAsync(
                Arg.Is<Book>(b => b.Description == null),
                Arg.Any<CancellationToken>());
        }

        [Theory]
        [InlineData("", "")] // Без пробельных символов
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public async Task UpdateAsync_BookExists_UpdatesValuesAndSavesChanges(string prefix, string suffix)
        {
            var catalogId = _fixture.Create<Guid>();
            var book = TestData.CreateBook(catalogId);
            var newTitle = _fixture.Create<string>();
            var newDescription = _fixture.Create<string>();
            var request = new UpdateBookRequest(
                prefix + newTitle + suffix,
                prefix + newDescription + suffix);
            _bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);

            var result = await _service.UpdateAsync(book.Id, request, CancellationToken.None);

            book.Title.ShouldBe(newTitle);
            book.Description.ShouldBe(newDescription);
            book.CatalogId.ShouldBe(catalogId);
            result.ShouldBe(new BookResponse(book.Id, newTitle, newDescription, catalogId));
            await _bookRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_BookNotFound_ThrowsAndDoesNotSave()
        {
            var id = _fixture.Create<Guid>();
            var request = TestData.ValidUpdateBookRequest();
            _bookRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Book?)null);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.UpdateAsync(id, request, CancellationToken.None));

            await _bookRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_BookExists_DeletesBook()
        {
            var id = _fixture.Create<Guid>();
            _bookRepository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(true);

            await Should.NotThrowAsync(
                () => _service.DeleteAsync(id, CancellationToken.None));

            await _bookRepository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_BookNotFound_ThrowsNotFoundException()
        {
            var id = _fixture.Create<Guid>();
            _bookRepository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(false);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.DeleteAsync(id, CancellationToken.None));
        }
    }
}
