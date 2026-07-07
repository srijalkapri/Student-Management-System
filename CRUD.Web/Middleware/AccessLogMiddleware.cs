using System.Security.Claims;
using CRUD.Application.Interfaces;
using CRUD.Application.Optionss;
using CRUD.Domain.Models;
using Microsoft.Extensions.Options;

namespace CRUD.Web.Middleware
{
    public class AccessLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AccessLogOptions _options;

        public AccessLogMiddleware(RequestDelegate next, IOptions<AccessLogOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context, IAcessLogWriter accessLogWriter)
        {
            await _next(context);

            if (!_options.Enabled)
            {
                return;
            }

            var apiPath = context.Request.Path.Value ?? string.Empty;
            if (_options.ExcludedPaths.Any(path =>
                    apiPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            int? userId = null;
            var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var log = new AcessLog
            {   
                UserId = userId,
                UserName = context.User.FindFirstValue(ClaimTypes.Name),
                ApiPath = apiPath,
                HttpMethod = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                CreatedAtUtc = DateTime.UtcNow
            };

            await accessLogWriter.Log(log, context.RequestAborted);
        }
    }
}
