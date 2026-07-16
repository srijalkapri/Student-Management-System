using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class ExamResultBatchConfiguration : IEntityTypeConfiguration<ExamResultBatch>
    {
        public void Configure(EntityTypeBuilder<ExamResultBatch> builder)
        {
            builder.ToTable("ExamResultBatches");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.ReviewComment)
                .HasMaxLength(500);

            builder.HasIndex(x => new { x.ExamSessionId, x.TeacherId })
                .IsUnique();

            builder.HasIndex(x => x.Status);

            builder.HasOne(x => x.ExamSession)
                .WithMany()
                .HasForeignKey(x => x.ExamSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Teacher)
                .WithMany()
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReviewedByUser)
                .WithMany()
                .HasForeignKey(x => x.ReviewedByUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
