using FluentValidation;
using OnlineLibrary.Application.Dtos.Books;

namespace OnlineLibrary.Application.Validators
{
    /// <summary>
    /// Валидатор данных для создания книги.
    /// </summary>
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("Название книги не может быть пустым.")
                .Must(title => title is null || title.Trim().Length <= 200)
                .WithMessage("Название книги не может превышать 200 символов.");

            RuleFor(b => b.Description)
                .Must(description => description is null || description.Trim().Length <= 2000)
                .WithMessage("Описание книги не может превышать 2000 символов.");

            RuleFor(b => b.CatalogId)
                .NotEmpty()
                .WithMessage("Идентификатор каталога обязателен.");
        }
    }
}
