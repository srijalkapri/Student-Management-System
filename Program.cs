using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Interfaces;
using CRUD.Repositories;
using CRUD.Services;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection")
    ?? throw new InvalidOperationException("Connection string 'PostgreSQLConnection' not found.");


builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies()
      .UseNpgsql(connectionString)
    );


builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();


builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherServices, TeacherService>();




var app = builder.Build();





app.Run();