using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineLibrary.Infrastructure.Data;

namespace OnlineLibrary.Infrastructure
{
    /// <summary>
    /// Методы расширения для регистрации сервисов слоя Infrastructure в DI-контейнере 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует AppDbContext с PostgreSQL
        /// </summary>
        /// <param name="services">Коллекция сервисов приложения</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <returns>Коллекция сервисов для цепочки вызовов</returns>
        /// <exception cref="InvalidOperationException">Строка подключения DefaultConnection не задана в конфигурации</exception>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            string? connection = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connection))
                throw new InvalidOperationException("Не задана строка подключения ConnectionStrings:DefaultConnection");

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));

            return services;
        }
    }
}
