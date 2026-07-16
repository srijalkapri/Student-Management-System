using System.Reflection;
using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace CRUD.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<GradeSubject> GradeSubjects { get; set; }
        public DbSet<GradeSubjectTeacher> GradeSubjectTeachers { get; set; }
        public DbSet<PromotionHistory> PromotionHistories { get; set; }
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<ExamSession> ExamSessions { get; set; }
        public DbSet<ExamResultBatch> ExamResultBatches { get; set; }
        public DbSet<ExamResultItem> ExamResultItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AcessLog> AcessLogs => Set<AcessLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
