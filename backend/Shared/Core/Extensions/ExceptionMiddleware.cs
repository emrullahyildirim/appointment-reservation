using Core.CrossCuttingConcerns.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace Core.Extensions
{
    /// <summary>
    /// Global exception handling middleware
    /// Standart API response formatında hata yanıtları döner
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerServiceBase _loggerServiceBase;
        private const string JsonContentType = "application/json";

        public ExceptionMiddleware(RequestDelegate next, ILoggerServiceBase loggerServiceBase)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _loggerServiceBase = loggerServiceBase ?? throw new ArgumentNullException(nameof(loggerServiceBase));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                LogRequestCompletion(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private void LogRequestCompletion(HttpContext context)
        {
            var logContext = CreateLogContext(context);
            var statusCode = context.Response.StatusCode;

            var message = $"[User: {logContext.UserId}] [IP: {logContext.IpAddress}] " +
                         $"{logContext.Method} {logContext.Path} -> {statusCode}";

            if (statusCode is 401 or 403)
            {
                _loggerServiceBase.Warn(message);
            }
            else
            {
                _loggerServiceBase.Info(message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var logContext = CreateLogContext(context);

            // Log the exception
            _loggerServiceBase.Error(
                $"[User: {logContext.UserId}] [IP: {logContext.IpAddress}] " +
                $"{logContext.Method} {logContext.Path} - {exception.GetType().Name}: {exception.Message}",
                exception);

            // Build response
            var (statusCode, errorResponse) = BuildErrorResponse(exception);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = JsonContentType;

            await context.Response.WriteAsync(errorResponse.ToString());
        }

        private static (int StatusCode, ApiResponse Response) BuildErrorResponse(Exception exception)
        {
            return exception switch
            {
                ValidationException validationException => BuildValidationErrorResponse(validationException),
                
                KeyNotFoundException => (
                    (int)HttpStatusCode.NotFound,
                    ApiResponse.ErrorResponse("İstenen kaynak bulunamadı.")
                ),
                
                UnauthorizedAccessException => (
                    (int)HttpStatusCode.Unauthorized,
                    ApiResponse.ErrorResponse("Bu kaynağa erişim yetkiniz bulunmamaktadır.")
                ),
                
                InvalidOperationException => (
                    (int)HttpStatusCode.BadRequest,
                    ApiResponse.ErrorResponse("İstenen işlem geçerli değil.")
                ),
                
                ArgumentNullException argNullEx => (
                    (int)HttpStatusCode.BadRequest,
                    ApiResponse.ErrorResponse($"'{argNullEx.ParamName}' parametresi boş olamaz.")
                ),
                
                ArgumentException argEx => (
                    (int)HttpStatusCode.BadRequest,
                    ApiResponse.ErrorResponse(argEx.Message)
                ),
                
                TimeoutException => (
                    (int)HttpStatusCode.GatewayTimeout,
                    ApiResponse.ErrorResponse("İşlem zaman aşımına uğradı. Lütfen tekrar deneyin.")
                ),
                
                OperationCanceledException => (
                    (int)HttpStatusCode.BadRequest,
                    ApiResponse.ErrorResponse("İşlem iptal edildi.")
                ),
                
                NotImplementedException => (
                    (int)HttpStatusCode.NotImplemented,
                    ApiResponse.ErrorResponse("Bu özellik henüz desteklenmiyor.")
                ),
                
                _ => (
                    (int)HttpStatusCode.InternalServerError,
                    ApiResponse.ErrorResponse("Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.")
                )
            };
        }

        private static (int, ApiResponse) BuildValidationErrorResponse(ValidationException exception)
        {
            var errors = exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => ToCamelCase(g.Key),
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return ((int)HttpStatusCode.BadRequest, ApiResponse.ErrorResponse("Doğrulama hatası.", errors));
        }

        private static string ToCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        private static RequestLogContext CreateLogContext(HttpContext context)
        {
            return new RequestLogContext
            {
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous",
                Method = context.Request.Method,
                Path = context.Request.Path
            };
        }

        private sealed class RequestLogContext
        {
            public string IpAddress { get; init; } = string.Empty;
            public string UserId { get; init; } = string.Empty;
            public string Method { get; init; } = string.Empty;
            public string Path { get; init; } = string.Empty;
        }
    }
}
