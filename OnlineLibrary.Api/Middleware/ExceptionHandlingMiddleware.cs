using Microsoft.AspNetCore.Mvc;
using OnlineLibrary.Application.Exceptions;

namespace OnlineLibrary.Api.Middleware
{
    /// <summary>
    /// Глобальный обработчик исключений: преобразует исключения в ответы ProblemDetails
    /// (404 для отсутствующих сущностей, 500 без технических деталей для остальных) и пишет их в лог.
    /// </summary>
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger, 
            IProblemDetailsService problemDetailsService)
        {
            _next = next;
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        /// <summary>
        /// Передаёт запрос дальше по конвейеру и перехватывает возникшие исключения.
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogInformation("Запрос {Method} {Path} отменён клиентом", context.Request.Method, context.Request.Path);
            }
            catch (Exception exception) when (!context.Response.HasStarted)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Преобразует исключение в ответ ProblemDetails 
        /// с соответствующим статусом и записывает его в лог.
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса.</param>
        /// <param name="exception">Перехваченное исключение.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title, detail) = exception switch
            {
                NotFoundException notFound => (
                    StatusCodes.Status404NotFound,
                    "Не найдено",
                    notFound.Message),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Внутренняя ошибка сервера",
                    "Произошла непредвиденная ошибка. Повторите запрос позже.")
            };

            if (statusCode >= StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Необработанное исключение при обработке запроса");
            else
                _logger.LogWarning("Ошибка запроса {StatusCode}: {Detail}", statusCode, detail);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = detail
                }
            });
        }
    }
}
