using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Models;

namespace CRUD.Data.Configurations
{
    public class ExamSessionConfiguration : IEntityTypeConfiguration<ExamSession>
    {
        public void Configure(EntityTypeBuilder<ExamSession> builder)
        {
            builder.ToTable("ExamSessions");
            builder.HasKey(es => es.Id);

            builder.Property(es => es.Notes)
                .HasMaxLength(500);

            builder.HasOne(es => es.ExamSchedule)
                .WithMany(es => es.ExamSessions)
                .HasForeignKey(es => es.ExamScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.GradeSubject)
                .WithMany()
                .HasForeignKey(es => es.GradeSubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(es => es.InvigilatorTeacher)
                .WithMany()
                .HasForeignKey(es => es.InvigilatorTeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
