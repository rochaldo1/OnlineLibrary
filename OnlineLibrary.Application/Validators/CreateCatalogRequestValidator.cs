using FluentValidation;
using OnlineLibrary.Application.Dtos.Catalogs;

namespace OnlineLibrary.Application.Validators
{
    /// <summary>
    /// Валидатор данных для создания каталога.
    /// </summary>
    public class CreateCatalogRequestValidator : AbstractValidator<CreateCatalogRequest>
    {
        public CreateCatalogRequestValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Название каталога не может быть пустым.")
                .MaximumLength(200).WithMessage("Название каталога не может превышать 200 символов.");
        }
    }
}
