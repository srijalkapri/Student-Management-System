using CRUD.Application.Interfaces;
using CRUD.Application.Services;
using CRUD.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherServices, TeacherService>();
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IGradeSubjectService, GradeSubjectService>();
        services.AddScoped<IGradeSubjectTeacherService, GradeSubjectTeacherService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IAcessLogWriter, AccessLogWriter>();
        services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();
        return services;
    }
}