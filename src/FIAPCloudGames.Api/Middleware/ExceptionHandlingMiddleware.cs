using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Domain.Exceptions;

namespace FIAPCloudGames.Api.Middleware
{
    // Presentation/Middleware/ExceptionHandlingMiddleware.cs
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Uma exceção não tratada ocorreu");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = validationEx.Message;
                    response.Errors = validationEx.Errors;
                    break;

                case NotFoundException notFoundEx:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = notFoundEx.Message;
                    break;

                case UnauthorizedException unauthorizedEx:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Message = unauthorizedEx.Message;
                    break;

                case ForbiddenException forbiddenEx:
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    response.StatusCode = StatusCodes.Status403Forbidden;
                    response.Message = forbiddenEx.Message;
                    break;

                case DomainException domainEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = domainEx.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "Ocorreu um erro interno no servidor";
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }

}
