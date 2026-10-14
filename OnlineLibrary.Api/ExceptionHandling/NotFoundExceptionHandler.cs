using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Application.Exceptions;

namespace OnlineLibrary.Api.ExceptionHandling
{
    /// <summary>
    /// Обработчик исключения <see cref="NotFoundException"/>: возвращает ответ 404 Not Found.
    /// </summary>
    public sealed class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<NotFoundExceptionHandler> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger, IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        /// <inheritdoc/>
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException notFound)
                return false;

            _logger.LogWarning("Сущность не найдена: {Message}", notFound.Message);
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            var problemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = "Не найдено",
                Detail = notFound.Message
            };

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
        }
    }
}
