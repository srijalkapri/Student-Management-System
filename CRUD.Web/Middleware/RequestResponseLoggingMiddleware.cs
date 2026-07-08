using System.Security.Claims;
using System.Text;
using CRUD.Application.Optionss;
using Microsoft.Extensions.Options;

namespace CRUD.Web.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestResponseLoggingOptions _options;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            IOptions<RequestResponseLoggingOptions> options,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_options.Enabled)
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value ?? string.Empty;
            if (_options.ExcludedPaths.Any(p =>
                    path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBody(context.Request);

            
            var originalBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);  

            
            var responseText = await ReadResponseBody(context.Response);
            await responseBody.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

           
            var userName = context.User.FindFirstValue(ClaimTypes.Name) ?? "anonymous";
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            var logMessage =
                $"[{context.Request.Method}] {path} | User: {userName} | Status: {context.Response.StatusCode}{Environment.NewLine}" +
                $"Request: {requestBody}{Environment.NewLine}" +
                $"Response: {responseText}";

            _logger.LogInformation(logMessage);
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
            {
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}