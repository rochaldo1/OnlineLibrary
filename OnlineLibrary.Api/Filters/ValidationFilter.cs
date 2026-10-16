using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace OnlineLibrary.Api.Filters
{
    /// <summary>
    /// Фильтр действий: проверяет аргументы зарегистрированным валидатором FluentValidation
    /// и при ошибке возвращает 400 с описанием по полям.
    /// </summary>
    public sealed class ValidationFilter : IAsyncActionFilter
    {
        /// <inheritdoc/>
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            var failures = new List<ValidationFailure>();

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                    continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

                if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
                    continue;

                var result = await validator.ValidateAsync(
                    new ValidationContext<object>(argument),
                    context.HttpContext.RequestAborted);

                failures.AddRange(result.Errors);
            }

            if (failures.Count > 0)
            {
                var errors = failures
                    .GroupBy(f => JsonNamingPolicy.CamelCase.ConvertName(f.PropertyName))
                    .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());

                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Ошибка валидации"
                });

                return;
            }

            await next();
        }
    }
}
