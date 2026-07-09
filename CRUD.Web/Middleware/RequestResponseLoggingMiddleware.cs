using System.Text;
using CRUD.Application.Optionss;
using Microsoft.Extensions.Options;


namespace CRUD.Web.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestResponseLoggingOptions _options;
        private readonly Serilog.ILogger _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            IOptions<RequestResponseLoggingOptions> options,
            Serilog.ILogger logger)
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

            var request = context.Request;
            var contentType = request.ContentType ?? "null";
            var contentLength = request.ContentLength?.ToString() ?? "null";
            _logger.Information(
                "Request starting {Method} {Scheme}://{Host}{Path} - {ContentType} {ContentLength}",
                request.Method,
                request.Scheme,
                request.Host,
                request.Path,
                contentType,
                contentLength);

            var originalBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var endpointName = context.GetEndpoint()?.DisplayName;
            if (!string.IsNullOrWhiteSpace(endpointName))
            {
                _logger.Information("Executing endpoint '{EndpointName}'", endpointName);
            }

            var responseText = await ReadResponseBody(context.Response);
            await responseBody.CopyToAsync(originalBody);
            context.Response.Body = originalBody;

            var endpoint = $"{context.Request.Method} {path}";
            var requestLog = string.IsNullOrWhiteSpace(requestBody) ? endpoint : requestBody;

            var logMessage =
                $"Request: {requestLog}{Environment.NewLine}" +
                $"Response: {responseText}";

            _logger.Information(logMessage);
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