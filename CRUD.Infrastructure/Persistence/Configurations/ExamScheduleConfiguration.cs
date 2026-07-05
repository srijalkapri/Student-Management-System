using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class ExamScheduleConfiguration : IEntityTypeConfiguration<ExamSchedule>
    {
        public void Configure(EntityTypeBuilder<ExamSchedule> builder)
        {
            builder.ToTable("ExamSchedules");
            builder.HasKey(es => es.Id);

            builder.Property(es => es.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(es => es.AcademicYear)
                .HasMaxLength(20);

            builder.Property(es => es.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(es => es.Grade)
                .WithMany()
                .HasForeignKey(es => es.GradeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
