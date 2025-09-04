using MagicECommerce_API.DTOS;
using MagicECommerce_API.Exceptions.Base;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace MagicECommerce_API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred: : {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = exception switch
            {
                BaseCustomException customEx => new APIResponse<string>
                {
                    StatusCode = customEx.StatusCode,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { customEx.Message }
                },
                DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) => new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.Conflict,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "A record with this information already exists" }
                },
                ArgumentException argEx => new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { argEx.Message }
                },
                _ => new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while processing your request" }
                }
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            var innerMessage = ex.InnerException?.Message?.ToLower() ?? "";
            return innerMessage.Contains("unique") ||
                innerMessage.Contains("duplicate") ||
                innerMessage.Contains("constraint");
        }
    }
}
