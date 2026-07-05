using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class PromotionHistoryConfiguration : IEntityTypeConfiguration<PromotionHistory>
    {
        public void Configure(EntityTypeBuilder<PromotionHistory> builder)
        {
            builder.ToTable("PromotionHistories");
            builder.HasKey(ph => ph.Id);

            builder.Property(ph => ph.PromotedAt)
                .IsRequired();

            builder.HasOne(ph => ph.Student)
                .WithMany()
                .HasForeignKey(ph => ph.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ph => ph.FromGrade)
                .WithMany()
                .HasForeignKey(ph => ph.FromGradeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ph => ph.ToGrade)
                .WithMany()
                .HasForeignKey(ph => ph.ToGradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}