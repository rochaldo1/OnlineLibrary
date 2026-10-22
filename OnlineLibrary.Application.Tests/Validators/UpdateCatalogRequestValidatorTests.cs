using AutoFixture;
using FluentValidation.TestHelper;
using OnlineLibrary.Application.Dtos.Catalogs;
using OnlineLibrary.Application.Tests.Common;
using OnlineLibrary.Application.Validators;

namespace OnlineLibrary.Application.Tests.Validators
{
    public sealed class UpdateCatalogRequestValidatorTests
    {
        private readonly UpdateCatalogRequestValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public void Validate_NameNullOrWhiteSpace_HasErrorForName(string? name)
        {
            var request = new UpdateCatalogRequest(name!);

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Name)
                .WithErrorMessage("Название каталога не может быть пустым.");
        }

        [Fact]
        public void Validate_ValidName_HasNoErrors()
        {
            var request = new UpdateCatalogRequest(_fixture.Create<string>());

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_NameOfMaxLength_HasNoErrorForName()
        {
            var request = new UpdateCatalogRequest(TestData.CreateString(200));

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Name);
        }

        [Fact]
        public void Validate_NameExceedsMaxLength_HasErrorForName()
        {
            var request = new UpdateCatalogRequest(TestData.CreateString(201));

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Name)
                .WithErrorMessage("Название каталога не может превышать 200 символов.");
        }

        [Theory]
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public void Validate_NameOfMaxLengthWithSurroundingWhiteSpace_HasNoErrorForName(string prefix, string suffix)
        {
            var request = new UpdateCatalogRequest(prefix + TestData.CreateString(200) + suffix);

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Name);
        }
    }
}
