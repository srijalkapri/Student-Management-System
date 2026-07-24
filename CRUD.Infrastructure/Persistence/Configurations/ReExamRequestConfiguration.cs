using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class ReExamRequestConfiguration : IEntityTypeConfiguration<ReExamRequest>
    {
        public void Configure(EntityTypeBuilder<ReExamRequest> builder)
        {
            builder.ToTable("ReExamRequests");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(x => x.StudentReason)
                .HasMaxLength(500);

            builder.Property(x => x.AdminComment)
                .HasMaxLength(500);

            builder.Property(x => x.MarksRemarks)
                .HasMaxLength(300);

            builder.Property(x => x.MarksReviewComment)
                .HasMaxLength(500);

            builder.Property(x => x.MarksObtained)
                .HasPrecision(5, 2);

            builder.Property(x => x.TotalMarks)
                .HasPrecision(5, 2);

            builder.HasIndex(x => new { x.StudentId, x.ExamSessionId, x.AttemptNumber })
                .IsUnique();

            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ExamSession)
                .WithMany()
                .HasForeignKey(x => x.ExamSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OriginalResultItem)
                .WithMany()
                .HasForeignKey(x => x.OriginalResultItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReviewedByUser)
                .WithMany()
                .HasForeignKey(x => x.ReviewedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.MarksReviewedByUser)
                .WithMany()
                .HasForeignKey(x => x.MarksReviewedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
