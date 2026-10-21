using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Api.Filters;

namespace OnlineLibrary.Api
{
    /// <summary>
    /// Методы расширения для регистрации сервисов слоя Api в DI-контейнере.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует контроллеры, фильтр валидации, единый формат ответов об ошибках и Swagger.
        /// </summary>
        /// <param name="services">Коллекция сервисов приложения.</param>
        /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<ValidationFilter>();
                    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                        new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Ошибка валидации"
                        });
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddProblemDetails();

            return services;
        }
    }
}
