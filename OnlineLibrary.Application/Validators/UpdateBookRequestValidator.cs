using FluentValidation;
using OnlineLibrary.Application.Common;
using OnlineLibrary.Application.Dtos.Books;

namespace OnlineLibrary.Application.Validators
{
    /// <summary>
    /// Валидатор данных для обновления книги.
    /// </summary>
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Название книги не может быть пустым.")
                .Must(title => title is null || TextNormalizer.NormalizeRequired(title).Length <= 200)
                .WithMessage("Название книги не может превышать 200 символов.");

            RuleFor(b => b.Description)
                .Must(description => (TextNormalizer.NormalizeOptional(description)?.Length ?? 0) <= 2000)
                .WithMessage("Описание книги не может превышать 2000 символов.");
        }
    }
}
