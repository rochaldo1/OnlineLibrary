using AutoFixture;
using NSubstitute;
using OnlineLibrary.Application.Dtos.Catalogs;
using OnlineLibrary.Application.Exceptions;
using OnlineLibrary.Application.Interfaces.Repositories;
using OnlineLibrary.Application.Services;
using OnlineLibrary.Application.Tests.Common;
using OnlineLibrary.Domain.Entities;
using Shouldly;

namespace OnlineLibrary.Application.Tests.Services
{
    public sealed class CatalogServiceTests
    {
        private readonly ICatalogRepository _repository = Substitute.For<ICatalogRepository>();
        private readonly CatalogService _service;
        private readonly Fixture _fixture = new();

        public CatalogServiceTests()
        {
            _service = new CatalogService(_repository);
        }

        [Fact]
        public async Task GetByIdAsync_CatalogExists_ReturnsCatalog()
        {
            var catalog = TestData.CreateCatalog();
            _repository.GetByIdAsync(catalog.Id, Arg.Any<CancellationToken>()).Returns(catalog);

            var result = await _service.GetByIdAsync(catalog.Id, CancellationToken.None);

            result.ShouldBe(new CatalogResponse(catalog.Id, catalog.Name));
        }

        [Fact]
        public async Task GetByIdAsync_CatalogNotFound_ThrowsNotFoundException()
        {
            var id = _fixture.Create<Guid>();
            _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Catalog?)null);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.GetByIdAsync(id, CancellationToken.None));
        }

        [Fact]
        public async Task GetListAsync_ReturnsMappedCatalogs()
        {
            var catalogs = new List<Catalog> { TestData.CreateCatalog(), TestData.CreateCatalog() };
            _repository.GetListAsync(Arg.Any<CancellationToken>()).Returns(catalogs);

            var result = await _service.GetListAsync(CancellationToken.None);

            result.ShouldBe(catalogs.Select(c => new CatalogResponse(c.Id, c.Name)));
        }

        [Theory]
        [InlineData("", "")] // Без пробельных символов
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public async Task CreateAsync_ValidRequest_CreatesCatalogWithNormalizedName(string prefix, string suffix)
        {
            var name = _fixture.Create<string>();
            var request = new CreateCatalogRequest(prefix + name + suffix);

            var result = await _service.CreateAsync(request, CancellationToken.None);

            await _repository.Received(1).CreateAsync(
                Arg.Is<Catalog>(c => c.Name == name),
                Arg.Any<CancellationToken>());
            result.Name.ShouldBe(name);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsIdAssignedOnCreate()
        {
            var id = _fixture.Create<Guid>();
            var request = new CreateCatalogRequest(_fixture.Create<string>());
            _repository
                .When(r => r.CreateAsync(Arg.Any<Catalog>(), Arg.Any<CancellationToken>()))
                .Do(call => call.Arg<Catalog>().Id = id);

            var result = await _service.CreateAsync(request, CancellationToken.None);

            result.Id.ShouldBe(id);
        }

        [Theory]
        [InlineData("", "")] // Без пробельных символов
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public async Task UpdateAsync_CatalogExists_UpdatesNameAndSavesChanges(string prefix, string suffix)
        {
            var catalog = TestData.CreateCatalog();
            var newName = _fixture.Create<string>();
            var request = new UpdateCatalogRequest(prefix + newName + suffix);
            _repository.GetByIdAsync(catalog.Id, Arg.Any<CancellationToken>()).Returns(catalog);

            var result = await _service.UpdateAsync(catalog.Id, request, CancellationToken.None);

            catalog.Name.ShouldBe(newName);
            result.ShouldBe(new CatalogResponse(catalog.Id, newName));
            await _repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_CatalogNotFound_ThrowsAndDoesNotSave()
        {
            var id = _fixture.Create<Guid>();
            var request = new UpdateCatalogRequest(_fixture.Create<string>());
            _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Catalog?)null);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.UpdateAsync(id, request, CancellationToken.None));

            await _repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_CatalogExists_DeletesCatalog()
        {
            var id = _fixture.Create<Guid>();
            _repository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(true);

            await Should.NotThrowAsync(
                () => _service.DeleteAsync(id, CancellationToken.None));

            await _repository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_CatalogNotFound_ThrowsNotFoundException()
        {
            var id = _fixture.Create<Guid>();
            _repository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(false);

            await Should.ThrowAsync<NotFoundException>(
                () => _service.DeleteAsync(id, CancellationToken.None));
        }
    }
}
