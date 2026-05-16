using System.Net;
using System.Text.Json;
using TransportManagementSG.Application.Exceptions;

namespace TransportManagementSG.UI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception occurred. TraceId: {TraceId}",
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context,Exception exception)
        {
            if (IsAjaxRequest(context.Request))
            {
                await HandleAjaxException(context, exception);
            }
            else
            {
                HandleMvcException(context, exception);
            }
        }

        private bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private async Task HandleAjaxException(HttpContext context,Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            string message = "Something went wrong.";

            switch (exception)
            {
                case AppException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
            }

            var response = new
            {
                Success = false,
                Message = message,
                TraceId = context.TraceIdentifier,
                Details = _environment.IsDevelopment()
                    ? exception.Message
                    : null
            };

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        private void HandleMvcException(HttpContext context,Exception exception)
        {
            context.Response.Redirect("/Home/Error");
        }
    }
}
