using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.PhoneNo)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(s => s.Grade)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Global query filter
            builder.HasQueryFilter(s => !s.IsDeleted);

            // Unique partial indexes
            builder.HasIndex(s => s.Email)
                .HasDatabaseName("IX_Students_Email_Active")
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder.HasIndex(s => s.PhoneNo)
                .HasDatabaseName("IX_Students_PhoneNo_Active")
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
