using CRUD.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class ExamResultItemConfiguration : IEntityTypeConfiguration<ExamResultItem>
    {
        public void Configure(EntityTypeBuilder<ExamResultItem> builder)
        {
            builder.ToTable("ExamResultItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalMarks)
                .HasPrecision(5, 2);

            builder.Property(x => x.MarksObtained)
                .HasPrecision(5, 2);

            builder.Property(x => x.Remarks)
                .HasMaxLength(300);

            builder.HasIndex(x => new { x.ExamResultBatchId, x.StudentId })
                .IsUnique();

            builder.HasOne(x => x.ExamResultBatch)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ExamResultBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
