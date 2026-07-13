using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.PhoneNo)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Global query filter
            builder.HasQueryFilter(t => !t.IsDeleted);

            // Unique partial indexes
            builder.HasIndex(t => t.Email)
                .HasDatabaseName("IX_Teachers_Email_Active")
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder.HasIndex(t => t.PhoneNo)
                .HasDatabaseName("IX_Teachers_PhoneNo_Active")
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder.HasIndex(t => t.UserId)
                .HasDatabaseName("IX_Teachers_UserId_Active")
                .IsUnique()
                .HasFilter("\"UserId\" IS NOT NULL AND \"IsDeleted\" = false");
        }
    }
}
