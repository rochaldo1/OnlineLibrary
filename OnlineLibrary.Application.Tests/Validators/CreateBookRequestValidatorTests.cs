using AutoFixture;
using FluentValidation.TestHelper;
using OnlineLibrary.Application.Tests.Common;
using OnlineLibrary.Application.Validators;

namespace OnlineLibrary.Application.Tests.Validators
{
    public sealed class CreateBookRequestValidatorTests
    {
        private readonly CreateBookRequestValidator _validator = new();

        [Fact]
        public void Validate_ValidRequest_HasNoErrors()
        {
            var request = TestData.ValidCreateBookRequest();

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public void Validate_TitleNullOrWhiteSpace_HasErrorForTitle(string? title)
        {
            var request = TestData.ValidCreateBookRequest() with { Title = title! };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Title)
                .WithErrorMessage("Название книги не может быть пустым.");
        }

        [Fact]
        public void Validate_TitleOfMaxLength_HasNoErrorForTitle()
        {
            var request = TestData.ValidCreateBookRequest() with { Title = TestData.CreateString(200) };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Title);
        }

        [Fact]
        public void Validate_TitleExceedsMaxLength_HasErrorForTitle()
        {
            var request = TestData.ValidCreateBookRequest() with { Title = TestData.CreateString(201) };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Title)
                .WithErrorMessage("Название книги не может превышать 200 символов.");
        }

        [Theory]
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public void Validate_TitleOfMaxLengthWithSurroundingWhiteSpace_HasNoErrorForTitle(string prefix, string suffix)
        {
            var request = TestData.ValidCreateBookRequest() with { Title = prefix + TestData.CreateString(200) + suffix };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public void Validate_DescriptionNullOrWhiteSpace_HasNoErrorForDescription(string? description)
        {
            var request = TestData.ValidCreateBookRequest() with { Description = description };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Description);
        }

        [Fact]
        public void Validate_DescriptionOfMaxLength_HasNoErrorForDescription()
        {
            var request = TestData.ValidCreateBookRequest() with { Description = TestData.CreateString(2000) };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Description);
        }

        [Fact]
        public void Validate_DescriptionExceedsMaxLength_HasErrorForDescription()
        {
            var request = TestData.ValidCreateBookRequest() with { Description = TestData.CreateString(2001) };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.Description)
                .WithErrorMessage("Описание книги не может превышать 2000 символов.");
        }

        [Theory]
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public void Validate_DescriptionOfMaxLengthWithSurroundingWhiteSpace_HasNoErrorForDescription(string prefix, string suffix)
        {
            var request = TestData.ValidCreateBookRequest() with { Description = prefix + TestData.CreateString(2000) + suffix };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(r => r.Description);
        }

        [Fact]
        public void Validate_CatalogIdEmpty_HasErrorForCatalogId()
        {
            var request = TestData.ValidCreateBookRequest() with { CatalogId = Guid.Empty };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(r => r.CatalogId)
                .WithErrorMessage("Идентификатор каталога обязателен.");
        }
    }
}
