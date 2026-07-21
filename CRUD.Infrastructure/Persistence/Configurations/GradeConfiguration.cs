using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grades");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.ClassName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.Level)
                .IsRequired();

            builder.HasIndex(g => g.Level)
                .IsUnique();

            builder.HasOne(g => g.ClassTeacher)
                .WithMany()
                .HasForeignKey(g => g.ClassTeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
