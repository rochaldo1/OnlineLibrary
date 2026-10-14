using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OnlineLibrary.Application.Interfaces.Services;
using OnlineLibrary.Application.Services;

namespace OnlineLibrary.Application
{
    /// <summary>
    /// Методы расширения для регистрации сервисов слоя Application в DI-контейнере. 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует сервисы и валидаторы.
        /// </summary>
        /// <param name="services">Коллекция сервисов приложения.</param>
        /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
