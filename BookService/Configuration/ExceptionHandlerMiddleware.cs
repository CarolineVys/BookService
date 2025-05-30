using System.Net;
using System.Net.Mime;

namespace BookService.Configuration
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Message} {StackTrace}", ex.Message, ex.StackTrace);

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
        }

    }
}
