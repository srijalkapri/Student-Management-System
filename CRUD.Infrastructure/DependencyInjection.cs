using CRUD.Application.Interfaces;
using CRUD.Infrastructure.Persistence;
using CRUD.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQLConnection")
            ?? throw new InvalidOperationException("Connection string 'PostgreSQLConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies().UseNpgsql(connectionString, npgsql =>
            {
                npgsql.EnableRetryOnFailure(3);
            }));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IGradeSubjectRepository, GradeSubjectRepository>();
        services.AddScoped<IGradeSubjectTeacherRepository, GradeSubjectTeacherRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IAcessLogRepository, AccessLogRepository>();

        return services;
    }
}