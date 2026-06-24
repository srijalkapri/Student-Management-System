using CRUD.Data;
using CRUD.Interfaces;
using CRUD.Repositories;
using CRUD.Services;
using CRUD.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CRUD.Responses;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection")
    ?? throw new InvalidOperationException("Connection string 'PostgreSQLConnection' not found.");

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers()
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

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
        };
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies()
        .UseNpgsql(connectionString));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IGradeSubjectRepository, GradeSubjectRepository>();
builder.Services.AddScoped<IGradeSubjectTeacherRepository, GradeSubjectTeacherRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherServices, TeacherService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IGradeSubjectService, GradeSubjectService>();
builder.Services.AddScoped<IGradeSubjectTeacherService, GradeSubjectTeacherService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();