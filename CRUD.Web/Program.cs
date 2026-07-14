using System.Text;
using CRUD.Application;
using CRUD.Application.Optionss;
using CRUD.Application.Responses;
using CRUD.Infrastructure;
using CRUD.Infrastructure.Persistence;
using CRUD.Web.Logging;
using CRUD.Web.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var rrOptions = builder.Configuration
    .GetSection("RequestResponseLogging")
    .Get<RequestResponseLoggingOptions>() ?? new RequestResponseLoggingOptions();

var requestResponseLogger = RequestResponseLoggerFactory.Create(rrOptions);

builder.Services.Configure<RequestResponseLoggingOptions>(
    builder.Configuration.GetSection("RequestResponseLogging"));
builder.Services.AddSingleton<Serilog.ILogger>(requestResponseLogger);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<AccessLogOptions>(builder.Configuration.GetSection("AccessLog"));
builder.Services.AddFluentValidationAutoValidation();

var allowedOrigins = ResolveCorsOrigins(builder.Configuration, builder.Environment);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

Console.WriteLine($"CORS allowed origins: {string.Join(", ", allowedOrigins)}");

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = new ServiceResponse<object>
        {
            Success = false,
            Message = "One or more validation errors occurred.",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

await DbInitializer.Initialize(app.Services);

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment() ||
    string.Equals(builder.Configuration["EnableSwagger"], "true", StringComparison.OrdinalIgnoreCase))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API v1");
    });
}

if (!app.Environment.IsDevelopment())
{
    // Behind Render/Neon reverse proxies; forwarded headers already applied.
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<AccessLogMiddleware>();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .AllowAnonymous();

app.MapControllers();

app.Lifetime.ApplicationStopping.Register(() =>
{
    if (requestResponseLogger is IDisposable disposable)
        disposable.Dispose();
});

app.Run();

static string[] ResolveCorsOrigins(IConfiguration configuration, IHostEnvironment environment)
{
    var fromArray = configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>()?
        .Where(o => !string.IsNullOrWhiteSpace(o))
        .Select(o => o.Trim().TrimEnd('/'))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray() ?? Array.Empty<string>();

    if (fromArray.Length > 0)
    {
        return fromArray;
    }

    // Supports: Cors__AllowedOrigins=https://a.vercel.app,http://localhost:5173
    var csv = configuration["Cors:AllowedOrigins"];
    if (!string.IsNullOrWhiteSpace(csv))
    {
        return csv
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(o => o.TrimEnd('/'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    if (environment.IsDevelopment())
    {
        return
        [
            "http://localhost:5173",
            "http://127.0.0.1:5173",
            "http://localhost:3000"
        ];
    }

    // Production with no CORS config would block all browser calls.
    throw new InvalidOperationException(
        "CORS is not configured. Set Cors__AllowedOrigins (comma-separated) or Cors__AllowedOrigins__0 " +
        "to your frontend URL, e.g. https://your-app.vercel.app");
}
