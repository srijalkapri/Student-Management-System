using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRUD.Domain.Models;

namespace CRUD.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.FullName)
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            // Global query filter
            builder.HasQueryFilter(u => !u.IsDeleted);

            // Unique partial index for username (only active users)
            builder.HasIndex(u => u.Username)
                .HasDatabaseName("IX_Users_Username_Active")
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
