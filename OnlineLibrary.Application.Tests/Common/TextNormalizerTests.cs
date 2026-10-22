using AutoFixture;
using OnlineLibrary.Application.Common;
using Shouldly;

namespace OnlineLibrary.Application.Tests.Common
{
    public sealed class TextNormalizerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void NormalizeRequired_Null_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => TextNormalizer.NormalizeRequired(null!));
        }

        [Theory]
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public void NormalizeRequired_ValueWithSurroundingWhiteSpace_ReturnsTrimmedValue(string prefix, string suffix)
        {
            var core = _fixture.Create<string>();

            var result = TextNormalizer.NormalizeRequired(prefix + core + suffix);

            result.ShouldBe(core);
        }

        [Fact]
        public void NormalizeRequired_ValueWithInnerSpaces_KeepsInnerSpaces()
        {
            var value = $"{_fixture.Create<string>()} {_fixture.Create<string>()}";

            var result = TextNormalizer.NormalizeRequired(value);

            result.ShouldBe(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public void NormalizeOptional_NullOrWhiteSpace_ReturnsNull(string? value)
        {
            var result = TextNormalizer.NormalizeOptional(value);

            result.ShouldBeNull();
        }

        [Theory]
        [InlineData("  ", "  ")]
        [InlineData("\t", "\n")]
        public void NormalizeOptional_ValueWithSurroundingWhiteSpace_ReturnsTrimmedValue(string prefix, string suffix)
        {
            var core = _fixture.Create<string>();

            var result = TextNormalizer.NormalizeOptional(prefix + core + suffix);

            result.ShouldBe(core);
        }
    }
}
